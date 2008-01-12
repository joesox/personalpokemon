using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
//TODO: Placing card does not show facedown or faceup

namespace PPokemon.Cards
{
    public class CardHolder : PictureBox
    {
        #region Protected member variables
        /// <summary>
        /// The image used when it has an item
        /// </summary>
		protected System.Drawing.Bitmap filledImage;
        /// <summary>
        /// Image used when there is no item
        /// </summary>
		protected System.Drawing.Bitmap emptyImage;
        /// <summary>
        /// Image used when the drag is over the tile
        /// </summary>
		protected System.Drawing.Bitmap overImage;

        /// <summary>
        /// Image used when TurnFaceDown() is called
        /// </summary>
        protected System.Drawing.Bitmap cardfacedown;

        /// <summary>
        /// Image used when TurnFaceUp() is called
        /// </summary>
        protected System.Drawing.Bitmap nullfaceup;
        #endregion

        /// <summary>
        /// Label for Name
        /// </summary>
        public Label lbName = new System.Windows.Forms.Label();

        public Label lbCardType = new System.Windows.Forms.Label();

        /// <summary>
        /// Holds image of Pokemon
        /// </summary>
        private WebBrowser webimage = new WebBrowser();

        /// <summary>
        /// Name of Card for Filled and Over
        /// </summary>
        public string CardName = "";

        private string LocalSWF = "";

        public string CardType = "";

        /// <summary>
        /// Holds the passed Card for the Pokemon
        /// </summary>
        public Card CurrentCard = null;

        public enum CurrentState { FaceUp, FaceDown, Null };

        public CurrentState CardState = CurrentState.Null;

        #region Constructor
        /// <summary>
        /// Loads the picutres and initializes the tile
        /// Only created on first drawings.
        /// </summary>
        public CardHolder()
		{
            //this.Location.Y;//down 72x102
            //this.Location.X;//across
            //Set the location for the lbName
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point((this.Location.X + 1), (this.Location.Y + 1));//Label must move with image
            this.lbName.Size = new System.Drawing.Size(65, 10);
            this.lbName.Text = "";
            this.lbName.BackColor = Color.Yellow;
            this.Controls.Add(this.lbName);
            this.lbName.Hide();

            //lbCardType
            this.lbCardType.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCardType.Location = new System.Drawing.Point((this.Location.X + 1), (this.Location.Y + 87));//from bottom
            this.lbCardType.Size = new System.Drawing.Size(65, 10);
            this.lbCardType.Text = "";
            this.lbCardType.BackColor = Color.Yellow;
            this.Controls.Add(this.lbCardType);
            this.lbCardType.Hide();

            //Setup the WebBrowser control
            this.webimage.Name = "webimage";
            this.webimage.ScriptErrorsSuppressed = true;
            this.webimage.ScrollBarsEnabled = false;
            this.webimage.Location = new System.Drawing.Point((this.Location.X + 1), (this.Location.Y + 15));
            this.webimage.Size = new System.Drawing.Size(65, 50);
            this.Controls.Add(this.webimage);
            this.webimage.Hide();//Hide every one created until ready

			// Load the pictures
            filledImage = new System.Drawing.Bitmap(PPokemon.Properties.Resources.CardBack_sm);
            emptyImage = new System.Drawing.Bitmap(PPokemon.Properties.Resources._null);
			overImage = new System.Drawing.Bitmap(PPokemon.Properties.Resources.GreyBack);
            cardfacedown = new System.Drawing.Bitmap(PPokemon.Properties.Resources.CardBack_sm);
            nullfaceup = new System.Drawing.Bitmap(PPokemon.Properties.Resources._null);
           
			// Initialize
			hasItem = false;

            // Initialize events and properties
			this.Image = emptyImage;
			this.Height = emptyImage.Height;
			this.Width = emptyImage.Width;
			this.MouseDown += new MouseEventHandler(OnMouseDown);
			this.DragEnter += new DragEventHandler(OnDragEnter);
			this.DragDrop += new DragEventHandler(OnDragDrop);
			this.DragLeave += new EventHandler(OnDragLeave);
            this.DoubleClick += new EventHandler(OnDoubleClick);
			this.AllowDrop = true;

            //
            CardState = CurrentState.Null;
		}
        #endregion

