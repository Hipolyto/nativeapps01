
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using System.Collections.Generic;
using MonoTouch.CoreLocation;
using iportogruaroLibraryShared;

namespace iportogruaroIOS
{
    public partial class UiMapScreen : baseView
    {
        public UiMapScreen () : base ("UiMapScreen", null)
        {
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
            starthud();
            showMapa(true);
            stophud();
            // Perform any additional setup after loading the view, typically from a nib.
        }
        #region propierties
        public iportogruaropos dataPos{get;set;}
        #endregion
        #region map
        MKMapView map;
        UIView vmap;
        private void hideMapa(bool hide)
        {
            
        }
        private void showMapa (bool doanimation)
        {
            double zoom = 5f;
            
            double latMain = Convert.ToDouble(dataPos.lat);
            double lonMain = Convert.ToDouble(dataPos.lon);
            List<MyAnnotation> pins = null;
            
            try {
                
                //Console.WriteLine ("Entra carga mapa"); 
                if (map != null)
                    hideMapa (false);
                
                
                
                map = new MKMapView ();// (new RectangleF (0, 0, 400f, 300f ));
                map.Frame = this.View.Bounds;
                map.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                //??
                map.Region = new MonoTouch.MapKit.MKCoordinateRegion (new CLLocationCoordinate2D (latMain, lonMain), new MKCoordinateSpan (zoom, zoom));
                
                vmap = new UIView (new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height ));
                vmap.AutosizesSubviews = true;
                vmap.BackgroundColor =  UIColor.FromRGBA(0.0f, 0.0f, 0.0f, 0.75f);
                vmap.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                
                
                
                vmap.AddSubview (map); 
                View.AddSubview (vmap);  
                //this.View.AddSubview (v); 
                
                
                
                
                
                
                
                
                double lat = latMain;
                double lon = lonMain;
                if (pins == null)
                    pins = new List<MyAnnotation> ();
                
                pins.Add (new MyAnnotation (new CLLocationCoordinate2D (lat, lon), "hotel.Name", "hotel.Adress", MKPinAnnotationColor.Purple, null));
                
                
                
                
                
                
                
                map.MapType = MonoTouch.MapKit.MKMapType.Standard;
                
                
                map.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (latMain, lonMain), new MKCoordinateSpan (zoom, zoom));
                
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
            map.ShowsUserLocation = false;  // shows the "blue dot" user location (if available)
            //map.UserLocation.Title = Language.GetString ("BEM00125"); 
            //map.UserLocation.Subtitle = Language.GetString ("BEM00126");  
            
            
            
            map.DidFailToLocateUser += delegate(object sender, NSErrorEventArgs e) {
                
            };
            
            map.LoadingMapFailed += delegate(object sender, NSErrorEventArgs e) {
                //Tools.ShowAlert("TODO: Lo sentimos el mapa no puede ser cargado en este momento");
                
               
            };  
            
            foreach (MyAnnotation hot in pins) {
                if (hot.Coordinate.IsValid ())  
                    map.AddAnnotationObject (hot);
                
            }
            
            
            
            map.CenterCoordinate = new CLLocationCoordinate2D (latMain, lonMain);
            map.Region = new MKCoordinateRegion (new CLLocationCoordinate2D (latMain, lonMain), new MKCoordinateSpan (.1, .1));
            map.ShowsUserLocation = true;  // shows the "blue dot" user location (if available)
           
        }
        #endregion
    
       
    }
}

