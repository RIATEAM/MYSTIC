using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace SSO
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Thread.CurrentPrincipal.Identity.Name.ToString();
            Label2.Text = Page.User.Identity.Name.ToString();
            Label3.Text = HttpContext.Current.User.Identity.Name.ToString();
            Label4.Text = Environment.UserDomainName + "/" + Environment.UserName.ToString();
        }
    }
}
