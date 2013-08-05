using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iportogruaroLibraryShared;
using MonoTouch.CoreAnimation;
using MonoTouch.Social;
using MonoTouch.MessageUI;
using MonoTouch.CoreGraphics;
using System.Threading;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using System.Collections.Generic;
using System.Globalization;

namespace iportogruaroIOS
{
	public partial class UIEventDetail : baseView
	{
		public class browserDelegateevent : UIWebViewDelegate {

			private UIEventDetail _vc;
			public browserDelegateevent (UIEventDetail controller):base()
			{
				_vc = controller;
			}

			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				// did the user click on a link?
				if ((navigationType == UIWebViewNavigationType.LinkClicked) || (navigationType == UIWebViewNavigationType.BackForward)) {
					NSUrl url = request.Url;
					// is it an http link?

					if (!_vc.loadingHtml) {

						//url.AbsoluteString
						_vc.NavigationController.PushViewController (new UiWebView(url.AbsoluteString), true);
						return false;
					}
					else
					{
						if (url.Scheme == "http"){
							// update the addressbar with the new link
							//  _vc.addressBar.Text = url.AbsoluteString;   
						}
					}
					return true;
				}
				return true;
			}


			public override void LoadStarted (UIWebView webView)
			{
				// let the user know we are working on their request
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
				_vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem (indicator);
				indicator.StartAnimating ();
			}

			public override void LoadingFinished (UIWebView webView)
			{

				// let the user know that we are done with their request
				_vc.NavigationItem.RightBarButtonItem = null;
				//_vc.btnBackWebview = webView.CanGoBack;
				// set the enabled state for the forward and back buttons


			}


		}
		#region cicle
		public UIEventDetail () : base (UserInterfaceIdiomIsPhone ? "UIEventDetail" : "UiIpadDetailScreen", null)
		{

		}

