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
using MapWithRoutes;

namespace iportogruaroIOS
{
	public partial class UiDetailScreen : baseView
	{
		#region propierties
		public int UpdatecurrenShareOpt{ get; set; }

		public int positionPos { get; set; }

		public UiCategoryListController controllerPast { get; set; }

		public  UiPosListController controllerpastPos{ get; set; }

		public  UiPosList controllerpastPosNew{ get; set; }

		public iportogruaropos dataPos{ get; set; }
		#endregion
		#region cicle
		public UiDetailScreen ()
			: base (UserInterfaceIdiomIsPhone ? "UiDetailScreen" : "UiIpadDetailScreen", null)
		{
			// Custom initialization
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			GC.Collect ();

			//imageView.Image.Dispose ();
			//imageView.Image = null;
			//imageView.RemoveFromSuperview ();
			try {
				imageView.Dispose ();
			} catch {
			}

			//imageView = null;
			// Release any cached data, images, etc that aren't in use.
		}

		private bool dontHadPhoto = false;
		private UIButton btnmoreInfo;
		private UIButton btnshowMap;
		private UIButton btnshowGal;
		CAGradientLayer btnoGradient;
		CAGradientLayer btnoGradientMap;
		CAGradientLayer btnoGradientGal;
		private const float yboundary = 65;
		private bool checkForRefresh;
		#region pull To Refresh
		private void setLoadingViewStyle ()
		{

			return;
	
			vLoading = new UIView (new RectangleF (0, 0 - this.View.Bounds.Size.Height, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height));

			vLoading.BackgroundColor = UIColor.FromRGB (225, 235, 239);

			vLoading.Layer.MasksToBounds = true;
			vLoading.Layer.CornerRadius = 0.0f;
			vLoading.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			UILabel lblTitle = new UILabel () {
				Font = UIFont.FromName("Helvetica Neue", 14),
				TextColor = UIColor.FromRGB (25, 119, 156),
				BackgroundColor = UIColor.Clear
			};
			//frame.size.height - 48.0f, 320.0f, 20.0f
			lblTitle.Frame = new RectangleF (0, vLoading.Frame.Height - 50, 320, 40);

			lblTitle.Layer.MasksToBounds = false;

			lblTitle.Layer.ShadowOffset = new SizeF (0, 1);
			lblTitle.Layer.ShadowOpacity = 0.5f;
			lblTitle.Opaque = true;
			lblTitle.TextAlignment = UITextAlignment.Center;
			lblTitle.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;


			lblTitle.Text = "Pull to Refresh...";

			vLoading.AddSubview (lblTitle);

			var imgViewArrow = new UIImageView (UIImage.FromBundle ("images/ic_menu_down.png"));

			imgViewArrow.Frame = new RectangleF (25, vLoading.Frame.Height - 65f, 30, 55);
			imgViewArrow.ContentMode = UIViewContentMode.ScaleAspectFit;
			imgViewArrow.Layer.MasksToBounds = true;
			imgViewArrow.Layer.Transform = CATransform3D.MakeRotation (3.14159265358979323846264338327950288f, 0f, 0f, 1f);

			vLoading.AddSubview (imgViewArrow);


			vLoading.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			this.s.AddSubview (vLoading);
		}

		private UIView vLoading{ get; set; }

		public bool reloading{ get; set; }

		public void loadDataFromSever ()
		{
			if (!InternetConnection.IsNetworkAvaialable (true)) {
				using (var alert = new UIAlertView("iPortogruaro", "Spiacente nessun collegamento internet al momento", null, "OK", null)) {//Viajes Telcel//Aceptar
					alert.Show ();
				}

				return;
			}

			if (reloading) 
				return;

			reloading = true;

			UIView.BeginAnimations ("slideAnimation");

			UIView.SetAnimationDuration (2);
			UIView.SetAnimationCurve (UIViewAnimationCurve.EaseInOut);
			UIView.SetAnimationRepeatCount (2);
			UIView.SetAnimationRepeatAutoreverses (true);
			this.s.ContentInset = UIEdgeInsets.Zero;
			UIView.CommitAnimations ();
			/*
                [UIView beginAnimations:nil context:NULL];
                [UIView setAnimationDuration:.3];
                [tblMain setContentInset:UIEdgeInsetsMake(0.0f, 0.0f, 0.0f, 0.0f)];
                [refreshHeaderView setStatus:kPullToReloadStatus];
                [refreshHeaderView toggleActivityView:NO];
                [UIView commitAnimations];
                */

			starthud ();
			NSUserDefaults.StandardUserDefaults.SetInt (1, "pulltoRefresh");
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (indicator);
			indicator.StartAnimating ();

			this.NavigationItem.RightBarButtonItem = null;

			this.tabBar.UserInteractionEnabled = false;

			ThreadPool.QueueUserWorkItem (state =>
			{




				var item = new iportogruaroLibraryShared.Mainiportogruaropos ().getPostById (dataPos.poi_id);// .getSubCategorys(cat_id,false);



				if (item == null) {
					InvokeOnMainThread (delegate {
						stophud (false);
						reloading = false;
					});
				} else if (item.poi_id == "0") {
					InvokeOnMainThread (delegate {
						stophud (false);
						reloading = false;
					});
				} else {
					InvokeOnMainThread (delegate {
						this.NavigationItem.LeftBarButtonItem = null;



						this.tabBar.UserInteractionEnabled = true;
						;
						try {

							//InvokeOnMainThread (delegate {
							try {
								if (controllerpastPos != null) {
									controllerpastPos.lst [positionPos] = item;
								} else if (controllerpastPosNew != null) {
									controllerpastPosNew.lst [positionPos] = item;
								} else if (controllerPast != null)
									controllerPast.loadData ();
								//});
							} catch {
							}

							lblLogintitle.Text = System.Web.HttpUtility.HtmlDecode (item.title);
							/*
                            lblLoginDes.Text = System.Web.HttpUtility.HtmlDecode (item.description);

                            if (lblLoginDes.Text.Length <= 0)
                                lblLoginDes.Text = System.Web.HttpUtility.HtmlDecode (item.title);
*/
							string html = "<style type='text/css'>h1{font-family: Helvetica Neue;font-size:16;color:#000000;}"
								+ "body{font-family:Helvetica Neue;font-size:14;color:rgb(174,65,61);;background-color: transparent;}"
								+ "</style>";

							dataPos = item;
							if (dataPos.description.Length > 0)
								html += System.Web.HttpUtility.HtmlDecode (dataPos.description);
							else
								html += System.Web.HttpUtility.HtmlDecode (dataPos.title);

            
							try {
								foreach (UIView vmain in lblLoginDes.Subviews) {
									if (vmain is UIImageView) {
										vmain.RemoveFromSuperview ();
									}

									if (vmain is UIScrollView) {
										UIScrollView svmain = vmain as UIScrollView;
										svmain.ShowsVerticalScrollIndicator = false;
										svmain.ShowsHorizontalScrollIndicator = false;
										foreach (UIView shadowview in svmain.Subviews) {
											if (shadowview is UIImageView)
												shadowview.Hidden = true;
										}
									}
								}
							} catch {
							}


							lblLoginDes.LoadHtmlString (html, null);

                            
							setLoadingViewStyle ();
							dontHadPhoto = false;
							if (dataPos != null) {
								imageView.Hidden = true;
								object[] on = new object[2];
								on [0] = imageView;
								on [1] = dataPos;
								System.Threading.ThreadPool.QueueUserWorkItem (RequestImage, on);   

								//System.Threading.ThreadPool.QueueUserWorkItem (setImageStatus, userMember);   

							}
							if (dataPos.galerias.Count > 0) {
								btnshowGal.Hidden = false;
								btnshowGal.TouchUpInside += delegate {
									this.NavigationController.PushViewController (new UiGaleryScreen (this) { dataPos = dataPos }, true);
								};
							} else {
								btnshowGal.Hidden = true;
							}
						} catch {
						}
						setScroll (this.View.Frame.Width, this.View.Frame.Height);
						//setGallery();
						loadTable ();
						stophud ();
						UIButton btnShare = UIButton.FromType (UIButtonType.Custom);
						UIImage imgShare = UIImage.FromFile ("images/ic_title_share_default.png");
						btnShare.TouchUpInside += delegate {
							if (UserInterfaceIdiomIsPhone)
								loadcomboTaks ();
							else
								loadpop (btnShare);
						};
						btnShare.SetImage (imgShare, UIControlState.Normal);
						btnShare.Frame = new RectangleF (0, 0, 30, 30);
						this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (btnShare);
						reloading = false;
					}
					);

				}

			});
			if (vLoading != null) {
				foreach (UIView vin in vLoading) {

					vin.RemoveFromSuperview ();
					vin.Dispose ();

				}
				vLoading.RemoveFromSuperview ();
				vLoading.Dispose ();
				vLoading = null;
			}
			//startHud();
			//}

		}
		#endregion
		private UIView vi;
		private UILabel lblLogintitle;
		private MonoTouch.UIKit.UIWebView lblLoginDes;
		private UIImageView imageView;
		private UiMapScreen myMap = null;
		private UiGaleryScreen myGal = null;
		private UiPosMoreInfoListController myinfo = null;

