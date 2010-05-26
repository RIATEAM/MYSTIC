using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AutoMapper;
using Editor.BE;
using Editor.BE.Model;
using Editor.BE.Model.Enumerators;
using Editor.BL;
using Editor.DTO;
using Iesi.Collections.Generic;
using NHibernate;

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

        public List<PageDTO> GetPagesByContentId(int contentId) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        List<Page> pages = new List<Page>();
                        pages = EditorServices.GetPageByContent(session, contentId) as List<Page>;

                        Mapper.CreateMap<Page, PageDTO>();

                        return Mapper.Map<List<Page>, List<PageDTO>>(pages);

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

        public PageDTO SavePage(PageDTO pagedto) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Page page = new Page();

                        Mapper.CreateMap<PageDTO, Page>();
                        Mapper.CreateMap<PageElementDTO, PageElement>();
                        //Mappo la PageDTO in Page
                        page = Mapper.Map<PageDTO, Page>(pagedto);

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
                                    PaElement.Value = el.Description;
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
                        //Rimappo l'oggetto da restituire
                        Mapper.CreateMap<Page, PageDTO>();
                        Mapper.CreateMap<PageElement, PageElementDTO>();
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
                using (ITransaction transaction = session.BeginTransaction()) {
                    Page page = new Page();
                    Mapper.CreateMap<PageDTO, Page>();
                    Mapper.CreateMap<PageElementDTO, PageElement>();
                    //Mappo la PageDTO in Page
                    page = Mapper.Map<PageDTO, Page>(pagedto);


                    //Prendo i Figli del nuovo padre della pagina DTO
                    List<Page> Figli = new List<Page>();
                    Figli = EditorServices.GetPageByParent(session, 1, page.Parentpageid);

                    //Incremento la posizione dei figli successivi al DTO
                    foreach (Page pg in Figli) {
                        if (pg.Position >= pagedto.Position) {
                            pg.Position = pg.Position + 1;
                            pg.Dirty = true;
                            HibernateHelper.Persist(pg, session);
                        }
                    }

                    //Aggiorno il Pagedto Salvato
                    HibernateHelper.Persist(page, session);

                    transaction.Commit();

                    try {
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
                            child.Value = el.Value;
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
                                HibernateHelper.Persist(childraw, session);
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
                        Figli = EditorServices.GetPageByParent(session, 1, page.Parentpageid);

                        //Sposto il Pagedto Salvato accodandolo all'ultima posizione del cestino 
                        Page Cestino = new Page();
                        Cestino = EditorServices.GetBasket(session, 1);
                        int actualpos = 0;
                        if (Cestino != null) {
                            List<Page> Cestinati = new List<Page>();
                            Cestinati = EditorServices.GetPageByParent(session, 1, Cestino.Pageid);
                            actualpos = (from c in Cestinati
                                         select c.Position).Max();
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

                    } catch (Exception ex) {
                        transaction.Rollback();
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                        status = true;
                    }
                }
            }
            return status;
        }

        public Boolean PublishPage(int pageID) {
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

                        EditorServices.PublicPage(pg, pathCont, session);

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

        public List<ThemeDTO> GetThemes() {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        List<Theme> themes = new List<Theme>();
                        themes = HibernateHelper.SelectCommand<Theme>(session, "");

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





        public PageDTO Savenew() {

            PageDTO page = new PageDTO();

            page = GetPage(6);


            DeletePage(page);


            //page.Title = " nuova pagina".Trim().Replace(" ", "_");
            //page.Publictitle = "Nuova Pagina";

            ////page.Skinid = 2;
            //page.Structureid = 1;
            //page.Contentid = 1;
            //page.IsNew = true;
            //page.Level = 1;
            //page.Position = 1;
            //page.State = 1;
            //page.PageelementsList = new List<PageelementDTO>();

            //page = SavePage(page);


            //page.Publictitle = "Nuova Pagina Modificata";
            //page.PageelementsList[0].Value = page.PageelementsList[0].Value + " Modificato";
            //page.PageelementsList[0].Dirty = true;
            //page.PageelementsList[1].Deleted = true;
            //page.Dirty = true;
            //page = SavePage(page);







            return page;
        }

    }

}
