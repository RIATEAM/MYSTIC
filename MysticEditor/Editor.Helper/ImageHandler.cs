using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.IO;
namespace Editor.Helper 
{

public class ImageHandler : IHttpHandler
{
	private Bitmap _bmp;
	private string _contentType;

 public ImageHandler(){}

	public ImageHandler(Bitmap bmp, string contentType)
	{
		this._bmp=bmp;
		this._contentType=contentType;
	}

	public bool IsReusable
	{
		get
		{ return true; }
	}
    public void ProcessRequest(HttpContext context)

      {
        context.Response.ContentType =this._contentType ;
					   ImageFormat fmt=ImageFormat.Jpeg ;
					   if(this._contentType=="Image/Gif")fmt=ImageFormat.Gif;
                      if (this._bmp !=null && this._bmp.Height!=0)
					   this._bmp.Save(context.Response.OutputStream,fmt);
					   context.Response.End() ;
      }
    }
}

