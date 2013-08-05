using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using iportogruaroLibraryShared;
using System.Collections.Generic;
using MonoTouch.Dialog;
using MonoTouch.CoreAnimation;
using System.Threading;

using System.Linq;
using System.Text;
using MonoTouch.CoreGraphics;


namespace iportogruaroIOS
{
    public class sourceSecondCategoryCustom: UITableViewSource
    {
        UiCategoryList controller;
        List<iportogruarocategories> list;

        public sourceSecondCategoryCustom (UiCategoryList controller,List<iportogruarocategories> _list)
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
            if ((controller as UiCategoryList).tl.ContentOffset.Y > -yboundary)
                return;
            else
                (controller as UiCategoryList).loadData();

        }

        const float yboundary = 65;
        */
        #endregion

        // Customize the number of sections in the table view.
        public override int NumberOfSections (UITableView tableView)
        {
            return 1;
        }
        public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
        {
			var item = list [indexPath.Row];
			if (item.isPresentAsCategory)
			{
				return 92;
			}
            return 51; //51
        }
        public override int RowsInSection (UITableView tableview, int section)
        {
            return list.Count;
        }
        static NSString cellIdentifier = new NSString ("CellId");
        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
			var item = list [indexPath.Row];

			if (item.isPresentAsCategory)
			{
				posItem cell = tableView.DequeueReusableCell (cellIdentifier) as posItem;

				if (cell == null) {
					cell = new posItem (cellIdentifier,controller.UserInterfaceIdiomIsPhoneProp);
					cell.isIphone = controller.UserInterfaceIdiomIsPhoneProp;
					//cell.controller = controller as UIDetail;

					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					UIView viewTbl = new UIView (new System.Drawing.RectangleF (0, 0, 1024, 92));
					viewTbl.AutosizesSubviews = true;
					viewTbl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					viewTbl.BackgroundColor = UIColor.Clear;
					//viewTbl.BackgroundColor = UIColor.Yellow;
					viewTbl.Layer.MasksToBounds = true;
					//viewTbl.Layer.CornerRadius = 5;

					var oGradienttblMain = new CAGradientLayer ();
					oGradienttblMain.BorderWidth = 0f;

					//oGradienttblMain.CornerRadius  = 3;
					oGradienttblMain.Frame = new System.Drawing.RectangleF (0, 0, 1024, 92);
					oGradienttblMain.Colors = new CGColor[] {

						//20130628
						/*UIColor.FromRGB (255,255,255).CGColor,
					UIColor.FromRGB (248,248,248).CGColor,
					UIColor.FromRGB (238,238,238).CGColor,
					UIColor.FromRGB (229,229,229).CGColor,
					UIColor.FromRGB (220,220,220).CGColor*/
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (248,248,248).CGColor,
						UIColor.FromRGB (234,234,234).CGColor
					};
					viewTbl.Layer.InsertSublayer (oGradienttblMain, 0);// .AddSublayer (oGradient);
					//cell.controller = controller as UIDetail;

					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					cell.BackgroundView = viewTbl;
				}
				iportogruaropos cat = new iportogruaropos ();
				cat.icon = item.icon_image;
				cat.poi_id = item.cat_id;
				cat.title = item.name;




				cat.lat = item.lat;
				cat.lon = item.lon;

				cell.userMember = cat;
				//if (indexPath.Section == 2)
				cell.UpdateCell (list [indexPath.Row].name);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);

