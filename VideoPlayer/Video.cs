using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoPlayer
{
    public partial class Video : UserControl
    {
        private LinkedList<Bitmap> bitmapList;
        private bool playVideo;
        public Video()
        {
            InitializeComponent();
            bitmapList = new LinkedList<Bitmap>();
            playVideo = false;
            this.videoPlayBack.BackgroundImageLayout = ImageLayout.Center;
        }
        public void addBitmapToEnd(Bitmap bmp)
        {
            
            bitmapList.AddLast(bmp);
        }

        private Bitmap removeBitmapToDisplay()
        {
            if (bitmapList.First != null)
            {
                Bitmap bmp = bitmapList.First.Value;
                bitmapList.RemoveFirst();
                return bmp;
            }
            else
                return null;
        }
        public void startPlayBack()
        {
            displayTimer.Enabled = true;
            playVideo = true;
        }
        public void pausePlayBack()
        {
            displayTimer.Enabled = false;
            playVideo = false;
        }

        public void teardownVideo()
        {
            int lengthOfList = bitmapList.Count;
            playVideo = false;
            for (int i = 0; i < lengthOfList; i++)
            {
                bitmapList.RemoveFirst();
            }
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            if (playVideo == true)
            {
                this.videoPlayBack.Image = (Image)this.removeBitmapToDisplay();
                this.videoPlayBack.Update();
            }
        }
    }
}
