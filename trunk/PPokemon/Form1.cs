/////////////////////////////////////////////////////////
// PPokemon - Personal Pokemon v1.14.0.0 Active Development
// by Joseph P. Socoloski III 
// Copyright 2007-2008. All Rights Reserved.
// NOTE:   
// WHAT'S NEW: 	
//          -Double-clicking on grey on Card or Card in handlist displays Card-popup.
//          -Trainer and Support card Support.
//          -Decks have version in xml to document xml changes.
// LIMITS:  
//          -Limited 'ex' Card support (does not display PokeBody if card has more than one attack. no extra special events support)
//          -Currently, Stadium Card support. 
//          -Currently, no Retreat support (Retreat Button is shown on Card Details Form).
//          -Currently, can not attach Energy Cards to Benched Pokemon.
//          -Currently, When ActivePokemon has a SpecialCondition the card does not turn but it is labelled correctly.
//          -Drag-n-Drop of cards is not supported but it is enabled. (in other words, drag-n-drop is not supported)
// TODO:    
//          -Finish Trainer card attacks (Supporter-Rival-OpponentNextAttack is its second attack?, Super Scoopup)
//          -BUG: Trainer got sent to Bench? - catch during PlaceCard?
//          -Finish some Terra attacks ()
//          -Finish SearchDeckForBasicOrEvolution to support Ex (This probably means adding BasicEx, Stage1Ex etc. to PPokemon.Cards.Cardtype)
//          -Finish Retreat on Card Details Form
//          -BUG:  -1 HP with Bench and being knockedout (Fighting and Water deck)
//          -Poke-Body/Power: continue working on supporting them
//          -Attacks: continue working on supporting them
//          -Attach Energy Cards to Bench Pokemon
//          -Continue to work on Ops.GetBestEnabledAttack()
//          -Drag and Drop cards on cardholder, update the cardholders (activecard=true, damage etc.)
//          -Redesign application: Seperate GameEngine in seperate dll project etc.
// ENABLED CardEventNames in this release for Attacks:
//      AddDamageToAttack
//      AddDamageToBenched
//      AddDamageToEachBenched
//      AddDamageToSelf
//      AddSpecialCToAttack
//      AttachCardToAPokemon
//      ChooseOpponPokemon
//      DiscardAnEnergy
//      DrawACard
//      FlipACoin
//      IfDamaged
//      IfDefendingDamaged
//      IfFlipHeads
//      IfFlipTails
//      IfLessEnergyThanDefending
//      MinusAmountMultipliedByAttachedDamage
//      MultiplyByDamageOnSelf
//      None
//      OpponentChooseCardsFromDrawn
//      RemoveDamageCounter
//      SearchDeckForBasic
//      SearchDeckForBasicPlaceToBench
//      SearchDeckForEnergy
//      SearchDeckForPokemon
//      SelectAllPokemonType
//      SwitchWithBenched
//      ShuffleDeck
//LICENSE
//BY DOWNLOADING AND USING, YOU AGREE TO THE FOLLOWING TERMS:
//If it is your intent to use this software for non-commercial purposes, 
//such as in academic research, this software is free and is covered under 
//the GNU GPL License, given here: <http://www.gnu.org/licenses/gpl.txt> 
////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Joe.Utils;
using System.IO;
using SWFToImage;
using System.Collections.Specialized;
using System.Xml;
using PPokemon.Cards;
using Wintellect.PowerCollections;

