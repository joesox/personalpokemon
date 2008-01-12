using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PPokemon.Cards;

namespace PPokemon
{
    public partial class CardPopUpForm : Form
    {
        public CardPopUpForm()
        {
            InitializeComponent();
        }

        private void CardPopUpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Remove the control so it doesn't get destroyed (assumed last one added)
            this.Controls.RemoveAt(this.Controls.Count - 1);
        }

        public void DisplayCurrentStatus()
        {
            CardHolder tempCH = ((CardHolder)this.Tag);
            btRetreat.Enabled = false;

            if (tempCH.HasItem)
            {
                #region Display AttachedEnergy...
                foreach (Card card in tempCH.CurrentCard.State.AttachedCards)
                {
                    if (card.Properties.CardType == Cardtype.Energy)
                    {
                        if (pBoxEnergy1.Image == null)
                        {
                            pBoxEnergy1.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy2.Image == null)
                        {
                            pBoxEnergy2.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy3.Image == null)
                        {
                            pBoxEnergy3.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy4.Image == null)
                        {
                            pBoxEnergy4.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy5.Image == null)
                        {
                            pBoxEnergy5.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy6.Image == null)
                        {
                            pBoxEnergy6.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                        else if (pBoxEnergy7.Image == null)
                        {
                            pBoxEnergy7.Image = Ops.GetEnergyTypeImage(card.Properties.PokemonType);
                        }
                    }
                }
                #endregion

                lbDamage.Text = tempCH.CurrentCard.State.CurrentDamage.ToString();
                lbCondition.Text = "Condition: " + tempCH.CurrentCard.State.CurrentCondition.ToString();

                //See if it can be retreated
                
                if (tempCH.CurrentCard.State.ActivePokemon)
                {
                    //Display Retreat button
                    if (tempCH.CurrentCard.State.GetAttachedEnergyCardCount() >= tempCH.CurrentCard.Properties.RetreatCost.Second)
                    {
                        btRetreat.Enabled = true;
                    }
                }
            }
        }

        private void btRetreat_Click(object sender, EventArgs e)
        {
            //Do something
            this.Close();
        }
    }
}
