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

namespace MysticEditorSample.WebSite.Editor_FX
{
    public partial class PageView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                hPageid.Value = Request.QueryString["idp"];
                hItemID.Value = Request.QueryString["iditem"];
                hType.Value = Request.QueryString["type"];
        }
    }
}
}
