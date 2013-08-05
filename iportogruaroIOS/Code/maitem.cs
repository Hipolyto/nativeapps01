

using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoTouch.CoreAnimation;
using System.Drawing;
using MonoTouch.CoreLocation;

using MonoTouch.MapKit;
namespace iportogruaroIOS
{
    public class MyAnnotation : MKAnnotation
    {
        public CLLocationCoordinate2D coordinate;
        private string title, subtitle;
        private MKPinAnnotationColor color;
        private UIButton btn;
        
        public override CLLocationCoordinate2D Coordinate {
            get { return coordinate; }
            set { coordinate = value;}
        }
        
        public override string Title {
            get { return title; }
        }
        
        public override string Subtitle {
            get { return subtitle; }
        }
        /// <summary>
        /// Custom property to use when displaying on the map,
        /// not part of the MKAnnotation protocol
        /// </summary>
        public MKPinAnnotationColor Color {
            get { return color; }
        }
        
        public UIButton btnSelect
        { get { return btn; } }
        
        public bool active { get; set; }
        
        public UIViewController app{ get; set; }
        
        
        
        /// <summary>
        /// The custom constructor is required to pass the values to this class,
        /// because in the MKAnnotation base the properties are read-only
        /// </summary>
        public MyAnnotation (CLLocationCoordinate2D l, string t, string s, MKPinAnnotationColor c, object h)
        {
            coordinate = l;
            title = t;
            subtitle = s;
            color = c;
            
            btn = UIButton.FromType (UIButtonType.DetailDisclosure);
            active = true;
            try {
                btn.TouchUpInside += delegate {
                    
                    //goHotel ();
                };
            } catch {
                active = false;
            }
            
        }
        
        public MyAnnotation (CLLocationCoordinate2D l, string t, string s, MKPinAnnotationColor c)
        {
            coordinate = l;
            title = t;
            subtitle = s;
            color = c;
            active = false;
            
            
        }
        
        public MyAnnotation (bool error)
        {
            active = error;
            
        }
    }
}

