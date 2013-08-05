
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

    public class posItem : UITableViewCell
    {
        UILabel headingLabel;
        UILabel distance;
        UIImageView imageView;
		UIImageView imageViewDistance;
		UIView viewline;
		UIView imageViewBorder;
		public iportogruaropos userMember{ get; set; }
		public bool isIphone{ get; set;}
        public posItem (string cellId,bool isIpad) : base (UITableViewCellStyle.Default, cellId)
        {

            SelectionStyle = UITableViewCellSelectionStyle.None;
            ContentView.BackgroundColor = UIColor.Clear;// .FromRGB (171,201,213);
            imageView = new UIImageView ();

            //20130623imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
            //20130623imageView.Layer.BorderWidth = 1.0f;


			imageViewDistance = new UIImageView ();
            //float heightText = 12.5f;
            float heightText = 16f;
            if (!isIpad)
                heightText = 18f;


			float heightTextDistance = 12.5f;
			if (!isIpad)
				heightTextDistance = 14f;


            headingLabel = new UILabel () {
                Font = UIFont.FromName("TrebuchetMS-Bold",heightText),
                TextColor = UIColor.FromRGB(174,65,61),
                BackgroundColor = UIColor.Clear
            };

            distance = new UILabel () {
                //Font = UIFont.FromName("TrebuchetMS-Bold", 10.5f),
				Font = UIFont.FromName("TrebuchetMS-Bold", heightTextDistance),
                TextColor = UIColor.DarkGray,
                BackgroundColor = UIColor.Clear,
                TextAlignment = UITextAlignment.Right
            };

			viewline = new UIView ();
			viewline.BackgroundColor = UIColor.LightGray;// .FromRGB (123, 187, 219);

			//20130628 bordo esterno
			imageViewBorder = new UIView ();
			imageViewBorder.BackgroundColor = UIColor.Clear;//.Yellow;
			imageViewBorder.Layer.BorderWidth = 1.0f;
			imageViewBorder.Layer.BorderColor = UIColor.LightGray.CGColor;
			imageViewBorder.Layer.MasksToBounds = true;

			ContentView.Add (headingLabel);
            ContentView.Add (distance);

            ContentView.Add (imageView);

			ContentView.Add (viewline);

			ContentView.Add (imageViewDistance);

			ContentView.Add (imageViewBorder); //20130628
        }

        public void UpdateCell (string caption)
        {


			imageViewDistance.Image = UIImage.FromFile("images/icons/km.png"); 
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            if (userMember.icon == null)
            {
				imageView.Image = UIImage.FromFile("images/DefaultPos.png");
            }
            else{
                UIImage imgRoom = geImgPos (userMember.icon.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png");
                if (imgRoom != null)
				{
					imageView.ContentMode = UIViewContentMode.ScaleAspectFill; //20130628 era. Center;
                    imageView.Image =imgRoom;
				}
                else
                {
                    imageView.Image = UIImage.FromFile("images/DefaultPos.png");
					imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    if (userMember != null) {

                        object[] on = new object[2];
                        on [0] = imageView;
                        on [1] = userMember;
                        System.Threading.ThreadPool.QueueUserWorkItem (RequestImage, on);   

                        //System.Threading.ThreadPool.QueueUserWorkItem (setImageStatus, userMember);   

                    }

                }
            }

            headingLabel.Text = System.Web.HttpUtility.HtmlDecode(caption);


			//Console.WriteLine("icona: " + userMember.icon);
			imageViewDistance.Hidden = true;
			distance.Hidden = true;
            try{
                if (userMember.lat ==  null)
                    return;

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
				imageViewDistance.Hidden = false;
				distance.Hidden = false;
				distance.Text = (Math.Round( distan / 1000.0,2)).ToString("N2") + " Km";
            }
            catch{
				imageViewDistance.Hidden = true;
				distance.Hidden = true;
            }
            // btnletsGo.AddTarget (this, new Selector ("HandleBotontListLiked"), UIControlEvent.TouchUpInside);

        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            /*
            imageView.Frame = new RectangleF (5, 3, 35, 35);

            headingLabel.Frame = new RectangleF (55, 5, ContentView.Bounds.Width - 55, 20);
*/
			imageViewBorder.Frame = new RectangleF (4, 4, 80, 80); //20130628
            imageView.Frame = new RectangleF (4+3, 4+3, 74, 74); //20130628 era 4,4,43,43
		

            headingLabel.Frame = new RectangleF (105, 13, ContentView.Bounds.Width - 140, 40);
			headingLabel.Lines = 2;
			//headingLabel.SizeToFit ();
			viewline.Frame = new RectangleF (0, 90, ContentView.Bounds.Width, 1f);

			distance.TextAlignment = UITextAlignment.Left;
			distance.Frame = new RectangleF (125, 55, 200, 20);
			imageViewDistance.Frame= new RectangleF (105, 58, 15, 15);//icon for distance -> position
        
		
			if (isIphone == false)
			{
				distance.TextAlignment = UITextAlignment.Right;
				headingLabel.Frame = new RectangleF (105, 40, ContentView.Bounds.Width - 105, 20);
				headingLabel.Lines = 1;
				distance.Frame = new RectangleF (ContentView.Bounds.Width - 215, 40, 200, 20);
				imageViewDistance.Frame= new RectangleF (ContentView.Bounds.Width - 115, 40, 20, 20);//icon for distance -> position

			}
		
			//distance.Frame = new RectangleF (ContentView.Bounds.Width - 220, 40, 200, 20);
			//imageViewDistance.Frame= new RectangleF (ContentView.Bounds.Width - 105, 40, 15, 15);//icon for distance -> position

		}
		/*
		 * 1.- The image should be big
		 * 2.- the name of the pos, shoube be putend more to the right
		 * 
		 * Iphone:
		 * 3.- the label should be set in to lines when the are necesarry
		 * 4.- the distance and distance image shulb be beloww the name
		 * 
		 * Ipad:
		 * 4.- the distance and the image distance staty the same
		 * 
		 * */
        private void RequestImage (object  state)
        {
            try {

                object[] on = state as object[];
                UIImageView controller = on [0] as UIImageView;
                iportogruaropos imgmain = on [1] as iportogruaropos;

                if (imgmain == null)
                    return;

                if (imgmain.icon == null)
                    return;

                if (imgmain.icon.Length > 0) {

                    // NSUrl imageUrl = NSUrl.FromString (imgmain.stringUrl);
                    // NSData imageData = NSData.FromUrl (imageUrl);

                    //BEM_Chain.UvCache.ImageHotel (imgmain.HotelParameters.PropertyNumber + imgmain.Room.RoomCode+ "_g.png", imgmain.ImageName);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    //  string urlImg =  imgmain.stringUrl;

                    UIImage imgRoom = ImageHotel(imgmain.icon.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", imgmain.icon);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    //UIImage imgRoom = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                    if (imgRoom != null) {
                        InvokeOnMainThread (delegate {
                            controller.Image = imgRoom;//SpCache.ImageUser (imgmain.UserId.ToString()+ "_g.png", urlImg);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
							controller.ContentMode = UIViewContentMode.ScaleAspectFill; //20130628 era Center
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

						//InvokeOnMainThread (delegate {
						    //20130628 era 37x37 messo 74x74
						    
							var res =ResizeImage(resp,148,148).AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
							if (!res)
								Console.WriteLine("Error: "+ err.LocalizedDescription ); 

						//01 Jul 2013
						resp = null;//12 Mb
						imageData.Dispose();
						imageData = null;
						resp = UIImage.FromFile (sCachedPath); 
						//}
						//); 

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

        private  UIImage geImgPos(string imgName)
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
        private void RefreshImage (UIImageView controller)
        {

            try {

                //Console.WriteLine("<<<<<<  Actualiza Imagen >>>>>>>>>");
                var animation = CABasicAnimation.FromKeyPath ("opacity");
                animation.From = NSNumber.FromFloat (0);
                animation.To = NSNumber.FromFloat (1);
                animation.Duration = .2;
                //animation.Delegate = new MyAnimationDelegate (controller, true);
                //AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;

                controller.Layer.AddAnimation (animation, "moveToHeader");
            } catch {
            }

        }

    }

}
