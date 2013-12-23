using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScreenSaver
{
    public partial class PetriDishForm : Form
    {
        #region DLL calls
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        #endregion

        private Point mouseLocation; // initializes to empty?
        private Random rand = new Random(); // not using this for cryptography so it doesn't have to be really random
        private bool previewMode = false; // set to true if we're previewing, so we don't pay attention to the mouse

        #region Constructors
        // Default constructor not used here
        //public ScreenSaverForm()
        //{
        //    InitializeComponent();
        //}

        public PetriDishForm(IntPtr hWnd)
        {
            // Preview mode: set the preview window as the parent of this window
            InitializeComponent();

            // Reparent the this window
            SetParent(this.Handle, hWnd);

            // Make this a child window, so it will close with parent window
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside theparent
            Rectangle parentRect;
            GetClientRect(hWnd, out parentRect);
            this.Size = parentRect.Size;
            this.Location = new Point(0, 0); // right at the top

            // Drop the text size (but I don't want to change the font)
            textLabel.Font = new System.Drawing.Font(textLabel.Font.Name.ToString(),(float) 6.0);

            previewMode = true;
        }

        public PetriDishForm(Rectangle theBounds)
        {
            InitializeComponent();
            this.Bounds = theBounds;
        }
        #endregion
        
        #region Form Events
        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            moveTimer.Interval = 3000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (previewMode) return;

            if (!mouseLocation.IsEmpty)
            {
                // Terminate if mouse moves (but only if it moves enough)
                if (Math.Abs(mouseLocation.X - e.X) > 5 || Math.Abs(mouseLocation.Y - e.Y) > 5)
                    Application.Exit();
            }

            // If we haven't moved before (or haven't moved much), update position
            mouseLocation = e.Location;
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (previewMode) return;

            // Just exit
            Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (previewMode) return;

            // Just exit
            Application.Exit();
        }
        
        #endregion

        #region Control Events

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            // Move text to new location
            textLabel.Left = rand.Next(Math.Max(1, Bounds.Width - textLabel.Width));
            textLabel.Top = rand.Next(Math.Max(1, Bounds.Height - textLabel.Height));    
        }

        #endregion
        
    }
}
