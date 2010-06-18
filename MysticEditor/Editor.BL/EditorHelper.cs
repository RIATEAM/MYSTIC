using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace Editor.BL {
    public partial class EditorServices {

        private static XmlDocument CreateXmlToDataSet(DataTable dt) {
            DataSet set = new DataSet();
            set.DataSetName = "Menu";

            dt.TableName = "Item";

            dt.Columns.Add("Href");
            dt.Columns.Add("Target");
            dt.Columns.Remove("Content");
            dt.Columns.Remove("Skin");
            dt.Columns.Remove("PageElements");
            dt.Columns.Remove("PageelementsList");
            dt.Columns.Remove("Contentid");
            dt.Columns.Remove("Position");
            dt.Columns.Remove("Level");
            dt.Columns.Remove("Skinid");
            dt.Columns.Remove("IsNew");
            dt.Columns.Remove("Dirty");
            dt.Columns.Remove("Deleted");
            dt.Columns.Remove("IsPersisted");
            dt.Columns.Remove("HasChanged");
            dt.Columns.Remove("Structureid");

            dt.Columns["Pageid"].ColumnMapping = MappingType.Attribute;
            dt.Columns["Parentpageid"].ColumnMapping = MappingType.Hidden;
            dt.Columns["Title"].ColumnMapping = MappingType.Hidden;
            dt.Columns["Publictitle"].ColumnMapping = MappingType.Attribute;
            dt.Columns["Href"].ColumnMapping = MappingType.Attribute;
            dt.Columns["Target"].ColumnMapping = MappingType.Attribute;
            dt.Columns["State"].ColumnMapping = MappingType.Attribute;


            dt.Columns["Pageid"].ColumnName = "Id";
            dt.Columns["Publictitle"].ColumnName = "Titolo";

            set.Tables.Add(dt);
            DataRelation relation = new DataRelation("ParentChild",
                            set.Tables["Item"].Columns["Id"],
                            set.Tables["Item"].Columns["Parentpageid"],
                            true);

            relation.Nested = true;
            set.Relations.Add(relation);

            DataRow Cestino = dt.Rows[0];
            foreach (DataRow dr in dt.Rows) {
                if (Convert.ToInt32(dr["State"]) == 99) {
                    Cestino = dr;
                }
            }

            if (Convert.ToInt32(Cestino["State"]) == 99) {
                set.Tables["Item"].Rows.Remove(Cestino);
            }

            foreach (DataRow dr in dt.Rows) {
                if (Convert.ToInt32(dr["Id"]) == Convert.ToInt32(dr["Parentpageid"])) {
                    dr["Parentpageid"] = DBNull.Value;

                    dr["Href"] = dr["Title"] + ".html";

                } else {
                    dr["Href"] = dr["Id"] + "_" + dr["Title"] + ".html";
                }
                dr["Target"] = "mainframe";
            }


            XmlDocument docXml = new XmlDocument();
            docXml.AppendChild(docXml.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            docXml.InnerXml += set.GetXml();

            return docXml;
        }


        private static XmlDocument CreateXmlToDataSetWidget(DataTable dt) {

            throw new NotImplementedException();
        
        }
        
        
        public static DataTable ToDataTable<T>(IList<T> items) {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props) {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items) {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++) {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);

            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t) {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t) {
            if (t != null && IsNullable(t)) {
                if (!t.IsValueType) {
                    return t;
                } else {
                    return Nullable.GetUnderlyingType(t);
                }
            } else {
                return t;
            }
        }

        /// <summary>
        /// Method for copying all the filesin a given directory
        /// </summary>
        /// <param name="origDir">Directory the files are in</param>
        /// <param name="destDir">Directory the files are being copied to</param>        
        private static void Copy(string sourceDirectory, string targetDirectory) {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false) {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles()) {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        public static void ZipFiles(string inputFolderPath, string outputPathAndFile, string password) {
            ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
            int TrimLength = inputFolderPath.Length;
            // find number of chars to remove     // from orginal file path
            TrimLength += 1; //remove '\'
            FileStream ostream = null;
            byte[] obuffer;
            string outPath = outputPathAndFile;
            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath)); // create zip stream
            if (password != null && password != String.Empty)
                oZipStream.Password = password;
            oZipStream.SetLevel(9); // maximum compression
            ZipEntry oZipEntry;
            foreach (string Fil in ar) // for each file, generate a zipentry
            {
                oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                oZipStream.PutNextEntry(oZipEntry);

                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    ostream = File.OpenRead(Fil);
                    obuffer = new byte[ostream.Length];
                    ostream.Read(obuffer, 0, obuffer.Length);
                    oZipStream.Write(obuffer, 0, obuffer.Length);
                    ostream.Close();
                }
            }
            oZipStream.Finish();
            oZipStream.Close();
        }


        private static ArrayList GenerateFileList(string Dir) {
            ArrayList fils = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                fils.Add(file);
                Empty = false;
            }

            if (Empty) {
                if (Directory.GetDirectories(Dir).Length == 0)
                // if directory is completely empty, add it
                {
                    fils.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs)) {
                    fils.Add(obj);
                }
            }
            return fils; // return file list
        }


        public static void UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile) {
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipPathAndFile));
            if (password != null && password != String.Empty)
                s.Password = password;
            ZipEntry theEntry;
            string tmpEntry = String.Empty;
            while ((theEntry = s.GetNextEntry()) != null) {
                string directoryName = outputFolder;
                string fileName = Path.GetFileName(theEntry.Name);
                // create directory 
                if (directoryName != "") {
                    Directory.CreateDirectory(directoryName);
                }
                if (fileName != String.Empty) {
                    if (theEntry.Name.IndexOf(".ini") < 0) {
                        string fullPath = directoryName + "\\" + theEntry.Name;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                        FileStream streamWriter = File.Create(fullPath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true) {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0) {
                                streamWriter.Write(data, 0, size);
                            } else {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
            }
            s.Close();
            if (deleteZipFile)
                File.Delete(zipPathAndFile);
        }



    }

}
