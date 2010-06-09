using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Editor.BE;
using Editor.BE.Model;
using Editor.BE.Model.Enumerators;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate;

namespace Editor.BL {
    public partial class EditorServices {

        /// <summary>
        /// Analizza un file Htm spittando per i tag H
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extractLocation"></param>
        /// <returns></returns>
        public static List<string> Export(string file, string extractLocation, string Title) {

            List<string> Files = new List<string>();
            Regex REGEXLIVELLO = new Regex("[<][h][0-9]");
            StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);


            Regex Titolo = new Regex("<title>");
            Regex EndTitolo = new Regex("</title>");

            try {
                string line = string.Empty;
                string testo = string.Empty;
                string outputText = string.Empty;
                int count = 1;
                int thislivello = 0;
                int lastlivello = 0;
                string lastbody = string.Empty;
                bool inserire = false;
                bool first = true;
                bool check = true;
                while (((line = sr.ReadLine()) != null)) {

                    if ((Titolo.Match(line).Length > 0 && line.StartsWith("<")) ||
                        (Titolo.Match(testo).Length > 0 && testo.StartsWith("<"))) {

                        testo += " " + line + System.Environment.NewLine;
                        while (EndTitolo.Match(line).Length == 0 && (line = sr.ReadLine()) != null) {
                            testo += " " + line + System.Environment.NewLine;
                        }
                        testo = "<title>" + Title + "</title>";
                        //POS                     LIVELLO                
                        outputText = count.ToString() + "|" + 0 + "|" + testo;
                        Files.Add(outputText);
                        count++;
                    }


                    if ((REGEXLIVELLO.Match(line).Length > 0 && line.StartsWith("<")) ||
                        (REGEXLIVELLO.Match(testo).Length > 0 && testo.StartsWith("<"))) {
                        testo += " " + line + System.Environment.NewLine;

                        if (lastlivello == 0) {//////
                            lastlivello = thislivello = System.Convert.ToInt32(testo.Trim().Substring(2, 1));
                            inserire = false;
                        } else {
                            thislivello = System.Convert.ToInt32(testo.Trim().Substring(2, 1));
                            if (check) {
                                if (lastlivello == thislivello) {
                                    inserire = false;
                                } else {
                                    inserire = true;
                                    lastlivello = thislivello;
                                    check = false;
                                }
                            } else {
                                inserire = true;
                            }
                        }


                        while ((line = sr.ReadLine()) != null && REGEXLIVELLO.Match(line).Length == 0) {
                            testo += " " + line + System.Environment.NewLine;
                        }
                        // tolgo gli endif
                        testo = testo.Replace("<![endif]>", " ");

                        // tolgo gli if
                        while (testo.IndexOf("<![if") != -1) {
                            testo = testo.Remove(testo.IndexOf("<![if"), testo.Substring(testo.IndexOf("<![if")).IndexOf("]>") + 2);
                        }

                        if (inserire) {
                            if (first) {
                                Files.Add(lastbody);
                                first = false;
                            }
                            count++;
                            //POS                     LIVELLO                
                            outputText = count.ToString() + "|" + thislivello + "|" + testo;
                            Files.Add(outputText);
                        } else {

                            lastbody = count.ToString() + "|" + thislivello + "|" + testo;
                        }
                        testo = line;

                    } else {
                        testo = "";
                    }
                }

                return Files;
            } catch (Exception e) {
                throw e;
            } finally {
                sr.Close();
            }
        }

        public static List<string> Export(string file, string extractLocation) {
            return Export(file, extractLocation, "Nuvo Documento");
        }

