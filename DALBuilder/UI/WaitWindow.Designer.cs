namespace DALBuilder.UI
{
     partial class WaitWindow
     {
          /// <summary>
          /// Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          /// Clean up any resources being used.
          /// </summary>
          /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
          protected override void Dispose(bool disposing)
          {
               if (disposing && (components != null))
               {
                    components.Dispose();
               }
               base.Dispose(disposing);
          }

          #region Windows Form Designer generated code

          /// <summary>
          /// Required method for Designer support - do not modify
          /// the contents of this method with the code editor.
          /// </summary>
          private void InitializeComponent()
          {
               this.lblMessage = new System.Windows.Forms.Label();
               this.SuspendLayout();
               // 
               // lblMessage
               // 
               this.lblMessage.AutoSize = true;
               this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.lblMessage.Location = new System.Drawing.Point(31, 25);
               this.lblMessage.Name = "lblMessage";
               this.lblMessage.Size = new System.Drawing.Size(354, 16);
               this.lblMessage.TabIndex = 0;
               this.lblMessage.Text = "Trying to locate SQL Servers in your network. Please wait...";
               // 
               // WaitWindow
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(407, 82);
               this.ControlBox = false;
               this.Controls.Add(this.lblMessage);
               this.Name = "WaitWindow";
               this.Text = "Loading Servers";
               this.ResumeLayout(false);
               this.PerformLayout();

          }

          #endregion

          private System.Windows.Forms.Label lblMessage;
     }
}