				return cell; 
			}
			else
			{

			mainItem cell = tableView.DequeueReusableCell (cellIdentifier) as mainItem;

            if (cell == null) {
                cell = new mainItem (cellIdentifier,controller.UserInterfaceIdiomIsPhoneProp);

				UIView viewTbl = new UIView (cell.Bounds);
				//viewTbl.Layer.BorderWidth = 0.5f;
				//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
				viewTbl.BackgroundColor = UIColor.Clear;
				viewTbl.BackgroundColor = UIColor.White;
				viewTbl.Layer.MasksToBounds = true;
				//viewTbl.Layer.CornerRadius = 5;

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
					viewTbl.AutosizesSubviews = true;
					viewTbl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
				cell.BackgroundView = viewTbl;
            }

            cell.userMember = list [indexPath.Row];
            //if (indexPath.Section == 2)
            cell.UpdateCell (list [indexPath.Row].name);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);

            return cell;  
			}
        }
        private void RequestImage (object  state)
        {
            try {

                object[] on = state as object[];
                UIImageView controller = on [0] as UIImageView;
                iportogruarocategories imgmain = on [1] as iportogruarocategories;

                if (imgmain == null)
                    return;

                if (imgmain.icon_image == null)
                    return;

                if (imgmain.icon_image.Length > 0) {

                    UIImage imgRoom;

                    imgRoom=  ImageHotel( imgmain.cat_id+"_s.png",imgmain.icon_image);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    if (imgRoom == null)
                    {
                        NSUrl imageUrl = NSUrl.FromString (imgmain.icon_image);
                        NSData imageData = NSData.FromUrl (imageUrl);

                        imgRoom = UIImage.LoadFromData (imageData);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
                    }
                    if (imgRoom != null) {
                        InvokeOnMainThread (delegate {
                            controller.Image = imgRoom;// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                            RefreshImage (state);
                        }
                        );
                        //imgRoom = null;
                    }
                }
            } catch (Exception ex) {

                Console.WriteLine (ex.ToString ());
            }
        }
        private  UIImage ImageHotel(string imgName,string url)
        {
            try
            {
                UIImage resp;
                string sCachedPath =System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal  ),  imgName);
                resp = UIImage.FromFile(sCachedPath);   

                //Console.WriteLine (sCachedPath); 

                if (resp == null && url.Length >0)
                {
                    //Console.WriteLine("No se encontro cache imagenes");  

                    NSUrl imageUrl = NSUrl.FromString (url);
                    NSData imageData = NSData.FromUrl (imageUrl);

                    resp = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

                    try
                    {
                        NSError err = new NSError(new NSString("http://www.iportogruaro.com"),0); 

						InvokeOnMainThread (delegate {
							var res =resp.AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
							if (!res)
								Console.WriteLine("Error: "+ err.LocalizedDescription ); 
						}
						);

                    }

                    catch
                    {
                        return null;
                    }

                }
                else
                {
                    //  Console.WriteLine("se carga cache imagenes");  
                }

                return resp ;
            }
            catch
            {
                return null;
            }

        }
        private void RefreshImage (object  state)
        {

            try {
                object[] on = state as object[];
                UIImageView controller = on [0] as UIImageView;

                //Console.WriteLine("<<<<<<  Actualiza Imagen >>>>>>>>>");
                var animation = CABasicAnimation.FromKeyPath ("opacity");
                animation.From = NSNumber.FromFloat (0);
                animation.To = NSNumber.FromFloat (1);
                animation.Duration = .2;
                //animation.Delegate = new MyAnimationDelegate (controller, true);
                //AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;

                controller.Layer.MasksToBounds = false;
                controller.Layer.CornerRadius = 7;

                controller.Layer.AddAnimation (animation, "moveToHeader");
                /*
                tableView.BeginUpdates();
                tableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
                //tableView.EndUpdates();
                */

            } catch {
            }
            /*
            UIView.BeginAnimations ("imageThumbnailTransitionIn");
            imageView2.Alpha =  1.0f; 
            UIView.SetAnimationDuration (0.5f);
            UIView.CommitAnimations ();
            */
        }
        private UiPosList detail;
        private UiCategoryListController catList;
        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
			try{
            if (this.controller.reloading == true)
                return;

            var item = list[indexPath.Row];

            if (item.parent == "POS" || item.isPresentAsCategory) 
            {
                var lstpos = new Mainiportogruaropos().getAllPost();

                if (lstpos != null)
                {

                    List<iportogruaropos>  auxPos = (from c in lstpos
                                                     orderby c.title ascending
                                                     where
                                                     c.poi_id.ToLower() == item.cat_id
                                                     select c).ToList (); 
                    if (auxPos != null)
                    {
                        if (auxPos.Count > 0)
                        {
                            var itempos = auxPos[0];

                            if (itempos.poi_id == null)
                                return;

                            if (itempos.poi_id == "0")
                                return;

                            var detail = new UiDetailScreen ();
                            //detail.controllerPast = this.controller;
                            detail.dataPos = itempos;
                            detail.positionPos = indexPath.Row;
                            this.controller.NavigationController.PushViewController(detail,true);
                        }
                    }
                }
                return;
            }

            if (item.parent  != "0")
            {

                if (subCategorys.ishasSong(item))
                    this.controller.NavigationController.PushViewController(new UiCategoryList(){cat_id = item.cat_id,Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
                else
                {
                    detail = new UiPosList ();
                    detail.Title = System.Web.HttpUtility.HtmlDecode(item.name);
                    detail.cat_id =  item.cat_id ;
                    this.controller.NavigationController.PushViewController(detail,true);
                }

            }
            else
            {
                if (item.cat_id == "0")
                    return;



                this.controller.NavigationController.PushViewController(new UiCategoryList(){cat_id = item.cat_id,Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
            }
			}
			catch {
			}
        }

    }
	public class sourceSecondCategory: UITableViewSource
	{
		UiCategoryListController controller;
		public List<iportogruarocategories> list{ get; set;}

		public sourceSecondCategory (UiCategoryListController controller,List<iportogruarocategories> _list)
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
			if ((controller as UiCategoryListController).tl.ContentOffset.Y > -yboundary)
				return;
			else
				(controller as UiCategoryListController).loadData();

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
		static NSString cellIdentifier = new NSString ("CellId");
		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var item = list [indexPath.Row];

			if (item.isPresentAsCategory)
			{
				posItem cell = tableView.DequeueReusableCell (cellIdentifier) as posItem;

				if (cell == null) {
					cell = new posItem (cellIdentifier,controller.UserInterfaceIdiomIsPhone);
					cell.isIphone = controller.UserInterfaceIdiomIsPhone;
					//cell.controller = controller as UIDetail;

					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					UIView viewTbl = new UIView (new System.Drawing.RectangleF (0, 0, 1024, 92));
					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					viewTbl.BackgroundColor = UIColor.Clear;
					//viewTbl.BackgroundColor = UIColor.Yellow;
					viewTbl.Layer.MasksToBounds = true;
					//viewTbl.Layer.CornerRadius = 5;

					var oGradienttblMain = new CAGradientLayer ();
					oGradienttblMain.BorderWidth = 0f;

					//oGradienttblMain.CornerRadius  = 3;
					oGradienttblMain.Frame = new System.Drawing.RectangleF (0, 0, 1024, 92);
					oGradienttblMain.Colors = new CGColor[] {

						//20130628
						/*UIColor.FromRGB (255,255,255).CGColor,
					UIColor.FromRGB (248,248,248).CGColor,
					UIColor.FromRGB (238,238,238).CGColor,
					UIColor.FromRGB (229,229,229).CGColor,
					UIColor.FromRGB (220,220,220).CGColor*/
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (255,255,255).CGColor,
						UIColor.FromRGB (248,248,248).CGColor,
						UIColor.FromRGB (234,234,234).CGColor
					};
					viewTbl.Layer.InsertSublayer (oGradienttblMain, 0);// .AddSublayer (oGradient);
					//cell.controller = controller as UIDetail;

					//viewTbl.Layer.BorderWidth = 0.5f;
					//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;
					cell.BackgroundView = viewTbl;
				}
				iportogruaropos cat = new iportogruaropos ();
				cat.icon = item.icon_image;
				cat.poi_id = item.cat_id;
				cat.title = item.name;




				cat.lat = item.lat;
				cat.lon = item.lon;

				cell.userMember = cat;
				//if (indexPath.Section == 2)
				cell.UpdateCell (list [indexPath.Row].name);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);

				return cell; 
			}
			else
			{

			mainItem cell = tableView.DequeueReusableCell (cellIdentifier) as mainItem;

			if (cell == null) {
				cell = new mainItem (cellIdentifier,controller.UserInterfaceIdiomIsPhone);

				//cell.controller = controller as UIDetail;

				//viewTbl.Layer.BorderWidth = 0.5f;
				//viewTbl.Layer.BorderColor = UIColor.FromRGB (102, 153, 173).CGColor;

			}

			cell.userMember = list [indexPath.Row];
			//if (indexPath.Section == 2)
			cell.UpdateCell (list [indexPath.Row].name);//,ItemsDinings [indexPath.Section].mapFeature + " >> " + ItemsDinings [indexPath.Section].adress);

			GC.Collect ();

			return cell;  
			}
		}
		private void RequestImage (object  state)
		{
			try {

				object[] on = state as object[];
				UIImageView controller = on [0] as UIImageView;
				iportogruarocategories imgmain = on [1] as iportogruarocategories;

				if (imgmain == null)
					return;

				if (imgmain.icon_image == null)
					return;

				if (imgmain.icon_image.Length > 0) {

					UIImage imgRoom;

					imgRoom=  ImageHotel( imgmain.cat_id+"_s.png",imgmain.icon_image);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					if (imgRoom == null)
					{
						NSUrl imageUrl = NSUrl.FromString (imgmain.icon_image);
						NSData imageData = NSData.FromUrl (imageUrl);

						imgRoom = UIImage.LoadFromData (imageData);// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});
					}
					if (imgRoom != null) {
						InvokeOnMainThread (delegate {
							controller.Image = imgRoom;// ;;    UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

							RefreshImage (state);
						}
						);
						//imgRoom = null;
					}
				}
			} catch (Exception ex) {

				Console.WriteLine (ex.ToString ());
			}
		}
		private  UIImage ImageHotel(string imgName,string url)
		{
			try
			{
				UIImage resp;
				string sCachedPath =System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal  ),  imgName);
				resp = UIImage.FromFile(sCachedPath);	

				//Console.WriteLine (sCachedPath); 

				if (resp == null && url.Length >0)
				{
					//Console.WriteLine("No se encontro cache imagenes");  

					NSUrl imageUrl = NSUrl.FromString (url);
					NSData imageData = NSData.FromUrl (imageUrl);

					resp = UIImage.LoadFromData (imageData);//   .Add(new BasicTableImageItem(){ Image = new UIImageView (   UIImage.LoadFromData (imageData)).Image , RoomType  =  controller.RoomType});

					try
					{
						NSError err = new NSError(new NSString("http://www.iportogruaro.com"),0); 

						InvokeOnMainThread (delegate {
							var res =resp.AsPNG().Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), imgName), true, out err);
							if (!res)
								Console.WriteLine("Error: "+ err.LocalizedDescription ); 
						}
						);

					}

					catch
					{
						return null;
					}

				}
				else
				{
					//	Console.WriteLine("se carga cache imagenes");  
				}

				return resp ;
			}
			catch
			{
				return null;
			}

		}
		private void RefreshImage (object  state)
		{

			try {
				object[] on = state as object[];
				UIImageView controller = on [0] as UIImageView;

				//Console.WriteLine("<<<<<<  Actualiza Imagen >>>>>>>>>");
				var animation = CABasicAnimation.FromKeyPath ("opacity");
				animation.From = NSNumber.FromFloat (0);
				animation.To = NSNumber.FromFloat (1);
				animation.Duration = .2;
				//animation.Delegate = new MyAnimationDelegate (controller, true);
				//AppDes.PropertyName = hotel.Hotel_HOD_RS.Property.GeneralInformation.Name;

				controller.Layer.MasksToBounds = false;
				controller.Layer.CornerRadius = 7;

				controller.Layer.AddAnimation (animation, "moveToHeader");
				/*
				tableView.BeginUpdates();
				tableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				//tableView.EndUpdates();
				*/

			} catch {
			}
			/*
            UIView.BeginAnimations ("imageThumbnailTransitionIn");
            imageView2.Alpha =  1.0f; 
            UIView.SetAnimationDuration (0.5f);
            UIView.CommitAnimations ();
            */
		}
		private UiPosList detail;
		private UiCategoryListController catList;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            if (this.controller.reloading == true)
                return;

			var item = list[indexPath.Row];

			if (item.parent == "POS" || item.isPresentAsCategory == true)
			{
				var lstpos = new Mainiportogruaropos().getAllPost();

				if (lstpos != null)
				{

					List<iportogruaropos>  auxPos = (from c in lstpos
					                                 orderby c.title ascending
					                                 where
					                                 c.poi_id.ToLower() == item.cat_id
					                                 select c).ToList (); 
					if (auxPos != null)
					{
						if (auxPos.Count > 0)
						{
							var itempos = auxPos[0];

							if (itempos.poi_id == null)
								return;

							if (itempos.poi_id == "0")
								return;

							var detail = new UiDetailScreen ();
							//detail.controllerPast = this.controller;
							detail.dataPos = itempos;
							detail.positionPos = indexPath.Row;
							this.controller.NavigationController.PushViewController(detail,true);
						}
					}
				}
				return;
			}

			if (item.parent  != "0")
			{

				if (subCategorys.ishasSong(item))
					this.controller.NavigationController.PushViewController(new UiCategoryListController(){cat_id = item.cat_id,Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
				else
				{
                    detail = new UiPosList ();
					detail.Title = System.Web.HttpUtility.HtmlDecode(item.name);
					detail.cat_id =  item.cat_id ;
					this.controller.NavigationController.PushViewController(detail,true);
				}

			}
			else
			{
				if (item.cat_id == "0")
					return;



				this.controller.NavigationController.PushViewController(new UiCategoryListController(){cat_id = item.cat_id,Title = System.Web.HttpUtility.HtmlDecode(item.name)},true);
			}
		}
	}
}
