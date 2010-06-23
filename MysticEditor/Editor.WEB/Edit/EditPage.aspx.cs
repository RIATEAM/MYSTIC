using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Editor.BE;
using Editor.BL;
using Syrinx.Gui.AspNet;

namespace Editor.Web.Edit {
    public partial class EditPage : System.Web.UI.Page {

        protected static string id = "";

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                id = Request.QueryString["id"];
                if (id.Length > 0) {
                    Save.Attributes.Add("onclick", "return refreshMenu()");
                    using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                        ITransaction transaction = session.BeginTransaction();
                        try {
                            int pageId = Convert.ToInt32(id);
                            Editor.BE.Model.Page page = new Editor.BE.Model.Page();
                            page = EditorServices.GetPageById(pageId, session);

                            TitoloAlbero.Text = page.Publictitle;

                            foreach (Editor.BE.Model.PageElement pgEl in page.PageElements) {

                                switch (pgEl.Element.ElementType.Description) {

                                    case "LabelText":
                                        TableRow row = new TableRow();
                                        TableCell lbRow = new TableCell();
                                        Label lab = new Label();
                                        lab.Text = pgEl.Element.Description;
                                        lbRow.Controls.Add(lab);
                                        row.Cells.Add(lbRow);

                                        TableCell lbTxt = new TableCell();
                                        TextBox txt = new TextBox();
                                        txt.Width = 600;
                                        txt.Height = 30;
                                        txt.Text = pgEl.Valore;
                                        lbTxt.Controls.Add(txt);
                                        row.Cells.Add(lbTxt);

                                        Table.Rows.Add(row);  
                                        
                                        break;
                                    case "RawHtml":
                                        TableRow riga = new TableRow();
                                        TableCell lbriga = new TableCell();
                                        Label labriga = new Label();
                                        labriga.Text = pgEl.Element.Description;
                                        lbriga.Controls.Add(labriga);
                                        riga.Cells.Add(lbriga);

                                        TableCell lbBody = new TableCell();
                                        CkEditor editor = new CkEditor();
                                        editor.Text = pgEl.Valore;
                                        lbBody.Controls.Add(editor);
                                        riga.Cells.Add(lbBody);

                                        Table.Rows.Add(riga);                                   
                                        
                                        break;

                                    default: break;
                                }
                            }
                        } catch (Exception ex) {
                            transaction.Rollback();
                            throw ex;
                        } finally {
                            session.Flush();
                            session.Close();
                        }
                    }
                } else {
                    Table.Rows[0].Cells[0].Text = "Id Page sconosciuto";
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e) {
 
        }

    
    }
}