        /// <summary>
        /// Calcola i parentPageId di ciascuna pagina
        /// </summary>
        /// <param name="List"></param>
        private static void SetParentPage(ISet<Page> List) {

            Page dad = new Page();
            Page fist = new Page();

            foreach (Page pg in List) {
                if (pg.Position == 1) {
                    //è il primo
                    fist = pg;
                    if (pg.Level == 0) {//1
                        // è pure padre
                        dad = pg;
                    }
                } else {
                    //Tutti gli altri
                    if (pg.Level == 0) {//1
                        // è padre
                        dad = pg;
                    } else {
                        if (pg.Level == dad.Level) {
                            //hanno lo stesso livello
                            pg.Parentpageid = dad.Parentpageid;
                            dad = pg;// è padre
                            pg.Dirty = true;
                        } else
                            if (pg.Level > dad.Level) {
                                //pg è figlio
                                pg.Parentpageid = dad.Pageid;
                                pg.Dirty = true;
                                dad = pg;// è padre
                            } else
                                if (pg.Level < dad.Level) {
                                    dad = GetDadID(dad, pg.Level, List);
                                    pg.Parentpageid = dad.Pageid;
                                    //pg.Parentpageid = dad.Parentpageid;
                                    dad = pg;// è padre
                                }
                    }
                }
            }



        }
        /// <summary>
        /// Restituisce la pagina Padre di una pagina Figlia
        /// </summary>
        /// <param name="child"></param>
        /// <param name="fam"></param>
        /// <returns></returns>
        private static Page GetDadID(Page child, int lev, ISet<Page> fam) {

            Page dad = new Page();

            dad = (from f in fam
                   where f.Pageid == child.Parentpageid
                   select f).First<Page>();

            if (dad.Level == lev) {
                dad = (from f in fam
                       where f.Pageid == dad.Parentpageid
                       select f).First<Page>();
                return dad;
            } else {
                if ((dad.Pageid == dad.Parentpageid)) {
                    dad = (from f in fam
                           where f.Pageid == dad.Parentpageid
                           select f).First<Page>();
                    return dad;
                } else {
                    return GetDadID(dad, lev, fam);
                }
            }
        }

        private static Boolean IsDeleted(Page Child, ISet<Page> fam) {

            if (Child.State == 99) {
                return true;
            } else if (Child.Pageid == Child.Parentpageid && Child.State == 1) {
                return false;
            } else {
                var pg = (from p in fam
                          where p.Pageid == Child.Parentpageid
                          select p).FirstOrDefault<Page>();
                return IsDeleted(pg, fam);
            }
        }

        public static List<T> GetContents<T>() {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {

                List<T> cntlist = GetContents<T>(session) as List<T>;

                return cntlist;
            }
        }

        public static List<T> GetContents<T>(ISession session) {
            List<T> cntlist = HibernateHelper.SelectCommand<T>(session, "") as List<T>;
            return cntlist;
        }

        public static Content GetContentById(int contentId, ISession session) {
            Content cont = new Content();
            cont = HibernateHelper.SelectIstance<Content>(session, new string[] { "Contentid" }, new object[] { contentId }, new Operators[] { Operators.Eq });
            return cont;
        }

        public static RawHtml GetRawHtmlById(int RawId, ISession sessison) {

            RawHtml raw = new RawHtml();
            raw = HibernateHelper.SelectIstance<RawHtml>(sessison, new string[] { "Rawhtmlid" }, new object[] { RawId }, new Operators[] { Operators.Eq });
            return raw;
        }

        public static List<Page> GetPageByParent(ISession session, int tmpCont, int tmpPage) {
            List<Page> pageList = HibernateHelper.SelectCommand<Page>(session, " PARENTPAGEID =" + tmpPage + " AND CONTENTID =" + tmpCont) as List<Page>;
            pageList.Sort(delegate(Page pg1, Page pg2) { return pg1.Position.CompareTo(pg2.Position); });
            return pageList;
        }

        public static Page GetBasket(NHibernate.ISession session, int contentId) {

            List<Page> Pages = GetPageByContent(session, contentId);

            return (from c in Pages
                    where c.State == 99
                    select c).FirstOrDefault<Page>();

        }

        public static List<Page> GetPageByContent(ISession session, int contentId) {

            List<Page> pageList = HibernateHelper.SelectCommand<Page>(session, "  CONTENTID =" + contentId) as List<Page>;
            pageList.Sort(delegate(Page pg1, Page pg2) { return pg1.Position.CompareTo(pg2.Position); });
            return pageList;

        }

        public static List<PageElement> GetPageelementByPage(NHibernate.ISession session, int pageid) {
            List<PageElement> pageList = HibernateHelper.SelectCommand<PageElement>(session, "  PAGEID =" + pageid) as List<PageElement>;
            return pageList;
        }

        public static Page GetPageById(int pageId, ISession session) {
            Page page = new Page();
            page = HibernateHelper.SelectIstance<Page>(session, new string[] { "Pageid" }, new object[] { pageId }, new Operators[] { Operators.Eq });
            return page;
        }

