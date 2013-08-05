// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace iportogruaroIOS
{
	[Register ("UiIpadDetailScreen")]
	partial class UiIpadDetailScreen
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblItemAdress { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSubTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnShare { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnMap { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDes { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}

			if (lblItemAdress != null) {
				lblItemAdress.Dispose ();
				lblItemAdress = null;
			}

			if (lblSubTitle != null) {
				lblSubTitle.Dispose ();
				lblSubTitle = null;
			}

			if (btnShare != null) {
				btnShare.Dispose ();
				btnShare = null;
			}

			if (btnMap != null) {
				btnMap.Dispose ();
				btnMap = null;
			}

			if (lblDes != null) {
				lblDes.Dispose ();
				lblDes = null;
			}
		}
	}
}
