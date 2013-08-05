// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace iportogruaroIOS
{
	[Register ("UiWebView")]
	partial class UiWebView
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView webview { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar barWebView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem btnitemBack { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem btnitemSafari { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (webview != null) {
				webview.Dispose ();
				webview = null;
			}

			if (barWebView != null) {
				barWebView.Dispose ();
				barWebView = null;
			}

			if (btnitemBack != null) {
				btnitemBack.Dispose ();
				btnitemBack = null;
			}

			if (btnitemSafari != null) {
				btnitemSafari.Dispose ();
				btnitemSafari = null;
			}
		}
	}
}
