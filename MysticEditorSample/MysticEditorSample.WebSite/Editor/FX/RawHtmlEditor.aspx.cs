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
using Editor.Services;
using Editor.DTO;
using Editor.BE.Model;
using System.IO;

namespace MysticEditorSample.WebSite.Editor_FX
{
    public partial class RawHtmlEditor : System.Web.UI.Page
    {
        private int RawHtmlId = 0;
        private PageServices pgserv = new PageServices();
        private int iditemamm = 0;
        private string type = "";
        private string idPageElement = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            //type = Request.QueryString["type"];
            idPageElement = Request.QueryString["idpel"];
            //iditemamm = Convert.ToInt32(Request.QueryString["iditem"]);
            if (!Page.IsPostBack)
            {


                PageServices pgserv = new PageServices();

                PageElementDTO pel = new PageElementDTO();
                pel = pgserv.GetPageelementByPageelementID(Convert.ToInt32(idPageElement));

                RawHtml row = new RawHtml();
                row = pgserv.GetRawHtmlById(pel.Rawhtmlid);
                edit.Text = row.Value;
                RawHtmlId = row.Rawhtmlid;

                PageDTO page = new PageDTO();
                page = pgserv.GetPage(pel.Pageid);
                title.Text = page.Publictitle;
                path.Text = page.Path;
            }

        }


        private static object _lock = new object();
        protected void Save_Click(object sender, EventArgs e)
        {

            lock (_lock)
            {

                PageElementDTO pel = new PageElementDTO();
                pel = pgserv.GetPageelementByPageelementID(Convert.ToInt32(idPageElement));

                PageDTO page = new PageDTO();
                page = pgserv.GetPage(pel.Pageid);

                Editor.Services.ContentServices contsrv = new Editor.Services.ContentServices();
                contsrv.SetStateContent(page.Contentid, (int)Editor.BE.Model.Enumerators.ContentStateEnum.NonAllineato);


                string FolderToSave = @"";
                FolderToSave = Path.Combine(FolderToSave, page.Contentid.ToString());                


                //if (type == "std")
                //{

                //    FolderToSave = Path.Combine(FolderToSave, "contenutiAdm");
                //    FolderToSave = Path.Combine(FolderToSave, iditemamm.ToString());

                //}
                //else if (type == "com")
                //{

                //    cms.Entities.Document doc = new cms.Entities.Document(Convert.ToInt32(iditemamm));
                //    doc.Load();
                //    FolderToSave = Path.Combine(FolderToSave, "contenutiComm");
                //    FolderToSave = Path.Combine(FolderToSave, doc.IdMarket + @"\" + doc.IdFeature + @"\" + doc.id);


                //}

                //		PageElementDTO pel = pgserv.GetPageelementByPageelementID(Convert.ToInt32(idPageElement));
                //		pel = pgserv.GetPageelementByPageelementID(Convert.ToInt32(idPageElement));

                RawHtml row = pgserv.GetRawHtmlById(pel.Rawhtmlid);
                row = pgserv.GetRawHtmlById(row.Rawhtmlid);

                row.Value = edit.Text;
                row.Dirty = true;
                pgserv.SaveRawHtml(row, FolderToSave);
            }
        }

    
    
    }
}
