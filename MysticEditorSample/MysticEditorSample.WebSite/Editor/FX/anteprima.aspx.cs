using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Editor.BL;
using System.IO;
using Editor.Services;
using Editor.DTO;

namespace MysticEditorSample.WebSite.Editor_FX
{
    public partial class anteprima : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idcont = Request.QueryString["idc"];
            string iditemamm = Request.QueryString["iditem"];
            string type = Request.QueryString["type"];

            //EditorService edsrvc = new EditorService();

            //if (type == "std") {
            //    //Contenuti Standard

            //    cms.Entities.TreeItem Item = new cms.Entities.TreeItem(Convert.ToInt32(iditemamm));
            //    DataSet DsItem = new DataSet();
            //    DsItem = Item.LoadItem(Convert.ToInt32(iditemamm));

            //    string Title = "";
            //    if (DsItem.Tables[0].Rows[0]["item_name"] != DBNull.Value) {
            //        Title = DsItem.Tables[0].Rows[0]["item_name"].ToString();
            //    }
            ContentServices contserv = new ContentServices();

            ContentDTO cont = new ContentDTO();
            cont = contserv.GetContentByID(Convert.ToInt32(idcont));

            string pathIdItem = Path.Combine(ConfigurationSettings.AppSettings["ServerPath"], "Stage");
            pathIdItem = Path.Combine(pathIdItem, idcont);
            string response = string.Empty;
            response = EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, cont.Title);

            response = "~/" + response.Replace("\\", "/");
            response = "~" + response.Substring(response.IndexOf("/Fileserver"));
            Response.Redirect(response);
            //} else if (type == "com") {
            //    //Contenuti Commerciali

            //    cms.Entities.Document doc = new cms.Entities.Document(Convert.ToInt32(iditemamm));
            //    doc.Load();

            //    string Title = doc.Title.ToString();
            //    string pathIdItem = edsrvc.GetItemPath(iditemamm, type);            

            //    string response = string.Empty;

            //    response = EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, Title);

            //    response = "~/" + response.Replace("\\", "/");

            //    Response.Redirect(response);
        }
    }
}
