using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BL;
using NHibernate;
using Editor.BE;

namespace Editor.Web.Public {
    public partial class PublicPage : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    Editor.BE.Model.Page pagina = new Editor.BE.Model.Page();
                    pagina = HibernateHelper.SelectIstance<Editor.BE.Model.Page>(session, new string[] { "Pageid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                    EditorServices.PublicPage(pagina, " "," "," ");

                    EditorServices.PublicPage(pagina.Pageid, "", Title);

                } catch (Exception ex) {
                    transaction.Rollback();
                    throw ex;
                } finally {
                    session.Flush();
                    session.Close();
                }
            }

        }
    }
}
