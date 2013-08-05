
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iportogruaroIOS
{
    public partial class UiWebView : baseView
    {
        public UiWebView (string _url) : base ("UiWebView", null)
        {
            urlStr = System.Web.HttpUtility.UrlDecode(_url);
        }
        public string urlStr{get;set;}
        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
            
            // Release any cached data, images, etc that aren't in use.
        }
        public bool btnBackWebview{
			get{
				return btnitemBack.Enabled;
			}
			set{
				btnitemBack.Enabled = value;
			}

		}
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            if (UserInterfaceIdiomIsPhone && IsTall )
            {
                Console.WriteLine("... is iphone 5 ....");
                this.View.Frame = new RectangleF (this.View.Frame.X, this.View.Frame.Y, this.View.Frame.Width, 511);
                this.webview.Frame = new RectangleF (this.View.Frame.X, this.View.Frame.Y , this.View.Frame.Width, 511+5);
            }
            

			this.barWebView.TintColor = UIColor.FromRGB (152, 22, 22);



            //myBar.Style =  UIBarButtonItemStyle.  // UIBarStyle.Black;
           
            webview.Delegate = new browserDelegate (this);
			Console.WriteLine (urlStr);
            NSUrl url = new NSUrl( urlStr);
            NSUrlRequest request = new NSUrlRequest (url);
            
            // tell the UIWebView to display our request
            webview.LoadRequest (request);
            

			btnitemBack.Enabled = webview.CanGoBack;
			btnitemBack.Clicked += delegate {
				webview.GoBack();
		};
			btnitemSafari.Clicked += delegate {
				NSUrl urlWeb = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(urlWeb)) {

				}
		};

            // initialize the forward and back buttons

            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
     
}

