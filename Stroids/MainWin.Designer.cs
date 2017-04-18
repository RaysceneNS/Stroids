using System.ComponentModel;
using System.Windows.Forms;

namespace Stroids
{
    partial class MainWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.frame1 = new System.Windows.Forms.PictureBox();
            this.frame2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.frame1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frame2)).BeginInit();
            this.SuspendLayout();
            // 
            // frame1
            // 
            this.frame1.BackColor = System.Drawing.SystemColors.WindowText;
            this.frame1.Location = new System.Drawing.Point(8, 8);
            this.frame1.Name = "frame1";
            this.frame1.Size = new System.Drawing.Size(100, 50);
            this.frame1.TabIndex = 0;
            this.frame1.TabStop = false;
            this.frame1.Paint += new System.Windows.Forms.PaintEventHandler(this.Frame_Paint);
            // 
            // frame2
            // 
            this.frame2.BackColor = System.Drawing.SystemColors.WindowText;
            this.frame2.Location = new System.Drawing.Point(8, 72);
            this.frame2.Name = "frame2";
            this.frame2.Size = new System.Drawing.Size(100, 50);
            this.frame2.TabIndex = 1;
            this.frame2.TabStop = false;
            this.frame2.Paint += new System.Windows.Forms.PaintEventHandler(this.Frame_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.ControlBox = false;
            this.Controls.Add(this.frame2);
            this.Controls.Add(this.frame1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MainWin_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWin_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainWin_KeyUp);
            this.Resize += new System.EventHandler(this.MainWin_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.frame1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frame2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox frame1;
        private PictureBox frame2;
    }
}

