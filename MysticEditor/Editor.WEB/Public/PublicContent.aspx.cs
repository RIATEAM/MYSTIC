using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BL;

namespace Editor.Web.Public {
    public partial class PublicContent : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                List<Editor.BE.Model.Content> LCont = EditorServices.GetContents<Editor.BE.Model.Content>();
                DropDownList1.DataSource = LCont;
                DropDownList1.DataMember = "Title";
                DropDownList1.DataTextField = "Title";
                DropDownList1.DataValueField = "Contentid";
                DropDownList1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e) {
            
            string response = string.Empty;
            response = EditorServices.PublicContent(Convert.ToInt32(DropDownList1.SelectedValue));

            response = "~/Fileserver/" + response;

            Response.Redirect(response);
        }
    }
}
