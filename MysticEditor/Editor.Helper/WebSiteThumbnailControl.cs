using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;

namespace Editor.Helper {
    public enum ImageType {
        Gif,
        Jpeg
    }

    public enum Persistence {
        Cache,
        Session
    }

    [Designer("PAB.WebControls.WebSiteThumbnailControlDesigner"), ToolboxDataAttribute("<{0}:WebSiteThumbnailControl Runat=\"server\"></{0}:WebSiteThumbnailControl>")]
    public class WebSiteThumbnailControl : Control {
        protected string ImageUrl;
        private ImageType imageType;
        [Description("Image Type")]
        [Category("Data")]
        [DefaultValue("Gif")]
        [Browsable(true)]
        public ImageType ImageType {
            get {
                return imageType;
            }

            set {
                imageType = value;
            }
        }


        private int _siteHeight = 600;
        [Description("Site Browser Height")]
        [Category("Misc")]
        [Browsable(true)]
        [DefaultValue(600)]
        public int SiteHeight {
            get { return _siteHeight; }
            set { _siteHeight = value; }
        }



        private int _siteWidth = 800;

        [Description("Site Browser Width")]
        [Category("Misc")]
        [Browsable(true)]
        [DefaultValue(800)]
        public int SiteWidth {
            get { return _siteWidth; }
            set { _siteWidth = value; }
        }


        private int _thumbnailHeight = 200;

        [Description("Thumbnail Height")]
        [Category("Misc")]
        [Browsable(true)]
        [DefaultValue(267)]
        public int ThumbnailHeight {
            get { return _thumbnailHeight; }
            set { _thumbnailHeight = value; }
        }



        private int _thumbnailWidth = 267;
        [Description("Thumbnail Width")]
        [Category("Misc")]
        [Browsable(true)]
        [DefaultValue(200)]
        public int ThumbnailWidth {
            get { return _thumbnailWidth; }
            set { _thumbnailWidth = value; }
        }


        private Persistence persistenceType;

        [Description("Cache or Session Persistence")]
        [Category("Data")]
        [Browsable(true)]
        public Persistence PersistenceType {
            get {
                return persistenceType;
            }

            set {
                if (value == Persistence.Session) HttpContext.Current.Session["persistenceType"] = "Session";
                persistenceType = value;
            }
        }

        private string _siteUrl;

        [Description("Site Url")]
        [Category("Misc")]
        [Browsable(true)]
        public string SiteUrl {
            get {
                return _siteUrl;

            }
            set {
                _siteUrl = value;

            }

        }

        private Bitmap _bitmap;
        [Browsable(false)]
        public Bitmap Bitmap {
            get {
                if (HttpContext.Current.Session["persistenceType"] != null) this.persistenceType = Persistence.Session;
                if (this.PersistenceType == Persistence.Session)
                    return (Bitmap)Context.Session[String.Concat(CreateUniqueIDString(), "Bitmap")];
                else
                    return (Bitmap)Context.Cache[String.Concat(CreateUniqueIDString(), "Bitmap")];


            }

            set {
                if (this.PersistenceType == Persistence.Session)
                    Context.Session[String.Concat(CreateUniqueIDString(), "Bitmap")] = value;
                else
                    Context.Cache[String.Concat(CreateUniqueIDString(), "Bitmap")] = value;

            }
        }

        private string CreateUniqueIDString() {
            string idStr = String.Empty;
            string tmpId = String.Empty;
            if (this.PersistenceType == Persistence.Session) {
                idStr = "__" + Context.Session.SessionID.ToString() + "_";
            } else {
                if (Context.Cache["idStr"] == null) {
                    Context.Cache["idStr"] = Guid.NewGuid().ToString();
                }

                idStr = "__" + Context.Cache["idStr"].ToString() + "_";
            }

            idStr = String.Concat(idStr, UniqueID);
            idStr = String.Concat(idStr, "_");
            idStr = String.Concat(idStr, Page.ToString());
            idStr = String.Concat(idStr, "_");
            return idStr;
        }

        private void WebSiteThumbnailControl_Init(EventArgs e) {
            HttpRequest httpRequest = Context.Request;
            HttpResponse httpResponse = Context.Response;
            if (this.Bitmap == null) {
                Editor.Helper.WebSiteThumbnail sh = new WebSiteThumbnail(this._siteUrl, this._siteWidth, this._siteHeight, this._thumbnailWidth, this._thumbnailHeight);
                this.Bitmap = sh.GetScreenShot();

            }
            if (httpRequest.Params[String.Concat("WebSiteThumbnailControl_", UniqueID)] != null) {
                httpResponse.Clear();
                if (this.ImageType == ImageType.Gif) {
                    httpResponse.ContentType = "Image/Gif";
                    //Bitmap.Save(httpResponse.OutputStream,ImageFormat.Gif );
                    // (ImageHandler class is an experiment based on some comments)
                    ImageHandler handler = new ImageHandler(Bitmap, "Image/Gif");
                    handler.ProcessRequest(HttpContext.Current);

                } else {
                    httpResponse.ContentType = "Image/Jpeg";
                    //Bitmap.Save(httpResponse.OutputStream, ImageFormat.Jpeg);
                    ImageHandler handler = new ImageHandler(Bitmap, "Image/Jpeg");
                    handler.ProcessRequest(HttpContext.Current);
                }
                // httpResponse.End();
            }
            string str = httpRequest.Url.ToString();
            if (str.IndexOf("?") == -1) {
                ImageUrl = String.Concat(str, "?WebSiteThumbnailControl_", UniqueID, "=1");
            } else {
                ImageUrl = String.Concat(str, "&WebSiteThumbnailControl_", UniqueID, "=1");
            }
        }

        protected override void OnInit(EventArgs e) {
            WebSiteThumbnailControl_Init(e);
        }

        protected override void Render(HtmlTextWriter output) {
            output.Write("<img id={0} src={1}>", this.UniqueID, ImageUrl);
        }
    }

   //public class WebSiteThumbnailControlDesigner : System.Web.UI.Design.ControlDesigner {
   //     public WebSiteThumbnailControlDesigner() { }
   //     public override string GetDesignTimeHtml() {

   //         return GetEmptyDesignTimeHtml();
   //     }

   //     protected override string GetEmptyDesignTimeHtml() {
   //         return CreatePlaceHolderDesignTimeHtml("<div>[Image is set at runtime. Place control inside <BR>Table TD or DIV for absolute positioning.]</div>");
   //     }
   // }
}