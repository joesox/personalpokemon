namespace PPokemon.Cards
{
    partial class CardEnergyPanel
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
            this.pBoxEnergyCard = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxEnergyCard)).BeginInit();
            this.SuspendLayout();
            // 
            // pBoxEnergyCard
            // 
            this.pBoxEnergyCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pBoxEnergyCard.InitialImage = null;
            this.pBoxEnergyCard.Location = new System.Drawing.Point(3, 3);
            this.pBoxEnergyCard.Name = "pBoxEnergyCard";
            this.pBoxEnergyCard.Size = new System.Drawing.Size(220, 342);
            this.pBoxEnergyCard.TabIndex = 0;
            this.pBoxEnergyCard.TabStop = false;
            this.pBoxEnergyCard.Layout += new System.Windows.Forms.LayoutEventHandler(this.pBoxEnergyCard_Layout);
            // 
            // CardEnergyPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pBoxEnergyCard);
            this.Name = "CardEnergyPanel";
            this.Size = new System.Drawing.Size(226, 346);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxEnergyCard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxEnergyCard;
    }
}
