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

using System.Drawing;

namespace iportogruaroIOS
{
    public class MissingApiView : UIView
    {
        UITextView tv;
        UIButton btn;

        public MissingApiView (RectangleF rect, string text, string url = "") : base(rect)
        {
            tv = new UITextView (){
                TextAlignment = UITextAlignment.Center,
                Text = text,
                BackgroundColor = UIColor.Black,
                TextColor = UIColor.White,
                Font = UIFont.SystemFontOfSize(18),
            };
            this.AddSubview (tv);
            btn = new UIButton ();
            btn.TouchUpInside += delegate {
                if (string.IsNullOrEmpty (url))
                    return;
                var nsurl = new NSUrl (url);
                if (UIApplication.SharedApplication.CanOpenUrl (nsurl))
                    UIApplication.SharedApplication.OpenUrl (nsurl);
            };
            this.AddSubview (btn);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            var offset = 100f;
            tv.Frame = new RectangleF (0, offset, this.Bounds.Width, this.Bounds.Height - offset);
            btn.Frame = Bounds;
        }
    }

}