		public UIEventDetail (bool _search) : base (UserInterfaceIdiomIsPhone ? "UIEventDetail" : "UiIpadDetailScreen", null)
		{
			search = true;
		}
		public bool loadingHtml{ get; set;}
		public bool search{ get; set;}
		public string key { get; set;}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		void setstyle ()
		{



			lblLogintitle = new UILabel () {
				Font = UIFont.FromName("HelveticaNeue-Bold", 20f),
				TextColor = UIColor.FromRGB (152, 22, 22),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth
			};
			if (!UserInterfaceIdiomIsPhone)
				lblLogintitle.TextAlignment = UITextAlignment.Center;

			lblLogintitle.Frame = new System.Drawing.RectangleF (5, 10, vi.Frame.Width - 10f, 45);

			lblLogintitle.Text = System.Web.HttpUtility.HtmlDecode (dataPos.title);


			this.Title = lblLogintitle.Text;
			vi.AddSubview (lblLogintitle);


			/*
            lblLoginDes = new UITextView () {
                Font = UIFont.FromName("HelveticaNeue-Bold", 18f),
                TextColor = UIColor.FromRGB (174,65,61),
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear,
                ScrollEnabled = true,
                Editable = false,
                ShowsVerticalScrollIndicator = true,

                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
            };

            if (UserInterfaceIdiomIsPhone) {
                lblLogintitle.Font = UIFont.FromName ("HelveticaNeue-Bold", 14f);
                lblLoginDes.Font = UIFont.FromName ("HelveticaNeue-Bold", 12f);
            }
            if (!UserInterfaceIdiomIsPhone)
                lblLoginDes.TextAlignment = UITextAlignment.Left;
            if (UserInterfaceIdiomIsPhone)
                lblLoginDes.Frame = new System.Drawing.RectangleF (5, 190, vi.Frame.Width - 10, 200);
            else
                lblLoginDes.Frame = new System.Drawing.RectangleF (10, 465, vi.Frame.Width - 20, 425);

            if (dataPos.des.Length > 0)
                lblLoginDes.Text = System.Web.HttpUtility.HtmlDecode (dataPos.des);
            else
                lblLoginDes.Text = System.Web.HttpUtility.HtmlDecode (dataPos.title);
*/

            if (UserInterfaceIdiomIsPhone)
                lblLoginDes = new MonoTouch.UIKit.UIWebView (new System.Drawing.RectangleF (5, 45, vi.Frame.Width - 10, 300));
            else
                lblLoginDes = new MonoTouch.UIKit.UIWebView ( new System.Drawing.RectangleF (10, 45, vi.Frame.Width - 20, 425));

            lblLoginDes.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            string html = "";
            if (UserInterfaceIdiomIsPhone)
                html = "<style type='text/css'>h1{font-family: Helvetica Neue;font-size:16;color:#000000;}"
                    + "body{font-family:Helvetica Neue;font-size:14;color:rgb(2,65,61);background-color: transparent;}"
                    + "</style>";
            else
                html = "<style type='text/css'>h1{font-family: Helvetica Neue;font-size:18;color:#000000;}"
                    + "body{font-family:Helvetica Neue;font-size:14;color:rgb(174,65,61);background-color: transparent;}"
                    + "</style>";
            if (dataPos.des.Length > 0)
                html  += System.Web.HttpUtility.HtmlDecode (dataPos.des);
            else
                html  += System.Web.HttpUtility.HtmlDecode (dataPos.title);
            loadingHtml = true;
            lblLoginDes.LoadHtmlString (html, null);
            lblLoginDes.BackgroundColor = UIColor.White;
            lblLoginDes.Layer.BorderColor = UIColor.White.CGColor;
            lblLoginDes.Opaque = false;
           

            lblLoginDes.Delegate = new browserDelegateevent (this);

            try{
                foreach(UIView vmain in lblLoginDes.Subviews)
                {
                    if (vmain is UIImageView)
                    {
                        vmain.RemoveFromSuperview();
                    }

                    if (vmain is UIScrollView)
                    {
                        UIScrollView svmain = vmain as UIScrollView;
                        svmain.ShowsVerticalScrollIndicator = false;
                        svmain.ShowsHorizontalScrollIndicator = false;
                        foreach(UIView shadowview in svmain.Subviews)
                        {
                            if (shadowview is UIImageView)
                                shadowview.Hidden = true;
                        }
                    }
                }
            }
            catch{
            }

            vi.AddSubview (lblLoginDes);

            loadingHtml = false;

        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            if (UserInterfaceIdiomIsPhone && IsTall) {
                View.Frame = new RectangleF (0, -50, 480, 520);
            } else if (!UserInterfaceIdiomIsPhone) {
                this.View.Frame = new RectangleF (0, 0, 768, 1024);
            }


            vi = new UIView (new RectangleF (10, 5, this.View.Bounds.Width - 20f, View.Bounds.Height - 35f));
            if (!UserInterfaceIdiomIsPhone) {
                vi.Frame = new RectangleF (10, 5, this.View.Bounds.Width - 20f, View.Bounds.Height - 125f);
            }
            //vi.AutosizesSubviews = true;
            vi.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;



            vi.BackgroundColor = UIColor.FromRGB (255, 255, 255);  
            vi.Layer.MasksToBounds = false;
            vi.Layer.CornerRadius = 7;
            this.View.BackgroundColor = UIColor.FromRGB (255, 255, 255);
            this.View.BackgroundColor = UIColor.FromRGB (255, 255, 255);
            this.View.InsertSubview (vi, 0); 
           
            if (search) {
                NSUserDefaults.StandardUserDefaults.SetBool(true,"refreshList");
                InvokeOnMainThread (delegate {
                    starthud ();
                }
                );


                ThreadPool.QueueUserWorkItem (state => {
                   
                   
                    var lst = new iportogruaroLibraryShared.mainEventos().geteventsbyName(key);
                    if (lst != null )
                    {

                        dataPos = lst;

                        InvokeOnMainThread (delegate {
                            setstyle ();
                            stophud();
                        }
                        );
                    }

                });

                stophud();
            } else {
                setstyle ();
                      

               
            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

     
        public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate (toInterfaceOrientation, duration);


            if (UserInterfaceIdiomIsPhone) {
                          


                //myinfo.View .Frame = new RectangleF (s.Frame.X,s.Frame.Y + 15,s.Frame.Width,s.Frame.Height -25);

            } else {
                switch (toInterfaceOrientation) {
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:


                    View.Frame = new RectangleF (0, 240, 1024, 768);
                    vi.Frame = new RectangleF (10, 20, 1004, 700);




                    break;
                default:
                    {
                        View.Frame = new RectangleF (0, -230, 768, 1024);
                        vi.Frame = new RectangleF (10, 10, 748, 1024);



                    }

                    // myMap.View.Frame = s.Bounds;
                    // myGal.View.Frame = s.Bounds;
                    // myinfo.View .Frame = s.Bounds;


                    break;
                }
            }
                        
            if (UserInterfaceIdiomIsPhone) {
                            
            } else {
                            

            }
                        
        }
        #endregion
        private UIView vi;
        private UILabel lblLogintitle;
        private MonoTouch.UIKit.UIWebView lblLoginDes;

        public iportogruaroLibraryShared.eventos dataPos{ get; set; }
    }
}

