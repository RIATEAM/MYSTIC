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
using Editor.BL;
using Editor.Services;
using Editor.DTO;

namespace MysticEditorSample.WebSite.Editor_FX
{
    public partial class pubblica : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string idcont = Request.QueryString["idc"];
            string iditemamm = Request.QueryString["iditem"];
            string type = Request.QueryString["type"];

            string fileserver = ConfigurationSettings.AppSettings["ServerPath"];
            //EditorService edsrvc = new EditorService();

            Editor.Services.ContentServices contsrv = new Editor.Services.ContentServices();
            contsrv.SetStateContent(Convert.ToInt32(idcont), (int)Editor.BE.Model.Enumerators.ContentStateEnum.Allineato);


            //if (type == "std") {
            //    //Contenuti Standard

            //    cms.Entities.TreeItem Item = new cms.Entities.TreeItem(Convert.ToInt32(iditemamm));
            //    DataSet DsItem = new DataSet();
            //    DsItem = Item.LoadItem(Convert.ToInt32(iditemamm));

            //    string Title = "";
            //    if (DsItem.Tables[0].Rows[0]["item_name"] != DBNull.Value) {
            //        Title = DsItem.Tables[0].Rows[0]["item_name"].ToString();
            //    }
            ContentServices contserv = new ContentServices();

            ContentDTO cont = new ContentDTO();
            cont = contserv.GetContentByID(Convert.ToInt32(idcont));

