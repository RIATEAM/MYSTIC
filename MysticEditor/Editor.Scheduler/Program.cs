using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor.BE;
using Editor.BE.Model;
using System.Configuration;
using Editor.BL;
using System.Data.OracleClient;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.IO;

namespace Editor.Scheduler {
    public class Program {

        public static void Main(string[] args) {

            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(container);
            CMSCustomer customer = container.Resolve<CMSCustomer>();

            Logs.Start("");
            string sql = "DATE_PUBLISH = '" + String.Format("{0:dd-mm-yyyy}", DateTime.Now) + "' and PUBLISH_ACTIVE=1";
            string[] fields = new string[] { "Date_publish", "Publish_active" };
            object[] ids = new object[] { String.Format("{0:dd-MM-yyyy}", DateTime.Now), 1 };
            Operators[] ops = new Operators[] { Operators.Eq, Operators.Eq };
            List<Content> contents = HibernateHelper.SelectCommand<Content>(fields, ids, ops);
            if (contents.Count > 0) {
                for (int i = 0; i < contents.Count; i++) {

                    Logs.WriteLine(string.Format("Inizio pubblicazione content {0} (ID={1}) (TYPE={2})",
                        contents[i].Title, contents[i].Contentid, contents[i].Repository));

                    Pubblica(contents[i], customer);
                }
            }
            Logs.Dispose();
        }

        private static void Pubblica(Content content, CMSCustomer customer) {
            string fileserver = ConfigurationSettings.AppSettings["ServerPath"];

            string idcont = content.Contentid.ToString();
            string iditemamm = content.Iditem.ToString();
            string type = content.Repository;

            if (type == "std") {
                //Contenuti Standard
            //    cms.CoreLib.Net1.validation validation = new cms.CoreLib.Net1.validation();

                string Title = customer.GetItemTitle(Convert.ToInt32(iditemamm), type);
                string pathIdItem = customer.GetItemPath(iditemamm, type);
                //   string pathIdItem = @"contenutiAdm\" + iditemamm;
                //   string response = string.Empty;
                EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, Title);

                //   response = "~/" + response.Replace("\\", "/");

                //Ricavo l'id Utente
                int iditemuser = Convert.ToInt32(customer.GetItemIdUser(Convert.ToInt32(iditemamm)));
                //cms.CoreLib.Net1.Item NewItem = new cms.CoreLib.Net1.Item();
                //OracleDataReader objReader = NewItem.GetItemContent(Convert.ToInt32(iditemamm));
                //objReader.Read();
                //if (objReader["corrispond_item_id"] != DBNull.Value) {
                //    iditemuser = Convert.ToInt32(objReader["corrispond_item_id"]);
                //    cms.CoreLib.Net1.Item NewItemUS = new cms.CoreLib.Net1.Item();
                //    OracleDataReader objReaderUS = NewItemUS.GetItemContent(iditemuser);
                //    objReaderUS.Read();
                //    if (objReaderUS["item_contents_id"] != DBNull.Value) {
                //        int CorrispondIDContentUS = Convert.ToInt32(objReaderUS["item_contents_id"]);
                //        NewItem.UpdContentStyle(CorrispondIDContentUS, 'F', "default.html");
                //    }
                //    objReaderUS.Close();
                //}
                //objReader.Close();
                string pathIdItemUser = @"contenuti\" + iditemuser;

                CopyDirectory(Path.Combine(fileserver, pathIdItem), Path.Combine(fileserver, pathIdItemUser), true);

                //response = "~/" + @"contenuti/" + iditemuser + @"/default.html";

                //Response.Redirect(response);
            } else if (type == "com") {
                //Contenuti Commerciali

                //cms.Entities.Document doc = new cms.Entities.Document(Convert.ToInt32(iditemamm));
                //doc.Load();

                string Title = customer.GetItemTitle(Convert.ToInt32(iditemamm), type);
             //   string pathIdItem = doc.View("");
                //string pathIdItemUser = "";
                //pathIdItemUser = pathIdItem = pathIdItem.Substring(0, pathIdItem.IndexOf("/Editor/")).Replace("/", @"\");
                //pathIdItemUser = pathIdItem + @"\";
                //pathIdItem = pathIdItem + @"\Admin";

                string pathIdItem = customer.GetItemPath(iditemamm, type);
                string pathIdItemUser = pathIdItem;// = pathIdItem.Substring(0, pathIdItem.IndexOf("/Editor/")).Replace("/", @"\");
                pathIdItemUser = pathIdItem + @"\";
                pathIdItem = pathIdItem + @"\Admin";
                string response = string.Empty;

                EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, Title);

                CopyDirectory(Path.Combine(fileserver, pathIdItem), Path.Combine(fileserver, pathIdItemUser), true);

                //pathIdItemUser = pathIdItemUser.Replace(@"\", "/");
                //response = "~/" + pathIdItemUser + @"/default.html";

                //Response.Redirect(response);
            }
        }
        private static void CopyDirectory(string sourcePath, string destPath, bool overwrite) {
            System.IO.DirectoryInfo sourceDir = new System.IO.DirectoryInfo(sourcePath);
            System.IO.DirectoryInfo destDir = new System.IO.DirectoryInfo(destPath);
            if ((sourceDir.Exists)) {
                if (!(destDir.Exists)) {
                    destDir.Create();
                }

                foreach (FileInfo file in sourceDir.GetFiles()) {
                    if ((overwrite)) {
                        file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), true);
                    } else {
                        if (((System.IO.File.Exists(System.IO.Path.Combine(destDir.FullName, file.Name))) == false)) {
                            file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), false);
                        }
                    }
                }
                foreach (DirectoryInfo dir in sourceDir.GetDirectories()) {
                    CopyDirectory(dir.FullName, System.IO.Path.Combine(destDir.FullName, dir.Name), overwrite);
                }
            }
        }

    }
}
