<%@ WebHandler Language="C#" Class="SSO.LoginHandler" %>

using System;
using System.Web;
using System.Text;
using System.Xml.Serialization;

namespace SSO
{
    public class LoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;

            //BL.whoAmI(Thread.CurrentPrincipal.Identity.Name.ToString) restituisce UserDTO
            UserDTO user = new UserDTO();
            user.Role = "user";

            XmlSerializer ser = new XmlSerializer(typeof(UserDTO));
            ser.Serialize(context.Response.Output, user);


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}