namespace PPokemon.Cards
{
    partial class CardTrainerPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardTrainerPanel));
            this.lbCardTitle = new System.Windows.Forms.Label();
            this.lbCardName = new System.Windows.Forms.Label();
            this.webBCardWindow = new System.Windows.Forms.WebBrowser();
            this.lbCardBody = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbCardTitle
            // 
            this.lbCardTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCardTitle.Location = new System.Drawing.Point(88, 27);
            this.lbCardTitle.Name = "lbCardTitle";
            this.lbCardTitle.Size = new System.Drawing.Size(131, 15);
            this.lbCardTitle.TabIndex = 8;
            this.lbCardTitle.Text = "Poke-Power Excavate";
            // 
            // lbCardName
            // 
            this.lbCardName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCardName.Location = new System.Drawing.Point(7, 9);
            this.lbCardName.Name = "lbCardName";
            this.lbCardName.Size = new System.Drawing.Size(100, 18);
            this.lbCardName.TabIndex = 7;
            this.lbCardName.Text = "ABCDEFGHIJK";
            // 
            // webBCardWindow
            // 
            this.webBCardWindow.AllowNavigation = false;
            this.webBCardWindow.AllowWebBrowserDrop = false;
            this.webBCardWindow.Location = new System.Drawing.Point(10, 54);
            this.webBCardWindow.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBCardWindow.Name = "webBCardWindow";
            this.webBCardWindow.ScriptErrorsSuppressed = true;
            this.webBCardWindow.ScrollBarsEnabled = false;
            this.webBCardWindow.Size = new System.Drawing.Size(209, 95);
            this.webBCardWindow.TabIndex = 6;
            // 
            // lbCardBody
            // 
            this.lbCardBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCardBody.Location = new System.Drawing.Point(6, 176);
            this.lbCardBody.Name = "lbCardBody";
            this.lbCardBody.Size = new System.Drawing.Size(217, 149);
            this.lbCardBody.TabIndex = 9;
            this.lbCardBody.Text = resources.GetString("lbCardBody.Text");
            // 
            // CardTrainerPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbCardBody);
            this.Controls.Add(this.lbCardTitle);
            this.Controls.Add(this.lbCardName);
            this.Controls.Add(this.webBCardWindow);
            this.Name = "CardTrainerPanel";
            this.Size = new System.Drawing.Size(226, 346);
            this.Load += new System.EventHandler(this.CardTrainerPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbCardTitle;
        private System.Windows.Forms.Label lbCardName;
        private System.Windows.Forms.WebBrowser webBCardWindow;
        private System.Windows.Forms.Label lbCardBody;
    }
}
