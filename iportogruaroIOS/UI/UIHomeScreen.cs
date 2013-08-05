
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using iportogruaroLibraryShared;
using MonoTouch.CoreAnimation;
using MapWithRoutes;

namespace iportogruaroIOS
{
	public partial class UIHomeScreen : baseView
	{
		#region cicle
        private bool timerstarted { get; set;}
        public int firstTime { get; set;}
		public UIHomeScreen () : base ("UIHomeScreen", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

        void loadMap ()
        {
			return;
            try {

                var _MapView = new MapView(new RectangleF(0, 0,10, 10));
                _MapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                _MapView.Hidden = true;



                View.AddSubview (_MapView); 

            } catch {
            }
        
        }


		public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);
			CurrentPage = 1;
			try{
            var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
            ad.GetLocation ();

            int pulltoRefresh = NSUserDefaults.StandardUserDefaults.IntForKey ("pulltoRefresh");

            if (pulltoRefresh == 1)
            {
                NSUserDefaults.StandardUserDefaults.SetInt (0, "pulltoRefresh");
                base.starthud();



                reloading = true;
                ThreadPool.QueueUserWorkItem (state =>
                                              {
                    var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorys(true);



                    InvokeOnMainThread (delegate {


                        tableCategory.Source = new sourceMainCategory(this,lst);
                        tableCategory.ReloadData();
                        reloading = false;
                        base.stophud();
                    }
                    );

                });
            }
            else
            {

				bool islandCape = false;
            if (!UserInterfaceIdiomIsPhone) {
                int iniheight = 110;
                int iniweight = 320;
                switch (InterfaceOrientation) {
					case UIInterfaceOrientation.LandscapeLeft:
					case UIInterfaceOrientation.LandscapeRight:
                    //iniheight = 240;
						iniheight = 290;
						iniweight = 1024;
						islandCape = true;
                    break;
                    default:
                    iniweight = 1024;
                    iniheight = 290;
                    break;
                }
            
            setScroll(iniweight,iniheight);
					loadMainCategoryOrientation(islandCape);
           
            
            
            }
            }
			}
			catch(System.Exception ex){
				Console.WriteLine (ex.ToString ());
			}
        }
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            timerstarted = false;

            this.Title = "iPortogruaro";
			int iniheight = 110;
			int iniweight = 320;
			if (UserInterfaceIdiomIsPhone)
			{
			switch (InterfaceOrientation) {
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
					if (IsTall)
					{

						iniheight = 568;
						iniweight = 100;
					}
					else
					{
						iniheight = 480;
						iniweight = 150;
					}
				break;
			default:
					iniheight = 110;
					iniweight = 320;
				break;
			}
			}
			else
			{
				switch (InterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					iniweight = 1024;
					//iniheight = 240;
					iniheight = 290;
					break;
				default:
					iniweight = 1024;
                    iniheight = 290;
					break;
				}
			}
			setScroll(iniweight,iniheight);
            loadMainCategory();
            loadMap ();

            if (firstTime > 1 && !gotoEvent) {
            
                int clicked = -1;
                var x = new UIAlertView ("iPortogruaro", "Si conferma l'aggiornamento dal server? L'operazione potrebbe durare alcuni secondi.", null, "Annullare", "Aggiornare");
                x.Show ();
                bool done = false;
                x.Clicked += (sender, buttonArgs) => {
                  
                    clicked = buttonArgs.ButtonIndex;
                    if (clicked == 1)
					{
						reloading = false;
                        loadDataServer ();
					}
                };    
               

           
            } else if (gotoEvent) {
            
                var detail = new UIEventDetail (true);
                detail.key = keyEvent;
                detail.Title = "Eventi";
                this.NavigationController.PushViewController (detail, true);
            
            }



