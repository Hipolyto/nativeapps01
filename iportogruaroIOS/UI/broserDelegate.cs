using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace iportogruaroIOS
{
    public class browserDelegate : UIWebViewDelegate {
        
        private UiWebView _vc;
        public browserDelegate (UiWebView controller):base()
        {
            _vc = controller;
        }
        
        public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            // did the user click on a link?
            if ((navigationType == UIWebViewNavigationType.LinkClicked) || (navigationType == UIWebViewNavigationType.BackForward)) {
                NSUrl url = request.Url;
                // is it an http link?
                if (url.Scheme == "http"){
					//_vc.urlStr = url.AbsoluteString;
                    // update the addressbar with the new link
                    //  _vc.addressBar.Text = url.AbsoluteString;   
                }
                return true;
            }
            return true;
        }
        
        
        public override void LoadStarted (UIWebView webView)
        {
            // let the user know we are working on their request
            var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
            _vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem (indicator);
            indicator.StartAnimating ();
        }
        
        public override void LoadingFinished (UIWebView webView)
        {
            
            // let the user know that we are done with their request
            _vc.NavigationItem.RightBarButtonItem = null;
			_vc.btnBackWebview = webView.CanGoBack;
            // set the enabled state for the forward and back buttons
            
            
        }
        
        
    }
}

