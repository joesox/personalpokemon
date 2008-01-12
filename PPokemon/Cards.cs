///////////
//http://www.akadia.com/services/dotnet_delegates_and_events.html#The%20very%20basic%20Delegate
//TODO: Finish Attacks and Pokepower objects
//      Finish the constructor
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Joe.Utils;
using System.IO;
using System.Collections.Specialized;
using System.Xml;
using System.Reflection;
using System.Resources;
using System.Drawing.Drawing2D;
using Wintellect.PowerCollections;
using System.Xml.XPath;

namespace PPokemon.Cards
{
    /// <summary>
    /// Used for attaching CardPanels
    /// </summary>
    public enum Cardtype { Null, Basic, Stage1, Stage2, LevelUp, Energy, Trainer, Supporter, Stadium };
    public enum PokemonTYPE { Colorless, Darkness, Fighting, Fire, Grass, Lightning, Metal, Null, Psychic, Water };
    public enum SpecialConditions { None, Asleep, Burned, Confused, Paralyzed, Poisoned };
    /// <summary>
    /// Currently built-in attack directions
    /// http://code.google.com/p/personalpokemon/wiki/CreatingCards
    /// </summary>
    public enum CardEventNames
    {
        AddDamageToAttack,
        AddDamageToBenched,
        AddDamageToEachBenched,
        AddDamageToSelf,
        AddSpecialCToAttack,
        AttachCardToAPokemon,
        ChooseCardsFromDrawn,
        ChooseCardFromHandPlaceInDeck,
        ChooseOpponPokemon,
        ChoosePokemon,
        DiscardAnEnergy,
        DiscardSpecialEnergyCard,
        DrawACard,
        FlipACoin,
        IfActivePokemon,
        IfDefendingDamaged,
        IfDefendingHasPowers,
        IfDamaged,
        IfFlipHeads,
        IfFlipTails,
        IfKnockedOut,
        IfLessEnergyThanDefending,
        IfMorePrizeCards,
        IfNextDamageLessThan,
        IfNoSpecialC,
        MinusAmountMultipliedByAttachedDamage,
        MultiplyDamageByNumberOfPokemonInPlay,
        MultiplyByDamageOnSelf,
        MultiplyByNonUsedEnergyCount,
        None,
        OpponentChooseCardsFromDrawn,
        OpponentNextAttack,
        RemoveDamageCounter,
        ResistenceOverride,
        SearchDeckForBasic,
        SearchDeckForBasicOrEvolution,
        SearchDeckForBasicPlaceToBench,
        SearchDeckForEnergy,
        SearchDeckForPokemon,
        SelectAllPokemonType,
        ShuffleDeck,
        SwitchDefendingWithBenched,
        SwitchWithBenched
    };

    /// <summary>
    /// Groups of CardEvents to use for Trainer/Supporter Planner/Decision System
    /// </summary>
    public enum CardNeedActions
    {
        AttackOffense,
        AttackDefense,
        Energy,
        Null,
        Pokemon,
        RemoveDamage,
        RemoveSpecialC
    }

    public class TrainerPlanner
    {
        /// <summary>
        /// Associates CardNeedActions with CardEventNames
        /// </summary>
        public Wintellect.PowerCollections.MultiDictionary<CardNeedActions, CardEventNames> PokemonNeedsDict = new MultiDictionary<CardNeedActions, CardEventNames>(true);
        
