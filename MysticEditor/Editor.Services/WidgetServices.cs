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
using Editor.BE.Model.Enumerators;
using System.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;


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

                            //Foreach delle WidgetElements
                            foreach (WidgetElement el in widget.WidgetElementsList) {
                                el.Widgetid = widget.Widgetid;
                                if (el.Type == 0) {
                                    el.Type = 1;
                                }
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
            //Setto lo stato del content a non allineato 
            ContentServices contSvc = new ContentServices();
            contSvc.SetStateContent(Widgetdto.Contentid, (int)ContentStateEnum.NonAllineato);
            
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

                        if (widget.Type == 0) {
                            widget.Type = 1;
                        }
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
                //Setto lo stato del content a non allineato 
                WidgetDTO widg = new WidgetDTO();                
                widg = GetWidget(wid.Widgetid);
                ContentServices contSvc = new ContentServices();
                contSvc.SetStateContent(widg.Contentid, (int)ContentStateEnum.NonAllineato);

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


        public List<WidgetElementDTO> SynchronizeWidgetElement(int contentId, int iditemamm, string type) {

            bool result = false;

            result = _SynchronizeWidgetElement(contentId, iditemamm, type);
            List<WidgetElementDTO> ListToReturn = new List<WidgetElementDTO>();

            if (result) {
                using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                    using (ITransaction transaction = session.BeginTransaction()) {
                        try {
                            //Ricava il/i Widget del content 
                            IList<Widget> widges = new List<Widget>();
                            widges = EditorServices.GetWidgetByContent(session, contentId) as List<Widget>;

                            foreach (Widget wid in widges) {
                                if (wid.Structureid == 4) {
                                    //è Utility e link
                                    if (wid.WidgetElementsList != null && wid.WidgetElementsList.Count > 0) {
                                        Mapper.CreateMap<WidgetElement, WidgetElementDTO>();
                                        Mapper.CreateMap<Element, ElementDTO>();

                                        ListToReturn.AddRange(Mapper.Map<List<WidgetElement>, List<WidgetElementDTO>>(wid.WidgetElementsList));
                                    }
                                }
                            }
                            //Riordino per widgetid e position

                            var OrderedList = (from c in ListToReturn
                                               orderby c.Widgetid ascending, c.Position ascending
                                               select c).ToList<WidgetElementDTO>();

                            return OrderedList;

                        } catch (Exception ex) {
                            transaction.Rollback();
                            throw ex;
                        } finally {
                            session.Flush();
                            session.Close();
                        }
                    }
                }

            }

            //Setto lo stato del content a non allineato 
            ContentServices contSvc = new ContentServices();
            contSvc.SetStateContent(contentId, (int)ContentStateEnum.NonAllineato);


            return ListToReturn;

        }
        

        private bool _SynchronizeWidgetElement(int contentId, int iditemamm, string type) {
            bool status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        //Ricava il/i Widget del content 
                        IList<Widget> widges = new List<Widget>();
                        widges = EditorServices.GetWidgetByContent(session, contentId) as List<Widget>;
                        var pos = 0;
                        foreach (Widget wid in widges) {
                            if (wid.Structureid == 4) {
                                //è Utility e link
                                if (wid.WidgetElements != null && wid.WidgetElements.Count > 0) {

                                    pos = (from f in wid.WidgetElements
                                           where f.Type != (int)WidgetElementTypeEnum.Importato
                                           select f.Position).Max();

                                    //Ricava i WidgetElement del Widget
                                    //Cancella gli eventuali WidgetElement di tipo 2 (importati)
                                    foreach (WidgetElement widel in wid.WidgetElementsList) {

                                        if (widel.Type == (int)WidgetElementTypeEnum.Importato) {
                                            widel.Deleted = true;
                                            wid.WidgetElements.Remove(widel);
                                            HibernateHelper.Persist(widel, session);
                                        }
                                    }
                                }
                            }

                            //Ricava per iditemamm e tipo i correlati 
                            IUnityContainer container = new UnityContainer();
                            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                            section.Containers.Default.Configure(container);
                            WidgetHelper unityHelper = container.Resolve<WidgetHelper>();

                            DataSet correlati = new DataSet();
                            correlati = unityHelper.GetCorrelated(iditemamm, type);
                            if (correlati.Tables != null && correlati.Tables[0].Rows.Count > 0) {
                                //Ho trovato dei correlati

                                foreach (DataRow Riga in correlati.Tables[0].Rows) {
                                    pos++;
                                    WidgetElement newidel = new WidgetElement();
                                    newidel.IsNew = true;
                                    newidel.Type = (int)WidgetElementTypeEnum.Importato;
                                    newidel.Elementid = 6;
                                    newidel.Position = pos;
                                    newidel.Widgetid = wid.Widgetid;
                                    newidel.Widget = wid;

                                    if (type == "std") {
                                        if (Riga["ITEM_NAME"] != DBNull.Value) {
                                            newidel.Name = Riga["ITEM_NAME"].ToString();
                                        }
                                        if (Riga["ITEM_LINK"] != DBNull.Value) {
                                            newidel.Valore = "\\cms\\" + Riga["ITEM_LINK"].ToString();
                                        }

                                    } else if (type == "com") {
                                        if (Riga["TITLE"] != DBNull.Value) {
                                            newidel.Name = Riga["TITLE"].ToString();
                                        }
                                        if (Riga["ID_MARKET"] != DBNull.Value && Riga["ID_CORRELATED"] != DBNull.Value) {
                                            newidel.Valore = "\\cms\\comm\\corr_doc_frameset.aspx?idcorr=" + Riga["ID_CORRELATED"].ToString() + "&idmarket=" + Riga["ID_MARKET"].ToString()
                                                + "&visInfo=no";
                                        }

                                    }
                                    // Inserisce i nuovi WidgetElement

                                    HibernateHelper.Persist(newidel, session);

                                    wid.WidgetElements.Add(newidel);
                                }
                            }
                            wid.Dirty = true;
                            HibernateHelper.Persist(wid, session);

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


    }

    public class WidgetHelper {

        private ICMSServices _CMSServices;
        [Dependency]
        public ICMSServices CMSServices {
            get { return _CMSServices; }
            set { _CMSServices = value; }
        }
        public string GetItemPath(string iditemamm, string type) {
            return CMSServices.GetItemPath(iditemamm, type);
        }
        public string GetItemTitle(int iditemamm, string type) {
            return CMSServices.GetItemTitle(iditemamm, type);
        }
        public string GetItemIdUser(int iditemamm) {
            return CMSServices.GetItemIdUser(iditemamm);
        }
        public DataSet GetCorrelated(int iditemamm, string type) {
            return CMSServices.GetCorrelated(iditemamm, type);
        }
    }

}