        #region HasItem
        /// <value>
        /// bool that signals whether this tile contains an item or not (read only)
        /// </value>
		public bool HasItem
        {
            get { return hasItem; }
            set { hasItem = value; }
        }
        bool hasItem = false;
        #endregion

        #region OnDragDrop
        /// <summary>
        /// Handles the DragDrop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		public void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            // Switch the cursor to the arrow
			this.Cursor = Cursors.Default;

			// Put an item here
			PutItem();			
		}
        #endregion

        #region OnDragEnter
        /// <summary>
        /// Handles the DragEnter event
        /// </summary>
        /// <param name="sender">The New CardHolder object just entered</param>
        /// <param name="e"></param>
		public void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (hasItem == false) {
		
		        // Check to see if we can handle what is being dragged
                //If the current cardholder that we are over is Null
                if (((CardHolder)sender).CardState == CurrentState.Null)
                {
                    // Change the cursor to a hand
				    this.Cursor = Cursors.Hand;

                    // Lets it know what effects can be done
					e.Effect = DragDropEffects.Copy;

                    // Change the image
					this.Image = overImage;
				}			
			}
			else {
                // Change the cursor to the arrow
				this.Cursor = Cursors.Default;
			}
		}
        #endregion

        #region OnDragLeave
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Previous Cardholder</param>
        /// <param name="e"></param>
		public void OnDragLeave(object sender, System.EventArgs e)
		{
            // Only do something if the tile has an no item
			if (hasItem == false) {
                // Reset the cursor
				this.Cursor = Cursors.Default;
                // Switch the image back to empty
				this.Image = emptyImage;
			}
		}
        #endregion

        #region OnGiveFeedback
        /// <summary>
        /// Handles the feedback event
        /// </summary>
        /// <param name="gfbEvent"></param>
		protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbEvent)
		{
            // Allow us to use our own cursors when dragging
			gfbEvent.UseDefaultCursors = true;			
		}
        #endregion

        #region OnMouseDown
        /// <summary>
        /// Handles the mouse down event
        /// </summary>
        /// <param name="sender">Current CardHolder selected by mousedown</param>
        /// <param name="e">MouseEventArgs</param>
		public void OnMouseDown(object sender,MouseEventArgs e)   
		{
            //Allow DoubleClicking!
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                // Only allowing dragging if there is an item
                if (hasItem)
                {
                    //Set the current variables in the settings
                    PPokemon.Properties.Settings.Default.CardName = ((CardHolder)sender).CardName;
                    PPokemon.Properties.Settings.Default.CardState = ((CardHolder)sender).CardState.ToString();
                    PPokemon.Properties.Settings.Default.LocalSWF = ((CardHolder)sender).LocalSWF;
                    PPokemon.Properties.Settings.Default.CardType = ((CardHolder)sender).CardType;
                    PPokemon.Properties.Settings.Default.Save();
                    // Start the dragging process
                    DragDropEffects effect = DoDragDrop(sender, DragDropEffects.Copy);

                    //Grab the items we need to pass
                    //e.Data.SetData("CardState", this.CardState);

                    // Check to see if the image was dropped somewhere
                    if (effect != DragDropEffects.None)
                    {

                        // It was dropped so remove the item
                        RemoveItem();
                    }
                }
            }
		}
        #endregion

        #region OnDoubleClick
        /// <summary>
        /// When PictureBox is DoubleClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDoubleClick(object sender, EventArgs e)
        {
            CardHolder senderCH = ((CardHolder)sender);
            /*
            if (senderCH.CardState == CurrentState.FaceDown)
                TurnCardFaceUp();
            else if (senderCH.CardState == CurrentState.FaceUp)
                TurnCardFaceDown();
             */
            if (senderCH.HasItem)
            {
                //Only enable if 
                if (senderCH.CardState == CurrentState.FaceUp)
                {
                    CardPopUpForm cardform = new CardPopUpForm();
                    senderCH.CurrentCard.Panel.Location = new Point(1, 1);
                    //Add the Card Panel to where you need to draw it
                    cardform.Controls.Add(senderCH.CurrentCard.Panel);
                    cardform.Tag = senderCH;
                    cardform.DisplayCurrentStatus();
                    cardform.Show();
                }
            }
        }

        #endregion

        #region PutItem
        /// <summary>
        /// Put an item on the tile.
        /// </summary>
        public void PutItem()
        {
            // Set the hasItem signal
            hasItem = true;

            if (PPokemon.Properties.Settings.Default.CardState == "FaceUp")
            {
                this.TurnCardFaceUp();
            }
            else if (PPokemon.Properties.Settings.Default.CardState == "FaceDown")
            {
                this.TurnCardFaceDown();
            }
        }
        #endregion

        #region RemoveItem
        /// <summary>
        /// Takes the items off the tile
        /// Must clear all stored setting data
        /// </summary>
		public void RemoveItem()
		{
            // Set the signal to false
			hasItem = false;
            // Set the image to empty
			this.Image = emptyImage;

            //Clear the name
            this.lbName.Text = "";//The Control
            this.lbName.BackColor = this.BackColor;
            this.CardName = "";//The local var

            //Clear the cardtype
            this.lbCardType.Text = "";//The Control
            this.lbCardType.BackColor = this.BackColor;
            this.CardType = "";//The local var

            //Clear LocalSWF
            //Hide webimage
            this.webimage.Refresh();//The Control
            this.webimage.Hide();
            this.LocalSWF = "";//The local var

            //Erase the CurrentCard
            this.CurrentCard = new Card(Cardtype.Null);

            //Change the CardState
            this.CardState = CurrentState.Null;
		}
        #endregion

        /// <summary>
        /// Place a Card in this CardHolder
        /// Sets the name, image, cardtype on cardholder
        /// </summary>
        /// <param name="pokemoncard"></param>
        public void PlaceCard(Card pokemoncard)
        {
            //Set the passed Card information to holder
            CurrentCard = new Card(pokemoncard.Properties.CardType);

            //Check and make sure the Card is valid
            if (CurrentCard.Properties.CardType != Cardtype.Null)
            {

                CurrentCard = pokemoncard;

                //Assign the card type
                this.CardType = CurrentCard.Properties.CardType.ToString();

                //Is this an Energy Card?
                if (CurrentCard.Properties.CardType != Cardtype.Energy)
                {
                    //Assign the card name to holder object
                    this.CardName = CurrentCard.Properties.Pokemon.Name;
                    //Assign the card's image url
                    this.LocalSWF = CurrentCard.Properties.Pokemon.LocalSWF;
                    this.webimage.Refresh();
                    this.webimage.Navigate(CurrentCard.Properties.Pokemon.LocalSWF);
                    this.webimage.Show();
                }
                else
                {
                    //This is an energy card so draw differently and get Energy type
                    //Assign the card name to holder object
                    this.CardName = CurrentCard.Properties.PokemonType.ToString();
                    this.Image = new Bitmap(Ops.GetEnergyCardImage(CurrentCard.Properties.PokemonType));
                }

                //this holder does have an item now
                this.hasItem = true;

                //Default it to place the card face down
                this.TurnCardFaceDown();
            }
            else
                MessageBox.Show("DEBUGING MESSAGE: Null type CurrentCard being placed in CardHolder!");
        }

        /// <summary>
        /// Evolve this CardHolder's Card
        /// Removes any SpecialConditions
        /// </summary>
        /// <param name="newpokemoncard"></param>
        /// <param name="pokedex"></param>
        public void Evolve(Card newpokemoncard)
        {
            //transfer the CurrentCard.State to the new card 
            CardState currentstate = new CardState();
            this.CurrentCard.State.CurrentCondition = SpecialConditions.None;//Evolving removes SpecialConditions
            currentstate = this.CurrentCard.State.Clone();

            //Clear the CardState before moving it to AttachedCards
            this.CurrentCard.State = new CardState();

            //Move CurrentCard to AttachedCards in new state
            currentstate.AttachedCards.Add(new Card(this.CurrentCard.Properties.CardType, this.CurrentCard.Properties.Pokemon,
                this.CurrentCard.Properties.PokemonType, this.CurrentCard.Properties.HP, this.CurrentCard.Properties.Attacks, this.CurrentCard.Properties.Weakness,
                this.CurrentCard.Properties.Resistance, this.CurrentCard.Properties.RetreatCost));

            //Now Assign the New Card
            this.PlaceCard(newpokemoncard);
            //Since this is an evolve, the cards must be faceup
            this.TurnCardFaceUp();

            //Let State know that this card has just been evolved
            currentstate.EvolvedPokemon = true;

            //Now complete the transfer of the CardState
            this.CurrentCard.State = currentstate;
        }

        /// <summary>
        /// If there is a card in the CardHolder, turn it face down
        /// </summary>
        public void TurnCardFaceDown()
        {
            //If the current image is not the empty image
            if (this.Image != emptyImage)
            {
                //draw the cardfacedown image
                this.Image = cardfacedown;
                //hide the name label
                this.lbName.Hide();
                //hide
                this.lbCardType.Hide();
                //hide the webimage
                this.webimage.Hide();
                //this holder does have an item
                this.hasItem = true;
                //Set the card state
                CardState = CurrentState.FaceDown;
            }
        }

        /// <summary>
        /// If there is a emptyImage or a cardfacedown, display a nullfaceup
        /// </summary>
        public void TurnCardFaceUp()
        {
            //If there is a emptyImage or a cardfacedown
            if (this.Image == emptyImage || this.Image == cardfacedown || this.Image == overImage)
            {
                //Set the label text to current cardname
                if (this.CardName != "")
                {
                    //If this is the first creation of the card use...
                    this.lbName.Text = this.CardName;
                    this.lbCardType.Text = this.CardType;
                }
                else
                {
                    this.lbName.Text = PPokemon.Properties.Settings.Default.CardName;
                    this.CardName = PPokemon.Properties.Settings.Default.CardName;
                    this.LocalSWF = PPokemon.Properties.Settings.Default.LocalSWF;
                    this.lbCardType.Text = PPokemon.Properties.Settings.Default.CardType;
                    this.CardType = PPokemon.Properties.Settings.Default.CardType;
                }
                //show the label
                this.lbName.BackColor = Color.Yellow;
                this.lbName.Show();
                //show the label
                this.lbCardType.BackColor = Color.Yellow;
                this.lbCardType.Show();

                //Make sure the energyimage doesn't get overridden
                if (this.lbCardType.Text != "Energy")
                {
                    //draw the nullfaceup null image
                    this.Image = nullfaceup;

                    //show the webimage
                    this.webimage.Refresh();
                    this.webimage.Navigate(this.LocalSWF);
                    this.webimage.Show();
                }
                else
                    this.Image = new Bitmap(Ops.GetEnergyCardImage(Ops.ConvertStringToPokemonTYPE(this.lbName.Text)));


                //this holder does have an item
                this.hasItem = true;
                //Set the card state
                CardState = CurrentState.FaceUp;
            }
        }

        /// <summary>
        /// Prepare this card for the discard pile
        /// clears CurrentDamage and CurrentCondition on all attached cards
        /// </summary>
        public void DiscardPrep()
        {
            this.CurrentCard.State.CurrentDamage = 0;
            this.CurrentCard.State.CurrentCondition = SpecialConditions.None;
            foreach (Card card in this.CurrentCard.State.AttachedCards)
            {
                card.State.CurrentDamage = 0;
                card.State.CurrentCondition = SpecialConditions.None;
            }
        }
    }
}
