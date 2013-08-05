

using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using System.Drawing;
using MonoTouch.CoreGraphics;


using MonoTouch.MapKit; 
using MonoTouch.CoreLocation;
using System.Linq ;
using MonoTouch.ObjCRuntime;

using System.Threading;
using System.Threading.Tasks;
using iportogruaroLibraryShared;


namespace iportogruaroIOS
{
    public class mainItem : UITableViewCell
    {
		public bool drawHeadeLine{ get; set;}
        UILabel headingLabel;
		UIView viewline;
        UIImageView imageView;
		UIImageView imageViewDistance;
        public iportogruarocategories userMember{ get; set; }





        public mainItem (string cellId,bool isIpad) : base (UITableViewCellStyle.Default, cellId)
        {

            SelectionStyle = UITableViewCellSelectionStyle.None;
            ContentView.BackgroundColor = UIColor.Clear;// .FromRGB (171,201,213);
            imageView = new UIImageView ();
			imageViewDistance = new UIImageView ();
            //imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
            //imageView.Layer.BorderWidth = 1.0f;
            //float heightText = 12.5f;
            float heightText = 16f;

            if (!isIpad)
                heightText = 18f;

            headingLabel = new UILabel () {
                Font = UIFont.FromName("TrebuchetMS-Bold", heightText),
                TextColor = UIColor.FromRGB(174,65,61),
                BackgroundColor = UIColor.Clear
            };

			distance = new UILabel () {
				//Font = UIFont.FromName("TrebuchetMS-Bold", 10.5f),
				Font = UIFont.FromName("TrebuchetMS-Bold", 12.5f),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right
			};

			viewline = new UIView ();
			viewline.BackgroundColor = UIColor.LightGray;// .FromRGB (123, 187, 219);

            ContentView.Add (headingLabel);

            ContentView.Add (imageView);

			ContentView.Add (distance);
			ContentView.Add (viewline);
			ContentView.Add (imageViewDistance);
        }
		UILabel distance;
        public void UpdateCell (string caption)
        {
			imageViewDistance.Image = UIImage.FromFile("images/icons/km.png"); 
			imageViewDistance.Hidden = true;

            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            headingLabel.Text = System.Web.HttpUtility.HtmlDecode(caption);
			foundImg = false;



            if (userMember.parent == "POS")
                imageView.Image = UIImage.FromFile("images/DefaultPos.png");
			else if (userMember.isPresentAsCategory )
				imageView.Image = UIImage.FromFile("images/DefaultPos.png");

            if (userMember.icon_image == null) {

                if (userMember.parent == "POS")
					imageView.Image = UIImage.FromFile("images/DefaultPos.png");
				else if (userMember.isPresentAsCategory )
					imageView.Image = UIImage.FromFile("images/DefaultPos.png");
                else
                    imageView.Image = UIImage.FromFile("images/icons/DefaultIcon.png");
            } else {

                UIImage imgRoom = geImgCat(userMember.icon_image.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png");// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                if (imgRoom != null)
				{
					foundImg = true;
                    imageView.Image = imgRoom;
				}
                else {
					foundImg = false;
					imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    if (userMember.parent == "POS")
						imageView.Image = UIImage.FromFile("images/DefaultPos.png");
					else if (userMember.isPresentAsCategory )
						imageView.Image = UIImage.FromFile("images/DefaultPos.png");
                    else
                        imageView.Image = UIImage.FromFile("images/icons/DefaultIcon.png");
                    if (userMember != null) {
						imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                        object[] on = new object[2];
                        on [0] = imageView;
                        on [1] = userMember;
                        System.Threading.ThreadPool.QueueUserWorkItem (RequestImage, on);   

                        //System.Threading.ThreadPool.QueueUserWorkItem (setImageStatus, userMember);   

                    }
                }

				if (imgRoom != null)
				{
					imgRoom.Dispose ();
					imgRoom = null;
				}
            }


            Console.WriteLine(userMember.icon_image);
			distance.Hidden = true;
			if (userMember.isPresentAsCategory)
			{
			try
			{
				string strlat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
				string strlon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");

				System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo ("en-US");    
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo ("en-US");          

				double lat = double.Parse(strlat);
				double lon = double.Parse(strlon);
				CLLocation origenDevice = new CLLocation (lat,lon);

				double userlat = double.Parse(userMember.lat);
				double userlon = double.Parse(userMember.lon);

				CLLocation point = new CLLocation (userlat,userlon);

				// CLLocationDistance distanceKey = new CLLocationDistance(); //= origenDevice.DistanceFrom(point);

				double distan = origenDevice.DistanceFrom(point);

				//CLLocationDistance kilometers = distanceKey / 1000.0;
				// or you can also use this..
				//CLLocationDistance meters = distanceKey;

					distance.Text = (Math.Round( distan / 1000.0,2)).ToString("N2") + " Km";

				distance.Hidden = false;
				imageViewDistance.Hidden = false;

			}
			catch {
				distance.Hidden = true;
			}
			}

            // btnletsGo.AddTarget (this, new Selector ("HandleBotontListLiked"), UIControlEvent.TouchUpInside);

        }



		private bool foundImg{ get; set;}
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            /*
            imageView.Frame = new RectangleF (5, 3, 35, 35);

            headingLabel.Frame = new RectangleF (55, 10, ContentView.Bounds.Width - 55, 20);
*/

            imageView.Frame = new RectangleF (4, 3, 43, 43);

			if (foundImg)
			{
			if (userMember.parent == "POS" || userMember.isPresentAsCategory)
			{
				imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
				imageView.Layer.BorderWidth = 1.0f;
				imageView.ContentMode = UIViewContentMode.Center;
			}
			}
			else

			{
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				if (userMember.parent == "POS" || userMember.isPresentAsCategory)
				{
					imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
					imageView.Layer.BorderWidth = 1.0f;

				}
			}
            headingLabel.Frame = new RectangleF (58, 13, ContentView.Bounds.Width - 55, 20);

			
			distance.Frame = new RectangleF (ContentView.Bounds.Width - 220, 28, 200, 20);

			//before -> 0.5
			viewline.Frame = new RectangleF (0, 48, ContentView.Bounds.Width, 1f);
			/*
			if (drawHeadeLine)
			{
				UIView headerLine = new UIView ();
				headerLine.BackgroundColor = UIColor.LightGray;// .FromRGB (123, 187, 219);

			

			ContentView.Add (headerLine);

			headerLine.Frame = new RectangleF (0, 0, ContentView.Bounds.Width, 3f);
			}
			*/
			imageViewDistance.Frame= new RectangleF (ContentView.Bounds.Width - 105, 31, 15, 15);//icon for distance -> position

        }



