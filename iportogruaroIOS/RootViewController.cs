using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using iportogruaroLibraryShared;
using System.Threading;
using MonoTouch.Dialog;

namespace iportogruaroIOS
{
	public partial class RootViewController : UITableViewController
	{
		DataSource dataSource;

		public RootViewController () : base ("RootViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Master", "Master");

			// Custom initialization
		}

		public DetailViewController DetailViewController {
			get;
			set;
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

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

			ThreadPool.QueueUserWorkItem (state =>
			                              {
			var lst = new iportogruaroLibraryShared.mainCategorys().getMainCategorys(true);
			
				InvokeOnMainThread (delegate {
					TableView.Source = dataSource = new DataSource (this,lst);
					TableView.ReloadData();
				}
				);

			});

		}

		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("DataSourceCell");
			//List<object> objects = new List<object> ();
			RootViewController controller;
			List<iportogruarocategories> list;

			public DataSource (RootViewController controller,List<iportogruarocategories> _list)
			{
				this.controller = controller;
				list = _list;
			}



			// Customize the number of sections in the table view.
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return list.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (CellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.TextLabel.Text = System.Web.HttpUtility.HtmlDecode( list [indexPath.Row].name) ;

				return cell;
			}

		



			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/

			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var item = list [indexPath.Row];
				Section Airlines = new Section ();



				var l = new iportogruaroLibraryShared.subCategorys().getSubCategorys(item.cat_id,true);

				foreach(iportogruarocategories i in l)
				{
					var e = new StyledStringElement (i.name,i.name);
					e.Caption = i.name;
					e.Value = i.name;
					Airlines.Add(e);
				}



				RootElement root = new RootElement (item.name) {
					Airlines
				};
				
				var myController = new DialogViewController (root, true);
				
				this.controller.NavigationController.PushViewController(myController,true);


			}
		}
	}
}
