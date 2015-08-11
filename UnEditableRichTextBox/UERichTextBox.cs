using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnEditableRichTextBox
{
    public partial class UERichTextBox : RichTextBox
    {
        public UERichTextBox()
        {
            InitializeComponent();
            this.BackColor = Color.White;
        }

        //Makes all user input ignored
        protected override void  OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
