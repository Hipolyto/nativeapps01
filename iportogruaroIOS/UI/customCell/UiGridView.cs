using System;
using MonoTouch.UIKit;

namespace iportogruaroIOS
{
    public class UiGridView
    {
        public UiGridView ()
        {
        }
        /*
        float pageControlHeight = 18.0f;
        UIScrollView scrollView;
        UIPageControl pageControl;
        public void BuildAndReloadUI(List<Marker> markers, int favCount)
        {

            Util.TurnOffNetworkActivity();


            CleanView();

            if (markers.Count == 0) 
            {
                ShowNoStops();


                return;
            }  


            Util.Log("Building UI");

            int pageCount = markers.Count;

            var scrollViewRect = View.Bounds;
            scrollViewRect.Size = new SizeF(scrollViewRect.Size.Width, scrollViewRect.Size.Height - pageControlHeight);


            scrollView = new UIScrollView(scrollViewRect);
            scrollView.PagingEnabled = true;
            scrollView.ContentSize = new System.Drawing.SizeF(scrollViewRect.Size.Width * pageCount, 1);
            scrollView.ShowsHorizontalScrollIndicator = false;
            scrollView.ShowsVerticalScrollIndicator = false;
            scrollView.BackgroundColor = UIColor.Black;


            scrollView.Scrolled += HandleScrollViewScrolled;

            var pageViewRect = View.Bounds;
            pageViewRect.Size = new SizeF(pageViewRect.Size.Width, pageControlHeight);
            pageViewRect.Y = scrollViewRect.Size.Height;

            pageControl = new UIPageControl(pageViewRect);
            pageControl.BackgroundColor = UIColor.Black;
            pageControl.Pages = pageCount;
            pageControl.CurrentPage = 0;
            pageControl.ValueChanged += HandlePageControlValueChanged;

            //setup the page items

            int tempPage = 0;
            //if (stops != null) 
            //{
            //      foreach(var stop in stops) 
            //      {
            //              stop.View.RemoveFromSuperview();
            //      }
            //}


            stops = new List<SingleStopViewController>();


            foreach(var marker in markers)
            {
                Util.Log("Building board: {0} {1}", tempPage, marker.Name);
                var bounds = scrollView.Bounds;
                bounds.X = bounds.Size.Width * tempPage;
                bounds.Y = 0;

                bounds.Width = 320;

                SingleStopViewController currentStop = new SingleStopViewController(marker, bounds, markers);
                currentStop.TheParent = this;
                if (tempPage < favCount) 
                {
                    currentStop.InitialDelay = 1000;
                } else {
                    currentStop.InitialDelay = (tempPage - favCount) * 500;
                }
                scrollView.AddSubview(currentStop.View);
                stops.Add(currentStop);


                tempPage++;
            }

            View.AddSubview(scrollView);
            View.AddSubview(pageControl);
            View.BringSubviewToFront(pageControl);

            SetInitialPage(favCount);

            //View.SetNeedsLayout();

        }

        public override bool CanBecomeFirstResponder
        {
            get
            {
                return true;
            }
        }

        public void SetInitialPage(int page)
        {
            pageControl.CurrentPage = page;
            var frame = scrollView.Frame;
            frame.X = frame.Size.Width * page;
            frame.Y = 0;

            scrollView.ScrollRectToVisible(frame, false);
        }

        void HandlePageControlValueChanged (object sender, EventArgs e)
        {
            int page = pageControl.CurrentPage;
            var frame = scrollView.Frame;
            frame.X = frame.Size.Width * page;
            frame.Y = 0;

            scrollView.ScrollRectToVisible(frame, true);
        }

        void HandleScrollViewScrolled (object sender, EventArgs e)
        {
            var sv = sender as UIScrollView;
            if (sv != null) 
            {
                double pageWidth = sv.Frame.Size.Width;
                int page = (int)Math.Floor((sv.ContentOffset.X - pageWidth / 2) / pageWidth) + 1;
                pageControl.CurrentPage = page;
            }
        }
        */
    }
}

