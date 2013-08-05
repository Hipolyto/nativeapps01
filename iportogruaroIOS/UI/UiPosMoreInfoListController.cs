
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;
using iportogruaroLibraryShared;
using System.Collections.Generic;

namespace iportogruaroIOS
{
    
    public class UiPosMoreInfoListController : UITableViewController
    {
        #region "propierties"
        public string cat_id{get;set;}
        #endregion
        #region "cicle"
        public  bool UserInterfaceIdiomIsPhone {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }
       
        public UiPosMoreInfoListController (iportogruaropos _item) : base (UITableViewStyle.Plain)
        {
            item = _item;
        }
        public iportogruaropos item{get;set;}
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
           
                
                starthud();
                ThreadPool.QueueUserWorkItem (state =>
                                              {
                   
                    InvokeOnMainThread (delegate {
                    TableView.Source = new sourceMoreInfo (this,item);
                        TableView.ReloadData();
                        setLoadingViewStyle(this.TableView);
                        stophud();
                    }
                    );
                    
                });

            
        }
        #endregion
        #region hud
        
        private LoadingHUDView _hud;
        public void starthud ()
        {
            _hud = new LoadingHUDView ("", "");
            this.TableView.AddSubview (_hud);
            _hud.StartAnimating ();
        }
        
        public void stophud ()
        {
            if (_hud == null)
                return;
            _hud.StopAnimating ();
            _hud.RemoveFromSuperview ();
            _hud.Dispose ();
            _hud = null;
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
        
        public void loadData ()
        {
            //if (!reloading) {
            
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

                starthud();
                ThreadPool.QueueUserWorkItem (state =>
                                              {
                   
                    
                    InvokeOnMainThread (delegate {
                    TableView.Source = new sourceMoreInfo (this,item);
                        TableView.ReloadData();
                        setLoadingViewStyle(this.TableView);
                        stophud();
                    }
                    );
                    
                });

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
