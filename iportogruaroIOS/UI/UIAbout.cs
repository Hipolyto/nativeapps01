using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.MessageUI;

namespace iportogruaroIOS
{
	public partial class UIAbout : baseView
	{
		#region cicle
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public UIAbout ()
			: base (UserInterfaceIdiomIsPhone ? "UIAbout_iPhone" : "UIAbout_iPad", null)
		{
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
			showAbount ();
			// Perform any additional setup after loading the view, typically from a nib.
		}
		#endregion

		#region monoDialog
		private UIView TopsectionHeaderTitle (string title)
		{
			UIView header = new UIView (new System.Drawing.RectangleF (0, 0, this.View.Bounds.Width, 30));

			UILabel lblsubtitle;// = new UILabel (new System.Drawing.RectangleF (30, 8, 100, 22));
			lblsubtitle = new UILabel () {
				Font = UIFont.FromName("HelveticaNeue-Bold", 16f),
				TextColor = UIColor.FromRGB(174,65,61),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};

			lblsubtitle.Frame = new System.Drawing.RectangleF (15, 10, this.View.Bounds.Width-20, 20);

			lblsubtitle.Text = title;

			if (!UserInterfaceIdiomIsPhone)
			{
				lblsubtitle.Frame = new System.Drawing.RectangleF (45, 10, this.View.Bounds.Width-20, 20);
			}

			header.AddSubview (lblsubtitle);

			return header;
		}
		private UIView sectionHeaderTitle (string title)
		{
			UIView header = new UIView (new System.Drawing.RectangleF (0, 0, this.View.Bounds.Width, 20));

			UILabel lblsubtitle;// = new UILabel (new System.Drawing.RectangleF (30, 8, 100, 22));
			lblsubtitle = new UILabel () {
				Font = UIFont.FromName("HelveticaNeue-Bold", 16f),
				TextColor = UIColor.FromRGB(174,65,61),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};

			lblsubtitle.Frame = new System.Drawing.RectangleF (15, 0, this.View.Bounds.Width-20, 20);

			lblsubtitle.Text = title;

			if (!UserInterfaceIdiomIsPhone)
			{
				lblsubtitle.Frame = new System.Drawing.RectangleF (45, 0, this.View.Bounds.Width-20, 20);
			}

			header.AddSubview (lblsubtitle);

			return header;
		}
		#region search
		void HandleMailFinished (object sender, MFComposeResultEventArgs e)
		{
			if (e.Result == MFMailComposeResult.Sent) {
				UIAlertView alert = new UIAlertView ("Mail Alert", "Mail Spedita",
				                                     null, "Ok", null);
				alert.Show ();

				// you should handle other values that could be returned 
				// in e.Result and also in e.Error 
			}
			e.Controller.DismissModalViewControllerAnimated (true);
		}
		public void sendMail(string to)
		{
			MFMailComposeViewController _mail;

			if (MFMailComposeViewController.CanSendMail) {
				_mail = new MFMailComposeViewController ();
				_mail.SetMessageBody ("", 
				                      false);
				_mail.SetSubject ("");
				_mail.Finished += HandleMailFinished;
				_mail.SetToRecipients (new string []{to});
				this.PresentModalViewController (_mail, true);

			} else {
				// handle not being able to send mail
			}
		}

