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
using System.IO;
using System.Collections.Generic;
using NHibernate;
using Editor.BE;
using Editor.BL;

namespace MysticEditorSample.WebSite
{
    public partial class Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                Upload.Attributes.Add("onClick", "return StartUpload()");

            }
            Close.Attributes.Add("onClick", "javascript:window.close();");
        }

        protected void Upload_Compila(object sender, EventArgs e)
        { //Rendo Null probabile variabile di sessione
            //Session["id_content"] = null;

            //Nome del file da compilare
            string filename;
            filename = tbxFileName.Text;

            //User Id che ha effettuato il caricamento
            string userid = "admin";
            //userid = Session["id_user"].ToString();

            //Stinga del FileserverUpd
            string FolderToSave;
            FolderToSave = Server.MapPath("") + "\\UploadedFiles\\";
            //FolderToSave = ConfigurationSettings.AppSettings["UpdTmpPath"] + "upd" + userid + "\\";

            string Filepath;
            Filepath = Path.Combine(FolderToSave, filename);


            if (!String.IsNullOrEmpty(userid))
            {
                //Controllo se esiste il file da compilare
                if (File.Exists(Filepath))
                {
                    //Analisi del documento
                    List<string> ListFile = new List<string>();
                    string Title =TextBox1.Text;
                    ListFile = Editor.BL.EditorServices.Export(Filepath, FolderToSave, Title);
                    if (ListFile.Count > 0)
                    {
                        using (ISession session = HibernateHelper.GetSession().OpenSession())
                        {
                            ITransaction transaction = session.BeginTransaction();
                            try
                            {

                                //Creo il Content
                                //Skin
                                Editor.BE.Model.Skin skinCont = new Editor.BE.Model.Skin();
                                skinCont = HibernateHelper.SelectIstance<Editor.BE.Model.Skin>(new string[] { "Skinid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                                //Content  
                                Editor.BE.Model.Content cont = new Editor.BE.Model.Content();
                                cont.Title = TextBox1.Text;
                                cont.IsNew = true;
                                cont.Skinid = skinCont.Skinid;
                                cont.Skin = skinCont;
                                cont.Themeid = 1;
                                cont.State = (int)Editor.BE.Model.Enumerators.ContentStateEnum.NonAllineato;
                                HibernateHelper.Persist(cont, session);

                                //Creo le pagine del content passando il percorso
                                EditorServices.SavePages(ListFile, cont, session, FolderToSave);


                                //Session["id_content"] = cont.Contentid;

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                session.Flush();
                                session.Close();
                            }
                        }

                    }
                    else
                    {

                        Response.Write("<script language=javascript>");
                        Response.Write("alert('Il Documento non contiene pagine.');");
                        Response.Write("</script/>");

                    }

                    Response.Write("<script language=javascript>");
                    Response.Write("alert('Compilazione completata.');");
                    Response.Write("</script/>");

                    Close.Visible = true;

                }
                else
                {
                    Response.Write("<script language=javascript>");
                    Response.Write("alert('Il file specificato non è stato trovato tra quelli caricati.');");
                    Response.Write("</script/>");

                }
            }
            else
            {
                Response.Write("<script language=javascript>");
                Response.Write("alert('UserId non valorizzato.');");
                Response.Write("</script/>");
            }
        }


}
}

