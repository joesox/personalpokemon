using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PPokemon.Cards
{
    public partial class CardPanel : UserControl
    {
        public CardPanel()
        {
            InitializeComponent();
        }

        private void CardPanel_Load(object sender, EventArgs e)
        {
            CardProperties cardprop = new CardProperties();
            cardprop = ((CardProperties)this.Tag);

            lbName.Text = cardprop.Pokemon.Name;
            lbHP.Text = cardprop.HP.ToString() + " HP";

            #region PokemonTYPE
            switch ((PokemonTYPE)cardprop.PokemonType)
            {
                case PokemonTYPE.Colorless:
                    pBoxType.Image = PPokemon.Properties.Resources.colorless;
                    break;
                case PokemonTYPE.Null:
                    pBoxType.Image = PPokemon.Properties.Resources._null;
                    break;
                case PokemonTYPE.Fighting:
                    pBoxType.Image = PPokemon.Properties.Resources.fighting;
                    break;
                case PokemonTYPE.Fire:
                    pBoxType.Image = PPokemon.Properties.Resources.fire;
                    break;
                case PokemonTYPE.Grass:
                    pBoxType.Image = PPokemon.Properties.Resources.grass;
                    break;
                case PokemonTYPE.Lightning:
                    pBoxType.Image = PPokemon.Properties.Resources.lightning;
                    break;
                case PokemonTYPE.Psychic:
                    pBoxType.Image = PPokemon.Properties.Resources.psychic;
                    break;
                case PokemonTYPE.Water:
                    pBoxType.Image = PPokemon.Properties.Resources.water;
                    break;
                default:
                    break;
            }
            #endregion PokemonTYPE

            //Draw the image
            webBCard.Navigate(cardprop.Pokemon.LocalSWF);

            #region CardType
            switch (cardprop.CardType)
            {
                case Cardtype.Null:
                    lbStage.Text = "Unknown";
                    break;
                case Cardtype.Basic:
                    lbStage.Text = "Basic";
                    break;
                case Cardtype.Stage1:
                    lbStage.Text = "Stage 1";
                    break;
                case Cardtype.Stage2:
                    lbStage.Text = "Stage 2";
                    break;
                case Cardtype.LevelUp:
                    lbStage.Text = "Level-Up";
                    break;
                default:
                    break;
            }
            #endregion CardType

            //Control what Attack space to write in
            int spacecount = 0;
            #region PowerDrawing
            //If there is a Poke-Power/Body, write it in the first Attack Space
            if (cardprop.PokePower.Title != "" && cardprop.Attacks.First.Count <= 1)
            {
                pBoxAttack1_1.Visible = false;
                lbAttack1Damage.Visible = false;
                lbAttack1Title.Text = cardprop.PokePower.Title;
                lbAttack1Body.Text = cardprop.PokePower.Body;
                spacecount = 1;
            }
            #endregion

            #region AttackDrawing
            //Draw each attack
            //Clear all lbAttack1Body
            lbAttack1Damage.Text = "";
            lbAttack1Title.Text = "";
            lbAttack1Body.Text = "";
            //Clear all lbAttack2Body
            lbAttack2Damage.Text = "";
            lbAttack2Title.Text = "";
            lbAttack2Body.Text = "";
            Dictionary<string, AttackProperties>.Enumerator AllAttackProps = cardprop.Attacks.First.GetEnumerator();
            while (AllAttackProps.MoveNext())//{[Ram, PPokemon.Cards.AttackProperties]}
            {
                if (spacecount == 0)
                {
                    IEnumerator<AttackDetails> detailsEnum = AllAttackProps.Current.Value.AttackDict.Values.GetEnumerator();
                    //Draw every energy needed for the attack
                    for (int x = 0; x < AllAttackProps.Current.Value.GetEnergyTotalCount(); x++)//1
                    {
                        detailsEnum.MoveNext();//Move to the next AttackDetails
                        switch (x)
                        {
                            case 0:
                                pBoxAttack1_1.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            case 1:
                                pBoxAttack1_2.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            case 2:
                                pBoxAttack1_3.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            default:
                                break;
                        }
                    }
                    lbAttack1Damage.Text = AllAttackProps.Current.Value.Damage.ToString();
                    lbAttack1Title.Text = AllAttackProps.Current.Value.Title;
                    lbAttack1Body.Text = AllAttackProps.Current.Value.Body;
                }
                if (spacecount == 1)
                {
                    IEnumerator<AttackDetails> detailsEnum = AllAttackProps.Current.Value.AttackDict.Values.GetEnumerator();
                    //Draw every energy needed for the attack
                    for (int y = 0; y < AllAttackProps.Current.Value.GetEnergyTotalCount(); y++)//1 //NOT TOTALING dict values
                    {
                        detailsEnum.MoveNext();//Move to the next AttackDetails
                        switch (y)
                        {
                            case 0:
                                pBoxAttack2_1.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);//{PPokemon.Cards.AttackProperties}[0]
                                break;
                            case 1:
                                pBoxAttack2_2.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            case 2:
                                pBoxAttack2_3.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            case 3:
                                pBoxAttack2_4.Image = Ops.GetEnergyTypeImage(detailsEnum.Current.EnergyRequired.First);
                                break;
                            default:
                                break;
                        }
                    }
                    lbAttack2Damage.Text = AllAttackProps.Current.Value.Damage.ToString();
                    lbAttack2Title.Text = AllAttackProps.Current.Value.Title;
                    lbAttack2Body.Text = AllAttackProps.Current.Value.Body;
                }

                spacecount = spacecount + 1;
            }
            #endregion AttackDrawing

            #region Draw bottom of card
            pBoxWeakness1.Image = Ops.GetEnergyTypeImage(cardprop.Weakness.First);

            pBoxResistance1.Image = Ops.GetEnergyTypeImage(cardprop.Resistance.First);

            if (cardprop.RetreatCost.Second == 0)
            {
                pBoxRetreat1.Image = null;
                pBoxRetreat2.Image = null;
                pBoxRetreat3.Image = null;
                pBoxRetreat4.Image = null;
            }
            else if (cardprop.RetreatCost.Second == 1)
            {
                pBoxRetreat1.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat2.Image = null;
                pBoxRetreat3.Image = null;
                pBoxRetreat4.Image = null;
            }
            else if (cardprop.RetreatCost.Second == 2)
            {
                pBoxRetreat1.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat2.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat3.Image = null;
                pBoxRetreat4.Image = null;
            }
            else if (cardprop.RetreatCost.Second == 3)
            {
                pBoxRetreat1.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat2.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat3.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat4.Image = null;
            }
            else if (cardprop.RetreatCost.Second == 4)
            {
                pBoxRetreat1.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat2.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat3.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
                pBoxRetreat4.Image = Ops.GetEnergyTypeImage(cardprop.RetreatCost.First);
            }
            #endregion Draw bottom of card
        }

    }
}
