using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EAI.BE.Model;
using NHibernate;
using EAI.BE;

namespace EAI.BL {
    public class TaskLogic {
        public Task SaveTask(Task obj) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        if (obj.Type != 0) {
                            HibernateHelper.Persist(obj, session);
                            transaction.Commit();
                        }
                    } catch (Exception) {
                        transaction.Rollback();
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
            return obj;

        }

        public Task SaveTask(ISession session, Task obj) {
            if (obj.Type != 0) {
                HibernateHelper.Persist(obj, session);
            }
            return obj;
        }

        public List<Task> GetTasks() {
            return GetTasks("");
        }
        
        public List<Task> GetTasks(string where) {
            List<Task> tsks = null;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                try {
                    tsks = HibernateHelper.SelectCommand<Task>(session, where);
                } catch (Exception) {
                } finally {
                    session.Flush();
                    session.Close();
                }
            }
            return tsks;
        }

        public Task UpdateTask(Task obj) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        obj.Dirty = true;
                        obj.DateUpd = DateTime.Now;
                        if (obj.Status > 0) {
                            obj = SaveTask(session, obj);
                            transaction.Commit();
                        }
                    } catch (Exception) {
                        transaction.Rollback();
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
            return obj;
        }
    }
}
