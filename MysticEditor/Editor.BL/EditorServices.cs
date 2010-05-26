using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Editor.BE;
using Editor.BE.Model;
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
        public static List<string> Export(string file, string extractLocation) {

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
        public static void PublicPage(Page pagina, string fileserver, ISession session) {

            //crea file xml con la struttura della pagina
            string pathxml = Path.Combine(fileserver, pagina.Pageid + "_" + pagina.Title.Trim().Replace(" ", "_") + ".xml");
            if (!File.Exists(pathxml)) {
                File.Delete(pathxml);
            }

            XmlTextWriter writer = new XmlTextWriter(pathxml, null);

            XmlDocument docXml = new XmlDocument();
            docXml.AppendChild(docXml.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode page = docXml.CreateNode(XmlNodeType.Element, "Page", null);

            foreach (PageElement pel in pagina.PageElements) {
                XmlNode nodo = docXml.CreateNode(XmlNodeType.Element, pel.Element.Description, null);
                XmlNode nodoValue = docXml.CreateNode(XmlNodeType.CDATA, null, null);

                nodoValue.Value = pel.Value;

                nodo.AppendChild(nodoValue);

                var el = (from c in pel.Element.ElementSkins
                          where c.Elementid == pel.Element.Elementid
                          select c).FirstOrDefault();

                page.AppendChild(nodo);
            }

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
                pageName = elOut.Value + "." + elExt.Value;
            }
            // Path file Html
            string pagePath = Path.Combine(fileserver, pageName);
            if (!File.Exists(pagePath)) {
                File.Delete(pagePath);
            }

            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(readXslt);

            XmlTextWriter mywriter = new XmlTextWriter(pagePath, null);
            myXslTrans.Transform(readXml, null, mywriter);

            readXml.Close();
            readXslt.Close();
            mywriter.Close();

        }

        public static string PublicContent(int contId) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content cont = new Content();
                        cont = HibernateHelper.SelectIstance<Content>(session, new string[] { "Contentid" }, new object[] { contId }, new Operators[] { Operators.Eq });

                        //creo pagina menu' con l'albero delle pagine 
                        string fileserver = ConfigurationSettings.AppSettings["FileServer"];

                        //creo una cartella sul fileserver
                        string pathCont = Path.Combine(fileserver, cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"));
                        if (!Directory.Exists(pathCont)) {
                            Directory.CreateDirectory(pathCont);
                        }

                        //Prelevo dallo stage tutti i file e sotto cartelle
                        string stageserver = ConfigurationSettings.AppSettings["FileStage"];
                        string pathstage = Path.Combine(stageserver, cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"));
                        if (Directory.Exists(pathstage)) {

                            Copy(pathstage, pathCont);

                        }


                        //Publico tutte le pagine del content
                        foreach (Page pg in cont.Pages) {
                            PublicPage(pg, pathCont, session);
                        }

                        //creo file xml con la stuttura del menu content 

                        var pages = from f in cont.Pages
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
                            attr.Value = @"\Themes\" + cont.Skin.Path;
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


                        string def = Path.Combine(Path.Combine(FileThemes, cont.Skin.Theme.Path), "default.html");
                        // Path file Html
                        if (File.Exists(Path.Combine(pathCont, "default.html"))) {
                            File.Delete(Path.Combine(pathCont, "default.html"));
                        }

                        File.Copy(def, Path.Combine(pathCont, "default.html"));

                        return Path.Combine(cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"), "default.html");


                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }
            }
        }

        public static Content SavePages(List<String> Files, Content contnt, ISession session) {
            ITransaction transaction = session.BeginTransaction();
            try {

                //Theme
                Theme tema = new Theme();
                tema = HibernateHelper.SelectIstance<Theme>(new string[] { "Themeid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                //Skin
                Skin skinPage = new Skin();
                skinPage = HibernateHelper.SelectIstance<Skin>(new string[] { "Skinid" }, new object[] { 2 }, new Operators[] { Operators.Eq });

                //Element Titolo
                Element Titolo = new Element();
                Titolo = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                //Element Contenuto
                Element Contenuto = new Element();
                Contenuto = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 2 }, new Operators[] { Operators.Eq });

                //Elemento Logo
                Element Logo = new Element();
                Logo = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 3 }, new Operators[] { Operators.Eq });

                //Elemento Titolo
                Element TitoloMenu = new Element();
                TitoloMenu = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 4 }, new Operators[] { Operators.Eq });

                //Elemento Sottotitolo
                Element Sottotitolo = new Element();
                Sottotitolo = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 6 }, new Operators[] { Operators.Eq });

                // Elemento Immagine
                Element Immagine = new Element();
                Immagine = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 5 }, new Operators[] { Operators.Eq });


                //Elemento Link
                Element Link = new Element();
                Link = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 7 }, new Operators[] { Operators.Eq });



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
                        menu.Title = menu.Publictitle = "index";
                        menu.State = 1;

                        menu.Skin = skinPage;
                        menu.Skinid = skinPage.Skinid;
                        menu.Content = contnt;
                        menu.Contentid = contnt.Contentid;

                        menu.IsNew = true;
                        HibernateHelper.Persist(menu, session);

                        //PageElement
                        ISet<PageElement> setMnEl = new HashedSet<PageElement>();

                        //Add Logo 
                        PageElement LogoEl = new PageElement();
                        LogoEl.Element = Logo;
                        LogoEl.Elementid = Logo.Elementid;
                        LogoEl.Value = "Logo";
                        LogoEl.Filename = "Logo.jpg";
                        LogoEl.Pageid = menu.Pageid;
                        LogoEl.Page = menu;
                        LogoEl.IsNew = true;
                        HibernateHelper.Persist(LogoEl, session);

                        setMnEl.Add(LogoEl);

                        //Add Titolo
                        PageElement MemutitleEl = new PageElement();
                        MemutitleEl.Element = TitoloMenu;
                        MemutitleEl.Elementid = TitoloMenu.Elementid;
                        MemutitleEl.Value = menu.Publictitle;
                        MemutitleEl.Pageid = menu.Pageid;
                        MemutitleEl.Page = menu;
                        MemutitleEl.IsNew = true;
                        HibernateHelper.Persist(MemutitleEl, session);

                        setMnEl.Add(MemutitleEl);

                        //Add Immagine
                        PageElement ImmagineEl = new PageElement();
                        ImmagineEl.Element = Immagine;
                        ImmagineEl.Elementid = Immagine.Elementid;
                        ImmagineEl.Value = "Immagine";
                        ImmagineEl.Filename = "Immagine.jpg";
                        ImmagineEl.Pageid = menu.Pageid;
                        ImmagineEl.Page = menu;
                        ImmagineEl.IsNew = true;
                        HibernateHelper.Persist(ImmagineEl, session);

                        setMnEl.Add(ImmagineEl);

                        //Sottotitolo
                        PageElement SottotitoloEl = new PageElement();
                        SottotitoloEl.Element = Sottotitolo;
                        SottotitoloEl.Elementid = Sottotitolo.Elementid;
                        SottotitoloEl.Value = "Sottotitolo " + menu.Publictitle;
                        SottotitoloEl.Pageid = menu.Pageid;
                        SottotitoloEl.Page = menu;
                        SottotitoloEl.IsNew = true;
                        HibernateHelper.Persist(SottotitoloEl, session);

                        setMnEl.Add(SottotitoloEl);

                        //Link
                        PageElement LinkEl = new PageElement();
                        LinkEl.Element = Link;
                        LinkEl.Elementid = Link.Elementid;
                        LinkEl.Value = "Links ";
                        LinkEl.Pageid = menu.Pageid;
                        LinkEl.Page = menu;
                        LinkEl.IsNew = true;
                        HibernateHelper.Persist(LinkEl, session);

                        setMnEl.Add(LinkEl);

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

                        page.Publictitle = num.Replace(temp, " ").Trim();
                        page.Title = punt.Replace(rgx.Replace(title, "").Trim().Replace(" ", "_"), "_");
                        page.State = 1;


                        String elbody = rgx.Replace(body.Substring(body.IndexOf("</h") + 5), "");
                        elbody = num.Replace(elbody, "");
                        elbody = punt.Replace(elbody, " ");
                        elbody = elbody.Trim().Replace(" ", "").ToLower();

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
                        titleEl.Value = page.Publictitle;
                        titleEl.Pageid = page.Pageid;
                        titleEl.Page = page;
                        titleEl.IsNew = true;
                        HibernateHelper.Persist(titleEl, session);
                        setPgEl.Add(titleEl);

                        //Add Contenuto
                        PageElement contEl = new PageElement();
                        contEl.Element = Contenuto;
                        contEl.Elementid = Contenuto.Elementid;

                        contEl.Value = "Rawhtml";
                        contEl.Filename = "";

                        contEl.Pageid = page.Pageid;
                        contEl.Page = page;
                        contEl.IsNew = true;

                        RawHtml contraw = new RawHtml();
                        contraw.IsNew = true;
                        contraw.Value = body.Substring(body.IndexOf("</h") + 5);
                        HibernateHelper.Persist(contraw, session);

                        contEl.Rawhtmlid = contraw.Rawhtmlid;
                        HibernateHelper.Persist(contEl, session);
                        setPgEl.Add(contEl);

                        page.PageElements = setPgEl;
                        setPgEl = page.PageElements;
                        page.Parentpageid = page.Pageid;
                        page.Dirty = true;
                        HibernateHelper.Persist(page, session);

                        setPage.Add(page);
                    }

                }
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

    }
}
