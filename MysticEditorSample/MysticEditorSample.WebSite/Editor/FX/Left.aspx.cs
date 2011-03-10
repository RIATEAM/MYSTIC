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
    public partial class Left : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                hContentID.Value = Request.QueryString["idc"];
                hItemID.Value = Request.QueryString["iditem"];
                hType.Value = Request.QueryString["type"];
                //   btnPreviewAll.PostBackUrl = "http://10.12.150.114/cms/editor/FX/anteprima.aspx?idc=" + hContentID.Value + "&iditem=" + hItemID.Value;
                //    btnPublishAll.PostBackUrl = "http://10.12.150.114/cms/editor/FX/pubblica.aspx?idc=" + hContentID.Value + "&iditem=" + hItemID.Value;
        }
    }
}
}