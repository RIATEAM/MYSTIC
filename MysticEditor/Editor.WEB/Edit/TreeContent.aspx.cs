using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BE;
using Editor.BE.Model;
using Editor.BL;
using NHibernate;

namespace Editor.Web.Edit {
    public partial class TreeContent : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            if (!Page.IsPostBack) {
                using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                    ITransaction transaction = session.BeginTransaction();
                    try {
                        List<Editor.BE.Model.Content> contList = new List<Editor.BE.Model.Content>();
                        contList = EditorServices.GetContents<Editor.BE.Model.Content>(session);

                        foreach (Editor.BE.Model.Content cnt in contList) {
                            TreeNode nodeContent = new TreeNode(cnt.Title, cnt.Contentid.ToString() + "|" + "0", "~/img/cont.gif");
                            nodeContent.Target = "mainFrame";
                            nodeContent.NavigateUrl = "EditContent.aspx?id="+cnt.Contentid;
                            //1 liv
                            var fist = (from f in cnt.Pages
                                        where f.Pageid == f.Parentpageid
                                        orderby f.Position ascending
                                        select f);
                            foreach (Editor.BE.Model.Page pg in fist) {
                                TreeNode nodePage = new TreeNode(pg.Publictitle, cnt.Contentid.ToString() + "|" + pg.Pageid.ToString(), "~/img/cild.gif");
                                nodePage.Target = "mainFrame";
                                nodePage.NavigateUrl = "EditPage.aspx?id="+pg.Pageid;
                                nodeContent.ChildNodes.Add(nodePage);
                            }
                            //2 liv
                            var sec = (from f in cnt.Pages
                                       join h in fist on f.Parentpageid equals h.Pageid
                                       where f.Pageid != f.Parentpageid 
                                       orderby f.Position ascending
                                       select f);
                            foreach (Editor.BE.Model.Page pg in sec) {
                                TreeNode nodePage = new TreeNode(pg.Publictitle, cnt.Contentid.ToString() + "|" + pg.Pageid.ToString(), "~/img/cild.gif");
                                foreach (TreeNode temp in nodeContent.ChildNodes) {
                                    if (temp.Value == cnt.Contentid.ToString() + "|" + pg.Parentpageid.ToString()) {
                                        nodePage.Target = "mainFrame";
                                        nodePage.NavigateUrl = "EditPage.aspx?id="+pg.Pageid;
                                        nodeContent.ChildNodes[nodeContent.ChildNodes.IndexOf(temp)].ChildNodes.Add(nodePage);
                                    }
                                }
                            }
                            nodeContent.CollapseAll();
                            TreeView1.Nodes[0].ChildNodes.Add(nodeContent);
                        }
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

        protected void TreeView1_TreeNodeExpanded(object sender, TreeNodeEventArgs e) {
            if (Page.IsPostBack) {
                if (e.Node.Value.Length > 0) {
                    string[] info = e.Node.Value.Split('|');
                    int contentId = Convert.ToInt32(info[0]);
                    int pageId = Convert.ToInt32(info[1]);
                    if (pageId != 0) {
                        using (ISession session = HibernateHelper.GetSession().OpenSession()) {
                            ITransaction transaction = session.BeginTransaction();
                            try {
                                foreach (TreeNode nodo in e.Node.ChildNodes) {
                                    string[] tmp = nodo.Value.Split('|');
                                    int tmpCont = Convert.ToInt32(tmp[0]);
                                    int tmpPage = Convert.ToInt32(tmp[1]);


                                    List<Editor.BE.Model.Page> pgList = new List<Editor.BE.Model.Page>();
                                    pgList = EditorServices.GetPageByParent(session, tmpCont, tmpPage);
                                    foreach (Editor.BE.Model.Page pg in pgList) {
                                        TreeNode nodePage = new TreeNode(pg.Publictitle, tmpCont.ToString() + "|" + pg.Pageid.ToString(), "~/img/cild.gif");
                                        nodePage.Target = "mainFrame";
                                        nodePage.NavigateUrl = "EditPage.aspx?id=" + pg.Pageid;
                                        nodo.ChildNodes.Add(nodePage);
                                    }
                                    nodo.CollapseAll();
                                }


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
        }

    }
}