        public TrainerPlanner()
        {
            //Create the Classifications...
            //Associate the CardEventNames...
            PokemonNeedsDict.Add(CardNeedActions.AttackDefense, CardEventNames.IfDamaged);
            PokemonNeedsDict.Add(CardNeedActions.AttackDefense, CardEventNames.ResistenceOverride);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.AddDamageToAttack);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.DiscardAnEnergy);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.DiscardSpecialEnergyCard);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.AddDamageToBenched);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.AddSpecialCToAttack);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.MultiplyDamageByNumberOfPokemonInPlay);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.MultiplyByDamageOnSelf);
            PokemonNeedsDict.Add(CardNeedActions.AttackOffense, CardEventNames.MultiplyByNonUsedEnergyCount);
            PokemonNeedsDict.Add(CardNeedActions.Energy, CardEventNames.SearchDeckForEnergy);
            PokemonNeedsDict.Add(CardNeedActions.Energy, CardEventNames.DrawACard);
            PokemonNeedsDict.Add(CardNeedActions.Pokemon, CardEventNames.DrawACard);
            PokemonNeedsDict.Add(CardNeedActions.Pokemon, CardEventNames.SearchDeckForBasic);
            PokemonNeedsDict.Add(CardNeedActions.Pokemon, CardEventNames.SearchDeckForBasicOrEvolution);
            PokemonNeedsDict.Add(CardNeedActions.Pokemon, CardEventNames.SearchDeckForBasicPlaceToBench);
            PokemonNeedsDict.Add(CardNeedActions.Pokemon, CardEventNames.SearchDeckForPokemon);
            PokemonNeedsDict.Add(CardNeedActions.RemoveDamage, CardEventNames.RemoveDamageCounter);
            PokemonNeedsDict.Add(CardNeedActions.RemoveSpecialC, CardEventNames.SearchDeckForBasicOrEvolution);//Evolve removes specialC

            /* NOT Used yet....
        AddDamageToSelf,
        AttachCardToAPokemon,
        ChooseCardsFromDrawn,
        ChooseCardFromHandPlaceInDeck,
        ChooseOpponPokemon,
        ChoosePokemon,
        DrawACard,
        FlipACoin,
        IfActivePokemon,
        IfDefendingDamaged,
        IfDefendingHasPowers,
        IfFlipHeads,
        IfFlipTails,
        IfKnockedOut,
        IfLessEnergyThanDefending,
        IfMorePrizeCards,
        IfNextDamageLessThan,
        IfNoSpecialC,
        MinusAmountMultipliedByAttachedDamage,
        OpponentChooseCardsFromDrawn,
        OpponentNextAttack,
        SelectAllPokemonType,
        ShuffleDeck,
        SwitchDefendingWithBenched,
        SwitchWithBenched
             */
        }

        public bool IsCardNeeded(CardNeedActions actionneed, Card nonpokecard, AttackProperties attack)
        {
            bool cardneeded = false;
            CardDirections cardDir = new CardDirections();
            cardDir = nonpokecard.GetAttackDirections(attack.Title);
            foreach (Wintellect.PowerCollections.Pair<CardEventNames, string> pair in cardDir.Directions)
            {
                if (this.PokemonNeedsDict.Contains(actionneed, pair.First))
                {
                    cardneeded = true;
                    break;
                }

                //Read all the arguments
                //StringCollection sc = new StringCollection();
                //sc = Ops.ReadCardEventNamesArgs(pair.Second);
            }
            return cardneeded;
        }
    }

    public class Ops
    {
        /// <summary>
        /// Get this attackproperty's image
        /// </summary>
        /// <returns></returns>
        public static Image GetEnergyTypeImage(PokemonTYPE ptype)
        {
            Image image = Image.FromHbitmap(PPokemon.Properties.Resources._null.GetHbitmap());
            // foreach (KeyValuePair<string, AttackDetails> detailPair in attackDictValues)
            // {

            switch (ptype)
            {
                case PokemonTYPE.Colorless:
                    image = PPokemon.Properties.Resources.colorless;
                    break;
                case PokemonTYPE.Darkness:
                    image = PPokemon.Properties.Resources.darkness;
                    break;
                case PokemonTYPE.Fighting:
                    image = PPokemon.Properties.Resources.fighting;
                    break;
                case PokemonTYPE.Fire:
                    image = PPokemon.Properties.Resources.fire;
                    break;
                case PokemonTYPE.Grass:
                    image = PPokemon.Properties.Resources.grass;
                    break;
                case PokemonTYPE.Lightning:
                    image = PPokemon.Properties.Resources.lightning;
                    break;
                case PokemonTYPE.Metal:
                    image = PPokemon.Properties.Resources.metal;
                    break;
                case PokemonTYPE.Null:
                    image = PPokemon.Properties.Resources._null;
                    break;
                case PokemonTYPE.Psychic:
                    image = PPokemon.Properties.Resources.psychic;
                    break;
                case PokemonTYPE.Water:
                    image = PPokemon.Properties.Resources.water;
                    break;
                default:
                    break;
            }
            // }
            return image;
        }

        /// <summary>
        /// Get this Energy Card's image
        /// </summary>
        /// <returns></returns>
        public static Image GetEnergyCardImage(PokemonTYPE ptype)
        {
            Image image = Image.FromHbitmap(PPokemon.Properties.Resources._null.GetHbitmap());
            // foreach (KeyValuePair<string, AttackDetails> detailPair in attackDictValues)
            // {

            switch (ptype)
            {
                case PokemonTYPE.Darkness:
                    image = PPokemon.Properties.Resources.ecard_darknessjpg;
                    break;
                case PokemonTYPE.Fighting:
                    image = PPokemon.Properties.Resources.ecard_fighting;
                    break;
                case PokemonTYPE.Fire:
                    image = PPokemon.Properties.Resources.ecard_fire;
                    break;
                case PokemonTYPE.Grass:
                    image = PPokemon.Properties.Resources.ecard_grass;
                    break;
                case PokemonTYPE.Lightning:
                    image = PPokemon.Properties.Resources.ecard_lightning;
                    break;
                case PokemonTYPE.Metal:
                    image = PPokemon.Properties.Resources.ecard_metal;
                    break;
                case PokemonTYPE.Psychic:
                    image = PPokemon.Properties.Resources.ecard_psychic;
                    break;
                case PokemonTYPE.Water:
                    image = PPokemon.Properties.Resources.ecard_water;
                    break;
                default:
                    break;
            }
            // }
            return image;
        }

        /// <summary>
        /// Null, Basic, Stage1, Stage2, Energy, Trainer, Supporter, Stadium
        /// </summary>
        /// <param name="cardtypename"></param>
        /// <returns></returns>
        public static Cardtype ConvertStringToCardType(string cardtypename)
        {
            cardtypename = cardtypename.ToLower().Trim();
            Cardtype cardT = Cardtype.Null;
            switch (cardtypename)
            {
                case "null":
                    cardT = Cardtype.Null;
                    break;
                case "basic":
                    cardT = Cardtype.Basic;
                    break;
                case "stage1":
                    cardT = Cardtype.Stage1;
                    break;
                case "stage2":
                    cardT = Cardtype.Stage2;
                    break;
                case "levelup":
                    cardT = Cardtype.LevelUp;
                    break;
                case "energy":
                    cardT = Cardtype.Energy;
                    break;
                case "trainer":
                    cardT = Cardtype.Trainer;
                    break;
                case "supporter":
                    cardT = Cardtype.Supporter;
                    break;
                case "stadium":
                    cardT = Cardtype.Stadium;
                    break;
                default:
                    break;
            }
            return cardT;
        }

        /// <summary>
        /// Colorless, Null, Fighting, Fire, Grass, Lightning, Psychic, Water
        /// </summary>
        /// <param name="cardtypename"></param>
        /// <returns></returns>
        public static PokemonTYPE ConvertStringToPokemonTYPE(string typename)
        {
            typename = typename.ToLower().Trim();
            PokemonTYPE cardT = PokemonTYPE.Null;
            switch (typename)
            {
                case "colorless":
                    cardT = PokemonTYPE.Colorless;
                    break;
                case "darkness":
                    cardT = PokemonTYPE.Darkness;
                    break;
                case "fighting":
                    cardT = PokemonTYPE.Fighting;
                    break;
                case "fire":
                    cardT = PokemonTYPE.Fire;
                    break;
                case "grass":
                    cardT = PokemonTYPE.Grass;
                    break;
                case "lightning":
                    cardT = PokemonTYPE.Lightning;
                    break;
                case "metal":
                    cardT = PokemonTYPE.Metal;
                    break;
                case "none":
                    cardT = PokemonTYPE.Null;
                    break;
                case "null":
                    cardT = PokemonTYPE.Null;
                    break;
                case "psychic":
                    cardT = PokemonTYPE.Psychic;
                    break;
                case "water":
                    cardT = PokemonTYPE.Water;
                    break;
                default:
                    break;
            }

            return cardT;
        }

        /// <summary>
        /// CardEventNames { FlipACoin, AddDamageToAttack, AddSpecialCToAttack, ChooseOpponPokemon, 
        ///       MultiplyDamageByNumberOfPokemonInPlay, DrawACard, DiscardAnEnergy, SearchDeckForBasic, ShuffleDeck, None };
        /// </summary>
        /// <param name="cardeventname"></param>
        /// <returns></returns>
        public static CardEventNames ConvertStringToCardEventNames(string cardeventname)
        {
            cardeventname = cardeventname.ToLower().Trim();
            CardEventNames cename = CardEventNames.None;
            switch (cardeventname)
            {
                case "flipacoin":
                    cename = CardEventNames.FlipACoin;
                    break;
                case "adddamagetoattack":
                    cename = CardEventNames.AddDamageToAttack;
                    break;
                case "adddamagetobenched":
                    cename = CardEventNames.AddDamageToBenched;
                    break;
                case "adddamagetoeachbenched":
                    cename = CardEventNames.AddDamageToEachBenched;
                    break;
                case "adddamagetoself":
                    cename = CardEventNames.AddDamageToSelf;
                    break;
                case "addspecialctoattack":
                    cename = CardEventNames.AddSpecialCToAttack;
                    break;
                case "attachcardtoapokemon":
                    cename = CardEventNames.AttachCardToAPokemon;
                    break;
                case "chooseopponpokemon":
                    cename = CardEventNames.ChooseOpponPokemon;
                    break;
                case "choosecardfromhandplaceindeck":
                    cename = CardEventNames.ChooseCardFromHandPlaceInDeck;
                    break;
                case "choosepokemon":
                    cename = CardEventNames.ChoosePokemon;
                    break;
                case "drawacard":
                    cename = CardEventNames.DrawACard;
                    break;
                case "discardanenergy":
                    cename = CardEventNames.DiscardAnEnergy;
                    break;
                case "discardspecialenergycard":
                    cename = CardEventNames.DiscardSpecialEnergyCard;
                    break;
                case "ifdamaged":
                    cename = CardEventNames.IfDamaged;
                    break;
                case "ifdefendingdamaged":
                    cename = CardEventNames.IfDefendingDamaged;
                    break;
                case "ifdefendinghaspowers":
                    cename = CardEventNames.IfDefendingHasPowers;
                    break;
                case "ifmoreprizecards":
                    cename = CardEventNames.IfMorePrizeCards;
                    break;
                case "iflessenergythandefending":
                    cename = CardEventNames.IfLessEnergyThanDefending;
                    break;
                case "ifnextdamagelessthan":
                    cename = CardEventNames.IfNextDamageLessThan;
                    break;
                case "Ifnospecialc":
                    cename = CardEventNames.IfNoSpecialC;
                    break;
                case "ifknockedout":
                    cename = CardEventNames.IfKnockedOut;
                    break;
                case "ifflipheads":
                    cename = CardEventNames.IfFlipHeads;
                    break;
                case "iffliptails":
                    cename = CardEventNames.IfFlipTails;
                    break;
                case "choosecardsfromdrawn":
                    cename = CardEventNames.ChooseCardsFromDrawn;
                    break;
                case "minusamountmultipliedbyattacheddamage":
                    cename = CardEventNames.MinusAmountMultipliedByAttachedDamage;
                    break;
                case "multiplydamageBynumberofpokemoninplay":
                    cename = CardEventNames.MultiplyDamageByNumberOfPokemonInPlay;
                    break;
                case "multiplybydamageonself":
                    cename = CardEventNames.MultiplyByDamageOnSelf;
                    break;
                case "multiplybynonusedenergycount":
                    cename = CardEventNames.MultiplyByNonUsedEnergyCount;
                    break;
                case "opponentnextattack":
                    cename = CardEventNames.OpponentNextAttack;
                    break;
                case "opponentchoosecardsfromdrawn":
                    cename = CardEventNames.OpponentNextAttack;
                    break;
                case "removedamagecounter":
                    cename = CardEventNames.RemoveDamageCounter;
                    break;
                case "resistenceoverride":
                    cename = CardEventNames.ResistenceOverride;
                    break;
                case "searchdeckforbasic":
                    cename = CardEventNames.SearchDeckForBasic;
                    break;
                case "searchdeckforbasicorevolution":
                    cename = CardEventNames.SearchDeckForBasicOrEvolution;
                    break;
                case "searchdeckforbasicplacetobench":
                    cename = CardEventNames.SearchDeckForBasicPlaceToBench;
                    break;
                case "searchdeckforenergy":
                    cename = CardEventNames.SearchDeckForEnergy;
                    break;
                case "searchdeckforpokemon":
                    cename = CardEventNames.SearchDeckForPokemon;
                    break;
                case "selectallpokemontype":
                    cename = CardEventNames.SelectAllPokemonType;
                    break;
                case "shuffledeck":
                    cename = CardEventNames.ShuffleDeck;
                    break;
                case "switchdefendingwithbenched":
                    cename = CardEventNames.SwitchDefendingWithBenched;
                    break;
                case "switchwithbenched":
                    cename = CardEventNames.SwitchWithBenched;
                    break;
                case "None":
                    cename = CardEventNames.None;
                    break;
                default:
                    break;
            }
            return cename;
        }

        /// <summary>
        /// Get the best attack's total energy required
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static int GetBestAttackEnergyTotalReq(Object card)
        {
            int totalE = 0;
            
            Dictionary<string, AttackProperties>.Enumerator AllAttackProps = ((Card)card).Properties.Attacks.First.GetEnumerator();
            while (AllAttackProps.MoveNext())//{[Ram, PPokemon.Cards.AttackProperties]}
            {
                totalE = 0;
                IEnumerator<AttackDetails> detailsEnum = AllAttackProps.Current.Value.AttackDict.Values.GetEnumerator();
                while(detailsEnum.MoveNext())
                {
                    //Keeps the last Attack's value as the highest
                        //Make sure not to count the first attacks with no energy required
                    if (detailsEnum.Current.EnergyRequired.First != PokemonTYPE.Null)
                    {
                        totalE = totalE + detailsEnum.Current.EnergyRequired.Second;
                    }
                }

            }
            return totalE;
        }

        /// <summary>
        /// Get the best attack's total energy required
        /// </summary>
        /// <param name="cardholder">CardHolder</param>
        /// <returns>Returns the most logical best attack for a given CardHolder.CurrentCard. Knows enabled attacks. If attack has 0 damage, logic chooses the attack that uses the most energy.</returns>
        public static AttackProperties GetBestEnabledAttack(CardHolder cardholder)
        {
            AttackProperties bestAttack = new AttackProperties();
            List<AttackProperties> enabledAttacks = new List<AttackProperties>();
            enabledAttacks = cardholder.CurrentCard.GetEnabledAttackList();
            if (enabledAttacks.Count > 0)
            {
                foreach (AttackProperties attack in enabledAttacks)
                {
                    //Choose the highest damage count attack
                    if (bestAttack.Damage < attack.Damage)
                        bestAttack = attack;
                }
                if (bestAttack.Damage == 0)
                    bestAttack = enabledAttacks[enabledAttacks.Count - 1];
            }

            return bestAttack;
        }

        /// <summary>
        /// Get the second part of a list
        /// eg. "Put on Bench::Paras Basic (72/112)" returns "Paras Basic (72/112)"
        /// </summary>
        /// <param name="fullstring">"Put on Bench::Paras Basic (72/112)"</param>
        /// <returns>string eg: "Paras Basic (72/112)"</returns>
        public static string GetListBoxCardDescription(string fullstring)
        {
            string carddescr = "";
            if(fullstring.Contains("::"))
            {
                string[] seperators = new string[] { "::" };
                string[] allpieces = fullstring.Trim().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                carddescr = allpieces[1].Trim();
            }
            return carddescr;
        }

        public static AttackProperties GetAttackFromCardHolder(CardHolder cardholder, string attacktitle)
        {
            AttackProperties attack = new AttackProperties();
            List<AttackProperties> attackprops = new List<AttackProperties>();
            attackprops = cardholder.CurrentCard.IsCanAttack(cardholder.CurrentCard.State.AttachedCards);
            foreach (AttackProperties ap in attackprops)
            {
                if (ap.Title == attacktitle)
                {
                    attack = ap;
                }
            }
            

            return attack;
        }

        public static AttackProperties GetLastAttackFromCard(Card card)
        {
            AttackProperties attack = new AttackProperties();
            //Search thru THIS card's attacks
            if (card.Properties.Attacks.First != null)
            {
                Dictionary<string, AttackProperties>.ValueCollection AttackProps = card.Properties.Attacks.First.Values;
                //Examine the Attack
                foreach (AttackProperties attackprop in AttackProps)
                {
                    attack = attackprop;
                }
            }
            return attack;
        }

        /// <summary>
        /// Read the arguments for CardDictions/CardEventNames
        /// eg:...CardEventName="FlipACoin">HEADS = NEXT<...
        /// ...CardEventName="AddSpecialCToAttack">Paralyzed<...
        /// </summary>
        /// <param name="fullstring"></param>
        /// <returns>StringCollection</returns>
        public static StringCollection ReadCardEventNamesArgs(string fullstring)
        {
            StringCollection sc = new StringCollection();
            string[] seperators = new string[] { " " };
            string[] allpieces = fullstring.Trim().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            sc.AddRange(allpieces);
            return sc;
        }

        /// <summary>
        /// Converts a string into a SpecialConditions
        /// (case-insensitive)
        /// </summary>
        /// <param name="conditionName">string name of SpecialConditions</param>
        /// <returns>SpecialConditions</returns>
        public static SpecialConditions ConvertStringToSpecialConditions(string conditionName)
        {
            SpecialConditions condition = SpecialConditions.None;
            conditionName = conditionName.ToLower().Trim();
            switch (conditionName)
            {
                case "none":
                    condition = SpecialConditions.None;
                    break;
                case "asleep":
                    condition = SpecialConditions.Asleep;
                    break;
                case "burned":
                    condition = SpecialConditions.Burned;
                    break;
                case "confused":
                    condition = SpecialConditions.Confused;
                    break;
                case "paralyzed":
                    condition = SpecialConditions.Paralyzed;
                    break;
                case "poisoned":
                    condition = SpecialConditions.Poisoned;
                    break;
                default:
                    break;
            }
            return condition;
        }

        /// <summary>
        /// Converts a string into a Coin object
        /// (case-insensitive)
        /// </summary>
        /// <param name="facename"></param>
        /// <returns></returns>
        public static GamePlay.Coin ConvertStringToCoin(string facename)
        {
            GamePlay.Coin coin = GamePlay.Coin.HEADS;
            facename = facename.ToLower().Trim();
            switch (facename)
            {
                case "heads":
                    coin = GamePlay.Coin.HEADS;
                    break;
                case "tails":
                    coin = GamePlay.Coin.TAILS;
                    break;
                default:
                    break;
            }
            return coin;
        }

        /// <summary>
        /// Returns the lowest HP card from provided list
        /// </summary>
        /// <param name="cardlist">List</param>
        /// <returns>Card</returns>
        public static Card GetLowestHPCard(List<Card> cardlist)
        {
            Card lowestHPCard = new Card(Cardtype.Null);
            if (cardlist.Count > 0)
            {
                foreach (Card cardchoosen in cardlist)
                {
                    //Make sure it is a card with 
                    if (cardchoosen.Properties.CardType == Cardtype.Basic || cardchoosen.Properties.CardType == Cardtype.Stage1 ||
                        cardchoosen.Properties.CardType == Cardtype.Stage2 || cardchoosen.Properties.CardType == Cardtype.LevelUp)
                    {
                        if (cardchoosen.Properties.HP < lowestHPCard.Properties.HP || lowestHPCard.Properties.CardType == Cardtype.Null)
                            lowestHPCard = cardchoosen;
                    }
                }
            }

            return lowestHPCard;
        }

        /// <summary>
        /// Returns the Highest HP card from provided list
        /// </summary>
        /// <param name="cardlist">List</param>
        /// <returns>Card</returns>
        public static Card GetHighestHPCard(List<Card> cardlist)
        {
            Card highestHPCard = new Card(Cardtype.Null);
            if (cardlist.Count > 0)
            {
                foreach (Card cardchoosen in cardlist)
                {
                    //Make sure it is a card with 
                    if (cardchoosen.Properties.CardType == Cardtype.Basic || cardchoosen.Properties.CardType == Cardtype.Stage1 ||
                        cardchoosen.Properties.CardType == Cardtype.Stage2 || cardchoosen.Properties.CardType == Cardtype.LevelUp)
                    {
                        if (cardchoosen.Properties.HP > highestHPCard.Properties.HP || highestHPCard.Properties.CardType == Cardtype.Null)
                            highestHPCard = cardchoosen;
                    }
                }
            }

            return highestHPCard;
        }

        public static CardDirections GetAttackDirections(AttackProperties attackprops)
        {
            CardDirections alldirections = new CardDirections();
            IEnumerator<AttackDetails> detailsEnum = attackprops.AttackDict.Values.GetEnumerator();
            detailsEnum.MoveNext();
            alldirections = detailsEnum.Current.Directions;
            return alldirections;
        }
    }

    /// <summary>
    /// Object that reads the xml then holds the index and associated Pokemon objects
    /// </summary>
    public class Pokedex
    {
        #region Variables
        private string _pokedexxml;
        public XmlDocument xmlDocPokedex = new XmlDocument();
        public Dictionary<int, Pokemon> IndexDictionary = new Dictionary<int, Pokemon>();
        #endregion Variables
        
        public Pokedex(string pokedexxml)
        {
            _pokedexxml = pokedexxml;
            xmlDocPokedex.Load(_pokedexxml);

            //Create a XmlNode from Pokedex.xmlDocPokedex
            Initialize(xmlDocPokedex.DocumentElement, xmlDocPokedex.DocumentElement.FirstChild);
        }

        /// <summary>
        /// Create the IndexDictionary, which holds the Pokemon objects
        /// </summary>
        private void Initialize(XmlElement inXmlElement, XmlNode inXmlNode)
        {
            // Loop through the XML nodes until the leaf is reached.
            if (inXmlElement.HasChildNodes)
            {
                if (inXmlNode.Name == "pokemon")
                {
                    IndexDictionary.Add(IndexDictionary.Count + 1, new Pokemon(inXmlNode));
                }
                if (inXmlNode.NextSibling != null)
                    Initialize(inXmlElement, inXmlNode.NextSibling);
            }
        }

        /// <summary>
        /// Get a Pokemon object out of the Pokedex by index number
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Pokemon GetPokemon(int index)
        {
            Pokemon pokemon = new Pokemon();
            IndexDictionary.TryGetValue(index, out pokemon);
            return pokemon;
        }

        /// <summary>
        /// Get Pokemon object from the Pokedex
        /// </summary>
        /// <param name="name"></param>
        /// <returns>first found Pokemon from Pokedex</returns>
        public Pokemon GetPokemonByName(string name)
        {
            name = name.Trim();
            Pokemon pokemon = new Pokemon();
            IEnumerator<Pokemon> PokedoxEnum = IndexDictionary.Values.GetEnumerator();
            while (PokedoxEnum.MoveNext())
            {
                if (PokedoxEnum.Current.Name.ToLower() == name.ToLower())
                    pokemon = PokedoxEnum.Current;
            }
            return pokemon;
        }
    }

    /// <summary>
    /// Holds the Directions for a card
    /// </summary>
    public class CardDirections
    {
        /// <summary>
        /// List of CardEventNames, string>
        /// </summary>
        public List<Pair<CardEventNames, string>> Directions = new List<Pair<CardEventNames, string>>();

        public CardDirections()
        {

        }
    }

    public class AttackDetails
    {
        public Wintellect.PowerCollections.Pair<PokemonTYPE, int> EnergyRequired = new Pair<PokemonTYPE, int>();
        public CardDirections Directions = new CardDirections();
    }

    /// <summary>
    /// Holds all the details for each attack on a card
    /// </summary>
    public class AttackProperties
    {
        public MultiDictionary<string, AttackDetails> AttackDict = new MultiDictionary<string, AttackDetails>(true);
        
        public int Damage = 0;
        public string Title = "";
        public string Body = "";

        public AttackProperties()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="ptype"></param>
        /// <param name="qty"></param>
        /// <param name="body"></param>
        /// <param name="damage"></param>
        /// <param name="directions"></param>
        public AttackProperties(string title, PokemonTYPE ptype, int qty, string body, int damage, CardDirections directions)
        {
            //Create AttackDetails to enumerate thru the Pokemon's two Attacks
            AttackDetails attackdetails = new AttackDetails();
            attackdetails.EnergyRequired = new Pair<PokemonTYPE, int>(ptype, qty);
            attackdetails.Directions = new CardDirections();
            attackdetails.Directions = directions;
            //The AttackDict is for enumeration
            this.AttackDict.Add(title, attackdetails);
            this.Add(title, body, damage);
        }

        /// <summary>
        /// Normally for the first of an attack energy typpw
        /// </summary>
        /// <param name="title"></param>
        /// <param name="ptype"></param>
        /// <param name="qty"></param>
        /// <param name="body"></param>
        /// <param name="damage"></param>
        public void Add(string title, string body, int damage)
        {
            try
            {
                this.Title = title;
                this.Body = body;
                this.Damage = damage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + " Error: " + ex.Message);
            }
            
        }

        /// <summary>
        /// Constructs a Card
        /// </summary>
        /// <param name="title">string</param>
        /// <param name="ptype">PokemonTYPE</param>
        /// <param name="qty">int</param>
        /// <param name="body">string</param>
        /// <param name="damage">int</param>
        /// <param name="directions">CardDirections</param>
        public void Add(string title, PokemonTYPE ptype, int qty, string body, int damage, CardDirections directions)
        {
            //Create AttackDetails to enumerate thru the Pokemon's two Attacks
            AttackDetails attackdetails = new AttackDetails();
            attackdetails.EnergyRequired = new Pair<PokemonTYPE, int>(ptype, qty);
            attackdetails.Directions = directions;
            //The AttackDict is for enumeration
            this.AttackDict.Add(title, attackdetails);
            this.Add(title, body, damage);
        }

        /// <summary>
        /// For a second energy attachment when 6 parameters have been already added
        /// </summary>
        /// <param name="title"></param>
        /// <param name="ptype"></param>
        /// <param name="qty"></param>
        /// <param name="body"></param>
        /// <param name="damage"></param>
        public void Add(string title, PokemonTYPE ptype, int qty)
        {
            try
            {
                this.Title = title;
                //Create AttackDetails to enumerate thru the Pokemon's two Attacks
                AttackDetails attackdetails = new AttackDetails();
                attackdetails.EnergyRequired = new Pair<PokemonTYPE, int>(ptype, qty);
                //The AttackDict is for enumeration
                this.AttackDict.Add(title, attackdetails);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + " Error: " + ex.Message);
            }

        }

        /// <summary>
        /// Get the sum of all of this Attack's Energy needed
        /// Does this by using AttackDict's  the Second int values in AttackDetails.EnergyRequired
        /// </summary>
        /// <returns></returns>
        public int GetEnergyTotalCount()
        {
            int total_energycount = 0;
            IEnumerator<AttackDetails> attackdetails = AttackDict.Values.GetEnumerator();
            while (attackdetails.MoveNext())
            {
                total_energycount = total_energycount + attackdetails.Current.EnergyRequired.Second;
            }
            return total_energycount;
        }
    }

    public class PokePowerDetails
    {
        /// <summary>
        /// Get or Set the Title of the PokePower/Body
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        string _Title = "";

        /// <summary>
        /// Get or Set the Body of the PokePower/Body
        /// </summary>
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        string _Body = "";

        /// <summary>
        /// Get or Set all of the Power's events (CardEventNames, arg)
        /// </summary>
        public List<Wintellect.PowerCollections.Pair<CardEventNames, string>> Events = new List<Wintellect.PowerCollections.Pair<CardEventNames, string>>();
    }

    public class CardProperties
    {
        /// <summary>
        /// Pokemon type. Grass, Fire, Water, etc.
        /// </summary>
        public PokemonTYPE PokemonType
        {
            get { return _PokemonType; }
            set { _PokemonType = value; }
        }
        PokemonTYPE _PokemonType = PokemonTYPE.Null;

        /// <summary>
        /// Stores the Pokemon object (Name, image, index)
        /// </summary>
        public Pokemon Pokemon
        {
            get { return _Pokemon; }
            set { _Pokemon = value; }
        }
        Pokemon _Pokemon = null;

        /// <summary>
        /// HP (Hit Points)
        /// </summary>
        public int HP
        {
            get { return _HP; }
            set { _HP = value; }
        }
        int _HP = -1;

        /// <summary>
        /// CardType: Basic, Stage1, Stage2, Energy, Trainer, Supporter, Stadium
        /// </summary>
        public Cardtype CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }
        Cardtype _CardType = Cardtype.Null;

        /// <summary>
        /// Get or Set Poke-Powers or Poke-Bodies by Name
        /// <"Power Name", List<Pair<"CardEventName", "arg">> 
        /// </summary>
        public PokePowerDetails PokePower = new PokePowerDetails();
            
        /// <summary>
        /// Get Attacks by Name
        /// </summary>
        public Wintellect.PowerCollections.Pair<Dictionary<string, AttackProperties>, bool> Attacks = new Pair<Dictionary<string, AttackProperties>, bool>();

        /// <summary>
        /// Weakness of card. Pair: PokemonTYPE, int(cost)
        /// </summary>
        public Wintellect.PowerCollections.Pair<PokemonTYPE, int> Weakness = new Pair<PokemonTYPE, int>();

        /// <summary>
        /// Resistance of card. Pair: PokemonTYPE, int(cost)
        /// </summary>
        public Wintellect.PowerCollections.Pair<PokemonTYPE, int> Resistance = new Pair<PokemonTYPE, int>();

        /// <summary>
        /// Retreat cost of card. Pair: PokemonTYPE, int(cost)
        /// </summary>
        public Wintellect.PowerCollections.Pair<PokemonTYPE, int> RetreatCost = new Pair<PokemonTYPE, int>();
    }

    public class CardState
    {
        /// <summary>
        /// List of all the Attached Cards
        /// NOTE: If you need to look just for Energy cards, look for Card.Properties.CardType
        /// or use GetAttachedEnergyCardCount()
        /// </summary>
        public List<Card> AttachedCards = new List<Card>();

        /// <summary>
        /// Is this Card the Active Pokemon?
        /// </summary>
        public bool ActivePokemon
        {
            get { return _ActivePokemon; }
            set { _ActivePokemon = value; }
        }
        bool _ActivePokemon = false;

        /// <summary>
        /// Get or Set if a Card has Evolved during this turn
        /// </summary>
        public bool EvolvedPokemon
        {
            get { return _EvolvedPokemon; }
            set { _EvolvedPokemon = value; }
        }
        bool _EvolvedPokemon = false;

        /// <summary>
        /// Current Condition or Special Condition of the Pokemon Card 
        /// </summary>
        public SpecialConditions CurrentCondition
        {
            get { return _CurrentCondition; }
            set { _CurrentCondition = value; }
        }
        SpecialConditions _CurrentCondition = SpecialConditions.None;

        /// <summary>
        /// Get the Count of EnergyCards in AttachedCards
        /// </summary>
        /// <returns></returns>
        public int GetAttachedEnergyCardCount()
        {
            int count = 0;
            foreach (Card card in AttachedCards)
            {
                if (card.Properties.CardType == Cardtype.Energy)
                {
                    count = count + 1;
                }
            }
            return count;
        }

        /// <summary>
        /// Current Damage Counter
        /// </summary>
        public int CurrentDamage
        {
            get { return _CurrentDamage; }
            set { _CurrentDamage = value; }
        }
        int _CurrentDamage = 0;

        /// <summary>
        /// Create a Clone instance of this CardState
        /// </summary>
        /// <returns>CardState</returns>
        public CardState Clone()
        {
            CardState newCardState = new CardState();
            newCardState.ActivePokemon = this.ActivePokemon;
            newCardState.AttachedCards.AddRange(this.AttachedCards.GetRange(0, this.AttachedCards.Count));
            newCardState.CurrentCondition = this.CurrentCondition;
            newCardState.CurrentDamage = this.CurrentDamage;
            newCardState.EvolvedPokemon = this.EvolvedPokemon;

            return newCardState;
        }

        /// <summary>
        /// Evaluates this current CardState and gets one CardNeedActions
        /// </summary>
        /// <returns></returns>
        public CardNeedActions GetCurrentNeed()
        {
            CardNeedActions currentneed = CardNeedActions.Null;

            if (this.CurrentDamage >= 30)
                currentneed = CardNeedActions.RemoveDamage;
            else if (this.CurrentCondition != SpecialConditions.None)
                currentneed = CardNeedActions.RemoveSpecialC;
            else if (this.EvolvedPokemon == false)
                currentneed = CardNeedActions.Pokemon;
            else if (this.GetAttachedEnergyCardCount() < 1)
                currentneed = CardNeedActions.Energy;
            else
                currentneed = CardNeedActions.AttackOffense;//or Defense

            return currentneed;
        }
    }

    public class Card
    {
        #region Drawing Properties
        public const byte Width = 142;
        public const byte Heigth = 192;
        private const byte InnerWidth = Width - 2;
        private const byte InnerHeigth = Heigth - 2;
        private const byte LeftRight = 12;
        private const byte TopDown = 11;

        private Point _topLeft = new Point(20, 20);
        public Point TopLeft
        {
            get
            {
                return _topLeft;
            }
            set
            {
                _topLeft = value;
            }
        }
        public Rectangle ClientRect
        {
            get
            {
                return new Rectangle(_topLeft, new Size(Width, Heigth));
            }
        }
        #endregion Drawing Properties
        
        #region Variables
        /// <summary>
        /// Stores important info to be shared amoung classes (Panels etc.)
        /// </summary>
        public CardProperties Properties
        {
            get { return _Properties; }
            set { _Properties = value; }
        }
        CardProperties _Properties = new CardProperties();

        /// <summary>
        /// Visual Style Panel
        /// Gets set(Basic, Energy, etc) on Card() construction
        /// Must be: CardPanel, 
        /// </summary>
        public UserControl Panel
        {
            get { return _Panel; }
            set { _Panel = value; }
        }
        UserControl _Panel = new CardPanel();

        /// <summary>
        /// The current CardState
        /// </summary>
        public CardState State
        {
            get { return _State; }
            set { _State = value; }
        }
        CardState _State = new CardState();

        /// <summary>
        /// An optional description for the card.
        /// eg. "55/109"
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        string _Description = "";
        #endregion Variables
        
        #region Constructors
        /// <summary>
        /// Basic Constructor
        /// </summary>
        public Card(Cardtype ctype)
        {
            this.SetCardType(ctype);
        }

        /// <summary>
        /// Create a Card object from an XmlNode
        /// </summary>
        /// <param name="xmlnode"></param>
        public Card(XmlNode xmlnode, Pokedex pokedex)
        {
            try
            {
                XPathNavigator navigator = xmlnode.CreateNavigator();
                navigator.MoveToNext();
                this._Description = navigator.Select("/Card").Current.GetAttribute("description", "");
                XPathNodeIterator CardPropNodes = navigator.Select("/Card/CardProperties");
                
                while (CardPropNodes.MoveNext())
                {
                    this._Properties.HP = Convert.ToInt16(CardPropNodes.Current.GetAttribute("HP", ""));
                    this._Properties.CardType = Ops.ConvertStringToCardType(CardPropNodes.Current.GetAttribute("CardType", ""));
                    this.SetCardType(this._Properties.CardType);

                    #region /Card/CardProperties
                    XPathNodeIterator XPathPokemon = navigator.Select("/Card/CardProperties/Pokemon");
                    XPathPokemon.MoveNext();
                    this._Properties.Pokemon = pokedex.GetPokemonByName(XPathPokemon.Current.Value.Trim());
                    this._Properties.PokemonType = Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.GetAttribute("PokemonType", ""));
                    XPathPokemon = navigator.Select("/Card/CardProperties/Resistance");
                    XPathPokemon.MoveNext();//
                    this._Properties.Resistance = new Pair<PokemonTYPE, int>(Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.GetAttribute("PokemonType", "")), XPathPokemon.Current.ValueAsInt);

                    XPathPokemon = navigator.Select("/Card/CardProperties/RetreatCost");
                    XPathPokemon.MoveNext();//
                    this._Properties.RetreatCost = new Pair<PokemonTYPE, int>(Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.GetAttribute("PokemonType", "")), XPathPokemon.Current.ValueAsInt);

                    XPathPokemon = navigator.Select("/Card/CardProperties/Weakness");
                    XPathPokemon.MoveNext();//
                    this._Properties.Weakness = new Pair<PokemonTYPE, int>(Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.GetAttribute("PokemonType", "")), XPathPokemon.Current.ValueAsInt);
                    #endregion /Card/CardProperties

                    #region Poke-Power and Poke-Bodies
                    this.Properties.PokePower = new PokePowerDetails();
                    XPathPokemon = navigator.Select("/Card/Power");
                    XPathPokemon.MoveNext();
                    //Get the Name of the PokePower/Body
                    this.Properties.PokePower.Title = XPathPokemon.Current.GetAttribute("title", "");
                    this.Properties.PokePower.Body = XPathPokemon.Current.GetAttribute("body", "");

                    if (this.Properties.PokePower.Title != "")
                    {
                        XPathPokemon.Current.MoveToFirstChild();
                        this.Properties.PokePower.Events.Add(new Pair<CardEventNames, string>(Ops.ConvertStringToCardEventNames(XPathPokemon.Current.GetAttribute("CardEventName", "")), XPathPokemon.Current.Value));//CardDirections
                        while (XPathPokemon.Current.MoveToNext() && XPathPokemon.Current.Name == "CardDirections")
                        {
                            this.Properties.PokePower.Events.Add(new Pair<CardEventNames, string>(Ops.ConvertStringToCardEventNames(XPathPokemon.Current.GetAttribute("CardEventName", "")), XPathPokemon.Current.Value));//CardDirections
                        }
                    }
                    #endregion

                    #region /Card/Attacks
                    XPathPokemon = navigator.Select("/Card/Attacks/Attack");
                    CardDirections directions = new CardDirections();

                    this.Properties.Attacks = new Pair<Dictionary<string, AttackProperties>, bool>(new Dictionary<string, AttackProperties>(), false);
                    //Iterate thru each Attack
                    while (XPathPokemon.MoveNext())
                    {
                        AttackProperties attackprops = new AttackProperties();
                        string title = XPathPokemon.Current.GetAttribute("title", "");
                        string body = XPathPokemon.Current.GetAttribute("body", "");

                        //Get damage points
                        XPathPokemon.Current.MoveToFirstChild();

                        directions = new CardDirections();
                        while (XPathPokemon.Current.Name == "CardDirections")
                        {
                            directions.Directions.Add(new Pair<CardEventNames, string>(Ops.ConvertStringToCardEventNames(XPathPokemon.Current.GetAttribute("CardEventName", "")), XPathPokemon.Current.Value));//CardDirections
                            XPathPokemon.Current.MoveToNext();
                        }

                        string strDamage = XPathPokemon.Current.GetAttribute("damage", "");

                        //Iterate thru all the AttackProperties/PokemonType
                        string sQty = "";
                        while (XPathPokemon.Current.MoveToFirstChild() ||
                            ((XmlNode)XPathPokemon.Current.UnderlyingObject).NextSibling != null)
                        {
                            if (XPathPokemon.Current.Name == "PokemonType")
                            {
                                sQty = XPathPokemon.Current.GetAttribute("qty", "");//Get the PokemonType for all iterations
                                attackprops.Add(title,
                                    Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.Value.Trim()),
                                    Convert.ToInt32(sQty),
                                    body,
                                    Convert.ToInt32(strDamage),
                                    directions);
                            }
                            if (((XmlNode)XPathPokemon.Current.UnderlyingObject).NextSibling != null &&
                                ((XmlNode)XPathPokemon.Current.UnderlyingObject).NextSibling.Name == "PokemonType")
                            {
                                XPathPokemon.Current.MoveToNext();
                                sQty = XPathPokemon.Current.GetAttribute("qty", "");
                                attackprops.Add(title, Ops.ConvertStringToPokemonTYPE(XPathPokemon.Current.Value.Trim()), Convert.ToInt32(sQty));
                            }
                        }
                        //Now store this completed Attack to Attacks Dictionary
                        this.Properties.Attacks.First.Add(title, attackprops);
                    }
                    #endregion /Card/Attacks

                    //Do this last...
                    this._Panel.Tag = (CardProperties)this._Properties;  //for Panel pBoxType
                }
            }
            catch (Exception Ex)
            {
                //XmlException xmlEx = new XmlException(Ex.Message, Ex.InnerException);
                MessageBox.Show("Card creation error: " + Ex.Message + "\r\n" + Ex.StackTrace);
            }

        }

        public Card(Cardtype _CardType, Pokemon _Pokemon, PokemonTYPE _PokemonType, int _HP, 
            Pair<Dictionary<string, AttackProperties>, bool> attackdict, Pair<PokemonTYPE, int> weaknesspair,
            Pair<PokemonTYPE, int> resistance, Pair<PokemonTYPE, int> retreatcost)
        {
            this.SetCardType(_CardType);                //Cardtype
            this._Properties.Pokemon = _Pokemon;        //Pokemon object
            this._Properties.PokemonType = _PokemonType;//PokemonTYPE
            this._Properties.HP = _HP;                  //Hit Points
            this._Properties.Attacks = attackdict;      //Holds AttackProperties, and how many
            this._Properties.Weakness = weaknesspair;   //Weakness/cost
            this._Properties.Resistance = resistance;   //Resistance/Subtractcount
            this._Properties.RetreatCost = retreatcost; //Retreat energycost/count

            //Do this last...
            this._Panel.Tag = (CardProperties)this._Properties;  //PokemonTYPE for Panel pBoxType
        }
        #endregion Constructors

        /// <summary>
        /// Assign the internal Card type
        /// </summary>
        /// <param name="ctype"></param>
        private void SetCardType(Cardtype ctype)
        {
            this._Properties.CardType = ctype;
            //Set the correct visual Panel...
            switch (ctype)
            {
                case Cardtype.Basic:
                    _Panel = new CardPanel();
                    break;
                case Cardtype.Stage1:
                    _Panel = new CardPanel();
                    break;
                case Cardtype.Stage2:
                    _Panel = new CardPanel();
                    break;
                case Cardtype.LevelUp:
                    _Panel = new CardPanel();
                    break;
                case Cardtype.Energy:
                    _Panel = new CardEnergyPanel();
                    break;
                case Cardtype.Trainer:
                    _Panel = new CardTrainerPanel();
                    break;
                case Cardtype.Supporter:
                    _Panel = new CardTrainerPanel();
                    break;
                case Cardtype.Stadium:
                    _Panel = new CardTrainerPanel();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Can this Pokemon Attack? 
        /// </summary>
        /// <param name="attackname"></param>
        /// <returns>A List of what Attacks are enabled: if(ReadyAttacks.Count > 0)</returns>
        public List<AttackProperties> IsCanAttack (List<Card> attachedcards)
        {
            int curEnergyCardCount = this._State.GetAttachedEnergyCardCount();
            Dictionary<PokemonTYPE, int> curEnergy = new Dictionary<PokemonTYPE, int>();
            List<AttackProperties> ReadyAttacks = new List<AttackProperties>();
            int SatisfiedCount = 0;
            int TotalAttachedEnergy = 0;

            //Search thru the current attached cards to record curEnergy
            foreach (Card card in attachedcards)
            {
                //If the card is an energy card
                if (card.Properties.CardType == Cardtype.Energy)
                {
                    if (!curEnergy.ContainsKey(card.Properties.PokemonType))
                    {
                        //Record this Energy type in the MultiDictionary
                        curEnergy.Add(card.Properties.PokemonType, 1);//Energy cards need this set to its Type
                    }
                    else
                    {
                        //Add another of same type to the int count
                        //Get the current value
                        int curCount;
                        curEnergy.TryGetValue(card.Properties.PokemonType, out curCount);

                        curEnergy.Remove(card.Properties.PokemonType);
                        curEnergy.Add(card.Properties.PokemonType, curCount + 1);
                    }
                    TotalAttachedEnergy = TotalAttachedEnergy + 1;
                }
            }

            //Search thru THIS card's attacks
            if (this._Properties.Attacks.First != null)
            {
                Dictionary<string, AttackProperties>.ValueCollection AttackProps = this._Properties.Attacks.First.Values;
                //Examine the Attack
                foreach (AttackProperties attack in AttackProps)
                {
                    //Does the attack have the required Energy Cards?
                    //If the attack is colorless or has the same PokemonTYPE
                    IEnumerator<AttackDetails> AttackDetails = attack.AttackDict.Values.GetEnumerator();//Get the attack's AttackDetails
                    int ColorlessCountReq = 0;
                    int NonColorlessreq = 0;
                    int TotEReqCount = 0;//Counts all the attack's energy required
                    //Examine each AttackDetails EnergyRequired Type and Count
                    while (AttackDetails.MoveNext())
                    {
                        //Is this attack's PokemonType in the CurEnergy dict? 
                        if (curEnergy.ContainsKey(AttackDetails.Current.EnergyRequired.First) || (AttackDetails.Current.EnergyRequired.First == PokemonTYPE.Colorless))
                        {
                            //Is there enough Energy or Colorless cards to use this attack?
                            //If an attack uses colorless energy, then verify EnergyCard count
                            //then add it to satisfied count
                            int ECountFound;
                            //Search for the needed energy card in the AttachedEnergyCards
                            curEnergy.TryGetValue(AttackDetails.Current.EnergyRequired.First, out ECountFound);

                            //If the countfound is greater than or equal to the qty of that type, then that satisfies
                            //note:Colorless will not be found since all energycards count as colorless
                            if (ECountFound >= AttackDetails.Current.EnergyRequired.Second)
                            {
                                SatisfiedCount = SatisfiedCount + ECountFound;
                                NonColorlessreq = NonColorlessreq + AttackDetails.Current.EnergyRequired.Second;
                            }
                            //If the energyneeded is a colorless & the countfound is greater than or equal to the qty of that type
                            if ((AttackDetails.Current.EnergyRequired.First == PokemonTYPE.Colorless) &&
                                ((TotalAttachedEnergy - NonColorlessreq) >= AttackDetails.Current.EnergyRequired.Second))
                            {
                                ColorlessCountReq = (TotalAttachedEnergy - NonColorlessreq);
                                //        SatisfiedCount = (TotalAttachedEnergy - SatisfiedCount) + SatisfiedCount;//Subtract the noncolorless because this counts them
                            }
                        }
                        else if (AttackDetails.Current.EnergyRequired.First == PokemonTYPE.Null)
                        {
                            //An attack may require 0 energy cards
                        }
                        else
                        {
                            return ReadyAttacks;//If the required EnergyCard is not attached, then the attack can not be used.
                        }
                        //Remember the qty counts for each required energy
                        TotEReqCount = TotEReqCount + AttackDetails.Current.EnergyRequired.Second;
                    }

                    //If attached energy qty >= number of Ecards req AND attached energy qty >= Colorlessreq AND SatisfiedCount >= NonColorlessreq
                    if (((TotalAttachedEnergy >= TotEReqCount) && (TotalAttachedEnergy >= ColorlessCountReq)
                        && (SatisfiedCount >= NonColorlessreq)) || (AttackDetails.Current.EnergyRequired.First == PokemonTYPE.Null))
                    {
                        //if the attack is already in the , don't add it
                        if (!ReadyAttacks.Contains(attack))
                            ReadyAttacks.Add(attack);
                        SatisfiedCount = 0;
                    }
                }
            }
            else
            { MessageBox.Show("DEBUG: PAUSE AND FIND OUT WHY THIS this._Properties.Attacks.First is null"); }

            return ReadyAttacks;
        }

        /// <summary>
        /// Convert a Card object to an XML object
        /// </summary>
        public void ToXmlNode()
        {
            XmlNode xmlnode;
            //Create a temp xml file
            string filename = Common.xmlFolder + "\\_card" + this.Properties.Pokemon.Name.ToString() + ".xml";
            XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            try
            {
                //xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Card");
                xmlWriter.WriteAttributeString("description", this._Description);
                xmlWriter.WriteStartElement("CardProperties");
                xmlWriter.WriteAttributeString("HP", this._Properties.HP.ToString());
                xmlWriter.WriteAttributeString("CardType", this._Properties.CardType.ToString());

                xmlWriter.WriteStartElement("Pokemon");
                xmlWriter.WriteAttributeString("PokemonType", this.Properties.PokemonType.ToString());
                xmlWriter.WriteValue(this.Properties.Pokemon.Name);
                xmlWriter.WriteEndElement();//Pokemon

                xmlWriter.WriteStartElement("Resistance");
                xmlWriter.WriteAttributeString("PokemonType", this.Properties.Resistance.First.ToString());
                xmlWriter.WriteValue(this.Properties.Resistance.Second);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RetreatCost");
                xmlWriter.WriteAttributeString("PokemonType", this.Properties.RetreatCost.First.ToString());
                xmlWriter.WriteValue(this.Properties.RetreatCost.Second);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Weakness");
                xmlWriter.WriteAttributeString("PokemonType", this.Properties.Weakness.First.ToString());
                xmlWriter.WriteValue(this.Properties.Weakness.Second);
                xmlWriter.WriteEndElement();//Weakness
                xmlWriter.WriteEndElement();//CardProperties

                xmlWriter.WriteStartElement("Attacks");
                //Search thru THIS card's attacks
                Dictionary<string, AttackProperties>.ValueCollection AttackProps = this._Properties.Attacks.First.Values;
                //Examine the Attack
                foreach (AttackProperties attack in AttackProps)
                {
                    xmlWriter.WriteStartElement("Attack");
                    xmlWriter.WriteAttributeString("title", attack.Title);
                    xmlWriter.WriteAttributeString("body", attack.Body);
                    //for each card directions
                    IEnumerator<AttackDetails> AttackDetails = attack.AttackDict.Values.GetEnumerator();//Get the attack's AttackDetails
                    while (AttackDetails.MoveNext())
                    {
                        foreach (Pair<CardEventNames, string> direction in AttackDetails.Current.Directions.Directions)
                        {
                            xmlWriter.WriteStartElement("CardDirections");
                            xmlWriter.WriteAttributeString("CardEventName", direction.First.ToString());
                            xmlWriter.WriteValue(direction.Second);
                            xmlWriter.WriteEndElement();//CardDirections
                        }
                    }

                    xmlWriter.WriteStartElement("AttackProperties");
                    xmlWriter.WriteAttributeString("damage", attack.Damage.ToString());
                    AttackDetails = attack.AttackDict.Values.GetEnumerator();//Get the attack's AttackDetails again to reset
                    while (AttackDetails.MoveNext())
                    {
                        xmlWriter.WriteStartElement("PokemonType");
                        xmlWriter.WriteAttributeString("qty", AttackDetails.Current.EnergyRequired.Second.ToString());
                        xmlWriter.WriteValue(AttackDetails.Current.EnergyRequired.First.ToString());
                        xmlWriter.WriteEndElement();//CardDirections
                    }
                    xmlWriter.WriteEndElement();//AttackProperties

                    xmlWriter.WriteEndElement();//Attack
                }
                xmlWriter.WriteEndElement();//Attacks
                xmlWriter.WriteEndElement();//Card
                xmlWriter.Close();

                //          MessageBox.Show(this.Properties.PokemonType.ToString());
                //Now read the temp file into an xml node
            }
            catch (Exception ex)
            {
                MessageBox.Show("ToXmlNode error: " + ex.Message);
            }

            //return xmlnode;
        }

        public void HookGame()
        {

        }

        public void Draw(Graphics grp)
        {
            using (Font fnt = new Font("Arial", 8, FontStyle.Bold))
            {
                Pen blackpen = new Pen(Color.Black, 3);
                //Draw card rectange
                grp.DrawRectangle(blackpen, this.ClientRect);

                //Draw Name
                //grp.DrawString(this.Pokemon.Name, fnt, blackpen.Brush, new Rectangle(_topLeft.X + 1, _topLeft.Y + 5, (this.ClientRect.Width - 30), 20));
                grp.DrawString(this.Properties.Pokemon.Name, fnt, blackpen.Brush, _topLeft.X + 1, _topLeft.Y + 5);

                //Draw HP
                //Rectangle HPRect = new Rectangle(_topLeft.X + 86, _topLeft.Y + 5, 15, 15);
                grp.DrawString(Convert.ToString(this.Properties.HP) + " HP", fnt, blackpen.Brush, _topLeft.X + 86, _topLeft.Y + 5);

                //Draw PokemonTypeImage
                Image PokemonTypeImage = PPokemon.Properties.Resources.grass;
                PokemonTypeImage.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                Rectangle PokeTypeRect = new Rectangle(_topLeft.X + 126, _topLeft.Y + 5, 15, 15);
                grp.DrawImage(PokemonTypeImage, PokeTypeRect);

                //Draw Character Image
                WebBrowser webb = new WebBrowser();
                webb.Focus();
                webb.AllowWebBrowserDrop = false;
                webb.IsWebBrowserContextMenuEnabled = false;
                webb.Location = new System.Drawing.Point(_topLeft.X + 1, _topLeft.Y + 15);
                webb.MinimumSize = new System.Drawing.Size(20, 20);
                webb.Name = "webb";
                webb.ScriptErrorsSuppressed = true;
                webb.ScrollBarsEnabled = false;
                webb.Size = new System.Drawing.Size(190, 96);

                //webb.SetBounds(_topLeft.X + 1, _topLeft.Y + 15, 190, 96);
                webb.Navigate(this.Properties.Pokemon.LocalSWF);
                webb.Show();
                webb.Update();

            }
        }

        /// <summary>
        /// Get all Attacks that can be used (attacks that are not enabled will not be returned)
        /// </summary>
        /// <returns>List<AttackProperties></returns>
        public List<AttackProperties> GetEnabledAttackList()
        {
            List<AttackProperties> EnabledAttacks = new List<AttackProperties>();
            EnabledAttacks = this.IsCanAttack(this.State.AttachedCards);
            if (EnabledAttacks.Count > 0)
            {
              //  foreach (AttackProperties attack in EnabledAttacks)
              //  {
              //      MessageBox.Show(this.Properties.Pokemon.Name + " can attack using " + attack.Title + " which causes " + attack.Damage.ToString() + " damage points!");
              //  }
            }

            return EnabledAttacks;
        }

        /// <summary>
        /// Returns the AttackProperties of the index requested 
        /// </summary>
        /// <param name="index">index in 'this.Properties.Attacks.First' Dictionary for desired AttackProperties</param>
        /// <returns>AttackProperties [attack.Title, attack.Damage.ToString(), etc.]</returns>
        public AttackProperties GetAttackProperties(int index)
        {
            AttackProperties attackprops = new AttackProperties();

            IEnumerator<AttackProperties> AttackEnum = this.Properties.Attacks.First.Values.GetEnumerator();
            int i = 0;
            while (AttackEnum.MoveNext())
            {//MessageBox.Show(this.Properties.Pokemon.Name + " can attack using " + attack.Title + " which causes " + attack.Damage.ToString() + " damage points!");
                if (i == index)
                {
                    attackprops = AttackEnum.Current;
                }
            }

            return attackprops;
        }

        /// <summary>
        /// Get the CardDirections for one specific attack (enabled or not)
        /// </summary>
        /// <param name="attacktitle">Name of the attack</param>
        /// <returns>List<CardDirections> [sequence of attack steps/rules]</returns>
        public CardDirections GetAttackDirections(string attacktitle)
        {
            CardDirections alldirections = new CardDirections();

            IEnumerator<AttackProperties> propssEnum = this.Properties.Attacks.First.Values.GetEnumerator();
            while (propssEnum.MoveNext())
            {
                if (propssEnum.Current.Title == attacktitle)
                {
                    IEnumerator<AttackDetails> detailsEnum = propssEnum.Current.AttackDict.Values.GetEnumerator();
                    detailsEnum.MoveNext();
                    alldirections = detailsEnum.Current.Directions;
                }
            }

            return alldirections;
        }
    }

    public class Deck
    {
        #region Variables
        /// <summary>
        /// Active Player's hand, list of all the cards in the players hand
        /// </summary>
        public List<Card> HandList = new List<Card>();

        /// <summary>
        /// Active Discard pile
        /// </summary>
        public List<Card> DiscardList = new List<Card>();

        /// <summary>
        /// Active faceDown deck pile
        /// </summary>
        public List<Card> FaceDownList = new List<Card>();

        /// <summary>
        /// Active Prize cards
        /// </summary>
        public List<Card> PrizeCardsList = new List<Card>();

        /// <summary>
        /// Active Bench cards
        /// </summary>
        public List<Card> BenchDict = new List<Card>();

        /// <summary>
        /// Path to the xml file that deck was created from
        /// </summary>
        private string xml
        {
            get { return _xml; }
            set { _xml = value; }
        }
        string _xml = "";

        /// <summary>
        /// Description of the Deck
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        string _Description = "";

        private Pokedex PokedexUsing
        {
            get { return _PokedexUsing; }
            set { _PokedexUsing = value; }
        }
        Pokedex _PokedexUsing ;

        /// <summary>
        /// Total Count of all Cards
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }
        int _Count = 0;

        public List<Card> AllCards
        {
            get { return _AllCards; }
            set { _AllCards = value; }
        }
        List<Card> _AllCards = new List<Card>();

        #endregion Variables
        
        public Deck()
        {
            //Owner ;Player or AI?
        }

        /// <summary>
        /// Create a Deck from XML
        /// </summary>
        /// <param name="xmlfile"></param>
        public Deck(string xmlfile, Pokedex pokedex)
        {
            try
            {
                this.xml = xmlfile;//remember the original xml
                this.PokedexUsing = pokedex;
                XPathDocument document = new XPathDocument(xmlfile);
                XPathNavigator navigator = document.CreateNavigator();
                XPathNodeIterator nodes = navigator.Select("/Deck");

                nodes.MoveNext();
                //Get the Deck's description
                this.Description = nodes.Current.GetAttribute("description", "");

                nodes = navigator.Select("/Deck/Card");
                //Examine each Card in Deck
                while (nodes.MoveNext())
                {
                    //Count how many cards are found
                    this.Count = this.Count + 1;

                    //Convert this node into a Card Object
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(nodes.Current.ReadSubtree());
                    Card newcard = new Card(xmldoc.FirstChild, pokedex);
                    newcard.Panel.Tag = new CardProperties();

                    //Panel handling
                    if (xmldoc.DocumentElement.ChildNodes[0].Attributes["CardType", ""].Value == "Energy")
                    {
                        newcard.Panel = new CardEnergyPanel();
                        newcard.Panel.Tag = newcard.Properties;
                    }
                    else if (xmldoc.DocumentElement.ChildNodes[0].Attributes["CardType", ""].Value == "Trainer" || xmldoc.DocumentElement.ChildNodes[0].Attributes["CardType", ""].Value == "Supporter" ||
                         xmldoc.DocumentElement.ChildNodes[0].Attributes["CardType", ""].Value == "Stadium")
                    {
                        newcard.Panel = new CardTrainerPanel();
                        newcard.Panel.Tag = newcard.Properties;
                    }
                    else
                    {
                        newcard.Panel = new CardPanel();
                        newcard.Panel.Tag = newcard.Properties;
                    }

                    this.AllCards.Add(newcard);
                    
                    //Display the card descriptions to the listbox
                    
                   // Console.WriteLine(nodes.Current.Name);//TESTING
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deck(string xmlfile) error: " + ex.Message);
            }


        }

        /// <summary>
        /// Does this deck have exactly 60 Cards?
        /// </summary>
        /// <returns>true if found 60</returns>
        public bool Is60Cards()
        {
            bool has60 = false;
            if (this.Count == 60)
                has60 = true;

            return has60;
        }

        /// <summary>
        /// Draw 1 card from the top of the deck AND adds it to the HandList
        /// (The top card is the highest index)
        /// </summary>
        public Card TakeCard(bool addToHandList)
        {
            Card topcard = new Card(Cardtype.Null);
            topcard = FaceDownList[FaceDownList.Count-1];
            FaceDownList.RemoveAt(FaceDownList.Count - 1);//remove the card from top

            if (addToHandList)
            {
                //Place this card in the HandList
                this.HandList.Add(topcard);
            }
            
            return topcard;
        }

        /// <summary>
        /// Draw 7 cards from the top of the deck
        /// </summary>
        public void Draw7CardsToHand()
        {
            for (int i = 0; i < 7; i++)
            {
                this.HandList.Add(this.TakeCard(false)); 
            }
            MessageBox.Show("Drew 7 cards from the top of " + this.Description);
            
        }

        /// <summary>
        /// Clears HandList, FaceDownList, DiscardList, PrizeCardsList 
        /// </summary>
        private void ClearDictionaries()
        {
            this.HandList.Clear();
            this.FaceDownList.Clear();
            this.DiscardList.Clear();
            this.PrizeCardsList.Clear();
        }

        /// <summary>
        /// Clear all Dict, Randomize cards in FaceDownList
        /// Checks for 60 cards in deck before proceeding
        /// </summary>
        public void ShuffleNewGame()
        {
            try
            {
                if (this.Is60Cards())
                {
                    //Clear all Dict
                    this.ClearDictionaries();

                    //Randomize cards and place them facedown
                    int count = this.AllCards.Count;
                    List<Card> cards = new List<Card>(this.AllCards.ToArray());
                    //     cards = .CopyTo(;

                    Random rand = new Random();
                    for (int i = 0; i < count; i++)
                    {
                        int randi = rand.Next(cards.Count);
                        Card randcard = cards[randi];
                        cards.RemoveAt(randi);
                        FaceDownList.Add(randcard);
                    }

                    Console.WriteLine(FaceDownList.Count.ToString());
                    MessageBox.Show(this.Description + ": Shuffled cards and place them facedown.");
                }
                else
                {
                  //  MessageBox.Show(this.Description + " deck does not have 60 cards. Please fix " + this.xml);
                    Exception GameDeckEx = new Exception(this.Description + " deck does not have 60 cards. Please fix " + this.xml);
                    throw GameDeckEx;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// Shuffle the faceDown cards, normally after a Supporter or Trainer card has been played.
        /// </summary>
        public void ShuffleFaceDown()
        {
            //Randomize FaceDownList
        }

        /// <summary>
        /// Search the HandList values for Basic Cards
        /// </summary>
        /// <returns></returns>
        public bool IsBasicInHand()
        {
            bool IsBasic = false;
            List<Card> cardList = new List<Card>();
            foreach (Card card in HandList)
            {
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    cardList.Add(card);//We may need to pass this later
                    
                }
            }

            if(cardList.Count > 0)
                IsBasic = true;
            else
                IsBasic = false;

            return IsBasic;
        }

        /// <summary>
        /// Search the HandList values for Basic Cards
        /// </summary>
        /// <returns></returns>
        public bool IsBasicInHand(out List<Card> basicinhandList)
        {
            bool IsBasic = false;
            List<Card> cardList = new List<Card>();
            foreach (Card card in HandList)
            {
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    cardList.Add(card);//We may need to pass this later
                }
            }
            if (cardList.Count > 0)
                IsBasic = true;
            else
                IsBasic = false;

            basicinhandList = cardList;

            return IsBasic;
        }

        /// <summary>
        /// Makes a clone of the Deck
        /// </summary>
        /// <returns>The cloned Deck</returns>
        public Deck CreateNewDeck()
        {
            Deck newDeck = new Deck(this.xml, this.PokedexUsing);
            return newDeck;
        }

        /// <summary>
        /// Selects all Basics in hand, then chooses Active Pokemon by highest HP
        /// </summary>
        public List<Card> GetAllBasicsInHand()
        {
            //List<Pair<int, Card>> totalList = new List<Pair<int, Card>>();
            List<Card> allbasics = new List<Card>();
            //Search for all Basics in hand
            this.IsBasicInHand(out allbasics);
            return allbasics;
        }

        /// <summary>
        /// Get a List of Energy Cards currently in HandList
        /// </summary>
        /// <returns></returns>
        public List<Card> GetAllEnergyInHand()
        {
            //List<Pair<int, Card>> totalList = new List<Pair<int, Card>>();
            List<Card> allenergy = new List<Card>();
            //Search for all Energy in hand
            foreach (Card card in HandList)
            {
                if (card.Properties.CardType == Cardtype.Energy)
                {
                    allenergy.Add(card);//We may need to pass this later
                }
            }
            return allenergy;
        }

        /// <summary>
        /// Can the given cardholder be evolved from a card in Hand?
        /// </summary>
        /// <param name="card">Desired card in a cardholder to evolve</param>
        /// <param name="processEvolve">True if you wish to auto evolve the card</param>
        /// <returns>True if the card can be evolved</returns>
        public bool CanEvolveCard(CardHolder cardholder, Pokedex pokedex, bool processEvolve)
        {
            bool canEvolve = false;
            if (cardholder.HasItem)
            {
                Card evolveCard = new Card(Cardtype.Null);
                //Get the card passed in this cardholder
                string neededCard = "None";
                //Get the card passed in this cardholder
                int ar = 0;
                if (cardholder.CurrentCard.Properties.CardType == Cardtype.Basic)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 1)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[2];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 2)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[3];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage2)
                {
                    //This maybe a Level-Up Card
                    ///MessageBox.Show("Deck.CanEvolveCard BUG: Evolving to Level-up card has not been tested.");
                }

                //What card does active pokemon need to evolve?
                //Search Hand for Pokemon

                foreach (Card item in this.HandList)
                {
                    //Make sure it is not a null Energy card etc.
                    if ((item.Properties.CardType == Cardtype.Stage1) || (item.Properties.CardType == Cardtype.Stage2) ||
                        (item.Properties.CardType == Cardtype.LevelUp))
                    {
                        if (item.Properties.Pokemon.Name.ToLower() == neededCard.ToLower())
                        {
                            if (processEvolve)
                            {
                                cardholder.Evolve(item);
                                canEvolve = true;
                            }
                            else
                            {
                                canEvolve = true;
                            }
                        }
                    }
                }
            }
            return canEvolve;
        }

        /// <summary>
        /// Get the card from HandList that can Evolve the given CardHolder.CurrentCard
        /// </summary>
        /// <param name="cardholder">CardHolder</param>
        /// <param name="pokedex">Current Pokedex</param>
        /// <returns>Possible Evolution Card</returns>
        public Card CanEvolveCard(CardHolder cardholder, Pokedex pokedex)
        {
            Card evolveCard = new Card(Cardtype.Null);
            if (cardholder.HasItem)
            {
                string neededCard = "None";
                //Get the card passed in this cardholder
                int ar = 0;
                if (cardholder.CurrentCard.Properties.CardType == Cardtype.Basic)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 1)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[2];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 2)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[3];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage2)
                {
                    //This maybe a Level-Up Card
                    ///MessageBox.Show("Deck.CanEvolveCard BUG: Evolving to Level-up card has not been tested.");
                }

                //What card does active pokemon need to evolve?
                //Search Hand for Pokemon

                foreach (Card item in this.HandList)
                {
                    //Make sure it is not a null Energy card etc.
                    if ((item.Properties.CardType == Cardtype.Stage1) || (item.Properties.CardType == Cardtype.Stage2) ||
                        (item.Properties.CardType == Cardtype.LevelUp))
                    {
                        if (item.Properties.Pokemon.Name.ToLower() == neededCard.ToLower())
                        {
                            evolveCard = item;
                        }
                    }
                }
            }
            return evolveCard;
        }

        /// <summary>
        /// Looks in Pokedex for name then searches AllCards for the Evolve card needed
        /// </summary>
        /// <param name="cardholder"></param>
        /// <param name="pokedex"></param>
        /// <returns></returns>
        public Card GetEvolveCardNeeded(CardHolder cardholder, Pokedex pokedex)
        {
            Card evolveCard = new Card(Cardtype.Null);
            if (cardholder.HasItem)
            {
                string neededCard = "None";
                //Get the card passed in this cardholder
                int ar = 0;
                if (cardholder.CurrentCard.Properties.CardType == Cardtype.Basic)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 1)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[2];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                {
                    if (cardholder.CurrentCard.Properties.Pokemon.EvolutionDict.Count > 2)
                    {
                        ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[3];
                        neededCard = pokedex.GetPokemon(ar).Name;
                    }
                }
                else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage2)
                {
                    //This maybe a Level-Up Card
                    ///MessageBox.Show("Deck.CanEvolveCard BUG: Evolving to Level-up card has not been tested.");
                }

                //What card does active pokemon need to evolve?
                //Search Hand for Pokemon

                foreach (Card item in this.AllCards)
                {
                    //Make sure it is not a null Energy card etc.
                    if ((item.Properties.CardType == Cardtype.Stage1) || (item.Properties.CardType == Cardtype.Stage2) ||
                        (item.Properties.CardType == Cardtype.LevelUp))
                    {
                        if (item.Properties.Pokemon.Name.ToLower() == neededCard.ToLower())
                        {
                            evolveCard = item;
                        }
                    }
                }
            }
            return evolveCard;
        }

        public bool CanAttachEnergyCard(CardHolder cardholder, GamePlay.GameState gstate, out Card energycard, bool processAttachment)
        {
            bool canattach = false;
            Card c = new Card(Cardtype.Null);
            //Is there energy card in hand?
            List<Card> EnergyCards = new List<Card>();
            EnergyCards = this.GetAllEnergyInHand();
            if (EnergyCards.Count > 0)
            {
                foreach (Card card in EnergyCards)
                {
                    //Is the attachedenergy count < count needed? Then do it
                    int bestattackneeds = Ops.GetBestAttackEnergyTotalReq(cardholder.CurrentCard);
                    if (card.State.GetAttachedEnergyCardCount() < bestattackneeds)
                    {
                        c = card;
                        canattach = true;
                        if (cardholder.CurrentCard.Properties.PokemonType == c.Properties.PokemonType)
                            break;

                    }
                }
                if (processAttachment)
                {
                    //Attach this Energy Card to ActivePokemon
                    cardholder.CurrentCard.State.AttachedCards.Add(c);
                    //Log this attached energy card, so no more
                    gstate.AttachedEnergy = true;
                    //Remove this Energy Card from HandList
                    this.HandList.Remove(c);
                }
            }
            energycard = c;
            return canattach;
        }

        /// <summary>
        /// Get a Card from this deck by same Description string.
        /// (Case-sensitive)
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Card from this deck, Card.CardType = Null if not found.</returns>
        public Card GetCardByDescription(string description)
        {
            Card cardFound = new Card(Cardtype.Null);

            foreach (Card card in this.AllCards)
            {
                if (card.Description == description)
                {
                    cardFound = card;
                    break;
                }
            }

            return cardFound;
        }

        /// <summary>
        /// Gets a certain PokemonTYPE Energy Card from the FaceDown pile
        /// </summary>
        /// <param name="desiredtype">PokemonTYPE</param>
        /// <returns>Card (Cardtype.Energy)</returns>
        public Card GetEnergyCard(PokemonTYPE desiredtype)
        {
            Card cardFound = new Card(Cardtype.Null);

            foreach (Card card in this.FaceDownList)
            {
                if (card.Properties.CardType == Cardtype.Energy &&
                    card.Properties.PokemonType == desiredtype)
                {
                    cardFound = card;
                    break;
                }
            }

            return cardFound;
        }

        /// <summary>
        /// Refreshes the BenchDict items by evaluating the current CardHolders
        /// </summary>
        /// <param name="player">GamePlay.Player</param>
        /// <param name="curGameState">GamePlay.GameState</param>
        public void BenchDictRefresh(GamePlay.Player player, GamePlay.GameState curGameState)
        {
            //Clear the current bench
            this.BenchDict.Clear();
            //Repopulate...
            if (player == PPokemon.GamePlay.Player.AIPLAYER)
            {
                foreach (CardHolder ch in curGameState.AIPlayerPokemon)
                {
                    //Make sure not to get the ActivePokemon
                    if (ch.HasItem)
                        if (!ch.CurrentCard.State.ActivePokemon)
                            this.BenchDict.Add(ch.CurrentCard);
                }
            }
            else
            {
                foreach (CardHolder ch in curGameState.Player1Pokemon)
                {
                    //Make sure not to get the ActivePokemon
                    if (ch.HasItem)
                        if (!ch.CurrentCard.State.ActivePokemon)
                            this.BenchDict.Add(ch.CurrentCard);
                }
            }
        }
    
    
    }

}
