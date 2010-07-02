using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.DTO;
using NHibernate;
using Editor.BE;
using Editor.BE.Model;
using AutoMapper;
using Editor.BL;
using System.Text.RegularExpressions;

namespace Editor.Services {
    public class WidgetServices {

        public IList<WidgetDTO> GetWidgetsByContentId(int contentId) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        IList<Widget> widges = new List<Widget>();
                        widges = EditorServices.GetWidgetByContent(session, contentId) as List<Widget>;

                        Mapper.CreateMap<Widget, WidgetDTO>();
                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<IList<Widget>, IList<WidgetDTO>>(widges);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public WidgetDTO GetWidget(int WidgetId) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Widget widget = new Widget();
                        widget = EditorServices.GetWidgetById(WidgetId, session);

                        Mapper.CreateMap<Widget, WidgetDTO>();
                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<Widget, WidgetDTO>(widget);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }


        }

        public WidgetDTO SaveWidget(WidgetDTO Widgetdto) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {

                        Widget widget = new Widget();

                        Mapper.CreateMap<WidgetDTO, Widget>();
                        Mapper.CreateMap<WidgetElementDTO, WidgetElement>();
                        Mapper.CreateMap<ElementDTO, Element>();

                        widget = Mapper.Map<WidgetDTO, Widget>(Widgetdto);

                        Regex punt = new Regex(@"[\t\r\n\e\a._%+-/]");
                        widget.Title = punt.Replace(widget.Title.Replace("&nbsp;", "").Trim().Replace(" ", "_"), "_");

                        if (widget.Publictitle != null && widget.Contentid > 0 && widget.Position > 0) {
                            
                            widget.Publictitle = EditorServices.ReplaceCharacters(widget.Publictitle);
                            
                            HibernateHelper.Persist(widget, session);

                            //Foreach delle pageelements
                            foreach (WidgetElement el in widget.WidgetElementsList) {
                                el.Widgetid = widget.Widgetid;
                                if (el.Deleted) {
                                    HibernateHelper.Persist(el, session);
                                    widget.WidgetElementsList.Remove(el);
                                } else {
                                    HibernateHelper.Persist(el, session);
                                }
                            }
                        }

                        //Rimappo l'oggetto da restituire
                        Mapper.CreateMap<Widget, WidgetDTO>();
                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        //Mappo la PageDTO in Page
                        Widgetdto = Mapper.Map<Widget, WidgetDTO>(widget);

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

            return Widgetdto;
        }

        public WidgetElementDTO GetWidgetElement(int WidgetElementId) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        WidgetElement widget = new WidgetElement();
                        widget = EditorServices.GetWidgetElementById(WidgetElementId, session);

                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<WidgetElement, WidgetElementDTO>(widget);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }


        }

        public WidgetElementDTO SaveWidgetElement(WidgetElementDTO wid) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        WidgetElement widget = new WidgetElement();

                        Mapper.CreateMap<WidgetElementDTO, WidgetElement>();
                        Mapper.CreateMap<ElementDTO, Element>();

                        widget = Mapper.Map<WidgetElementDTO, WidgetElement>(wid);

                        if (widget.Name != null && widget.Widgetid > 0 && widget.Position > 0) {
                            widget.Name = EditorServices.ReplaceCharacters(widget.Name);
                            HibernateHelper.Persist(widget, session);
                        }

                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        //Mappo la PageDTO in Page
                        wid = Mapper.Map<WidgetElement, WidgetElementDTO>(widget);

                        transaction.Commit();


                    } catch (Exception ex) {
                        transaction.Rollback();
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
                return wid;
            }

        }

        public WidgetElementDTO MoveWidgetElement(WidgetElementDTO wid) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    if (wid.Position > 0) {
                        Widget Padre = EditorServices.GetWidgetById(wid.Widgetid, session);


                        int oldpos = 0;

                        var con = (from f in Padre.WidgetElements
                                   where f.Widgetelementid == wid.Widgetelementid
                                   select f).FirstOrDefault<WidgetElement>();

                        if (con != null) {
                            oldpos = con.Position;
                        }

                        foreach (WidgetElement pg in Padre.WidgetElements) {

                            if (pg.Widgetelementid == wid.Widgetelementid) {
                                oldpos = pg.Position;
                                pg.Position = wid.Position;
                                pg.Dirty = true;
                                HibernateHelper.Persist(pg, session);
                            } else
                                if (pg.Position > wid.Position) {
                                    pg.Position = pg.Position + 1;
                                    pg.Dirty = true;
                                    HibernateHelper.Persist(pg, session);
                                } else
                                    if (pg.Position <= wid.Position
                                        //    && pg.Position != 1 
                                        && con != null) {
                                        if (pg.Position == wid.Position && oldpos > wid.Position) {
                                            pg.Position = pg.Position + 1;
                                        } else {
                                            pg.Position = pg.Position - 1;
                                        }
                                        pg.Dirty = true;
                                        HibernateHelper.Persist(pg, session);
                                    }

                        }
                    }
                } catch (Exception ex) {
                    transaction.Rollback();
                    throw ex;
                } finally {
                    session.Flush();
                    session.Close();
                    transaction.Dispose();
                }

            }
            return wid;
        }

        public bool MoveWidgetContent(int ItemId, int NewContentID, string repositoty) {
            bool status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {

                        List<Content> cont = new List<Content>();
                        cont = HibernateHelper.SelectCommand<Content>(session, "CONTENTID <>" + NewContentID + " AND IDITEM =" + ItemId + " ORDER BY TO_DATE( DATE_CREATION ,'DD-MM-YYYY-HH24-MI-SS') DESC ");

                        if (cont != null && cont.Count > 0) {

                            IList<Widget> widgesnew = new List<Widget>();
                            widgesnew = EditorServices.GetWidgetByContent(session, NewContentID) as List<Widget>;

                            foreach (Widget wid in widgesnew) {
                                wid.Deleted = true;
                                HibernateHelper.Persist(wid, session);
                            }



                            IList<Widget> widges = new List<Widget>();
                            widges = EditorServices.GetWidgetByContent(session, cont[0].Contentid) as List<Widget>;

                            foreach (Widget wid in widges) {
                                wid.Contentid = NewContentID;
                                wid.Dirty = true;

                                HibernateHelper.Persist(wid, session);

                            }




                            transaction.Commit();

                            status = true;
                        }
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
