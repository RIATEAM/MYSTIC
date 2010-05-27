using System.Collections.Generic;
using Editor.BL;
using Editor.BE.Model;
using Editor.DTO;
using AutoMapper;
using NHibernate;
using System;
using Editor.BE;


namespace Editor.Services {
    public class ContentServices {

        public IList<ContentDTO> GetContents() {
            IList<Content> List = new List<Content>();
            List = EditorServices.GetContents<Content>() as List<Content>;
            Mapper.CreateMap<Content, ContentDTO>();
            return Mapper.Map<IList<Content>,IList<ContentDTO>>(List);
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
    }
}