namespace PPokemon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Form Global Variables
        /// <summary>
        /// Pokedex Object
        /// </summary>
        private Pokedex Pokedex;

        /// <summary>
        /// Holds all of the filepath info in /deck folder
        /// </summary>
        private string[] dirDeckFiles;

        private Deck SelectedDeck = new Deck();
        private Deck CurrentDeck = new Deck();
        private Deck PCdeck = new Deck();

        /// <summary>
        /// Main Game object
        /// </summary>
        private GamePlay.GameEngine game = new PPokemon.GamePlay.GameEngine();
        #endregion

        #region MenuStrip
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Restart the application and start new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        /// <summary>
        /// AutoSetup: Download swfs, xml, then convert swf to jpgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadSetupFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btDownload_Click(sender, e);
        }

        /// <summary>
        /// Converts all .swf files in the swf folder to jpg and places them in the jpg folder
        /// </summary>
        private void convertSwfToJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ConvertSWFtoJPG();
            Cursor.Current = Cursors.Default;
        }
        #endregion MenuStrip

        /// <summary>
        /// Create the application's folders if they do not exist
        /// </summary>
        /// <returns>true if Setup is needed</returns>
        private bool InitAppFolders()
        {
            //Is something in \xml?
            if (!Directory.Exists(Common.xmlFolder))
            {
                //folder does not exist so create it
                Directory.CreateDirectory(Common.xmlFolder);
                //obviously nothing there
            }
            else
            {
                //Are there files in the directory?
                string[] dirFiles = Directory.GetFiles(Common.xmlFolder);
                if (dirFiles.Length >= Common.pokemonCount)
                {
                    //Looks like the user has finished the Setup, so disable the setup
                    gboxSetup.Enabled = false;
                    downloadSetupFilesToolStripMenuItem.Enabled = false;
                }
            }

            //Is something in \swf?
            if (!Directory.Exists(Common.swfFolder))
            {
                //folder does not exist so create it
                Directory.CreateDirectory(Common.swfFolder);
                //obviously nothing there
            }
            else
            {
                //Are there files in the directory?
                string[] dirFiles = Directory.GetFiles(Common.swfFolder);
                if (dirFiles.Length >= Common.pokemonCount)
                {
                    //Looks like the user has finished the Setup, so disable the setup
                    gboxSetup.Enabled = false;
                    downloadSetupFilesToolStripMenuItem.Enabled = false;
                }
            }

            //Is something in \jpg?
            if (!Directory.Exists(Common.jpgFolder))
            {
                //folder does not exist so create it
                Directory.CreateDirectory(Common.jpgFolder);
                //obviously nothing there
            }
            else
            {
                //Are there files in the directory?
                string[] dirFiles = Directory.GetFiles(Common.jpgFolder);
                if (dirFiles.Length >= Common.pokemonCount)
                {
                    //Looks like the user has finished the Setup, so disable the setup
                    gboxSetup.Enabled = false;
                    downloadSetupFilesToolStripMenuItem.Enabled = false;
                }
            }

            //Is something in \decks?
            if (!Directory.Exists(Common.decksFolder))
            {
                //folder does not exist so create it
                Directory.CreateDirectory(Common.decksFolder);
                //obviously nothing there
            }
            else
            {
                //Are there files in the directory?
                dirDeckFiles = Directory.GetFiles(Common.decksFolder);
            }

            return gboxSetup.Enabled;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //Hide the lBoxTurnOptions for Player1
            lBoxTurnOptions.Visible = false;

            //Directory checks...
            if (InitAppFolders())
            {
                //Setup is needed so prep
                progressBar1.Maximum = Common.pokemonCount;
                progressBar1.Step = 1;
            }
            else
            {
                lbProgressbar.Text = "Setup previously completed";
                //See if Pokedex.xml exists
                if (!File.Exists(Common.Pokedex_xml))
                {
                    ImportglobalPokedex(Common.globalPokedex_xml, Common.Pokedex_xml);
                    //Reload the application to load the new Common.Pokedex_xml
                    MessageBox.Show("Created the new Pokedex.xml file. Application will now restart.");
                    Application.Restart();
                }
                else
                {
                    //Initialize the Pokedex Object
                    Pokedex = new Pokedex(Common.Pokedex_xml);

                    //Populate the Decks combobox
                    if (dirDeckFiles.Length > 0)
                    {
                        //Load all of the Names of the decks in the dropdownbox
                        foreach (string file in dirDeckFiles)
                        {
                            cBoxAvailDecks.Items.Add(Joe.Utils.Helper.GetFileNameFromFullPath(file));
                        }
                        cBoxAvailDecks.SelectedIndex = 0;
                    }
                    

                    //Load the Pokedex into the Listbox
                    Dictionary<int, Pokemon>.ValueCollection ValueColl = Pokedex.IndexDictionary.Values;
                    foreach (Pokemon pokemon in ValueColl)
                    {
                        listBoxPokemon.Items.Add(pokemon.Name);
                    }

                    //Select the first one
                    listBoxPokemon.SelectedIndex = 0;

                    //Go to the tabPagePokemon
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPagePokemon"];
                }

                //Load all of the current Cardtype in the textbox
                foreach (string name in Enum.GetNames(typeof(Cardtype)))
                {
                    tbCardType.AppendText(name + "\r\n");
                }

                //Load all of the current CardEventName in the textbox
                foreach (string name in Enum.GetNames(typeof(CardEventNames)))
                {
                    tbCardEventNames.AppendText(name + "\r\n");
                }

                //Load all of the current PokemonTYPE in the textbox
                foreach (string name in Enum.GetNames(typeof(PokemonTYPE)))
                {
                    tbPokemonType.AppendText(name + "\r\n");
                }

                startNewGameToolStripMenuItem.Enabled = false;
            }

            //Subscribe to GamePlay HookEvents
            game.HookEvent += new PPokemon.GamePlay.GameEngine.HookHandler(GameEventHook);
        }

        #region Setup Steps
        /// <summary>
        /// Downloaded Character swf images to the swf folder
        /// </summary>
        private void Downloadswfs()
        {
            progressBar1.Value = 0;
            lbProgressbar.Text = "Step 1 of 2: Downloading Character Images...";
            this.Update();
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 1; i <= Common.pokemonCount; i++)
            {
                Joe.Utils.Helper.DownloadWebpage(Common.urlPokedexcharacterswfs + Convert.ToString(i) + ".swf", Common.swfFolder + "\\" + Convert.ToString(i) + ".swf");
                progressBar1.PerformStep();
                this.Focus();
            }
            Cursor.Current = Cursors.Default;
            lbProgressbar.Text = "Downloading Character Complete!";
        }

        /// <summary>
        /// Downloaded Character swf images to the swf folder
        /// </summary>
        private void Downloadxmls()
        {
            progressBar1.Value = 0;
            lbProgressbar.Text = "Step 2 of 2: Downloading Character XML...";
            this.Update();
            Cursor.Current = Cursors.WaitCursor;
            //Download globalPokedex.xml
            Joe.Utils.Helper.DownloadWebpage(Common.urlPokedexXMLglobalPokedex, Common.xmlFolder + "\\globalPokedex.xml");

            for (int i = 1; i <= Common.pokemonCount; i++)
            {
                Joe.Utils.Helper.DownloadWebpage(Common.urlPokedexXML + Convert.ToString(i) + ".xml", Common.xmlFolder + "\\" + Convert.ToString(i) + ".xml");
                progressBar1.PerformStep();
                this.Focus();
            }
            Cursor.Current = Cursors.Default;
            lbProgressbar.Text = "Downloading Character XML Complete!";
        }

        /// <summary>
        /// Converts all .swf files in the swf folder to jpg and places them in the jpg folder
        /// </summary>
        private void ConvertSWFtoJPG()
        {
            SWFToImageObjectClass swf2jpg = new SWFToImageObjectClass();

            string[] dirFiles = Directory.GetFiles(Common.swfFolder);
            if (dirFiles.Length == Common.pokemonCount)
            {
                //gboxSetup.Enabled = true;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                progressBar1.Maximum = Common.pokemonCount;
                lbProgressbar.Text = "Converting swf to jpg...";
                this.Update();
                
                for (int i = 1; i <= Common.pokemonCount; i++)
                {
                    swf2jpg.InputSWFFileName = Common.swfFolder + "\\" + Convert.ToString(i) + ".swf";
                    swf2jpg.FrameIndex = 0; //number of frame to extract
                    swf2jpg.ImageOutputType = TImageOutputType.iotJPG; //set output image type to Jpeg (0 = BMP, 1 = JPG, 2 = GIF)
                    swf2jpg.Execute();
                    swf2jpg.SaveToFile(Common.jpgFolder + "\\" + Convert.ToString(i) + ".jpg");

                    progressBar1.PerformStep();
                    this.Focus();
                }
                //gboxSetup.Enabled = false;

            }
            else
            {
                MessageBox.Show("ConvertSWFtoJPG Error: dirFiles.Length != Common.pokemonCount\r\nSolution: Delete your xml,swf, and jpg folders and re-run PPokemon setup.");
            }
            lbProgressbar.Text = "Converting swf to jpg Complete!";
            this.Update();
        }
        #endregion Setup Steps

        /// <summary>
        /// Navigate to the PPokemon Googlecode website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pPokemonWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Common.urlPPokemonGooglecode);
        }

        /// <summary>
        /// Navigate to Crate A Card and Deck Wiki page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelCreateCard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Common.urlCreateCardWiki);
        }

        /// <summary>
        /// Google findacard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.com/search?sourceid=navclient&ie=UTF-8&rlz=1T4GGIH_enUS250US250&q=pokemon+findacard");
        }

        /// <summary>
        /// AutoSetup: Download swfs, xml, then convert swf to jpgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Downloadswfs();     //Step 1
                Downloadxmls();     //Step 2
                //ConvertSWFtoJPG();  //Step 3 No Longer needed, optional from menu

                //Disable the setup groupbox
                gboxSetup.Enabled = false;
                downloadSetupFilesToolStripMenuItem.Enabled = false;

                //Import all of the global index and create a better Pokedex.xml
                ImportglobalPokedex(Common.globalPokedex_xml, Common.Pokedex_xml);
                //Reload the application to load the new Common.Pokedex_xml
                MessageBox.Show("Setup is Complete! Application will now restart.", "Setup is Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("btDownload_Click Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Import globalPokedex.xml: list of all Pokemon and attributes
        /// each order listed is the Pokemon's index number
        /// </summary>
        /// <param name="filename"></param>
        private void ImportglobalPokedex(string filename, string newxmlfilename)
        {
         //   try
         //   {
                if (File.Exists(filename))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode root = xmlDoc.DocumentElement;
                    XmlTextWriter xmlWriter = new XmlTextWriter(newxmlfilename, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");

                    //Clear SB_MyInventory, SC_MyInvInBrackets, OD_MyInventory
                    //SB_MyInventory.Remove(0, SB_MyInventory.Length);
                    //SC_MyInvInBrackets.Clear();
                    //OD_MyInventory.Clear();

                    //Read the xml
                    //append each element into the 
                    StringCollection rawlineCollection = new StringCollection();
                    rawlineCollection.AddRange(File.ReadAllLines(filename));
                    rawlineCollection.RemoveAt(0);
                    rawlineCollection.RemoveAt(rawlineCollection.Count - 1);
                    //rawlineCollection Count = 491, [0]="<poke>"

                    // Enumerates the elements in the StringCollection.
                    StringEnumerator myEnumerator = rawlineCollection.GetEnumerator();
                    int i = 0;
                    while (myEnumerator.MoveNext())
                    {
                        if (i == 0)
                        {
                            xmlWriter.WriteStartElement("poke");
                        }
                        else
                        {
                            ConvertRawLine(myEnumerator.Current, xmlWriter, i);
                        }

                        i = i + 1;
                    }

                    xmlWriter.Close();//write file
                }
                else
                    MessageBox.Show(filename + "\r\ndoes not exist");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("CreateIndexesFromXML error: " + ex.Message);
        //    }
        }

        /// <summary>
        /// Name, type, height_inches, weight, pbHP, pbATTACK, pbDEFENSE, pbSPECIALATTACK, pbSPECIALDEFENSE, pbSPEED, ability, UNKNOWN, evolution lowest to highest
        /// <p><i>BULBASAUR</i><i>grass,poison</i><i>28</i><i>15.2</i><i>20,40,40,40,40,40</i><i>OVERGROW</i><i>5</i><i>1,2,3</i></p>
        /// </summary>
        /// <param name="rawline"></param>
        /// <returns></returns>
        private void ConvertRawLine(string rawline, XmlWriter xmlWriter, int index)
        {
            rawline = rawline.Replace("<p>", "");
            rawline = rawline.Replace("</p>", "");
            rawline = rawline.Replace("<i>", "");
            string[] seperators = new string[] { "</i>" };
            string[] allpieces = rawline.ToString().Split(seperators, StringSplitOptions.None);

            XmlDocument doc = new XmlDocument();
            xmlWriter.WriteStartElement("pokemon");
            xmlWriter.WriteAttributeString("name", allpieces[0]);
            xmlWriter.WriteAttributeString("type", allpieces[1]);
            xmlWriter.WriteAttributeString("height_inches", allpieces[2]);
            xmlWriter.WriteAttributeString("weight", allpieces[3]);

            string[] Commaseperators = new string[] { "," };
            string[] pBarpieces = allpieces[4].ToString().Split(Commaseperators, StringSplitOptions.None);
            if(pBarpieces[0].Trim() == "")
                xmlWriter.WriteAttributeString("pbHP", "50");
            else
                xmlWriter.WriteAttributeString("pbHP", pBarpieces[0]);

            if (pBarpieces[1].Trim() == "")
                xmlWriter.WriteAttributeString("pbATTACK", "50");
            else
                xmlWriter.WriteAttributeString("pbATTACK", pBarpieces[1]);

            if (pBarpieces[2].Trim() == "")
                xmlWriter.WriteAttributeString("pbDEFENSE", "50");
            else
                xmlWriter.WriteAttributeString("pbDEFENSE", pBarpieces[2]);

            if (pBarpieces[3].Trim() == "")
                xmlWriter.WriteAttributeString("pbSPECIALATTACK", "50");
            else
                xmlWriter.WriteAttributeString("pbSPECIALATTACK", pBarpieces[3]);

            if (pBarpieces[4].Trim() == "")
                xmlWriter.WriteAttributeString("pbSPECIALDEFENSE", "50");
            else
                xmlWriter.WriteAttributeString("pbSPECIALDEFENSE", pBarpieces[4]);

            if (pBarpieces[5].Trim() == "")
                xmlWriter.WriteAttributeString("pbSPEED", "50");
            else
                xmlWriter.WriteAttributeString("pbSPEED", pBarpieces[5]);

            xmlWriter.WriteAttributeString("ability", allpieces[5]);
            xmlWriter.WriteAttributeString("unknown", allpieces[6]);

            //Pokemon's main index number...
            xmlWriter.WriteAttributeString("index", Convert.ToString(index));

            //Pokemon's Description
            xmlWriter.WriteStartElement("desc");
            xmlWriter.WriteValue(Pokemon.CreateDescription(index));
            xmlWriter.WriteEndElement();

            //Pokemon's Local swf path
            xmlWriter.WriteStartElement("localimage");
            xmlWriter.WriteValue(Common.swfFolder + "\\" + (Convert.ToString(index)) + ".swf");
            xmlWriter.WriteEndElement();

            //Evolution Pokemon...
            string[] Evolvpieces = allpieces[7].ToString().Split(Commaseperators, StringSplitOptions.None);
            if (Evolvpieces.Length > 0)
            {
                foreach (string pokemon in Evolvpieces)
                {
                    xmlWriter.WriteStartElement("evolution");
                    xmlWriter.WriteValue(pokemon);
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                //There are no evolutions for this pokemon
                xmlWriter.WriteStartElement("evolution");
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            //return node;
        }

        /// <summary>
        /// Occurs when SelectedIndex changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokemon pokemon = new Pokemon();

            if (Pokedex.IndexDictionary.TryGetValue(listBoxPokemon.SelectedIndex + 1, out pokemon))
            {
                //show the first selected pokemon in the webrowser control
                this.webBrowserPokemon.Navigate(pokemon.LocalSWF);

                //Character's Labels...
                lbName.Text = pokemon.Name;
                lbType.Text = "Type: " + pokemon.Type;
                lbAbility.Text = "Ability: " + pokemon.Ability;
                lbHeight.Text = "Height: " + pokemon.GetHeightToFeetString();
                lbWeight.Text = "Weight: " + pokemon.Weight + " lbs";
                lbDescription.Text = pokemon.Description;

                //Character's Progressbars...
                pbarHP.Value = pokemon.pbHP;
                pbarAttack.Value = pokemon.pbATTACK;
                pbarDefense.Value = pokemon.pbDEFENSE;
                pbarSpecAttack.Value = pokemon.pbSPECIALATTACK;
                pbarSpecDefense.Value = pokemon.pbSPECIALDEFENSE;
                pbarSpeed.Value = pokemon.pbSPEED;

                Pokemon EPokemon = new Pokemon();
                int pokemonindex;

                if (pokemon.EvolutionDict.Count == 1)
                {
                    panelEvolve1.Visible = true;
                    panelEvolve2.Visible = false;
                    panelEvolve3.Visible = false;

                    //Count == 1...
                    pokemon.EvolutionDict.TryGetValue(1, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve1_1.Navigate(EPokemon.LocalSWF);
                    lbEvolve1_1.Text = EPokemon.Name;
                }
                if (pokemon.EvolutionDict.Count == 2)
                {
                    panelEvolve1.Visible = false;
                    panelEvolve2.Visible = true;
                    panelEvolve3.Visible = false;
                    //Get first evolve
                    pokemon.EvolutionDict.TryGetValue(1, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve2_1.Navigate(EPokemon.LocalSWF);
                    lbEvolve2_1.Text = EPokemon.Name;
                    //Get second evolve
                    pokemon.EvolutionDict.TryGetValue(2, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve2_2.Navigate(EPokemon.LocalSWF);
                    lbEvolve2_2.Text = EPokemon.Name;
                }
                if(pokemon.EvolutionDict.Count == 3)
                {
                    panelEvolve1.Visible = false;
                    panelEvolve2.Visible = false;
                    panelEvolve3.Visible = true;
                    //Get first evolve
                    pokemon.EvolutionDict.TryGetValue(1, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve3_1.Navigate(EPokemon.LocalSWF);
                    lbEvolve3_1.Text = EPokemon.Name;
                    //Get second evolve
                    pokemon.EvolutionDict.TryGetValue(2, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve3_2.Navigate(EPokemon.LocalSWF);
                    lbEvolve3_2.Text = EPokemon.Name;
                    //Get third evolve
                    pokemon.EvolutionDict.TryGetValue(3, out pokemonindex);
                    Pokedex.IndexDictionary.TryGetValue(pokemonindex, out EPokemon);
                    webrowserEvolve3_3.Navigate(EPokemon.LocalSWF);
                    lbEvolve3_3.Text = EPokemon.Name;
                }
                if (pokemon.EvolutionDict.Count > 3)
                {
                    panelEvolve1.Visible = false;
                    panelEvolve2.Visible = false;
                    panelEvolve3.Visible = false;
                }
                this.Update();
            }
        }

        /// <summary>
        /// Search the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            if (listBoxPokemon.FindString(tbSearch.Text.Trim()) != -1)
            {
                listBoxPokemon.SelectedIndex = (listBoxPokemon.FindString(tbSearch.Text));
                this.Update();
            }
        }

        private void attackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
                sound.SoundLocation = @"C:\Documents and Settings\Joseph\My Documents\Visual Studio 2005\Projects\PPokemon\PPokemon\Resources\attacksound.wav";
                sound.Play();

                #region TESTING
                //Examine the Attack
                CardDirections attackdir = new CardDirections();
                attackdir = pbActivePokemon.CurrentCard.GetAttackDirections(pbActivePokemon.CurrentCard.GetAttackProperties(0).Title);
                //List of the direction pairs
                //DO EACH DIRECTION IN THE ATTACK
                CardEventNames eventName = CardEventNames.None;
                string args = "";
                for (int i = 0; i < attackdir.Directions.Count; i++)
                {
                    eventName = attackdir.Directions[i].First;
                    args = attackdir.Directions[i].Second;
                    ///DoAttackStep(eventName, args);
                }

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                #region testing create Card
                /*
                //Graphics for Card tab
                Graphics grp = tabControl1.TabPages["tabPageDeck"].CreateGraphics();//testcard.Draw(grp);

                //Get the pokemon object and assign it to the Card
                Pokemon pokemonChar = Pokedex.GetPokemon(1);

                ///ATTACKING///
                Pair<Dictionary<string, AttackProperties>, bool> Attacks = 
                    new Pair<Dictionary<string, AttackProperties>, bool>(new  Dictionary<string, AttackProperties>(), false);
                //Add the Title and AttackProperties to the AttacksDictionary
                    
                CardDirections directions = new CardDirections();
                //The first attack is only one Energy and 10 damage points
                directions.Directions.Add(new Pair<CardEventNames,string>(CardEventNames.None, ""));
                Attacks.First.Add("Ram", new AttackProperties("Ram", PokemonTYPE.Colorless, 1, "", 10, directions));

                directions = new CardDirections();
                //The second attack needs one Grass and one Colorless Energy Card; 20 damage points
                directions.Directions.Add(new Pair<CardEventNames, string>(CardEventNames.FlipACoin, "NEXT"));
                directions.Directions.Add(new Pair<CardEventNames, string>(CardEventNames.AddDamageToAttack, "HEAD = +10"));
                
                AttackProperties GougeAttackProp = new AttackProperties("Gouge", 
                    PokemonTYPE.Grass, 
                    1, 
                    "Flip a coin. If heads, this attack does 20 damage plus 10 more damage.", 
                    20, 
                    directions);
                GougeAttackProp.Add("Gouge", PokemonTYPE.Colorless, 1);
                //now add it to Attacks object
                Attacks.First.Add("Gouge", GougeAttackProp);

                //Init weakness
                Pair<PokemonTYPE, int> Weakness = new Pair<PokemonTYPE, int>(PokemonTYPE.Psychic, 0);

                //Init resistance
                Pair<PokemonTYPE, int> Resistance = new Pair<PokemonTYPE, int>(PokemonTYPE.Null, 0);

                //Init Retreat cost
                Pair<PokemonTYPE, int> RetreatCost = new Pair<PokemonTYPE, int>(PokemonTYPE.Colorless, 1);

                //Create the Card + type, and properties and previously defined attacks
                Card testcard = new Card(Cardtype.Basic, pokemonChar, pokemonChar.GetPokemonTYPE(), 50, Attacks, Weakness, Resistance, RetreatCost);

                //Set the Location of the Card
                testcard.Panel.Location = new Point(238, 13);//testcard.Panel.Size = new System.Drawing.Size(190, 96);

                //Add the Card Panel to where you need to draw it
                tabControl1.TabPages["tabPageDeck"].Controls.Add(testcard.Panel);

                //ATTACK TESTING//
                //attach an energy card
                Card GrassEnergyCard = new Card(Cardtype.Energy);
                GrassEnergyCard.Properties.PokemonType = PokemonTYPE.Water;
                testcard.State.AttachedCards.Add(GrassEnergyCard);
                //attatch a second energycard
                Card FightingEnergyCard = new Card(Cardtype.Energy);
                FightingEnergyCard.Properties.PokemonType = PokemonTYPE.Fighting;
                testcard.State.AttachedCards.Add(FightingEnergyCard);
                //
                Card PsychicEnergyCard2 = new Card(Cardtype.Energy);
                PsychicEnergyCard2.Properties.PokemonType = PokemonTYPE.Psychic;
                testcard.State.AttachedCards.Add(PsychicEnergyCard2);

                List<AttackProperties> EnabledAttacks = new List<AttackProperties>();
                EnabledAttacks = testcard.IsCanAttack(testcard.State.AttachedCards);
                if (EnabledAttacks.Count > 0)
                {
                    foreach (AttackProperties attack in EnabledAttacks)
                    {
                        MessageBox.Show(testcard.Properties.Pokemon.Name + " can attack using " + attack.Title + " which causes " + attack.Damage.ToString() + " damage points!");
                    }
                }
                 */
                #endregion testing create Card

                #region testing loadxml
                /*
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.StartupPath + "\\cardsample.xml");
                Card testingcard = new Card(xmlDoc.FirstChild, Pokedex);

                //Set the Location of the Card
                testingcard.Panel.Location = new Point(238, 13);//testcard.Panel.Size = new System.Drawing.Size(190, 96);

                //Add the Card Panel to where you need to draw it
                tabControl1.TabPages["tabPageDeck"].Controls.Add(testingcard.Panel);
                //ATTACK TESTING//
                //attach an energy card
                Card GrassEnergyCard = new Card(Cardtype.Energy);
                GrassEnergyCard.Properties.PokemonType = PokemonTYPE.Grass;
                testingcard.State.AttachedCards.Add(GrassEnergyCard);

                Card LightningEnergyCard = new Card(Cardtype.Energy);
                LightningEnergyCard.Properties.PokemonType = PokemonTYPE.Lightning;
                testingcard.State.AttachedCards.Add(LightningEnergyCard);

                Card LightningEnergyCard2 = new Card(Cardtype.Energy);
                LightningEnergyCard2.Properties.PokemonType = PokemonTYPE.Lightning;
                testingcard.State.AttachedCards.Add(LightningEnergyCard2);

                List<AttackProperties> EnabledAttacks = new List<AttackProperties>();
                EnabledAttacks = testingcard.IsCanAttack(testingcard.State.AttachedCards);
                if (EnabledAttacks.Count > 0)
                {
                    foreach (AttackProperties attack in EnabledAttacks)
                    {
                        MessageBox.Show(testingcard.Properties.Pokemon.Name + " can attack using " + attack.Title + " which causes " + attack.Damage.ToString() + " damage points!");
                    }
                }
                */
                #endregion testing loadxml

                #region testingcard.ToXmlNode()
                /*
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(Application.StartupPath + "\\cardenergy.xml");
                xmlDoc.Load(Application.StartupPath + "\\cardsample.xml");
                //xmlDoc.Load(Common.xmlFolder + "\\cardsample.xml");
                Card testingcard = new Card(xmlDoc.FirstChild, Pokedex);

                //Set the Location of the Card
                testingcard.Panel.Location = new Point(238, 13);//testcard.Panel.Size = new System.Drawing.Size(190, 96);

                //Add the Card Panel to where you need to draw it
                tabControl1.TabPages["tabPageDeck"].Controls.Add(testingcard.Panel);

                List<AttackProperties> EnabledAttacks = new List<AttackProperties>();
                EnabledAttacks = testingcard.IsCanAttack(testingcard.State.AttachedCards);
                if (EnabledAttacks.Count > 0)
                {
                    foreach (AttackProperties attack in EnabledAttacks)
                    {
                        MessageBox.Show(testingcard.Properties.Pokemon.Name + " can attack using " + attack.Title + " which causes " + attack.Damage.ToString() + " damage points!");
                    }
                }
                //testingcard.ToXmlNode();
                 */
                #endregion testingcard.ToXmlNode()

                #region Testing Deck
                /*
                Deck LeafGreen = new Deck(Common.decksFolder + "\\Ex LeafGreen Starter.xml", Pokedex);

                //Add the Card Panel to where you need to draw it
                tabControl1.TabPages["tabPageDeck"].Controls.Add(LeafGreen.AllCards[0].Panel);
                 */
                #endregion Testing Deck

                #region testing
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("testToolStripMenuItem_Click error: " + ex.Message + "\r\n" + ex.StackTrace);
            }


        }

        private void importDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// When index changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cBoxAvailDecks_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //Clear all of the items in the Deck-Card list
            lBoxDeck.Items.Clear();

            //Create the current Deck
            SelectedDeck = new Deck(dirDeckFiles[cBoxAvailDecks.SelectedIndex], Pokedex);

            //Add each card's discription to the listbox
            foreach (Card card in SelectedDeck.AllCards)
            {
                lBoxDeck.Items.Add(card.Description);
            }
            //Display the deck count
            if (lBoxDeck.Items.Count == 60)
            {
                lCardCount.ForeColor = Color.Green;
                lCardCount.Text = "Cards in deck: " + lBoxDeck.Items.Count.ToString();
            }
            else
            {
                lCardCount.ForeColor = Color.Red;
                lCardCount.Text = "Cards in deck: " + lBoxDeck.Items.Count.ToString();
            }


            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// When index changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBoxDeck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gBoxAvailDecks.Controls.Count >= 5)
            {
                gBoxAvailDecks.Controls.RemoveAt(4);//remove the previous drawn Card
            }
            //Draw the selected card
            Card curCard = SelectedDeck.AllCards[lBoxDeck.SelectedIndex];
            curCard.Panel.Location = new Point(275, 25);
            //Add the Card Panel to where you need to draw it
            gBoxAvailDecks.Controls.Add(curCard.Panel);
            gBoxAvailDecks.Update();

            //Refresh the lBoxDeck.Update();
            lBoxDeck.Visible = true;
            lBoxDeck.Update();

            //Select this Pokemon on the Pokedex
            listBoxPokemon.SelectedItem = curCard.Properties.Pokemon.Name;
            
        }
        
        /// <summary>
        /// When the player's current listBoxCurrentHand SelectedIndexChanged
        /// Update the Pokedex selected index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxCurrentHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.ListBox)sender).SelectedIndex != -1)
            {
                //show this card in the Deck listbox
                lBoxDeck.SelectedItem = ((System.Windows.Forms.ListBox)sender).SelectedItem;
            }
        }

        /// <summary>
        /// If mouse click down on pbActivePokemon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbActivePokemon_MouseDown(object sender, MouseEventArgs e)
        {
            if (pbActivePokemon.CardState != CardHolder.CurrentState.Null)
            {
                //show this card in the Deck listbox
                lBoxDeck.SelectedItem = pbActivePokemon.CurrentCard.Description;
            }
            else
            {
                //There is no current card so do nothing
            }
        }

        #region MouseHovers
        #region pbActivePokemon
        /// <summary>
        /// If mouse hovers over pbActivePokemon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbActivePokemon_MouseHover(object sender, EventArgs e)
        {
            if (pbActivePokemon.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbActivePokemon.CurrentCard.State.AttachedCards.Count - 
                    pbActivePokemon.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbActivePokemon.CardName;
                toolTipGlobal.SetToolTip(pbActivePokemon, pbActivePokemon.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbActivePokemon.CurrentCard.Properties.HP.ToString() + "\r\nCondition: " +
                    pbActivePokemon.CurrentCard.State.CurrentCondition.ToString() + "\r\nDamage: " +
                    pbActivePokemon.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbActivePokemon.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Active Pokemon";
                toolTipGlobal.SetToolTip(pbActivePokemon, "No card placed");
            }
        }
        #endregion
        #region pbActivePokemonPC
        private void pbActivePokemonPC_MouseHover(object sender, EventArgs e)
        {
            if (pbActivePokemonPC.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbActivePokemonPC.CurrentCard.State.AttachedCards.Count -
                    pbActivePokemonPC.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbActivePokemonPC.CardName;
                toolTipGlobal.SetToolTip(pbActivePokemonPC, pbActivePokemonPC.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbActivePokemonPC.CurrentCard.Properties.HP.ToString() + "\r\nCondition: " +
                    pbActivePokemonPC.CurrentCard.State.CurrentCondition.ToString() + "\r\nDamage: " +
                    pbActivePokemonPC.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbActivePokemonPC.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Active Pokemon";
                toolTipGlobal.SetToolTip(pbActivePokemonPC, "No card placed");
            }
        }
        #endregion 
        #region listBoxPrizeCards
        private void listBoxPrizeCards_MouseHover(object sender, EventArgs e)
        {
            toolTipGlobal.ToolTipTitle = "Current Prize Cards";

            if (listBoxPrizeCards.Items.Count > 0)
            {
                toolTipGlobal.SetToolTip(listBoxPrizeCards, listBoxPrizeCards.Items.Count.ToString() + " prize cards");
            }
            else
            {
                toolTipGlobal.SetToolTip(listBoxPrizeCards, "No prize cards placed");
            }
        }
        #endregion
        #region pbDeck
        private void pbDeck_MouseHover(object sender, EventArgs e)
        {
            if (CurrentDeck.FaceDownList.Count > 0)
            {
                toolTipGlobal.ToolTipTitle = "Deck (Facedown)";
                toolTipGlobal.SetToolTip(pbDeck, "Cards: " + CurrentDeck.FaceDownList.Count.ToString());
            }
            else
            {
                toolTipGlobal.ToolTipTitle = "Deck (Facedown)";
                toolTipGlobal.SetToolTip(pbDeck, "No card placed");
            }
        }
        #endregion
        #region pbDeckPC
        private void pbDeckPC_MouseHover(object sender, EventArgs e)
        {
            if (PCdeck.FaceDownList.Count > 0)
            {
                toolTipGlobal.ToolTipTitle = "Deck (Facedown)";
                toolTipGlobal.SetToolTip(pbDeckPC, "Cards: " + PCdeck.FaceDownList.Count.ToString());
            }
            else
            {
                toolTipGlobal.ToolTipTitle = "Deck (Facedown)";
                toolTipGlobal.SetToolTip(pbDeckPC, "No card placed");
            }

        }
        #endregion
        #region pbBench1
        private void pbBench1_MouseHover(object sender, EventArgs e)
        {
            if (pbBench1.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBench1.CurrentCard.State.AttachedCards.Count -
                    pbBench1.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBench1.CardName;
                toolTipGlobal.SetToolTip(pbBench1, pbBench1.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBench1.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBench1.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBench1.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Bench1 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion
        #region pbBench2
        private void pbBench2_MouseHover(object sender, EventArgs e)
        {
            if (pbBench2.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBench2.CurrentCard.State.AttachedCards.Count -
                       pbBench2.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBench2.CardName;
                toolTipGlobal.SetToolTip(pbBench2, pbBench2.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBench2.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBench2.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBench2.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Bench2 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion pbBench2
        #region pbBench3
        private void pbBench3_MouseHover(object sender, EventArgs e)
        {
            if (pbBench3.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBench3.CurrentCard.State.AttachedCards.Count -
                          pbBench3.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBench3.CardName;
                toolTipGlobal.SetToolTip(pbBench3, pbBench3.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBench3.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBench3.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBench3.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Bench3 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion pbBench3
        #region pbBench4
        private void pbBench4_MouseHover(object sender, EventArgs e)
        {
            if (pbBench4.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBench4.CurrentCard.State.AttachedCards.Count -
                             pbBench4.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBench4.CardName;
                toolTipGlobal.SetToolTip(pbBench4, pbBench4.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBench4.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBench4.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBench4.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Bench4 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion pbBench4
        #region pbBench5
        private void pbBench5_MouseHover(object sender, EventArgs e)
        {
            if (pbBench5.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBench5.CurrentCard.State.AttachedCards.Count -
                                pbBench5.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBench5.CardName;
                toolTipGlobal.SetToolTip(pbBench5, pbBench5.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBench5.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBench5.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBench5.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "Bench5 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion pbBench4
        #region BenchPC1
        private void pbBenchPC1_MouseHover(object sender, EventArgs e)
        {
            if (pbBenchPC1.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBenchPC1.CurrentCard.State.AttachedCards.Count -
                                pbBenchPC1.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBenchPC1.CardName;
                toolTipGlobal.SetToolTip(pbBenchPC1, pbBenchPC1.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBenchPC1.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBenchPC1.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBenchPC1.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "BenchPC1 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion
        #region BenchPC2
        private void pbBenchPC2_MouseHover(object sender, EventArgs e)
        {
            if (pbBenchPC2.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBenchPC2.CurrentCard.State.AttachedCards.Count -
                                pbBenchPC2.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBenchPC2.CardName;
                toolTipGlobal.SetToolTip(pbBenchPC2, pbBenchPC2.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBenchPC2.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBenchPC2.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBenchPC2.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "BenchPC2 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion 
        #region BenchPC3
        private void pbBenchPC3_MouseHover(object sender, EventArgs e)
        {
            if (pbBenchPC3.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBenchPC3.CurrentCard.State.AttachedCards.Count -
                                pbBenchPC3.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBenchPC3.CardName;
                toolTipGlobal.SetToolTip(pbBenchPC3, pbBenchPC3.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBenchPC3.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBenchPC3.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBenchPC3.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "BenchPC3 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion
        #region BenchPC4
        private void pbBenchPC4_MouseHover(object sender, EventArgs e)
        {
            if (pbBenchPC4.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBenchPC4.CurrentCard.State.AttachedCards.Count -
                                pbBenchPC4.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBenchPC4.CardName;
                toolTipGlobal.SetToolTip(pbBenchPC4, pbBenchPC4.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBenchPC4.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBenchPC4.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBenchPC4.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "BenchPC4 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion
        #region BenchPC5
        private void pbBenchPC5_MouseHover(object sender, EventArgs e)
        {
            if (pbBenchPC5.CardState != CardHolder.CurrentState.Null)
            {
                int iNonenergycount = (pbBenchPC5.CurrentCard.State.AttachedCards.Count -
                                pbBenchPC5.CurrentCard.State.GetAttachedEnergyCardCount());
                toolTipGlobal.ToolTipTitle = pbBenchPC5.CardName;
                toolTipGlobal.SetToolTip(pbBenchPC5, pbBenchPC5.CurrentCard.Properties.CardType.ToString() + "\r\nHP: " +
                    pbBenchPC5.CurrentCard.Properties.HP.ToString() + "\r\nDamage: " +
                    pbBenchPC5.CurrentCard.State.CurrentDamage.ToString() + "\r\nAttached Non-Energy Cards: " +
                    iNonenergycount.ToString() + "\r\nAttached Energy Cards: " +
                    pbBenchPC5.CurrentCard.State.GetAttachedEnergyCardCount().ToString());
            }
            else
            {
                toolTipGlobal.RemoveAll();
                toolTipGlobal.ToolTipTitle = "BenchPC5 Pokemon";
                toolTipGlobal.SetToolTip(pbBench1, "No card placed");
            }
        }
        #endregion
        #region listBoxCurrentHand
        private void listBoxCurrentHand_MouseHover(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.ListBox)sender).SelectedIndex != -1)
            {
                toolTipGlobal.ToolTipTitle = "Current Cards in Hand";

                if (listBoxCurrentHand.Items.Count > 0)
                {
                    toolTipGlobal.SetToolTip(listBoxCurrentHand, listBoxCurrentHand.Items.Count.ToString() + " cards");
                }
                else
                {
                    toolTipGlobal.SetToolTip(listBoxCurrentHand, "No cards in hand.");
                }
            }
        }
        #endregion
        #endregion

        #region Coin Flip on Check Changed
        private void checkBoxHeads_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeads.Checked == true)
                checkBoxTails.Checked = false;
            else if (checkBoxHeads.Checked == false)
                checkBoxTails.Checked = true;
        }

        private void checkBoxTails_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTails.Checked == true)
                checkBoxHeads.Checked = false;
            else if (checkBoxTails.Checked == false)
                checkBoxHeads.Checked = true;
        }

        /// <summary>
        /// Flips the first coin to see who goes first
        /// </summary>
        private void FlipTheFirstCoin()
        {
            if (game.State.Round == 1)
            {
                //We are still just starting
                btPlayHandButton.Text = "Flip a Coin";
                toolStripStatusLabel1.Text = "Flip a coin on the Play Hand tab.";

                //Display the Heads and Tails checkboxes and select a default
                checkBoxHeads.Visible = true;
                checkBoxTails.Visible = true;
                pBoxPlayHand.Visible = true;
                checkBoxHeads.Checked = true;
                checkBoxTails.Checked = false;
            }
        }

        #endregion

        /// <summary>
        /// Try to start a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startNewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goto Deck tab
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageDeck"];
            //Ask Player if these are the decks to use
            DialogResult dResult = MessageBox.Show("Are you sure you want to use these Decks?", "Accept Decks", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dResult == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                //Get the current selected deck for player1 and PC
                PCdeck.Description = "Your opponent's deck: " + PCdeck.Description;
                CurrentDeck.Description = "Your deck: " + CurrentDeck.Description;
                this.Cursor = Cursors.Default;
                //Select opponent's deck
                //2 Shuffle player1's deck
                //3 Draw 7 cards into player1's hand
                if (game.StartGame(this.Pokedex, CurrentDeck, PCdeck, pbActivePokemon, pbBench1, pbBench2, pbBench3, pbBench4, pbBench5,
                    pbActivePokemonPC, pbBenchPC1, pbBenchPC2, pbBenchPC3, pbBenchPC4, pbBenchPC5))
                {
                    //**Display the inhand in the Play Hand tab **Handled by game handler since this is an event
                    //game.StartGame delt the cards so show the image for the deck-facedown
                    pbDeck.Image = new System.Drawing.Bitmap(PPokemon.Properties.Resources.CardBack_sm);
                    pbDeckPC.Image = new System.Drawing.Bitmap(PPokemon.Properties.Resources.CardBack_sm);

                    //4 Select an Active Pokemon
                    btPlayHandButton.Text = "Select an Active Pokemon";
                    toolStripStatusLabel1.Text = "Select an Active Pokemon";
                    //Autoselect the first item in the listboxhandlist so it does crash when nothing is selected
                    listBoxCurrentHand.SelectedIndex = 0;

                    //5 Choose Basic Pokemon to Bench, face down
                    //6 Set aside Prize cards
                    //7 Flip a coin to see how goes first

                    //8 Reveal all Active and Benched Pokemon
                }
                else
                    MessageBox.Show("Can not start can due to bad deck count.");
            }
            else if (dResult == DialogResult.No)
            {
                MessageBox.Show("Not Starting Game so you may rechoose the decks.");
                btSelectDeck.Text = "Play With This Deck";

            }
            else if (dResult == DialogResult.Cancel)
            {
                btSelectDeck.Text = "Play With This Deck";
            }

        }

        /// <summary>
        /// Place a card to the Player or AIplayer's bench.
        /// Does remove any cards from anylists or dictionaries
        /// Fires:PlayHandChanged, BenchPokemonChoosen for GamePlay.Player.PLAYER
        /// </summary>
        /// <param name="card">Card to place</param>
        /// <param name="activeplayer">GamePlay.Player</param>
        private bool PlaceCardToBench(Card card, GamePlay.Player activeplayer)
        {
            //DEBUGGING
            if (card.Description == "")
                MessageBox.Show("TrACE BACK HERE");
            bool placedcard = false;
            if (activeplayer == PPokemon.GamePlay.Player.PLAYER)
            {
                #region PLAYER
                if (!pbBench1.HasItem)
                {
                    pbBench1.PlaceCard(card);
                    CurrentDeck.BenchDict.Add(pbBench1.CurrentCard);
                    placedcard = true;
                }
                else if (!pbBench2.HasItem)
                {
                    pbBench2.PlaceCard(card);
                    CurrentDeck.BenchDict.Add(pbBench2.CurrentCard);
                    placedcard = true;
                }
                else if (!pbBench3.HasItem)
                {
                    pbBench3.PlaceCard(card);
                    CurrentDeck.BenchDict.Add(pbBench3.CurrentCard);
                    placedcard = true;
                }
                else if (!pbBench4.HasItem)
                {
                    pbBench4.PlaceCard(card);
                    CurrentDeck.BenchDict.Add(pbBench4.CurrentCard);
                    placedcard = true;
                }
                else if (!pbBench5.HasItem)
                {
                    pbBench5.PlaceCard(card);
                    CurrentDeck.BenchDict.Add(pbBench5.CurrentCard);
                    placedcard = true;
                }
                else
                {
                    //Add the "End phase" option
                    lBoxTurnOptions.Items.Add("End phase 2 of turn");//Placecard to bench??follow this out
                    lBoxTurnOptions.SelectedItem = "End phase 2 of turn";
                    lBoxTurnOptions.Update();
                    btPlayHandButton.Text = "End phase 2 of turn";
                    btPlayHandButton.Update();
                    MessageBox.Show("Bench is full. Can not add anymore Pokemon to bench.");//Selection is disapearing and user is stuck
                    placedcard = false;
                }

                //MUST remove the card first before checking hand for basics
                //If a card was placed then update the HandList and listbox
                if (placedcard)
                {
                    //This card is now out of your hand, so remove it
                    CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                    GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                    //We also BenchPokemonChoosen so fire event 
                    GameEventHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.PLAYER);
                }
                #endregion
            }
            else if (activeplayer == PPokemon.GamePlay.Player.AIPLAYER)
            {
                #region AIPLAYER
                //If the selected card is a basic, then Place it in an open bench spot
                if (card.Properties.CardType == Cardtype.Basic)
                {
                    if (!pbBenchPC1.HasItem)
                    {
                        pbBenchPC1.PlaceCard(card);
                        PCdeck.BenchDict.Add(pbBenchPC1.CurrentCard);
                        PCdeck.HandList.Remove(card);//Remove since it is now in play
                        placedcard = true;
                    }
                    else if (!pbBenchPC2.HasItem)
                    {
                        pbBenchPC2.PlaceCard(card);
                        PCdeck.BenchDict.Add(pbBenchPC2.CurrentCard);
                        PCdeck.HandList.Remove(card);//Remove since it is now in play
                        placedcard = true;
                    }
                    else if (!pbBenchPC3.HasItem)
                    {
                        pbBenchPC3.PlaceCard(card);
                        PCdeck.BenchDict.Add(pbBenchPC3.CurrentCard);
                        PCdeck.HandList.Remove(card);//Remove since it is now in play
                        placedcard = true;
                    }
                    else if (!pbBenchPC4.HasItem)
                    {
                        pbBenchPC4.PlaceCard(card);
                        PCdeck.BenchDict.Add(pbBenchPC4.CurrentCard);
                        PCdeck.HandList.Remove(card);//Remove since it is now in play
                        placedcard = true;
                    }
                    else if (!pbBenchPC5.HasItem)
                    {
                        pbBenchPC5.PlaceCard(card);
                        PCdeck.BenchDict.Add(pbBenchPC5.CurrentCard);
                        PCdeck.HandList.Remove(card);//Remove since it is now in play
                        placedcard = true;
                    }

                }
                #endregion
            }
            return placedcard;
        }

        /// <summary>
        /// Turns on and off the visibility of the Player1 controls on relating to the TurnPhase2 and 3 PlayHand
        /// </summary>
        /// <param name="turnon">true if visible, false to hide controls</param>
        private void TurnOnPlayHandControls(bool turnon)
        {
            if (turnon)
            {
                lbPlayHandMessage.Visible = true;
                lbPlayHandTurn.Visible = true;
                lBoxTurnOptions.Items.Clear();
                lBoxTurnOptions.Visible = true;
                lbFlipResult.Text = game.State.CurrentTurnPhase.ToString();
                lbFlipResult.Visible = true;
            }
            else
            {
                //Hide the combobox
                lBoxTurnOptions.Visible = false;
                //Hide and Clear lbPlayHandMessage
                lbPlayHandMessage.Text = "";
                lbPlayHandMessage.Visible = false;
                lbFlipResult.Visible = false;
            }
        }

        /// <summary>
        /// Controls the inputs needed from the User/Player1
        /// Depending upon the current text of the button, do it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPlayHandButton_Click(object sender, EventArgs e)
        {
            if (lBoxTurnOptions.SelectedIndex != null)
            {
                if (btPlayHandButton.Text == "Select an Active Pokemon")
                {
                    #region Select an Active Pokemon
                    //Check to see if it is a Basic on first Round of Play
                    if (game.State.Round == 1)
                    {
                        #region Player1
                        //If the selected card is a basic, then Place it in the active pokemon CardHolder
                        if (CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex].Properties.CardType == Cardtype.Basic)
                        {
                            //Place the card facedown
                            pbActivePokemon.PlaceCard(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            //Set the CurrentCard.State.ActivePokemon
                            pbActivePokemon.CurrentCard.State.ActivePokemon = true;

                            //This card is now out of your hand, so remove it
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);//Is removing from hand??
                            GameEventHook(GamePlay.GameHooks.ActivePokemonChoosen, GamePlay.Player.PLAYER);

                            #region AIPlayer
                            //OK for AIPlayer to choose ActivePokemon and bench now that Player1 chose a valid choice
                            InitAIPlayerSetup();
                            #endregion

                        }
                        else
                        {
                            //No Basic card selected
                            MessageBox.Show("Play has not started yet and you must choose a Basic Pokemon. Please try again.");
                        }
                        #endregion
                    }
                    else
                    {
                        //Past Round 1 and selecting a new Pokemon
                    }
                    #endregion
                }
                else if (btPlayHandButton.Text == "Select Basic to Bench")
                {
                    #region Select Basic to Bench
                    //Check to see if it is a Basic on first Round of Play
                    if (game.State.Round == 1)
                    {
                        if (listBoxCurrentHand.SelectedIndex == -1)
                            listBoxCurrentHand.SelectedIndex = 0;
                        //If the selected card is a basic, then Place it in an open bench spot
                        if (CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex].Properties.CardType == Cardtype.Basic)
                        {
                            PlaceCardToBench(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex], PPokemon.GamePlay.Player.PLAYER);

                            //If there is a Basic card in your hand, make sure the button is still enabled
                            if (CurrentDeck.IsBasicInHand())
                            {
                                //Change the button
                                btPlayHandButton.Text = "Select Basic to Bench";
                            }
                            else
                            {
                                //There are no more Basics in your hand
                                FlipTheFirstCoin();
                            }
                        }
                    }
                    #endregion
                }
                else if (btPlayHandButton.Text == "Flip a Coin")
                {
                    game.FlipACoin();
                }
                else if (btPlayHandButton.Text == "OK\r\nReveal all Pokemon")
                {
                    //This occurs when Player1 presses no matter whose turn it is
                    #region Reveal all Pokemon
                    //Hide the checkboxs
                    checkBoxHeads.Visible = false;
                    checkBoxTails.Visible = false;
                    this.lbFlipResult.Text = "";
                    this.lbFlipResult.Visible = false;
                    pBoxPlayHand.Visible = false;

                    //Turn over Player1 Cards
                    pbActivePokemon.TurnCardFaceUp();
                    pbBench1.TurnCardFaceUp();
                    pbBench2.TurnCardFaceUp();
                    pbBench3.TurnCardFaceUp();
                    pbBench4.TurnCardFaceUp();
                    pbBench5.TurnCardFaceUp();

                    //Turn over AIPlayer Cards
                    pbActivePokemonPC.TurnCardFaceUp();
                    pbBenchPC1.TurnCardFaceUp();
                    pbBenchPC2.TurnCardFaceUp();
                    pbBenchPC3.TurnCardFaceUp();
                    pbBenchPC4.TurnCardFaceUp();
                    pbBenchPC5.TurnCardFaceUp();

                    //Forward to the PlayMat
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPagePlayMat"];
                    #endregion

                    //We are now in the Draw a Card Phase
                    game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;

                    //Decide whose move to start
                    if (game.State.CurrentPlayer == PPokemon.GamePlay.Player.PLAYER)
                    {
                        //Enable the 'Draw a Card' Button
                        btDrawACard.Enabled = true;
                        //Also on PlayHand
                        btPlayHandButton.Text = "Draw a Card";
                        toolStripStatusLabel1.Text = "Draw a Card";
                    }
                    else
                    {
                        ///////////////////////////////////////
                        //Must be AIPlayer's begining of turn//
                        ///////////////////////////////////////
                        tbAIPlayerLog.Visible = true;
                        toolStripStatusLabel1.Text = "AIPlayer's turn...";
                        tbAIPlayerLog.AppendText(toolStripStatusLabel1.Text + "Round: " + game.State.Round.ToString() + "\r\n");
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;
                        PCdeck.TakeCard(true);
                        tbAIPlayerLog.AppendText("Took Card from top of deck.\r\n");
                        GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        GameEventHook(PPokemon.GamePlay.GameHooks.DrewACard, GamePlay.Player.AIPLAYER);
                        //The rest happens in the eventhandler
                    }
                }
                else if (btPlayHandButton.Text == "Draw a Card")
                {
                    btDrawACard_Click(sender, e);
                }
                else if (btPlayHandButton.Text == "TurnPhase 2 do it!")
                {
                    //Make sure listBoxCurrentHand IS selected correctly and not -1
                    if (listBoxCurrentHand.SelectedIndex == -1)
                    {
                        //See if the listbox has items
                        if (listBoxCurrentHand.Items.Count > 0)
                        {
                            if (!SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                                listBoxCurrentHand.SelectedIndex = 0;
                        }
                    }

                    #region TurnPhase 2 Command, asking to do something in listbox
                    if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Put on Bench::"))
                    {
                        //OnSelectChanged should have updated the listbox so we can use that
                        if (PlaceCardToBench(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex], PPokemon.GamePlay.Player.PLAYER))
                        {

                            //Remove this from dropdown list
                            if (((string)lBoxTurnOptions.SelectedItem) != "End phase 2 of turn")
                                lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            else
                                lBoxTurnOptions.Items.RemoveAt(0);

                            GameEventHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                        {
                            //Could not add the Card to the Bench, so leave it in the HandList and update its listbox
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Attach EnergyCard to ActivePokemon::"))
                    {
                        #region Attach EnergyCard to ActivePokemon::
                        int listboxselectedItem = listBoxCurrentHand.Items.IndexOf(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString()));
                        //Attach this Energy Card to ActivePokemon
                        pbActivePokemon.CurrentCard.State.AttachedCards.Add(CurrentDeck.HandList[listboxselectedItem]);
                        //Log this attached energy card, so no more
                        game.State.AttachedEnergy = true;
                        //Remove this Energy Card from HandList
                        CurrentDeck.HandList.Remove(CurrentDeck.HandList[listboxselectedItem]);
                        listBoxCurrentHand.Refresh();
                        listBoxCurrentHand.SelectedIndex = 0;//force a redraw update
                        listBoxCurrentHand.Update();
                        lBoxTurnOptions.Items.Remove(lBoxTurnOptions.SelectedItem.ToString());
                        //               lBoxTurnOptions.SelectedIndex = 0;
                        lBoxTurnOptions.Update();
                        GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                        GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        #endregion
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve ActivePokemon to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbActivePokemon.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve Bench1 to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbBench1.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve Bench2 to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbBench2.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve Bench3 to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbBench3.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve Bench4 to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbBench4.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Evolve Bench5 to::"))
                    {
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            pbBench5.Evolve(CurrentDeck.HandList[listBoxCurrentHand.SelectedIndex]);
                            CurrentDeck.HandList.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            listBoxCurrentHand.Items.RemoveAt(listBoxCurrentHand.SelectedIndex);
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                        else
                            MessageBox.Show("Please Select the Card you want to Evolve from the Current Hand List and try again.");
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Play Trainer::") || 
                        lBoxTurnOptions.SelectedItem.ToString().StartsWith("Play Supporter::"))
                    {
                        //Select this Card in the listbox
                        if (SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString())))
                        {
                            AttackProperties[] attack = new Cards.AttackProperties[5];
                            Card trainerCard = new Card(Cardtype.Trainer);
                            int selectedindex = listBoxCurrentHand.SelectedIndex;
                            trainerCard = CurrentDeck.HandList[selectedindex];
                            //Copy this Card's AttackProperties to attack so it can be passed to ProcessAttack
                            trainerCard.Properties.Attacks.First.Values.CopyTo(attack, 0);
                            MessageBox.Show(game.ProcessAttack(pbActivePokemon, pbActivePokemonPC, attack[0], true).ToString());//Process Trainer
                            //Add this Trainer card to the discard pile
                            CurrentDeck.DiscardList.Add(trainerCard);
                            //Remove from HandList
                            CurrentDeck.HandList.Remove(trainerCard);
                            listBoxCurrentHand.Items.RemoveAt(selectedindex);//Sometimes is -1
                            lBoxTurnOptions.Items.RemoveAt(lBoxTurnOptions.SelectedIndex);
                            lBoxTurnOptions.SelectedItem = 0;
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                            GameEventHook(GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.PLAYER);
                        }
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("Play Staduim::"))
                    {
                    }
                    else if (lBoxTurnOptions.SelectedItem.ToString() == "End phase 2 of turn")
                    {
                        //Are there Attacks to do?? 
                        List<AttackProperties> attackp = pbActivePokemon.CurrentCard.IsCanAttack(pbActivePokemon.CurrentCard.State.AttachedCards);
                        if (attackp.Count > 0)
                        {
                            //There are attacks, so prompt the user which attack to use
                            foreach (AttackProperties attackprops in attackp)
                            {
                                lBoxTurnOptions.Items.Add("Attack with::" + attackprops.Title);
                            }
                        }
                        //Prep for Attacking
                        btPlayHandButton.Text = "Attack!";
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase3;
                    }
                    else
                        MessageBox.Show("Not finished yet");
                    #endregion

                }
                else if (btPlayHandButton.Text == "Attack!")
                {
                    #region Attack by Player1
                    //Not developed
                    if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhase3)
                    {
                        //Can only attack if not Paralyzed or Asleep
                        if (pbActivePokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Paralyzed)
                        {
                            MessageBox.Show(pbActivePokemon.CurrentCard.Properties.Pokemon.Name + " Paralyzed! Can not attack. It will recover next turn.");
                            game.EndOfTurn(PPokemon.GamePlay.Player.PLAYER, pbActivePokemon);
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayerChange, PPokemon.GamePlay.Player.PLAYER);
                        }
                        else if (pbActivePokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Asleep)
                        {
                            MessageBox.Show(pbActivePokemon.CurrentCard.Properties.Pokemon.Name + " Asleep! Can not attack. You will flip a coin at end of turn.");
                            DoPlayer1Attack();//checks for Asleep???
                        }
                        else if (pbActivePokemon.CurrentCard.State.CurrentCondition == SpecialConditions.Confused)
                        {
                            //ActivePokemon is Confused, so must flip coin
                            if (game.FlipACoin() == PPokemon.GamePlay.Coin.HEADS)
                            {
                                GameEventHook(PPokemon.GamePlay.GameHooks.FlippedCoin, GamePlay.Player.PLAYER);
                                //HEADS! Attack works normally
                                MessageBox.Show(pbActivePokemon.CurrentCard.Properties.Pokemon.Name + " attack will work normally!");
                                DoPlayer1Attack();
                            }
                            else
                            {
                                GameEventHook(PPokemon.GamePlay.GameHooks.FlippedCoin, GamePlay.Player.PLAYER);
                                //Tails so receive three damage counters = +30
                                MessageBox.Show(pbActivePokemon.CurrentCard.Properties.Pokemon.Name + " Confused Attack! +30 Damage!");
                                pbActivePokemon.CurrentCard.State.CurrentDamage = pbActivePokemon.CurrentCard.State.CurrentDamage + 30;
                                //Check to see if Pokemon KnockedOut
                                if (pbActivePokemon.CurrentCard.State.CurrentDamage >= pbActivePokemon.CurrentCard.Properties.HP)
                                {
                                    MessageBox.Show(pbActivePokemon.CurrentCard.Properties.Pokemon.Name + " is Knocked Out!!");
                                    game.KnockOut(pbActivePokemon, GamePlay.Player.PLAYER);
                                    GameEventHook(PPokemon.GamePlay.GameHooks.PokemonKnockedOut, GamePlay.Player.PLAYER);
                                }
                                //Change the button text, etc
                                game.EndOfTurn(PPokemon.GamePlay.Player.PLAYER, pbActivePokemon);
                                GameEventHook(PPokemon.GamePlay.GameHooks.PlayerChange, PPokemon.GamePlay.Player.PLAYER);
                            }

                        }
                        else
                            DoPlayer1Attack();
                    }
                    #endregion
                }
                else if (btPlayHandButton.Text == "Start\r\nAIPlayer's Turn")
                {
                    ///////////////////////////////////////
                    //Must be AIPlayer's begining of turn//
                    ///////////////////////////////////////
                    tbAIPlayerLog.Visible = true;
                    toolStripStatusLabel1.Text = "AIPlayer's turn...";
                    tbAIPlayerLog.AppendText(toolStripStatusLabel1.Text + "Round: " + game.State.Round.ToString() + "\r\n");
                    game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;
                    PCdeck.TakeCard(true);
                    tbAIPlayerLog.AppendText("Took Card from top of deck.\r\n");
                    GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                    GameEventHook(PPokemon.GamePlay.GameHooks.DrewACard, GamePlay.Player.AIPLAYER);
                    //The rest happens in the eventhandler
                }
                else if (btPlayHandButton.Text == "Start\r\nTurn")
                {
                    if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhaseStart && game.State.CurrentPlayer == PPokemon.GamePlay.Player.PLAYER)
                    {
                        //Enable the 'Draw a Card' Button
                        btDrawACard.Enabled = true;
                        //Also on PlayHand
                        btPlayHandButton.Text = "Draw a Card";
                        toolStripStatusLabel1.Text = "Draw a Card";
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;
                    }
                    if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhaseStart && game.State.CurrentPlayer == PPokemon.GamePlay.Player.AIPLAYER)
                    {
                        ///////////////////////////////////////
                        //Must be AIPlayer's begining of turn//
                        ///////////////////////////////////////
                        tbAIPlayerLog.Visible = true;
                        toolStripStatusLabel1.Text = "AIPlayer's turn...";
                        tbAIPlayerLog.AppendText(toolStripStatusLabel1.Text + "Round: " + game.State.Round.ToString() + "\r\n");
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;
                        PCdeck.TakeCard(true);
                        tbAIPlayerLog.AppendText("Took Card from top of deck.\r\n");
                        GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        GameEventHook(PPokemon.GamePlay.GameHooks.DrewACard, GamePlay.Player.AIPLAYER);
                        //The rest happens in the eventhandler
                    }

                }
                else if (btPlayHandButton.Text == "New ActivePokemon")
                {
                    //New Active Pokemon can only be from the bench, so move bench players to active
                    //  Must Keep damage and other conditions in move. Also do this for AIPlayer
                    if (lBoxTurnOptions.SelectedItem.ToString().StartsWith("New ActivePokemon::"))
                    {
                        string carddescript = Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString());
                        //Place as active pokemon
                        //SelectlistboxHandUsingName(Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString()));
                        foreach (CardHolder chitem in game.State.Player1Pokemon)
                        {
                            if (chitem.HasItem)
                            {
                                //Only read benches
                                if (!chitem.CurrentCard.State.ActivePokemon)
                                {
                                    if (chitem.CurrentCard.Description == carddescript)
                                    {
                                        pbActivePokemon.PlaceCard(chitem.CurrentCard);
                                        pbActivePokemon.TurnCardFaceUp();
                                        RemoveCardFromBenchCardHolder(chitem.CurrentCard, PPokemon.GamePlay.Player.PLAYER);
                                        break;
                                    }
                                }
                            }
                        }
                            
                        //Set the CurrentCard.State.ActivePokemon
                        pbActivePokemon.CurrentCard.State.ActivePokemon = true;

                        //RemoveCardFromBenchCardHolder(pbActivePokemon.CurrentCard, GamePlay.Player.PLAYER);
                        //This card is now out of the bench, so remove it
                        CurrentDeck.BenchDictRefresh(PPokemon.GamePlay.Player.PLAYER, game.State);

                        btPlayHandButton.Text = "Start\r\nTurn";
                        toolStripStatusLabel1.Text = "Start\r\nTurn";
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhaseStart;
                        GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                        GameEventHook(GamePlay.GameHooks.ActivePokemonChoosen, GamePlay.Player.PLAYER);
                    }
                }
            }
        }

        /// <summary>
        /// Get the selected AttackProperty and ProcessAttack, then EndOfTurn
        /// (Only for Player1)
        /// </summary>
        public void DoPlayer1Attack()
        {
            if (lBoxTurnOptions.Items.Count > 0)
            {
                //Get the AttackProperty choosen
                AttackProperties attack = new AttackProperties();
                attack = Ops.GetAttackFromCardHolder(pbActivePokemon, Ops.GetListBoxCardDescription(lBoxTurnOptions.SelectedItem.ToString()));
                MessageBox.Show(attack.Body + "\r\n-------\r\n" + game.ProcessAttack(pbActivePokemon, pbActivePokemonPC, attack, false).ToString(), attack.Title);
            }
            else
                MessageBox.Show("No current attacks!");

            game.EndOfTurn(PPokemon.GamePlay.Player.PLAYER, pbActivePokemon);
            GameEventHook(PPokemon.GamePlay.GameHooks.PlayerChange, PPokemon.GamePlay.Player.PLAYER);
        }

        /// <summary>
        /// Select the AIPlayer basic Pokemon and add any Pokemon to its bench
        /// then Fire PlayHandChanged and ActivePokemonChoosen GameEventHooks
        /// </summary>
        public void InitAIPlayerSetup()
        {
            if (game.State.Round == 1)
            {
                #region AIPlayer
                //AutoSelect all of the basics
                List<Card> PCBasics = new List<Card>();
                PCBasics = PCdeck.GetAllBasicsInHand();
                //Since Sort not working, just add the first 
                pbActivePokemonPC.PlaceCard(PCBasics[0]);//Place the first Basic
                pbActivePokemonPC.CurrentCard.State.ActivePokemon = true;
                PCdeck.HandList.Remove(PCBasics[0]);
                PCBasics.RemoveAt(0);//Remove so we can easily iterate bench
                if (PCBasics.Count >= 1)
                {
                    PCdeck.BenchDict.AddRange(PCBasics.GetRange(0, PCBasics.Count));
                }
                GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                GameEventHook(GamePlay.GameHooks.ActivePokemonChoosen, GamePlay.Player.AIPLAYER);
                #endregion
            }
        }

        /// <summary>
        /// Draw a card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDrawACard_Click(object sender, EventArgs e)
        {
            if (game.State.CurrentPlayer == PPokemon.GamePlay.Player.PLAYER)
            {
                string newcard = CurrentDeck.TakeCard(true).Description;
                GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);

                lbPlayHandMessage.Visible = true;
                lbPlayHandMessage.Text = "You drew " + newcard;
                lBoxTurnOptions.Visible = true;
                //Forward to the PlayMat
                tabControl1.SelectedTab = tabControl1.TabPages["tabPagePlayHand"];
                //Alert Form1 that Player drew a card (TurnPhase1)
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase1;
                GameEventHook(PPokemon.GamePlay.GameHooks.DrewACard, GamePlay.Player.PLAYER);
            }
            else
                btDrawACard.Enabled = false;//Hey disable this button since not Player's turn
        }

        /// <summary>
        /// Selects listBoxCurrentHand.SelectedIndex if found, returns false if not found
        /// </summary>
        /// <param name="pokemonname"></param>
        /// <returns>returns false if not found</returns>
        private bool SelectlistboxHandUsingName(string pokemonname)
        {
            bool foundindex = false;

            foreach (String stritem in listBoxCurrentHand.Items)
            {
                if (stritem.ToLower().Contains(pokemonname.ToLower()))
                {
                    listBoxCurrentHand.SelectedIndex = listBoxCurrentHand.Items.IndexOf(stritem);
                    foundindex = true;
                    break;
                }
            }
            return foundindex;
        }

        /// <summary>
        /// Removes a Card from the player's cardholder on their bench
        /// by matching Card.Description
        /// </summary>
        /// <param name="oldbenchcard">Card</param>
        /// <param name="player">GamePlay.Player</param>
        private void RemoveCardFromBenchCardHolder(Card oldbenchcard, GamePlay.Player player)
        {
            bool found = false;
            if (player == PPokemon.GamePlay.Player.PLAYER)
            {
                while (!found)
                {
                    if (pbBench1.HasItem)
                    {
                        if (pbBench1.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBench1.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBench2.HasItem)
                    {
                        if (pbBench2.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBench2.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBench3.HasItem)
                    {
                        if (pbBench3.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBench3.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBench4.HasItem)
                    {
                        if (pbBench4.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBench4.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBench5.HasItem)
                    {
                        if (pbBench5.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBench5.RemoveItem(); found = true; break;
                        }
                    }
                }
            }
            else
            {
                while (!found)
                {
                    if (pbBenchPC1.HasItem)
                    {
                        if (pbBenchPC1.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBenchPC1.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBenchPC2.HasItem)
                    {
                        if (pbBenchPC2.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBenchPC2.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBenchPC3.HasItem)
                    {
                        if (pbBenchPC3.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBenchPC3.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBenchPC4.HasItem)
                    {
                        if (pbBenchPC4.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBenchPC4.RemoveItem(); found = true; break;
                        }
                    }
                    if (pbBenchPC5.HasItem)
                    {
                        if (pbBenchPC5.CurrentCard.Description == oldbenchcard.Description)
                        {
                            pbBenchPC5.RemoveItem(); found = true; break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// When an action is choosen, Select it in the Handlistbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lBoxTurnOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
            {
                if (((ListBox)sender).SelectedItem.ToString().Trim() != "End phase 2 of turn")
                {
                    listBoxCurrentHand.SelectedItem = listBoxCurrentHand.Items.IndexOf(Ops.GetListBoxCardDescription(((ListBox)sender).SelectedItem.ToString().Trim())); ;
                }
            }
            else
            {
                if(listBoxCurrentHand.Items.Count > 0)
                    listBoxCurrentHand.SelectedItem = listBoxCurrentHand.Items.Count - 1;
            }
        }

        /// <summary>
        /// Selects the Decks for the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSelectDeck_Click(object sender, EventArgs e)
        {
            if (SelectedDeck.Count == 60)
            {
                this.Cursor = Cursors.WaitCursor;
                if (btSelectDeck.Text == "Play With This Deck")
                {
                    //Choosing Player1's Deck
                    CurrentDeck = SelectedDeck.CreateNewDeck();
                    btSelectDeck.Text = "Select AIPlayer's Deck";
                    gBoxAvailDecks.Text = "Select AIPlayer's Deck";
                    cBoxAvailDecks.SelectedIndex = 0;//reset

                }
                else if (btSelectDeck.Text == "Select AIPlayer's Deck")
                {
                    PCdeck = SelectedDeck.CreateNewDeck();
                    startNewGameToolStripMenuItem.Enabled = true;
                    cBoxAvailDecks.SelectedIndex = 0;//reset
                    btSelectDeck.Text = "Start Game";
                }
                else
                {
                    startNewGameToolStripMenuItem_Click(sender, e);
                }

                this.Cursor = Cursors.Default;
            }
            else
                MessageBox.Show("Can not select this deck because it has " + SelectedDeck.Count.ToString() + " cards.");
        }

        private void selectDecksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goto Deck tab
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageDeck"];
        }

        private void listBoxCurrentHand_DoubleClick(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.ListBox)sender).SelectedIndex != -1)
            {
                //Draw the selected card
                Card curCard = CurrentDeck.GetCardByDescription(((System.Windows.Forms.ListBox)sender).SelectedItem.ToString());
                CardPopUpForm cardform = new CardPopUpForm();
                curCard.Panel.Location = new Point(1, 1);
                //Add the Card Panel to where you need to draw it
                cardform.Controls.Add(curCard.Panel);
                cardform.Tag = new CardHolder();//Cards in hand have no CardHolders
                cardform.DisplayCurrentStatus();
                cardform.Show();
            }
        }

        /// <summary>
        /// Subscribed method to GameEngine.HookHandler
        /// Process the sent message from GameEngine
        /// </summary>
        /// <param name="message">GamePlay.GameHooks parameter</param>
        public void GameEventHook(GamePlay.GameHooks message, object args)
        {
            #region No more moves? Tell user
            if (game.State.CurrentPlayer != PPokemon.GamePlay.Player.AIPLAYER)
            {
                if (game.State.CurrentTurnPhase != PPokemon.GamePlay.TurnPhase.TurnPhaseStart)
                {
                    if ((lBoxTurnOptions.Items.Count) == 0 && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
                    {
                        //Tell the user there are no more moves, so let aiplayer do turn
                        btPlayHandButton.Text = "Start\r\nAIPlayer's Turn";
                        btPlayHandButton.Update();
                    }
                }
            }
            #endregion

            #region PlayHandChanged, reupdate listBoxCurrentHand and go there
            if ((message == PPokemon.GamePlay.GameHooks.PlayHandChanged) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                //Update the playhand listbox
                listBoxCurrentHand.Items.Clear();
                listBoxCurrentHand.Refresh();
                foreach (Card card in CurrentDeck.HandList)
                {
                    listBoxCurrentHand.Items.Add(card.Description);
                }
                listBoxCurrentHand.Refresh();
                listBoxCurrentHand.Update();

                //Also update the combobox
                lBoxTurnOptions.Refresh();
                lBoxTurnOptions.Update();

                //Send user to the Play Hand Tab
                tabControl1.SelectedTab = tabControl1.TabPages["tabPagePlayHand"];
            }

            if ((message == PPokemon.GamePlay.GameHooks.PlayHandChanged) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {

            }
            #endregion

            #region ActivePokemonChoosen
            if ((message == PPokemon.GamePlay.GameHooks.ActivePokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                toolStripStatusLabel1.Text = "Active Pokemon Choosen.";

                if (game.State.Round == 1)
                {
                    //Check to see if the last basic was taken from hand
                    // Set next Play hand button
                    if (CurrentDeck.IsBasicInHand())
                    {
                        //Change the button
                        btPlayHandButton.Text = "Select Basic to Bench";
                        toolStripStatusLabel1.Text = "Choose Basic Pokemon to bench.";
                    }
                    else
                    {
                        //No more Basics in hand so go to the next game setup stage
                        GameEventHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.PLAYER);
                    }
                }
            }
            if ((message == PPokemon.GamePlay.GameHooks.ActivePokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                if (game.State.Round == 1)
                {
                    //Draw the Bench cards since Pokemon was choosen...
                    while (PCdeck.IsBasicInHand())
                    {
                        Card[] tempCardArray = new Card[PCdeck.HandList.Count];
                        PCdeck.HandList.CopyTo(tempCardArray);
                        foreach (Card carditem in tempCardArray)
                        {
                            PlaceCardToBench(carditem, PPokemon.GamePlay.Player.AIPLAYER);
                        }
                    }
                    //No more Basics in hand so go to the next game setup stage
                    GameEventHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.AIPLAYER);
                }
            }
            #endregion

            #region BenchPokemonChoosen
            if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhaseStart)
            {
                #region If TurnPhaseStart
                if ((message == PPokemon.GamePlay.GameHooks.BenchPokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
                {
                    #region Bench action, so lets refresh BenchDict
                    CurrentDeck.BenchDictRefresh(PPokemon.GamePlay.Player.PLAYER, game.State);
                    #endregion

                    #region Make sure we are in game setup still and no more basic pokemon in hand
                    if ((game.State.Round == 1) && (!CurrentDeck.IsBasicInHand()))
                    {
                        //Draw the prize cards
                        for (int i = 0; i < 6; i++)
                        {
                            listBoxPrizeCards.Items.Add(CurrentDeck.TakeCard(false));
                        }
                        //send event message
                        //There are no more Basics in your hand so start the flipping
                        FlipTheFirstCoin();
                    }
                    #endregion
                }
                if ((message == PPokemon.GamePlay.GameHooks.BenchPokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
                {
                    #region Bench action, so lets refresh BenchDict
                    PCdeck.BenchDictRefresh(PPokemon.GamePlay.Player.AIPLAYER, game.State);
                    #endregion

                    #region Make sure we are in game setup still and no more basic pokemon in hand
                    if ((game.State.Round == 1) && (!PCdeck.IsBasicInHand()))
                    {
                        //Draw the prize cards
                        for (int i = 0; i < 6; i++)
                        {
                            listBoxPrizeCardsPC.Items.Add(PCdeck.TakeCard(false));
                        }
                    }
                    #endregion
                }
                #endregion
            }
            if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhase2)
            {
                #region If TurnPhase2 (before Attack)
                if ((message == PPokemon.GamePlay.GameHooks.BenchPokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
                {
                    //Player just sent a handcard to the bench so process as TurnPhase2 would
                    //Make sure all Bench card are faceup
                    //Turn over Player1 Cards
                    pbBench1.TurnCardFaceUp();
                    pbBench2.TurnCardFaceUp();
                    pbBench3.TurnCardFaceUp();
                    pbBench4.TurnCardFaceUp();
                    pbBench5.TurnCardFaceUp();


                }
                else if ((message == PPokemon.GamePlay.GameHooks.BenchPokemonChoosen) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
                {
                    //Turn over AIPlayer Cards
                    pbBenchPC1.TurnCardFaceUp();
                    pbBenchPC2.TurnCardFaceUp();
                    pbBenchPC3.TurnCardFaceUp();
                    pbBenchPC4.TurnCardFaceUp();
                    pbBenchPC5.TurnCardFaceUp();
                }
                #endregion
            }
            #endregion

            #region FlippedCoin
            if (message == PPokemon.GamePlay.GameHooks.FlippedCoin)
            {
                #region Flipped coin to see who goes first (only in Round 1)
                //If Player flipped coin to see who goes first (only in Round 1)
                if (game.State.Round == 1)
                {
                    string wonmessage = "";
                    lbFlipResult.Visible = true;
                    //display the flip coin image
                    pBoxPlayHand.Image = PPokemon.Properties.Resources.CoinFlip39;

                    GamePlay.Coin c = (PPokemon.GamePlay.Coin)args;
                    if (((c == PPokemon.GamePlay.Coin.HEADS) && (checkBoxHeads.Checked == true)) ||
                        ((c == PPokemon.GamePlay.Coin.TAILS) && checkBoxTails.Checked == true))
                    {
                        //Player won because heads or won tails
                        wonmessage = "You won so you go first! Draw a Card.";
                        if (c == PPokemon.GamePlay.Coin.HEADS)
                        {
                            lbFlipResult.Text = "HEADS! You Win";
                        }
                        else
                            lbFlipResult.Text = "TAILS! You Win";

                        //Set the GameEnging State
                        game.State.CurrentPlayer = PPokemon.GamePlay.Player.PLAYER;
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhaseStart;
                    }
                    else
                    {
                        //AIPlayer won because heads or won tails
                        wonmessage = "AIPlayer won so AIPlayer goes first.";
                        lbFlipResult.Text = "Opponent Wins";

                        //Set the GameEnging State
                        game.State.CurrentPlayer = PPokemon.GamePlay.Player.AIPLAYER;
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhaseStart;
                    }
                    //Change the button
                    btPlayHandButton.Text = "OK\r\nReveal all Pokemon";
                    toolStripStatusLabel1.Text = toolStripStatusLabel1.Text = wonmessage;
                }
                else
                {
                    //Round > 1,  so probably flipping a coin because of an attack or

                    #region Flipped coin from Attack Directions
                    if (args.GetType() == typeof(CardDirections))
                    {
                        /*
                        //DO EACH DIRECTION IN THE ATTACK
                        CardEventNames eventName = CardEventNames.None;
                        string args = "";
                        for (int i = 0; i < attackdir.Directions.Count; i++)
                        {
                            eventName = attackdir.Directions[i].First;
                            args = attackdir.Directions[i].Second;
                            //DoAttackStep(eventName, args);
                        }
                         */
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            #region DrewACard
            //IF Player1 drew a card, Go display choices
            if (((message == PPokemon.GamePlay.GameHooks.DrewACard) || (message == PPokemon.GamePlay.GameHooks.ReEvalTurnPhase2)) &&
                ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                #region PHASE 2 ACTIONS...
                //It should be TurnPhase 1 or 2 only entering into this block
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase2;
                //Disable the 'Draw a Card' Button
                btDrawACard.Enabled = false;

                //refresh and show helper contols, unless "End phase 2 of turn" is already in lBoxTurnOptions.Items
                //if (!lBoxTurnOptions.Items.Contains("End phase 2 of turn"))
                TurnOnPlayHandControls(true);
                if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhase2)
                    lBoxTurnOptions.Items.Add("End phase 2 of turn");//Always have this option unless TurnPhase3

                ////a)Put any Basic Pokemon cards from HandDict on the BenchDict (loop until finished) If EVENTBUTTON////
                //Do only if BenchDict < 5
                if (CurrentDeck.BenchDict.Count < 5)
                {
                    List<Card> basics = new List<Card>();
                    basics = CurrentDeck.GetAllBasicsInHand();
                    if (basics.Count > 0)
                    {
                        foreach (Card carditem in basics)
                        {
                            lBoxTurnOptions.Items.Add("Put on Bench::" + carditem.Description);
                        }
                    }
                }


                #region b)EVOLVE Pokemon (loop until finished)
                //Check all possible evolves only if not the first round
                if (game.State.Round != 1)
                {
                    if (!game.State.AttachedEnergy)
                    {
                        //ActivePokemon...
                        if (pbActivePokemon.HasItem && pbActivePokemon.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbActivePokemon, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve ActivePokemon to::" + posCard.Properties.Pokemon.Name);
                            }
                        }

                        //pbBench1...
                        if (pbBench1.HasItem && pbBench1.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbBench1, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve Bench1 to::" + posCard.Properties.Pokemon.Name);
                            }
                        }
                        //pbBench2...
                        if (pbBench2.HasItem && pbBench2.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbBench2, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve Bench2 to::" + posCard.Properties.Pokemon.Name);
                            }
                        }
                        //pbBench3...
                        if (pbBench3.HasItem && pbBench3.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbBench3, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve Bench3 to::" + posCard.Properties.Pokemon.Name);
                            }
                        }
                        //pbBench4...
                        if (pbBench4.HasItem && pbBench4.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbBench4, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve Bench4 to::" + posCard.Properties.Pokemon.Name);
                            }
                        }
                        //pbBench5...
                        if (pbBench5.HasItem && pbBench5.CurrentCard.State.EvolvedPokemon == false)
                        {
                            Card posCard = new Card(Cardtype.Null);
                            posCard = CurrentDeck.CanEvolveCard(pbBench5, Pokedex);
                            if (posCard.Properties.CardType != Cardtype.Null)
                            {
                                lBoxTurnOptions.Items.Add("Evolve Bench5 to::" + posCard.Properties.Pokemon.Name);
                            }
                        }
                    }
                }
                #endregion

                #region c)ATTACH 1 energy to 1 Pokemon (then break;only allowed 1 per turn)
                //Are we allowed to attach an energy card during this current turn?
                if (!game.State.AttachedEnergy)
                {
                    //Energy to Active Pokemon?
                    //If the bestattack's total energyreq is less than the current attached energy or if the current attack properties are less than two
                    List<AttackProperties> attackp = pbActivePokemon.CurrentCard.IsCanAttack(pbActivePokemon.CurrentCard.State.AttachedCards);

                    if ((Ops.GetBestAttackEnergyTotalReq(pbActivePokemon.CurrentCard) < pbActivePokemon.CurrentCard.State.GetAttachedEnergyCardCount())
                       || (attackp.Count < 2))
                    {
                        Card ECard = new Card(Cardtype.Energy);
                        bool canattachE = CurrentDeck.CanAttachEnergyCard(pbActivePokemon, game.State, out ECard, false);
                        if (canattachE)
                        {
                            lBoxTurnOptions.Items.Add("Attach EnergyCard to ActivePokemon::" + ECard.Description);
                        }
                    }

                }
                #endregion

                //d)Play Trainer Card (loop until finished)
                //e)Play Supporter Card (then break;only allowed 1 per turn)
                //f)Play Stadium Card (then break;only allowed 1 per turn)
                foreach (Card trainercard in CurrentDeck.HandList)
                {
                    if (trainercard.Properties.CardType == Cardtype.Trainer || trainercard.Properties.CardType == Cardtype.Supporter ||
                        trainercard.Properties.CardType == Cardtype.Stadium)
                    {
                        lBoxTurnOptions.Items.Add("Play " + trainercard.Properties.CardType.ToString() + "::" + trainercard.Description);
                    }
                }
                
                //g)Retreat Active Pokemon (then break;only allowed 1 per turn)
                //h)Use Poke-Powers (loop until finished)

                lBoxTurnOptions.Update();

                //If the combobox has at more then "End phase" option
                if (lBoxTurnOptions.Items.Count >= 1)
                {
                    //Select the first item in the dropdownlist
                    lBoxTurnOptions.SelectedIndex = 0;

                    //Change the
                    btPlayHandButton.Text = "TurnPhase 2 do it!";
                    toolStripStatusLabel1.Text = "Select the action for Turn Phase 2, then click the button 'TurnPhase 2 do it!' ";
                }
                else
                {
                    //There are no moves to make, so go to TurnPhase 3: Attack?
                    lBoxTurnOptions.Items.Clear();

                    ////ATTACK!////
                    //Can only attack if not confused
                    if (pbActivePokemon.CurrentCard.State.CurrentCondition == SpecialConditions.None)
                    {
                        //Are there Attacks to do?? 
                        List<AttackProperties> attackp = pbActivePokemon.CurrentCard.IsCanAttack(pbActivePokemon.CurrentCard.State.AttachedCards);
                        if (attackp.Count > 0)
                        {
                            //There are attacks, so prompt the user which attack to use
                            foreach (AttackProperties attackprops in attackp)
                            {
                                lBoxTurnOptions.Items.Add("Attack with::" + attackprops.Title);
                            }
                        }

                        lBoxTurnOptions.Update();
                        btPlayHandButton.Text = "Attack!";
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase3;
                        //Hide the controls for Player1
                        TurnOnPlayHandControls(false);
                        //In TurnPhase3 so show lBoxTurnOptions with attacks
                        lBoxTurnOptions.Visible = true;
                        if (lBoxTurnOptions.Items.Count > 0)
                            lBoxTurnOptions.SelectedIndex = 0;

                        //But show 
                        lbFlipResult.Text = PPokemon.GamePlay.TurnPhase.TurnPhase3.ToString();
                    }

                    //Player1 just drew a card so disable
                    //Hide the PlayMat DrawACard button
                    btDrawACard.Enabled = false;
                }
                #endregion
            }
            if ((message == PPokemon.GamePlay.GameHooks.DrewACard) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                #region AIPLAYER PHASE 2 ACTIONS...
                if (game.State.CurrentTurnPhase == PPokemon.GamePlay.TurnPhase.TurnPhase1)
                {
                    game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase2;
                    #region a)Put any Basic Pokemon cards from HandDict on the BenchDict (loop until finished) If EVENTBUTTON////

                    List<Card> basics = new List<Card>();
                    basics = PCdeck.GetAllBasicsInHand();
                    if (basics.Count > 0)
                    {
                        foreach (Card carditem in basics)
                        {
                            if (PlaceCardToBench(carditem, PPokemon.GamePlay.Player.AIPLAYER))
                            {
                                tbAIPlayerLog.AppendText("Placed " + carditem.Description + " on Bench.\r\n");
                                GameEventHook(PPokemon.GamePlay.GameHooks.BenchPokemonChoosen, GamePlay.Player.AIPLAYER);
                            }
                            else
                            {
                                //Bench must be full
                                tbAIPlayerLog.AppendText("Placed " + carditem.Description + " in hand.\r\n");//Is this being added to the other bench??
                            }
                        }

                    }
                    else
                        tbAIPlayerLog.AppendText("No basic cards in hand.\r\n");
                    #endregion

                    #region b)EVOLVE Pokemon (Bench or Active) (loop until finished)////
                    if ((!game.State.AttachedEnergy) && (game.State.Round != 1))
                    {
                        //ActivePokemon...
                        if (PCdeck.CanEvolveCard(pbActivePokemonPC, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("Active Pokemon EVOLVED to " + pbActivePokemonPC.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbActivePokemonPC.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("Active Pokemon could not evolve.\r\n");

                        //pbBenchPC1...
                        if (PCdeck.CanEvolveCard(pbBenchPC1, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("BenchPC1 Pokemon EVOLVED to " + pbBenchPC1.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbBenchPC1.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("BenchPC1 Pokemon could not evolve.\r\n");
                        //pbBenchPC2...
                        if (PCdeck.CanEvolveCard(pbBenchPC2, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("BenchPC2 Pokemon EVOLVED to " + pbBenchPC2.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbBenchPC2.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("BenchPC2 Pokemon could not evolve.\r\n");

                        //pbBenchPC3...
                        if (PCdeck.CanEvolveCard(pbBenchPC3, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("BenchPC3 Pokemon EVOLVED to " + pbBenchPC3.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbBenchPC3.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("BenchPC3 Pokemon could not evolve.\r\n");
                        //pbBenchPC4...
                        if (PCdeck.CanEvolveCard(pbBenchPC4, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("BenchPC4 Pokemon EVOLVED to " + pbBenchPC4.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbBenchPC4.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("BenchPC4 Pokemon could not evolve.\r\n");
                        //pbBenchPC5...
                        if (PCdeck.CanEvolveCard(pbBenchPC5, Pokedex, true))
                        {
                            tbAIPlayerLog.AppendText("BenchPC5 Pokemon EVOLVED to " + pbBenchPC5.CurrentCard.Properties.Pokemon.Name + ".\r\n");
                            //Remove the Card from the HandList, so it does continue Evolving each turn
                            PCdeck.HandList.RemoveAt(PCdeck.HandList.IndexOf(pbBenchPC5.CurrentCard));
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                        }
                        else
                            tbAIPlayerLog.AppendText("BenchPC5 Pokemon could not evolve.\r\n");

                    }
                    else
                    {
                        tbAIPlayerLog.AppendText("Could not evolve because Energy already used or first turn in current game.\r\n");
                    }
                    #endregion

                    #region c)ATTACH 1 energy to 1 Pokemon (then break;only allowed 1 per turn)
                    //Are we allowed to attach an energy card during this current turn?
                    if (!game.State.AttachedEnergy)
                    {
                        //If the bestattack's total energyreq is less than the current attached energy or if the current attack properties are less than two
                        List<AttackProperties> attackp = pbActivePokemonPC.CurrentCard.IsCanAttack(pbActivePokemonPC.CurrentCard.State.AttachedCards);
                        if ((pbActivePokemonPC.CurrentCard.State.GetAttachedEnergyCardCount() < Ops.GetBestAttackEnergyTotalReq(pbActivePokemonPC.CurrentCard))
                           || (attackp.Count < pbActivePokemonPC.CurrentCard.Properties.Attacks.First.Count))
                        {
                            //Energy to Active Pokemon?
                            Card ECard = new Card(Cardtype.Energy);
                            if (PCdeck.CanAttachEnergyCard(pbActivePokemonPC, game.State, out ECard, true))
                            {
                                tbAIPlayerLog.AppendText("ATTACHED " + ECard.Description + " to Active Pokemon " + pbActivePokemonPC.CurrentCard.Description + "\r\n");
                            }
                            else
                                tbAIPlayerLog.AppendText("Could not attach Energy to Active Pokemon.\r\n");
                        }
                        else
                        {
                            tbAIPlayerLog.AppendText("Could not attach Energy to Active Pokemon (Pokemon maxed out).\r\n");
                        }
                    }

                    #endregion

                    #region d,e,f Play Trainer and Supporter Cards?
                    List<Card> trainerList = new List<Card>();
                    CardNeedActions curNeed = pbActivePokemonPC.CurrentCard.State.GetCurrentNeed();
                    foreach (Card handCard in PCdeck.HandList)
                    {
                        if (handCard.Properties.CardType == Cardtype.Trainer || handCard.Properties.CardType == Cardtype.Supporter)
                        {
                            //Remember this card
                            trainerList.Add(handCard);
                        }
                    }

                    foreach (Card availablecard in trainerList)
                    {
                        //Get the AttackDirections of this card so we can evaluate it
                        AttackProperties aprops = Ops.GetLastAttackFromCard(availablecard);
                        //If this Card has the need, then do it
                        if (game.TrainerEngine.IsCardNeeded(curNeed, availablecard, aprops))
                        {
                            //do it!
                            game.ProcessAttack(pbActivePokemonPC, pbActivePokemon, aprops, true);
                            tbAIPlayerLog.AppendText("PLAYED: " + availablecard.Description + ".\r\n");
                            //Remove card from hand
                            PCdeck.HandList.Remove(availablecard);
                            GameEventHook(PPokemon.GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                            GameEventHook(PPokemon.GamePlay.GameHooks.ReEvalTurnPhase2, GamePlay.Player.AIPLAYER);
                            break;
                        }
                        else
                        {
                            tbAIPlayerLog.AppendText("Current need is " + curNeed.ToString() + ". " + availablecard.Description + " not played.\r\n");
                        }
                    }


                    #endregion 
                    ////ATTACK!////
                    game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase3;
                    //Can only attack if not confused or not Paralyzed
                    if (pbActivePokemonPC.CurrentCard.State.CurrentCondition == SpecialConditions.Paralyzed)
                    {
                        tbAIPlayerLog.AppendText("CurrentCondition = Paralyzed. Can not attack.\r\n");
                    }
                    if (pbActivePokemonPC.CurrentCard.State.CurrentCondition == SpecialConditions.None ||
                        pbActivePokemonPC.CurrentCard.State.CurrentCondition == SpecialConditions.Poisoned)
                    {
                        #region Normal Attack...
                        //Get the AttackProperty choosen
                        AttackProperties attack = new AttackProperties();
                        attack = Ops.GetAttackFromCardHolder(pbActivePokemonPC, Ops.GetBestEnabledAttack(pbActivePokemonPC).Title);
                        tbAIPlayerLog.AppendText(game.ProcessAttack(pbActivePokemonPC, pbActivePokemon, attack, false).ToString());
                        if (game.State.CurrentTurnPhase != PPokemon.GamePlay.TurnPhase.Waiting)
                            game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase3;
                        #endregion
                    }
                    else if (pbActivePokemonPC.CurrentCard.State.CurrentCondition == SpecialConditions.Confused)
                    {
                        #region Confused
                        //ActivePokemon is Confused, so must flip coin
                        if (game.FlipACoin() == PPokemon.GamePlay.Coin.HEADS)
                        {
                            GameEventHook(PPokemon.GamePlay.GameHooks.FlippedCoin, GamePlay.Player.AIPLAYER);
                            //HEADS! Attack works normally
                            tbAIPlayerLog.AppendText(pbActivePokemonPC.CurrentCard.Properties.Pokemon.Name + " attack will work normally!\r\n");
                            //Get the AttackProperty choosen
                            AttackProperties attack = new AttackProperties();
                            attack = Ops.GetAttackFromCardHolder(pbActivePokemonPC, Ops.GetBestEnabledAttack(pbActivePokemonPC).Title);
                            tbAIPlayerLog.AppendText(game.ProcessAttack(pbActivePokemonPC, pbActivePokemon, attack, false).ToString());
                        }
                        else
                        {
                            GameEventHook(PPokemon.GamePlay.GameHooks.FlippedCoin, GamePlay.Player.AIPLAYER);
                            //Tails so receive three damage counters = +30
                            MessageBox.Show(pbActivePokemonPC.CurrentCard.Properties.Pokemon.Name + " Confused Attack! +30 Damage!");
                            pbActivePokemonPC.CurrentCard.State.CurrentDamage = pbActivePokemonPC.CurrentCard.State.CurrentDamage + 30;
                            //Check to see if Pokemon KnockedOut
                            if (pbActivePokemonPC.CurrentCard.State.CurrentDamage >= pbActivePokemonPC.CurrentCard.Properties.HP)
                            {
                                MessageBox.Show(pbActivePokemonPC.CurrentCard.Properties.Pokemon.Name + " is Knocked Out!!");
                                game.KnockOut(pbActivePokemonPC, GamePlay.Player.AIPLAYER);
                                GameEventHook(PPokemon.GamePlay.GameHooks.PokemonKnockedOut, GamePlay.Player.AIPLAYER);
                            }
                        }
                        #endregion

                        #region Poisoned gets checked in EndOfTurn
                        #endregion
                    }

                    if (game.State.CurrentTurnPhase != PPokemon.GamePlay.TurnPhase.Waiting)
                        game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.TurnPhase3;//make sure

                    game.EndOfTurn(PPokemon.GamePlay.Player.AIPLAYER, pbActivePokemonPC);
                    GameEventHook(PPokemon.GamePlay.GameHooks.PlayerChange, PPokemon.GamePlay.Player.AIPLAYER);
                }
                #endregion
            }
            #endregion

            #region PlayerChanged
            if ((message == PPokemon.GamePlay.GameHooks.PlayerChange) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                if (game.State.CurrentTurnPhase != PPokemon.GamePlay.TurnPhase.Waiting)
                {
                    //Disable the Player1 controls and enable the AIPlayer
                    btPlayHandButton.Text = "Start\r\nAIPlayer's Turn";
                    toolStripStatusLabel1.Text = "Start\r\nAIPlayer's Turn";

                    //Hide the controls for Player1
                    TurnOnPlayHandControls(false);

                    //Also hide the Playmat Draw a card
                    btDrawACard.Enabled = false;
                }
            }
            if ((message == PPokemon.GamePlay.GameHooks.PlayerChange) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                if (game.State.CurrentTurnPhase != PPokemon.GamePlay.TurnPhase.Waiting)
                {
                    //Disable the AIPlayer controls and enable the Player1
                    btPlayHandButton.Text = "Start\r\nTurn";
                    toolStripStatusLabel1.Text = "Start\r\nTurn";
                }
            }
            #endregion

            #region PokemonKnockedOut
            if ((message == PPokemon.GamePlay.GameHooks.PokemonKnockedOut) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                #region Take a Prize Card
                Card prizecard = new Card(Cardtype.Null);
                if (listBoxPrizeCardsPC.Items.Count == 0)
                {
                    this.GameEventHook(PPokemon.GamePlay.GameHooks.WonGamePrizeCards, GamePlay.Player.AIPLAYER);
                }
                else
                {
                    prizecard = (Card)listBoxPrizeCardsPC.Items[0];
                    PCdeck.HandList.Add(prizecard);
                    this.GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.AIPLAYER);
                    listBoxPrizeCardsPC.Items.RemoveAt(0);
                    tbAIPlayerLog.AppendText("Prize card taken! " + listBoxPrizeCardsPC.Items.Count.ToString() + " remaining.\r\n");
                    if (listBoxPrizeCardsPC.Items.Count == 0)
                        this.GameEventHook(PPokemon.GamePlay.GameHooks.WonGamePrizeCards, GamePlay.Player.AIPLAYER);
                }

                #endregion

                //Since there was a knockout, lets just make sure BenchDict is uptodate
                #region Bench action, so lets refresh BenchDict
                CurrentDeck.BenchDictRefresh(PPokemon.GamePlay.Player.PLAYER, game.State);
                #endregion

                //Make sure the discard pile has a facedown image
                pbDiscard.Image = PPokemon.Properties.Resources.CardBack_sm;

                #region Prompt Player1 to choose an Active Pokemon: TurnPhase.Waiting
                if (this.CurrentDeck.BenchDict.Count > 0)
                {
                    MessageBox.Show("Place a Pokemon from your Bench as your Active Pokemon.", "Choose New Active Pokemon", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    //Fill the combobox with choices
                    lBoxTurnOptions.Items.Clear();
                    foreach (CardHolder ch in this.game.State.Player1Pokemon)
                    {
                        //DEBUG:
//                        if (ch.CurrentCard != null && ch.CurrentCard.Description == "")
//                            MessageBox.Show("Trace back and see why this card is incomplete.");
                        if (ch.HasItem)
                            lBoxTurnOptions.Items.Add("New ActivePokemon::" + ch.CurrentCard.Description);
                    }
                    if (lBoxTurnOptions.Items.Count > 0)
                        lBoxTurnOptions.SelectedIndex = 0;
                    lBoxTurnOptions.Visible = true;
                    lBoxTurnOptions.Update();
                    //Change the button text
                    btPlayHandButton.Text = "New ActivePokemon";
                    toolStripStatusLabel1.Text = "Select New ActivePokemon";

                    game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.Waiting;
                }
                else
                {
                    //There is no Bench Pokemon, AIPlayer Wins
                    MessageBox.Show("There are no Pokemon on your Bench to replace the Active Pokemon. You LOST!!");
                    GameEventHook(PPokemon.GamePlay.GameHooks.WonGameKnockedOutLast, GamePlay.Player.AIPLAYER);
                }
                #endregion
            }
            if ((message == PPokemon.GamePlay.GameHooks.PokemonKnockedOut) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                #region Take a Prize Card
                Card prizecard = new Card(Cardtype.Null);
                if (listBoxPrizeCards.Items.Count == 0)
                    this.GameEventHook(PPokemon.GamePlay.GameHooks.WonGamePrizeCards, GamePlay.Player.PLAYER);
                else
                {
                    prizecard = (Card)listBoxPrizeCards.Items[0];
                    CurrentDeck.HandList.Add(prizecard);
                    this.GameEventHook(GamePlay.GameHooks.PlayHandChanged, GamePlay.Player.PLAYER);
                    listBoxPrizeCards.Items.RemoveAt(0);
                    string prizemess = ("Prize card taken! " + listBoxPrizeCards.Items.Count.ToString() + " remaining.\r\n");
                    MessageBox.Show(prizemess, "Prize card taken!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (listBoxPrizeCards.Items.Count == 0)
                        this.GameEventHook(PPokemon.GamePlay.GameHooks.WonGamePrizeCards, GamePlay.Player.PLAYER);
                }
                #endregion

                //Since there was a knockout, lets just make sure BenchDict is uptodate
                #region Bench action, so lets refresh BenchDict
                PCdeck.BenchDictRefresh(PPokemon.GamePlay.Player.AIPLAYER, game.State);
                #endregion

                //Make sure the discard pile has a facedown image
                pbDiscardPC.Image = PPokemon.Properties.Resources.CardBack_sm;

                #region Autochoose the new Pokemon
                if (this.PCdeck.BenchDict.Count > 0)
                {
                    //Search for the highest stage on bench
                    foreach (CardHolder cardholder in game.State.AIPlayerPokemon)
                    {
                        if (cardholder.HasItem)
                        {
                            if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage2)
                            {
                                pbActivePokemonPC.PlaceCard(cardholder.CurrentCard);
                                pbActivePokemonPC.CurrentCard.State.ActivePokemon = true;
                                pbActivePokemonPC.TurnCardFaceUp();
                                //Clear the previous
                                cardholder.RemoveItem();
                                break;
                            }
                            else if (cardholder.CurrentCard.Properties.CardType == Cardtype.Stage1)
                            {
                                pbActivePokemonPC.PlaceCard(cardholder.CurrentCard);
                                pbActivePokemonPC.CurrentCard.State.ActivePokemon = true;
                                pbActivePokemonPC.TurnCardFaceUp();
                                //Clear the previous
                                cardholder.RemoveItem();
                                break;
                            }
                        }
                    }
                    //If there was no Stage1 or Stage2 on the Bench to evolve from
                    //Find the best card from the bench
                    if (pbActivePokemonPC.HasItem == false)
                    {
                        if (PCdeck.BenchDict.Count > 0)
                        {
                            Card basiccardfromBench = new Card(Cardtype.Basic);
                            basiccardfromBench = game.State.GetBestBasicCardFromBench(PPokemon.GamePlay.Player.AIPLAYER);
                            pbActivePokemonPC.PlaceCard(basiccardfromBench);
                            pbActivePokemonPC.CurrentCard.State.ActivePokemon = true;
                            pbActivePokemonPC.TurnCardFaceUp();
                            //Clear the previous
                            RemoveCardFromBenchCardHolder(basiccardfromBench, PPokemon.GamePlay.Player.AIPLAYER);
                            //This card is now out of the bench, so remove it
                            PCdeck.BenchDictRefresh(PPokemon.GamePlay.Player.AIPLAYER, game.State);
                        }
                    }

                    tbAIPlayerLog.AppendText("ActivePokemonChoosen:" + pbActivePokemonPC.CurrentCard.Description);
                    GameEventHook(GamePlay.GameHooks.ActivePokemonChoosen, GamePlay.Player.AIPLAYER);
                }
                else
                {
                    MessageBox.Show("There is no Pokemon on the Bench to replace the Active Pokemon! You WIN!!");
                    GameEventHook(PPokemon.GamePlay.GameHooks.WonGameKnockedOutLast, GamePlay.Player.PLAYER);
                }
                #endregion
            }
            #endregion

            #region WonGameKnockedOutLast
            if ((message == PPokemon.GamePlay.GameHooks.WonGameKnockedOutLast) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                MessageBox.Show("You knocked out your opponent's last Pokemon in play", "You WIN!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
                game.PlayEndSong();
            }
            if ((message == PPokemon.GamePlay.GameHooks.WonGameKnockedOutLast) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                MessageBox.Show("Your opponent knocked out your last Pokemon in play.", "You Lost.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
            }
            #endregion

            #region WonGamePrizeCards
            if ((message == PPokemon.GamePlay.GameHooks.WonGamePrizeCards) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                MessageBox.Show("You collected all of your prize cards!", "You WIN!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
                game.PlayEndSong();
            }
            if ((message == PPokemon.GamePlay.GameHooks.WonGamePrizeCards) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                MessageBox.Show("Your opponent collected all of its prize cards!", "You Lost.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
            }
            #endregion

            #region WonGameOutOfDeckCards
            if ((message == PPokemon.GamePlay.GameHooks.WonGameOutOfDeckCards) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                MessageBox.Show("Your opponent is out of cards in its deck!", "You WIN!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
                game.PlayEndSong();
            }
            if ((message == PPokemon.GamePlay.GameHooks.WonGameOutOfDeckCards) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                MessageBox.Show("You are out of cards in your deck.", "You Lost.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                game.State.CurrentTurnPhase = PPokemon.GamePlay.TurnPhase.GameWon;
            }
            #endregion

            #region DamageDone
            if ((message == PPokemon.GamePlay.GameHooks.DamageDone) && ((GamePlay.Player)args == GamePlay.Player.PLAYER))
            {
                //Execute all of the in-play pokepowers/bodies
                bool didpowers;
                game.DoPokePowers(PPokemon.GamePlay.Player.PLAYER, PPokemon.GamePlay.GameHooks.DamageDone, out didpowers);
            }
            if ((message == PPokemon.GamePlay.GameHooks.DamageDone) && ((GamePlay.Player)args == GamePlay.Player.AIPLAYER))
            {
                //Execute all of the in-play pokepowers/bodies
                bool didpowers;
                game.DoPokePowers(PPokemon.GamePlay.Player.AIPLAYER, PPokemon.GamePlay.GameHooks.DamageDone, out didpowers);
            }
            #endregion
        }
    }

    public class Common
    {
        #region Remote locations
        /// <summary>
        /// http://www.pokemon.com/Pokedex/XML/
        /// </summary>
        public static string urlPokedexXML = "http://www.pokemon.com/Pokedex/XML/";
        /// <summary>
        /// http://www.pokemon.com/Pokedex/XML/globalPokedex.xml
        /// </summary>
        public static string urlPokedexXMLglobalPokedex = "http://www.pokemon.com/Pokedex/XML/globalPokedex.xml";
        /// <summary>
        /// http://www.pokemon.com/Pokedex/characterswfs/evo_0/
        /// </summary>
        public static string urlPokedexcharacterswfs = "http://www.pokemon.com/Pokedex/characterswfs/evo_0/";

        /// <summary>
        /// http://code.google.com/p/personalpokemon/wiki/CreatingCards
        /// </summary>
        public static string urlCreateCardWiki = "http://code.google.com/p/personalpokemon/wiki/CreatingCards";

        /// <summary>
        /// http://personalpokemon.googlecode.com
        /// </summary>
        public static string urlPPokemonGooglecode = "http://personalpokemon.googlecode.com";
        #endregion Remote locations

        #region Local Application Folders and files
        /// <summary>
        /// Application.StartupPath + "\\xml
        /// </summary>
        public static string xmlFolder = Application.StartupPath + "\\xml";
        /// <summary>
        /// Application.StartupPath + "\\swf"
        /// </summary>
        public static string swfFolder = Application.StartupPath + "\\swf";
        /// <summary>
        /// Application.StartupPath + "\\jpg"
        /// </summary>
        public static string jpgFolder = Application.StartupPath + "\\jpg";

        /// <summary>
        /// xmlFolder + "\\globalPokedex.xml";
        /// </summary>
        public static string globalPokedex_xml = xmlFolder + "\\globalPokedex.xml";

        /// <summary>
        /// xmlFolder + "\\Pokedex.xml";
        /// </summary>
        public static string Pokedex_xml = xmlFolder + "\\Pokedex.xml";

        /// <summary>
        /// Application.StartupPath + "\\decks"
        /// </summary>
        public static string decksFolder = Application.StartupPath + "\\decks";

        public static string attacksound_wav = Application.StartupPath + "\\sounds\\attacksound.wav";

        public static string endsong_wav = Application.StartupPath + "\\sounds\\EndSong.wav";

        #endregion Local Application Folders and files
        
        /// <summary>
        /// Current count of Pokemon listed on the Official website
        /// </summary>
        public static int pokemonCount = 490;
    }
}