		void setstyle ()
		{

			tabBar.TintColor = base.headerColor;
			/*
            if (!UserInterfaceIdiomIsPhone)
            {
                UITextAttributes titleTextAttributes = new UITextAttributes();
                titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 20);
                tabBar.Items[0].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[1].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[2].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[3].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);




            }
            else
            {
                UITextAttributes titleTextAttributes = new UITextAttributes();
                titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 18);
                tabBar.Items[0].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[1].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[2].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                tabBar.Items[3].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
            }
            */


			UITextAttributes titleTextAttributes = new UITextAttributes ();
			//titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 18);
			titleTextAttributes.TextColor = UIColor.White;
			tabBar.Items [0].SetTitleTextAttributes (titleTextAttributes, UIControlState.Normal);
			tabBar.Items [1].SetTitleTextAttributes (titleTextAttributes, UIControlState.Normal);
			tabBar.Items [2].SetTitleTextAttributes (titleTextAttributes, UIControlState.Normal);
			tabBar.Items [3].SetTitleTextAttributes (titleTextAttributes, UIControlState.Normal);


			tabBar.Items [0].Title = "Info";
			tabBar.Items [1].Title = "Gallery";
			tabBar.Items [2].Title = "Mappa";
			tabBar.Items [3].Title = "Altre info";
			//put images
			//tabBar.Items [2].Image = UIImage.FromFile ("images/mainBar/Barra_Home.png");
			tabBar.Items [0].SetFinishedImages (UIImage.FromFile ("images/secondBar/info.png"), UIImage.FromFile ("images/secondBar/info.png"));
			tabBar.Items [1].SetFinishedImages (UIImage.FromFile ("images/secondBar/gallery.png"), UIImage.FromFile ("images/secondBar/gallery.png"));
			tabBar.Items [2].SetFinishedImages (UIImage.FromFile ("images/secondBar/map.png"), UIImage.FromFile ("images/secondBar/map.png"));
			tabBar.Items [3].SetFinishedImages (UIImage.FromFile ("images/secondBar/moreinfo.png"), UIImage.FromFile ("images/secondBar/moreinfo.png"));


			//tabBar.Items [3].Image

			// myMap = new UiMapScreen (){dataPos = dataPos};

			//myGal = new UiGaleryScreen (){dataPos = dataPos};
			//myinfo = new UiPosMoreInfoListController (dataPos);

			/*
            s.DraggingStarted += delegate(object sender, EventArgs e) {
                checkForRefresh = true;
            };

            s.DraggingEnded += delegate(object sender, DraggingEventArgs e) {
                checkForRefresh = false;
                if (s.ContentOffset.Y > -yboundary)
                    return;
                else
                    loadDataFromSever ();
            };
*/
			tabBar.ItemSelected += delegate(object sender, UITabBarItemEventArgs e) {

				switch (e.Item.Tag) {
				case 1:
					{
						if (vmap != null)
							vmap.Hidden = true;

				

						/*
                        if (!myGal.View.Hidden) {
                            myGal.View.RemoveFromSuperview ();
                            // myGal = null;
                        }
                        
*/
						if (v != null) {
							v.Hidden = true;
							sramdom.Hidden = true;
							pageControl.Hidden = true;
							tableInfo.Hidden = true;
							//vi.Hidden = true;
						} else
							sramdom.Hidden = true;
						pageControl.Hidden = true;
						tableInfo.Hidden = true;
						//vi.Hidden = false;
						v.Hidden = true;
					}

					this.View.BringSubviewToFront (s);
					//s.BackgroundColor = UIColor.Yellow;
					s.Hidden = false;
					vi.Hidden = false;
					this.s.BringSubviewToFront (vi);
					//vi.BackgroundColor = UIColor.Blue;

					//setScroll (this.View.Frame.Width, this.View.Frame.Height);
					break;
				case 2:
					{

                  
						if (vmap != null)
							vmap.Hidden = true;
						/*
                        if (!myGal.View.Hidden) {
                            myGal.View.RemoveFromSuperview ();
                            // myGal = null;
                        }
                    */
						tableInfo.Hidden = true;

						//sramdom.Hidden = false;
						pageControl.Hidden = false;

						vi.Hidden = true;
						if (v != null)
							v.Hidden = false;
						else
							sramdom.Hidden = false;


						setScroll (this.View.Frame.Width, this.View.Frame.Height);
					}
					break;
				case 3:
					{
						//var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
						//ad.GetLocation ();
						if (vmap != null)
							vmap.Hidden = true;
						/*
                        if (!myGal.View.Hidden) {
                            myGal.View.RemoveFromSuperview ();
                            // myGal = null;
                        }
*/
						if (v != null)
							v.Hidden = true;
						else
							sramdom.Hidden = true;

						pageControl.Hidden = true;
						tableInfo.Hidden = true;
                    
						if (vmap != null)
							vmap.Hidden = false;
						vi.Hidden = true;
						showMapa ();
						break;
					}
				case 4:
					{
						if (vmap != null)
							vmap.Hidden = true;
                    
						/*
                        if (!myGal.View.Hidden) {
                            myGal.View.RemoveFromSuperview ();
                            // myGal = null;
                        }
*/
						pageControl.Hidden = true;
						if (v != null)
							v.Hidden = true;
						else
							sramdom.Hidden = true;
						tableInfo.Hidden = false;
						vi.Hidden = true;
                
						break;
					}
				}
			};

			if (UserInterfaceIdiomIsPhone && IsTall) {
				s.Frame = new RectangleF (0, -50, 480, 520);
				s.ContentSize = new SizeF (480, 700);             
				s.ShowsHorizontalScrollIndicator = true;
			} else if (!UserInterfaceIdiomIsPhone) {
				this.View.Frame = new RectangleF (0, 0, 768, 1024);
				s.Frame = new RectangleF (0, 50, 1024, this.View.Bounds.Height - 10);
				s.ContentSize = new SizeF (this.View.Bounds.Width, this.View.Bounds.Height);             
				s.ShowsHorizontalScrollIndicator = true;
			}
			loadTable ();
			setScroll (this.View.Frame.Width, this.View.Frame.Height);
			try {
				vmap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - 45));
				this.View.AddSubview (vmap);
				//showMapa ();
			} catch {

				//vmap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height-45));
				//this.View.AddSubview (vmap);

			}
			setLoadingViewStyle ();
			vmap.Hidden = true;
			s.ContentSize = new SizeF (this.View.Bounds.Width, this.View.Bounds.Height);             
			s.ShowsHorizontalScrollIndicator = true;

			UIButton btnShare = UIButton.FromType (UIButtonType.Custom);
			UIImage imgShare = UIImage.FromFile ("images/ic_title_share_default.png");
			btnShare.TouchUpInside += delegate {
				if (UserInterfaceIdiomIsPhone)
					loadcomboTaks ();
				else
					loadpop (btnShare);
			};
			btnShare.SetImage (imgShare, UIControlState.Normal);
			btnShare.Frame = new RectangleF (0, 0, 30, 30);
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (btnShare);

			vi = new UIView (new RectangleF (10, 35, this.View.Bounds.Width - 20f, View.Bounds.Height - 35f));
			if (!UserInterfaceIdiomIsPhone) {
				vi.Frame = new RectangleF (10, 35, this.View.Bounds.Width - 20f, View.Bounds.Height - 125f);
			}
			//vi.AutosizesSubviews = true;
			vi.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            
            
            
			vi.BackgroundColor = UIColor.FromRGB (255, 255, 255);  
			vi.Layer.MasksToBounds = false;
			vi.Layer.CornerRadius = 7;
			this.s.BackgroundColor = UIColor.FromRGB (152, 22, 22);
			this.View.BackgroundColor = UIColor.FromRGB (152, 22, 22);
			this.s.InsertSubview (vi, 0); 


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

			imageView = new UIImageView ();
			imageView.Image = UIImage.FromFile ("images/NOIMG.png");
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;	 	//20130626
			if (UserInterfaceIdiomIsPhone)
				imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;


