
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iportogruaroLibraryShared;
using MonoTouch.CoreAnimation;
using MonoTouch.MessageUI;
using MonoTouch.Social;

namespace iportogruaroIOS
{
    public partial class UiGaleryScreen : baseView
    {
        #region propierties
        public iportogruaropos dataPos{get;set;}
        public int positionImg{ get; set;}
		UiDetailScreen controller{ get; set;}
        #endregion
		/*
        public UiGaleryScreen () : base ("UiGaleryScreen", null)
        {
        }
        */
		public UiGaleryScreen (UiDetailScreen _controller) : base (UserInterfaceIdiomIsPhone ? "UiGaleryScreenIpad" : "UiGaleryScreenIpad", null)
		{
			controller = _controller;
		}

        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
            
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();


			UIButton btnShare = UIButton.FromType (UIButtonType.Custom);
			UIImage imgShare = UIImage.FromFile ("images/ic_title_share_default.png");

			btnShare.TouchUpInside += delegate {
			
				if(UserInterfaceIdiomIsPhone)
					this.loadcomboTaks();
				else
					loadpop(btnShare);
				return;
				//20130708 popup facebook, twitter e mail
				foreach (UIView v in sramdom) {
					if (v.Tag == positionImg) {
						if (v is UIImageView) {
							UIImageView imgs = v as UIImageView;
							//controller.shareFb(imgs.Image);
							//controller.loadcomboTaks();
						}
						break;
					}
				}
			};
			btnShare.SetImage (imgShare, UIControlState.Normal);
			btnShare.Frame = new RectangleF (0, 0, 30, 30);
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem (btnShare);

            if (UserInterfaceIdiomIsPhone && IsTall)
            {
               this.View.Frame = new RectangleF (0,0,320,560);

				setScroll(this.View.Bounds.Width,this.View.Bounds.Height );
            }
			else if (UserInterfaceIdiomIsPhone && !IsTall)
			{
				this.View.Frame = new RectangleF (0,0,320,460);

				setScroll(this.View.Bounds.Width,this.View.Bounds.Height );
			}
            else if(!UserInterfaceIdiomIsPhone)
            {
			
				switch(InterfaceOrientation)
				{
				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					this.View.Frame = new RectangleF (0,0,1024,768 );
					break;
				default:
					this.View.Frame = new RectangleF (0,0,768,1024 );
					break;
				}
               
				setScroll(this.View.Bounds.Width,this.View.Bounds.Height );
            }
            

            if (positionImg > 0) {
            
                //var pc = (UIPageControl)sender; 
                double fromPage = Math.Floor ((sramdom.ContentOffset.X - sramdom.Frame.Width / 2) / sramdom.Frame.Width) + 1;                    
                var toPage = positionImg; 
                var pageOffset = sramdom.Frame.Width * toPage;
                if (fromPage > toPage) 
                    pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
                PointF p = new PointF (pageOffset, 0); 
                sramdom.SetContentOffset (p, true); 
            }

            // Perform any additional setup after loading the view, typically from a nib.
        }
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
                        setScroll(568,320);
                    else
                        setScroll(480,320);
                    
