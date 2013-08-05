using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using iportogruaroLibraryShared;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;

namespace iportogruaroIOS
{
	public class sourceMainCategory: UITableViewSource
	{
		static readonly NSString CellIdentifier = new NSString ("DataSourceCell");
		//List<object> objects = new List<object> ();
		UIHomeScreen controller;
		List<iportogruarocategories> list;
		
		public sourceMainCategory (UIHomeScreen controller,List<iportogruarocategories> _list)
		{
			this.controller = controller;
			list = _list;
		}
		
        #region pull to Refresh
        bool checkForRefresh;
        
        public override void DraggingStarted (UIScrollView scrollView)
        {
            checkForRefresh = true;
        }
        
        public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
        {
            checkForRefresh = false;
            if ((controller as UIHomeScreen).tl.ContentOffset.Y > -yboundary)
                return;
            else
                (controller as UIHomeScreen).loadData();
            
        }
        
        const float yboundary = 65;
        #endregion
		
		// Customize the number of sections in the table view.
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return list.Count;
		}

        public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
        {
			//if (indexPath.Row == 0)
			//	return 60f;
			//else
            	return 51;

        }

		static NSString cellIdentifier = new NSString ("CellId");
		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			mainItem cell = tableView.DequeueReusableCell (cellIdentifier) as mainItem;

			if (cell == null) {
                cell = new mainItem (cellIdentifier,controller.UserInterfaceIdiomIsPhoneProp);
				
				//cell.controller = controller as UIDetail;
				
			
				

				UIView viewTbl = new UIView (cell.Bounds);
				//viewTbl.Layer.BorderWidth = 0.5f;
				//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
				viewTbl.BackgroundColor = UIColor.Clear;
				viewTbl.BackgroundColor = UIColor.White;
				viewTbl.Layer.MasksToBounds = true;
				//viewTbl.Layer.CornerRadius = 5;
				//CAT
				//255, 255, 255, 
				//248, 248, 248,
				//238, 238, 238,
				//229, 229, 229,
				//220, 220, 220
				//POS
				//255,255,255,
				//255,255,255,
				//255,255,255,
				//248,248,248,
				//234,234,234,

				var oGradienttblMain = new CAGradientLayer ();
				oGradienttblMain.BorderWidth = 0f;

				//oGradienttblMain.CornerRadius  = 3;
			oGradienttblMain.Frame = new System.Drawing.RectangleF (0, 0, 1024, 51);
				oGradienttblMain.Colors = new CGColor[] {

					//20130628
					UIColor.FromRGB (255,255,255).CGColor,
					UIColor.FromRGB (248,248,248).CGColor,
					UIColor.FromRGB (238,238,238).CGColor,
					UIColor.FromRGB (229,229,229).CGColor,
					UIColor.FromRGB (220,220,220).CGColor

				};
				viewTbl.Layer.InsertSublayer (oGradienttblMain, 0);// .AddSublayer (oGradient);

				//viewTbl.Layer.BorderWidth = 0.2f;

				cell.BackgroundView = viewTbl;
				
				
			}

			cell.userMember = list [indexPath.Row];
			//if (indexPath.Section == 2)
			if (indexPath.Row == 0)
				cell.drawHeadeLine = true;
			cell.UpdateCell (list [indexPath.Row].name);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);
			
			
			
			
			
			return cell;   
		}
	
		
		
	
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            if (this.controller.reloading == true)
                return;

			var item = list [indexPath.Row];
		
			if (item.cat_id == "0")
				return;
            if (item.cat_id == "EVENTI")
            {
                this.controller.NavigationController.PushViewController(new UiEventsListController(){Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
				return;
            }

			if (item.cat_id == "INFO")
			{
				this.controller.NavigationController.PushViewController(new UIAbout(){Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
				return;
			}





            if (item.cat_id == "UPDATE")
            {
                (controller as UIHomeScreen).loadData();
                return;
            }

            else if (item.cat_id == "NOTIZIE")
            {
                this.controller.NavigationController.PushViewController(new UiNewsListController(){Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
                
            }
            else
                this.controller.NavigationController.PushViewController(new UiCategoryList(){cat_id = item.cat_id,Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
			
			
		}
	}
}

