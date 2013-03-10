using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgLibrary.CharacterClasses
{

    public abstract class Character
    {
        #region Vital Field and Property Region

        protected string characterType;

        public string CharacterType
        {
            get { return characterType; }
        }

        #endregion

        #region Basic Attribute and Property Region

        protected int strength;
        protected int dexterity;
        protected int cunning;
        protected int willpower;
        protected int magic;
        protected int constitution;

        protected int strengthModifier;
        protected int dexterityModifier;
        protected int cunningModifier;
        protected int willpowerModifier;
        protected int magicModifier;
        protected int constitutionModifier;

        public int Strength
        {
            get { return strength + strengthModifier; }
            protected set { strength = value; }
        }

        public int Dexterity
        {
            get { return dexterity + dexterityModifier; }
            protected set { dexterity = value; }
        }

        public int Cunning
        {
            get { return cunning + cunningModifier; }
            protected set { cunning = value; }
        }


        public int Willpower
        {
            get { return willpower + willpowerModifier; }
            protected set { willpower = value; }
        }

        public int Magic
        {
            get { return magic + magicModifier; }
            protected set { magic = value; }
        }

        public int Constitution
        {
            get { return constitution + constitutionModifier; }
            protected set { constitution = value; }
        }

        #endregion

        #region Calculated Attribute Field and Property Region

        protected StatusBar health;
        protected StatusBar stamina;
        protected StatusBar mana;

        public StatusBar Health
        {
            get { return health; }
        }

        public StatusBar Stamina
        {
            get { return stamina; }
        }

        public StatusBar Mana
        {
            get { return mana; }
        }

        protected int attack;
        protected int damage;
        protected int defense;

        #endregion

        #region Level Field and Property Region

        protected int level;
        protected long experience;

        public int Level
        {
            get { return level; }
            protected set { level = value; }
        }

        public long Experience
        {
            get { return experience; }
            protected set { experience = value; }
        }


        #endregion

        #region Constructor Region

        private Character()
        {
            Strength = 0;
            Dexterity = 0;
            Cunning = 0;
            Willpower = 0;
            Magic = 0;
            Constitution = 0;

            health = new StatusBar(0);
            stamina = new StatusBar(0);
            mana = new StatusBar(0);
        }

        public Character(Attributes attributeData)
        {
            characterType = attributeData.EntityName;
            Strength = attributeData.Strength;
            Dexterity = attributeData.Dexterity;
            Cunning = attributeData.Cunning;
            Willpower = attributeData.Willpower;
            Magic = attributeData.Magic;
            Constitution = attributeData.Constitution;

            health = new StatusBar(0);
            stamina = new StatusBar(0);
            mana = new StatusBar(0);
        }

        #endregion
    }
}