        /// <summary>
        /// Publica una pagina in formato html sul fileserver
        /// </summary>
        /// <param name="pageid"></param>
        public static string PublicPage(Page pagina, string fileserver, string pathIdItem, string Title, ISession session) {

            //crea file xml con la struttura della pagina
            string pathxml = Path.Combine(fileserver, pagina.Pageid + "_" + pagina.Title.Trim().Replace(" ", "_") + ".xml");
            if (File.Exists(pathxml)) {
                File.Delete(pathxml);
            }

            XmlTextWriter writer = new XmlTextWriter(pathxml, null);

            XmlDocument docXml = new XmlDocument();
            docXml.AppendChild(docXml.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode page = docXml.CreateNode(XmlNodeType.Element, "Page", null);

            //Nodo TitoloContent
            XmlNode TitoloContent = docXml.CreateNode(XmlNodeType.Element, "TitoloContent", null);
            XmlNode TitoloContentValue = docXml.CreateNode(XmlNodeType.CDATA, null, null);
            TitoloContentValue.Value = Title;
            TitoloContent.AppendChild(TitoloContentValue);
            page.AppendChild(TitoloContent);

            foreach (PageElement pel in pagina.PageElements) {
                XmlNode nodo = docXml.CreateNode(XmlNodeType.Element, pel.Element.Description, null);
                XmlNode nodoValue = docXml.CreateNode(XmlNodeType.CDATA, null, null);

                if (pel.Element.Elementtypeid == (int)ElementTypeEnum.RawHtml) {
                    RawHtml rw = new RawHtml();
                    rw = GetRawHtmlById(pel.Rawhtmlid, session);
                    nodoValue.Value = rw.Value;
                } else
                    if (pel.Element.Elementtypeid == (int)ElementTypeEnum.Img) {
                        nodoValue.Value = pel.Filename;

                    } else {
                        nodoValue.Value = pel.Valore;
                    }

                nodo.AppendChild(nodoValue);

                var el = (from c in pel.Element.ElementSkins
                          where c.Elementid == pel.Element.Elementid
                          select c).FirstOrDefault();

                page.AppendChild(nodo);
            }

            //Nodo Theme
            XmlNode pathTema = docXml.CreateNode(XmlNodeType.Element, "Theme", null);
            XmlAttribute attr = docXml.CreateAttribute("Path");
            attr.Value = @"..\..\Themes\" + pagina.Skin.Path;
            pathTema.Attributes.Append(attr);
            page.AppendChild(pathTema);


            docXml.AppendChild(page);
            docXml.WriteTo(writer);            

            writer.Close();

            //Html
            //Leggo il File XMl della pagina
            XmlTextReader readXml = new XmlTextReader(pathxml);

            //Leggi il File di configurazione della SKIN associata alla pagina
            string FileThemes = ConfigurationSettings.AppSettings["FileThemes"];
            string pathSkinConfig = Path.Combine(FileThemes, pagina.Skin.Path);


            var configXml = from c in XElement.Load(Path.Combine(pathSkinConfig, "skin.config")).Nodes()
                            select c;

            XElement elXslt = (XElement)configXml.ToList().Find(delegate(XNode nd) {
                return ((XElement)nd).Name.LocalName == "page_xslt";
            });

            //Leggo il file xslt
            string pathXslt = Path.Combine(pathSkinConfig, elXslt.Value);

            XmlTextReader readXslt = new XmlTextReader(pathXslt);


            XElement elOut = (XElement)configXml.ToList().Find(delegate(XNode nd) {
                return ((XElement)nd).Name.LocalName == "page_out";
            });
            XElement elExt = (XElement)configXml.ToList().Find(delegate(XNode nd) {
                return ((XElement)nd).Name.LocalName == "page_ext";
            });

            //Nome file Html
            string pageName = string.Empty;
            if (elOut.Value.StartsWith("#")) {
                pageName = pagina.Pageid + "_" + pagina.Title + "." + elExt.Value;
            } else {
                pageName = elOut.Value;
            }
            // Path file Html
            string pagePath = Path.Combine(fileserver, pageName);
            if (File.Exists(pagePath)) {
                File.Delete(pagePath);
            }

            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(readXslt);

            XmlTextWriter mywriter = new XmlTextWriter(pagePath, null);
            myXslTrans.Transform(readXml, null, mywriter);

            readXml.Close();
            readXslt.Close();
            mywriter.Close();

            //cancello il file XML
            File.Delete(pathxml);
            return pageName;
        }

        public static string PublicContent(int contId, string pathIdItem, string Title) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content cont = new Content();
                        cont = HibernateHelper.SelectIstance<Content>(session, new string[] { "Contentid" }, new object[] { contId }, new Operators[] { Operators.Eq });

                        //creo pagina menu' con l'albero delle pagine 
                        string fileserver = ConfigurationSettings.AppSettings["FileServer"];

                        //creo una cartella sul fileserver
                        //string pathCont = Path.Combine(fileserver, cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"));
                        string pathCont = Path.Combine(fileserver, pathIdItem);
                        if (!Directory.Exists(pathCont)) {
                            Directory.CreateDirectory(pathCont);
                        }

                        //Prelevo dallo stage tutti i file e sotto cartelle
                        //string stageserver = ConfigurationSettings.AppSettings["FileStage"];
                        //string pathstage = Path.Combine(stageserver, cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"));
                        //if (Directory.Exists(pathstage)) {

                        //    Copy(pathstage, pathCont);

                        //}

                        //Prelevo dalla directory Img le img di default 
                        string imgserver = ConfigurationSettings.AppSettings["Img"];
                        Copy(imgserver, pathCont);

                        //Publico tutte le pagine del content

                        List<Page> ListTempPage = new List<Page>();

                        foreach (Page pg in cont.Pages) {

                            if (!IsDeleted(pg, cont.Pages)) {
                                PublicPage(pg, pathCont, pathIdItem, Title, session);
                                ListTempPage.Add(pg);
                            }
                        }

                        //creo file xml con la stuttura del menu content 

                        var pages = from f in ListTempPage
                                    orderby f.Position, f.Pageid, f.Parentpageid
                                    select f;

                        DataTable dt = ToDataTable<Page>(pages.ToList<Page>());
                        string FileThemes = ConfigurationSettings.AppSettings["FileThemes"];
                        string pathSkinConfig = Path.Combine(FileThemes, cont.Skin.Path);

                        string xmlpath = Path.Combine(pathCont, "content.xml");
                        using (XmlWriter write = new XmlTextWriter(xmlpath, null)) {

                            XmlDocument docXml = new XmlDocument();
                            docXml = CreateXmlToDataSet(dt);
                            XmlNode pathTema = docXml.CreateNode(XmlNodeType.Element, "Theme", null);
                            XmlAttribute attr = docXml.CreateAttribute("Path");
                            attr.Value = @"..\..\Themes\" + cont.Skin.Path;
                            pathTema.Attributes.Append(attr);

                            docXml.GetElementsByTagName("Menu")[0].AppendChild(pathTema);
                            docXml.WriteTo(write);

                        }

                        //Leggo il File XMl del content
                        XmlTextReader readXml = new XmlTextReader(xmlpath);

                        //Leggi il File di configurazione della SKIN associata al content

                        var configXml = from c in XElement.Load(Path.Combine(pathSkinConfig, "skin.config")).Nodes()
                                        select c;

                        XElement elXslt = (XElement)configXml.ToList().Find(delegate(XNode nd) {
                            return ((XElement)nd).Name.LocalName == "content_xslt";
                        });

                        //Leggo il file xslt
                        string pathXslt = Path.Combine(pathSkinConfig, elXslt.Value);

                        XmlTextReader readXslt = new XmlTextReader(pathXslt);

                        XElement elOut = (XElement)configXml.ToList().Find(delegate(XNode nd) {
                            return ((XElement)nd).Name.LocalName == "content_out";
                        });

                        //Nome file Html
                        string contName = elOut.Value;

                        // Path file Html
                        string contPath = Path.Combine(pathCont, contName);
                        if (File.Exists(contPath)) {
                            File.Delete(contPath);
                        }

                        XslCompiledTransform myXslTrans = new XslCompiledTransform();
                        myXslTrans.Load(readXslt);


                        XmlTextWriter mywriter = new XmlTextWriter(contPath, null);
                        myXslTrans.Transform(readXml, null, mywriter);

                        readXml.Close();
                        readXslt.Close();
                        mywriter.Close();

                        //Cancello il file XML
                        File.Delete(xmlpath);

                        string def = Path.Combine(Path.Combine(FileThemes, cont.Skin.Theme.Path), "default.html");
                        // Path file Html
                        if (File.Exists(Path.Combine(pathCont, "default.html"))) {
                            File.Delete(Path.Combine(pathCont, "default.html"));
                        }

                        File.Copy(def, Path.Combine(pathCont, "default.html"));

                        return pathIdItem + "/default.html";


                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public static string PublicContent(int contId) {
            return PublicContent(contId, " ", " ");
        }

        public static string PublicPage(int idpage, string pathIdItem, string Title) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {

                        Page pg = new Page();
                        pg = HibernateHelper.SelectIstance<Page>(session, new string[] { "Pageid" }, new object[] { idpage }, new Operators[] { Operators.Eq });

                        string fileserver = ConfigurationSettings.AppSettings["FileServer"];
                        string pathCont = Path.Combine(fileserver, pathIdItem);

                        string ret = PublicPage(pg, pathCont, pathIdItem, Title, session);

                        return pathIdItem + @"\" + ret;

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }

        }


