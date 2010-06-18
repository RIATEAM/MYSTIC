using System;
using System.Collections.Generic;
using System.IO;
using Editor.BL;
using NHibernate;
using Editor.BE;
using System.Configuration;

namespace Editor.Web {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {

                List<Editor.BE.Model.Content> LCont = EditorServices.GetContents<Editor.BE.Model.Content>();

                DropDownList1.DataSource = LCont;
                DropDownList1.DataMember = "Title";
                DropDownList1.DataTextField = "Title";
                DropDownList1.DataValueField = "Contentid";
                DropDownList1.DataBind();
            }
        }
        protected void btnCompila_Click(object sender, EventArgs e) {

            var file = Request.Files["fuDoc"];
            if (file == null) {
                return;
            }

            try {
                using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                    ITransaction transaction = session.BeginTransaction();
                    try {
                        Editor.BE.Model.Content cont = new Editor.BE.Model.Content();

                        if (!CheckBox1.Checked) {
                            cont = EditorServices.GetContentById(Convert.ToInt32(DropDownList1.SelectedItem.Value), session);
                        } else {
                            //Skin
                            Editor.BE.Model.Skin skinCont = new Editor.BE.Model.Skin();
                            skinCont = HibernateHelper.SelectIstance<Editor.BE.Model.Skin>(new string[] { "Skinid" }, new object[] { 1 }, new Operators[] { Operators.Eq });

                            //Content                           
                            cont.Title = "Content " + DateTime.Now.ToShortTimeString();
                            cont.IsNew = true;
                            cont.Skinid = skinCont.Skinid;
                            cont.Skin = skinCont;
                            HibernateHelper.Persist(cont, session);
                        }


                        string fileserver = ConfigurationSettings.AppSettings["FileStage"];

                        //creo una cartella sul FileStage
                        string pathCont = Path.Combine(fileserver, cont.Contentid.ToString() + "_" + cont.Title.Trim().Replace(" ", "_"));
                        if (!Directory.Exists(pathCont)) {
                            Directory.CreateDirectory(pathCont);
                        }

                        string fileLocation = Path.Combine(pathCont, Path.GetFileName(file.FileName));
                        file.SaveAs(fileLocation);


                        //Unzip Files
                        EditorServices.UnZipFiles(fileLocation, pathCont, "", true);
                        List<string> list = EditorServices.Export(Directory.GetFiles(pathCont)[0], pathCont);

                        EditorServices.SavePages(list, cont,session,fileserver);


                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        session.Flush();
                        session.Close();
                    }
                }

                Response.Redirect("~/Edit/Index.htm");
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
