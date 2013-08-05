
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
    public partial class UiCategoryList : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public  bool UserInterfaceIdiomIsPhoneProp {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        #region "propierties"
        public string cat_id{get;set;}
        #endregion
        #region "cicle"
       
        public UiCategoryList ()
            : base (UserInterfaceIdiomIsPhone ? "UiPosListiPhone" : "UiCategoryList_iPad", null)
        {
            modeSearch = false;
        }

        public UiCategoryList (bool serach)
            : base (UserInterfaceIdiomIsPhone ? "UiPosListiPhone" : "UiCategoryList_iPad", null)
        {
            modeSearch = true;
        }
		/*
       public void loadInterface()
		{

			tblMain.ReloadData();
			setLoadingViewStyle(this.tblMain);
			reloading = false;
		}
		*/
		/*
		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			UIViewController parent = this.NavigationController.ViewControllers [this.NavigationController.ViewControllers.Length - 2];

			if (parent is UiCategoryList)
			{
				(parent as UiCategoryList).loadInterface ();
			}

			base.WillRotate (toInterfaceOrientation, duration);
		}
*/
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
                tblMain.Source = new sourceSecondCategoryCustom (this,lstSearch);
                tblMain.ReloadData();
                setLoadingViewStyle(this.tblMain);
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
                        tblMain.Source = new sourceSecondCategoryCustom (this,lst);
                        tblMain.ReloadData();
                        setLoadingViewStyle(this.tblMain);
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
			return;
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
        
        UIColor headerColor = UIColor.FromRGB (152, 22, 22); //barra in alto
		//UIColor headerColor = UIColor.FromRGB (30, 40, 50);
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
				// image_up
				// image_down
				UITabBarItem home = new UITabBarItem ("Home",UIImage.FromFile("images/BarraHome.png"),0);
				home.SetFinishedImages(UIImage.FromFile("images/mainBar/Barra_Home.png"),UIImage.FromFile("images/mainBar/Barra_Home.png"));
                UITabBarItem Servizi = new UITabBarItem ("Servizi",null,1);//130
				Servizi.SetFinishedImages(UIImage.FromFile("images/mainBar/Barra_Servizi.png"),UIImage.FromFile("images/mainBar/Barra_Servizi.png"));


                UITabBarItem NumeriUtili = new UITabBarItem ("Numeri Utili",null,2);//166

				NumeriUtili.SetFinishedImages(UIImage.FromFile("images/mainBar/Barra_Numeri_Utili.png"),UIImage.FromFile("images/mainBar/Barra_Numeri_Utili.png"));


                UITabBarItem Eventi = new UITabBarItem ("Eventi",null,3);

				Eventi.SetFinishedImages(UIImage.FromFile("images/mainBar/Barra_Eventi.png"),UIImage.FromFile("images/mainBar/Barra_Eventi.png"));


                UITabBarItem[] arrayA = new UITabBarItem[] { home,Servizi,NumeriUtili,Eventi };

                tabBar.Items = arrayA;

                tabBar.TintColor =  UIColor.FromRGB (152, 22, 22); //bottom bar
				//tabBar.TintColor =  UIColor.FromRGB (30, 40, 50);;

                if (!UserInterfaceIdiomIsPhone)
                {
					/*
                    UITextAttributes titleTextAttributes = new UITextAttributes();
                    titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 20);
                    tabBar.Items[0].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[1].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[2].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[3].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    */
                }
                else
                {
					/*
                    UITextAttributes titleTextAttributes = new UITextAttributes();
                    titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 18);
                    tabBar.Items[0].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[1].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[2].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    tabBar.Items[3].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
                    */
                }
				UITextAttributes titleTextAttributes = new UITextAttributes();
				//titleTextAttributes.Font = UIFont.FromName("TrebuchetMS-Bold", 18);
				titleTextAttributes.TextColor = UIColor.White;
				tabBar.Items[0].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
				tabBar.Items[1].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
				tabBar.Items[2].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);
				tabBar.Items[3].SetTitleTextAttributes(titleTextAttributes,UIControlState.Normal);

				tabBar.Items [0].Title = "Home";
                tabBar.Items [1].Title = "Servizi";
                tabBar.Items [2].Title = "Numeri utili";
                tabBar.Items [3].Title = "Eventi";
				/*
				tabBar.Items [0].Title = "";
				tabBar.Items [1].Title = "";
				tabBar.Items [2].Title = "";
				tabBar.Items [3].Title = "";
				*/
                // myMap = new UiMapScreen (){dataPos = dataPos};

                tabBar.ItemSelected +=  delegate(object sender, UITabBarItemEventArgs e) {

                    switch(e.Item.Tag)
                    {
                        case 0:
                        this.NavigationController.PopToRootViewController(true);
                        break;
                        case 1:
                    {
                        this.NavigationController.PushViewController(new UiCategoryList(){cat_id = "130",Title = "Servizi"},true);

                    }
                        break;
                        case 2:
                    {

                        this.NavigationController.PushViewController(new UiCategoryList(){cat_id = "166",Title = "Numeri Utili"},true);

                    }
                        break;
                        case 3:
                    {
                        this.NavigationController.PushViewController(new UiEventsListController(){Title = "Eventi"},true);

                    }
                        break;
                    }
                };

				tblMain.BackgroundView = null;
				tblMain.BackgroundColor = UIColor.Clear;
				tblMain.SeparatorStyle = UITableViewCellSeparatorStyle.None;
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
        #region pull To Refresh
        private void setLoadingViewStyle (UITableView t)
        {
            return;
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
            
            this.tblMain.AddSubview (vLoading);
        }
        
        private UIView vLoading{ get; set; }
        
        public UITableView tl {
            get {
                return this.tblMain; 
            }
            set {
                this.tblMain = value;    
            }
        }
        
        public bool reloading{ get; set; }
        public List<iportogruarocategories> lst{ get; set;}
        public void loadData()
        {
            int clicked = -1;
            var x = new UIAlertView ("iPortogruaro", "Si conferma l'aggiornamento dal server? L'operazione potrebbe durare alcuni secondi.", null, "Annullare", "Aggiornare");
            x.Show ();
            bool done = false;
            x.Clicked += (sender, buttonArgs) => {

                clicked = buttonArgs.ButtonIndex;
                if (clicked == 1)
                    loadDataServer ();
            };    
        }

        public void loadDataServer ()
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
            this.tblMain.ContentInset = UIEdgeInsets.Zero;
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
                tblMain.Source = new sourceSecondCategoryCustom (this,lstSearch);
                tblMain.ReloadData();
                setLoadingViewStyle(this.tblMain);
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
                    NSUserDefaults.StandardUserDefaults.SetInt (1, "pulltoRefresh");
                   

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
                        tblMain.Source = new sourceSecondCategoryCustom (this,lst);
                        tblMain.ReloadData();
                        setLoadingViewStyle(this.tblMain);
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

