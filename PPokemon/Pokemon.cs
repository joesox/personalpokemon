/////////////////////////////////////////////////////////
// PPokemon - Personal Pokemon
// Pokedex.cs
// by Joseph P. Socoloski III 
// Copyright 2007. All Rights Reserved.
// NOTE:   Loads a Pokedex.xml file to create a Pokedex object
// WHAT'S NEW: 	
//  - 
// LIMITS:  -
// TODO:    -
//LICENSE
//BY DOWNLOADING AND USING, YOU AGREE TO THE FOLLOWING TERMS:
//If it is your intent to use this software for non-commercial purposes, 
//such as in academic research, this software is free and is covered under 
//the GNU GPL License, given here: <http://www.gnu.org/licenses/gpl.txt> 
////////////////////////////////////////////////////////////////////////////
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
using PPokemon.Cards;

namespace PPokemon
{
    public class Pokemon
    {
        #region Variables
        /// <summary>
        /// Stores the Pokemon global index for the swf and xml files etc.
        /// </summary>
        public int Pokemonindex
        {
            get { return _pokemonindex; }
            set { _pokemonindex = value; }
        }
        int _pokemonindex = 0;

        /// <summary>
        /// Stores the local path and file name to its .swf image
        /// </summary>
        public string LocalSWF
        {
            get { return _LocalSWF; }
            set { _LocalSWF = value; }
        }
        string _LocalSWF = null;

        /// <summary>
        /// Stores the name of the Pokemon
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        string _Name = null;

        /// <summary>
        /// Stores the type of the Pokemon
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        string _Type = null;

        /// <summary>
        /// Stores the height_inches of the Pokemon
        /// </summary>
        public string Height_inches
        {
            get { return _Height_inches; }
            set { _Height_inches = value; }
        }
        string _Height_inches = null;

        /// <summary>
        /// Stores the height_inches of the Pokemon
        /// </summary>
        public string Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        string _Weight = null;

        /// <summary>
        /// Stores the HP (Hit Points) Percentage of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbHP
        {
            get { return _pbHP; }
            set { _pbHP = value; }
        }
        int _pbHP = 0;

        /// <summary>
        /// Stores the pbATTACK of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbATTACK
        {
            get { return _pbATTACK; }
            set { _pbATTACK = value; }
        }
        int _pbATTACK = 0;

        /// <summary>
        /// Stores the pbDEFENSE of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbDEFENSE
        {
            get { return _pbDEFENSE; }
            set { _pbDEFENSE = value; }
        }
        int _pbDEFENSE = 0;

        /// <summary>
        /// Stores the pbSPECIALATTACK of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbSPECIALATTACK
        {
            get { return _pbSPECIALATTACK; }
            set { _pbSPECIALATTACK = value; }
        }
        int _pbSPECIALATTACK = 0;

        /// <summary>
        /// Stores the pbSPECIALDEFENSE of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbSPECIALDEFENSE
        {
            get { return _pbSPECIALDEFENSE; }
            set { _pbSPECIALDEFENSE = value; }
        }
        int _pbSPECIALDEFENSE = 0;

        /// <summary>
        /// Stores the pbSPEED of the Pokemon for the Progressbar object only
        /// </summary>
        public int pbSPEED
        {
            get { return _pbSPEED; }
            set { _pbSPEED = value; }
        }
        int _pbSPEED = 0;

        /// <summary>
        /// Stores the Ability of the Pokemon
        /// </summary>
        public string Ability
        {
            get { return _Ability; }
            set { _Ability = value; }
        }
        string _Ability = null;

        /// <summary>
        /// Unknown from globalPokedex.xml
        /// </summary>
        public int Unknown
        {
            get { return _Unknown; }
            set { _Unknown = value; }
        }
        int _Unknown = 0;

        /// <summary>
        /// Stores the Description of the Pokemon
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        string _Description = null;

        /// <summary>
        /// Holds all of the Pokemon's evolution values
        /// int, int
        /// </summary>
        public Dictionary<int, int> EvolutionDict = new Dictionary<int,int>();

        /// <summary>
        /// Holds all of the Attached Energy Cards that are attached to this Pokemon
        /// string, Card
        /// </summary>
        Dictionary<string, Card> EnergyCardsAttachedDict = new Dictionary<string, Card>();
        #endregion Variables

        #region Constructors
        /// <summary>
        /// Create the Pokemon null
        /// </summary>
        public Pokemon()
        {
        }

        /// <summary>
        /// Manually Create the Pokemon's Description
        /// </summary>
        public Pokemon(int _pokemonindex, string _Name, string _LocalSWF, string _Type, string _Height_inches, string _Weight, int _pbHP, int _pbATTACK,
            int _pbDEFENSE, int _pbSPECIALATTACK, int _pbSPECIALDEFENSE, int _pbSPEED, string _Ability, int _Unknown, string _Description)
        {
            Pokemonindex = _pokemonindex;
            Name = _Name;
            LocalSWF = _LocalSWF;
            Type = _Type;
            Height_inches = _Height_inches;
            Weight = _Weight;
            pbHP = _pbHP;
            pbATTACK = _pbATTACK;
            pbDEFENSE = _pbDEFENSE;
            pbSPECIALATTACK = _pbSPECIALATTACK;
            pbSPECIALDEFENSE = _pbSPECIALDEFENSE;
            pbSPEED = _pbSPEED;
            Ability = _Ability.ToLower();
            Unknown = _Unknown;
            Description = _Description;
        }

