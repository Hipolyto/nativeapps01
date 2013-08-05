
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iportogruaroLibraryShared;
namespace iportogruaroIOS
{
    
    public class cmbShare : UIPickerViewModel
    {

        UIViewController app;
        private string[] tasks;
        public cmbShare (UIViewController register,string[] currenttasks)
        {
            app = register;
            tasks = currenttasks;
            //currentcurrentdestinations = new ControllerDestinations().getAllDestinations();
        }
        
        public override int GetComponentCount (UIPickerView picker)
        {
            return 1;
        }
        
        public override int GetRowsInComponent (UIPickerView picker, int component)
        {
            return tasks.Length;// .Count;
        }
        
        public override string GetTitle (UIPickerView picker, int row, int component)
        {
            //app.UpdatecurrenShareOpt = row; 
            return tasks [row];
        }
        
        public override void Selected (UIPickerView picker, int row, int component)
        {
            //app.loadNewAvability(row);
			if (app is UiDetailScreen)
				(app as UiDetailScreen).UpdatecurrenShareOpt = row;  
			else
				(app as UiGaleryScreen).UpdatecurrenShareOpt = row;  
        }
    }
}
