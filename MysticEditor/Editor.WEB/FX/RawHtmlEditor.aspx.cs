using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Editor.BE.Model;
using Editor.BL;
using Editor.Services;
using Editor.DTO;
using Syrinx.Gui.AspNet;

public partial class cms_Editor_Fx_PageElement_editpageelement : System.Web.UI.Page {
    private static int RawHtmlId = 0;
    private static PageServices pgserv = new PageServices();
    private static int iditemamm = 0;
    protected void Page_Load(object sender, EventArgs e) {

        if (!Page.IsPostBack) {
            string idPageElement = Request.QueryString["idpel"];
            iditemamm = Convert.ToInt32(Request.QueryString["iditem"]);
            PageServices pgserv = new PageServices();

            PageElementDTO pel = new PageElementDTO();
            pel = pgserv.GetPageelementByPageelementID(Convert.ToInt32(idPageElement));

            RawHtml row = new RawHtml();
            row = pgserv.GetRawHtmlById(pel.Rawhtmlid);
            edit.Text = row.Value;
            RawHtmlId = row.Rawhtmlid;
        }

    }
    protected void Save_Click(object sender, EventArgs e) {

        RawHtml row = new RawHtml();
        row = pgserv.GetRawHtmlById(RawHtmlId);

        row.Value = edit.Text;
        row.Dirty = true;
        pgserv.SaveRawHtml(row, iditemamm);

    }
}
