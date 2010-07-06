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
        /// Setta la position di ciascun figlio
        /// </summary>
        /// <param name="setPage"></param>
        public static void SetPosition(ISet<Page> List) {


            foreach (Page pg in List) {
                int position = 1;
                var child = from cd in List
                            where cd.Parentpageid == pg.Pageid
                            orderby cd.Position ascending
                            select cd;
                foreach (Page ch in child) {

                    ch.Position = position;
                    ch.Dirty = true;
                    position++;

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
            return IsDeleted(Child, fam.ToList<Page>());
        }

        private static Boolean IsDeleted(Page Child, List<Page> fam) {
            if (Child.State == 99) {
                return true;
            } else if (Child.Pageid == Child.Parentpageid && Child.State == 1) {
                return false;
            } else {
                var pg = (from p in fam
                          where p.Pageid == Child.Parentpageid
                          select p).FirstOrDefault<Page>();
                if (pg != null)
                    return IsDeleted(pg, fam);
                else
                    return false;
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
            List<Page> pageList = HibernateHelper.SelectCommand<Page>(session, " PARENTPAGEID =" + tmpPage + "AND PAGEID <>" + tmpPage + " AND CONTENTID =" + tmpCont) as List<Page>;
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

        public static List<Widget> GetWidgetByContent(NHibernate.ISession session, int contentId) {
            List<Widget> pageList = HibernateHelper.SelectCommand<Widget>(session, "  CONTENTID =" + contentId) as List<Widget>;
            pageList.Sort(delegate(Widget pg1, Widget pg2) { return pg1.Position.CompareTo(pg2.Position); });


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

        public static Widget GetWidgetById(int WidgetId, NHibernate.ISession session) {
            Widget page = new Widget();
            page = HibernateHelper.SelectIstance<Widget>(session, new string[] { "Widgetid" }, new object[] { WidgetId }, new Operators[] { Operators.Eq });
            return page;
        }

        public static WidgetElement GetWidgetElementById(int WidgetElementId, NHibernate.ISession session) {
            WidgetElement page = new WidgetElement();
            page = HibernateHelper.SelectIstance<WidgetElement>(session, new string[] { "Widgetelementid" }, new object[] { WidgetElementId }, new Operators[] { Operators.Eq });
            return page;
        }


        public static string PublicPage(int idpage, string pathIdItem, string Title) {

            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {

                        Page pg = new Page();
                        pg = HibernateHelper.SelectIstance<Page>(session, new string[] { "Pageid" }, new object[] { idpage }, new Operators[] { Operators.Eq });

                        string fileserver = ConfigurationSettings.AppSettings["ServerPath"];
                        string pathCont = Path.Combine(fileserver, pathIdItem);

                        string ret = PublicPage(pg, pathCont, pathIdItem, Title);

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

        public static string PublicPage(Page pagina, string fileserver, string pathIdItem, string Title) {
            using (ISession _session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = _session.BeginTransaction()) {
                    try {
                        XmlDocument docXml = new XmlDocument();
                        var widgets = from f in pagina.Content.Widgets
                                      orderby f.Position, f.Widgetid
                                      select f;
                        XmlNode WIDGETS = docXml.CreateNode(XmlNodeType.Element, "WIDGETS", "");

                        WIDGETS = CreateWidgetXML(WIDGETS, docXml, widgets);

                        return PublicPage(pagina, fileserver, pathIdItem, Title, WIDGETS, docXml, _session);

                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        _session.Flush();
                        _session.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Publica una pagina in formato html sul fileserver
        /// </summary>
        /// <param name="pageid"></param>
        private static string PublicPage(Page pagina, string fileserver, string pathIdItem, string Title, XmlNode WIDGETS, XmlDocument docXml, ISession session) {
            //crea file xml con la struttura della pagina
            string pathxml = Path.Combine(fileserver, pagina.Pageid + "_" + pagina.Title.Trim().Replace(" ", "_") + ".xml");
            if (File.Exists(pathxml)) {
                File.Delete(pathxml);
            }

            XmlTextWriter writer = new XmlTextWriter(pathxml, null);

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
            attr.Value = @"\Themes\" + pagina.Skin.Path;
            pathTema.Attributes.Append(attr);
            page.AppendChild(pathTema);

            //Nodo Content
            XmlNode ContentNode = docXml.CreateNode(XmlNodeType.Element, "Content", null);
            XmlAttribute ContentState = docXml.CreateAttribute("State");
            ContentState.Value = pagina.Content.State.ToString();
            ContentNode.Attributes.Append(ContentState);
            XmlAttribute ContentDataPublish = docXml.CreateAttribute("DatePublish");
            ContentDataPublish.Value = " ";
            if (pagina.Content.Date_publish != null) {
                ContentDataPublish.Value = pagina.Content.Date_publish.ToString();
            }
            ContentNode.Attributes.Append(ContentDataPublish);
            XmlAttribute ContentPublishActive = docXml.CreateAttribute("PublishActive");
            ContentPublishActive.Value = pagina.Content.Publish_active.ToString();
            ContentNode.Attributes.Append(ContentPublishActive);
            page.AppendChild(ContentNode);


            // Nodo Child
            XmlNode Childs = docXml.CreateNode(XmlNodeType.Element, "Childs", null);

            List<Page> ListChild = new List<Page>();
            ListChild = GetPageByParent(session, pagina.Contentid, pagina.Pageid);

            List<Page> ListTempChild = new List<Page>();

            foreach (Page pg in ListChild) {
                if (!IsDeleted(pg, ListChild)) {
                    ListTempChild.Add(pg);
                }
            }
            ListTempChild.Sort(delegate(Page p1, Page p2) { return p1.Position.CompareTo(p2.Position); });
            DataTable dt = ToDataTable<Page>(ListTempChild);
            Childs.InnerXml = CreateNodeCilds(dt).Replace("<Childs>", "").Replace("</Childs>", "").Replace("<Childs />", "");

            page.AppendChild(Childs);

            page.AppendChild(WIDGETS);

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
            //File.Delete(pathxml);
            return pageName;
        }

        public static string PublicContent(int contId) {
            return PublicContent(contId, " ", " ");
        }

        public static string PublicContent(int contId, string pathIdItem, string Title) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    try {
                        Content cont = new Content();
                        cont = HibernateHelper.SelectIstance<Content>(session, new string[] { "Contentid" }, new object[] { contId }, new Operators[] { Operators.Eq });

                        //creo pagina menu' con l'albero delle pagine 
                        string fileserver = ConfigurationSettings.AppSettings["ServerPath"];

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
                        XmlDocument docXml = new XmlDocument();
                        var widgets = from f in cont.Widgets
                                      orderby f.Position, f.Widgetid
                                      select f;
                        XmlNode WIDGETS = docXml.CreateNode(XmlNodeType.Element, "WIDGETS", "");

                        WIDGETS = CreateWidgetXML(WIDGETS, docXml, widgets);

                        foreach (Page pg in cont.Pages) {

                            if (!IsDeleted(pg, cont.Pages)) {
                                PublicPage(pg, pathCont, pathIdItem, Title, WIDGETS, docXml, session);
                                ListTempPage.Add(pg);
                                docXml.RemoveAll();
                            }
                        }

                        //creo file xml con la stuttura del menu content 

                        var pages = from f in ListTempPage
                                    orderby f.Position, f.Pageid, f.Parentpageid
                                    select f;
                        DataTable dt = ToDataTable<Page>(pages.ToList<Page>());

                        string FileThemes = ConfigurationSettings.AppSettings["FileThemes"];
                        string pathSkinConfig = Path.Combine(FileThemes, cont.Skin.Path);
                        
                        docXml.RemoveAll();
                        string xmlpath = Path.Combine(pathCont, "content.xml");
                        using (XmlWriter write = new XmlTextWriter(xmlpath, null)) {


                            docXml = CreateXmlToDataSet(dt, docXml);
                            XmlNode pathTema = docXml.CreateNode(XmlNodeType.Element, "Theme", null);
                            XmlAttribute attr = docXml.CreateAttribute("Path");
                            attr.Value = @"\Themes\" + cont.Skin.Path;
                            pathTema.Attributes.Append(attr);

                            docXml.GetElementsByTagName("Menu")[0].AppendChild(pathTema);

                            docXml.GetElementsByTagName("Menu")[0].AppendChild(WIDGETS);

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
                        //File.Delete(xmlpath);

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

        private static XmlNode CreateWidgetXML(XmlNode WIDGETS, XmlDocument docXml, IOrderedEnumerable<Widget> widgets) {
            foreach (Widget widget in widgets) {

                XmlNode WIDGET = docXml.CreateNode(XmlNodeType.Element, "WIDGET", "");

                XmlAttribute Widgetid = docXml.CreateAttribute("Id");
                Widgetid.Value = widget.Widgetid.ToString();
                WIDGET.Attributes.Append(Widgetid);

                XmlAttribute Titolo = docXml.CreateAttribute("Titolo");
                Titolo.Value = widget.Publictitle.ToString();
                WIDGET.Attributes.Append(Titolo);

                XmlAttribute Position = docXml.CreateAttribute("Position");
                Position.Value = widget.Position.ToString();
                WIDGET.Attributes.Append(Position);

                XmlAttribute State = docXml.CreateAttribute("State");
                State.Value = widget.State.ToString();
                WIDGET.Attributes.Append(State);


                // XmlNode WIDGETELEMENTS = docXml.CreateNode(XmlNodeType.Element, "WIDGETELEMENTS", "");

                foreach (WidgetElement WDTO in widget.WidgetElements) {

                    XmlNode WIDGETELEMENT = docXml.CreateNode(XmlNodeType.Element, "WIDGETELEMENT", "");

                    XmlAttribute Widgetelementid = docXml.CreateAttribute("Id");
                    Widgetelementid.Value = WDTO.Widgetelementid.ToString();
                    WIDGETELEMENT.Attributes.Append(Widgetelementid);

                    XmlAttribute Valore = docXml.CreateAttribute("Href");
                    Valore.Value = WDTO.Valore.ToString();
                    WIDGETELEMENT.Attributes.Append(Valore);

                    XmlAttribute Name = docXml.CreateAttribute("Titolo");
                    Name.Value = WDTO.Name.ToString();
                    WIDGETELEMENT.Attributes.Append(Name);

                    XmlAttribute Position_ = docXml.CreateAttribute("Position");
                    Position_.Value = WDTO.Position.ToString();
                    WIDGETELEMENT.Attributes.Append(Position_);

                    XmlAttribute Target = docXml.CreateAttribute("Target");
                    Target.Value = "_new";
                    WIDGETELEMENT.Attributes.Append(Target);

                    WIDGET.AppendChild(WIDGETELEMENT);
                    //WIDGETELEMENTS.AppendChild(WIDGETELEMENT);
                }

                //WIDGET.AppendChild(WIDGETELEMENTS);

                WIDGETS.AppendChild(WIDGET);
            }
            return WIDGETS;
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


                //Elemento Logo
                //Element Logo = new Element();
                //Logo = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 7 }, new Operators[] { Operators.Eq });

                //Elemento Titolo
                Element TitoloMenu = new Element();
                TitoloMenu = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 3 }, new Operators[] { Operators.Eq });

                //Elemento Corpo
                Element CorpoMenu = new Element();
                CorpoMenu = HibernateHelper.SelectIstance<Element>(new string[] { "Elementid" }, new object[] { 8 }, new Operators[] { Operators.Eq });


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
                        menu.State = (int)PageStateEnum.Nessuna;
                        menu.Structureid = 2;

                        menu.Skin = SkinHome;
                        menu.Skinid = SkinHome.Skinid;
                        menu.Content = contnt;
                        menu.Contentid = contnt.Contentid;

                        menu.IsNew = true;
                        HibernateHelper.Persist(menu, session);

                        //PageElement
                        ISet<PageElement> setMnEl = new HashedSet<PageElement>();

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

                        //Add RowHtml

                        PageElement MenuBody = new PageElement();
                        MenuBody.Pageid = menu.Pageid;
                        MenuBody.Page = menu;
                        MenuBody.Element = CorpoMenu;
                        MenuBody.Elementid = CorpoMenu.Elementid;
                        MenuBody.IsNew = true;

                        RawHtml MenuRawHtml = new RawHtml();
                        MenuRawHtml.IsNew = true;
                        MenuRawHtml.Value = " ";
                        HibernateHelper.Persist(MenuRawHtml, session);

                        MenuBody.Filename = MenuRawHtml.Rawhtmlid + "_RawHtml.jpg";
                        MenuBody.Valore = "RawHtml";
                        MenuBody.Rawhtmlid = MenuRawHtml.Rawhtmlid;
                        HibernateHelper.Persist(MenuBody, session);
                        ///Generare JPG e salvarla in FolderToSave
                        ///  Add immagine blank

                        string emptyfile = Path.Combine(FolderToSave, MenuRawHtml.Rawhtmlid + "_RawHtml.htm");

                        string htmlDocument = "<html><body></body></html>";
                        FileStream fs = File.OpenWrite(emptyfile);
                        StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                        writer.Write(htmlDocument);
                        writer.Close();
                        Editor.Helper.WebSiteThumbnail.SaveImage(emptyfile, FolderToSave);

                        //cancello il file temporaneo html
                        File.Delete(emptyfile);

                        setMnEl.Add(MenuBody);

                        //Add DataCreazione
                        PageElement DataCreazioneEl = new PageElement();
                        DataCreazioneEl.Element = DataCreazione;
                        DataCreazioneEl.Elementid = DataCreazione.Elementid;
                        DataCreazioneEl.Valore = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                        DataCreazioneEl.Pageid = menu.Pageid;
                        DataCreazioneEl.Page = menu;
                        DataCreazioneEl.IsNew = true;
                        HibernateHelper.Persist(DataCreazioneEl, session);

                        setMnEl.Add(DataCreazioneEl);
                        //Add DataModifica
                        PageElement DataModificaEL = new PageElement();
                        DataModificaEL.Element = DataModifica;
                        DataModificaEL.Elementid = DataModifica.Elementid;
                        DataModificaEL.Valore = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                        DataModificaEL.Pageid = menu.Pageid;
                        DataModificaEL.Page = menu;
                        DataModificaEL.IsNew = true;
                        HibernateHelper.Persist(DataModificaEL, session);

                        setMnEl.Add(DataModificaEL);


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
                        Regex punt = new Regex(@"[\t\r\n\e\a._%+-/€’‘]");

                        Regex puntPub = new Regex(@"[\t\r\n\e\a._%+-/]");

                        Page page = new Page();
                        page.Position = actualpos + Convert.ToInt32(pos);
                        page.Level = Convert.ToInt32(lev);
                        string temp = rgx.Replace(title, "");
                        temp = punt.Replace(temp, " ");

                        page.Publictitle = ReplaceCharacters(num.Replace(rgx.Replace(puntPub.Replace(title," "), ""), "").Trim());
                        page.Title = punt.Replace(rgx.Replace(title, "").Replace("&nbsp;", "").Trim().Replace(" ", "_"), "_");

                        if (page.Level == 1) {
                            page.State = (int)PageStateEnum.NonCliccabile;
                        } else {
                            page.State = (int)PageStateEnum.Nessuna;
                        }

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
                        page.Structureid = 1;
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
                        contraw.Value = ReplaceCharacters(contraw.Value);
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

                //Widget Applicativi Link
                Widget AppLink = new Widget();
                AppLink.Position = 1;
                AppLink.Publictitle = "Link Utili";
                AppLink.Title = "Link_Utili";
                AppLink.Skinid = 0;
                AppLink.Skin = skinPage;
                AppLink.State = (int)PageStateEnum.NonCliccabile;
                AppLink.Contentid = contnt.Contentid;
                AppLink.Structureid = 4;
                AppLink.IsNew = true;
                HibernateHelper.Persist(AppLink, session);


                //Setto il padre di ciascuna pagina
                SetParentPage(setPage);
                //Setto le position 
                SetPosition(setPage);

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
