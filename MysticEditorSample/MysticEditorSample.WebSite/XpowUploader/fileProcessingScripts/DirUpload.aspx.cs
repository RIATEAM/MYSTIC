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

namespace MysticEditorSample.WebSite.XpowUploader.fileProcessingScripts
{
    public partial class DirUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sessionID = string.Empty;
            try
            {

                //sessionID = Request.QueryString["iduser"];

                //if (string.IsNullOrEmpty(Session["id_user"].ToString()))
                //    throw new Exception("SessionID non valorizzata");

                string FolderToSave = Server.MapPath("") + "\\UploadedFiles\\";
                //string FolderToSave = ConfigurationSettings.AppSettings["UpdTmpPath"] + "upd" + Session["id_user"] + "\\";

                string clientRelativePath;
                string nameToSave;
                int Uploaded = 0;
                string root = string.Empty;
                string SelectedPath = string.Empty;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile myFile = Request.Files[i];

                    SelectedPath = Request.Form["SelectedPath_" + i];

                    root = SelectedPath.Split("\\".ToCharArray()).GetValue(0).ToString() + "\\";

                    clientRelativePath = SelectedPath.Substring(root.Length);

                    if (clientRelativePath == null || clientRelativePath.IndexOf("\\") == -1) //If SelectedPath havn't '\' symbol it is filename
                        clientRelativePath = "";
                    nameToSave = GetWinSafeFileName(myFile.FileName);

                    if (nameToSave != "")
                    {
                        //checkbox x UploadStructure sempre true
                        if (Request.Form["UploadStructure"] == "yes")
                        {
                            //Create and Save whole folders structure							
                            SaveToFolder(myFile, FolderToSave, nameToSave, clientRelativePath);
                            Response.Write("File " + nameToSave + " succesfully saved.<br>");
                        }
                        else
                        {
                            //Save all files to single root folder
                            myFile.SaveAs(FolderToSave + nameToSave);
                            Response.Write("File " + nameToSave + " succesfully saved.<br>");

                        }

                        Uploaded++;
                    }

                }
                if (Uploaded == 0)
                    Response.Write("No files sent!");

                // getFeedback(Uploaded);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void getFeedback(int uploaded)
        //{
        //switch (uploaded)
        //{
        //    case 0:
        //        {
        //            Response.Write("Nessun file salvato.");
        //            break;
        //        }
        //    case Request.Files.Count:
        //        {
        //            Response.Write("Tutti i files salvati.");
        //            break;
        //        }
        //    default:
        //        {
        //            Response.Write("Si è verificato un errore durante il salvataggio.");
        //            break;
        //        }
        //}


        //}

        /*Creates folders.*/
        private void AddPath(string PathToCreate)
        {
            int iBreak;
            iBreak = PathToCreate.LastIndexOf("\\");
            if (iBreak != -1)
            {
                string Parent = PathToCreate.Substring(0, iBreak);
                if (!System.IO.Directory.Exists(Parent))
                {
                    AddPath(Parent);
                }
            }

            if (!System.IO.Directory.Exists(PathToCreate))
            {
                System.IO.Directory.CreateDirectory(PathToCreate);
            }
        }


        private string GetWinSafeFileName(string strPath)
        {
            return GetWinSafeName(strPath, false);
        }

        private string GetWinSafePath(string strPath)
        {
            return GetWinSafeName(strPath, true);
        }

        /*
         Returns the safe file name and extension or safe path of the specified path string.
        All characters that are illegal in file names or paths on Windows 
        and not safe relative path substrings like "?", "..\" are deleted.
        */
        private string GetWinSafeName(string strPath, bool isPath)
        {
            string safeName = "";
            if (!isPath)
            {
                int slashind = strPath.LastIndexOf("\\");
                int backslashind = strPath.LastIndexOf("/");
                if (slashind == -1 && backslashind == -1)
                    safeName = strPath;
                else if (slashind > backslashind)
                    safeName = strPath.Substring(slashind + 1, strPath.Length - slashind - 1);
                else
                    safeName = strPath.Substring(backslashind + 1, strPath.Length - backslashind - 1);
            }
            else
                safeName = strPath.Replace("/", "\\");

            int i, charpos;
            char[] mywrongchars = new char[] { '?', '*', '/', ':' };
            char[] systemwrongchars = System.IO.Path.InvalidPathChars;
            char[] wrongchars = new char[systemwrongchars.Length + mywrongchars.Length];

            mywrongchars.CopyTo(wrongchars, 0);
            systemwrongchars.CopyTo(wrongchars, mywrongchars.Length);

            for (i = 0; i <= wrongchars.Length - 1; i++)
            {
                do
                {
                    charpos = safeName.IndexOf(wrongchars[i]);
                    if (charpos != -1)
                        safeName = safeName.Remove(charpos, 1);
                } while (charpos != -1);
            }
            if (!isPath)
            {
                if (safeName.Length > 255)
                    safeName = safeName.Substring(safeName.Length - 255, 255);
            }
            //Replace dangerous ..\ at the begin or ..\ at the end or \..\ at the any place
            //of folder path.
            if (isPath)
            {
                while (safeName.IndexOf("\\..\\") != -1)
                    safeName = safeName.Replace("\\..\\", "\\pp\\");
                if (safeName.EndsWith("\\.."))
                    safeName = safeName.Substring(0, safeName.Length - 2) + "pp";
                if (safeName.StartsWith("..\\"))
                    safeName = "pp" + safeName.Substring(2, safeName.Length - 2);
            }
            return safeName;
        }

        private void SaveToFolder(HttpPostedFile myFile, string FolderToSave, string nameToSave, string clientRelativePath)
        {
            string serverRelativePath;
            string tmpStr;
            /*Replace "/" with the "\" symbols
              Cut filename and "\" symbols at the begin and end of path.
              So path should be in format "folder1\folder2\folder3"
            */
            tmpStr = clientRelativePath.Replace("/", "\\");
            if (tmpStr.IndexOf("\\") != -1)
                tmpStr = tmpStr.Substring(0, tmpStr.LastIndexOf("\\"));
            if (tmpStr.StartsWith("\\"))
                tmpStr = tmpStr.Substring(1, tmpStr.Length - 1);
            serverRelativePath = GetWinSafePath(tmpStr);
            if (nameToSave != "")
            {
                AddPath(FolderToSave + serverRelativePath);
                myFile.SaveAs(FolderToSave + serverRelativePath + "\\" + nameToSave);
            }
        }


    }

}