		EntryElement searchBoxEntry;
		EntryElement doTodayEntry;
		EntryElement doTomorrowEntry;
		EntryElement issuesEntry;
		EntryElement commentsEntry;
		public void showAbount ()
		{

			DialogViewController searchBoxDialog;

			searchBoxEntry = new EntryElement ("Subject", "Type Here", "");

			StyledStringElement developerName = new StyledStringElement ("Sviluppatore", "Alessandro Facchini");

			UIFont fontLeftSide = UIFont.FromName ("HelveticaNeue-Bold", 14f);
			UIFont fontRighttSide = UIFont.FromName ("Helvetica Neue", 14f);
			developerName.Font = fontLeftSide;
			developerName.SubtitleFont = fontRighttSide;
			
			//
			StyledStringElement develoerEmail = new StyledStringElement ("Email", "info@facchini-ts.net");

			develoerEmail.Font = fontLeftSide;
			develoerEmail.SubtitleFont = fontRighttSide;

			StyledStringElement develoerWeb = new StyledStringElement ("Web Site", "http://www.facchini-ts.net");

			develoerWeb.Font = fontLeftSide;
			develoerWeb.SubtitleFont = fontRighttSide;

			develoerWeb.Tapped += delegate {
				string urlStr = "http://www.facchini-ts.net/";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
		};


			develoerEmail.Tapped += delegate {
				sendMail("info@facchini-ts.net");
			};

			StyledStringElement ideamail_a = new StyledStringElement ("Marco Piccolo", "marco.piccolo@iportogruaro.it");
			StyledStringElement ideamail_b = new StyledStringElement ("Giovanni Gobesso", "giovanni.gobbesso@iportogruaro.it");
			StyledStringElement ideamail_c;
			if (UserInterfaceIdiomIsPhone)
				ideamail_c = new StyledStringElement ("Pigo - " + Environment.NewLine
			                                                          + "Soluzioni Tecnologiche", 
			                                                          "info@" + Environment.NewLine+"iportogruaro.it");
			else
				ideamail_c = new StyledStringElement ("Pigo - Soluzioni Tecnologiche", 
				                                      "info@iportogruaro.it");

			ideamail_a.Font = fontLeftSide;
			ideamail_a.SubtitleFont = fontRighttSide;
			ideamail_a.Lines = 2;

			ideamail_c.Tapped += delegate {


				sendMail("info@iportogruaro.it");
			};

			ideamail_a.Tapped += delegate {
				sendMail("marco.piccolo@iportogruaro.it");
		};

			ideamail_b.Tapped += delegate {
				sendMail("giovanni.gobbesso@iportogruaro.it");
			};

			ideamail_b.Font = fontLeftSide;//UIFont.FromName ("HelveticaNeue-Bold", 12f);;
			ideamail_b.SubtitleFont = fontRighttSide;//UIFont.FromName ("Helvetica Neue", 12f);;
			ideamail_b.LineBreakMode = UILineBreakMode.WordWrap;


			ideamail_c.SubtitleFont = fontRighttSide;
			//ideamail_c.Lines = 2;

			ideamail_c.Font = fontLeftSide;//UIFont.FromName ("HelveticaNeue-Bold", 12f);;
			ideamail_c.SubtitleFont = fontRighttSide;//UIFont.FromName ("Helvetica Neue", 12f);;
			ideamail_c.LineBreakMode = UILineBreakMode.WordWrap;

			//UIFont fontLeftSide = UIFont.FromName ("HelveticaNeue-Bold", 14f);
			//UIFont fontRighttSide = UIFont.FromName ("Helvetica Neue", 14f);

			StyledStringElement colaboracion_a = new StyledStringElement ("Comune di Portogruaro", "");
			StyledStringElement colaboracion_b = new StyledStringElement ("ASCOM", "");
			StyledStringElement colaboracion_c = new StyledStringElement ("Confartigianato", "");
			StyledStringElement colaboracion_d = new StyledStringElement ("CNA", "");
			StyledStringElement colaboracion_e = new StyledStringElement ("Coldiretti", "");
			colaboracion_a.Font = fontLeftSide;
			colaboracion_a.SubtitleFont = fontRighttSide;

			colaboracion_b.Font = fontLeftSide;
			colaboracion_b.SubtitleFont = fontRighttSide;

			colaboracion_c.Font = fontLeftSide;
			colaboracion_c.SubtitleFont = fontRighttSide;

			colaboracion_d.Font = fontLeftSide;
			colaboracion_d.SubtitleFont = fontRighttSide;

			colaboracion_e.Font = fontLeftSide;
			colaboracion_e.SubtitleFont = fontRighttSide;

			colaboracion_a.Tapped += delegate {
				string urlStr = "http://www.comune.portogruaro.ve.it/";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
		};


			colaboracion_b.Tapped += delegate {
				string urlStr = "http://www.ascomportogruaro.it/";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
			};

			colaboracion_c.Tapped += delegate {
				string urlStr = "http://www.confartigianato.it/";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
			};

			colaboracion_d.Tapped += delegate {
				string urlStr = "http://www.cna.it/";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
			};

			colaboracion_e.Tapped += delegate {
				string urlStr = "http://www.coldiretti.it/Pagine/default.aspx";
				NSUrl url = new NSUrl(urlStr);
				if (!UIApplication.SharedApplication.OpenUrl(url)) {

				}
			};



			//comune di..
			//ascom
			//confartia..
			//CNA
			//coldiretti

			colaboracion_a.Image = UIImage.FromFile("images/about/portogruaro.png");
			colaboracion_b.Image = UIImage.FromFile("images/about/confcommercio.png");
			colaboracion_c.Image = UIImage.FromFile("images/about/Confartigianato.png");
			colaboracion_d.Image = UIImage.FromFile("images/about/CNA.png");
			colaboracion_e.Image = UIImage.FromFile("images/about/coldiretti.png");

			developerName.Image = UIImage.FromFile("images/about/TS.png");
			//developerName.Value = string.Empty;
			developerName.Caption = String.Empty;
			//developerName.su
			develoerEmail.Image = null;
			develoerWeb.Image = null;

			searchBoxDialog = new DialogViewController (new RootElement ("Iportuaro") {



				new Section (TopsectionHeaderTitle("Realizzata da")){
					developerName,develoerEmail,develoerWeb},
				new Section (sectionHeaderTitle("Da un'idea di")){
					ideamail_c},


				new Section (sectionHeaderTitle("In collaborazione con")){
					colaboracion_a,colaboracion_b,colaboracion_c,colaboracion_d}
				//colaboracion_e

			});

			//flightNumerEntry.KeyboardType = UIKeyboardType.Default;
			//searchBoxDialog.Root.UnevenRows = true;
			searchBoxDialog.TableView.BackgroundView = null;
			searchBoxDialog.TableView.BackgroundColor = UIColor.White;
			UINavigationController nav = new UINavigationController (searchBoxDialog);

			nav.NavigationBar.TintColor = UIColor.FromRGB (9, 76, 107);


			//this.PresentViewController (nav, true, null);
			searchBoxEntry.ReturnKeyType = UIReturnKeyType.Done;
		

			this.Add (searchBoxDialog.View);

		}
		#endregion

		#endregion

	}
}

