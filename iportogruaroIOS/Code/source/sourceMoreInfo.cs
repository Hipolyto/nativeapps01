using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using iportogruaroLibraryShared;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;
using System.Threading;

namespace iportogruaroIOS
{
    public class sourceMoreInfo: UITableViewSource
    {
        UIViewController controller;

        public UiDetailScreen detailController{get;set;}
        public iportogruaropos item{get;set;}
        public sourceMoreInfo (UiPosMoreInfoListController controller,iportogruaropos _item)
        {
            item = _item;
            this.controller = controller;

        }

		/*
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60;
		}
        */

        public sourceMoreInfo (UiDetailScreen controller,iportogruaropos _item)
        {
            item = _item;
            this.detailController = controller;
            
        }
        /*
        #region pull to Refresh
        bool checkForRefresh;
        
        public override void DraggingStarted (UIScrollView scrollView)
        {
            checkForRefresh = true;
        }
        
        public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
        {
            checkForRefresh = false;
            if ((controller as UiPosMoreInfoListController).tl.ContentOffset.Y > -yboundary)
                return;
            else
                (controller as UiPosMoreInfoListController).loadData();
            
        }
        
        const float yboundary = 65;
        #endregion
        */
        // Customize the number of sections in the table view.
        public override int NumberOfSections (UITableView tableView)
        {
            return 1;
        }
        
        public override int RowsInSection (UITableView tableview, int section)
        {
            return 6;
        }
        static NSString cellIdentifier = new NSString ("CellId");
        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            moreinfoItem cell = tableView.DequeueReusableCell (cellIdentifier) as moreinfoItem;
            
            if (cell == null) {
                cell = new moreinfoItem (cellIdentifier,detailController.UserInterfaceIdiomIsPhoneProp);
                
                //cell.controller = controller as UIDetail;
                
              
                
            }
			//cell.ImageView.Image  = UIImage.FromFile("images/NOIMG.png"); 
			switch(indexPath.Row)
			{
			case 0:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/Address.png"); 
				break;
			case 1:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/Phone.png"); 
				break;
			case 2:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/Website.png"); 
				break;
			case 3:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/Mail.png"); 
				break;
			case 4:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/GoogleMaps.png"); 
				break;
			case 5:
				cell.ImageView.Image  = UIImage.FromFile("images/moreInfo/Maps.png"); 
				break;
			default:
			

				break;
			}

            cell.userMember = item;
            //if (indexPath.Section == 2)
            cell.UpdateCell (item.title,indexPath.Row);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);
            
            return cell;   
        }
   
        private UiPosListController detail;
        private UiCategoryListController catList;
        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {

            switch (indexPath.Row) {
            
            case 1:

                {
                if (item.phone == null)
                    return;
                if (item.phone == "")
                    return;
                    NSUrl url = new NSUrl("tel:" + item.phone.Replace("tel: ",""));
                    if (!UIApplication.SharedApplication.OpenUrl(url)) {
                    using(var alert = new UIAlertView("iPortogruaro", "Le chiamate sono possibili solo da dispositivi con funzionalità di telefonia.", null, "Ok", null))//Viajes Telcel//Aceptar
                        {
                            alert.Show();
                        }
                    }
                }
                break;
            case 2:
                {
                if (item.website == null)
                    return;
                if (item.website == "")
                    return;
                this.detailController.NavigationController.PushViewController (new UiWebView(item.website), true);
                }
                break;
            case 3:
            {
                if (item.mail == null)
                    return;
                if (item.mail == "")
                    return;
                this.detailController.sendMail (item.mail);
            }
            

                break;
            case 4:
                {
               // var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
                //ad.GetLocation ();
                string strlat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
                string strlon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");

                string urlStr = "http://maps.google.com/maps?saddr=" + strlat+","+strlon+ "&daddr=" + item.lat+","+item.lon;
                    this.detailController.NavigationController.PushViewController (new UiWebView(urlStr), true);
            
                }
                break;
            case 5:
            {
                //var ad = (AppDelegate)UIApplication.SharedApplication.Delegate;
                //ad.GetLocation ();
                string strlat = NSUserDefaults.StandardUserDefaults.StringForKey ("Latitude");
                string strlon = NSUserDefaults.StandardUserDefaults.StringForKey ("Longitude");


                //strlat =  "44.7201182";
                //strlon =  "11.1439136";


                    string urlStr = "http://maps.apple.com/maps?saddr=" + strlat + "," + strlon + "&daddr=" + item.lat+","+item.lon;
                NSUrl url = new NSUrl(urlStr);
                if (!UIApplication.SharedApplication.OpenUrl(url)) {
                   
                }

            }
                break;
            }

           
        }
    }
}