			if (UserInterfaceIdiomIsPhone && IsTall)
				imageView.Frame = new RectangleF (10, 45, 280, 200);
			else if (UserInterfaceIdiomIsPhone && !IsTall) {
				imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 180);
			} else {
				imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 420);
			}
			imageView.Layer.MasksToBounds = false;
			imageView.Layer.CornerRadius = 10;
			vi.AddSubview (imageView);


			if (dataPos != null) {
				imageView.Hidden = true;
				object[] on = new object[2];
				on [0] = imageView;
				on [1] = dataPos;
				System.Threading.ThreadPool.QueueUserWorkItem (RequestImage, on);   
                
				//System.Threading.ThreadPool.QueueUserWorkItem (setImageStatus, userMember);   
                
			}
			/*
            lblLoginDes = new UiWebView () {
               
                Font = UIFont.FromName("HelveticaNeue-Bold", 18f),
                TextColor = UIColor.FromRGB (174,65,61),
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear,
                ScrollEnabled = true,
                Editable = false,
                ShowsVerticalScrollIndicator = true,

                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight

            };
 */
			/*if (UserInterfaceIdiomIsPhone)
            {
                lblLogintitle.Font = UIFont.FromName ("HelveticaNeue-Bold", 14f);
                lblLoginDes.Font = UIFont.FromName ("HelveticaNeue-Bold", 12f);
            }
            if (!UserInterfaceIdiomIsPhone)
                lblLoginDes.TextAlignment = UITextAlignment.Left;
                */
			if (UserInterfaceIdiomIsPhone && IsTall)
				lblLoginDes = new MonoTouch.UIKit.UIWebView (new System.Drawing.RectangleF (5, 250, vi.Frame.Width - 10, 160));
			else if (UserInterfaceIdiomIsPhone && !IsTall) 
				lblLoginDes = new MonoTouch.UIKit.UIWebView (new System.Drawing.RectangleF (5, 190, vi.Frame.Width - 10, 180));
			else
				lblLoginDes = new MonoTouch.UIKit.UIWebView (new System.Drawing.RectangleF (10, 465, vi.Frame.Width - 20, 425));

			if (!UserInterfaceIdiomIsPhone)
				lblLoginDes.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			string html = "";
			if (UserInterfaceIdiomIsPhone)
				html = "<style type='text/css'>h1{font-family: Helvetica Neue;font-size:16;color:#000000;}"
					+ "body{font-family:Helvetica Neue;font-size:14;color:rgb(174,65,61);background-color: transparent;}"
					+ "</style>";
			else
				html = "<style type='text/css'>h1{font-family: Helvetica Neue;font-size:18;color:#000000;}"
					+ "body{font-family:Helvetica Neue;font-size:14;color:rgb(174,65,61);background-color: transparent;}"
					+ "</style>";
			if (dataPos.description.Length > 0)
				html += System.Web.HttpUtility.HtmlDecode (dataPos.description);
			else
				html += System.Web.HttpUtility.HtmlDecode (dataPos.title);
			lblLoginDes.BackgroundColor = UIColor.White;
			lblLoginDes.Layer.BorderColor = UIColor.White.CGColor;
			lblLoginDes.Opaque = false;


			try {
				foreach (UIView vmain in lblLoginDes.Subviews) {
					if (vmain is UIImageView) {
						vmain.RemoveFromSuperview ();
					}

					if (vmain is UIScrollView) {
						UIScrollView svmain = vmain as UIScrollView;
						svmain.ShowsVerticalScrollIndicator = true;
						svmain.ShowsHorizontalScrollIndicator = true;
						foreach (UIView shadowview in svmain.Subviews) {
							if (shadowview is UIImageView)
								shadowview.Hidden = true;
						}
					}
				}
			} catch {
			}


			lblLoginDes.LoadHtmlString (html, null);

			/*
            lblLoginDes.Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation";
            lblLoginDes.Text += "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation";
            lblLoginDes.Text += "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation";
            lblLoginDes.Text += "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation";
*/

			vi.AddSubview (lblLoginDes);



		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			Console.WriteLine ("ViewWillDisappear");
			if (_MapView != null) {
				_MapView._MapView.Dispose ();
				_MapView._MapView = null;

				_MapView.Dispose ();


				_MapView = null;

				GC.Collect ();
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
			ad.GetLocation ();

			setstyle ();
			loadData ();

			checkrotation ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public void checkrotation ()
		{
			if (UserInterfaceIdiomIsPhone) {
				switch (InterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto
					if (IsTall) {
						s.Frame = new RectangleF (0, 180, 560, 520);
						s.ContentSize = new SizeF (480, 800);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (40, 15, 500, 500);

						//vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 560, this.View.Bounds.Size.Height - 10);


					}
                                // setScroll(568,150);
                                else {
						///vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 480, this.View.Bounds.Size.Height - 200);

						s.Frame = new RectangleF (0, 180, 480, 520);
						s.ContentSize = new SizeF (480, 800);             
						s.ShowsHorizontalScrollIndicator = true;

					}


                                
					break;
				default:

					break;
				}


				//myinfo.View .Frame = new RectangleF (s.Frame.X,s.Frame.Y + 15,s.Frame.Width,s.Frame.Height -25);

			} else {
				switch (this.InterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
					{
						// vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 1024, this.View.Bounds.Size.Height - 200);

						s.Frame = new RectangleF (0, 310, 1024, 980);
						s.ContentSize = new SizeF (1024, 1000);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (10, 20, 1004, 980);
						/*
						s.Frame = new RectangleF (0, 240, 1024, 980);
						s.ContentSize = new SizeF (1024, 1000);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (10, 20, 1004, 980);
                           */

						imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 300);

						lblLoginDes.Frame = new System.Drawing.RectangleF (10, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 20, 280);

					}


                                //myMap.View.Frame = s.Bounds;
                                // myGal.View.Frame = s.Bounds;
                                // myinfo.View .Frame = new RectangleF (s.Frame.X,s.Frame.Y + 15,s.Frame.Width,s.Frame.Height -25);


					PointF p = new PointF (0, 0);
					s.SetContentOffset (p, true);

					break;
				default:
					{
						return;
					}

                                // myMap.View.Frame = s.Bounds;
                                // myGal.View.Frame = s.Bounds;
                                // myinfo.View .Frame = s.Bounds;


					break;
				}
			}
			int showgal = 0;
			if (!sramdom.Hidden)
				showgal = 1;
			if (UserInterfaceIdiomIsPhone) {
				switch (this.InterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto
					if (IsTall)
						setScroll (568, 270);
					else
						setScroll (480, 220);

                                //  loadMainCategory();
					break;
				default:
					if (IsTall)
						setScroll (320, 568);
					else
						setScroll (320, 380);
                                //loadMainCategory();
					break;
				}
			} else {
				switch (InterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto

					setScroll (1024, 768);

                                //loadMainCategory();
					break;
				default:
					setScroll (768, 1024);
                                //loadMainCategory();
					break;
				}

			}
			if (showgal == 1) {
				pageControl.Hidden = true;
				sramdom.Hidden = false;
			}

		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);


			if (UserInterfaceIdiomIsPhone) {
				switch (toInterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto
					if (IsTall) {
						s.Frame = new RectangleF (0, 230, 560, 520);
						s.ContentSize = new SizeF (480, 800);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (40, 15, 500, 500);

						//vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 560, this.View.Bounds.Size.Height - 10);


					}
                                // setScroll(568,150);
                                else {
						// vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 480, this.View.Bounds.Size.Height - 200);

						s.Frame = new RectangleF (0, 120, 480, 420);
						s.ContentSize = new SizeF (480, 600);             
						s.ShowsHorizontalScrollIndicator = true;

					}


					imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 80);

					lblLoginDes.Frame = new System.Drawing.RectangleF (5, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 10, 160);


					break;
				default:
					if (IsTall) {
						s.Frame = new RectangleF (0, -210, 320, 440);
						s.ContentSize = new SizeF (320, 600);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (10, 5, 300, 420);
					} else {
						s.Frame = new RectangleF (0, -150, 320, 480);
						s.ContentSize = new SizeF (320, 600);             
						s.ShowsHorizontalScrollIndicator = true;
						if (vLoading != null)
							vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 320, this.View.Bounds.Size.Height);

					}
                                
					imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 200);

					lblLoginDes.Frame = new System.Drawing.RectangleF (10, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 20, 160);

					break;
				}


				//myinfo.View .Frame = new RectangleF (s.Frame.X,s.Frame.Y + 15,s.Frame.Width,s.Frame.Height -25);

			} else {
				switch (toInterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
                                
                                
                                    //vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 1024, this.View.Bounds.Size.Height - 200);

					s.Frame = new RectangleF (0, 240, 1024, 980);
					s.ContentSize = new SizeF (1024, 1000);             
					s.ShowsHorizontalScrollIndicator = true;
					vi.Frame = new RectangleF (10, 20, 1004, 980);



					imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 300);
                                
					lblLoginDes.Frame = new System.Drawing.RectangleF (5, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 10, 280);


                   // lblLoginDes.Frame  =  new System.Drawing.RectangleF (10, 275, vi.Frame.Width - 20, 355);

					PointF p = new PointF (0, 0);
					s.SetContentOffset (p, true);

					break;
				default:
					{
						//vLoading.Frame = new RectangleF (0, 0 - this.View.Bounds.Size.Height, 768, this.View.Bounds.Size.Height);

						s.Frame = new RectangleF (0, -230, 768, 1024);
						s.ContentSize = new SizeF (768, 1024);             
						s.ShowsHorizontalScrollIndicator = true;
						vi.Frame = new RectangleF (10, 0, 748, 1024);
						imageView.Frame = new RectangleF (5, 45, vi.Frame.Width - 10, 420);

						lblLoginDes.Frame = new System.Drawing.RectangleF (5, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 10, 425);


						//lblLoginDes.Frame  =  new System.Drawing.RectangleF (10, imageView.Frame.Y + imageView.Frame.Height + 10, vi.Frame.Width - 20, 280);
						//lblLoginDes = new MonoTouch.UIKit.UIWebView ( new System.Drawing.RectangleF (10, 665, vi.Frame.Width - 20, 425));
					}

                                // myMap.View.Frame = s.Bounds;
                                // myGal.View.Frame = s.Bounds;
                                // myinfo.View .Frame = s.Bounds;


					break;
				}
			}
			int showgal = 0;
			if (!sramdom.Hidden)
				showgal = 1;
			if (UserInterfaceIdiomIsPhone) {
				switch (toInterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto
					if (IsTall)
						setScroll (568, 270);
					else
						setScroll (480, 220);

                                //  loadMainCategory();
					break;
				default:
					if (IsTall)
						setScroll (320, 568);
					else
						setScroll (320, 380);
                                //loadMainCategory();
					break;
				}
			} else {
				switch (toInterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
                                // chiudo il menu popover se aperto

					setScroll (1024, 768);

                                //loadMainCategory();
					break;
				default:
					setScroll (768, 1024);
                                //loadMainCategory();
					break;
				}

			}
			if (showgal == 1) {
				pageControl.Hidden = true;
				sramdom.Hidden = false;
			}
		}

		void loadData ()
		{
			lblItemAdress.Text = "Adress:   " + dataPos.adress;
			lblTitle.Text = "Title: " + dataPos.title;
			lblSubTitle.Text = "Phone: " + dataPos.phone;
			lblDes.Text = "Lat: " + dataPos.lat;// .description;


			lblItemAdress.Hidden = true;
			lblTitle.Hidden = true;
			lblSubTitle.Hidden = true;
			lblDes.Hidden = true;

			if (dataPos.lat.Length > 0) {
				//btnMap.Hidden = false;
				btnMap.TouchUpInside += delegate {
					this.NavigationController.PushViewController (new UiMapScreen () { dataPos = dataPos }, true);
				};
			} else {
				btnMap.Hidden = true;
			}
			btnShare.SetTitle ("Share", UIControlState.Normal);
			//btnShare.Hidden = false;
			btnShare.TouchUpInside += delegate(object sender, EventArgs e) {
				var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;


				//ad.LoginFb ();

				//ad.PostUserWall (dataPos.title, "http://Iportogruaro.com", dataPos.icon, dataPos.adress);
			};
		}

		private void NoImage (UIImageView imgView)
		{
			InvokeOnMainThread (delegate {
				imgView.Image = UIImage.FromFile ("images/NOIMG.png");
				imgView.Frame = new RectangleF (imgView.Frame.X, imgView.Frame.Y, imgView.Frame.Width, imgView.Frame.Height - 33); //20130626 per evitare che venga troncata la parte bassa dellimmagine
				imgView.ContentMode = UIViewContentMode.ScaleAspectFit;
				//controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
				imgView.Hidden = false;// .ContentMode = UIViewContentMode.ScaleAspectFit;
				RefreshImage (imgView);
			}
			);
		}

		private void RequestImage (object  state)
		{
			try {

				object[] on = state as object[];
				UIImageView controller = on [0] as UIImageView;
				iportogruaropos imgmain = on [1] as iportogruaropos;

				if (imgmain == null) {
					dontHadPhoto = false;
					NoImage (controller);
					return;
				}
				if (imgmain.photo == null) {
					dontHadPhoto = false;
					NoImage (controller);
					return;
				}

				if (imgmain.photo.Length == 0) {
					dontHadPhoto = false;
					NoImage (controller);
					return;
				}
				//icon: www.image/imgmain.png
				//detail:www.image/imgmain.png_detail


				//NSCache: nameimage

				if (imgmain.photo.Length > 0) {

					// NSUrl imageUrl = NSUrl.FromString (imgmain.stringUrl);
					// NSData imageData = NSData.FromUrl (imageUrl);

					//BEM_Chain.UvCache.ImageHotel (imgmain.HotelParameters.PropertyNumber + imgmain.Room.RoomCode+ "_g.png", imgmain.ImageName);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					//  string urlImg =  imgmain.stringUrl;

					UIImage imgRoom = ImageHotel (imgmain.photo.Replace ("/", "").Replace ("-", "").Replace (".", "").Replace (",", "") + "detail_.png", imgmain.photo);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					//UIImage imgRoom = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

					if (imgRoom != null) {
						InvokeOnMainThread (delegate {
							controller.Hidden = false;
							controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							imgRoom.Dispose ();
							imgRoom = null;

							dontHadPhoto = true;
							if (UserInterfaceIdiomIsPhone) {
								switch (this.InterfaceOrientation) {
								case UIInterfaceOrientation.LandscapeLeft:
								case UIInterfaceOrientation.LandscapeRight:
									break;
								default:
                                    //controller.ContentMode = UIViewContentMode.ScaleAspectFit;
									break;
								}
                              
                                        
                                    
								RefreshImage (controller);
								GC.Collect ();
							}
						}
						);
						// imgRoom = null;
					}
				} else {
					NoImage (controller);
					return;
				}
			} catch (Exception ex) {

				Console.WriteLine (ex.ToString ());
			}
		}

		private  UIImage ImageHotel (string imgName, string url)
		{
			try {
				UIImage resp;
				string sCachedPath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), imgName);
				resp = UIImage.FromFile (sCachedPath);   

				//Console.WriteLine (sCachedPath); 

				if (resp == null && url.Length > 0) {
					//Console.WriteLine("No se encontro cache imagenes");  

					NSUrl imageUrl = NSUrl.FromString (url);
					NSData imageData = NSData.FromUrl (imageUrl);
					if (imageData == null)
						return null;
					resp = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

					try {
						NSError err = new NSError (new NSString ("http://www.univisit.com"), 0); 

						//InvokeOnMainThread (delegate {
						var res = resp.AsPNG ().Save (System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), imgName), true, out err);
						if (!res)
							Console.WriteLine ("Error: " + err.LocalizedDescription); 
						//  }
						// );
						//01 Jul 2013
						resp = null;//12 Mb
						imageData.Dispose ();
						imageData = null;
						GC.Collect ();//cal
						resp = UIImage.FromFile (sCachedPath); // load from file sysrtem
					} catch (Exception ex) {

						Console.WriteLine (ex.ToString ());
						return null;
					}

				} else {
					//  Console.WriteLine("se carga cache imagenes");  
				}

				return resp;
			} catch (Exception ex) {

				Console.WriteLine (ex.ToString ());

				return null;
			}

		}

		private void RefreshImage (UIImageView controller)
		{

			try {

				//Console.WriteLine("<<<<<<  Actualiza Imagen >>>>>>>>>");
				var animation = CABasicAnimation.FromKeyPath ("opacity");
				animation.From = NSNumber.FromFloat (0);
				animation.To = NSNumber.FromFloat (1);
				animation.Duration = .5;
				//animation.Delegate = new MyAnimationDelegate (controller, true);
				//AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;

				controller.Layer.AddAnimation (animation, "moveToHeader");
			} catch {
			}

		}

		private void RefreshImageGal (UIImageView controller)
		{

			try {

				//Console.WriteLine("<<<<<<  Actualiza Imagen >>>>>>>>>");
				var animation = CABasicAnimation.FromKeyPath ("opacity");
				animation.From = NSNumber.FromFloat (0);
				animation.To = NSNumber.FromFloat (1);
				animation.Duration = .5;
				//animation.Delegate = new MyAnimationDelegate (controller, true);
				//AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;

				controller.Layer.AddAnimation (animation, "moveToHeader");
			} catch {
			}

		}
		#endregion
		#region combo
		#region LoadComboShare
		UIPickerView pkrShare;
		UIToolbar BarraShare;
		bool showComboTaks = false;
		private string[] shareList;

		public UIPopoverController detailparameters { get; set; }

		public UIButton LastTappedButton { get; set; }

		UINavigationController popviewNavigate;

		public void loadpop (NSObject sender)
		{

			int items = 0;
			try {
				items = dataPos.shareoptions.Count;
			} catch {
				items = 0;
			}

			if (items == 0)
				return;

			shareList = new string[items];
			items = 0;
			foreach (shareOptions s in dataPos.shareoptions) {

				shareList [items] = s.key;
				items ++;
			}
			/*
            shareList[0]= "FaceBook";
            shareList[1]= "Twitter";
            shareList[2]= "Email";
*/
			if (showComboTaks)
				return;
            
			cmbShare _pickerDataModel;
			_pickerDataModel = new cmbShare (this, shareList);
            
			BarraShare = BarraShareGet ();
			pkrShare = new UIPickerView (new RectangleF (0, 200, 320, 216)); 
            
			pkrShare.Source = _pickerDataModel;
			pkrShare.ShowSelectionIndicator = true; 

			UIViewController vistaMain = new UIViewController ();
			vistaMain.View.Frame = new RectangleF (0, 0, 320, 240);
			BarraShare.Frame = new RectangleF (0, 0, 320, 40);
			pkrShare.Frame = new RectangleF (0, 40, 320, 216);
			vistaMain.View.AddSubview (BarraShare);
			vistaMain.View.AddSubview (pkrShare);
			popviewNavigate = new UINavigationController (vistaMain);


			detailparameters = new UIPopoverController (popviewNavigate);
			detailparameters.SetPopoverContentSize (new SizeF (320, 240), true);
            
			detailparameters.DidDismiss += delegate {
				LastTappedButton = null;
				detailparameters = null;
			};
			UIButton tappedButton = (UIButton)sender;
			// this is for present the pop View
			detailparameters.PresentFromRect (tappedButton.Frame, View, UIPopoverArrowDirection.Any, true);
			LastTappedButton = tappedButton;

		}

		public void loadcomboTaks ()
		{
			int items = 0;
			try {
				items = dataPos.shareoptions.Count;
			} catch {
				items = 0;
			}

			if (items == 0)
				return;
            
			shareList = new string[items];
			items = 0;
			foreach (shareOptions s in dataPos.shareoptions) {

				shareList [items] = s.key;
				items ++;
			}
			/*
            shareList[0]= "FaceBook";
            shareList[1]= "Twitter";
            shareList[2]= "Email";
*/
			if (showComboTaks)
				return;

			cmbShare _pickerDataModel;
			_pickerDataModel = new cmbShare (this, shareList);

			if (IsTall)
				pkrShare = new UIPickerView (new RectangleF (0, 290, 320, 216));
			else
				pkrShare = new UIPickerView (new RectangleF (0, 200, 320, 216)); 
            
			pkrShare.Source = _pickerDataModel;
			pkrShare.ShowSelectionIndicator = true; 
            
            
            
			var animation = CABasicAnimation.FromKeyPath ("transform.translation.y");
			animation.From = NSNumber.FromFloat (100);
			animation.To = NSNumber.FromFloat (0);
			animation.Duration = .2;
			animation.FillMode = CAFillMode.Both;
            
			this.View.AddSubview (pkrShare);
			BarraShare = BarraShareGet ();
			this.View.AddSubview (BarraShare);
            
			animation.AutoReverses = false;
			//animation.Delegate = new MyAnimationDelegate (v, true);
			pkrShare.Layer.AddAnimation (animation, "moveToHeader");
            
			pkrShare.Select (0, 0, false);
            
			showComboTaks = true;

			UpdatecurrenShareOpt = 0;
		}

		private UIToolbar BarraShareGet ()
		{
			UIBarButtonItem cancelButton = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, delegate {
				hidecomboShare ();
			}
			);//Cancelar
			UIBarButtonItem Donebtn = new UIBarButtonItem (UIBarButtonSystemItem.Done, delegate {
                
				donecomboShare ();
			}
			);
            
			UIBarButtonItem space = new UIBarButtonItem (UIBarButtonSystemItem.FixedSpace);
			space.Width = 185;  
            
            
			UIToolbar bar;
			if (IsTall)
				bar = new UIToolbar (new RectangleF (0, 250, 320, 40));
			else
				bar = new UIToolbar (new RectangleF (0, 160, 320, 40));
            
            
			bar.Items = new UIBarButtonItem[] { cancelButton, space, Donebtn }; 
			bar.TintColor = UIColor.FromRGB (152, 22, 22);  
            
			return bar;
		}

		private void hidecomboShare ()
		{
			pkrShare.RemoveFromSuperview ();
			pkrShare = null;
			BarraShare.RemoveFromSuperview ();
			BarraShare = null;
			GC.Collect ();
            
			showComboTaks = false;

			if (!UserInterfaceIdiomIsPhone)
				detailparameters.Dismiss (true);
            
            
		}

		public void sendMail (string to)
		{
			MFMailComposeViewController _mail;

			if (MFMailComposeViewController.CanSendMail) {
				_mail = new MFMailComposeViewController ();
				_mail.SetMessageBody ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode (dataPos.title), 
				                      false);
				_mail.SetSubject (System.Web.HttpUtility.HtmlDecode (dataPos.title));
				_mail.Finished += HandleMailFinished;
				if (dontHadPhoto) {
					NSData dat = imageView.Image.AsJPEG (0);
					_mail.AddAttachmentData (dat, "image/png", System.Web.HttpUtility.HtmlDecode (dataPos.title));
				}
				_mail.SetToRecipients (new string [] { to });
				this.PresentModalViewController (_mail, true);

			} else {
				// handle not being able to send mail
			}
		}

		public void shareFb (UIImage imgToShare)
		{
			SLComposeViewController slComposer;

			if (SLComposeViewController.IsAvailable (SLServiceKind.Facebook)) {
				slComposer = SLComposeViewController.FromService (SLServiceType.Facebook);
				slComposer.SetInitialText ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode (dataPos.title));
				if (dontHadPhoto)  
					slComposer.AddImage (imgToShare);
				// slComposer.AddImage (UIImage.FromFile ("monkey.png"));
				slComposer.CompletionHandler += (result) => {
					InvokeOnMainThread (() => {
						DismissViewController (true, null);
						if (result == SLComposeViewControllerResult.Done) {
							var alert = new UIAlertView ("Esito", "messaggio mandato", null, "Ok");

							alert.Show ();
						}
					});
				};
				PresentViewController (slComposer, true, null);
			} else {
				InvokeOnMainThread (() => {

					var alert = new UIAlertView ("Facebook", "Entrare nei settaggi per configurare l'account facebook", null, "Ok");

					alert.Show ();


				});
			}
		}
		//20130708 popup facebook, twitter e mail
		public void donecomboShare ()
		{
           
            
			hidecomboShare ();
			string key = shareList [UpdatecurrenShareOpt];

			Console.WriteLine (key + " Llave: " + UpdatecurrenShareOpt);

			switch (key) {
			case "facebook":
				{
					SLComposeViewController slComposer;
                
					if (SLComposeViewController.IsAvailable (SLServiceKind.Facebook)) {
						slComposer = SLComposeViewController.FromService (SLServiceType.Facebook);
						slComposer.SetInitialText ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode (dataPos.title));
						if (dontHadPhoto)  
							slComposer.AddImage (imageView.Image);
						// slComposer.AddImage (UIImage.FromFile ("monkey.png"));
						slComposer.CompletionHandler += (result) => {
							InvokeOnMainThread (() => {
								DismissViewController (true, null);
								if (result == SLComposeViewControllerResult.Done) {
									var alert = new UIAlertView ("Esito", "messaggio mandato", null, "Ok");
                                
									alert.Show ();
								}
							});
						};
						PresentViewController (slComposer, true, null);
					} else {
						InvokeOnMainThread (() => {
                   
							var alert = new UIAlertView ("Facebook", "Entrare nei settaggi per configurare l'account facebook", null, "Ok");
                        
							alert.Show ();


						});
					}

				}
				break;
			case "twitter":
				{
					SLComposeViewController slComposer;
                
					if (SLComposeViewController.IsAvailable (SLServiceKind.Twitter)) {
						slComposer = SLComposeViewController.FromService (SLServiceType.Twitter);
						slComposer.SetInitialText ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode (dataPos.title));
						if (dontHadPhoto)  
							slComposer.AddImage (imageView.Image);
						// slComposer.AddImage (UIImage.FromFile ("monkey.png"));
						slComposer.CompletionHandler += (result) => {
							InvokeOnMainThread (() => {
								DismissViewController (true, null);
								if (result == SLComposeViewControllerResult.Done) {
									var alert = new UIAlertView ("Esito", "messaggio mandato", null, "Ok");

									alert.Show ();
								}
							});
						};
						PresentViewController (slComposer, true, null);
					} else {
						InvokeOnMainThread (() => {
                        
							var alert = new UIAlertView ("Twitter", "Entrare nei settaggi per configurare l'account Twitter", null, "Ok");
                        
							alert.Show ();
                        
                        
						});
					}
					break;


				}
			case "mail":
				{
					MFMailComposeViewController _mail;
                
					if (MFMailComposeViewController.CanSendMail) {
						_mail = new MFMailComposeViewController ();
						_mail.SetMessageBody ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode (dataPos.title), 
						                      false);
						_mail.SetSubject (System.Web.HttpUtility.HtmlDecode (dataPos.title));
						_mail.Finished += HandleMailFinished;
						if (dontHadPhoto == true) {
							NSData dat = imageView.Image.AsJPEG (0);
							_mail.AddAttachmentData (dat, "image/png", System.Web.HttpUtility.HtmlDecode (dataPos.title));
						}
						this.PresentModalViewController (_mail, true);
                    
					} else {
						// handle not being able to send mail
					}
					break;
				}
			}
            
		}

		void HandleMailFinished (object sender, MFComposeResultEventArgs e)
		{
			if (e.Result == MFMailComposeResult.Sent) {
				UIAlertView alert = new UIAlertView ("Mail Alert", "Mail Spedita",
				                                     null, "Ok", null);
				alert.Show ();
                
				// you should handle other values that could be returned 
				// in e.Result and also in e.Error 
			}
			e.Controller.DismissModalViewControllerAnimated (true);
		}
		#endregion
		#endregion
		#region mainScroll
		private UIScrollView sramdom;
		private UIPageControl pageControl;

		private float heightscroll { get; set; }

		private float widthscroll { get; set; }

		private void setScroll (float w, float h)
		{   
			heightscroll = h - 85;//this.View.Bounds.Height;
			widthscroll = w;//this.View.Bounds.Width;;
            
			if (sramdom != null) {
				foreach (UIView v in sramdom) {
					v.RemoveFromSuperview ();
					v.Dispose ();
				}
                
				sramdom.RemoveFromSuperview ();
				sramdom.Dispose ();
				sramdom = null;
			}

			if (pageControl != null) {

                
				pageControl.RemoveFromSuperview ();
				pageControl.Dispose ();
				pageControl = null;
			}

			if (UserInterfaceIdiomIsPhone)
				sramdom = new UIScrollView (new RectangleF (0, 0, widthscroll, heightscroll));
			else
				sramdom = new UIScrollView (new RectangleF (0, -40, widthscroll, heightscroll));
			RectangleF scrollFrame = sramdom.Frame; 
			try {
                
                
				if (dataPos.galerias.Count <= 0) {
					sramdom = new UIScrollView (new RectangleF (0, -10, this.View.Bounds.Width, this.View.Bounds.Height - 55));
					sramdom.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

					sramdom.Hidden = true;
					sramdom.PagingEnabled = true;
					// sramdom.ContentSize = scrollFrame.Size;               
					sramdom.ShowsHorizontalScrollIndicator = false;
					pageControl = new UIPageControl (new RectangleF (0, heightscroll - 10, this.View.Bounds.Width, 20));
					UIImageView imgViewMainImage = new UIImageView (UIImage.FromFile ("images/NOIMG.png"));
					imgViewMainImage.Frame = this.sramdom.Frame;
					imgViewMainImage.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					imgViewMainImage.ContentMode = UIViewContentMode.ScaleAspectFit;
                   
					imgViewMainImage.UserInteractionEnabled = true;
					imgViewMainImage.Hidden = false;
					this.sramdom.AddSubview (imgViewMainImage);
					this.View.AddSubview (sramdom);


					if (tabBar.SelectedItem.Tag == 2) {
						sramdom.Hidden = false;
					}

					return;
				}
                
				// UIImage image;
               
				pageControl = new UIPageControl (new RectangleF (0, heightscroll - 10, this.View.Bounds.Width, 20));
				// vistaImagenes = new UIView (new RectangleF (0, 0, 320 , 367));
				//MyScrollView = new UIScrollView (new RectangleF (0, 0,320 , 367));
				//vistaImagenes.BackgroundColor = UIColor.Gray;//  .Clear ;         
                 
				//MyScrollView.BackgroundColor =UIColor.Green ; 
				//if (gal.Count <= 5)
				scrollFrame.Width = scrollFrame.Width * dataPos.galerias.Count;
				//else
				//  scrollFrame.Width = scrollFrame.Width * 5;
                
				sramdom.Scrolled += delegate(object sender, EventArgs e) {
					try {
                        
						double page = Math.Floor ((sramdom.ContentOffset.X - sramdom.Frame.Width / 2) / sramdom.Frame.Width) + 1;
						pageControl.CurrentPage = (int)page;
                        
                        
					} catch {
					}
				};
                
				pageControl.ValueChanged += delegate(object sender, EventArgs e) {
                    
					var pc = (UIPageControl)sender; 
					double fromPage = Math.Floor ((sramdom.ContentOffset.X - sramdom.Frame.Width / 2) / sramdom.Frame.Width) + 1;                    
					var toPage = pc.CurrentPage; 
					var pageOffset = sramdom.Frame.Width * toPage;
					if (fromPage > toPage) 
						pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
					PointF p = new PointF (pageOffset, 0); 
					sramdom.SetContentOffset (p, true); 
					//btnaddItinerary.SetTitle ("+ Bookmark", UIControlState.Normal);
                    
				};
                
				pageControl.Hidden = false;
                
                
				pageControl.Pages = dataPos.galerias.Count;
				//  else
				//      pageControl.Pages = 5;//maximo 5 imagenes
                
                
                
				sramdom.BackgroundColor = UIColor.Clear;
                
                
				sramdom.PagingEnabled = true;
				sramdom.ContentSize = scrollFrame.Size;                
				sramdom.ShowsHorizontalScrollIndicator = false;
				//sramdom.AutoresizingMask = UIViewAutoresizing.All;
				RectangleF frame = sramdom.Frame;
				frame.X = 0;
				int i = 0;
				foreach (galeriaImagenes g in dataPos.galerias) {
					System.Drawing.RectangleF Fr = new System.Drawing.RectangleF (frame.Width * i, 0, widthscroll, heightscroll); 
                    
                    
                    
                    
                    
                    
					UIImageView imgViewMainImage = new UIImageView ();//(UIImage.FromFile("images/"+namebigPicture));
					imgViewMainImage.Frame = Fr;
					imgViewMainImage.ContentMode = UIViewContentMode.ScaleToFill;
					imgViewMainImage.UserInteractionEnabled = true;
					sramdom.InsertSubview (imgViewMainImage, 0);
                    
                    
					object[] on = new object[2];
					on [0] = imgViewMainImage;
					on [1] = g;
					System.Threading.ThreadPool.QueueUserWorkItem (RequestImageGal, on);   
                    
                    
                    
					i++;
                    
                    
				}
				sramdom.Hidden = true;
				pageControl.Hidden = true;
				this.View.AddSubview (setGallery (w, h));
				// View.AddSubview (pageControl);
                
                
                
                
                
                
			} catch {
			}
            
		}
		#endregion
		#region galery
		private void NoImageGal (UIImageView imgView)
		{
			InvokeOnMainThread (delegate {
				imgView.Image = UIImage.FromFile ("images/NOIMG.png");
				;
				; //   UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
				imgView.Hidden = false;
				imgView.ContentMode = UIViewContentMode.ScaleAspectFit;
				RefreshImage (imgView);
			}
			);
		}

		private void RequestImageGal (object  state)
		{
			try {
                
				object[] on = state as object[];
				UIImageView controller = on [0] as UIImageView;
				galeriaImagenes imgmain = on [1] as galeriaImagenes;
                
				if (imgmain == null) {
					NoImageGal (controller);
					return;
				}
				if (imgmain.urlString == null) {
					NoImageGal (controller);
					return;
				}
                
				if (imgmain.urlString.Length > 0) {
                    
					// NSUrl imageUrl = NSUrl.FromString (imgmain.stringUrl);
					// NSData imageData = NSData.FromUrl (imageUrl);
                    
					//BEM_Chain.UvCache.ImageHotel (imgmain.HotelParameters.PropertyNumber + imgmain.Room.RoomCode+ "_g.png", imgmain.ImageName);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					//  string urlImg =  imgmain.stringUrl;
                    
					UIImage imgRoom = ImageHotel (imgmain.urlString.Replace ("/", "").Replace ("-", "").Replace (".", "").Replace (",", "") + ".png", imgmain.urlString);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					//UIImage imgRoom = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    
					if (imgRoom != null) {
						InvokeOnMainThread (delegate {
							controller.Hidden = false;
							controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							imgRoom.Dispose ();
							imgRoom = null;
							controller.ContentMode = UIViewContentMode.ScaleAspectFit; 
							RefreshImageGal (controller);
							GC.Collect ();
						}
						);
						// imgRoom = null;
					}
				}
			} catch (Exception ex) {
                
				Console.WriteLine (ex.ToString ());
			}
		}
		#endregion
		#region map
		MapView _MapView { get; set; }

		MKMapView map;
		UIView vmap;

		private void hideMapa (bool hide)
		{
            
		}
		/*
         * BAckend -> New Event 
         * **** Bacneknd -> Send notify Evtents (working in the process)
         * json(msg,device id) - Apple
         * Certifacte 
         * 
         * 1.- Create the certicate
         * 2.- send a push notification to the device
         * 3.- Hanldre the push notification in the app
         * 4.- work in back end to senf pudsh notification (***)
         */
		private void showMapa ()
		{
			//return;
			if (dataPos.lat.Length <= 0 || dataPos.lon.Length <= 0) {
				if (vmap != null)
					vmap.Hidden = true;
				return;
			}

			if (_MapView != null)
				return;
			try {


				//starthud ();
				double latMain = double.Parse (dataPos.lat);//.Replace(".",","));
				double lonMain = double.Parse (dataPos.lon);//.Replace(".",","));

				ThreadPool.QueueUserWorkItem (state =>
				{
			
					//vmstrathuap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height-45));
					System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo ("en-US");    
					System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo ("en-US");          

					InvokeOnMainThread (delegate {
						vmap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - 50));
						vmap.AutosizesSubviews = true;
						vmap.BackgroundColor = UIColor.White;// .FromRGBA (0.0f, 0.0f, 0.0f, 0.75f);
						vmap.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
						//	View.AddSubview (vmap); 

				
						_MapView = new MapView (new RectangleF (0, 0, vmap.Frame.Width, vmap.Frame.Height));
						_MapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

				
					


					
						vmap.AddSubview (_MapView);   

						View.AddSubview (vmap); 


						View.BringSubviewToFront (tabBar);

					});

					System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo ("en-US");    
					System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo ("en-US");          





					double userlat = 0;
					double userlon = 0;

					try {
						string strlat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
						string strlon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");

						userlat = double.Parse (strlat);
						userlon = double.Parse (strlon);

						//userlat = double.Parse(dataPos.lat);//.Replace(".",","));
						//userlon = double.Parse(dataPos.lon);//.Replace(".",","));
					} catch {
						userlat = double.Parse (dataPos.lat);//.Replace(".",","));
						userlon = double.Parse (dataPos.lon);//.Replace(".",","));
					}
				

					var home = new Place () {
						Name = "Location",
						Description = "Current Location",
						//Name = System.Web.HttpUtility.HtmlDecode(dataPos.title),
						//Description = System.Web.HttpUtility.HtmlDecode(dataPos.adress),

						Latitude = userlat,
						Longitude = userlon,
					};

					var office = new Place () {
						Name = System.Web.HttpUtility.HtmlDecode(dataPos.title),
						Description = System.Web.HttpUtility.HtmlDecode(dataPos.adress),
						Latitude = double.Parse(dataPos.lat),
						Longitude = double.Parse(dataPos.lon),
					};

					_MapView.ShowRouteFrom (office, home);

					InvokeOnMainThread (delegate {
						//stophud ();

					});
				});
				return;

				double zoom = 5f;
				//double d = Double.Parse(s, new CultureInfo("es-ES"));

				//double latMain = double.Parse(dataPos.lat);//.Replace(".",","));
				//double lonMain = double.Parse(dataPos.lon);//.Replace(".",","));
				//latMain = 24.114285600477704;
				//lonMain =  -110.34087270498276;
				List<MyAnnotation> pins = null;

				try {

					//C//onsole.WriteLine ("Entra carga mapa"); 
					if (map != null)
						hideMapa (false);




					vmap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - 50));
					vmap.AutosizesSubviews = true;
					vmap.BackgroundColor = UIColor.White;// .FromRGBA (0.0f, 0.0f, 0.0f, 0.75f);
					vmap.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

					map = new MKMapView ();// (new RectangleF (0, 0, 400f, 300f ));
					map.Frame = vmap.Frame;
					map.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					//??
					//map.Region = new MonoTouch.MapKit.MKCoordinateRegion (new CLLocationCoordinate2D (latMain, lonMain), new MKCoordinateSpan (zoom, zoom));

					map.Hidden = false;

					vmap.AddSubview (map); 
					View.AddSubview (vmap);  
					//this.View.AddSubview (v); 
					/*
                    string strlat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
                    string strlon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");

                   
                    double userlat = double.Parse(strlat);
                    double userlon = double.Parse(strlon);
                    
                  */
					//var lst = iportogruaroLibraryShared.routeJson.getRoute(userlat,userlon,latMain,lonMain);



					double lat = latMain;
					double lon = lonMain;
					if (pins == null)
						pins = new List<MyAnnotation> ();
					pins.Add (new MyAnnotation (new CLLocationCoordinate2D (lat, lon), "hotel.Name", "hotel.Adress", MKPinAnnotationColor.Purple, null));

					/*
                                        foreach(gpsRoute g in lst)
                                        {
                                            pins.Add (new MyAnnotation (new CLLocationCoordinate2D (g.lat_start, g.lon_start), "hotel.Name", "hotel.Adress", MKPinAnnotationColor.Red, null));
                                            pins.Add (new MyAnnotation (new CLLocationCoordinate2D (g.lat_end, g.lon_end), "hotel.Name", "hotel.Adress", MKPinAnnotationColor.Red, null));

                                        }



                                        //map.GetViewForOverlay = Map_GetViewForOverlay;

                                        CLLocationCoordinate2D[] polyPoints = new  CLLocationCoordinate2D[lst.Count];

                                        int index = 0;

                                        foreach (MyAnnotation hot in pins) {
                                            polyPoints[index] = hot.coordinate;

                                        }

                                        MKPolyline line = MKPolyline.FromCoordinates(polyPoints);

                                        map.AddOverlay(line);

                                        map.SetVisibleMapRect(line.BoundingMapRect, true);
*/
					map.MapType = MonoTouch.MapKit.MKMapType.Standard;


					map.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (lat, lon), 
					                                     new MKCoordinateSpan (zoom, zoom));

				} catch (Exception ex) {
					Console.WriteLine ("Error en la carga de Mapas. Error: " + ex.Message); 
				}

				//var logo = UIImage.FromBundle ("images/googlemaps-point");
				map.GetViewForAnnotation = delegate(MKMapView mapView, NSObject annotation) {
					// Called by the map whenever an annotation is added and needs to be displayed
					if (annotation is MKUserLocation)
						return null;

					MyAnnotation myAnn = annotation as MyAnnotation;


					var annView = mapView.DequeueReusableAnnotation ("mypin");
					if (annView == null) {
						var pinView = new MKPinAnnotationView (myAnn, "mypin");
						pinView.AnimatesDrop = true;


						//pinView.Image =  UIImage.FromBundle  ("images/googlemaps-point");
						//UIImageView imgView = new UIImageView (logo);

						//imgView.Frame = new System.Drawing.RectangleF (-8, -5, 32, 39);

						//pinView.AddSubview (imgView);//   .PinColor =   myAnn.Color;
						pinView.PinColor = MKPinAnnotationColor.Purple;// .PinColor =   myAnn.Color;
						//pinView.AutosizesSubviews =true;  

						//  pinView.Image =  UIImage.FromFile   ("images/googlemaps-point.png");
						pinView.CanShowCallout = false;

						//pinView.LeftCalloutAccessoryView = imgView ;  

						UIButton rightCallout = UIButton.FromType (UIButtonType.DetailDisclosure);
						rightCallout.TouchUpInside += delegate {
							//pinView.AddSubview(imgView);//   .PinColor =   myAnn.Color;

						};

						myAnn.app = this; 

						//myAnn.list =list; 
						//  myAnn.HotelSearch = HotelSearch ; 
						if (myAnn.active)  
							pinView.RightCalloutAccessoryView = myAnn.btnSelect;//   rightCallout;

						annView = pinView;
					} else {
						annView.Annotation = annotation;
					}
					return annView;
				};

				map.ZoomEnabled = true;
				map.ScrollEnabled = true;
				map.UserInteractionEnabled = true;
				//map.ShowsUserLocation = false;  // shows the "blue dot" user location (if available)
				//map.UserLocation.Title = Language.GetString ("BEM00125"); 
				//map.UserLocation.Subtitle = Language.GetString ("BEM00126");  



				map.DidFailToLocateUser += delegate(object sender, NSErrorEventArgs e) {

                                    };

				map.LoadingMapFailed += delegate(object sender, NSErrorEventArgs e) {
					//Tools.ShowAlert("TODO: Lo sentimos el mapa no puede ser cargado en este momento");


				};  

				map.AddAnnotationObject (pins [0]);

                                    
				foreach (MyAnnotation hot in pins) {
					if (hot.Coordinate.IsValid ())  
						map.AddAnnotationObject (hot);
                    
				}



				map.CenterCoordinate = new CLLocationCoordinate2D (latMain, lonMain);
				map.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (latMain, lonMain), new MKCoordinateSpan (.1, .1));
				map.ShowsUserLocation = true;  // shows the "blue dot" user location (if available)

			} catch {
			}
		}

		MKOverlayView Map_GetViewForOverlay (MKMapView mapView, NSObject overlay)
		{
			if (overlay.GetType () == typeof(MKPolyline)) {
				MKPolylineView p = new MKPolylineView ((MKPolyline)overlay);
				p.LineWidth = 3.5f;
				p.StrokeColor = UIColor.Red;//.Green;
				return p;
			} else
				return null;
		}
		#endregion
		#region tableInfo
		private UITableView tableInfo;

		private void loadTable ()
		{
			if (tableInfo != null) {

				tableInfo.RemoveFromSuperview ();
				tableInfo.Dispose ();
				tableInfo = null;
			}

			tableInfo = new UITableView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - 50), UITableViewStyle.Plain);
			tableInfo.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			tableInfo.Source = new sourceMoreInfo (this, dataPos);

			this.View.AddSubview (tableInfo);

			tableInfo.ReloadData ();
			tableInfo.Hidden = true;


		}
		#endregion
		#region UIGridView
		UIView v;
		UIScrollView ss;

		private UIView setGallery (float WidthGal, float heightGal)
		{
			if (ss != null) {

				foreach (UIView myView in ss.Subviews) {
					myView.RemoveFromSuperview ();

				}
				ss.RemoveFromSuperview ();
				ss.Dispose ();
				ss = null;

			}
			if (v != null) {

				foreach (UIView myView in v.Subviews) {
					myView.RemoveFromSuperview ();

				}
				v.RemoveFromSuperview ();
				v.Dispose ();
				v = null;

			} else {

				/*
                for (int x = 0; x < 8; x++) {
                
                    dataPos.galerias.Add (dataPos.galerias[0]);
                
                }
            */
			}


			GC.Collect ();

			if (IsTall)
				heightGal = heightGal - 125;
			else
				heightGal = heightGal - 25;


			int ancholabel = 80;
			int largolabel = 80;
			int margin = 0;
			int margintop = 55;

			if (!UserInterfaceIdiomIsPhone) {

				// IPAD
				ancholabel = 210;
				largolabel = 210;
				margin = 0;
				margintop = 140;

				heightGal = heightGal - 125;
			}

								
			v = new UIView ();
			v.Tag = 0;
			v.BackgroundColor = UIColor.Clear;
			v.Frame = new RectangleF (0, 0, WidthGal, heightGal);



			ss = new UIScrollView (new RectangleF (30, 10, WidthGal, heightGal));
			if (!UserInterfaceIdiomIsPhone) {
				switch (InterfaceOrientation) {

				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					if (WidthGal == 768) {
						ss.Frame = new RectangleF (60, 10, WidthGal, heightGal);
					} else
						ss.Frame = new RectangleF (205, 10, WidthGal, heightGal);
					break;
				default:
					if (WidthGal == 1024) {
						ss.Frame = new RectangleF (205, 10, WidthGal, heightGal);
					} else
						ss.Frame = new RectangleF (60, 10, WidthGal, heightGal);
					break;

				}



			}
			ss.AutosizesSubviews = true;
			ss.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;


			System.Drawing.RectangleF contentRect = new System.Drawing.RectangleF (10, 100, this.View.Bounds.Width - 20, this.View.Bounds.Height - 100);   
			ss.ContentSize = new System.Drawing.SizeF (contentRect.Width, contentRect.Height);  
			ss.ShowsHorizontalScrollIndicator = false;
			ss.ScrollEnabled = true;
			ss.BackgroundColor = UIColor.Clear;

			int h = 0;
			// dataPos.galerias
			//galeriaImagenes g in dataPos.galeria
			contentRect = new System.Drawing.RectangleF (0, 0, v.Frame.Width, ((dataPos.galerias.Count / 3 + dataPos.galerias.Count % 3) * (largolabel + 5)) + largolabel);      


			ss.ContentSize = new System.Drawing.SizeF (contentRect.Width, contentRect.Height);  
			ss.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			/*
								UIImageView Maincontent;
								Maincontent = new UIImageView (new RectangleF (10, 10, v.Frame.Width - 300, v.Frame.Height - 35));  
								galeriaImagenes imgmain = dataPos.galerias [0];
								Maincontent.Image = UIImage.FromFile ("images/NOIMG.png");  //BEM_Chain.UvCache.ImageHotel (this.hotel.Hotel_HOD_RS.Property.GeneralInformation.PropertyNumber + "_" + imgmain.imgId + "_g.png", imgmain.imgUrl);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
			Maincontent.ContentMode= UIViewContentMode.ScaleAspectFit;	//20130626
			object[] on = new object[2];
								on [0] = new object ();
								on [0] = Maincontent;
								on [1] = new object ();
								on [1] = imgmain;
								*/
			//System.Threading.ThreadPool.QueueUserWorkItem (RequestImageUiGridView, on);   
			//Maincontent.ContentMode = UIViewContentMode.ScaleToFill;
			//v.AddSubview (Maincontent); 
			for (int x = 0; x < dataPos.galerias.Count; x++) {
				galeriaImagenes img = dataPos.galerias [x];
				UIImageView content = new UIImageView (new RectangleF (margin, (largolabel - margintop + 5) * x, ancholabel, largolabel));  

				content.Layer.BorderColor = UIColor.LightGray.CGColor;
				content.Layer.BorderWidth = 3.0f;

				content.Image = UIImage.FromFile ("images/NOIMG.png");  //BEM_Chain.UvCache.ImageHotel (this.hotel.Hotel_HOD_RS.Property.GeneralInformation.PropertyNumber + "_" + img.imgId + "_g.png", img.imgUrl);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
				content.ContentMode = UIViewContentMode.ScaleAspectFit;		//20130626		
				object[] on_1 = new object[2];
				on_1 [0] = new object ();
				on_1 [0] = content;
				on_1 [1] = new object ();
				on_1 [1] = img;
				System.Threading.ThreadPool.QueueUserWorkItem (RequestImageUiGridView, on_1);     
				//content.ContentMode = UIViewContentMode.ScaleToFill;
				ss.AddSubview (content); 

				var btn_a = UIButton.FromType (UIButtonType.Custom);
				btn_a.Frame = content.Frame;
				btn_a.Tag = x;
				btn_a.TouchUpInside += delegate(object sender, EventArgs e) {
					UIButton b = sender as UIButton;
					this.NavigationController.PushViewController (new UiGaleryScreen (this) { dataPos = dataPos, positionImg = b.Tag }, true);
				};
				ss.AddSubview (btn_a); 

				if ((x + 1) < dataPos.galerias.Count) {

					UIImageView content_b = new UIImageView (new RectangleF (ancholabel + (margin * 2) + 10, (largolabel - margintop + 5) * x, ancholabel, largolabel));    

					content_b.Layer.BorderColor = UIColor.LightGray.CGColor;
					content_b.Layer.BorderWidth = 3.0f;

					x++;
					img = dataPos.galerias [x];
					content_b.Image = UIImage.FromFile ("images/NOIMG.png");  //BEM_Chain.UvCache.ImageHotel (this.hotel.Hotel_HOD_RS.Property.GeneralInformation.PropertyNumber + "_" + img.imgId + "_g.png", img.imgUrl);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					content.ContentMode = UIViewContentMode.ScaleAspectFit;	 	//20130626
					object[] on_2 = new object[2];
					on_2 [0] = new object ();
					on_2 [0] = content_b;
					on_2 [1] = new object ();
					on_2 [1] = img;
					System.Threading.ThreadPool.QueueUserWorkItem (RequestImageUiGridView, on_2); 
					//content_b.ContentMode = UIViewContentMode.ScaleToFill;
					ss.AddSubview (content_b); 

					var btn_2 = UIButton.FromType (UIButtonType.Custom);
					btn_2.Frame = content_b.Frame;
					btn_2.Tag = x;
					btn_2.TouchUpInside += delegate(object sender, EventArgs e) {
						UIButton b = sender as UIButton;
						this.NavigationController.PushViewController (new UiGaleryScreen (this)
 { dataPos = dataPos, positionImg = b.Tag }, true);

					};
					ss.AddSubview (btn_2); 

				}

				if ((x + 1) < dataPos.galerias.Count) {

					UIImageView content_c = new UIImageView (new RectangleF ((ancholabel * 2) + (margin * 2) + 20, (largolabel - margintop + 5) * (x - 1), ancholabel, largolabel));    

					content_c.Layer.BorderColor = UIColor.LightGray.CGColor;
					content_c.Layer.BorderWidth = 3.0f;
					content.ContentMode = UIViewContentMode.ScaleAspectFit;	 	//20130626
					x++;
					img = dataPos.galerias [x];
					content_c.Image = UIImage.FromFile ("images/NOIMG.png");  //BEM_Chain.UvCache.ImageHotel (this.hotel.Hotel_HOD_RS.Property.GeneralInformation.PropertyNumber + "_" + img.imgId + "_g.png", img.imgUrl);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

					object[] on_3 = new object[2];
					on_3 [0] = new object ();
					on_3 [0] = content_c;
					on_3 [1] = new object ();
					on_3 [1] = img;
					System.Threading.ThreadPool.QueueUserWorkItem (RequestImageUiGridView, on_3); 

					//content_c.ContentMode = UIViewContentMode.ScaleToFill;
					ss.AddSubview (content_c); 

					var btn = UIButton.FromType (UIButtonType.Custom);
					btn.Frame = content_c.Frame;
					btn.Tag = x;
					btn.TouchUpInside += delegate(object sender, EventArgs e) {
						UIButton b = sender as UIButton;
						this.NavigationController.PushViewController (new UiGaleryScreen (this) {
							dataPos = dataPos,
							positionImg = b.Tag
						}, true);

					};
					ss.AddSubview (btn); 
				}


			}

			v.AddSubview (ss); 

			v.Hidden = true;
								

			if (tabBar.SelectedItem.Tag == 2) {
				v.Hidden = false;
			}

			return v;
		}

		private void RequestImageUiGridView (object  state)
		{
			try {


				object[] on = state as object[];
				UIImageView controller = on [0] as UIImageView;
				galeriaImagenes imgmain = on [1] as galeriaImagenes;
				if (imgmain.urlString.Length > 0) {



					/*
                    NSUrl imageUrl = NSUrl.FromString (controller.Url);
                    NSData imageData = NSData.FromUrl (imageUrl);
                    if (_imgUrls == null)
                        _imgUrls = new List<BasicTableImageItem> ();
                    */
					UIImage imgRoom = ImageHotel (imgmain.urlString.Replace ("/", "").Replace ("-", "").Replace (".", "").Replace (",", "") + ".png", imgmain.urlString);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					//UIImage imgRoom = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

					if (imgRoom != null) {
						InvokeOnMainThread (delegate {
							controller.Hidden = false;
							controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							imgRoom.Dispose ();
							imgRoom = null;
							controller.ContentMode = UIViewContentMode.ScaleAspectFit; //aaa
							RefreshImageGal (controller);
							GC.Collect ();
						}
						);
						// imgRoom = null;
					}
				}
			} catch {


			}
		}
		#endregion
	}
}

