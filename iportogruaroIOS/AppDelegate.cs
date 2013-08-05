using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
//using MonoTouch.FacebookConnect;
using System.Json;
using System.Net;
using MonoTouch.CoreLocation;
using iportogruaroLibraryShared;
//using ParseLib;
using ParseTouch;

namespace iportogruaroIOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
    [Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
    {
		// class-level declarations
        UINavigationController navigationController;
        UIWindow window;
        const string AppId = "467505806653484";
        //Facebook facebook;
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		
        const string appid = "tloH1kWo9nWAbxcWiUz8GjFSHnjHOyEk84lOJ6Kt";
         const string clientid = "SgsRHhjWK8boRIYBNigquoys995OaQArP0OAR6Pr";
        const string ApiUrl = "https://parse.com/apps/new";
        const string ApiMessage = "A Parse API key is required to run this sample app.\nPlease sign up for one at:\n" + ApiUrl;
       
        
		public override void ReceiveMemoryWarning (UIApplication application)
		{
			GC.Collect ();
			/*
			navigationController.ViewControllers = new UIViewController[0];// = new UINavigationController (controller);

			var viewController = new UINavigationController (new UIHomeScreen());

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();
*/
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            window = new UIWindow (UIScreen.MainScreen.Bounds);

			System.Threading.Thread.Sleep (1500); //splash time
           
            Parse.SetAppId (appid, clientid);
            /*
            WithValidParseIds (delegate {
                Parse.SetApplicationIdClientKey (appid, clientid);

            });;
*/

            //loadData ();
			
            var controller = new UIHomeScreen ();

            int firstTime = NSUserDefaults.StandardUserDefaults.IntForKey ("firstTime");

            firstTime ++;

            NSUserDefaults.StandardUserDefaults.SetInt (firstTime, "firstTime");
            controller.firstTime = firstTime;

            navigationController = new UINavigationController (controller);
            window.RootViewController = navigationController;

            try
            {
            if (options.ContainsKey (new NSString ("UIApplicationLaunchOptionsRemoteNotificationKey"))) {

                NSDictionary aps = options.ObjectForKey (new NSString ("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;

                Console.WriteLine ("entro aqui...");

                string alert = "NADA";

                if (aps.ContainsKey (new NSString ("alert"))) {

                   Console.WriteLine ("trae alert aqui...");

                    alert = (aps [new NSString ("alert")] as NSString).ToString ();

                    alert = alert.Replace ("Nuovo evento creato: ", "");
            
                    app.ApplicationIconBadgeNumber = 0;

                    app.CancelAllLocalNotifications ();//.ca

                        controller.gotoEvent = true;
                        controller.keyEvent = alert;


                    var x = new UIAlertView ("iPortogruaro", "Un nuovo evento è stato creato vuoi vedere?", null, "Annullare", "Ok");
                    x.Show ();
                    
                        x.Clicked += (sender, buttonArgs) => {
                        int clicked = buttonArgs.ButtonIndex;

                        if (clicked == 1) {
                            var detail = new UIEventDetail (true);
                            detail.key = alert;
                            detail.Title = "Eventi";
                            this.navigationController.PushViewController (detail, true);
                            //ReceivedRemoteNotification (app, aps);
                        }

                    }; 


                }
                    else if (aps.ContainsKey (new NSString ("aps"))) {

                        Console.WriteLine("NSDictionary: 123"); 

                        NSDictionary aps2 = aps.ObjectForKey (new NSString ("aps")) as NSDictionary;

                        Console.WriteLine("NSDictionary: " + aps2.ToString());
                        alert = (aps2 [new NSString ("alert")] as NSString).ToString ();

                        alert = alert.Replace ("Nuovo evento creato: ", "");

                        Console.WriteLine(alert);

                        app.ApplicationIconBadgeNumber = 0;

                        app.CancelAllLocalNotifications ();//.ca

                        controller.gotoEvent = true;
                        controller.keyEvent = alert;
                        var detail = new UIEventDetail (true);
                        detail.key = alert;
                        detail.Title = "Eventi";
                        this.navigationController.PushViewController (detail, true);
                    }
                    else
                    {
                       /*
{
aps =     {
alert = "Nuovo evento creato: Evento Prova Gi\U00f2";
};
}
                        */ 



                        Console.WriteLine("NSDictionary: " + aps.ToString());

                        JsonValue value = JsonObject.Parse (aps.ToString());

                        Console.WriteLine(value.ToString());

                        JsonValue ar = value ["aps"];
                        string msg = "";

                        foreach (JsonValue v in ar) {
                        
                             msg = v ["alert"].ToString ().Replace("\"","");
                        

                            Console.WriteLine("json parse: " + msg);


                        }

                        controller.gotoEvent = true;
                        controller.keyEvent = msg;


                    }
            }
            }
            catch(Exception ex) {
                Console.WriteLine ("error geting the push notifications Error: "+ex.Message);


				if (controller == null)
				{
					 controller = new UIHomeScreen ();



					navigationController = new UINavigationController (controller);
					window.RootViewController = navigationController;
				}
            }
			//20130711 start
			if (window.RootViewController == null) {
				controller = new UIHomeScreen ();
				navigationController = new UINavigationController (controller);
				window.RootViewController = navigationController;
			}
			//20130711 end

            // make the window visible
            window.MakeKeyAndVisible ();

            //ValidateSetup ();
            //SetupUI ();
            GetLocation ();

            //string  newDeviceToken =  NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            //Console.WriteLine("Device Token: " + newDeviceToken);

            registerNotifications ();

            return true;
        }
        #region Data
		public void loadpos ()
		{
			loadpos (true);
		}
        public void loadpos (bool onlyimages)
        {
			if (onlyimages == false)
			{
           new iportogruaroLibraryShared.Mainiportogruaropos ().refreshData ();


                                         
                 new iportogruaroLibraryShared.mainEventos ().refreshData();

				new iportogruaroLibraryShared.mainNews ().refreshData (); //20130717

			}
			GC.Collect ();
			//return;
            ThreadPool.QueueUserWorkItem (state =>
                                          {

                Console.WriteLine("<<<<<<< IMAGES >>>>>>>>");

                var lst = new iportogruaroLibraryShared.mainCategorys().getAllCategorys(true);


                foreach(iportogruarocategories c in lst)
                {
                    var img = ImageHotel(c.icon_image.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", c.icon_image);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					if (img != null)
					{
					img.Dispose();
					img = null;
					}
                }
				lst = null;
				GC.Collect();
                var lstpos = new iportogruaroLibraryShared.Mainiportogruaropos().getAllPost();

                foreach(iportogruaropos c in lstpos)
                {
					var img = ImageHotelPos(c.icon.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", c.icon);

					if (img != null)
					{
						img.Dispose();
						img = null;
					}

					img = ImageHotel(c.photo.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+"detail_.png", c.photo);

					if (img != null)
					{
						img.Dispose();
						img = null;
					}
					/*
					 img = ImageHotel(c.icon.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", c.icon);

					if (img != null)
					{
						img.Dispose();
						img = null;
					}
*/
                    foreach (galeriaImagenes g in c.galerias) {
                        
						var img2 = ImageHotel(g.urlString.Replace("/","").Replace("-","").Replace(".","").Replace(",","")+".png", g.urlString);
						if (img2 != null)
						{
							img2.Dispose();
							img2 = null;
						}
					}
                }

				lstpos = null;
                Console.WriteLine("<<<<<<< END  IMAGES >>>>>>>>");
                //dowload and save the image

				GC.Collect();
            });
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


                        var res =resp.AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
						//02 Jul 2013
						resp.Dispose();
						resp = null;//12 Mb
						imageData.Dispose();
						imageData = null;
						GC.Collect();//cal

						//if (!res)
                          //  Console.WriteLine("Error: "+ err.LocalizedDescription ); 

                    }

                    catch (Exception ex) {

                       // Console.WriteLine (ex.ToString ());
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
		private  UIImage ImageHotelPos(string imgName,string url)
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


						var res =ResizeImage(resp,148,148).AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
						//02 Jul 2013
						resp.Dispose();
						resp = null;//12 Mb
						imageData.Dispose();
						imageData = null;
						GC.Collect();//cal

						//if (!res)
						//  Console.WriteLine("Error: "+ err.LocalizedDescription ); 

					}

					catch (Exception ex) {

						// Console.WriteLine (ex.ToString ());
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
        public void loadCat ()
        {
            ThreadPool.QueueUserWorkItem (state =>
                                          {
                new iportogruaroLibraryShared.mainCategorys().refreshData();
            });
        }

        private void loadData ()
        {

            if (!InternetConnection.IsNetworkAvaialable (true)) {
               
                return;
            }
			/*
            ThreadPool.QueueUserWorkItem (state =>

                                          {

               

                // new iportogruaroLibraryShared.mainCategorys().refreshData();
                new iportogruaroLibraryShared.Mainiportogruaropos().refreshData();
				GC.Collect();
                loadpos();
            });
*/


           
        }
        #endregion
		/*
        #region url Handle
        public override bool HandleOpenURL (UIApplication application, NSUrl url)
        {
            return facebook.HandleOpenURL (url);
        }
        // Post 4.2 callback
        public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            facebook.HandleOpenURL (url);
            return true;
        }
        #endregion
        */
        #region GPS
        public void GetLocation ()
        {
            try{
			if (!InternetConnection.IsNetworkAvaialable (true)) {

				return;
			}
            Util.RequestLocation (newLocation => {
                
                try
                {

                    
                    
                       
                        
                   
                        
                    /*
                     *  var defaults = NSUserDefaults.StandardUserDefaults;
            if (defaults ["tripchiusername"] != null) {
                username = defaults ["tripchiusername"].ToString ();// as NSString;
                lat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
                lon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");
                     * */
					System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo ("en-US");    
				
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo ("en-US");          
					
                    double lat = double.Parse(newLocation.Coordinate.Latitude.ToString ().Replace(",","."));
                    double lon =  double.Parse(newLocation.Coordinate.Longitude.ToString ().Replace(",","."));
                NSUserDefaults.StandardUserDefaults.SetString (lat.ToString(), "Latitude");
                NSUserDefaults.StandardUserDefaults.SetString (lon.ToString(), "Longitude");
                NSUserDefaults.StandardUserDefaults.Synchronize ();
                
                }
                catch(Exception ex)
                {
						Console.WriteLine("Errog GPS"+ ex.ToString()); //20130711
					}
            }
            );
			}
			catch(Exception exs)
			{
				Console.WriteLine("Errog Main GPS"+ exs.ToString()); //20130711
			}
        }
        #endregion
        #region push Notifications
        /*
        void WithValidParseIds (Action act)
        {
            if (string.IsNullOrEmpty (appid) || string.IsNullOrEmpty (clientid)) {
                window.AddSubview (new MissingApiView (window.Bounds, ApiMessage, ApiUrl));
            } else {
                act ();
            }
        }
        */
        public void registerNotifications ()
        {
            UIApplication.SharedApplication.RegisterForRemoteNotificationTypes (UIRemoteNotificationType.Alert
                                                                               | UIRemoteNotificationType.Badge
                                                                               | UIRemoteNotificationType.Sound);
        }

        public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
        {
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey ("PushDeviceToken");

            //There's probably a better way to do this
            var strFormat = new NSString ("%@");
            var dt = new NSString (MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(new MonoTouch.ObjCRuntime.Class("NSString").Handle, new MonoTouch.ObjCRuntime.Selector("stringWithFormat:").Handle, strFormat.Handle, deviceToken.Handle));
            var newDeviceToken = dt.ToString ().Replace ("<", "").Replace (">", "").Replace (" ", "");

            if (string.IsNullOrEmpty (oldDeviceToken) || !deviceToken.Equals (newDeviceToken)) {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            //Save device token now
            NSUserDefaults.StandardUserDefaults.SetString (newDeviceToken, "PushDeviceToken");

            //var wc = new WebClient();
            //registrationId="199";

            //userLogin u = controllerUserLogin.getuserLogin();
            /*
            string myParameters = "iddev="+newDeviceToken+"&uname=iphone5Hipolyto&iduser=1024";
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            
             var result = wc.UploadString("http://www.letsolutions.com.mx/pushTestSpotlight/submit.aspx", "POST",myParameters);
*/
            // ParseLib.ParsePush..StoreDeviceToken (deviceToken);

            Console.WriteLine ("Device Token: " + newDeviceToken);

            ParsePush.StoreDeviceToken (deviceToken);
            ParsePush.SubscribeToChannelAsync ("events");


            // vtmService.registerDevice(newDeviceToken,int.Parse(u.userId));

        }

        public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
        {
            ParsePush.HandlePush (userInfo);

            NSDictionary aps = userInfo.ObjectForKey (new NSString ("aps")) as NSDictionary;

            string alert = "";

            if (aps.ContainsKey (new NSString ("alert")))
                alert = (aps [new NSString ("alert")] as NSString).ToString ();

            alert = alert.Replace ("Nuovo evento creato: ", "");
            var detail = new UIEventDetail (true);
            detail.key = alert;
            detail.Title = "Eventi";
            this.navigationController.PushViewController (detail, true);

           
            application.ApplicationIconBadgeNumber = 0;

            application.CancelAllLocalNotifications ();//.ca

        }

        public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
        {
            Console.WriteLine ("Failed to register for notifications");
           
                

        }
        #endregion
    }
}