        private void RequestImage (object  state)
        {
            try {


                object[] on = state as object[];
                UIImageView controller = on [0] as UIImageView;
                iportogruarocategories imgmain = on [1] as iportogruarocategories;

                if (imgmain == null)
                    return;

                if (imgmain.icon_image == null)
                    return;

                if (imgmain.icon_image.Length > 0) {




                    // NSUrl imageUrl = NSUrl.FromString (imgmain.stringUrl);
                    // NSData imageData = NSData.FromUrl (imageUrl);

                    //BEM_Chain.UvCache.ImageHotel (imgmain.HotelParameters.PropertyNumber + imgmain.Room.RoomCode+ "_g.png", imgmain.ImageName);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    //  string urlImg =  imgmain.stringUrl;

                    UIImage imgRoom = ImageHotel(imgmain.icon_image.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", imgmain.icon_image);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    //UIImage imgRoom = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                    if (imgRoom != null) {
                        InvokeOnMainThread (delegate {
                            controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							foundImg = true;
							imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
							imageView.Layer.BorderWidth = 1.0f;
							imageView.ContentMode = UIViewContentMode.Center;
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

        private  UIImage geImgCat(string imgName)
        {
            try
            {
                UIImage resp;
                string sCachedPath =System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal  ),  imgName);
                resp = UIImage.FromFile(sCachedPath);   

                //Console.WriteLine (sCachedPath); 

                if (resp == null)
                {
                    return null;
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

					if (imageData == null)
						return null;
						resp = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});


                    

                    try
                    {
                        NSError err = new NSError(new NSString("http://www.univisit.com"),0); 

					//InvokeOnMainThread (delegate {

						int newvalue = 43;

						if (userMember.parent == "POS" || userMember.isPresentAsCategory)
							newvalue = 37;

						var res =ResizeImage(resp,newvalue,newvalue).AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
                        if (!res)
                            Console.WriteLine("Error: "+ err.LocalizedDescription ); 
					//	}
					//	);
						//01 Jul 2013
						resp = null;//12 Mb
						imageData.Dispose();
						imageData = null;
						resp = UIImage.FromFile (sCachedPath);   
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

		public static UIImage ResizeImage(UIImage source, float width, float height)
		{


			if (source == null)
			{
				return source;
			}

			float imageWidth = source.Size.Width;
			float imageHeight = source.Size.Height;

			float newImageHeight = height;
			float newImageWidth = width;

			if (width == -1 && height != -1)
			{
				newImageWidth = imageWidth / (imageHeight / newImageHeight);
			} else if (width != -1 && height == -1)
			{
				newImageHeight = imageHeight / (imageWidth / newImageWidth);
			}

			try {
				UIGraphics.BeginImageContext(new System.Drawing.SizeF(newImageWidth, newImageHeight));
				source.Draw(new System.Drawing.RectangleF(0,0,newImageWidth, newImageHeight));
				return UIGraphics.GetImageFromCurrentImageContext();
			} finally 
			{
				UIGraphics.EndImageContext();
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



    }

}

