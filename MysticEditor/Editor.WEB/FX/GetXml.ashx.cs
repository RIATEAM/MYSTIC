using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using Editor.BL;
using Editor.DTO;
using Editor.Services;
using System.Xml;
using System.Xml.Serialization;

namespace Editor.Web.FX {
    /// <summary>
    /// Descrizione di riepilogo per $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetXml : IHttpHandler {

        public void ProcessRequest(HttpContext context) {

            string contID = context.Request.QueryString["contentID"];

            PageServices srvc = new PageServices();

            DataSet ds = new DataSet();
            IList<PageDTO> pages = new List<PageDTO>();
            pages = srvc.GetPagesByContentId(Convert.ToInt32(contID));

            ds = ConvertToDataSet(pages);

            XmlDocument docXml = new XmlDocument();

            docXml.AppendChild(docXml.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode Content = docXml.CreateNode(XmlNodeType.Element, "CONTENT", "");

            Content.InnerXml = ds.GetXml();


            WidgetServices wsvc = new WidgetServices();

            DataSet wid = new DataSet();
            IList<WidgetDTO> widgs = new List<WidgetDTO>();
            widgs = wsvc.GetWidgetsByContentId(Convert.ToInt32(contID));


            XmlNode WIDGETS = docXml.CreateNode(XmlNodeType.Element, "WIDGETS", "");

            foreach (WidgetDTO widget in widgs) {

                XmlNode WIDGET = docXml.CreateNode(XmlNodeType.Element, "WIDGET", "");

                XmlAttribute Widgetid = docXml.CreateAttribute("Widgetid");
                Widgetid.Value = widget.Widgetid.ToString();
                WIDGET.Attributes.Append(Widgetid);

                XmlAttribute Title = docXml.CreateAttribute("Title");
                Title.Value = widget.Title.ToString();
                WIDGET.Attributes.Append(Title);

                XmlAttribute Publictitle = docXml.CreateAttribute("Publictitle");
                Publictitle.Value = widget.Publictitle.ToString();
                WIDGET.Attributes.Append(Publictitle);

                XmlAttribute Contentid = docXml.CreateAttribute("Contentid");
                Contentid.Value = widget.Contentid.ToString();
                WIDGET.Attributes.Append(Contentid);

                XmlAttribute Structureid = docXml.CreateAttribute("Structureid");
                Structureid.Value = widget.Structureid.ToString();
                WIDGET.Attributes.Append(Structureid);

                XmlAttribute Position = docXml.CreateAttribute("Position");
                Position.Value = widget.Position.ToString();
                WIDGET.Attributes.Append(Position);

                XmlAttribute Skinid = docXml.CreateAttribute("Skinid");
                Skinid.Value = widget.Skinid.ToString();
                WIDGET.Attributes.Append(Skinid);

                XmlAttribute State = docXml.CreateAttribute("State");
                State.Value = widget.State.ToString();
                WIDGET.Attributes.Append(State);


               // XmlNode WIDGETELEMENTS = docXml.CreateNode(XmlNodeType.Element, "WIDGETELEMENTS", "");

                foreach (WidgetElementDTO WDTO in widget.WidgetElementsList) {

                    XmlNode WIDGETELEMENT = docXml.CreateNode(XmlNodeType.Element, "WIDGETELEMENT", "");

                    XmlAttribute Widgetelementid = docXml.CreateAttribute("Widgetelementid");
                    Widgetelementid.Value = WDTO.Widgetelementid.ToString();
                    WIDGETELEMENT.Attributes.Append(Widgetelementid);

                    XmlAttribute Elementid = docXml.CreateAttribute("Elementid");
                    Elementid.Value = WDTO.Elementid.ToString();
                    WIDGETELEMENT.Attributes.Append(Elementid);

                    XmlAttribute Widgetid_ = docXml.CreateAttribute("Widgetid");
                    Widgetid_.Value = WDTO.Widgetid.ToString();
                    WIDGETELEMENT.Attributes.Append(Widgetid_);

                    XmlAttribute Valore = docXml.CreateAttribute("Valore");
                    Valore.Value = WDTO.Valore.ToString();
                    WIDGETELEMENT.Attributes.Append(Valore);

                    XmlAttribute Name = docXml.CreateAttribute("Publictitle");
                    Name.Value = WDTO.Name.ToString();
                    WIDGETELEMENT.Attributes.Append(Name);

                    XmlAttribute Position_ = docXml.CreateAttribute("Position");
                    Position_.Value = WDTO.Position.ToString();
                    WIDGETELEMENT.Attributes.Append(Position_);

                    WIDGET.AppendChild(WIDGETELEMENT);
                    //WIDGETELEMENTS.AppendChild(WIDGETELEMENT);
                }

                //WIDGET.AppendChild(WIDGETELEMENTS);
                
                WIDGETS.AppendChild(WIDGET);
            }


           // wid = ConvertToDataSetWidget(widgs);


            //Content.InnerXml += wid.GetXml();

            Content.InnerXml += WIDGETS.OuterXml;


            docXml.AppendChild(Content);

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Output.Write(docXml.InnerXml);

        }

        private DataSet ConvertToDataSetWidget(IList<WidgetDTO> widgs) {
            DataSet ds = new DataSet("WIDGETS");
            DataTable dt = new DataTable("WIDGET");
            dt = EditorServices.ToDataTable<WidgetDTO>(widgs);

            dt.TableName = "WIDGET";
            ds.Tables.Add(dt);


            //ds.Tables["WIDGET"].Columns.Remove("WidgetElementsList");

            ds.Tables["WIDGET"].Columns["WidgetElementsList"].ColumnMapping = MappingType.Element;
            ds.Tables["WIDGET"].Columns["WidgetElementsList"].Namespace = null;

            ds.Tables["WIDGET"].Columns["WidgetElementsList"].ColumnName = "WIDGETELEMENTS";

            ds.Tables["WIDGET"].Columns["Title"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Widgetid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Contentid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Structureid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Position"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Publictitle"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["Skinid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["WIDGET"].Columns["State"].ColumnMapping = MappingType.Attribute;

            ds.Tables["WIDGET"].Columns["IsPersisted"].ColumnMapping = MappingType.Hidden;
            ds.Tables["WIDGET"].Columns["IsNew"].ColumnMapping = MappingType.Hidden;
            ds.Tables["WIDGET"].Columns["Dirty"].ColumnMapping = MappingType.Hidden;
            ds.Tables["WIDGET"].Columns["Deleted"].ColumnMapping = MappingType.Hidden;
            ds.Tables["WIDGET"].Columns["HasChanged"].ColumnMapping = MappingType.Hidden;

            return ds;
        }

        private DataSet ConvertToDataSet(IList<PageDTO> pages) {
            DataSet ds = new DataSet("PAGES");
            DataTable dt = new DataTable("PAGE");
            dt = EditorServices.ToDataTable<PageDTO>(pages);

            dt.TableName = "PAGE";

            ds.Tables.Add(dt);

            DataRelation relation = new DataRelation("ParentChild", ds.Tables["PAGE"].Columns["Pageid"]
   , ds.Tables["PAGE"].Columns["Parentpageid"]
    , true);

            relation.Nested = true;
            ds.Relations.Add(relation);


            DataRow Cestino = dt.Rows[0];

            foreach (DataRow dr in dt.Rows) {
                if (Convert.ToInt32(dr["State"]) == 99) {
                    Cestino = dr;
                }
            }

            if (Convert.ToInt32(Cestino["State"]) == 99) {
                ds.Tables["PAGE"].Rows.Remove(Cestino);
            }


            ds.Tables["PAGE"].Columns.Remove("PageelementsList");
            ds.Tables["PAGE"].Columns["Title"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Pageid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Parentpageid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Contentid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Structureid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Position"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Level"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Publictitle"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Skinid"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["Level"].ColumnMapping = MappingType.Attribute;
            ds.Tables["PAGE"].Columns["State"].ColumnMapping = MappingType.Attribute;

            ds.Tables["PAGE"].Columns["IsPersisted"].ColumnMapping = MappingType.Hidden;
            ds.Tables["PAGE"].Columns["IsNew"].ColumnMapping = MappingType.Hidden;
            ds.Tables["PAGE"].Columns["Dirty"].ColumnMapping = MappingType.Hidden;
            ds.Tables["PAGE"].Columns["Deleted"].ColumnMapping = MappingType.Hidden;
            ds.Tables["PAGE"].Columns["HasChanged"].ColumnMapping = MappingType.Hidden;


            foreach (DataRow dr in dt.Rows) {
                if (Convert.ToInt32(dr["Pageid"]) == Convert.ToInt32(dr["Parentpageid"])) {
                    dr["Parentpageid"] = DBNull.Value;
                }
            }


            return ds;
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}
