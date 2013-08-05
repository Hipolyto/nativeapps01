
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

    public class moreinfoItem : UITableViewCell
    {
        UILabel headingLabel;
        UILabel deatilLabel;
      
        public iportogruaropos userMember{ get; set; }
        
        public moreinfoItem (string cellId,bool isIpad) : base (UITableViewCellStyle.Default, cellId)
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
                TextColor =  UIColor.FromRGB(174,65,61),
                BackgroundColor = UIColor.Clear
            };

             heightText = 10.5f;
            
            if (isIpad)
                heightText = 12f;

            deatilLabel = new UILabel () {
                Font = UIFont.FromName("Trebuchet MS", heightText),
                TextColor = UIColor.DarkGray,
                BackgroundColor = UIColor.Clear
            };
            
            ContentView.Add (headingLabel);
            ContentView.Add (deatilLabel);
           
        }
        


        public void UpdateCell (string caption,int row)
        {

            headingLabel.Text = System.Web.HttpUtility.HtmlDecode(caption);
            
           switch(row)
            {
            case 0:
                headingLabel.Text ="Indirizzo";
                deatilLabel.Text = userMember.adress;
                break;
            case 1:
                headingLabel.Text ="Telefono";
                deatilLabel.Text = userMember.phone;
                break;
            case 2:
                headingLabel.Text ="sito web";
                deatilLabel.Text = userMember.website;
                break;
            case 3:
                headingLabel.Text ="Posta";
                deatilLabel.Text = userMember.mail;
                break;
            case 4:
                headingLabel.Text = "Portami con google maps";
                deatilLabel.Text = userMember.lat + ", " + userMember.lon;
                break;
            case 5:
                headingLabel.Text = "Portami con maps";
                deatilLabel.Text = userMember.lat + ", " + userMember.lon;
                break;
            }
            
            // btnletsGo.AddTarget (this, new Selector ("HandleBotontListLiked"), UIControlEvent.TouchUpInside);
            
        }
        
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
         
            
            headingLabel.Frame = new RectangleF (50, 5, ContentView.Bounds.Width - 5, 20);
            deatilLabel.Frame = new RectangleF (50, 20, ContentView.Bounds.Width - 5, 20);
            
        }
        
      
        
    }
	
}
