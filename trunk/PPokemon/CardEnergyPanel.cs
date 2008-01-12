using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PPokemon.Cards
{
    public partial class CardEnergyPanel : UserControl
    {
        public CardEnergyPanel()
        {
            InitializeComponent();
        }

        private void pBoxEnergyCard_Layout(object sender, LayoutEventArgs e)
        {
            CardProperties cardprop = new CardProperties();
            cardprop = ((CardProperties)this.Tag);

            pBoxEnergyCard.Image = new Bitmap(Ops.GetEnergyCardImage(cardprop.PokemonType), 221, 341);
        }
    }
}