        public static Content SavePages(List<String> Files, Content contnt, ISession session, string FolderToSave) {
            ITransaction transaction = session.BeginTransaction();
            try {

                //Theme
                Theme tema = new Theme();
                tema = HibernateHelper.SelectIstance<Theme>(new string[] { "Themeid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                //Skin
                Skin skinPage = new Skin();
                skinPage = HibernateHelper.SelectIstance<Skin>(new string[] { "Skinid" }, new object[] { 2 }, new Operators[] { Operators.Eq });

                //SkinHome
                Skin SkinHome = new Skin();
                SkinHome = HibernateHelper.SelectIstance<Skin>(new string[] { "Skinid" }, new object[] { 4 }, new Operators[] { Operators.Eq });


                //Element Titolo
                Element Titolo = new Element();
                Titolo = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                //Element Contenuto
                Element Contenuto = new Element();
                Contenuto = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 2 }, new Operators[] { Operators.Eq });

                //Elemento Titolo
                Element TitoloMenu = new Element();
                TitoloMenu = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 3 }, new Operators[] { Operators.Eq });

                //Elemento DataCreazione
                Element DataCreazione = new Element();
                DataCreazione = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 4 }, new Operators[] { Operators.Eq });


                //Elemento DataCreazione
                Element DataModifica = new Element();
                DataModifica = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 5 }, new Operators[] { Operators.Eq });


                ISet<Page> setPage = new HashedSet<Page>();

                int actualpos = 0;
                if (contnt.Pages != null && contnt.Pages.Count > 0) {
                    actualpos = (from c in contnt.Pages
                                 select c.Position).Max();
                }

                String pos = "";
                String lev = "";
                foreach (string file in Files) {
                    String info = file.Substring(0, file.IndexOf("<"));
                    String[] split = info.Split('|');
                    pos = split[0];
                    lev = split[1];

                    if (pos.Equals("1") && lev.Equals("0")) {
                        //creo la pagina del menù
                        String bodymenu = file.Substring(file.IndexOf("<"));
                        String titlemenu = bodymenu.Substring(bodymenu.IndexOf("<title") + 7, (bodymenu.IndexOf("</title")) - (bodymenu.IndexOf("<title") + 7));

                        Page menu = new Page();
                        menu.Position = actualpos + Convert.ToInt32(pos);
                        menu.Level = Convert.ToInt32(lev);
                        menu.Title = "home";
                        string TitoloEL = menu.Publictitle;
                        menu.Publictitle = "HOME";
                        menu.State = 1;

                        menu.Skin = SkinHome;
                        menu.Skinid = SkinHome.Skinid;
                        menu.Content = contnt;
                        menu.Contentid = contnt.Contentid;

                        menu.IsNew = true;
                        HibernateHelper.Persist(menu, session);

                        //PageElement
                        ISet<PageElement> setMnEl = new HashedSet<PageElement>();

                        ////Add Logo 
                        //PageElement LogoEl = new PageElement();
                        //LogoEl.Element = Logo;
                        //LogoEl.Elementid = Logo.Elementid;
                        //LogoEl.Value = "Logo";
                        //LogoEl.Filename = "Logo.jpg";
                        //LogoEl.Pageid = menu.Pageid;
                        //LogoEl.Page = menu;
                        //LogoEl.IsNew = true;
                        //HibernateHelper.Persist(LogoEl, session);


                        //string originimg = Path.Combine(ConfigurationSettings.AppSettings["Img"], "Logo.jpg");
                        //if (File.Exists(Path.Combine(FolderToSave, "Logo.jpg"))) {
                        //    File.Delete(Path.Combine(FolderToSave, "Logo.jpg"));
                        //}
                        //File.Copy(originimg, Path.Combine(FolderToSave, "Logo.jpg"));

                        //setMnEl.Add(LogoEl);

                        //Add Titolo
                        PageElement MemutitleEl = new PageElement();
                        MemutitleEl.Element = TitoloMenu;
                        MemutitleEl.Elementid = TitoloMenu.Elementid;
                        MemutitleEl.Valore = titlemenu;
                        MemutitleEl.Pageid = menu.Pageid;
                        MemutitleEl.Page = menu;
                        MemutitleEl.IsNew = true;
                        HibernateHelper.Persist(MemutitleEl, session);

                        setMnEl.Add(MemutitleEl);

                        //Add DataCreazione
                        PageElement DataCreazioneEl = new PageElement();
                        DataCreazioneEl.Element = DataCreazione;
                        DataCreazioneEl.Elementid = DataCreazione.Elementid;
                        DataCreazioneEl.Valore = DateTime.Now.ToShortDateString();
                        DataCreazioneEl.Pageid = menu.Pageid;
                        DataCreazioneEl.Page = menu;
                        DataCreazioneEl.IsNew = true;
                        HibernateHelper.Persist(DataCreazioneEl, session);

                        setMnEl.Add(DataCreazioneEl);
                        //Add DataModifica
                        PageElement DataModificaEL = new PageElement();
                        DataModificaEL.Element = DataModifica;
                        DataModificaEL.Elementid = DataModifica.Elementid;
                        DataModificaEL.Valore = DateTime.Now.ToShortDateString();
                        DataModificaEL.Pageid = menu.Pageid;
                        DataModificaEL.Page = menu;
                        DataModificaEL.IsNew = true;
                        HibernateHelper.Persist(DataModificaEL, session);

                        setMnEl.Add(DataModificaEL);


                        ////Add RowHtml

                        //PageElement MenuBody = new PageElement();
                        //MenuBody.Pageid = menu.Pageid;
                        //MenuBody.Page = menu;
                        //MenuBody.Element = ContenutoHome;
                        //MenuBody.Elementid = ContenutoHome.Elementid;
                        //MenuBody.IsNew = true;

                        //RawHtml MenuRawHtml = new RawHtml();
                        //MenuRawHtml.IsNew = true;
                        //MenuRawHtml.Value = " ";
                        //HibernateHelper.Persist(MenuRawHtml, session);

                        //MenuBody.Filename = MenuRawHtml.Rawhtmlid + "_RawHtml.jpg";
                        //MenuBody.Value = "RawHtml";
                        //MenuBody.Rawhtmlid = MenuRawHtml.Rawhtmlid;
                        //HibernateHelper.Persist(MenuBody, session);
                        /////Generare JPG e salvarla in FolderToSave
                        /////  Add immagine blank

                        //string emptyfile = Path.Combine(FolderToSave, MenuRawHtml.Rawhtmlid + "_RawHtml.htm");

                        //string htmlDocument = "<html><body></body></html>";
                        //FileStream fs = File.OpenWrite(emptyfile);
                        //StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                        //writer.Write(htmlDocument);
                        //writer.Close();
                        //Editor.Helper.WebSiteThumbnail.SaveImage(emptyfile, FolderToSave);

                        ////cancello il file temporaneo html
                        //File.Delete(emptyfile);

                        menu.PageElements = setMnEl;
                        setMnEl = menu.PageElements;
                        menu.Parentpageid = menu.Pageid;
                        menu.Dirty = true;
                        HibernateHelper.Persist(menu, session);

                        setPage.Add(menu);

                    } else {

                        String body = file.Substring(file.IndexOf("<"));

                        String title = body.Substring(body.IndexOf("<h"), (body.IndexOf("</h")) - (body.IndexOf("<h")));

                        string pattern = "<[^<>]+>";
                        Regex rgx = new Regex(pattern);
                        Regex num = new Regex("[0-9]");
                        Regex punt = new Regex(@"[\t\r\n\e\a._%+-/]");

                        Page page = new Page();
                        page.Position = actualpos + Convert.ToInt32(pos);
                        page.Level = Convert.ToInt32(lev);
                        string temp = rgx.Replace(title, "");
                        temp = punt.Replace(temp, " ");

                        page.Publictitle = num.Replace(temp, " ").Replace("&nbsp;", "").Trim();
                        page.Title = punt.Replace(rgx.Replace(title, "").Replace("&nbsp;", "").Trim().Replace(" ", "_"), "_");
                        page.State = 1;


                        String elbody = rgx.Replace(body.Substring(body.IndexOf("</h") + 5), "");
                        elbody = num.Replace(elbody, "");
                        elbody = punt.Replace(elbody, " ");
                        elbody = elbody.Trim().Replace(" ", "").ToLower();

                        elbody = elbody.Replace("&nbsp;", "");
                        elbody = elbody.Replace("‘", "");
                        elbody = elbody.Replace("’", "");
                        elbody = elbody.Replace("'", "");

                        if (elbody.StartsWith("nonapplicabile")) {
                            page.State = 2;
                        }
                        page.Skin = skinPage;
                        page.Skinid = skinPage.Skinid;
                        page.Content = contnt;
                        page.Contentid = contnt.Contentid;

                        page.IsNew = true;
                        HibernateHelper.Persist(page, session);

                        //PageElement
                        ISet<PageElement> setPgEl = new HashedSet<PageElement>();
                        //Add Titolo
                        PageElement titleEl = new PageElement();
                        titleEl.Element = Titolo;
                        titleEl.Elementid = Titolo.Elementid;
                        titleEl.Valore = page.Publictitle;
                        titleEl.Pageid = page.Pageid;
                        titleEl.Page = page;
                        titleEl.IsNew = true;
                        HibernateHelper.Persist(titleEl, session);
                        setPgEl.Add(titleEl);

                        //Add Contenuto
                        PageElement contEl = new PageElement();
                        contEl.Element = Contenuto;
                        contEl.Elementid = Contenuto.Elementid;

                        contEl.Valore = "RawHtml";


                        contEl.Pageid = page.Pageid;
                        contEl.Page = page;
                        contEl.IsNew = true;

                        RawHtml contraw = new RawHtml();
                        contraw.IsNew = true;
                        contraw.Value = body.Substring(body.IndexOf("</h") + 5);
                        HibernateHelper.Persist(contraw, session);

                        contEl.Filename = contraw.Rawhtmlid + "_RawHtml.jpg";
                        contEl.Rawhtmlid = contraw.Rawhtmlid;
                        HibernateHelper.Persist(contEl, session);
                        setPgEl.Add(contEl);

                        ///TODO:Generare JPG e salvarla in FolderToSave
                        ///
                        string contrawfile = Path.Combine(FolderToSave, contraw.Rawhtmlid + "_RawHtml.htm");

                        string htmlDocument = contraw.Value;
                        FileStream fs = File.OpenWrite(contrawfile);
                        StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                        writer.Write(htmlDocument);
                        writer.Close();
                        Editor.Helper.WebSiteThumbnail.SaveImage(contrawfile, FolderToSave);
                        //cancello il file temporaneo html
                        File.Delete(contrawfile);

                        page.PageElements = setPgEl;
                        setPgEl = page.PageElements;
                        page.Parentpageid = page.Pageid;
                        page.Dirty = true;
                        HibernateHelper.Persist(page, session);

                        setPage.Add(page);
                    }

                }
                Page Cestino = new Page();
                Cestino.Title = Cestino.Publictitle = "Cestino";
                Cestino.Structureid = 3;
                Cestino.Skin = null;
                Cestino.State = 99;
                Cestino.Position = 1;
                Cestino.IsNew = true;
                Cestino.Contentid = contnt.Contentid;
                HibernateHelper.Persist(Cestino, session);
                Cestino.Parentpageid = Cestino.Pageid;
                Cestino.Dirty = true;
                HibernateHelper.Persist(Cestino, session);

                SetParentPage(setPage);

                contnt.Pages = setPage;
                setPage = contnt.Pages;
                contnt.Dirty = true;
                contnt.Parentcontentid = contnt.Contentid;
                HibernateHelper.Persist(contnt, session);

                transaction.Commit();

                return contnt;

            } catch (Exception ex) {
                throw ex;
            }
        }

        public static Content SavePages(List<String> Files, Content contnt, ISession session) {
            return SavePages(Files, contnt, session, @"D:\GIT_SRC\MYSTIC\MysticEditor\Editor.WEB\Fileserver\999999");
        }
    }
}
