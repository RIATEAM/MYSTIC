using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BL;

namespace MysticEditorSample.WebSite
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           List<Editor.BE.Model.Content> Lista = new List<Editor.BE.Model.Content>();
            Lista = EditorServices.GetContents<Editor.BE.Model.Content>();


            GridView1.DataSource = Lista;
            GridView1.DataBind();

        }

    }
}
