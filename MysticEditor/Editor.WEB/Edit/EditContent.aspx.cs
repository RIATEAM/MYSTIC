using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BL;
using Editor.BE;
using NHibernate;

namespace Editor.Web.Edit {
    public partial class EditContent : System.Web.UI.Page {

        protected static string id = "";

        protected void Page_Load(object sender, EventArgs e) {
            lbTitle.Text = "Titolo";
            if (!Page.IsPostBack) {
                id = Request.QueryString["id"];
                if (id.Length > 0) {
                    Save.Attributes.Add("onclick", "return refreshMenu()");
                    using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                        ITransaction transaction = session.BeginTransaction();
                        try {
                            int contentId = Convert.ToInt32(id);
                            Editor.BE.Model.Content content = new Editor.BE.Model.Content();
                            content = EditorServices.GetContentById(contentId, session);
                            txtTitle.Text = content.Title;

                            pages.DataSource = content.Pages;
                            //pages.DataMember = "PAGE";
                            pages.DataBind();
                        } catch (Exception ex) {
                            transaction.Rollback();
                            throw ex;
                        } finally {
                            session.Flush();
                            session.Close();
                        }
                    }
                } else {
                    txtTitle.Text = "Id Content sconosciuto";
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e) {
            using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                ITransaction transaction = session.BeginTransaction();
                try {
                    int contentId = Convert.ToInt32(id);
                    Editor.BE.Model.Content content = new Editor.BE.Model.Content();
                    content = EditorServices.GetContentById(contentId, session);
                    content.Title = txtTitle.Text;
                    content.Dirty = true;
                    HibernateHelper.Persist(content, session);
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
