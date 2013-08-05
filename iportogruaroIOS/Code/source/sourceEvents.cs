using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using iportogruaroLibraryShared;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;

namespace iportogruaroIOS
{
	
    public class sourceEvents: UITableViewSource
    {
        static readonly NSString CellIdentifier = new NSString ("DataSourceCell");
        //List<object> objects = new List<object> ();
        UiEventsListController controller;
        List<eventos> list;
        
        public sourceEvents (UiEventsListController controller,List<eventos> _list)
        {
            this.controller = controller;
            list = _list;
        }
        
        #region pull to Refresh
        /*
        bool checkForRefresh;
        
        public override void DraggingStarted (UIScrollView scrollView)
        {
            checkForRefresh = true;
        }
        
        public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
        {
            checkForRefresh = false;
            if ((controller as UiEventsListController).tl.ContentOffset.Y > -yboundary)
                return;
            else
                (controller as UiEventsListController).loadData();
            
        }
        
        const float yboundary = 65;
        */
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
            return 51;
        }
        static NSString cellIdentifier = new NSString ("CellId");
        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            eventItem cell = tableView.DequeueReusableCell (cellIdentifier) as eventItem;
            
            if (cell == null) {
                cell = new eventItem (cellIdentifier,controller.UserInterfaceIdiomIsPhone);
                
                //cell.controller = controller as UIDetail;
                
                //viewTbl.Layer.BorderWidth = 0.5f;
                //viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
                
            }
          
            cell.UpdateCell (list [indexPath.Row].title);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);
            
            return cell;   
        }
        
        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            var item = list [indexPath.Row];
            this.controller.NavigationController.PushViewController (new UIEventDetail(){dataPos = item}, true); 
        }
    }
}