        /// <summary>
        /// Auto Create the Pokemon's Description
        /// </summary>
        public Pokemon(int _pokemonindex, string _Name, string _LocalSWF, string _Type, string _Height_inches, string _Weight, int _pbHP, int _pbATTACK,
            int _pbDEFENSE, int _pbSPECIALATTACK, int _pbSPECIALDEFENSE, int _pbSPEED, string _Ability, int _Unknown)
        {
            Pokemonindex = _pokemonindex;
            Name = _Name;
            LocalSWF = _LocalSWF;
            Type = _Type;
            Height_inches = _Height_inches;
            Weight = _Weight;
            pbHP = _pbHP;
            pbATTACK = _pbATTACK;
            pbDEFENSE = _pbDEFENSE;
            pbSPECIALATTACK = _pbSPECIALATTACK;
            pbSPECIALDEFENSE = _pbSPECIALDEFENSE;
            pbSPEED = _pbSPEED;
            Ability = _Ability.ToLower();
            Unknown = _Unknown;
            Description = CreateDescription(_pokemonindex);
        }

        /// <summary>
        /// Creates a Pokemon from an XmlNode
        /// </summary>
        /// <param name="xNode">XmlNode</param>
        public Pokemon(XmlNode xNode)
        {
            XmlNodeList nodeList;
            XmlNode xnode;

            //Assign of of its attrubutes first..
            Pokemonindex = Convert.ToInt16(xNode.Attributes["index"].Value);
            Name = xNode.Attributes["name"].Value;
            Type = xNode.Attributes["type"].Value;
            Height_inches = xNode.Attributes["height_inches"].Value;
            Weight = xNode.Attributes["weight"].Value;
            pbHP = Convert.ToInt16(xNode.Attributes["pbHP"].Value);
            pbATTACK = Convert.ToInt16(xNode.Attributes["pbATTACK"].Value);
            pbDEFENSE = Convert.ToInt16(xNode.Attributes["pbDEFENSE"].Value);
            pbSPECIALATTACK = Convert.ToInt16(xNode.Attributes["pbSPECIALATTACK"].Value);
            pbSPECIALDEFENSE = Convert.ToInt16(xNode.Attributes["pbSPECIALDEFENSE"].Value);
            pbSPEED = Convert.ToInt16(xNode.Attributes["pbSPEED"].Value);
            Ability = xNode.Attributes["ability"].Value.ToLower();
            Unknown = Convert.ToInt16(xNode.Attributes["unknown"].Value);

            // Loop through the XML nodes until the leaf is reached.
            if (xNode.HasChildNodes)
            {
                nodeList = xNode.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    xnode = xNode.ChildNodes[i];

                    if (xnode.Name == "desc")
                    {
                        Description = xnode.InnerText;
                    }

                    if (xnode.Name == "localimage")
                    {
                        LocalSWF = xnode.InnerText;
                    }

                    if (xnode.Name == "evolution")
                    {
                        //Add this value to the Evolution array
                        EvolutionDict.Add(EvolutionDict.Count+1, Convert.ToInt16(xnode.InnerText));
                    }
                }
            }

            //LocalSWF = _LocalSWF;
        }

        #endregion Constructors

        #region Height Methods
        /// <summary>
        /// Convert the Height_inches string to Int16
        /// </summary>
        /// <returns></returns>
        public Int16 Height_inchesToInt16()
        {
            return Convert.ToInt16(Height_inches);
        }

        /// <summary>
        /// Convert the Height_inches string to Int16
        /// </summary>
        /// <returns></returns>
        public string GetHeightToFeetString()
        {
            string strHeight = "";

            float ft = 0;
            int inches = 0;
            int feet = 0;
            float inches_per_foot = 12;
            ft = Convert.ToSingle(this.Height_inchesToInt16()) / inches_per_foot;
            feet = (int)ft;
            inches = (int)((ft % 1) * 10);

            strHeight = Convert.ToString(feet) + "'" + Convert.ToString(inches) + "\"";

            return strHeight;
        }
        #endregion Height Methods

        /// <summary>
        /// Gets the Pokemon's description from Common.xmlFolder
        /// </summary>
        /// <param name="pokemonindex"></param>
        public static string CreateDescription(int pokemonindex)
        {
            string desc = "";
            XmlNode xNode;
            XmlNodeList nodeList;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Common.xmlFolder + "\\" + Convert.ToString(pokemonindex) + ".xml");
                        
            // Loop through the XML nodes until the leaf is reached.
            if (xmlDoc.HasChildNodes)
            {
                nodeList = xmlDoc.ChildNodes;
                for (int i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = xmlDoc.ChildNodes[i];
                    if (xNode.Name == "D")
                    {
                        if (xNode.HasChildNodes)
                        {
                            desc = xNode.FirstChild.InnerText;
                        }
                    }
                }
            }
            return desc;
        }

        /// <summary>
        /// Gets the first type listed from the Pokedex
        /// </summary>
        /// <returns></returns>
        public PokemonTYPE GetPokemonTYPE()
        {
            PokemonTYPE t = PokemonTYPE.Null;
            switch (Type.Trim().ToLower().Substring(0, Type.Trim().ToLower().IndexOf(",")))
            {
                case "colorless":
                    t = PokemonTYPE.Colorless;
                    break;
                case "darkness":
                    t = PokemonTYPE.Darkness;
                    break;
                case "fighting":
                    t = PokemonTYPE.Fighting;
                    break;
                case "fire":
                    t = PokemonTYPE.Fire;
                    break;
                case "grass":
                    t = PokemonTYPE.Grass;
                    break;
                case "lightning":
                    t = PokemonTYPE.Lightning;
                    break;
                case "metal":
                    t = PokemonTYPE.Metal;
                    break;
                case "psychic":
                    t = PokemonTYPE.Psychic;
                    break;
                case "water":
                    t = PokemonTYPE.Water;
                    break;
                default:
                    break;
            }
            //TODO: May wish to check if second type if defaulted out colorless
            return t;
        }
    }
}