            string pathIdItem = Path.Combine(ConfigurationSettings.AppSettings["ServerPath"], idcont);
            string response = string.Empty;
            response = EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, cont.Title);

            response = "~/" + response.Replace("\\", "/");
            response = "~" + response.Substring(response.IndexOf("/Fileserver"));
            Response.Redirect(response);

            //    //Ricavo l'id Utente
            //    int iditemuser = 0;
            //    cms.CoreLib.Net1.Item NewItem = new cms.CoreLib.Net1.Item();
            //    OracleDataReader objReader = NewItem.GetItemContent(Convert.ToInt32(iditemamm));
            //    objReader.Read();
            //    if (objReader["corrispond_item_id"] != DBNull.Value) {
            //        iditemuser = Convert.ToInt32(objReader["corrispond_item_id"]);
            //        cms.CoreLib.Net1.Item NewItemUS = new cms.CoreLib.Net1.Item();
            //        , objReaderUS = NewItemUS.GetItemContent(iditemuser);
            //        objReaderUS.Read();
            //        if (objReaderUS["item_contents_id"] != DBNull.Value) {
            //            int CorrispondIDContentUS = Convert.ToInt32(objReaderUS["item_contents_id"]);
            //            NewItem.UpdContentStyle(CorrispondIDContentUS, 'F', "default.html");
            //        }
            //        objReaderUS.Close();
            //    }
            //    objReader.Close();
            //    string pathIdItemUser = @"contenuti\" + iditemuser;

            //    CopyDirectory(Path.Combine(fileserver, pathIdItem), Path.Combine(fileserver, pathIdItemUser), true);

            //    //MOTORE LUCENE 
            //    IndexItem(iditemuser);
            //    //EAI
            //    EAI.Services.CMSServices.UpdStdDoc(iditemuser);

            //    response = "~/" + @"contenuti/" + iditemuser + @"/default.html";

            //    Response.Redirect(response);
            //} else if (type == "com") {
            //    //Contenuti Commerciali

            //    //cms.Entities.Document doc = new cms.Entities.Document(Convert.ToInt32(iditemamm));
            //    //doc.Load();

            //    string Title = doc.Title.ToString();
            //    string pathIdItem = edsrvc.GetItemPath(iditemamm, type);
            //    string pathIdItemUser = "";
            //    pathIdItemUser = pathIdItem.Replace(@"\Admin", @"\");

            //    string response = string.Empty;

            //    response = EditorServices.PublicContent(Convert.ToInt32(idcont), pathIdItem, Title);

            //    CopyDirectory(Path.Combine(fileserver, pathIdItem), Path.Combine(fileserver, pathIdItemUser), true);

            //    //EAI
            //    EAI.Services.CMSServices.UpdCommDoc(Convert.ToInt32(iditemamm));

            //    pathIdItemUser = pathIdItemUser.Replace(@"\", "/");
            //    response = "~/" + pathIdItemUser + @"/default.html";

            //    Response.Redirect(response);
            //}
        }

        private void CopyDirectory(string sourcePath, string destPath, bool overwrite)
        {
            System.IO.DirectoryInfo sourceDir = new System.IO.DirectoryInfo(sourcePath);
            System.IO.DirectoryInfo destDir = new System.IO.DirectoryInfo(destPath);
            if ((sourceDir.Exists))
            {
                if (!(destDir.Exists))
                {
                    destDir.Create();
                }

                foreach (FileInfo file in sourceDir.GetFiles())
                {
                    if ((overwrite))
                    {
                        file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), true);
                    }
                    else
                    {
                        if (((System.IO.File.Exists(System.IO.Path.Combine(destDir.FullName, file.Name))) == false))
                        {
                            file.CopyTo(System.IO.Path.Combine(destDir.FullName, file.Name), false);
                        }
                    }
                }
                foreach (DirectoryInfo dir in sourceDir.GetDirectories())
                {
                    CopyDirectory(dir.FullName, System.IO.Path.Combine(destDir.FullName, dir.Name), overwrite);
                }
            }

        }

        //private void IndexItem(int IdItemUsr)
        //{

        //    cms.CoreLib.Net1.Item NewItem = new cms.CoreLib.Net1.Item();
        //    bool res = false;
        //    int IDus = IdItemUsr;

        //    string strServerPath = ConfigurationSettings.AppSettings["ServerPath"];
        //    cms.DatabaseGateway.TreeItemDataBase dbGate = new cms.DatabaseGateway.TreeItemDataBase();

        //    OracleDataReader useritem = NewItem.GetItemContent(IDus);
        //    useritem.Read();

        //    string strMethod = "EMPTY";
        //    string ContentType = useritem["content_type"].ToString();

        //    if (ContentType == "K")
        //    {
        //        strMethod = "EMPTY";
        //    }
        //    else
        //    {
        //        string ItemType = useritem["item_type"].ToString();
        //        if (ItemType == "filehtmlfolder")
        //        {
        //            strMethod = "MINISITE";
        //        }
        //        else
        //        {
        //            strMethod = "FILE";
        //        }
        //    }
        //    int ParentItemAdm = Convert.ToInt32(useritem["parent_item_id"]);
        //    int PositionItemAdm = Convert.ToInt32(useritem["item_posview"]);
        //    string Namefile = useritem["content_file"].ToString();
        //    int IDContent = Convert.ToInt32(useritem["item_contents_id"]);
        //    string title = useritem["content_title"].ToString();
        //    string key = "";
        //    if (useritem["keywords"] != DBNull.Value)
        //    {
        //        key = useritem["keywords"].ToString();
        //    }
        //    string dataDoc = "";

        //    if (useritem["content_date_update"] != DBNull.Value)
        //    {
        //        DateTime dt = default(DateTime);
        //        dt = Convert.ToDateTime(useritem["content_date_update"]);
        //        dataDoc = dt.ToString("yyyy-MM-dd HH:mm:ss");

        //    }
        //    string priority = useritem["priority"].ToString();
        //    string ContetType = useritem["content_type"].ToString();
        //    string Storico = useritem["ITEM_STORED"].ToString();

        //    res = cms.portal.search.FullTextSearcher.update(IDus.ToString(), title, key, strServerPath + "contenuti\\" + IDus + "\\" + Namefile, strMethod, Storico, "Contenuti Standard", dbGate.GetPathById(IDus), dataDoc, priority.ToString(),
        //    ContetType);
        //    useritem.Close();
        //}

    }
}

