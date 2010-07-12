using System.Collections.Generic;
using Editor.BL;
using Editor.BE.Model;
using Editor.DTO;
using AutoMapper;
using NHibernate;
using System;
using Editor.BE;
using Editor.BE.Model.Enumerators;


namespace Editor.Services {
    public class ContentServices {

        public IList<ContentDTO> GetContents() {
            IList<Content> List = new List<Content>();
            List = EditorServices.GetContents<Content>() as List<Content>;
            Mapper.CreateMap<Content, ContentDTO>();
            return Mapper.Map<IList<Content>, IList<ContentDTO>>(List);
        }

        public ContentDTO GetContentByID(int id) {
            string strSQL = "CONTENTID = " + id;
            List<Content> contents = HibernateHelper.SelectCommand<Content>(strSQL);
            Content content = null;
            if (contents.Count > 0) {
                content = contents[0];
            } else {
                throw new Exception("Impossibile trovare il content");
            }
            Mapper.CreateMap<Content, ContentDTO>();
            return Mapper.Map<Content, ContentDTO>(content);
        }

        public ContentDTO SaveContent(ContentDTO contentDto) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content content = new Content();
                        Mapper.CreateMap<ContentDTO, Content>();
                        //Mappo la  Content in ContentDTO
                        content = Mapper.Map<ContentDTO, Content>(contentDto);

                        HibernateHelper.Persist(content, session);

                        //Rimappo l'oggetto da restituire
                        Mapper.CreateMap<Content, ContentDTO>();

                        //Mappo la ContentDTO in Content
                        contentDto = Mapper.Map<Content, ContentDTO>(content);

                        transaction.Commit();

                    } catch (Exception ex) {
                        transaction.Rollback();
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
            return contentDto;

        }

        public bool SetStateContent(int contentId, int state) { 
         bool status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content cont = new Content();
                        cont = EditorServices.GetContentById(contentId, session);

                        if (state == (int)ContentStateEnum.Allineato || state == (int)ContentStateEnum.NonAllineato ) {
                            cont.Dirty = true;
                            cont.State = state;
                            HibernateHelper.Persist(cont, session);
                        }
                        transaction.Commit();
                        
                        status = true;
                        return status;
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return status;
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public bool SetContentItemid(int contentId, int itemId, string repository) {

            bool status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content cont = new Content();
                        cont = EditorServices.GetContentById(contentId, session);

                        cont.Iditem = itemId;
                        cont.Repository = repository;
                        cont.Date_creation = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
                        cont.Dirty = true;

                        HibernateHelper.Persist(cont, session);

                        transaction.Commit();

                        status = true;
                        return status;
                    } catch (Exception ex) {
                        transaction.Rollback();
                        return status;
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }

        }
    }
}
