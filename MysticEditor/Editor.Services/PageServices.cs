using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Editor.BE;
using Editor.BE.Model;
using Editor.BE.Model.Enumerators;
using Editor.BL;
using Editor.DTO;
using Editor.Helper;
using Iesi.Collections.Generic;
using NHibernate;
using System.Text.RegularExpressions;

namespace Editor.Services {
    public class PageServices {

        public PageDTO GetPage(int pageId) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Page page = new Page();
                        page = EditorServices.GetPageById(pageId, session);

                        Mapper.CreateMap<Page, PageDTO>();
                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<Page, PageDTO>(page);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }
        }

        public IList<PageDTO> GetPagesByContentId(int contentId) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        IList<Page> pages = new List<Page>();
                        pages = EditorServices.GetPageByContent(session, contentId) as List<Page>;

                        Mapper.CreateMap<Page, PageDTO>();
                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<IList<Page>, IList<PageDTO>>(pages);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public IList<PageElementDTO> GetPageelementByPageId(int pageid) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        IList<PageElement> pages = new List<PageElement>();
                        pages = EditorServices.GetPageelementByPage(session, pageid) as List<PageElement>;

                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<IList<PageElement>, IList<PageElementDTO>>(pages);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }
        }

        public PageElementDTO GetPageelementByPageelementID(int PageelementID) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        PageElement pel = new PageElement();
                        pel = HibernateHelper.SelectIstance<PageElement>(session, new string[] { "PageElementid" }, new object[] { PageelementID }, new Operators[] { Operators.Eq });


                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();

                        return Mapper.Map<PageElement, PageElementDTO>(pel);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }


        }

        public RawHtml GetRawHtmlById(int Rawhtmlid) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        RawHtml pel = new RawHtml();
                        pel = HibernateHelper.SelectIstance<RawHtml>(session, new string[] { "Rawhtmlid" }, new object[] { Rawhtmlid }, new Operators[] { Operators.Eq });

                        return pel;

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }


        }

        public void SaveRawHtml(RawHtml Row, string Folder) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        if (Folder.StartsWith(@"\")== true){
                           Folder= Folder.Remove(0, 1);
                        }                        
                        string SavePath = ConfigurationSettings.AppSettings["ServerPath"];
                        string FolderToSave = Path.Combine(SavePath, Folder);
                        HibernateHelper.Persist(Row, session);

                        if (Row.IsPersisted) {
                            string contrawfile = Path.Combine(FolderToSave, Row.Rawhtmlid + "_RawHtml.htm");

                            string htmlDocument = Row.Value;
                            FileStream fs = File.OpenWrite(contrawfile);
                            StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                            writer.Write(htmlDocument);
                            writer.Close();
                            WebSiteThumbnail.SaveImage(contrawfile, FolderToSave);
                            //cancello il file temporaneo html
                            File.Delete(contrawfile);
                        }
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

        }

        public PageDTO SavePage(PageDTO pagedto) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Page page = new Page();

                        Mapper.CreateMap<PageDTO, Page>();
                        Mapper.CreateMap<PageElementDTO, PageElement>();
                        Mapper.CreateMap<ElementDTO, Element>();
                        //Mappo la PageDTO in Page
                        page = Mapper.Map<PageDTO, Page>(pagedto);

                        Regex punt = new Regex(@"[\t\r\n\e\a._%+-/]");

                        page.Title = punt.Replace(page.Title.Replace("&nbsp;", "").Trim().Replace(" ", "_"), "_");

                        if (page.Publictitle != null && page.Contentid > 0 && page.Position > 0) {
                            if (page.IsNew) {

                                // Salvo la Nuova pagina 
                                HibernateHelper.Persist(page, session);

                                if (page.PageelementsList.Count == 0) {
                                    //Creo Le Page Elements in base alla struttura
                                    //Ricavo la lista degli elementi della struttura per generare i PageElement
                                    List<Element> structList = new List<Element>();
                                    structList = HibernateHelper.SelectCommand<Element>(session, " STRUCTUREID =" + page.Structureid);

                                    ISet<PageElement> PaElements = new HashedSet<PageElement>();
                                    foreach (Element el in structList) {

                                        PageElement PaElement = new PageElement();
                                        PaElement.Element = el;
                                        PaElement.Elementid = el.Elementid;
                                        PaElement.Page = page;
                                        PaElement.Pageid = page.Pageid;
                                        PaElement.Valore = el.Description;
                                        PaElement.IsNew = true;

                                        if (el.Elementtypeid == (int)ElementTypeEnum.RawHtml) {
                                            //solo RawHtml 
                                            RawHtml raw = new RawHtml();
                                            raw.Value = el.Description;
                                            raw.IsNew = true;
                                            HibernateHelper.Persist(raw, session);
                                            PaElement.Rawhtmlid = raw.Rawhtmlid;
                                        }
                                        HibernateHelper.Persist(PaElement, session);
                                        PaElements.Add(PaElement);
                                    }
                                    page.PageElements = PaElements;
                                    PaElements = page.PageElements;
                                }

                                // Setto il padre  al pageid se è zero
                                if (page.Parentpageid == 0) {
                                    page.Parentpageid = page.Pageid;
                                } else {
                                    page.Parentpageid = page.Parentpageid;
                                }
                                // Setto skinID a null se è zero
                                if (page.Skinid == 0) {
                                    page.Skinid = null;
                                }
                                page.Dirty = true;
                                HibernateHelper.Persist(page, session);
                            } else
                                if (page.Dirty) {
                                    //Update della Pagina Esistente
                                    HibernateHelper.Persist(page, session);

                                    //Foreach delle pageelements
                                    foreach (PageElement el in page.PageelementsList) {
                                        el.Pageid = page.Pageid;
                                        if (el.Deleted) {
                                            HibernateHelper.Persist(el, session);
                                            page.PageelementsList.Remove(el);
                                        } else {
                                            HibernateHelper.Persist(el, session);
                                        }
                                    }
                                }
                        }
                        //Rimappo l'oggetto da restituire
                        Mapper.CreateMap<Page, PageDTO>();
                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();
                        //Mappo la PageDTO in Page
                        pagedto = Mapper.Map<Page, PageDTO>(page);

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

            return pagedto;
        }

        public PageDTO MovePage(PageDTO pagedto) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    if (pagedto.Parentpageid > 0) {
                        //Prendo i Figli del nuovo padre della pagina DTO
                        List<Page> Figli = new List<Page>();
                        Figli = EditorServices.GetPageByParent(session, pagedto.Contentid, pagedto.Parentpageid);

                        bool update = false;

                        //Aggiorno la posizione dei figli successivi al DTO

                        //Controllo se esiste un figlio in posizione 1

                        //var uone = (from u in Figli
                        //            where u.Position == 1
                        //            select u).FirstOrDefault<Page>();

                        int oldpos = 0;

                        var con = (from f in Figli
                                   where f.Pageid == pagedto.Pageid
                                   select f).FirstOrDefault<Page>();

                        if (con != null) {
                            oldpos = con.Position;
                        }
                        foreach (Page pg in Figli) {

                            if (pg.Pageid == pagedto.Pageid) {
                                oldpos = pg.Position;
                                pg.Position = pagedto.Position;
                                pg.Parentpageid = pagedto.Parentpageid;
                                pg.Dirty = true;
                                HibernateHelper.Persist(pg, session);
                                update = true;
                            } else
                                if (pg.Position > pagedto.Position) {
                                    pg.Position = pg.Position + 1;
                                    pg.Dirty = true;
                                    HibernateHelper.Persist(pg, session);
                                } else
                                    if (pg.Position <= pagedto.Position
                                        //    && pg.Position != 1 
                                        && con != null) {
                                        if (pg.Position == pagedto.Position && oldpos > pagedto.Position) {
                                            pg.Position = pg.Position + 1;
                                        } else {
                                            pg.Position = pg.Position - 1;
                                        }
                                        pg.Dirty = true;
                                        HibernateHelper.Persist(pg, session);
                                    }


                        }

                        if (!update) {
                            Page page = new Page();
                            Mapper.CreateMap<PageDTO, Page>();
                            Mapper.CreateMap<PageElementDTO, PageElement>();
                            Mapper.CreateMap<ElementDTO, Element>();
                            //Mappo la PageDTO in Page
                            page = Mapper.Map<PageDTO, Page>(pagedto);

                            HibernateHelper.Persist(page, session);
                        }

                        transaction.Commit();

                        transaction = session.BeginTransaction();

                        Figli = EditorServices.GetPageByParent(session, pagedto.Contentid, pagedto.Parentpageid);

                        var child = from cd in Figli
                                    orderby cd.Position ascending
                                    select cd;

                        int position = 1;
                        foreach (Page ch in child) {

                            ch.Position = position;
                            ch.Dirty = true;
                            position++;
                            HibernateHelper.Persist(ch, session);
                        }

                        transaction.Commit();

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
            return pagedto;
        }

        public PageDTO ClonePage(PageDTO pagedto, int idItemAmm, string FolderToSave) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Page page = new Page();

                        Mapper.CreateMap<PageDTO, Page>();
                        Mapper.CreateMap<PageElementDTO, PageElement>();
                        //Mappo la PageDTO in Page
                        page = Mapper.Map<PageDTO, Page>(pagedto);

                        //Tengo in memoria il pageid da clonare
                        int pageId = page.Pageid;

                        //Salvo la nuova Pagina
                        HibernateHelper.Persist(page, session);

                        //Ricavo i PageElements del padre 
                        List<PageElement> CloneElements = HibernateHelper.SelectCommand<PageElement>(session, new string[] { "Pageid" }, new object[] { pageId }, new Operators[] { Operators.Eq });


                        //Ciclo  sui CloneElements
                        ISet<PageElement> ChildElements = new HashedSet<PageElement>();
                        foreach (PageElement el in CloneElements) {

                            PageElement child = new PageElement();
                            child.IsNew = true;
                            child.Page = page;
                            child.Pageid = page.Pageid;
                            child.Valore = el.Valore;
                            child.Filename = el.Filename;
                            child.Element = el.Element;
                            child.Elementid = el.Elementid;
                            if (el.Element.ElementType.Elementtypeid == (int)ElementTypeEnum.RawHtml) {

                                //Get RawHtml Clone
                                RawHtml cloneraw = new RawHtml();
                                cloneraw = HibernateHelper.SelectIstance<RawHtml>(session, new string[] { "Rawhtmlid" }, new object[] { el.Rawhtmlid }, new Operators[] { Operators.Eq });

                                RawHtml childraw = new RawHtml();
                                childraw.IsNew = true;
                                childraw.Value = cloneraw.Value;
                                SaveRawHtml(childraw, FolderToSave);
                                child.Rawhtmlid = childraw.Rawhtmlid;


                            }

                            HibernateHelper.Persist(child, session);
                            ChildElements.Add(child);
                        }

                        page.PageElements = ChildElements;
                        ChildElements = page.PageElements;

                        // Setto il padre  al pageid se è zero
                        if (page.Parentpageid == 0) {
                            page.Parentpageid = page.Pageid;
                        } else {
                            page.Parentpageid = page.Parentpageid;
                        }
                        // Setto skinID a null se è zero
                        if (page.Skinid == 0) {
                            page.Skinid = null;
                        }
                        page.Dirty = true;
                        HibernateHelper.Persist(page, session);


                        //Rimappo l'oggetto da restituire
                        Mapper.CreateMap<Page, PageDTO>();
                        Mapper.CreateMap<PageElement, PageElementDTO>();
                        Mapper.CreateMap<Element, ElementDTO>();
                        //Mappo la PageDTO in Page
                        pagedto = Mapper.Map<Page, PageDTO>(page);


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
            return pagedto;

        }

        public PageDTO ClonePage(PageDTO pagedto) {

            return ClonePage(pagedto, 0,"");
        }

        public Boolean DeletePage(PageDTO pagedto) {
            Boolean status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Page page = new Page();
                        Mapper.CreateMap<PageDTO, Page>();
                        Mapper.CreateMap<PageElementDTO, PageElement>();
                        //Mappo la PageDTO in Page
                        page = Mapper.Map<PageDTO, Page>(pagedto);


                        //Prendo i Figli del padre della pagina DTO
                        List<Page> Figli = new List<Page>();
                        Figli = EditorServices.GetPageByParent(session, page.Contentid, page.Parentpageid);

                        //Sposto il Pagedto Salvato accodandolo all'ultima posizione del cestino 
                        Page Cestino = new Page();
                        Cestino = EditorServices.GetBasket(session, page.Contentid);
                        int actualpos = 0;
                        if (Cestino != null) {
                            List<Page> Cestinati = new List<Page>();
                            Cestinati = EditorServices.GetPageByParent(session, page.Contentid, Cestino.Pageid);
                            if (Cestinati.Count > 0) {
                                actualpos = (from c in Cestinati
                                             select c.Position).Max();
                            }
                        }

                        //Decremento la posizione dei figli successivi al DTO
                        foreach (Page pg in Figli) {
                            if (pg.Position > pagedto.Position) {
                                pg.Position = pg.Position - 1;
                                pg.Dirty = true;
                                HibernateHelper.Persist(pg, session);
                            } else
                                if (pg.Pageid == page.Pageid) {
                                    page = pg;
                                    pg.Parentpageid = Cestino.Pageid;
                                    pg.Position = actualpos + 1;
                                    pg.Dirty = true;
                                    pg.Deleted = false;
                                    HibernateHelper.Persist(pg, session);
                                }
                        }

                        transaction.Commit();
                        status = true;
                    } catch (Exception ex) {
                        transaction.Rollback();
                        status = false;
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
            return status;
        }

        public Boolean PublishPage(int pageID, string pathIdItem) {
            Boolean status = false;
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        string fileserver = ConfigurationSettings.AppSettings["FileServer"];
                        Page pg = new Page();
                        pg = EditorServices.GetPageById(pageID, session);

                        //creo una cartella sul fileserver
                        string pathCont = Path.Combine(fileserver, pg.Content.Contentid.ToString() + "_" + pg.Content.Title.Trim().Replace(" ", "_"));
                        if (!Directory.Exists(pathCont)) {
                            Directory.CreateDirectory(pathCont);
                        }

                        EditorServices.PublicPage(pg, pathCont, pathIdItem, " ");

                    } catch (Exception ex) {
                        transaction.Rollback();
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                        status = true;
                    }
                }
            } return status;
        }

        public List<StructureDTO> GetStructures() {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        List<Structure> structures = new List<Structure>();
                        structures = HibernateHelper.SelectCommand<Structure>(session, "");

                        Mapper.CreateMap<Structure, StructureDTO>();

                        return Mapper.Map<List<Structure>, List<StructureDTO>>(structures);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }

        }

        public List<ElementTypeDTO> GetElementTypes() {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        List<ElementType> eltypes = new List<ElementType>();
                        eltypes = HibernateHelper.SelectCommand<ElementType>(session, "");

                        Mapper.CreateMap<ElementType, ElementTypeDTO>();

                        return Mapper.Map<List<ElementType>, List<ElementTypeDTO>>(eltypes);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

            }

        }

        //public List<ThemeDTO> GetThemes() {

        //    using (ISession session = HibernateHelper.GetSession().OpenSession()) {
        //        using (ITransaction transaction = session.BeginTransaction()) {
        //            try {
        //                List<Theme> themes = new List<Theme>();
        //                themes = HibernateHelper.SelectCommand<Theme>(session, "");

        //                Mapper.CreateMap<Theme, ThemeDTO>();
        //                return Mapper.Map<List<Theme>, List<ThemeDTO>>(themes);

        //            } catch (Exception ex) {
        //                throw ex;
        //            } finally {
        //                session.Flush();
        //                session.Close();
        //            }
        //        }

        //    }

        //}
    }
}
