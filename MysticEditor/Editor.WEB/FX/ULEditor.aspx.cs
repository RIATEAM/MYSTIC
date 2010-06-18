using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.Services;
using Editor.DTO;
using System.Configuration;
using System.IO;

namespace Editor.Web.FX {
    public partial class ULEditor : System.Web.UI.Page {
        private static int WidgetElementid = 0;
        private static int iditemamm = 0;
        private static WidgetServices widgserv = new WidgetServices();
        protected void Page_Load(object sender, EventArgs e) {

            if (!Page.IsPostBack) {
                string idWidgetElement = Request.QueryString["idwel"];
                iditemamm = Convert.ToInt32(Request.QueryString["iditem"]);
                WidgetServices widgserv = new WidgetServices();
                WidgetElementDTO pel = new WidgetElementDTO();
                WidgetElementid = Convert.ToInt32(idWidgetElement);
                pel = widgserv.GetWidgetElement(WidgetElementid);

                Link.Text = pel.Valore;

            }

        }

        protected void Save_Click(object sender, EventArgs e) {
            if ( Link.Text.Length > 0) {

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

        protected void Upload_Click(object sender, EventArgs e) {

            string FolderToSave = ConfigurationSettings.AppSettings["ServerPath"];
            FolderToSave = Path.Combine(FolderToSave, "contenutiAdm");
            FolderToSave = Path.Combine(FolderToSave, iditemamm.ToString());

            if (FileUpload.PostedFile.FileName.Length > 0) {

                FileUpload.PostedFile.SaveAs(Path.Combine(FolderToSave, FileUpload.PostedFile.FileName));

                //Carico il file 

                WidgetServices widgserv = new WidgetServices();
                WidgetElementDTO pel = new WidgetElementDTO();
                WidgetElementid = Convert.ToInt32(WidgetElementid);
                pel = widgserv.GetWidgetElement(WidgetElementid);

                pel.Valore = Path.GetFileName(FileUpload.PostedFile.FileName);
                pel.Dirty = true;

                pel = widgserv.SaveWidgetElement(pel);

                Link.Text = pel.Valore;

            }

        }
    }
}
