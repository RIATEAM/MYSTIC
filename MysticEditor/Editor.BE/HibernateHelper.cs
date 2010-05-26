using System;
using System.Collections.Generic;
using Editor.BE.Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections;
using NHibernate.Tool.hbm2ddl;

namespace Editor.BE {
    /// <summary>
    /// Class of Services NHibernate to save and get data from DB 
    /// </summary>
    public class HibernateHelper {

        /// <summary>
        /// Property set assembly reference
        /// </summary>
        private static string AssemblyName = "Editor.BE";

        private static Configuration cfg;
        private static ISessionFactory factory;

        public static ISessionFactory GetSession() {
            try {
                if (cfg == null) {
                    cfg = new Configuration();
                    cfg.AddAssembly(AssemblyName);
                }

                if (factory == null) {
                    factory = cfg.BuildSessionFactory();
                }
                return factory;
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Method to retrive a list of object from DB, filtering for specific fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="ids"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static List<T> SelectCommand<T>(string[] fields, object[] ids, Operators[] operators) {

            using (ISession session = GetSession().OpenSession()) {

                List<T> List = new List<T>();
                List = SelectCommand<T>(session, fields, ids, operators);
                return List;

            }
        }

        public static List<T> SelectCommand<T>(ISession session, string[] fields, object[] ids, Operators[] operators) {

            using (ITransaction transaction = session.BeginTransaction()) {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                try {
                    for (int i = 0; i < fields.Length; i++) {
                        ICriterion se = null;
                        if (operators != null) {
                            switch (operators[i]) {
                                case Operators.Eq: se = Expression.Eq(fields[i], ids[i]); break;
                                case Operators.Like: se = Expression.Like(fields[i], ids[i]); break;
                                //     case Operators.In: se = Expression.In(fields[i], ids[i]); break;
                                case Operators.Ge: se = Expression.Ge(fields[i], ids[i]); break;
                                case Operators.Gt: se = Expression.Gt(fields[i], ids[i]); break;
                                case Operators.InsensitiveLike: se = Expression.InsensitiveLike(fields[i], ids[i]); break;
                                case Operators.IsNull: se = Expression.IsNull(fields[i]); break;
                                case Operators.IsNotNull: se = Expression.IsNotNull(fields[i]); break;
                                case Operators.IsEmpty: se = Expression.IsEmpty(fields[i]); break;
                                case Operators.Le: se = Expression.Le(fields[i], ids[i]); break;
                                case Operators.Lt: se = Expression.Lt(fields[i], ids[i]); break;
                                case Operators.Distinct:
                                    se = Expression.Gt(fields[i], ids[i]);
                                    criteria.Add(se);
                                    se = Expression.Lt(fields[i], ids[i]);
                                    break;
                                default: se = Expression.Eq(fields[i], ids[i]); break;
                            }

                            if (se != null) {
                                criteria.Add(se);
                            }
                        } else {
                            se = Expression.Eq(fields[i], ids[i]);
                            criteria.Add(se);
                        }
                    }
                } catch {
                    return new List<T>();
                }
                return (List<T>)criteria.List<T>();
            }
        }

        /// <summary>
        /// Method to retrive a list of object from DB, filtering for specific SQL Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SQLwhere"></param>
        /// <returns></returns>
        public static List<T> SelectCommand<T>(string SQLwhere) {
            using (ISession session = GetSession().OpenSession()) {
                List<T> List = new List<T>();
                List = SelectCommand<T>(session, SQLwhere);
                return List;
            }
        }

        public static List<T> SelectCommand<T>(ISession session, string SQLwhere) {
            List<T> List = new List<T>();

                ICriteria criteria = session.CreateCriteria(typeof(T));
                IResultTransformer resultTransformer = new DistinctRootEntityResultTransformer();

                try {
                    ICriterion se = null;
                    if (SQLwhere.Length > 0) {
                        se = Expression.Sql(SQLwhere);
                    } else {
                        se = Expression.Sql(" 1 = 1");
                    }

                    if (se != null) {
                        criteria.Add(se);
                        criteria.SetResultTransformer(resultTransformer);
                    }

                } catch (Exception ex) {
                    throw ex;
                }
                List = (List<T>)criteria.List<T>();

            return List;
        }


        /// <summary>
        /// Method to retrive an object from DB, filtering for specific fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="ids"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static T SelectIstance<T>(string[] fields, object[] ids, Operators[] operators) {

            List<T> Ilist = SelectCommand<T>(fields, ids, operators);

            if (Ilist.Count > 0) {
                return Ilist[0];
            } else {
                return default(T);
            }
        }

        public static T SelectIstance<T>(ISession session, string[] fields, object[] ids, Operators[] operators) {

            List<T> Ilist = SelectCommand<T>(session, fields, ids, operators);

            if (Ilist.Count > 0) {
                return Ilist[0];
            } else {
                return default(T);
            }
        }

        /// <summary>
        /// Method to save an object on DB 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object InsertCommand(object obj) {
            using (ISession session = GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        session.Save(obj);
                        transaction.Commit();
                    } catch (HibernateException ex) {
                        throw ex;
                    }
                    return obj;
                }
            }
        }
        /// <summary>
        /// Method to update an object on DB 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object UpdateCommand(object obj) {
            using (ISession session = GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    session.Update(obj);
                    transaction.Commit();
                } catch (HibernateException ex) {
                    transaction.Rollback();
                    throw ex;
                }
                return obj;
            }
        }


        /// <summary>
        /// Esegue la logica di persistenza dell elemento sul DB
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public static IPersistente Persist(IPersistente el) {
            using (ISession session = GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    IPersistente result = Persist(el, session);
                    transaction.Commit();
                    return result;
                } catch (HibernateException ex) {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public static IPersistente Persist(IPersistente el, ISession session) {
            try {
                if (el.Deleted && el.IsNew) {
                    return null;
                }
                if (el.Deleted) {
                    session.Delete(el);
                    session.Flush();
                    return null;
                } else if (el.IsNew) {
                    el.IsPersisted = false;
                    session.Save(el);
                    el.IsNew = false;
                } else if (el.Dirty) {
                    el.IsPersisted = true;
                    session.Update(el);
                    el.Dirty = false;
                }
                return el;
            } catch (HibernateException ex) {
                throw ex;
            }
        }


    }
    /// <summary>
    /// Eq = Uguale
    /// In =
    /// Ge = Maggiore uguale
    /// Gt = Maggiore di
    /// Le = Minore uguale
    /// Lt = minore di
    /// </summary>
    public enum Operators {
        /// <summary>
        /// Uguale
        /// </summary>
        Eq = 0,
        Like,
        In,
        /// <summary>
        /// Maggiore uguale
        /// </summary>
        Ge,
        /// <summary>
        /// Maggiore di
        /// </summary>
        Gt,
        InsensitiveLike,
        IsNull,
        IsNotNull,
        IsNotEmpty,
        IsEmpty,
        /// <summary>
        /// Minore uguale
        /// </summary>
        Le,
        /// <summary>
        /// Minore di
        /// </summary>
        Lt,
        /// <summary>
        /// Diverso
        /// </summary>
        Distinct
    }
}