			// Perform any additional setup after loading the view, typically from a nib.
		}

        public bool gotoEvent{ get; set;}
        public string keyEvent { get; set;}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			

			if (UserInterfaceIdiomIsPhone)
			{
			switch (toInterfaceOrientation) {
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
				this.NavigationItem.LeftBarButtonItem = null;
				// chiudo il menu popover se aperto
				if (IsTall)
					setScroll(568,100);
				else
					setScroll(480,150);

				loadMainCategory();
				break;
			default:
				setScroll(320,110);
				loadMainCategory();
				break;
			}
			}
			else
			{
				bool islandCape = false;
				switch (toInterfaceOrientation) {
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.NavigationItem.LeftBarButtonItem = null;
					// chiudo il menu popover se aperto
					islandCape = true;
                    //setScroll(1024,240);
					setScroll(1024,290);
					loadMainCategory();
					break;
				default:
                    setScroll(1024,290);
					loadMainCategory();
					break;
				}
				loadMainCategoryOrientation(islandCape);
			}
			
		}

		#endregion

		#region mainScroll

        private void automaticScroll()
        {
            if (timerstarted)
                return;
            timerstarted = true;
            setTimer ();
        }

        private void setTimer()
        {
            NSTimer.CreateScheduledTimer(new TimeSpan(0, 0, 10),delegate 
                                         {

                try{
                    

					if (CurrentPage-1 < 0 )
						CurrentPage = 0;
					if (CurrentPage == 5)
						CurrentPage = 0;

						double fromPage = CurrentPage -1;   
                    var toPage = CurrentPage; 

                    
                    var pageOffset = sramdom.Frame.Width * toPage;
                    if (fromPage > toPage) 
                        pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
                    PointF p = new PointF (pageOffset, 0); 
                    sramdom.SetContentOffset (p, true); 

                    CurrentPage++;
                }
                catch{
                    var toPage = 0; 
                    var pageOffset = sramdom.Frame.Width * toPage;

                    pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
                    PointF p = new PointF (pageOffset, 0); 
                    sramdom.SetContentOffset (p, true); 
                    CurrentPage = 1;
                }

                setTimer();
            });
        }

        private int CurrentPage = 1;
		private UIScrollView sramdom;
		private int heightscroll {get;set;}
		private int widthscroll {get;set;}
		private void setScroll (int Widthscroll,int Heightscroll)
		{   
			heightscroll = Heightscroll;
			widthscroll = Widthscroll;
            bool settimer = true;
            if (sramdom != null)
			{
                settimer = false;

				foreach(UIView v in sramdom)
				{
					v.RemoveFromSuperview();
					v.Dispose();
				}

				sramdom.RemoveFromSuperview();
				sramdom.Dispose();
				sramdom = null;
			}
			
			sramdom = new UIScrollView (new RectangleF (0, 0, Widthscroll, heightscroll));
			
			try {
				
				
				
				
				// UIImage image;
				
                //UIPageControl pageControl = new UIPageControl (new RectangleF (0, 20 , 320, 20));
                UIPageControl pageControl = new UIPageControl (new RectangleF (0, heightscroll - 20, Widthscroll, 20));
				// vistaImagenes = new UIView (new RectangleF (0, 0, 320 , 367));
				//MyScrollView = new UIScrollView (new RectangleF (0, 0,320 , 367));
				//vistaImagenes.BackgroundColor = UIColor.Gray;//  .Clear ;         
				RectangleF scrollFrame = sramdom.Frame;    
				//MyScrollView.BackgroundColor =UIColor.Green ; 
				//if (gal.Count <= 5)
				scrollFrame.Width = scrollFrame.Width * 5;
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
                    Console.WriteLine(toPage.ToString());
					var pageOffset = sramdom.Frame.Width * toPage;
					if (fromPage > toPage) 
						pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
					PointF p = new PointF (pageOffset, 0); 
					sramdom.SetContentOffset (p, true); 
					//btnaddItinerary.SetTitle ("+ Bookmark", UIControlState.Normal);
					
				};
				
               
				pageControl.Hidden = false;
				
				
				pageControl.Pages = 5;
				//  else
				//      pageControl.Pages = 5;//maximo 5 imagenes
				

				
				sramdom.BackgroundColor = UIColor.FromRGB (114, 105, 116);


				sramdom.PagingEnabled = true;
				sramdom.ContentSize = scrollFrame.Size;                
				sramdom.ShowsHorizontalScrollIndicator = false;
				//sramdom.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
				RectangleF frame = sramdom.Frame;
				frame.X = 0;
				
				for (int i = 0; i < 5; i++) {
					
					System.Drawing.RectangleF Fr = new System.Drawing.RectangleF (frame.Width * i, 0,Widthscroll, heightscroll); 
					string namebigPicture = "img_1.png";
					
					switch (i) {
					case 0:
						namebigPicture = "img_1.png";
						break;
					case 1:
						namebigPicture = "img_2.png";
						break;
					case 2:
						namebigPicture = "img_3.png";
						break;
					case 3:
						namebigPicture = "img_4.png";
						break;
					case 4:
						namebigPicture = "img_5.png";
						break;
					}
					
					
					
					
					UIImageView imgViewMainImage = new UIImageView (UIImage.FromFile("images/"+namebigPicture));
					imgViewMainImage.Frame = Fr;
						imgViewMainImage.ContentMode = UIViewContentMode.ScaleToFill;
					imgViewMainImage.UserInteractionEnabled = true;
                  sramdom.AddSubview (imgViewMainImage);
					
					

					

					

					
				}
               
				this.View.AddSubview (sramdom);
                //this.View.AddSubview (pageControl);
                if (settimer)
               automaticScroll();

				


				
			} catch {
			}
			
		}

