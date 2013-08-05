using System;
using System.Drawing;
using iportogruaroLibraryShared;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iportogruaroLibraryShared;
using MonoTouch.CoreAnimation;
using MonoTouch.Social;
using MonoTouch.MessageUI;
using MonoTouch.CoreGraphics;

/*

namespace iportogruaroIOS
{
	public class CreatePopupSocialNetwork
	{
		public CreatePopupSocialNetwork ()
		{
		}
		public iportogruaropos dataPos{get;set;}
		public int UpdatecurrenShareOpt{ get; set; }
		private string[] shareList;
		bool showComboTaks = false;
		UIPickerView pkrShare;
		UIToolbar BarraShare;
		private void loadcomboTaks (iportogruaropos _dataPos, UIViewController ui, int _updatecurrenShareOpt)
	{
			this.dataPos = _dataPos;
			this.UpdatecurrenShareOpt = _updatecurrenShareOpt;
		int items = 0;
		try {
			items = dataPos.shareoptions.Count;
		} catch {
			items = 0;
		}

		if (items == 0)
			return;

		shareList = new string[items];
		items = 0;
		foreach (shareOptions s in dataPos.shareoptions) {

			shareList [items] = s.key;
			items ++;
		}
            if (showComboTaks)
                return;

            cmbShare _pickerDataModel;
            _pickerDataModel = new cmbShare (this, shareList);

            if (baseView.IsTall)
                pkrShare = new UIPickerView (new RectangleF (0, 290, 320, 216));
            else
                pkrShare = new UIPickerView (new RectangleF (0, 200, 320, 216)); 
            
            pkrShare.Source = _pickerDataModel;
            pkrShare.ShowSelectionIndicator = true; 
            
            
            
            var animation = CABasicAnimation.FromKeyPath ("transform.translation.y");
            animation.From = NSNumber.FromFloat (100);
            animation.To = NSNumber.FromFloat (0);
            animation.Duration = .2;
            animation.FillMode = CAFillMode.Both;
            
            
			ui.View.AddSubview (pkrShare);
			BarraShare = BarraShareGet ();
            ui.View.AddSubview (BarraShare);
            
            animation.AutoReverses = false;
          
            pkrShare.Layer.AddAnimation (animation, "moveToHeader");
            
            pkrShare.Select (0, 0, false);
            
            showComboTaks = true;

            UpdatecurrenShareOpt = 0;
        }
	

	private UIToolbar BarraShareGet ()
	{
		UIBarButtonItem cancelButton = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, delegate {
			hidecomboShare ();
		}
		);//Cancelar
		UIBarButtonItem Donebtn = new UIBarButtonItem (UIBarButtonSystemItem.Done, delegate {

			donecomboShare ();
		}
		);

		UIBarButtonItem space = new UIBarButtonItem (UIBarButtonSystemItem.FixedSpace);
		space.Width = 185;  


		UIToolbar bar;
		if (baseView.IsTall)
			bar = new UIToolbar (new RectangleF (0, 250, 320, 40));
		else
			bar = new UIToolbar (new RectangleF (0, 160, 320, 40));


		bar.Items = new UIBarButtonItem[] { cancelButton, space, Donebtn }; 
		bar.TintColor = UIColor.FromRGB (152, 22, 22);  

		return bar;
	}

		public void donecomboShare ()
		{


			hidecomboShare ();
			string key = shareList [UpdatecurrenShareOpt];

			Console.WriteLine(key + " Llave: " + UpdatecurrenShareOpt);

			switch (key) {
				case "facebook":
			{
				SLComposeViewController slComposer;

				if (SLComposeViewController.IsAvailable (SLServiceKind.Facebook)) {
					slComposer = SLComposeViewController.FromService (SLServiceType.Facebook);
					slComposer.SetInitialText ("Raccomandato da iPortogruaro - l'app che ti aiuta ad ottimizzare il tuo tempo -- " + System.Web.HttpUtility.HtmlDecode(dataPos.title));
					if (dontHadPhoto)  
						slComposer.AddImage (imageView.Image);
					// slComposer.AddImage (UIImage.FromFile ("monkey.png"));
					slComposer.CompletionHandler += (result) => {
						InvokeOnMainThread (() => {
							DismissViewController (true, null);
							if (result == SLComposeViewControllerResult.Done) {
								var alert = new UIAlertView ("Esito", "messaggio mandato", null, "Ok");

								alert.Show ();
							}
						});
					};
					PresentViewController (slComposer, true, null);
				}

				else
				{
					InvokeOnMainThread (() => {

						var alert = new UIAlertView ("Facebook", "Entrare nei settaggi per configurare l'account facebook", null, "Ok");

						alert.Show ();


					});
				}

			}
				break;
				case "twitter":
			{
				SLComposeViewController slComposer;

				if (SLComposeViewController.IsAvailable (SLServiceKind.Twitter)) {
					slComposer = SLComposeViewController.FromService (SLServiceType.Twitter);
					slComposer.SetInitialText ("Raccomandato da iPortogruaro - l'app che ti aiuta ad ottimizzare il tuo tempo -- " + System.Web.HttpUtility.HtmlDecode(dataPos.title));
					if (dontHadPhoto)  
						slComposer.AddImage (imageView.Image);
					// slComposer.AddImage (UIImage.FromFile ("monkey.png"));
					slComposer.CompletionHandler += (result) => {
						InvokeOnMainThread (() => {
							DismissViewController (true, null);
							if (result == SLComposeViewControllerResult.Done) {
								var alert = new UIAlertView ("Esito", "messaggio mandato", null, "Ok");

								alert.Show ();
							}
						});
					};
					PresentViewController (slComposer, true, null);
				}
				else
				{
					InvokeOnMainThread (() => {

						var alert = new UIAlertView ("Twitter", "Entrare nei settaggi per configurare l'account Twitter", null, "Ok");

						alert.Show ();


					});
				}
				break;


			}
				case "mail":
			{
				MFMailComposeViewController _mail;

				if (MFMailComposeViewController.CanSendMail) {
					_mail = new MFMailComposeViewController ();
					_mail.SetMessageBody ("Raccomandato da iPortogruaro - l'app che ti aiuta ad ottimizzare il tuo tempo --  -- " + System.Web.HttpUtility.HtmlDecode(dataPos.title), 
					                      false);
					_mail.SetSubject (System.Web.HttpUtility.HtmlDecode(dataPos.title));
					_mail.Finished += HandleMailFinished;
					if (dontHadPhoto == true)
					{
						NSData dat = imageView.Image.AsJPEG (0);
						_mail.AddAttachmentData (dat, "image/png", System.Web.HttpUtility.HtmlDecode(dataPos.title));
					}
					this.PresentModalViewController (_mail, true);

				} else {
					// handle not being able to send mail
				}
				break;
			}
			}

		}




	}
}

*/