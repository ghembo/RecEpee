using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace RecEpee.Utilities
{
    class Printer
    {
        public static void PrintHtmlFile(string TempExportPath)
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += (a, b) => webBrowser.ShowPrintDialog();
            webBrowser.Url = new System.Uri(TempExportPath);            
        }

        public static void ShowPrintPreviewForHtmlFile(string TempExportPath)
        {            
            WebBrowser webBrowser = new WebBrowser();

            webBrowser.Parent = GetFakeParentWindow();
            webBrowser.DocumentCompleted += (a, b) => webBrowser.ShowPrintPreviewDialog();
            webBrowser.Url = new System.Uri(TempExportPath);            
        }

        private static Form GetFakeParentWindow()
        {
            Form window = new Form();
            window.Visible = false;
            window.BackColor = Color.Lime;
            window.TransparencyKey = Color.Lime;
            window.FormBorderStyle = FormBorderStyle.None;
            window.Width = PointsToPixels(System.Windows.Application.Current.MainWindow.ActualWidth, LengthDirection.Horizontal);
            window.Height = PointsToPixels(System.Windows.Application.Current.MainWindow.ActualWidth, LengthDirection.Vertical);
            window.Show();
            window.Hide();
            return window;
        }

        private static int PointsToPixels(double wpfPoints, LengthDirection direction)
        {
            if (direction == LengthDirection.Horizontal)
            {
                return (int)Math.Round(wpfPoints * Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.WorkArea.Width);
            }
            else
            {
                return (int)Math.Round(wpfPoints * Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.WorkArea.Height);
            }
        }

        public enum LengthDirection
        {
            Vertical,
            Horizontal
        }
    }
}
