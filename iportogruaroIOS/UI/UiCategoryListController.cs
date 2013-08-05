
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;
using iportogruaroLibraryShared;
using System.Collections.Generic;
using RedPlum;

namespace iportogruaroIOS
{
	public class UiCategoryListController : UITableViewController
	{
		#region "propierties"
		public string cat_id{get;set;}
		#endregion
		#region "cicle"
		public  bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public UiCategoryListController (bool serach) : base (UITableViewStyle.Plain)
		{
			modeSearch = true;
		}
		public UiCategoryListController () : base (UITableViewStyle.Plain)
		{
			modeSearch = false;
		}
		public bool modeSearch{get;set;}
		public List<iportogruarocategories> lstSearch{get;set;}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			setstyle();
			if (modeSearch == true)
			{
				TableView.Source = new sourceSecondCategory (this,lstSearch);
				TableView.ReloadData();
				setLoadingViewStyle(this.TableView);
			}
			else
			{

				starthud();
                var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
                this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (indicator);
                indicator.StartAnimating ();
                reloading = true;
				ThreadPool.QueueUserWorkItem (state =>
				                              {

					var lst = new iportogruaroLibraryShared.subCategorys().getSubCategorys(cat_id,true);

					InvokeOnMainThread (delegate {
						TableView.Source = new sourceSecondCategory (this,lst);
						TableView.ReloadData();
						setLoadingViewStyle(this.TableView);
                        reloading = false;
                        this.NavigationItem.LeftBarButtonItem = null;
						stophud();
					}
					);

				});
			}

		}
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
		
		#endregion
		#region color
		
		UIColor headerColor = UIColor.FromRGB (152, 22, 22);
		UIColor defaultBackgroundColor =  UIColor.White;
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
			
			
			
			searchBoxDialog = new DialogViewController (new RootElement ("iPortogruaro") {
				
				
				
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
                            this.NavigationController.PushViewController(new UiCategoryListController(true){lstSearch = lst},true);
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
                        this.NavigationController.PushViewController(new UiCategoryListController(true){lstSearch = lst},true);
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
        #region pull To Refresh
        private void setLoadingViewStyle (UITableView t)
        {
            vLoading = new UIView (new RectangleF (0, 0 - this.View.Bounds.Size.Height, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height));
            
            vLoading.BackgroundColor = UIColor.FromRGB (225, 235, 239);
            
            vLoading.Layer.MasksToBounds = true;
            vLoading.Layer.CornerRadius = 0.0f;
            
            
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
            
            this.TableView.AddSubview (vLoading);
        }
        
        private UIView vLoading{ get; set; }
        
        public UITableView tl {
            get {
                return this.TableView; 
            }
            set {
                this.TableView = value;    
            }
        }
        
        public bool reloading{ get; set; }
        public List<iportogruarocategories> lst{ get; set;}
        public void loadData ()
        {
            if (!InternetConnection.IsNetworkAvaialable(true))
            {
                using(var alert = new UIAlertView("iPortogruaro", "Spiacente nessun collegamento internet al momento", null, "OK", null))//Viajes Telcel//Aceptar
                {
                    alert.Show();
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
            this.TableView.ContentInset = UIEdgeInsets.Zero;
            UIView.CommitAnimations ();
            /*
                [UIView beginAnimations:nil context:NULL];
                [UIView setAnimationDuration:.3];
                [tblMain setContentInset:UIEdgeInsetsMake(0.0f, 0.0f, 0.0f, 0.0f)];
                [refreshHeaderView setStatus:kPullToReloadStatus];
                [refreshHeaderView toggleActivityView:NO];
                [UIView commitAnimations];
                */
			if (modeSearch == true)
			{
				TableView.Source = new sourceSecondCategory (this,lstSearch);
				TableView.ReloadData();
				setLoadingViewStyle(this.TableView);
			}
			else
			{
				starthud();
                //this.NavigationController.NavigationItem.HidesBackButton = true;
                var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
                this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (indicator);
                indicator.StartAnimating ();
				ThreadPool.QueueUserWorkItem (state =>
				                              {



					lst = new iportogruaroLibraryShared.subCategorys().getSubCategorys(cat_id,false);

                    var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
                    ad.loadpos();

                    if (lst == null)
                    {
                        InvokeOnMainThread (delegate {
                            using(var alert = new UIAlertView("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                            {
                                alert.Show();
                            }
                        }
                        );

                    }

                    if (lst.Count <= 0)
                    {
                        InvokeOnMainThread (delegate {
                            using(var alert = new UIAlertView("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                            {
                                alert.Show();
                            }
                        }
                        );

                    }

                    if (lst[0].cat_id  == "0")
                    {
                        InvokeOnMainThread (delegate {
                        using(var alert = new UIAlertView("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                        {
                            alert.Show();
                        }
                        }
                        );

                    }


					InvokeOnMainThread (delegate {
						TableView.Source = new sourceSecondCategory (this,lst);
						TableView.ReloadData();
						setLoadingViewStyle(this.TableView);
                        this.NavigationItem.LeftBarButtonItem = null;

						stophud();
						reloading = false;
					}
					);

				});
			}
			foreach (UIView vin in vLoading) {

				vin.RemoveFromSuperview ();
				vin.Dispose ();

			}
			vLoading.RemoveFromSuperview ();
			vLoading.Dispose ();
			vLoading = null;
			//startHud();
			//}

		}
		#endregion  
	}
}

