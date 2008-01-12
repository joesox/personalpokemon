///////////////////////////////////////////////////////////
//
// http://www.go-pokemon.com/tcg/howtoplay/rules.html
///////////////////////////////////////////////////////////
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using PPokemon.Cards;

namespace PPokemon.GamePlay
{
    public enum GameHooks { 
        ActivePokemonChoosen,
        BenchPokemonChoosen,
        DamageDone,
        DrewACard,
        FlippedCoin,
        PlayerChange,
        PlayHandChanged,
        PokemonKnockedOut,
        ReEvalTurnPhase2, 
        SpecialConditionEnabled, 
        WonGamePrizeCards, 
        WonGameKnockedOutLast, 
        WonGameOutOfDeckCards
    };
    public enum Coin {HEADS, TAILS};
    public enum Player {PLAYER, AIPLAYER , NONE};
    public enum TurnPhase { TurnPhaseStart, TurnPhase1, TurnPhase2, TurnPhase3, TurnPhaseOver, Waiting, GameWon};

    public class GameState
    {
        /// <summary>
        /// Tracks each Round of turns
        /// </summary>
        public int Round
        {
            get { return _Round; }
            set { _Round = value; }
        }
        int _Round = 0;

        /// <summary>
        /// Player that is moving
        /// </summary>
        public Player CurrentPlayer
        {
            get { return _CurrentPlayer; }
            set { _CurrentPlayer = value; }
        }
        Player _CurrentPlayer = Player.NONE;

        /// <summary>
        /// Read the NextPlayer value. Opposite of CurrentPlayer
        /// </summary>
        public Player NextPlayer
        {
            get
            {
                if (this.CurrentPlayer == Player.PLAYER)
                    return Player.AIPLAYER;
                else if (this.CurrentPlayer == Player.AIPLAYER)
                    return Player.PLAYER;
                else
                    return Player.NONE;
            }
        }

        /// <summary>
        /// Gets or Sets the current Turn Phase
        /// </summary>
        public TurnPhase CurrentTurnPhase
        {
            get { return _CurrentTurnPhase; }
            set { _CurrentTurnPhase = value; }
        }
        TurnPhase _CurrentTurnPhase = TurnPhase.TurnPhaseStart;

        /// <summary>
        /// Get or Set if an Energy card was attached to a Pokemon dureing this turn
        /// </summary>
        public bool AttachedEnergy
        {
            get { return _AttachedEnergy; }
            set { _AttachedEnergy = value; }
        }
        bool _AttachedEnergy = false;

        /// <summary>
        /// Holds all of Player1's CardHolders to process attacks
        /// </summary>
        public List<CardHolder> Player1Pokemon = new List<CardHolder>();

        /// <summary>
        /// Holds all of AIPlayer's CardHolders to process attacks
        /// </summary>
        public List<CardHolder> AIPlayerPokemon = new List<CardHolder>();

        public Deck Player1Deck = new Deck();
        public Deck AIPlayerDeck = new Deck();

