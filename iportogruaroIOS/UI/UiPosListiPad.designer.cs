// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace iportogruaroIOS
{
	[Register ("UiPosListiPad")]
	partial class UiPosListiPad
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tblMain { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITabBar tabBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblMain != null) {
				tblMain.Dispose ();
				tblMain = null;
			}

			if (tabBar != null) {
				tabBar.Dispose ();
				tabBar = null;
			}
		}
	}
}
