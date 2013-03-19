using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.CharacterClasses
{
    public class Attributes
    {
        #region Field Region

        public string EntityName;

        public int Strength;
        public int Dexterity;
        public int Cunning;
        public int Willpower;
        public int Magic;
        public int Constitution;

        public string HealthFormula;
        public string StaminaFormula;
        public string MagicFormula;

        #endregion

        #region Constructor Region

        private Attributes()
        {
        }

        #endregion

        #region Static Method Region

        public static void ToFile(string filename)
        {
        }

        public static Attributes FromFile(string filename)
        {
            Attributes character = new Attributes();

            return character;
        }

        #endregion
    }
}
