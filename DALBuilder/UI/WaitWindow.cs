using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DALBuilder.UI
{
     public partial class WaitWindow : Form
     {
          public WaitWindow(string message)
          {
               InitializeComponent();
               lblMessage.Text = message;
               
          }

     }
}