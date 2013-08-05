
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
    
    public class newsItemCell : UITableViewCell
    {
        UILabel headingLabel;
        
        UIImageView imageView;
        public iportogruarocategories userMember{ get; set; }
        
		public newsItemCell (string cellId,bool isIpad) : base (UITableViewCellStyle.Default, cellId)
        {
            
            SelectionStyle = UITableViewCellSelectionStyle.None;
            ContentView.BackgroundColor = UIColor.Clear;// .FromRGB (171,201,213);
            
            //imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
            //imageView.Layer.BorderWidth = 1.0f;
            
            float heightText = 12.5f;
            
            if (!isIpad)
                heightText = 18f;
            
            headingLabel = new UILabel () {
                Font = UIFont.FromName("TrebuchetMS-Bold", heightText),
                TextColor = UIColor.FromRGB(174,65,61),
                BackgroundColor = UIColor.Clear,
				Lines = 3

            };
            
            ContentView.Add (headingLabel);
            
        }
        
        public void UpdateCell (string caption)
        {
            headingLabel.Text = System.Web.HttpUtility.HtmlDecode(caption);
            
            // btnletsGo.AddTarget (this, new Selector ("HandleBotontListLiked"), UIControlEvent.TouchUpInside);
            
        }
        
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            
            headingLabel.Frame = new RectangleF (5, 10, ContentView.Bounds.Width - 10, 50);
            
        }
        
    }
}