                  //  loadMainCategory();
                    break;
                default:
                    if (IsTall)
                        setScroll(320,568);
                    else
                        setScroll(320,480);
                    //loadMainCategory();
                    break;
                }
            }
            else
            {


				switch(toInterfaceOrientation)
				{
					case UIInterfaceOrientation.LandscapeLeft:
					case UIInterfaceOrientation.LandscapeRight:
					this.View.Frame = new RectangleF (0,0,1024,768 );
					break;
					default:
					this.View.Frame = new RectangleF (0,0,768,1024 );
					break;
				}

                switch (toInterfaceOrientation) {
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    this.NavigationItem.LeftBarButtonItem = null;
                    // chiudo il menu popover se aperto
                    
                    setScroll(1024,768);
					//setScroll(768,1024);
                    //loadMainCategory();
                    break;
                default:
                    setScroll(768,1024);
                    //loadMainCategory();
                    break;
                }


				UIViewController c = this.NavigationController.ViewControllers [this.NavigationController.ViewControllers.Length -2];


				if (c is UiDetailScreen)
				{
					(c as UiDetailScreen).WillRotate (toInterfaceOrientation, duration);// .checkrotation ();
				}

            }
			if (positionImg > 0) {

				//var pc = (UIPageControl)sender; 
				double fromPage = Math.Floor ((sramdom.ContentOffset.X - sramdom.Frame.Width / 2) / sramdom.Frame.Width) + 1;                    
				var toPage = positionImg; 
				var pageOffset = sramdom.Frame.Width * toPage;
				if (fromPage > toPage) 
					pageOffset = sramdom.ContentOffset.X - sramdom.Frame.Width; 
				PointF p = new PointF (pageOffset, 0); 
				sramdom.SetContentOffset (p, true); 
			}
            
        }
		#region


		UIPickerView pkrShare;
		UIToolbar BarraShare;
		bool showComboTaks = false;
		private string[] shareList;

		public UIPopoverController detailparameters { get; set; }

		public UIButton LastTappedButton { get; set; }

		UINavigationController popviewNavigate;

		private void loadpop (NSObject sender)
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
		public int UpdatecurrenShareOpt=0;
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

		public void donecomboShare ()
		{


			hidecomboShare ();
			string key = shareList [UpdatecurrenShareOpt];

			Console.WriteLine(key + " Llave: " + UpdatecurrenShareOpt);

			switch (key) {
				case "facebook":
			{
				SLComposeViewController slComposer;

				if (SLComposeViewController.IsAvailable (SLServiceKind.Facebook)) {
					slComposer = SLComposeViewController.FromService (SLServiceType.Facebook);
					slComposer.SetInitialText ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode(dataPos.title));


					foreach (UIView v in sramdom) {
						if (v.Tag == positionImg) {
							if (v is UIImageView) {
								UIImageView imgs = v as UIImageView;
								slComposer.AddImage (imgs.Image);
							}
							break;
						}
					}

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
				}

				else
				{
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
					slComposer.SetInitialText ("Condiviso tramite iPortogruaro. Scaricala da iTunes! " + System.Web.HttpUtility.HtmlDecode(dataPos.title));
					foreach (UIView v in sramdom) {
						if (v.Tag == positionImg) {
							if (v is UIImageView) {
								UIImageView imgs = v as UIImageView;
								slComposer.AddImage (imgs.Image);
							}
							break;
						}
					}
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
				}
				else
				{
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
					_mail.SetMessageBody ("Condiviso tramite iPortogruaro. Scaricala da iTunes! "+ System.Web.HttpUtility.HtmlDecode(dataPos.title), 
					                      false);
					_mail.SetSubject (System.Web.HttpUtility.HtmlDecode(dataPos.title));
					_mail.Finished += HandleMailFinished;

					foreach (UIView v in sramdom) {
						if (v.Tag == positionImg) {
							if (v is UIImageView) {
								UIImageView imgs = v as UIImageView;
								NSData dat = imgs.Image.AsJPEG (0);
								_mail.AddAttachmentData (dat, "image/png", System.Web.HttpUtility.HtmlDecode(dataPos.title));
							}
							break;
						}
					}
						
					this.PresentModalViewController (_mail, true);

				} else {
					// handle not being able to send mail
				}
				break;
			}
			}

		}
		bool dontHadPhoto = true;
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
        #region mainScroll
		private UIPageControl pageControl ;
        private UIScrollView sramdom;
        private float heightscroll {get;set;}
        private float widthscroll {get;set;}

        private void setScroll (float w,float h)
        {   
            heightscroll = h-45;//this.View.Bounds.Height;
            widthscroll = w;//this.View.Bounds.Width;;
            
            if (sramdom != null)
            {
                foreach(UIView v in sramdom)
                {
                    v.RemoveFromSuperview();
                    v.Dispose();
                }
                
                sramdom.RemoveFromSuperview();
                sramdom.Dispose();
                sramdom = null;
            }
            
			if (pageControl != null)
			{
				pageControl.RemoveFromSuperview ();
				pageControl.Dispose ();
				pageControl = null;
			}

            sramdom = new UIScrollView (new RectangleF (0, 0, widthscroll, heightscroll));
            
            try {
                
                
                if (dataPos.galerias.Count <= 0)
                {

                    UIImageView imgViewMainImage = new UIImageView (UIImage.FromFile("images/NOIMG.png"));
                    imgViewMainImage.Frame = this.View.Frame;
					imgViewMainImage.ContentMode = UIViewContentMode.ScaleAspectFit;
                    imgViewMainImage.UserInteractionEnabled = true;
                    this.View.AddSubview(imgViewMainImage);
                    return;
                }
                
                // UIImage image;
				var sw = this.View.Bounds.Width;

				//if (w == 1024)
				//	w = 850;

                 pageControl = new UIPageControl (new RectangleF (0, heightscroll - 100, w, 20));
				pageControl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth ;
				// vistaImagenes = new UIView (new RectangleF (0, 0, 320 , 367));
                //MyScrollView = new UIScrollView (new RectangleF (0, 0,320 , 367));
				//pageControl.BackgroundColor = UIColor.Gray;//  .Clear ;         
                RectangleF scrollFrame = sramdom.Frame;    
                //MyScrollView.BackgroundColor =UIColor.Green ; 
                //if (gal.Count <= 5)
                scrollFrame.Width = scrollFrame.Width * dataPos.galerias.Count;
                //else
                //  scrollFrame.Width = scrollFrame.Width * 5;
                
                sramdom.Scrolled += delegate(object sender, EventArgs e) {
                    try {
                        
                        double page = Math.Floor ((sramdom.ContentOffset.X - sramdom.Frame.Width / 2) / sramdom.Frame.Width) + 1;
                        pageControl.CurrentPage = (int)page;
						positionImg = (int)page;
                        
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
                
                
                pageControl.Pages = dataPos.galerias.Count ;
                //  else
                //      pageControl.Pages = 5;//maximo 5 imagenes
                
                
                
                sramdom.BackgroundColor = base.headerColor;
                
                
                sramdom.PagingEnabled = true;
                sramdom.ContentSize = scrollFrame.Size;                
                sramdom.ShowsHorizontalScrollIndicator = false;
                //sramdom.AutoresizingMask = UIViewAutoresizing.All;
                RectangleF frame = sramdom.Frame;
                frame.X = 0;
                int i = 0;
                foreach(galeriaImagenes g in dataPos.galerias)


                {
                    System.Drawing.RectangleF Fr = new System.Drawing.RectangleF (frame.Width * i, 0,widthscroll, heightscroll); 
                   
                    

                    
                    
                    
                    UIImageView imgViewMainImage = new UIImageView ();//(UIImage.FromFile("images/"+namebigPicture));
                    imgViewMainImage.Frame = Fr;
					imgViewMainImage.ContentMode = UIViewContentMode.ScaleAspectFill;// .ScaleToFill;
                    imgViewMainImage.UserInteractionEnabled = true;
							imgViewMainImage.Tag = i;
							sramdom.InsertSubview (imgViewMainImage, 0);
                    
                   
                    object[] on = new object[2];
                    on [0] = imgViewMainImage;
                    on [1] = g;
                    System.Threading.ThreadPool.QueueUserWorkItem (RequestImage, on);   
                    
                    
                    
                    i++;
                    
                    
                }
                this.View.AddSubview (sramdom);
                this.View.AddSubview (pageControl);
                
                
                
                
                
                
            } catch {
            }
            
        }
        
        #endregion
        #region galery
        private void NoImage(UIImageView imgView)
        {
            InvokeOnMainThread (delegate {
                imgView.Image = UIImage.FromFile("images/NOIMG.png");;; //   UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
				imgView.ContentMode = UIViewContentMode.ScaleAspectFit;
                RefreshImage (imgView);
            }
            );
        }
        private void RequestImage (object  state)
        {
            try {
                
                object[] on = state as object[];
                UIImageView controller = on [0] as UIImageView;
                galeriaImagenes imgmain = on [1] as galeriaImagenes;
                
                if (imgmain == null)
                {
                    NoImage(controller);
                    return;
                }
                if (imgmain.urlString == null)
                {
                    NoImage(controller);
                    return;
                }
                
                if (imgmain.urlString.Length > 0) {
                    
                    // NSUrl imageUrl = NSUrl.FromString (imgmain.stringUrl);
                    // NSData imageData = NSData.FromUrl (imageUrl);
                    
                    //BEM_Chain.UvCache.ImageHotel (imgmain.HotelParameters.PropertyNumber + imgmain.Room.RoomCode+ "_g.png", imgmain.ImageName);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    //  string urlImg =  imgmain.stringUrl;
                    
                    UIImage imgRoom = ImageHotel (imgmain.urlString.Replace("/","").Replace("-","").Replace(".","").Replace(",","") + ".png", imgmain.urlString);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                    if (imgRoom != null) {
                        InvokeOnMainThread (delegate {
                            controller.Hidden = false;
                            controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							controller.ContentMode = UIViewContentMode.ScaleAspectFit;
                            RefreshImage (controller);
                        }
                        );
                        // imgRoom = null;
                    }
                }
            } catch (Exception ex) {
                
                Console.WriteLine (ex.ToString ());
            }
        }
        private  UIImage ImageHotel(string imgName,string url)
        {
            try
            {
                UIImage resp;
                string sCachedPath =System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal  ),  imgName);
                resp = UIImage.FromFile(sCachedPath);   
                
                //Console.WriteLine (sCachedPath); 
                
                if (resp == null && url.Length >0)
                {
                    //Console.WriteLine("No se encontro cache imagenes");  
                    
                    NSUrl imageUrl = NSUrl.FromString (url);
                    NSData imageData = NSData.FromUrl (imageUrl);
                    
                    resp = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    
                    try
                    {
                        NSError err = new NSError(new NSString("http://www.univisit.com"),0); 
                        
						InvokeOnMainThread (delegate {
							var res =resp.AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
							if (!res)
								Console.WriteLine("Error: "+ err.LocalizedDescription ); 
							//01 Jul 2013
							resp = null;//12 Mb
							imageData.Dispose();
							imageData = null;
							GC.Collect();//cal
							resp = UIImage.FromFile (sCachedPath);
						}
						);
                        
                    }
                    
                    catch (Exception ex) {
                        
                        Console.WriteLine (ex.ToString ());
                        return null;
                    }
                    
                }
                else
                {
                    //  Console.WriteLine("se carga cache imagenes");  
                }
                
                return resp ;
            }
            catch (Exception ex) {
                
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
                animation.Duration = .1;
                //animation.Delegate = new MyAnimationDelegate (controller, true);
                //AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;
                
                controller.Layer.AddAnimation (animation, "moveToHeader");
            } catch {
            }
            
        }
        #endregion

    }
}

