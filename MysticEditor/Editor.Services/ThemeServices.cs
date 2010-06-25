using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.DTO;
using NHibernate;
using Editor.BE;
using Editor.BE.Model;
using AutoMapper;

namespace Editor.Services {
    public class ThemeServices {

        public List<ThemeDTO> GetThemes() {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        List<Theme> themes = HibernateHelper.SelectCommand<Theme>(session, "");

                        Mapper.CreateMap<Theme, ThemeDTO>();
                        return Mapper.Map<List<Theme>, List<ThemeDTO>>(themes);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public bool SetTheme(int themeID, int contentID) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        string strSQL = "CONTENTID = " + contentID;
                        List<Content> contents = HibernateHelper.SelectCommand<Content>(session, strSQL);
                        Content content = null;
                        if (contents.Count > 0) {
                            content = contents[0];
                        }else{
                            throw new Exception("Impossibile trovare il content");
                        }

                        strSQL = "CONTENTID = " + contentID;
                        List<Page> pages = HibernateHelper.SelectCommand<Page>(session, strSQL);

                        strSQL = "CONTENTID = " + contentID;
                        List<Widget> widgets = HibernateHelper.SelectCommand<Widget>(session, strSQL);

                        strSQL = "THEMEID = " + themeID;
                        List<Skin> skins = HibernateHelper.SelectCommand<Skin>(session, strSQL);

                        // ---------------------
                        content.Themeid = themeID;

                        foreach (Skin s in skins) {
                            if (content.Skin.Codice == s.Codice) {
                                content.Skinid = s.Skinid;
                                HibernateHelper.UpdateCommand(content);
                                break;
                            }
                        }

                        for (int i = 0; i < pages.Count; i++) {
                            Page page = pages[i];
                            foreach (Skin s in skins) {
                                if (page.Skin.Codice == s.Codice) {
                                    page.Skinid = s.Skinid;
                                    HibernateHelper.UpdateCommand(page);
                                    break;
                                }
                            }
                        }

                        for (int i = 0; i < widgets.Count; i++) {
                            Widget widget = widgets[i];
                            foreach (Skin s in skins) {
                                if (widget.Skin.Codice == s.Codice) {
                                    widget.Skinid = s.Skinid;
                                    HibernateHelper.UpdateCommand(widget);
                                    break;
                                }
                            }
                        }

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
