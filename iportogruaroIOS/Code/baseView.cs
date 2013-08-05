using System;

using System.Drawing;
using System.Collections.Generic;
using RedPlum;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using MonoTouch.Dialog;
using System.Threading;

namespace iportogruaroIOS
{
	public class baseView:UIViewController
	{
		#region cicle
		public static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
        public  bool UserInterfaceIdiomIsPhoneProp {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }
		public baseView ()
		{
		}

		
		public baseView (string nibName,object objec)
		{

		}
		public static bool IsTall {
			get { 
				return UIDevice.CurrentDevice.UserInterfaceIdiom 
					== UIUserInterfaceIdiom.Phone 
						&& UIScreen.MainScreen.Bounds.Height 
						* UIScreen.MainScreen.Scale >= 1136;
			}     
		}

		public override void ViewDidLoad ()
		{
			setstyle();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		#endregion

		#region color

		public UIColor headerColor = UIColor.FromRGB (152, 22, 22);
		UIColor defaultBackgroundColor =  UIColor.White;
		#endregion
		#region hud
		
		
		private LoadingHUDView _hud;
        private MBProgressHUD hud;
		public void starthud ()
		{
            /*
			_hud = new LoadingHUDView ("", "");
			this.View.AddSubview (_hud);
			_hud.StartAnimating ();
            */
               
             hud = new MBProgressHUD (base.View);
              hud.Mode = MBProgressHUDMode.Indeterminate;
               hud.TitleText = "iPortogruaro";
               hud.DetailText = "Loading...";
               this.View.AddSubview(hud);
                   hud.Show (true);


            this.NavigationItem.RightBarButtonItem = null;
		}
		
		public void stophud ()
		{
            /*
			if (_hud == null)
				return;
			_hud.StopAnimating ();
			_hud.RemoveFromSuperview ();
			_hud.Dispose ();
			_hud = null;
*/
            if (hud == null)
                return;
            hud.Hide(true);
            hud.RemoveFromSuperview ();
           // hud.Dispose ();
            hud = null;

            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Search,delegate {
                showSearchBox();
            });

		}

        public void stophud (bool sw)
        {
            /*
            if (_hud == null)
                return;
            _hud.StopAnimating ();
            _hud.RemoveFromSuperview ();
            _hud.Dispose ();
            _hud = null;
*/
            if (hud == null)
                return;
            hud.Hide(true);
            hud.RemoveFromSuperview ();
           // hud.Dispose ();
            hud = null;

           

        }
		
		#endregion

		#region style

		private void setstyle ()
		{
			try
			{
				//-moz-linear-gradient(#981616, #410909) repeat scroll 0 0 #6D1010
				this.NavigationController.NavigationBar.TintColor = headerColor;
				
				this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Search,delegate {
					showSearchBox();
				});

				this.View.BackgroundColor = defaultBackgroundColor;
			}
			catch
			{

			}
		}

		#endregion
        #region search
        private UIView sectionHeaderTitle (string title)
        {
            UIView header = new UIView (new System.Drawing.RectangleF (0, 0, 320, 28));
            
            UILabel lblsubtitle;// = new UILabel (new System.Drawing.RectangleF (30, 8, 100, 22));
            lblsubtitle = new UILabel () {
                Font = UIFont.FromName("HelveticaNeue-Bold", 18f),
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear
            };
            
            lblsubtitle.Frame = new System.Drawing.RectangleF (15, 1, 300, 20);
            
            lblsubtitle.Text = title;
            
            
            header.AddSubview (lblsubtitle);
            
            return header;
        }
        EntryElement searchBoxEntry;
        public void showSearchBox()
        {
            DialogViewController searchBoxDialog;
            
            searchBoxEntry = new EntryElement ("Cerca", "Digita parola:","");
            
            
            searchBoxDialog = new DialogViewController (new RootElement ("iportogruaro") {
                
                
                
                new Section (sectionHeaderTitle("Type in a keyword:")){
                    searchBoxEntry},
                
            });
            
            //flightNumerEntry.KeyboardType = UIKeyboardType.Default;
            
            searchBoxDialog.TableView.BackgroundView = null;
            searchBoxDialog.TableView.BackgroundColor = UIColor.White;
            UINavigationController nav = new UINavigationController (searchBoxDialog);
            
            nav.NavigationBar.TintColor = headerColor;
            
            this.PresentViewController (nav, true, null);
            searchBoxEntry.ReturnKeyType = UIReturnKeyType.Search;
            searchBoxEntry.ShouldReturn += delegate {
                starthud();
                
                
                ThreadPool.QueueUserWorkItem (state =>
                                              {
                    string key = "";
                    InvokeOnMainThread (delegate {
                        key = searchBoxEntry.Value;
                    });
                    var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorysSearch(key);
                    if (lst != null && lst.Count > 0 )
                    {
                        InvokeOnMainThread (delegate {
                            nav.DismissViewController (false, null);
                            this.NavigationController.PushViewController(new UiCategoryList(true){lstSearch = lst},true);
                            stophud();
                        }
                        );
                    }
                    
                });
                
                stophud();
                
                return true;
            };
            
            searchBoxDialog.NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Cerca", UIBarButtonItemStyle.Plain, (s,e) => {
                
                starthud();
                
                
                ThreadPool.QueueUserWorkItem (state =>
                                              {
                    string key = "";
                    InvokeOnMainThread (delegate {
                        key = searchBoxEntry.Value;
                    });
                    var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorysSearch(key);
                    if (lst != null && lst.Count > 0 )
                    {
                        InvokeOnMainThread (delegate {
                            nav.DismissViewController (false, null);
                            this.NavigationController.PushViewController(new UiCategoryList(true){lstSearch = lst},true);
                            stophud();
                        }
                        );
                    }
                    
                });
                
                stophud();
            });
            
            searchBoxDialog.NavigationItem.LeftBarButtonItem = new UIBarButtonItem ("Annulla", UIBarButtonItemStyle.Plain, (s,e) => {
                
                nav.DismissViewController (true, null);
            });
        }
        #endregion

	}
}