#endregion

		#region mainCategory
		private UITableView tableCategory;
		public void loadMainCategory ()
		{


			if (tableCategory != null)
			{
				foreach(UIView v in tableCategory)
				{
					v.RemoveFromSuperview();
					v.Dispose();
				}
				
				tableCategory.RemoveFromSuperview();
				tableCategory.Dispose();
				tableCategory = null;
			}
			tableCategory = new UITableView(new RectangleF (0,   0,0,1),UITableViewStyle.Plain);

			/*
			if (UserInterfaceIdiomIsPhone)
			{
			if (!IsTall)
			{
				switch (heightscroll) {
				case 150:
					tableCategory = new UITableView(new RectangleF (0,   heightscroll + 2, widthscroll, 120),UITableViewStyle.Plain);
					break;
				default:
					tableCategory = new UITableView(new RectangleF (0,   heightscroll +2 , widthscroll, 310),UITableViewStyle.Plain);
					break;
				}

			}
			else
			{
				switch (heightscroll) {
				case 150:
					tableCategory = new UITableView(new RectangleF (0,   heightscroll + 2, widthscroll, 100),UITableViewStyle.Plain);
					break;
                    case 100:
                        tableCategory = new UITableView(new RectangleF (0,   heightscroll + 2, widthscroll, 165),UITableViewStyle.Plain);
                        break;
				default:
					tableCategory = new UITableView(new RectangleF (0,   heightscroll + 2, widthscroll, 400),UITableViewStyle.Plain);
					break;
				}
				//tableCategory = new UITableView(new RectangleF (0,  heightscroll, widthscroll, 300),UITableViewStyle.Plain);
			}
			}
			else
			{
			
				switch (InterfaceOrientation) {
					case UIInterfaceOrientation.LandscapeLeft:
					case UIInterfaceOrientation.LandscapeRight:
					switch (heightscroll) {
						case 240:
						tableCategory = new UITableView(new RectangleF (0,   heightscroll + 2, widthscroll, 200),UITableViewStyle.Plain);
						break;
						default:
						tableCategory = new UITableView(new RectangleF (0,   heightscroll, widthscroll, 300),UITableViewStyle.Plain);
						break;
					}
					break;
					default:
					switch (heightscroll) {
						case 240:
						tableCategory = new UITableView(new RectangleF (0,   heightscroll, widthscroll, 500-40),UITableViewStyle.Plain);
						break;
						default:
						tableCategory = new UITableView(new RectangleF (0,   heightscroll, widthscroll, 700-40),UITableViewStyle.Plain);
						break;
					}
					break;
				}

			}
			*/
            setLoadingViewStyle(tableCategory);
			tableCategory.BackgroundView = null;//new UIImageView (UIImage.FromFile ("images/bg-flightscreen.png"));
			tableCategory.BackgroundColor = UIColor.White;
			tableCategory.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			

			

			this.View.AddSubview (tableCategory);   
			base.starthud();
            reloading = true;
			ThreadPool.QueueUserWorkItem (state =>
			                              {
				var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorys(true);

               new iportogruaroLibraryShared.Mainiportogruaropos().getAllPost(true);

				var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
				ad.loadpos(true);

				InvokeOnMainThread (delegate {
					try
					{
                    tableCategory.Source = new sourceMainCategory(this,lst);
                    tableCategory.ReloadData();
					reloading = false;
					base.stophud();
					}
					catch(System.Exception ex)
					{
						Console.WriteLine(ex.ToString());
						reloading = false;
						base.stophud();
					}
				}
				);
				
			});
			
			if (UserInterfaceIdiomIsPhone)
				loadMainCategoryOrientation (false);
		}
		public void loadMainCategoryOrientation (bool islandCape)
        {

			UIView headerLine = new UIView ();
			headerLine.BackgroundColor = UIColor.FromRGB(174,65,61);//UIColor.DarkGray;//UIColor.Clear;// .FromRGB(174,65,61);//UIColor.LightGray;// .FromRGB (123, 187, 219);
			View.AddSubview (headerLine);
			//line above the slide
			float K_Line = 4f;
			headerLine.Frame = new RectangleF (0, sramdom.Frame.Height, widthscroll, K_Line);
		

			switch (islandCape) {

				case true:
				switch (heightscroll) {
					case 240:
					//tableCategory.Frame = new RectangleF (0,   heightscroll , widthscroll, 400);
					tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line, widthscroll, 400 - K_Line);
					break;
					default:
					tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line , widthscroll, 400- K_Line) ;
					break;
				}
				break;
				default:
				switch (heightscroll) {
					case 240:
					tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line, widthscroll, 460 - K_Line);
					break;
				case 110:
				{
						if (IsTall) {
						tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line, widthscroll, 397 - K_Line);
						}
					else
						tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line, widthscroll, 300 - K_Line);
					break;
				}
					default:
					tableCategory.Frame = new RectangleF (0,   heightscroll + K_Line, widthscroll, 700-35-K_Line);
					break;
				}
				break;
			}

                


          


        }
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
            
            tableCategory.AddSubview (vLoading);
        }
        
        private UIView vLoading{ get; set; }
        
        public UITableView tl {
            get {
                return tableCategory; 
            }
            set {
                tableCategory = value;    
            }
        }
        
        public bool reloading{ get; set; }
        public void loadData()
        {
            int clicked = -1;
            var x = new UIAlertView ("iPortogruaro", "Si conferma l'aggiornamento dal server? L'operazione potrebbe durare alcuni secondi?", null, "Annullare", "Aggiornare");
            x.Show ();
            bool done = false;
            x.Clicked += (sender, buttonArgs) => {

                clicked = buttonArgs.ButtonIndex;
                if (clicked == 1)
				{
					reloading = false;
                    loadDataServer ();
				}
            };    
        }
        public void loadDataServer ()
        {



            if (!InternetConnection.IsNetworkAvaialable(true))
            {
                using(var alert = new UIAlertView ("iPortogruaro", "Spiacente nessun collegamento internet al momento", null, "OK", null))//Viajes Telcel//Aceptar
                {
                    alert.Show();
                }
				reloading = false;
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
            tableCategory.ContentInset = UIEdgeInsets.Zero;
            UIView.CommitAnimations ();
            /*
                [UIView beginAnimations:nil context:NULL];
                [UIView setAnimationDuration:.3];
                [tblMain setContentInset:UIEdgeInsetsMake(0.0f, 0.0f, 0.0f, 0.0f)];
                [refreshHeaderView setStatus:kPullToReloadStatus];
                [refreshHeaderView toggleActivityView:NO];
                [UIView commitAnimations];
                */
            base.starthud();
            ThreadPool.QueueUserWorkItem (state =>
                                          {

               
				var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorys(false);

                var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
                ad.loadpos(false);


                if (lst == null)
                {
                    InvokeOnMainThread (delegate {
                        using(var alert = new UIAlertView ("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                        {
                            alert.Show();
                        }
                    }
                    );
					reloading = false;
                }

                if (lst.Count <= 0)
                {
                    InvokeOnMainThread (delegate {
                        using(var alert = new UIAlertView ("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                        {
                            alert.Show();
                        }
                    }
                    );
					reloading = false;
                }

                if (lst[0].cat_id  == "0")
                {
                    InvokeOnMainThread (delegate {
                        using(var alert = new UIAlertView ("iPortogruaro", "Impossibile ricevere gli aggiornamenti dal server remoto", null, "OK", null))//Viajes Telcel//Aceptar
                        {
                            alert.Show();
                        }
                    }
                    );
					reloading = false;
                }
                
                InvokeOnMainThread (delegate {
                    
                    
                    tableCategory.Source = new sourceMainCategory(this,lst);
                    tableCategory.ReloadData();
                    setLoadingViewStyle(tableCategory);
                    base.stophud();
                    reloading = false;
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
#endregion
	}
}

