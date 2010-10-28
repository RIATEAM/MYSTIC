using System;
using System.Threading;

namespace SSO
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Boolean DEBUG = true;
            string destUrl;

            //BL.whoAmI(Thread.CurrentPrincipal.Identity.Name.ToString) restituisce UserDTO
            UserDTO user = new UserDTO();
            user.Role = "user";

            if (user != null)
            {
                lblmsg.Text = "UTENTE NON VALIDO";
            }

            if (user.isAdmin())
            {

                destUrl = "adminapp.html";

                // L'utente è valido
                if (DEBUG == true) lblmsg.Text = destUrl; else Response.Redirect(destUrl);

            }
            else
            {

                destUrl = "userapp.html";

                // L'utente è valido
                if (DEBUG == true) lblmsg.Text = destUrl; else Response.Redirect(destUrl);
            }
        }
    }
}
