using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mint.Controls
{
    public class DoubleBufferedListView : ListView
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        public DoubleBufferedListView()
        {
            DoubleBuffered = true;
        }
        
        //This is to prevent the listview from scrolling when the mouse is over it
        //It passes the scroll event to the parent panel
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                Control parent = Parent;
                while (parent != null)
                {
                    if (parent is Panel panel)
                    {
                        SendMessage(panel.Handle, m.Msg, m.WParam, m.LParam);
                        return;
                    }
                    parent = parent.Parent;
                }
            }

            base.WndProc(ref m);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
