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
using System.IO;

namespace MysticEditorSample.WebSite.Editor_FX
{

    public partial class ULEditor : System.Web.UI.Page
    {
        private static int WidgetElementid = 0;
        private static int iditemamm = 0;
        private static WidgetServices widgserv = new WidgetServices();
        private static string type = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            type = Request.QueryString["type"];
            string idWidgetElement = Request.QueryString["idwel"];
            iditemamm = Convert.ToInt32(Request.QueryString["iditem"]);

            if (!Page.IsPostBack)
            {
                if (Convert.ToInt32(idWidgetElement) != 0)
                {
                    WidgetServices widgserv = new WidgetServices();
                    WidgetElementDTO pel = new WidgetElementDTO();
                    WidgetElementid = Convert.ToInt32(idWidgetElement);
                    pel = widgserv.GetWidgetElement(WidgetElementid);

                    Link.Text = pel.Valore;

                }
            }

        }
        protected void Save_Click(object sender, EventArgs e)
        {
            if (Link.Text.Length > 0)
            {

                WidgetServices widgserv = new WidgetServices();
                WidgetElementDTO pel = new WidgetElementDTO();
                WidgetElementid = Convert.ToInt32(WidgetElementid);
                pel = widgserv.GetWidgetElement(WidgetElementid);

                pel.Valore = Link.Text;
                pel.Dirty = true;

                pel = widgserv.SaveWidgetElement(pel);

                Link.Text = pel.Valore;
            }


        }
        protected void Upload_Click(object sender, EventArgs e)
        {
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
            //    FolderToSave = Path.Combine(FolderToSave, doc.IdMarket + @"\" + doc.IdFeature + @"\" + doc.id + @"\Admin");
            //}

            if (FileUpload.PostedFile.FileName.Length > 0)
            {
                WidgetServices widgserv = new WidgetServices();
                WidgetElementDTO pel = new WidgetElementDTO();
                WidgetElementid = Convert.ToInt32(WidgetElementid);
                pel = widgserv.GetWidgetElement(WidgetElementid);

                WidgetDTO widget = new WidgetDTO();
                widget = widgserv.GetWidget(pel.Widgetid);

                string FolderToSave = Path.Combine(ConfigurationSettings.AppSettings["ServerPath"], widget.Contentid.ToString());

                //Carico il file 
                FileUpload.PostedFile.SaveAs(Path.Combine(FolderToSave, Path.GetFileName(FileUpload.PostedFile.FileName)));


                pel.Valore = Path.GetFileName(FileUpload.PostedFile.FileName);
                pel.Dirty = true;

                pel = widgserv.SaveWidgetElement(pel);

                Link.Text = pel.Valore;

            }

        }

    }
}