        /// <summary>
        /// Auto gets a Basic Card from the FaceDown pile
        /// </summary>
        /// <returns>Card (Cardtype.Basic)</returns>
        public Card GetBestBasicCardFromFaceDownPile(Player player)
        {
            Card cardFound = new Card(Cardtype.Null);
            List<Card> Stage1sInHandList = new List<Card>();

            Deck playersdeck = new Deck();

            //Get all of the Stage1 cards in hand
            if (player == Player.PLAYER)
            {
                playersdeck = this.Player1Deck;
                foreach (CardHolder onecardholder in Player1Pokemon)
                {
                    if (onecardholder.HasItem)
                    {
                        if (onecardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                            Stage1sInHandList.Add(onecardholder.CurrentCard);
                    }
                }
            }
            else
            {
                playersdeck = this.AIPlayerDeck;
                foreach (CardHolder onecardholder in AIPlayerPokemon)
                {
                    if (onecardholder.HasItem)
                    {
                        if (onecardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                            Stage1sInHandList.Add(onecardholder.CurrentCard);
                    }
                }
            }

            foreach (Card card in playersdeck.FaceDownList)
            {
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    //Get all of the Stage1 on this player's bench
                    if (Stage1sInHandList.Count > 0)
                    {
                        //Check to see if this card is the Basic card Evolve card on this player's h, if yes keep it
                        foreach (Card stage1card in Stage1sInHandList)
                        {
                            //if(stage1card.Properties.Pokemon.EvolutionDict
                        }
                    }
                    else
                    {
                        //else keep going, if none then choose the highest damage attack
                    }

                    cardFound = card;
                    break;
                }
            }
            //Remove the card from the facedown list
            playersdeck.FaceDownList.Remove(cardFound);
            return cardFound;
        }

        /// <summary>
        /// Auto gets a Basic Card from the FaceDown pile only for 
        /// a specific PokemonTYPE
        /// </summary>
        /// <returns>Card (Cardtype.Basic)</returns>
        public Card GetBestBasicCardFromFaceDownPile(Player player, PokemonTYPE pokemontype)
        {
            Card cardFound = new Card(Cardtype.Null);
            List<Card> Stage1sInHandList = new List<Card>();
            List<Card> BasicsFound = new List<Card>();

            Deck playersdeck = new Deck();

            //Get all of the Stage1 cards in hand
            if (player == Player.PLAYER)
            {
                playersdeck = this.Player1Deck;
                foreach (CardHolder onecardholder in Player1Pokemon)
                {
                    if (onecardholder.HasItem)
                    {
                        if (onecardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                            Stage1sInHandList.Add(onecardholder.CurrentCard);
                    }
                }
            }
            else
            {
                playersdeck = this.AIPlayerDeck;
                foreach (CardHolder onecardholder in AIPlayerPokemon)
                {
                    if (onecardholder.HasItem)
                    {
                        if (onecardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                            Stage1sInHandList.Add(onecardholder.CurrentCard);
                    }
                }
            }

            foreach (Card card in playersdeck.FaceDownList)
            {
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    //This is a Basic so add it
                    BasicsFound.Add(card);

                    //Get all of the Stage1 on this player's bench
                    if (Stage1sInHandList.Count > 0)
                    {
                        //Check to see if this card is the Basic card Evolve card on this player's h, if yes keep it
                        foreach (Card stage1card in Stage1sInHandList)
                        {
                            //if(stage1card.Properties.Pokemon.EvolutionDict
                            ///int ar = 0;
                            ///ar = cardholder.CurrentCard.Properties.Pokemon.EvolutionDict[1];
                            ///neededBasicCard = pokedex.GetPokemon(ar).Name;
                        }
                    }
                    else
                    {
                        //else keep going, if none then choose the highest damage attack
                    }

                    if (card.Properties.PokemonType == pokemontype || pokemontype == PokemonTYPE.Null)
                    {
                        cardFound = card;
                        break;
                    }
                }
            }
            //Remove the card from the facedown list
            playersdeck.FaceDownList.Remove(cardFound);
            return cardFound;
        }

        /// <summary>
        /// Auto gets a Basic Card from the player's BenchDict
        /// </summary>
        /// <returns>Card (Cardtype.Basic)</returns>
        public Card GetBestBasicCardFromBench(Player player)
        {
            Card cardFound = new Card(Cardtype.Null);
            bool found = false;

            if (player == Player.PLAYER)
            {
                if (this.Player1Deck.BenchDict.Count > 0)
                {
                    foreach (Card benchcard in this.Player1Deck.BenchDict)
                    {
                        //See if any have attached cards, if so choose that one
                        if (benchcard.State.AttachedCards.Count > 0)
                        {
                            cardFound = benchcard;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    //see which has the highest HP, and choose that one
                    //Just assign the first Card as the highest HP
                    cardFound = this.Player1Deck.BenchDict[0];
                    foreach (Card benchcard in this.Player1Deck.BenchDict)
                    {
                        if (benchcard.Properties.HP > cardFound.Properties.HP)
                            cardFound = benchcard;
                    }
                    found = true;
                }
            }
            else
            {
                if (this.AIPlayerDeck.BenchDict.Count > 0)
                {
                    foreach (Card benchcard in this.AIPlayerDeck.BenchDict)
                    {
                        //See if any have attached cards, if so choose that one
                        if (benchcard.State.AttachedCards.Count > 0)
                        {
                            cardFound = benchcard;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    //see which has the highest HP, and choose that one
                    //Just assign the first Card as the highest HP
                    cardFound = this.AIPlayerDeck.BenchDict[0];
                    foreach (Card benchcard in this.AIPlayerDeck.BenchDict)
                    {
                        if (benchcard.Properties.HP > cardFound.Properties.HP)
                            cardFound = benchcard;
                    }
                    found = true;
                }
            }
            return cardFound;
        }

        /// <summary>
        /// Get the CardHolder if it is the current Active Pokemon
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Active Pokemon CardHolder</returns>
        public CardHolder GetActivePokemonCardHolder(Player player)
        {
            CardHolder chActiveP = new CardHolder();
            if (player == Player.PLAYER)
            {
                foreach (CardHolder ch in this.Player1Pokemon)
                {
                    if (ch.CurrentCard.State.ActivePokemon)
                    {
                        chActiveP = ch;
                        break;
                    }
                }
            }
            else
            {
                foreach (CardHolder ch in this.AIPlayerPokemon)
                {
                    if (ch.CurrentCard.State.ActivePokemon)
                    {
                        chActiveP = ch;
                        break;
                    }
                }
            }

            return chActiveP;
        }

        /// <summary>
        /// Gets first Energy Card from the FaceDown pile only for 
        /// a specific PokemonTYPE. Removes the Card from the FaceDownPile
        /// </summary>
        /// <returns>PokemonTYPE (PokemonTYPE.Grass, etc.)</returns>
        public List<Card> GetEnergyCardFromFaceDownPile(Player player, PokemonTYPE pokemontype, int qty)
        {
            List<Card> EnergyFound = new List<Card>();

            Deck playersdeck = new Deck();

            //Get all of the Stage1 cards in hand
            if (player == Player.PLAYER)
            {
                playersdeck = this.Player1Deck;
            }
            else
            {
                playersdeck = this.AIPlayerDeck;
            }

            Card cardFound = new Card(Cardtype.Energy);
            for (int i = 0; i < qty; i++)
            {
                foreach (Card card in playersdeck.FaceDownList)
                {
                    if (card.Properties.CardType == Cardtype.Energy)
                    {
                        if (card.Properties.PokemonType == pokemontype)
                        {
                            //This is an Energy card so add it
                            EnergyFound.Add(card);
                            cardFound = card;
                            break;
                        }
                    }
                }

                //If nothing found and  pokemontype == PokemonTYPE.Null search again and get the first energy card
                if (EnergyFound.Count <= 0 && pokemontype == PokemonTYPE.Null)
                {
                    foreach (Card card in playersdeck.FaceDownList)
                    {
                        if (card.Properties.CardType == Cardtype.Energy)
                        {
                                //This is an Energy card so add it
                                EnergyFound.Add(card);
                                cardFound = card;
                                break;
                        }
                    }
                }

                //Remove the card from the facedown list
                playersdeck.FaceDownList.Remove(cardFound);
            }
            return EnergyFound;
        }

        /// <summary>
        /// Current Pokedex being used for game
        /// </summary>
        public Pokedex PokeDex;
    }

    class GameEngine
    {
        #region Hooks
        public delegate void HookHandler(GameHooks message, object args);
        public event HookHandler HookEvent;
        protected void OnProcessHook(GameHooks message, object args)
        {
            if (HookEvent != null)
            {
                HookEvent(message, args);
            }
        }
        #endregion

        #region Constructor
        public GameEngine()
        {

        }
        #endregion

        #region Variables
        public GameState State = new GameState();
        public TrainerPlanner TrainerEngine = new TrainerPlanner();
        #endregion

        /// <summary>
        /// Check both players deck count for 60
        /// </summary>
        public bool CheckDeckCounts(Deck playerdeck1, Deck ai_playerdeck)
        {
            bool result = false;

            if (!playerdeck1.Is60Cards())
            {
                MessageBox.Show(playerdeck1.Description + "deck is not 60 cards. Can not start game until fixed.");
                result = false;
            }
            else
                result = true;

            if (!ai_playerdeck.Is60Cards())
            {
                MessageBox.Show(ai_playerdeck.Description + "deck is not 60 cards. Can not start game until fixed.");
                result = false;
            }
            else
                result = true;

                return result;
        }

        /// <summary>
        /// Shake Hands, Shuffle, Draw 7, 
        /// </summary>
        /// <param name="playerdeck1"></param>
        /// <param name="playerdeck2"></param>
        public bool StartGame(Pokedex pokedex, Deck playerdeck1, Deck ai_playerdeck, CardHolder player1ActivePokemon, CardHolder player1Bench1,
            CardHolder player1Bench2, CardHolder player1Bench3, CardHolder player1Bench4, CardHolder player1Bench5,
            CardHolder aiplayerActivePokemon, CardHolder aiplayerBench1, CardHolder aiplayerBench2, CardHolder aiplayerBench3,
            CardHolder aiplayerBench4, CardHolder aiplayerBench5)
        {
            bool CanStartGame = false;

            //Shake Hands
            if (CheckDeckCounts(playerdeck1, ai_playerdeck))
            {
                //Shuffle
                Shuffle(playerdeck1);
                Shuffle(ai_playerdeck);

                //Draw 7
                Draw7CardsToHand(playerdeck1, Player.PLAYER);
                Draw7CardsToHand(ai_playerdeck, Player.AIPLAYER);

                //Any Basic Pokemon in player's hands?
                IsBasicInHand(playerdeck1, Player.PLAYER);
                IsBasicInHand(ai_playerdeck, Player.AIPLAYER);

                //Learn the CardHolders
                this.State.Player1Pokemon.Add(player1ActivePokemon);
                this.State.Player1Pokemon.Add(player1Bench1);
                this.State.Player1Pokemon.Add(player1Bench2);
                this.State.Player1Pokemon.Add(player1Bench3);
                this.State.Player1Pokemon.Add(player1Bench4);
                this.State.Player1Pokemon.Add(player1Bench5);
                this.State.AIPlayerPokemon.Add(aiplayerActivePokemon);
                this.State.AIPlayerPokemon.Add(aiplayerBench1);
                this.State.AIPlayerPokemon.Add(aiplayerBench2);
                this.State.AIPlayerPokemon.Add(aiplayerBench3);
                this.State.AIPlayerPokemon.Add(aiplayerBench4);
                this.State.AIPlayerPokemon.Add(aiplayerBench5);
                //Learn the Decks
                this.State.Player1Deck = playerdeck1;
                this.State.AIPlayerDeck = ai_playerdeck;
                //Learn the Pokedex
                this.State.PokeDex = pokedex;

                this.State.Round = 1;
                CanStartGame = true;
            }
            else
            {
                CanStartGame = false;
            }

            return CanStartGame;
        }

        /// <summary>
        /// Play local copy of attacksound.wav
        /// </summary>
        private void PlayAttackSound()
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
            sound.SoundLocation = Common.attacksound_wav;
            sound.Play();
        }

        /// <summary>
        /// Song sound for end of game if won.
        /// </summary>
        public void PlayEndSong()
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
            sound.SoundLocation = Common.endsong_wav;
            sound.Play();
        }

        /// <summary>
        /// Check reshuffling until player receives basic cards
        /// </summary>
        /// <param name="playerdeck1"></param>
        public void IsBasicInHand(Deck playerdeck, Player player)
        {
            if (playerdeck.IsBasicInHand())
            {
                //breakout of loop
            }
            else
            {
                MessageBox.Show("NO BASIC CARD DRAWN FOR ACTIVE POKEMON!\r\nMust re-shuffle and draw 7 more cards to hand...");
                Shuffle(playerdeck);
                Draw7CardsToHand(playerdeck, player);
                IsBasicInHand(playerdeck, player);
            }
        }

        /// <summary>
        /// Draw 7 cards from the top of the deck
        /// </summary>
        /// <param name="playerdeck">Deck</param>
        public void Draw7CardsToHand(Deck playerdeck, Player player)
        {
            playerdeck.Draw7CardsToHand();
            //Fire the PlayHandChanged
            if (player == GamePlay.Player.PLAYER)
            {
                this.OnProcessHook(GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
            }
            else if (player == GamePlay.Player.AIPLAYER)
            {
                this.OnProcessHook(GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
            }
        }

        /// <summary>
        /// Shuffle a player's deck
        /// </summary>
        /// <param name="playerdeck"></param>
        public void Shuffle(Deck playerdeck)
        {
            playerdeck.ShuffleNewGame();
        }

        /// <summary>
        /// Flip a Coin
        /// </summary>
        /// <returns>HEADS or TAILS</returns>
        public Coin FlipACoin()
        {
            GamePlay.Coin coin = new Coin();
            //Browse to the xml File
            Random rand = new Random();
            switch (rand.Next(0, 2))
            {
                case 0:
                    coin = PPokemon.GamePlay.Coin.HEADS;
                    break;
                case 1:
                    coin = PPokemon.GamePlay.Coin.TAILS;
                    break;
                default:
                    break;
            }
            //Fire the Event only if first Round
            if (this.State.Round <= 1)
            {
                this.OnProcessHook(GameHooks.FlippedCoin, coin);
            }

            return coin;
        }

        /// <summary>
        /// Call this at the end of a turn
        /// Updates: Round, CurrentTurnPhase, CurrentPlayer, CurrentCard.State.EvolvedPokemon, and AttachedEnergy
        /// </summary>
        /// <param name="player"></param>
        public void EndOfTurn(Player player, CardHolder activepokemon)
        {
            //Do all the things that need to be done at the end of this player's turn
            #region SpecialCondition checks
            if (activepokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Poisoned)
            {
                #region Take Poison Damage, KnockedOut?
                activepokemon.CurrentCard.State.CurrentDamage = activepokemon.CurrentCard.State.CurrentDamage + 10;
                MessageBox.Show(player.ToString() + "'s ActivePokemon was still Poisoned so +10 damage was applied!", "EndOfTurn: SpecialConditions");
                //Now check to see if it was KnockedOut
                if (activepokemon.CurrentCard.State.CurrentDamage >= activepokemon.CurrentCard.Properties.HP)
                {
                    MessageBox.Show(activepokemon.CurrentCard.Properties.Pokemon.Name + " is Knocked Out!!", "EndOfTurn: SpecialConditions");
                    this.KnockOut(activepokemon, player);
                    this.OnProcessHook(GameHooks.PokemonKnockedOut, player);
                }
                #endregion
            }
            if (activepokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Asleep)
            {
                #region Flip a coin to see if Asleep recovery
                if (player == Player.PLAYER)
                {
                    MessageBox.Show("If heads, your active pokemon wakes up. If tails, it is still asleep and can not attack or retreat.", "ActivePokemon is Asleep. Recovery?!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (this.FlipACoin() == Coin.HEADS)
                    {
                        //Active Pokemon wakes up!
                        activepokemon.CurrentCard.State.CurrentCondition = SpecialConditions.None;
                        MessageBox.Show("HEADS! Your active pokemon has woke up.", "ActivePokemon is Awake!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //Still asleep
                        MessageBox.Show("TAILS! Your active pokemon is still asleep.\r\nIt can not attack or retreat on your next turn.", "ActivePokemon is Still Asleep!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    //AIPlayer
                    if (this.FlipACoin() == Coin.HEADS)
                    {
                        //Active Pokemon wakes up!
                        activepokemon.CurrentCard.State.CurrentCondition = SpecialConditions.None;
                        MessageBox.Show("HEADS! Your Opponent's active pokemon has woke up.", "Opponent's ActivePokemon is Awake!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //Still asleep
                        MessageBox.Show("TAILS! Your Opponent's active pokemon is still asleep.\r\nIt can not attack or retreat on Opponent's next turn.", "Opponent's ActivePokemon is Still Asleep!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                #endregion
            }
            if (activepokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Paralyzed)
            {
                #region Paralyzed recovery
                //No flips necessary, Pokemon auto recovers
                if(player == Player.PLAYER)
                    MessageBox.Show("Your active pokemon is no longer Paralyzed.", "ActivePokemon may attack or retreat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Your Opponent's active pokemon is no longer Paralyzed.", "Opponent's ActivePokemon may attack or retreat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                activepokemon.CurrentCard.State.CurrentCondition = SpecialConditions.None;
                #endregion
            }
            #endregion

            //Update Round
            this.State.Round = this.State.Round + 1;
            #region See if State needs to Wait, if not TurnPhase.TurnPhaseStart
            if (this.State.CurrentTurnPhase != TurnPhase.Waiting)
                this.State.CurrentTurnPhase = TurnPhase.TurnPhaseStart;
            #endregion
            //Reset value so player may attach energy card
            this.State.AttachedEnergy = false;

            //Reset this player's CurrentCard.State.EvolvedPokemon
            if (player == Player.PLAYER)
            {
                foreach (CardHolder ch in this.State.Player1Pokemon)
                {
                    if(ch.HasItem)
                        ch.CurrentCard.State.EvolvedPokemon = false;
                }
                
            }
            else
            {
                foreach (CardHolder ch in this.State.AIPlayerPokemon)
                {
                    if (ch.HasItem)
                        ch.CurrentCard.State.EvolvedPokemon = false;
                }
            }

            #region Actual Player change of state, Begining of Turn, Check Deck Count
            if (player == Player.PLAYER)
            {
                this.State.CurrentPlayer = Player.AIPLAYER;
                if (this.State.AIPlayerDeck.FaceDownList.Count == 0)
                    this.HookEvent(GameHooks.WonGameOutOfDeckCards, Player.PLAYER);
            }
            else
            {
                this.State.CurrentPlayer = Player.PLAYER;
                if (this.State.Player1Deck.FaceDownList.Count == 0)
                    this.HookEvent(GameHooks.WonGameOutOfDeckCards, Player.AIPLAYER);
            }
            #endregion
        }

        /// <summary>
        /// Perform an attack
        /// (this.State.CurrentPlayer must be set correctly)
        /// </summary>
        /// <param name="chAttackingPokemon">Attacking CardHolder</param>
        /// <param name="chDefendingPokemon">Defending CardHolder</param>
        /// <param name="activeAttack">AttackProperties of the attack being used.</param>
        public StringBuilder ProcessAttack(CardHolder chAttackingPokemon, CardHolder chDefendingPokemon, AttackProperties activeAttack, bool nonattack)
        {
            StringBuilder explainations = new StringBuilder();//explain what happened
            int attackDamage = activeAttack.Damage;
            bool FlipNeeded = false;
            bool FlipResultPassed = false;
            bool okRemovefromSelf = true;//RemoveDamageCounter and DAMAGEDONE
            Wintellect.PowerCollections.Pair<Coin, int> FlipStorageDict = new Wintellect.PowerCollections.Pair<Coin, int>();
            bool playattacksound = false;
            List<CardHolder> KOPlayerBenchList = new List<CardHolder>();
            List<CardHolder> KOAIPlayerBenchList = new List<CardHolder>();
            List<CardHolder> ChooseOpponPokemonList = new List<CardHolder>();
            List<CardHolder> SelectedPokemonList = new List<CardHolder>();
            List<Card> EnergyList = new List<Card>();
            List<Card> CardsFromDeckList = new List<Card>();
            IEnumerator<AttackDetails> attackEnum = activeAttack.AttackDict.Values.GetEnumerator();

            if (activeAttack.Title.Trim() == "" && activeAttack.Damage == 0)
            {
                explainations.AppendLine("Could not attack. No active attack.");
            }
            else
            {
                if (nonattack == false)
                    explainations.AppendLine(activeAttack.Title + " attack results:");
                else
                    explainations.AppendLine(activeAttack.Title + " results:");

                //Double check that this Pokemon has this attack enabled, or is it a nonattack (trainer etc.)
                if (chAttackingPokemon.CurrentCard.GetEnabledAttackList().Contains(activeAttack) || nonattack == true)
                {
                    CardDirections cardDir = new CardDirections();
                    if (nonattack == false)
                        cardDir = chAttackingPokemon.CurrentCard.GetAttackDirections(activeAttack.Title);
                    else
                    {
                        //This is a Trainer card, etc. 
                        cardDir = Ops.GetAttackDirections(activeAttack);
                    }

                    //Step thru each AttackDirectionsCardEventNames
                    //First is the CardEventNames
                    //Second is the args string
                    foreach (Wintellect.PowerCollections.Pair<CardEventNames, string> pair in cardDir.Directions)
                    {
                        //Read all the arguments
                        StringCollection sc = new StringCollection();
                        sc = Ops.ReadCardEventNamesArgs(pair.Second);
 
                        if (pair.First != CardEventNames.None)
                        {
                            #region FlipACoin
                            if (pair.First == CardEventNames.FlipACoin)
                            {
                                FlipNeeded = true;

                                if (sc[0].ToLower().Trim() == "until")
                                {
                                    //sc[0]: "UNTIL TAILS" etc
                                    //when do we break the flipping loop?
                                    bool continueflipping = true;
                                    Coin flip = Coin.HEADS;
                                    Coin StopResult = Ops.ConvertStringToCoin(sc[1]);
                                    int ResultCount = 0;
                                    while (continueflipping)
                                    {
                                        flip = this.FlipACoin();
                                        if (flip != StopResult)
                                        {
                                            ResultCount = ResultCount + 1;
                                        }
                                        else
                                        {
                                            continueflipping = false;
                                        }
                                    }
                                    //Flipping loop has ended, flip will always be StopResult at this point so record opposite
                                    if (flip == Coin.TAILS)
                                        FlipStorageDict.First = Coin.HEADS;
                                    else
                                        FlipStorageDict.First = Coin.TAILS;

                                    FlipStorageDict.Second = ResultCount;
                                }
                                else
                                {
                                   // sc[0]: HEADS = command
                                    Coin cResult = this.FlipACoin();//Flip a coin
                                    explainations.AppendLine("Coin flip result:" + cResult.ToString());
                                    //Evaluate if the flip enables the attack
                                    if (cResult == Ops.ConvertStringToCoin(sc[0]))
                                    {
                                        FlipResultPassed = true;
                                        //now read each arg starting after desired result
                                        for (int i = 1; i < sc.Count; i++)
                                        {
                                            if (sc[i].ToLower() == "=")
                                            {
                                                //Nothing goto next value
                                            }
                                            if (sc[i].ToLower() == "next")
                                            {
                                                //Wants to read next Directions so break out of this loop
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            
                            #endregion
                        }
                        if (pair.First == CardEventNames.ShuffleDeck)
                        {
                            #region ShuffleDeck
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                this.State.Player1Deck.ShuffleFaceDown();
                                explainations.AppendLine("Shuffled deck.");
                            }
                            else
                            {
                                //AIPlayer
                                this.State.AIPlayerDeck.ShuffleFaceDown();
                                explainations.AppendLine("Shuffled deck.");
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.AddSpecialCToAttack)
                        {
                            #region AddSpecialCToAttack
                            if (sc.Count > 0)
                            {
                                if (FlipNeeded == false || FlipResultPassed == true)
                                {
                                    //Should only be one specialcondition listed in arg
                                    DoSpecialCondition(chDefendingPokemon, sc[0]);
                                    explainations.AppendLine("AddSpecialCToAttack: " + pair.Second.ToString());
                                }
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.SearchDeckForBasic)
                        {
                            #region SearchDeckForBasic (Adds to HandList)
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                //Get the Best Basic Card from the Deck.FaceDownlist
                                Card bestBasic = new Card(Cardtype.Basic);
                                bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.PLAYER);

                                if (bestBasic.Description != "")
                                {
                                    //If there are no arguments on this attack direction then just add the Basic to hand
                                    if (sc.Count == 0)
                                    {
                                        this.State.Player1Deck.HandList.Add(bestBasic);
                                        explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                        this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                    }
                                    else
                                    {
                                        //There are some directions so lets do them
                                        //There should only be one PokemonTYPE in args
                                        if (bestBasic.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[0].Trim()))
                                        {
                                            this.State.Player1Deck.HandList.Add(bestBasic);
                                            explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                            this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                        }
                                        else
                                        {
                                            //Just get the first card thast works
                                            bestBasic = this.State.GetBestBasicCardFromFaceDownPile(this.State.CurrentPlayer, Ops.ConvertStringToPokemonTYPE(sc[0].Trim()));

                                            if (bestBasic.Description != "")
                                            {
                                                this.State.Player1Deck.HandList.Add(bestBasic);
                                                explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Get the Best Basic Card from the Deck.FaceDownlist
                                Card bestBasic = new Card(Cardtype.Basic);
                                bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.AIPLAYER);
                                if (bestBasic.Description != "")
                                {//If there are no arguments on this attack direction then just add the Basic to hand
                                    if (sc.Count == 0)
                                    {
                                        this.State.AIPlayerDeck.HandList.Add(bestBasic);
                                        explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                        this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                    }
                                    else
                                    {
                                        //There are some directions so lets do them
                                        //There should only be one PokemonTYPE in args
                                        if (bestBasic.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[0].Trim()))
                                        {
                                            this.State.AIPlayerDeck.HandList.Add(bestBasic);
                                            explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                            this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                        }
                                        else
                                        {
                                            //Just get the first card thast works
                                            bestBasic = this.State.GetBestBasicCardFromFaceDownPile(this.State.CurrentPlayer, Ops.ConvertStringToPokemonTYPE(sc[0].Trim()));

                                            if (bestBasic.Description != "")
                                            {
                                                this.State.AIPlayerDeck.HandList.Add(bestBasic);
                                                explainations.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.SearchDeckForBasicPlaceToBench)
                        {
                            #region SearchDeckForBasicPlaceToBench
                            StringBuilder ToBenchLog = new StringBuilder();
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                //Get the Best Basic Card from the Deck.FaceDownlist
                                Card bestBasic = new Card(Cardtype.Basic);
                                bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.PLAYER);

                                if (bestBasic.Description != "")
                                {
                                    //If there are no arguments on this attack direction then just add the Basic to Bench
                                    if (sc.Count == 0)
                                    {
                                        this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                        explainations.Append(ToBenchLog.ToString());
                                    }
                                    else
                                    {
                                        //There are some directions so lets do them
                                        //There should only be one PokemonTYPE in args
                                        if (bestBasic.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[0].Trim()))
                                        {
                                            this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                            explainations.Append(ToBenchLog.ToString());
                                        }
                                        else
                                        {
                                            //Just get the first card thast works
                                            bestBasic = this.State.GetBestBasicCardFromFaceDownPile(this.State.CurrentPlayer, Ops.ConvertStringToPokemonTYPE(sc[0].Trim()));

                                            if (bestBasic.Description != "")
                                            {
                                                this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                                explainations.Append(ToBenchLog.ToString());
                                            }
                                            else
                                                explainations.AppendLine("No Basics found.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //AIPLAYER
                                //Get the Best Basic Card from the Deck.FaceDownlist
                                Card bestBasic = new Card(Cardtype.Basic);
                                bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.AIPLAYER);
                                if (bestBasic.Description != "")
                                {
                                    this.PlaceCardToBenchForAttack(bestBasic, Player.AIPLAYER, out ToBenchLog);
                                    explainations.Append(ToBenchLog.ToString());
                                }
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.AddDamageToAttack)
                        {
                            #region AddDamageToAttack (adds damage to attackDamage)
                            //Add the value to the attackDamage
                            try
                            {
                                if (FlipNeeded == false || FlipResultPassed == true)
                                {
                                    if (ChooseOpponPokemonList.Count > 0)
                                    {
                                        //There is Choosen Pokemon so assume that this dammage is for it/them
                                        if (this.State.CurrentPlayer == Player.PLAYER)
                                        {
                                            //Now add the Damage to these
                                            foreach (CardHolder ch in ChooseOpponPokemonList)
                                            {   //sc[1] is the '='
                                                if (!ch.CurrentCard.State.ActivePokemon)
                                                {
                                                    ch.CurrentCard.State.CurrentDamage = ch.CurrentCard.State.CurrentDamage + Convert.ToInt16(sc[0].Trim());
                                                    explainations.AppendLine("AddDamageToAttack: Applied " + sc[0].Trim() + " to " + ch.CurrentCard.Description);
                                                    if (ch.CurrentCard.State.CurrentDamage >= ch.CurrentCard.Properties.HP)
                                                    {
                                                        KOAIPlayerBenchList.Add(ch);//Add this Pokemon to the Knockedoutlist
                                                        this.State.AIPlayerDeck.BenchDict.Remove(ch.CurrentCard);//Remove from BenchDict b/c no event fired
                                                    }
                                                    playattacksound = true;
                                                }
                                                else
                                                {
                                                    //add the damage to the attackdamge so weakness and resistence is applied
                                                    attackDamage = attackDamage + Convert.ToInt16(sc[0].Trim());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Now add the Damage to these
                                            foreach (CardHolder ch in ChooseOpponPokemonList)
                                            {   //sc[1] is the '='
                                                if (!ch.CurrentCard.State.ActivePokemon)
                                                {
                                                    ch.CurrentCard.State.CurrentDamage = ch.CurrentCard.State.CurrentDamage + Convert.ToInt16(sc[0].Trim());
                                                    explainations.AppendLine("AddDamageToAttack: Applied " + sc[0].Trim() + " to " + ch.CurrentCard.Description);
                                                    if (ch.CurrentCard.State.CurrentDamage >= ch.CurrentCard.Properties.HP)
                                                    {
                                                        KOPlayerBenchList.Add(ch);//Add this Pokemon to the Knockedoutlist
                                                        this.State.Player1Deck.BenchDict.Remove(ch.CurrentCard);//Remove from BenchDict b/c no event fired
                                                    }
                                                    playattacksound = true;
                                                }
                                                else
                                                {
                                                    //add the damage to the attackdamge so weakness and resistence is applied
                                                    attackDamage = attackDamage + Convert.ToInt16(sc[0].Trim());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //No ChoosenPokemon
                                        attackDamage = attackDamage + Convert.ToInt16(sc[0].Trim());
                                        explainations.AppendLine("AddDamageToAttack: " + sc[0].Trim());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ProcessAttack Error: " + chAttackingPokemon.CurrentCard.Description + " has invalid value for " +
                                    pair.First.ToString() + "\r\nThis is normally caused by incorrect CardDirections values in this card's xml.\r\nPlease visit the online documentation to learn the correct settings for " +
                                    pair.First.ToString() + "\r\nOther Exceptions: " + ex.Message);
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.DiscardAnEnergy)
                        {
                            #region DiscardAnEnergy (Removes sc[0]'s first Energy card found or sc[2] Energy type)
                            if (FlipNeeded == false || FlipResultPassed == true)
                            {
                                string pokemonArg = sc[0].ToLower().Trim();
                                Card cardtoremove = new Card(Cardtype.Energy);
                                
                                if (pokemonArg == "" || pokemonArg == "attacking")
                                {
                                    #region attacking pokemon
                                    if (chAttackingPokemon.CurrentCard.State.GetAttachedEnergyCardCount() > 0)
                                    {
                                        foreach (Card carditem in chAttackingPokemon.CurrentCard.State.AttachedCards)
                                        {
                                            if (carditem.Properties.CardType == Cardtype.Energy)
                                            {
                                                if (sc.Count == 1)
                                                {
                                                    cardtoremove = carditem;
                                                    break;
                                                }
                                                else
                                                {
                                                    //Looking for a specific energy card
                                                    if (carditem.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[2].Trim()))
                                                    {
                                                        cardtoremove = carditem;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (cardtoremove.Properties.PokemonType != PokemonTYPE.Null)
                                        {
                                            //Found an energy card to remove
                                            chAttackingPokemon.CurrentCard.State.AttachedCards.Remove(cardtoremove);
                                            explainations.AppendLine("DiscardAnEnergy: AttackingPokemon's " + cardtoremove.Description);
                                            playattacksound = true;
                                        }
                                        else
                                        {
                                            //This card has no energy cards so something is wrong
                                            explainations.AppendLine("DiscardAnEnergy: Could not discard any energy cards from AttackingPokemon.");
                                        }
                                    }
                                    else
                                    {
                                        //This card has no energy cards so something is wrong
                                        explainations.AppendLine("DiscardAnEnergy: AttackingPokemon has no energy cards.");
                                    }
                                    #endregion
                                }
                                if (pokemonArg == "defending")
                                {
                                    #region defending pokemon
                                    if (chDefendingPokemon.CurrentCard.State.GetAttachedEnergyCardCount() > 0)
                                    {
                                        foreach (Card carditem in chDefendingPokemon.CurrentCard.State.AttachedCards)
                                        {
                                            if (carditem.Properties.CardType == Cardtype.Energy)
                                            {
                                                if (sc.Count == 1)
                                                {
                                                    cardtoremove = carditem;
                                                    break;
                                                }
                                                else
                                                {
                                                    //Looking for a specific energy card
                                                    if (carditem.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[2].Trim()))
                                                    {
                                                        cardtoremove = carditem;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (cardtoremove.Properties.PokemonType != PokemonTYPE.Null)
                                        {
                                            //Found an energy card to remove
                                            chDefendingPokemon.CurrentCard.State.AttachedCards.Remove(cardtoremove);
                                            explainations.AppendLine("DiscardAnEnergy: DefendingPokemon's " + cardtoremove.Description);
                                            playattacksound = true;
                                        }
                                        else
                                        {
                                            //This card has no energy cards so something is wrong
                                            explainations.AppendLine("DiscardAnEnergy: Could not discard any energy cards from DefendingPokemon.");
                                        }
                                    }
                                    else
                                    {
                                        //This card has no energy cards so something is wrong
                                        explainations.AppendLine("DiscardAnEnergy: DefendingPokemon has no energy cards.");
                                    }
                                    #endregion
                                }
                                
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.IfLessEnergyThanDefending)
                        {
                            #region IfLessEnergyThanDefending (If This Pokemon has less Energy attached then the defending then continue)
                            int atackingEnergyC = chAttackingPokemon.CurrentCard.State.GetAttachedEnergyCardCount();
                            int defendingEnergyC = chDefendingPokemon.CurrentCard.State.GetAttachedEnergyCardCount();
                            if (atackingEnergyC < defendingEnergyC)
                            {
                                explainations.AppendLine("IfLessEnergyThanDefending: Continuing attack; " + atackingEnergyC.ToString() + " < " + defendingEnergyC.ToString());
                            }
                            else
                            {
                                //Do not continue this attack since condition not met
                                explainations.AppendLine("IfLessEnergyThanDefending: Can not continue this attack; " + atackingEnergyC.ToString() + " > " + defendingEnergyC.ToString());
                                break;
                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.AddDamageToSelf)
                        {
                            #region AddDamageToSelf (Adds damage/from xml value to AttackingPokemon)
                            int internaldamage = Convert.ToInt16(sc[0].Trim());
                            chAttackingPokemon.CurrentCard.State.CurrentDamage = chAttackingPokemon.CurrentCard.State.CurrentDamage + internaldamage;
                            explainations.AppendLine("AddDamageToSelf Damage done: " + internaldamage.ToString());
                            this.OnProcessHook(GameHooks.DamageDone, this.State.NextPlayer);
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.AddDamageToEachBenched)
                        {
                            #region AddDamageToEachBenched (Adds damage/from xml value to Oppenents entire Bench. Then adds to KnockedOutBenchList)
                            if (FlipNeeded == false || FlipResultPassed == true)
                            {
                                int damagetoapply = Convert.ToInt16(sc[0].Trim());
                                if (this.State.CurrentPlayer == Player.PLAYER)
                                {
                                    //Hit AIPlayer Bench...
                                    foreach (CardHolder chitem in this.State.AIPlayerPokemon)
                                    {
                                        if (chitem.HasItem)
                                        {
                                            if (!chitem.CurrentCard.State.ActivePokemon)
                                            {
                                                playattacksound = true;
                                                chitem.CurrentCard.State.CurrentDamage = chitem.CurrentCard.State.CurrentDamage + damagetoapply;
                                                explainations.AppendLine("AddDamageToEachBenched: Applied " + damagetoapply.ToString() + " to " + chitem.CurrentCard.Description);
                                                if (chitem.CurrentCard.State.CurrentDamage >= chitem.CurrentCard.Properties.HP)
                                                {
                                                    KOAIPlayerBenchList.Add(chitem);//Add this Pokemon to the Knockedoutlist
                                                    this.State.AIPlayerDeck.BenchDict.Remove(chitem.CurrentCard);//Remove from BenchDict b/c no event fired
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Hit Player1 Bench...
                                    foreach (CardHolder chitem in this.State.Player1Pokemon)
                                    {
                                        if (chitem.HasItem)
                                        {
                                            if (!chitem.CurrentCard.State.ActivePokemon)
                                            {
                                                playattacksound = true;
                                                chitem.CurrentCard.State.CurrentDamage = chitem.CurrentCard.State.CurrentDamage + damagetoapply;
                                                explainations.AppendLine("AddDamageToEachBenched: Applied " + damagetoapply.ToString() + " to " + chitem.CurrentCard.Description);
                                                if (chitem.CurrentCard.State.CurrentDamage >= chitem.CurrentCard.Properties.HP)
                                                {
                                                    KOPlayerBenchList.Add(chitem);//Add this Pokemon to the Knockedoutlist
                                                    this.State.Player1Deck.BenchDict.Remove(chitem.CurrentCard);//Remove from BenchDict b/c no event fired
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.AddDamageToBenched)
                        {
                            #region AddDamageToBenched (Calls GetBenchCardHolder() and adds damage to a List of benched Pokemon)
                            List<CardHolder> selectedBench = new List<CardHolder>();
                            //Get the Selected Bench by highest Damaged done
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                selectedBench = GetBenchCardHolder(Player.AIPLAYER, Convert.ToInt16(sc[0].Trim()), true);
                                //Now add the Damage to these
                                foreach (CardHolder ch in selectedBench)
                                {   //sc[1] is the '='
                                    ch.CurrentCard.State.CurrentDamage = ch.CurrentCard.State.CurrentDamage + Convert.ToInt16(sc[2].Trim());
                                    explainations.AppendLine("AddDamageToBenched: Applied " + sc[2].Trim() + " to " + ch.CurrentCard.Description);
                                    if (ch.CurrentCard.State.CurrentDamage >= ch.CurrentCard.Properties.HP)
                                    {
                                        KOAIPlayerBenchList.Add(ch);//Add this Pokemon to the Knockedoutlist
                                        this.State.AIPlayerDeck.BenchDict.Remove(ch.CurrentCard);//Remove from BenchDict b/c no event fired
                                    }
                                    playattacksound = true;
                                }
                            }
                            else
                            {
                                selectedBench = GetBenchCardHolder(Player.PLAYER, Convert.ToInt16(sc[0].Trim()), true);
                                //Now add the Damage to these
                                foreach (CardHolder ch in selectedBench)
                                {   //sc[1] is the '='
                                    ch.CurrentCard.State.CurrentDamage = ch.CurrentCard.State.CurrentDamage + Convert.ToInt16(sc[2].Trim());
                                    explainations.AppendLine("AddDamageToBenched: Applied " + sc[2].Trim() + " to " + ch.CurrentCard.Description);
                                    if (ch.CurrentCard.State.CurrentDamage >= ch.CurrentCard.Properties.HP)
                                    {
                                        KOPlayerBenchList.Add(ch);//Add this Pokemon to the Knockedoutlist
                                        this.State.Player1Deck.BenchDict.Remove(ch.CurrentCard);//Remove from BenchDict b/c no event fired
                                    }
                                    playattacksound = true;
                                }
                            }
                            #endregion AddDamageToBenched
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.MultiplyByDamageOnSelf)
                        {
                            #region MultiplyByDamageOnSelf (counts damage counters on self then multiplies by xml value. Then adds to attackDamage)
                            try
                            {
                                if (FlipNeeded == false || FlipResultPassed == true)
                                {
                                    //Count the number of damage counters Damage/10
                                    int DCounters = (chAttackingPokemon.CurrentCard.State.CurrentDamage / 10);
                                    int thisdamage = (DCounters * Convert.ToInt16(sc[0].Trim()));
                                    attackDamage = attackDamage + thisdamage;
                                    explainations.AppendLine("MultiplyByDamageOnSelf: " + thisdamage.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ProcessAttack Error: " + chAttackingPokemon.CurrentCard.Description + " has invalid value for " + 
                                    pair.First.ToString() + "\r\nThis is normally caused by incorrect CardDirections values in this card's xml.\r\nPlease visit the online documentation to learn the correct settings for " + 
                                    pair.First.ToString() + "\r\nOther Exceptions: " + ex.Message);
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.MinusAmountMultipliedByAttachedDamage)
                        {
                            #region MinusAmountMultipliedByAttachedDamage (Minus xml value damage for each damage counter on self(activepokemon))
                            try
                            {
                                if (FlipNeeded == false || FlipResultPassed == true)
                                {
                                    //sc[0] shoulds have a value or Convert will throw exception
                                    //Count the number of damage counters Damage/10
                                    int DCounters = (chAttackingPokemon.CurrentCard.State.CurrentDamage / 10);
                                    int thisdamage = (DCounters * Convert.ToInt16(sc[0].Trim()));
                                    if (thisdamage > 0)
                                    {
                                        attackDamage = attackDamage - thisdamage;
                                    }
                                    else
                                        thisdamage = 0;
                                    explainations.AppendLine("MultiplyByDamageOnSelf: " + thisdamage.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ProcessAttack Error: " + chAttackingPokemon.CurrentCard.Description + " has invalid value for " +
                                    pair.First.ToString() + "\r\nThis is normally caused by incorrect CardDirections values in this card's xml.\r\nPlease visit the online documentation to learn the correct settings for " +
                                    pair.First.ToString() + "\r\nOther Exceptions: " + ex.Message);
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.IfDamaged)
                        {
                            #region IfDamaged (If AttackingPokemon damaged)
                            if (chAttackingPokemon.CurrentCard.State.CurrentDamage > 0)
                            {
                                explainations.AppendLine("IfDamaged: Continuing attack; Attacking Pokemon Damage = " + chAttackingPokemon.CurrentCard.State.CurrentDamage.ToString());
                            }
                            else
                            {
                                //Condition not met, so stop this attack
                                explainations.AppendLine("IfDamaged: Can not continue this attack; Attacking Pokemon Damage = " + chAttackingPokemon.CurrentCard.State.CurrentDamage.ToString());
                                break;
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.IfDefendingDamaged)
                        {
                            #region IfDefendingDamaged (Is the DefendingPokemon damaged? if true, continue attack)
                            if (chDefendingPokemon.CurrentCard.State.CurrentDamage > 0)
                            {
                                explainations.AppendLine("IfDefendingDamaged: Continuing attack; Defending Pokemon Damage = " + chDefendingPokemon.CurrentCard.State.CurrentDamage.ToString());
                            }
                            else
                            {
                                //Condition not met, so stop this attack
                                explainations.AppendLine("IfDefendingDamaged: Can not continue this attack; Defending Pokemon Damage = " + chDefendingPokemon.CurrentCard.State.CurrentDamage.ToString());
                                break;
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.ChooseOpponPokemon)
                        {
                            #region ChooseOpponPokemon (If no args, get a card from the bench. Gets qty and arg[2] assigns to ChooseOpponPokemonList)
                            int qty = 0;

                            //Get a list
                            if (sc.Count == 0)
                            {
                                //No args, so just choose the highest damaged opponent
                                ChooseOpponPokemonList = GetBenchCardHolder(this.State.NextPlayer, 1, true);//take these/this pokemon and do next
                            }
                            else
                            {
                                if (sc.Count == 3)
                                {
                                    //Has args, get the first parameter (Qty)
                                    //Get the next parameter if it is greater than 1
                                    qty = Convert.ToInt16(sc[0].ToLower().Trim());

                                    if (sc[2].ToLower().Trim() == "damaged")
                                    {
                                        #region "damaged"
                                        //Get all of the requested bench in damaged ranked order
                                        ChooseOpponPokemonList = GetBenchCardHolder(this.State.NextPlayer, qty, true);
                                        if (ChooseOpponPokemonList.Count > 0)
                                        {
                                            //The list is ranked by damage so the first one should have dmage if any have damage
                                            if (ChooseOpponPokemonList[0].CurrentCard.State.CurrentDamage > 0)
                                            {
                                            }
                                            else
                                            {
                                                //The request was for "damaged" Pokemon and none found, try looking at defendingpokemon
                                                ChooseOpponPokemonList.Clear();//so none is used
                                                if (this.State.AIPlayerPokemon[0].CurrentCard.State.CurrentDamage > 0)
                                                    ChooseOpponPokemonList.Add(this.State.AIPlayerPokemon[0]);//Found so add
                                                else
                                                    break;//stop this attack
                                            }
                                        }
                                        else
                                        {
                                            //No Pokemon on Bench, so must choose defendingpokemon
                                            ChooseOpponPokemonList.Clear();//so none is used
                                            if (this.State.AIPlayerPokemon[0].CurrentCard.State.CurrentDamage > 0)
                                                ChooseOpponPokemonList.Add(this.State.AIPlayerPokemon[0]);//Found so add
                                            else
                                                break;//stop this attack
                                        }
                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }
                        if (pair.First == CardEventNames.SelectAllPokemonType)
                        {
                            #region SelectAllPokemonType
                            
                            string location = sc[0].ToLower().Trim();
                            PokemonTYPE desiredCardType = PokemonTYPE.Null;
                            //If there is more than just a location arg then there must be a requested Cardtype
                            if (sc.Count > 1)
                            {
                                desiredCardType = Ops.ConvertStringToPokemonTYPE(sc[2].Trim());
                            }
                            if (location == "self")
                            {
                                if (this.State.CurrentPlayer == Player.PLAYER)
                                {
                                    foreach (CardHolder ch in this.State.Player1Pokemon)
                                    {
                                        if (ch.HasItem)
                                        {
                                            if (desiredCardType != PokemonTYPE.Null)
                                            {
                                                if (ch.CurrentCard.Properties.PokemonType == desiredCardType)
                                                {
                                                    SelectedPokemonList.Add(ch);
                                                }
                                            }
                                            else
                                            {
                                                //Cardtype.Null so selected all of the Pokemon
                                                SelectedPokemonList.Add(ch);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //AIPlayer...
                                    foreach (CardHolder ch in this.State.AIPlayerPokemon)
                                    {
                                        if (ch.HasItem)
                                        {
                                            if (desiredCardType != PokemonTYPE.Null)
                                            {
                                                if (ch.CurrentCard.Properties.PokemonType == desiredCardType)
                                                {
                                                    SelectedPokemonList.Add(ch);
                                                }
                                            }
                                            else
                                            {
                                                //Cardtype.Null so selected all of the Pokemon
                                                SelectedPokemonList.Add(ch);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Not self so must be Defending side to select
                                if (this.State.CurrentPlayer == Player.PLAYER)
                                {
                                    foreach (CardHolder ch in this.State.Player1Pokemon)
                                    {
                                        if (ch.HasItem)
                                        {
                                            if (desiredCardType != PokemonTYPE.Null)
                                            {
                                                if (ch.CurrentCard.Properties.PokemonType == desiredCardType)
                                                {
                                                    SelectedPokemonList.Add(ch);
                                                }
                                                else
                                                {
                                                    //Cardtype.Null so selected all of the Pokemon
                                                    SelectedPokemonList.Add(ch);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //AIPlayer...
                                    foreach (CardHolder ch in this.State.AIPlayerPokemon)
                                    {
                                        if (ch.HasItem)
                                        {
                                            if (desiredCardType != PokemonTYPE.Null)
                                            {
                                                if (ch.CurrentCard.Properties.PokemonType == desiredCardType)
                                                {
                                                    SelectedPokemonList.Add(ch);
                                                }
                                                else
                                                {
                                                    //Cardtype.Null so selected all of the Pokemon
                                                    SelectedPokemonList.Add(ch);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            #endregion
                        }
                        if (pair.First == CardEventNames.RemoveDamageCounter)
                        {
                            #region RemoveDamageCounter
                            //Get the number of counters to remove
                            int iDpoints = 0;
                            if (sc.Count == 1)
                            {
                                //Is iArg a number or char?
                                if (Char.IsNumber(sc[0].ToLower().Trim(), 0))
                                {
                                    int iArg = Convert.ToInt16(sc[0].ToLower().Trim());
                                    if (iArg < 10)
                                    {
                                        //The user is using damage counter count (1 = 10)
                                        //Calculate the points to remove
                                        iDpoints = 10 * iArg;

                                    }
                                    else
                                    {
                                        //The user must be using damage points (10, 20, etc)
                                        iDpoints = iArg;
                                    }
                                }
                                else
                                {
                                    //sc[0].Trim() is Alpha! so might be 
                                    string sArg = sc[0].ToLower().Trim();
                                    if (sArg == "damagedone")
                                    {//"RemoveDamageCounter">DAMAGEDONE Beautifly Stage2 (19/130)
                                        //the number of damage counters equal to the damage you did to the Defending
                                        //Set some sort of flag here so after attack do this?
                                        okRemovefromSelf = false;//do later not here

                                    }
                                }
                            }

                            //Previous SelectedPokemonList actions??
                            if (SelectedPokemonList.Count > 0)
                            {
                                //There are selected Pokemon
                                //Remove the points from the Selected Pokemon
                                foreach (CardHolder selectedCH in SelectedPokemonList)
                                {
                                    selectedCH.CurrentCard.State.CurrentDamage = selectedCH.CurrentCard.State.CurrentDamage - iDpoints;
                                    explainations.AppendLine("RemoveDamageCounter: Removed " + iDpoints.ToString() + " from " + selectedCH.CurrentCard.Description);
                                    if (selectedCH.CurrentCard.State.CurrentDamage < 0)
                                        selectedCH.CurrentCard.State.CurrentDamage = 0;
                                }
                            }
                            else
                            {
                                //Assume to remove from ActivePokemon
                                if (okRemovefromSelf)
                                {
                                    chAttackingPokemon.CurrentCard.State.CurrentDamage = chAttackingPokemon.CurrentCard.State.CurrentDamage - iDpoints;
                                    explainations.AppendLine("RemoveDamageCounter: Removed " + iDpoints.ToString() + " from " + chAttackingPokemon.CurrentCard.Description);
                                    if (chAttackingPokemon.CurrentCard.State.CurrentDamage < 0)
                                        chAttackingPokemon.CurrentCard.State.CurrentDamage = 0;
                                }
                                else
                                {
                                    //Do rest after computing of resistance etc.
                                }

                            }
                            #endregion
                        }
                        if (pair.First == PPokemon.Cards.CardEventNames.SearchDeckForEnergy)
                        {
                            #region SearchDeckForEnergy (GetEnergyCardFromFaceDownPile and Adds to EnergyList)
                            PokemonTYPE DesiredPokeType = Ops.ConvertStringToPokemonTYPE(sc[0].Trim());
                            if (DesiredPokeType == PokemonTYPE.Null)
                            {
                                //Assign the DesiredPokeType the type of the ActivePokemon to help find Energy for it
                                DesiredPokeType = chAttackingPokemon.CurrentCard.Properties.PokemonType;
                            }
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                //Get the Energy Card from the Deck.FaceDownlist
                                EnergyList = this.State.GetEnergyCardFromFaceDownPile(Player.PLAYER, DesiredPokeType, 1);
                            }
                            else
                            {
                                //Get the Energy Card from the Deck.FaceDownlist
                                EnergyList = this.State.GetEnergyCardFromFaceDownPile(Player.AIPLAYER, DesiredPokeType, 1);
                            }

                            //Evaluate sc[2]
                            if (sc[2].ToLower().Trim() == "hand")
                            {
                                //Place the Energy Cards directly in to the hand
                                foreach (Card card in EnergyList)
                                {
                                    if (this.State.CurrentPlayer == Player.PLAYER)
                                    {
                                        this.State.Player1Deck.HandList.Add(card);
                                    }
                                    else
                                    {
                                        this.State.AIPlayerDeck.HandList.Add(card);
                                    }
                                    explainations.AppendLine("SearchDeckForEnergy [Placed in Hand]: " + card.Description);
                                }
                                
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.AttachCardToAPokemon)
                        {
                            #region AttachCardToAPokemon (only attempts to add all the cards in Lists to activepokemon)

                            //Check the Args
                            if (sc[0].ToLower().Trim() == "energy")
                            {
                                //Request is for EnergyCard so check to see if previous commands populated EnergyList
                                if (EnergyList.Count > 0)
                                {
                                    foreach (Card ecard in EnergyList)
                                    {
                                        if (this.State.CurrentPlayer == Player.PLAYER)
                                        {
                                            this.State.Player1Pokemon[0].CurrentCard.State.AttachedCards.Add(ecard);//ActivePokemon
                                            explainations.AppendLine("AttachCardToAPokemon: Attached " + ecard.Description + " to " + this.State.Player1Pokemon[0].CurrentCard.Description);
                                        }
                                        else
                                        {
                                            this.State.AIPlayerPokemon[0].CurrentCard.State.AttachedCards.Add(ecard);//ActivePokemon
                                            explainations.AppendLine("AttachCardToAPokemon: Attached " + ecard.Description + " to " + this.State.Player1Pokemon[0].CurrentCard.Description);
                                        }
                                    }
                                    EnergyList.Clear();
                                }
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.DrawACard)
                        {
                            #region DrawACard (Deck.TakeCard(false) and adds to CardsFromDeckList. Remember to remove from CardsFromDeckList on anyother events)
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                if (sc.Count == 0)
                                {
                                    //Just draw one card
                                    CardsFromDeckList.Add(this.State.Player1Deck.TakeCard(false));
                                }
                                else
                                {
                                    int timestodraw = Convert.ToInt16(sc[0].Trim());
                                    for (int i = 0; i < timestodraw; i++)
                                    {
                                        CardsFromDeckList.Add(this.State.Player1Deck.TakeCard(false));
                                    }
                                }
                            }
                            else
                            {
                                //AIPlayerDeck
                                if (sc.Count == 0)
                                {
                                    //Just draw one card
                                    CardsFromDeckList.Add(this.State.AIPlayerDeck.TakeCard(false));
                                }
                                else
                                {
                                    int timestodraw = Convert.ToInt16(sc[0].Trim());
                                    for (int i = 0; i < timestodraw; i++)
                                    {
                                        CardsFromDeckList.Add(this.State.AIPlayerDeck.TakeCard(false));
                                    }
                                }
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.OpponentChooseCardsFromDrawn)
                        {
                            #region OpponentChooseCardsFromDrawn (opponent chooses sc[0] of those cards. Put those cards into hand and put other cards back in deck)
                            if (CardsFromDeckList.Count > 0)
                            {
                                //Previous DrawACard etc. ...
                                if (sc.Count > 0)
                                {
                                    int choosecount = Convert.ToInt16(sc[0].Trim());
                                    if (CardsFromDeckList.Count >= choosecount)
                                    {
                                        for (int i = 0; i < choosecount; i++)
                                        {
                                            Card c = new Card(Cardtype.Null);
                                            //Opponent prefers choosing cards with lowest HP first
                                            if (this.State.CurrentPlayer == Player.PLAYER)
                                            {
                                                c = Ops.GetLowestHPCard(CardsFromDeckList);
                                                this.State.Player1Deck.HandList.Add(c);
                                                CardsFromDeckList.Remove(c);
                                                explainations.AppendLine("OpponentChooseCardsFromDrawn: " + c.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                            }
                                            else
                                            {
                                                //AIPlayer
                                                c = Ops.GetLowestHPCard(CardsFromDeckList);
                                                this.State.AIPlayerDeck.HandList.Add(c);
                                                CardsFromDeckList.Remove(c);
                                                explainations.AppendLine("OpponentChooseCardsFromDrawn: " + c.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                            }
                                        }
                                        //Now the selecting is over, If remaining from CardsFromDeckList, place them back into deck
                                        if (CardsFromDeckList.Count > 0)
                                        {
                                            foreach (Card c in CardsFromDeckList)
                                            {
                                                if (this.State.CurrentPlayer == Player.PLAYER)
                                                {
                                                    this.State.Player1Deck.FaceDownList.Add(c);
                                                    CardsFromDeckList.Remove(c);
                                                }
                                                else
                                                {
                                                    //AIPlayer
                                                    this.State.AIPlayerDeck.FaceDownList.Add(c);
                                                    CardsFromDeckList.Remove(c);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        explainations.AppendLine("OpponentChooseCardsFromDrawn error: CardsFromDeckList.Count < choosecount");
                                    }
                                }
                            }
                            if (EnergyList.Count > 0)
                            {
                                //Previous SearchDeckForEnergy etc. ...
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.ChooseCardsFromDrawn)
                        {
                            #region ChooseCardsFromDrawn (chooses sc[0] of those cards. Put those cards into hand and put other cards back in deck)
                            if (CardsFromDeckList.Count > 0)
                            {
                                //Previous DrawACard etc. ...
                                if (sc.Count > 0)
                                {
                                    int choosecount = Convert.ToInt16(sc[0].Trim());
                                    if (CardsFromDeckList.Count >= choosecount)
                                    {
                                        for (int i = 0; i < choosecount; i++)
                                        {
                                            Card c = new Card(Cardtype.Null);
                                            //Opponent prefers choosing cards with lowest HP first
                                            if (this.State.CurrentPlayer == Player.PLAYER)
                                            {
                                                c = Ops.GetHighestHPCard(CardsFromDeckList);
                                                this.State.Player1Deck.HandList.Add(c);
                                                CardsFromDeckList.Remove(c);
                                                explainations.AppendLine("ChooseCardsFromDrawn: " + c.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                            }
                                            else
                                            {
                                                //AIPlayer
                                                c = Ops.GetHighestHPCard(CardsFromDeckList);
                                                this.State.AIPlayerDeck.HandList.Add(c);
                                                CardsFromDeckList.Remove(c);
                                                explainations.AppendLine("ChooseCardsFromDrawn: " + c.Description + " added to hand.");
                                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                            }
                                        }
                                        //Now the selecting is over, If remaining from CardsFromDeckList, place them back into deck
                                        if (CardsFromDeckList.Count > 0)
                                        {
                                            foreach (Card c in CardsFromDeckList)
                                            {
                                                if (this.State.CurrentPlayer == Player.PLAYER)
                                                {
                                                    this.State.Player1Deck.FaceDownList.Add(c);
                                                }
                                                else
                                                {
                                                    //AIPlayer
                                                    this.State.AIPlayerDeck.FaceDownList.Add(c);
                                                }
                                            }
                                            CardsFromDeckList.Clear();
                                        }
                                    }
                                    else
                                    {
                                        explainations.AppendLine("ChooseCardsFromDrawn error: CardsFromDeckList.Count < choosecount");
                                    }
                                }
                            }
                            if (EnergyList.Count > 0)
                            {
                                //Previous SearchDeckForEnergy etc. ...
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.SwitchWithBenched)
                        {
                            #region SwitchWithBenched (Switches the ActivePokemon with Highest HP on Bench)
                            List<CardHolder> BenchCHList = new List<CardHolder>();
                            CardHolder BestBenchCH = new CardHolder();
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                BenchCHList = GetBenchCardHolder(Player.PLAYER, 5, true);
                                if (BenchCHList.Count > 0)
                                {
                                    BestBenchCH = BenchCHList[0];
                                    //Search for highest HP on Bench
                                    foreach (CardHolder chitem in BenchCHList)
                                    {
                                        if (chitem.HasItem)
                                        {
                                            if (chitem.CurrentCard.Properties.HP > BestBenchCH.CurrentCard.Properties.HP)
                                            {
                                                BestBenchCH = chitem;
                                            }
                                        }
                                    }

                                    //Do Switch
                                    string oldActiveDescription = chAttackingPokemon.CurrentCard.Description;
                                    CardState oldActiveState = new CardState();
                                    oldActiveState = chAttackingPokemon.CurrentCard.State.Clone();
                                    chAttackingPokemon.PlaceCard(BestBenchCH.CurrentCard);
                                    chAttackingPokemon.TurnCardFaceUp();
                                    chAttackingPokemon.CurrentCard.State.ActivePokemon = true;

                                    BestBenchCH.PlaceCard(this.State.Player1Deck.GetCardByDescription(oldActiveDescription));
                                    BestBenchCH.TurnCardFaceUp();
                                    BestBenchCH.CurrentCard.State = oldActiveState;

                                    explainations.AppendLine("SwitchWithBenched: " + oldActiveDescription + " switched with " + chAttackingPokemon.CurrentCard.Description);
                                    this.OnProcessHook(GameHooks.BenchPokemonChoosen, Player.PLAYER);
                                    this.OnProcessHook(GameHooks.ActivePokemonChoosen, Player.PLAYER);
                                }
                                else
                                {
                                    explainations.AppendLine("SwitchWithBenched: Can not switch with Bench because none on Bench.");
                                }
                            }
                            else
                            {
                                //AIPlayer...
                                BenchCHList = GetBenchCardHolder(Player.AIPLAYER, 5, true);
                                if (BenchCHList.Count > 0)
                                {
                                    BestBenchCH = BenchCHList[0];
                                    //Search for highest HP on Bench
                                    foreach (CardHolder chitem in BenchCHList)
                                    {
                                        if (chitem.HasItem)
                                        {
                                            if (chitem.CurrentCard.Properties.HP > BestBenchCH.CurrentCard.Properties.HP)
                                            {
                                                BestBenchCH = chitem;
                                            }
                                        }
                                    }

                                    //Do Switch
                                    string oldActiveDescription = chAttackingPokemon.CurrentCard.Description;
                                    CardState oldActiveState = new CardState();
                                    oldActiveState = chAttackingPokemon.CurrentCard.State.Clone();
                                    chAttackingPokemon.PlaceCard(BestBenchCH.CurrentCard);
                                    chAttackingPokemon.TurnCardFaceUp();
                                    chAttackingPokemon.CurrentCard.State.ActivePokemon = true;

                                    BestBenchCH.PlaceCard(this.State.AIPlayerDeck.GetCardByDescription(oldActiveDescription));
                                    BestBenchCH.TurnCardFaceUp();
                                    BestBenchCH.CurrentCard.State = oldActiveState;

                                    explainations.AppendLine("SwitchWithBenched: " + oldActiveDescription + " switched with " + chAttackingPokemon.CurrentCard.Description);
                                    this.OnProcessHook(GameHooks.BenchPokemonChoosen, Player.AIPLAYER);
                                    this.OnProcessHook(GameHooks.ActivePokemonChoosen, Player.AIPLAYER);
                                }
                                else
                                {
                                    explainations.AppendLine("SwitchWithBenched: Can not switch with Bench because none on Bench.");
                                }
                            }
                            #endregion
                        }
                        if (pair.First == CardEventNames.IfFlipHeads)
                        {
                            #region IfFlipHeads
                            int damagepoints = 0;
                            //Is sc[0] a number?  or it is a character command
                            if (System.Char.IsNumber(sc[0].ToLower().Trim(), 0))
                            {
                                damagepoints = Convert.ToInt16(sc[0].ToLower().Trim());//create damage points
                                //It is a number, so what is the rest of the command?
                                if (sc[1].ToLower().Trim() == "x")
                                {
                                    //Multiply what by sc[0] number??
                                    if (FlipStorageDict.Second > 0)
                                    {
                                        //Previous Flip counts
                                        attackDamage = attackDamage + (damagepoints * FlipStorageDict.Second);
                                        explainations.AppendLine("IfFlipHeads: Heads rolled " + FlipStorageDict.Second.ToString() + " * " + damagepoints.ToString());
                                    }
                                }
                            }

                            //
                            #endregion
                        }
                        if (pair.First == CardEventNames.IfFlipTails)
                        {
                            #region IfFlipTails
                            int damagepoints = 0;
                            //Is sc[0] a number?  or it is a character command
                            if (System.Char.IsNumber(sc[0].ToLower().Trim(), 0))
                            {
                                damagepoints = Convert.ToInt16(sc[0].ToLower().Trim());//create damage points
                                //It is a number, so what is the rest of the command?
                                if (sc[1].ToLower().Trim() == "x")
                                {
                                    //Multiply what by sc[0] number??
                                    if (FlipStorageDict.Second > 0)
                                    {
                                        //Previous Flip counts
                                        attackDamage = attackDamage + (damagepoints * FlipStorageDict.Second);
                                        explainations.AppendLine("IfFlipTails: Tails rolled " + FlipStorageDict.Second.ToString() + " * " + damagepoints.ToString());
                                    }
                                }
                            }

                            //
                            #endregion
                        }
                        if (pair.First == CardEventNames.SearchDeckForBasicOrEvolution || pair.First == CardEventNames.SearchDeckForPokemon)
                        {//If the Active has over 50% damage and is a basic, search for it's Evolution dam/hp
                            #region SearchDeckForBasicOrEvolution
                            StringBuilder ToBenchLog = new StringBuilder();
                            Card nextEvolveCard = new Card(Cardtype.Null);
                            bool IsCardinDeck = false;

                            //Search for the ActivePokemon's next evolution
                            if (this.State.CurrentPlayer == Player.PLAYER)
                            {
                                nextEvolveCard = this.State.Player1Deck.GetEvolveCardNeeded(chAttackingPokemon, this.State.PokeDex);
                                //Search the Deck to see if this card is available
                                foreach (Card card in this.State.Player1Deck.FaceDownList)
                                {
                                    if (card.Description == nextEvolveCard.Description)
                                    {
                                        //Found the card needed in the deck
                                        IsCardinDeck = true;
                                        break;
                                    }
                                }

                                if (IsCardinDeck)
                                {
                                    //Place the card in hand
                                    this.State.Player1Deck.FaceDownList.Remove(nextEvolveCard);
                                    this.State.Player1Deck.HandList.Add(nextEvolveCard);
                                    explainations.AppendLine("SearchDeckForBasicOrEvolution: " + nextEvolveCard.Description + " added to hand.");
                                    this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                                }
                            }
                            else
                            {
                                //AIPlayer
                                nextEvolveCard = this.State.AIPlayerDeck.GetEvolveCardNeeded(chAttackingPokemon, this.State.PokeDex);
                                //Search the Deck to see if this card is available
                                foreach (Card card in this.State.AIPlayerDeck.FaceDownList)
                                {
                                    if (card.Description == nextEvolveCard.Description)
                                    {
                                        //Found the card needed in the deck
                                        IsCardinDeck = true;
                                        break;
                                    }
                                }

                                if (IsCardinDeck)
                                {
                                    //Place the card in hand
                                    this.State.AIPlayerDeck.FaceDownList.Remove(nextEvolveCard);
                                    this.State.AIPlayerDeck.HandList.Add(nextEvolveCard);
                                    explainations.AppendLine("SearchDeckForBasicOrEvolution: " + nextEvolveCard.Description + " added to hand.");
                                    this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                                }
                            }

                            //If no Evolution Card was found in the deck, then find a basic
                            if (!IsCardinDeck)
                            {
                                #region Get BestBasic
                                if (this.State.CurrentPlayer == Player.PLAYER)
                                {
                                    //Get the Best Basic Card from the Deck.FaceDownlist
                                    Card bestBasic = new Card(Cardtype.Basic);
                                    Card firstCard = new Card(Cardtype.Null);
                                    bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.PLAYER);

                                    if (bestBasic.Description != "")
                                    {
                                        //If there are no arguments on this attack direction then just add the Basic to Bench
                                        if (sc.Count == 0)
                                        {
                                            this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                            explainations.Append(ToBenchLog.ToString());
                                        }
                                        else
                                        {
                                            //There are some directions so lets do them

                                            //is sc[0].Trim() a Number or boolean?
                                            if (Char.IsNumber(sc[0].ToLower().Trim(), 0))
                                            {
                                                #region IsNumber
                                                //There should only be one PokemonTYPE in args
                                                if (bestBasic.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[0].Trim()))
                                                {
                                                    this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                                    explainations.Append(ToBenchLog.ToString());
                                                }
                                                else
                                                {
                                                    //Just get the first card thast works
                                                    bestBasic = this.State.GetBestBasicCardFromFaceDownPile(this.State.CurrentPlayer, Ops.ConvertStringToPokemonTYPE(sc[0].Trim()));

                                                    if (bestBasic.Description != "")
                                                    {
                                                        this.PlaceCardToBenchForAttack(bestBasic, Player.PLAYER, out ToBenchLog);
                                                        explainations.Append(ToBenchLog.ToString());
                                                    }
                                                    else
                                                        explainations.AppendLine("No Basics found.");
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region !IsNumber
                                                if (sc[0].ToLower().Trim() == "true")
                                                {
                                                    //true = stop at first Pokemon
                                                    //Search the Deck to see if this card is available
                                                    foreach (Card card in this.State.AIPlayerDeck.FaceDownList)
                                                    {
                                                        if (card.Properties.CardType == Cardtype.Basic || card.Properties.CardType == Cardtype.Stage1 ||
                                                            card.Properties.CardType == Cardtype.Stage2 || card.Properties.CardType == Cardtype.LevelUp)
                                                        {
                                                            //Found the card needed in the deck
                                                            firstCard = card;
                                                            break;
                                                        }
                                                    }

                                                    if (bestBasic.Properties.CardType != Cardtype.Null)
                                                    {
                                                        //Place the card in hand
                                                        this.State.Player1Deck.FaceDownList.Remove(firstCard);
                                                        this.State.Player1Deck.HandList.Add(firstCard);
                                                        explainations.AppendLine("SearchDeckForPokemon: " + firstCard.Description + " added to hand.");
                                                        this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);//What about this attack for AIPayer???
                                                    }
                                                }
                                                else
                                                {

                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //AIPLAYER
                                    //Get the Best Basic Card from the Deck.FaceDownlist
                                    Card bestBasic = new Card(Cardtype.Basic);//DEBUG: FIND OUT IF THIS IS DOING WHAT THE CARD WANTS!!!
                                    bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.AIPLAYER);
                                    if (bestBasic.Description != "")
                                    {
                                        this.PlaceCardToBenchForAttack(bestBasic, Player.AIPLAYER, out ToBenchLog);
                                        explainations.Append(ToBenchLog.ToString());
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        
                        //Not working yet....
                        CheckAttachedTrainerCards(chDefendingPokemon, attackDamage);
                    }
                    ////////////////////////////////////\/\/\/\/ALWAYS DO...\/\/\/\////////////////////////////////////////////////////////////
                    #region Evaluate CardsFromDeckList, if still there add to HandList
                    if (CardsFromDeckList.Count > 0)
                    {
                        if (this.State.CurrentPlayer == Player.PLAYER)
                        {
                            foreach (Card card in CardsFromDeckList)
                            {
                                this.State.Player1Deck.HandList.Add(card);
                                explainations.AppendLine("Added drawn card " + card.Description + " to hand.");
                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                            }
                        }
                        else
                        {
                            foreach (Card card in CardsFromDeckList)
                            {
                                this.State.AIPlayerDeck.HandList.Add(card);
                                explainations.AppendLine("Added drawn card " + card.Description + " to hand.");
                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                            }
                        }
                    }
                    #endregion
                    
                    #region Computes Weakness and Resistance here...
                    if (attackDamage > 0)
                    {
                        //Is this AttackType DefendingPokemon's Weakness? if yes double the damage
                        if (chDefendingPokemon.CurrentCard.Properties.Weakness.First == chAttackingPokemon.CurrentCard.Properties.PokemonType)
                        {
                            attackDamage = activeAttack.Damage + activeAttack.Damage;//Double the damage
                            explainations.AppendLine("Defending Pokemon has " + chDefendingPokemon.CurrentCard.Properties.Weakness.First.ToString() + " weakness. Doubled the damage!");
                        }
                        //Is this AttackType DefendingPokemon's Resistance? if yes, subtract 30
                        if (chDefendingPokemon.CurrentCard.Properties.Resistance.First == chAttackingPokemon.CurrentCard.Properties.PokemonType)
                        {
                            attackDamage = activeAttack.Damage - 30;//Subtract 30
                            if (attackDamage < 0)
                                attackDamage = 0;

                            explainations.AppendLine("Defending Pokemon has " + chDefendingPokemon.CurrentCard.Properties.Resistance.First.ToString() + " resistance. Subtracted 30!");
                        }
                    }
                    #endregion
                    
                    //OK add the attack damage
                    if (okRemovefromSelf)
                    {
                        //normally,do not add from self
                        chDefendingPokemon.CurrentCard.State.CurrentDamage = chDefendingPokemon.CurrentCard.State.CurrentDamage + attackDamage;
                    }
                    else
                    {
                        //Remove from chDefendingPokemon
                        chDefendingPokemon.CurrentCard.State.CurrentDamage = chDefendingPokemon.CurrentCard.State.CurrentDamage + attackDamage;
                        //Different removal from self/chAttackingPokemon
                        chAttackingPokemon.CurrentCard.State.CurrentDamage = chAttackingPokemon.CurrentCard.State.CurrentDamage - attackDamage;
                        explainations.AppendLine("RemoveDamageCounter: Removed " + attackDamage.ToString() + " from " + chAttackingPokemon.CurrentCard.Description);
                        if (chAttackingPokemon.CurrentCard.State.CurrentDamage < 0)
                            chAttackingPokemon.CurrentCard.State.CurrentDamage = 0;
                    }
                                    
                    
                    
                    //If damage was done, play Attack sound! and send the EventHook
                    if (attackDamage > 0 || playattacksound == true)
                    {
                        PlayAttackSound();
                        this.OnProcessHook(GameHooks.DamageDone, this.State.NextPlayer);
                    }
                    //Was it an attack or Trainer, etc.?
                    if (!nonattack)
                        explainations.AppendLine(activeAttack.Title + " Damage done: " + attackDamage.ToString());
                    else
                        explainations.AppendLine(activeAttack.Title + " actions have completed.");

                    //Is chDefendingPokemon knocked out??
                    if (chDefendingPokemon.CurrentCard.State.CurrentDamage >= chDefendingPokemon.CurrentCard.Properties.HP)
                    {//IS  THIS CARD DESCRIPT ""??????????
                        MessageBox.Show(chDefendingPokemon.CurrentCard.Properties.Pokemon.Name + " is Knocked Out!!", "DefendingPokemon Knocked Out!!");

 //                       if (this.State.NextPlayer != Player.AIPLAYER)
 //                           MessageBox.Show("DEBUG: STOP HERE THIS MAY BE BUG");
                        this.KnockOut(chDefendingPokemon, this.State.NextPlayer);
                        this.OnProcessHook(GameHooks.PokemonKnockedOut, this.State.NextPlayer);
                    }
                    //Is chAttackingPokemon knocked out?? (by other conditions met during this attack)
                    if (chAttackingPokemon.CurrentCard.State.CurrentDamage >= chAttackingPokemon.CurrentCard.Properties.HP)
                    {
                        MessageBox.Show(chAttackingPokemon.CurrentCard.Properties.Pokemon.Name + " is Knocked Out by Conditions met during this attack!!", "AttackingPokemon Knocked Out!!");
                        this.KnockOut(chAttackingPokemon, this.State.CurrentPlayer);
                        this.OnProcessHook(GameHooks.PokemonKnockedOut, this.State.CurrentPlayer);
                    }
                    //Process all KOPlayerBenchList
                    foreach (CardHolder chitem in KOPlayerBenchList)
                    {
                        if (chitem.CurrentCard.State.CurrentDamage >= chitem.CurrentCard.Properties.HP)
                        {
                            MessageBox.Show(chitem.CurrentCard.Properties.Pokemon.Name + " is Knocked Out by Conditions met during this attack!!", "Bench Pokemon Knocked Out!!");
                            this.KnockOut(chitem, Player.PLAYER);
                            this.OnProcessHook(GameHooks.PokemonKnockedOut, Player.PLAYER);//This was prompting to replace the ActivePokemon
                        }
                    }
                    //Process all KOAIPlayerBenchList
                    foreach (CardHolder chitem in KOAIPlayerBenchList)
                    {
                        if (chitem.CurrentCard.State.CurrentDamage >= chitem.CurrentCard.Properties.HP)
                        {
                            //
                            MessageBox.Show(chitem.CurrentCard.Properties.Pokemon.Name + " is Knocked Out by Conditions met during this attack!!", "Bench Pokemon Knocked Out!!");
                            this.KnockOut(chitem, Player.AIPLAYER);
                            this.OnProcessHook(GameHooks.PokemonKnockedOut, Player.AIPLAYER);//This was prompting to replace the ActivePokemon
                        }
                    }
                    ////////////////////////////////////\/\/\/\/\ALWAYS DO.../\/\/\/\///////////////////////////////////////////////////////////

                }
            }
            return explainations;
        }

        /// <summary>
        /// Clear currentCondition and Damage from passed cardholder, then add to whospokemon DiscardList
        /// </summary>
        /// <param name="activepokemon">CardHolder</param>
        /// <param name="whospokemon">Player</param>
        public void KnockOut(CardHolder activepokemon, Player whospokemon)
        {
            //Clear currentCondition and Damage
            activepokemon.DiscardPrep();
            //What player's pieces? Add cards to the discardlist
            if (whospokemon == Player.AIPLAYER)
                this.State.AIPlayerDeck.DiscardList.AddRange(activepokemon.CurrentCard.State.AttachedCards.GetRange(0, activepokemon.CurrentCard.State.AttachedCards.Count));
            else
                this.State.Player1Deck.DiscardList.AddRange(activepokemon.CurrentCard.State.AttachedCards.GetRange(0, activepokemon.CurrentCard.State.AttachedCards.Count));

            activepokemon.RemoveItem();
        }

        public void DoSpecialCondition(CardHolder activepokemon, string conditionName)
        {
            activepokemon.CurrentCard.State.CurrentCondition = Ops.ConvertStringToSpecialConditions(conditionName);
            this.OnProcessHook(GameHooks.SpecialConditionEnabled, activepokemon);
        }

        /// <summary>
        /// Figure out damage effects of Trainer cards and Energy cards on the Defending Pokemon
        /// (like Metal Energy, or Buffer Piece)
        /// </summary>
        /// <param name="chDefendingPokemon"></param>
        /// <param name="currentdamge"></param>
        /// <returns></returns>
        public int CheckAttachedTrainerCards(CardHolder chDefendingPokemon, int currentdamge)
        {

            return currentdamge;
        }

        /// <summary>
        /// Get all of the CardHolders that have In-Play Poke-Power/Bodies
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>List(CardHolder)</returns>
        private List<CardHolder> GetPokePowersDict(Player player)
        {
            List<CardHolder> PokePower = new List<CardHolder>();

            if (player == Player.PLAYER)
            {
                foreach (CardHolder ch in this.State.Player1Pokemon)
                {
                    if (ch.HasItem)
                    {
                        if (ch.CurrentCard.Properties.PokePower.Events.Count > 0)
                        {
                            //There is a PokePower in this cardholder so add this
                            PokePower.Add(ch);
                        }
                    }
                } 
            }
            else
            {
                foreach (CardHolder ch in this.State.AIPlayerPokemon)
                {
                    if (ch.HasItem)
                    {
                        if (ch.CurrentCard.Properties.PokePower.Events.Count > 0)
                        {
                            //There is a PokePower in this cardholder so add this
                            PokePower.Add(ch);
                        }
                    }
                } 
            }
            return PokePower;
        }

        private void ProcessCardEvents(StringBuilder log, List<Wintellect.PowerCollections.Pair<CardEventNames, string>> eventsteps)
        {
            bool FlipNeeded = false;
            bool FlipResultPassed = false;

            CardHolder AttackingActivePokemon = new CardHolder();
            //Figure out the Attacking pokemon
            AttackingActivePokemon = this.State.GetActivePokemonCardHolder(this.State.CurrentPlayer);

            //First is the CardEventNames
            //Second is the args string
            foreach (Wintellect.PowerCollections.Pair<CardEventNames, string> pair in eventsteps)
            {
                //Read all the arguments
                StringCollection sc = new StringCollection();
                sc = Ops.ReadCardEventNamesArgs(pair.Second);

                if (pair.First != CardEventNames.None)
                {
                    #region FlipACoin
                    if (pair.First == CardEventNames.FlipACoin)
                    {
                        FlipNeeded = true;
                        Coin cResult = this.FlipACoin();//Flip a coin
                        log.AppendLine("Coin flip result:" + cResult.ToString());
                        //Evaluate if the flip enables the attack
                        if (cResult == Ops.ConvertStringToCoin(sc[0]))
                        {
                            FlipResultPassed = true;
                            //now read each arg starting after desired result
                            for (int i = 1; i < sc.Count; i++)
                            {
                                if (sc[i].ToLower() == "=")
                                {
                                    //Nothing goto next value
                                }
                                if (sc[i].ToLower() == "next")
                                {
                                    //Wants to read next Directions so break out of this loop
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                }
                if (pair.First == PPokemon.Cards.CardEventNames.AddSpecialCToAttack)
                {
                    #region AddSpecialCToAttack
                    if (sc.Count > 0)
                    {
                        if (FlipNeeded == false || FlipResultPassed == true)
                        {
                            //Should only be one specialcondition listed in arg
                            //The current player is the attacker pokemon to this power needs action on it
                            DoSpecialCondition(AttackingActivePokemon, sc[0]);
                            log.AppendLine("AddSpecialCToAttack: " + pair.Second.ToString());
                        }
                    }
                    #endregion
                }
                if (pair.First == PPokemon.Cards.CardEventNames.SearchDeckForBasic)
                {
                    #region SearchDeckForBasic
                    if (this.State.CurrentPlayer == Player.PLAYER)
                    {
                        //Get the Best Basic Card from the Deck.FaceDownlist
                        Card bestBasic = new Card(Cardtype.Basic);
                        bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.PLAYER);

                        //If there are no arguments on this attack direction then just add the Basic to hand
                        if (sc.Count == 0)
                        {
                            this.State.Player1Deck.HandList.Add(bestBasic);
                            log.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                            this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                        }
                        else
                        {
                            //There are some directions so lets do them
                            //There should only be one PokemonTYPE in args
                            if (bestBasic.Properties.PokemonType == Ops.ConvertStringToPokemonTYPE(sc[0].Trim()))
                            {
                                this.State.Player1Deck.HandList.Add(bestBasic);
                                log.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                            }
                            else
                            {
                                //Just get the first card thast works
                                bestBasic = this.State.GetBestBasicCardFromFaceDownPile(this.State.CurrentPlayer, Ops.ConvertStringToPokemonTYPE(sc[0].Trim()));
                                this.State.Player1Deck.HandList.Add(bestBasic);
                                log.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                                this.OnProcessHook(GameHooks.PlayHandChanged, Player.PLAYER);
                            }
                        }
                    }
                    else
                    {
                        //Get the Best Basic Card from the Deck.FaceDownlist
                        Card bestBasic = new Card(Cardtype.Basic);
                        bestBasic = this.State.GetBestBasicCardFromFaceDownPile(Player.AIPLAYER);
                        this.State.AIPlayerDeck.HandList.Add(bestBasic);
                        log.AppendLine("SearchDeckForBasic: " + bestBasic.Description + " added to hand.");
                        this.OnProcessHook(GameHooks.PlayHandChanged, Player.AIPLAYER);
                    }
                    #endregion
                }

                if (pair.First == CardEventNames.None)
                {
                }
            }
        }

        /// <summary>
        /// Execute all of the in-play pokepowers/bodies
        /// CurrentCard.Properties.PokePower.Events[0].First must match the  GameHooks
        /// </summary>
        /// <param name="player"></param>
        /// <param name="hooktriggered"></param>
        /// <returns></returns>
        public StringBuilder DoPokePowers(Player player, GameHooks hooktriggered, out bool didpower)
        {
            bool DidPowers_Bodies = false;
            StringBuilder explainations = new StringBuilder();//explain what happened
            List<CardHolder> PowerList = GetPokePowersDict(player);

            //Iterate thru all of the Poke-Powers in play
            foreach (CardHolder ch in PowerList)
            {
                #region GameHooks.DamageDone
                if (hooktriggered == GameHooks.DamageDone)
                {
                    if (ch.HasItem)
                    {
                        if (ch.CurrentCard.Properties.PokePower.Events[0].First == CardEventNames.IfDamaged)
                        {
                            ProcessCardEvents(explainations, ch.CurrentCard.Properties.PokePower.Events);
                            DidPowers_Bodies = true;
                        }
                    }
                }
                #endregion
            }

            didpower = DidPowers_Bodies;
            return explainations;
        }

        /// <summary>
        /// Place a card to the Player or AIplayer's bench.
        /// Does remove any cards from anylists or dictionaries
        /// Fires:PlayHandChanged, BenchPokemonChoosen for GamePlay.Player.PLAYER
        /// </summary>
        /// <param name="card">Card to place</param>
        /// <param name="activeplayer">GamePlay.Player</param>
        private bool PlaceCardToBenchForAttack(Card card, GamePlay.Player activeplayer, out StringBuilder log)
        {
            bool placedcard = false;
            StringBuilder thislog = new StringBuilder();
            if (activeplayer == PPokemon.GamePlay.Player.PLAYER)
            {
                #region PLAYER
                if (!this.State.Player1Pokemon[1].HasItem)
                {
                    this.State.Player1Pokemon[1].PlaceCard(card);
                    this.State.Player1Pokemon[1].TurnCardFaceUp();
                    this.State.Player1Deck.BenchDict.Add(this.State.Player1Pokemon[1].CurrentCard);
                    thislog.AppendLine("Added " + this.State.Player1Pokemon[1].CurrentCard.Description + " to bench.");
                    placedcard = true;
                }
                else if (!this.State.Player1Pokemon[2].HasItem)
                {
                    this.State.Player1Pokemon[2].PlaceCard(card);
                    this.State.Player1Pokemon[2].TurnCardFaceUp();
                    this.State.Player1Deck.BenchDict.Add(this.State.Player1Pokemon[2].CurrentCard);
                    thislog.AppendLine("Added " + this.State.Player1Pokemon[2].CurrentCard.Description + " to bench.");
                    placedcard = true;
                }
                else if (!this.State.Player1Pokemon[3].HasItem)
                {
                    this.State.Player1Pokemon[3].PlaceCard(card);
                    this.State.Player1Pokemon[3].TurnCardFaceUp();
                    this.State.Player1Deck.BenchDict.Add(this.State.Player1Pokemon[3].CurrentCard);
                    thislog.AppendLine("Added " + this.State.Player1Pokemon[3].CurrentCard.Description + " to bench.");
                    placedcard = true;
                }
                else if (!this.State.Player1Pokemon[4].HasItem)
                {
                    this.State.Player1Pokemon[4].PlaceCard(card);
                    this.State.Player1Pokemon[4].TurnCardFaceUp();
                    this.State.Player1Deck.BenchDict.Add(this.State.Player1Pokemon[4].CurrentCard);
                    thislog.AppendLine("Added " + this.State.Player1Pokemon[4].CurrentCard.Description + " to bench.");
                    placedcard = true;
                }
                else if (!this.State.Player1Pokemon[5].HasItem)
                {
                    this.State.Player1Pokemon[5].PlaceCard(card);
                    this.State.Player1Pokemon[5].TurnCardFaceUp();
                    this.State.Player1Deck.BenchDict.Add(this.State.Player1Pokemon[5].CurrentCard);
                    thislog.AppendLine("Added " + this.State.Player1Pokemon[5].CurrentCard.Description + " to bench.");
                    placedcard = true;
                }
                else
                {
                    thislog.AppendLine("Bench is full. Can not add anymore Pokemon to bench.");
                    thislog.AppendLine("Instead, added " + card.Description + " to hand.");
                    this.State.Player1Deck.HandList.Add(card);
                    placedcard = false;
                    this.OnProcessHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                }

                //MUST remove the card first before checking hand for basics
                //If a card was placed then update the HandList and listbox
                if (placedcard)
                {
                    //We BenchPokemonChoosen so fire event 
                    this.OnProcessHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.PLAYER);
                }
                #endregion
            }
            else if (activeplayer == PPokemon.GamePlay.Player.AIPLAYER)
            {
                #region AIPLAYER
                //If the selected card is a basic, then Place it in an open bench spot
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    if (!this.State.AIPlayerPokemon[1].HasItem)
                    {
                        this.State.AIPlayerPokemon[1].PlaceCard(card);
                        this.State.AIPlayerPokemon[1].TurnCardFaceUp();
                        this.State.AIPlayerDeck.BenchDict.Add(this.State.AIPlayerPokemon[1].CurrentCard);
                        thislog.AppendLine("Added " + this.State.AIPlayerPokemon[1].CurrentCard.Description + " to bench.");
                        placedcard = true;
                    }
                    else if (!this.State.AIPlayerPokemon[2].HasItem)
                    {
                        this.State.AIPlayerPokemon[2].PlaceCard(card);
                        this.State.AIPlayerPokemon[2].TurnCardFaceUp();
                        this.State.AIPlayerDeck.BenchDict.Add(this.State.AIPlayerPokemon[2].CurrentCard);
                        thislog.AppendLine("Added " + this.State.AIPlayerPokemon[2].CurrentCard.Description + " to bench.");
                        placedcard = true;
                    }
                    else if (!this.State.AIPlayerPokemon[3].HasItem)
                    {
                        this.State.AIPlayerPokemon[3].PlaceCard(card);
                        this.State.AIPlayerPokemon[3].TurnCardFaceUp();
                        this.State.AIPlayerDeck.BenchDict.Add(this.State.AIPlayerPokemon[3].CurrentCard);
                        thislog.AppendLine("Added " + this.State.AIPlayerPokemon[3].CurrentCard.Description + " to bench.");
                        placedcard = true;
                    }
                    else if (!this.State.AIPlayerPokemon[4].HasItem)
                    {
                        this.State.AIPlayerPokemon[4].PlaceCard(card);
                        this.State.AIPlayerPokemon[4].TurnCardFaceUp();
                        this.State.AIPlayerDeck.BenchDict.Add(this.State.AIPlayerPokemon[4].CurrentCard);
                        thislog.AppendLine("Added " + this.State.AIPlayerPokemon[4].CurrentCard.Description + " to bench.");
                        placedcard = true;
                    }
                    else if (!this.State.AIPlayerPokemon[5].HasItem)
                    {
                        this.State.AIPlayerPokemon[5].PlaceCard(card);
                        this.State.AIPlayerPokemon[5].TurnCardFaceUp();
                        this.State.AIPlayerDeck.BenchDict.Add(this.State.AIPlayerPokemon[5].CurrentCard);
                        thislog.AppendLine("Added " + this.State.AIPlayerPokemon[5].CurrentCard.Description + " to bench.");
                        placedcard = true;
                    }
                    else
                    {
                        thislog.AppendLine("Bench is full. Can not add anymore Pokemon to bench.");
                        thislog.AppendLine("Instead, added " + card.Description + " to hand.");
                        this.State.AIPlayerDeck.HandList.Add(card);
                        placedcard = false;
                        this.OnProcessHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                    }
                    if (placedcard)
                    {
                        //We BenchPokemonChoosen so fire event 
                        this.OnProcessHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.AIPLAYER);
                    }
                }
                #endregion
            }
            log = thislog;
            return placedcard;
        }

        /// <summary>
        /// Get a List of CardHolders from a Player's bench.
        /// </summary>
        /// <param name="player">Player whom bench you wish to return</param>
        /// <param name="qty">Desired return amount of CardHolders</param>
        /// <param name="highestdamage">true if to sort by highest damage on the CurrentCard</param>
        /// <returns></returns>
        private List<CardHolder> GetBenchCardHolder(Player player, int qty, bool highestdamage)
        {
            List<CardHolder> SelectedBenchCardHolders = new List<CardHolder>();
            
            if (player == Player.PLAYER)
            {
                //MessageBox.Show("DEBUG: AddDamageToBenched");
                //Player1 Bench...
                foreach (CardHolder chitem in this.State.Player1Pokemon)
                {
                    if (chitem.HasItem)
                    {
                        if (!chitem.CurrentCard.State.ActivePokemon)
                        {
                            SelectedBenchCardHolders.Add(chitem);
                        }
                    }
                }
            }
            else
            {
                //AIPlayer Bench...
                foreach (CardHolder chitem in this.State.AIPlayerPokemon)
                {
                    if (chitem.HasItem)
                    {
                        if (!chitem.CurrentCard.State.ActivePokemon)
                        {
                            SelectedBenchCardHolders.Add(chitem);
                        }
                    }
                }
            }

            //If Sorted List requested then clear and rebuild
            if (highestdamage)
            {
                List<CardHolder> SortedchList = new List<CardHolder>();
                CardHolder HighestCH = new CardHolder();
                
                for (int i = 0; i < qty; i++)
                {
                    if (SelectedBenchCardHolders.Count > 0)
                    {
                        HighestCH = SelectedBenchCardHolders[0];
                        for (int x = 0; x < SelectedBenchCardHolders.Count; x++)
                        {
                            if (HighestCH.CurrentCard.State.CurrentDamage < SelectedBenchCardHolders[x].CurrentCard.State.CurrentDamage)
                                HighestCH = SelectedBenchCardHolders[x];
                        }
                        SortedchList.Add(HighestCH);
                        SelectedBenchCardHolders.Remove(HighestCH);//We grabed it so remove it so it is not grabbed again
                    }
                }

                SelectedBenchCardHolders = SortedchList;//Pass only the ones selected by highest damage counts
            }
            else
            {
                //Sorted List not requested, Now just return qty requested
                if (SelectedBenchCardHolders.Count > qty)
                {
                    //Too many results so trim the list
                    while (SelectedBenchCardHolders.Count != qty)
                    {
                        SelectedBenchCardHolders.RemoveAt(SelectedBenchCardHolders.Count - 1);
                    }
                }
            }

            return SelectedBenchCardHolders;
        }
    }
}
