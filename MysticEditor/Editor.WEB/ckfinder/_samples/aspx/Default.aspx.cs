using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace Editor {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            if (!Page.IsPostBack) {
                StreamReader read = new StreamReader(@"D:\2010\CKEditor\SyrinxCkEditor\SyrinxCkEditor\DemoSite\HTMLPage1.htm");
                ed1.Text = read.ReadToEnd().ToString();
                read.Close();
            } else {
                
                
                
                string Body = ed1.Text;
                StringBuilder html = new StringBuilder();
                StreamReader str = new StreamReader(@"D:\2010\CKEditor\SyrinxCkEditor\SyrinxCkEditor\DemoSite\HTMLPage1.htm");
                string line;
                while (str.Peek() > -1) {
                    line = str.ReadLine();
                    html.AppendLine(line);
                    if (line.Contains("<body")) {
                        html.AppendLine(Body);
                        while (str.Peek() > -1) {
                            line = str.ReadLine();
                            if (line.Contains("</body>")) {
                                html.AppendLine(line);
                                while (str.Peek() > -1) {
                                    html.AppendLine(str.ReadLine());
                                }
                            }
                        }
                    }
                }
                str.Close();

                StreamWriter strWr = new StreamWriter(@"D:\2010\CKEditor\SyrinxCkEditor\SyrinxCkEditor\DemoSite\HTMLPage1.htm", false);
                strWr.Write(html);
                strWr.Close();

            }
        }


    }
}
