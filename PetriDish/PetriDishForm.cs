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
        private Bitmap screenBitmap = null; // set to the way the screen looks before the scrsaver starts

        #region Constructors
        // Default constructor not used here
        //public PetriDishForm()
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

        public PetriDishForm(Screen aScreen)
        {
            InitializeComponent();

            this.Bounds = aScreen.Bounds; // get w/h

            Bitmap screenBitmap = new Bitmap(this.Size.Width, this.Size.Height);
            Graphics myGraphics = Graphics.FromImage(screenBitmap);
            // From X/Y is 0,0 for screen 1 - but not for screen 2!
            myGraphics.CopyFromScreen(aScreen.Bounds.X, aScreen.Bounds.Y, 0, 0, this.Size, CopyPixelOperation.SourceCopy); // from 0,0 to 0,0

            this.BackgroundImage = screenBitmap;
            //this.CreateGraphics().DrawImageUnscaled(screenBitmap, 0, 0);

        }
        #endregion
        
        #region Form Events
        private void PetriDishForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            // My size has already been set by the constructor

            moveTimer.Interval = 1000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

        }

        #endregion

        #region Generic Screen Saver Events

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
            int newX;
            int newY;

            Graphics screenGraphics = this.CreateGraphics();
            
            //newX = rand.Next(Math.Max(1, Bounds.Width - textLabel.Width));
            //newY = rand.Next(Math.Max(1, Bounds.Height - textLabel.Height));
            //// Move text to new location
            //textLabel.Left = newX;
            //textLabel.Top = newY;
            //textLabel.Text = string.Format("{0}, {1}", newX, newY);

            //Graphics myOutput = this.CreateGraphics();
            //myOutput.DrawImage(screenBitmap, new Point(0, 0));
            //myOutput.FillRectangle(Brushes.DarkBlue, new Rectangle(5, 5, 10, 10));

            // Get the contents of a random rectangle
            //newX = rand.Next(Math.Max(1, this.Bounds.Width - 10));
            //newY = rand.Next(Math.Max(1, this.Bounds.Height - 10));

            //Bitmap savedBits = new Bitmap(20, 20);
            //Graphics g = Graphics.FromImage(savedBits);
            

            // Move them to a random spot!
            newX = rand.Next(Math.Max(1, this.Bounds.Width - 10));
            newY = rand.Next(Math.Max(1, this.Bounds.Height - 10));
            screenGraphics.FillRectangle(Brushes.Red, new Rectangle(newX, newY, 10, 10));
        }

        #endregion

    }
}
