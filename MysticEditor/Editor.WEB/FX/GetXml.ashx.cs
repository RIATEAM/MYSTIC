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

namespace Editor.Web.FX {
    /// <summary>
    /// Descrizione di riepilogo per $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetXml : IHttpHandler {  

        public   void ProcessRequest(HttpContext context) {

            string contID = context.Request.QueryString["contentID"];
            
            PageServices srvc = new PageServices();

            DataSet ds = new DataSet();
            IList<PageDTO> pages = new List<PageDTO>();
            pages = srvc.GetPagesByContentId(Convert.ToInt32(contID));

            ds = ConvertToDataSet(pages);

            XmlDocument docXml = new XmlDocument();
            docXml.AppendChild(docXml.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            docXml.InnerXml += ds.GetXml();


            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Output.Write(docXml.InnerXml);
        }

        private DataSet ConvertToDataSet(IList<PageDTO> pages) {
            DataSet ds = new DataSet("PAGES");
            DataTable dt = new DataTable("PAGE");
            dt = EditorServices.ToDataTable<PageDTO>(pages);

           
            dt.TableName = "PAGE";
            
            ds.Tables.Add(dt);

            DataRelation relation = new DataRelation("ParentChild",ds.Tables["PAGE"].Columns["Pageid"]
   , ds.Tables["PAGE"].Columns["Parentpageid"]
    ,true);

            relation.Nested = true;
            ds.Relations.Add(relation);
            

            DataRow Cestino = dt.Rows[0];

            foreach (DataRow dr in dt.Rows) {
                if (Convert.ToInt32(dr["State"]) == 99) {
                   Cestino =dr ;                    
                }
            }

            if (Convert.ToInt32(Cestino["State"]) == 99) {
                ds.Tables["PAGE"].Rows.Remove(Cestino);            
            }
            
           
            ds.Tables["PAGE"].Columns.Remove("PageelementsList");
            ds.Tables["PAGE"].Columns.Remove("Title");
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
