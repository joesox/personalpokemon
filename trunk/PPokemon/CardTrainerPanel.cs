using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PPokemon.Cards
{
    public partial class CardTrainerPanel : UserControl
    {
        public CardTrainerPanel()
        {
            InitializeComponent();
        }

        private void CardTrainerPanel_Load(object sender, EventArgs e)
        {
            CardProperties cardprop = new CardProperties();
            cardprop = ((CardProperties)this.Tag);

            lbCardName.Text = cardprop.CardType.ToString();
            

            Dictionary<string, AttackProperties>.Enumerator AllAttackProps = cardprop.Attacks.First.GetEnumerator();
            AllAttackProps.MoveNext();
            IEnumerator<AttackDetails> detailsEnum = AllAttackProps.Current.Value.AttackDict.Values.GetEnumerator();
            lbCardTitle.Text = AllAttackProps.Current.Value.Title;
            lbCardBody.Text = AllAttackProps.Current.Value.Body;
        }
    }
}
