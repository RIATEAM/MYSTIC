using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.DTO;
using NHibernate;
using Editor.BE;
using Editor.BE.Model;
using AutoMapper;
using Editor.BE.Model.Enumerators;

namespace Editor.Services {
    public class PubblicazioneTempoServices {

        public bool SetPubblicazioneTempo(int contentID, string date, int active) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        string strSQL = "CONTENTID = " + contentID;
                        List<Content> contents = HibernateHelper.SelectCommand<Content>(session, strSQL);
                        Content content = null;
                        if (contents.Count > 0) {
                            content = contents[0];
                        } else {
                            throw new Exception("Impossibile trovare il content");
                        }

                        content.Date_publish = date;
                        content.Publish_active = active;
                        content.State = (int)ContentStateEnum.DaPubblicare;
                        HibernateHelper.UpdateCommand(session, content);
                        transaction.Commit();
                        return true;

                    } catch (Exception ex) {
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
