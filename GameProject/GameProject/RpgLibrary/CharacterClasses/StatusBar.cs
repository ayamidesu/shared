using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgLibrary.CharacterClasses
{
    public class StatusBar
    {
        #region Field Region

        int currentValue;
        int maximumValue;

        #endregion

        #region Property Region

        public int CurrentValue
        {
            get { return currentValue; }
        }

        public int MaximumValue
        {
            get { return maximumValue; }
        }

        public static StatusBar Zero
        {
            get { return new StatusBar(); }
        }

        #endregion

        #region Constructor Region

        private StatusBar()
        {
            currentValue = 0;
            maximumValue = 0;
        }

        public StatusBar(int maxValue)
        {
            currentValue = maxValue;
            maximumValue = maxValue;
        }

        #endregion

        #region Method Region

        public void Heal(ushort value)
        {
            currentValue += value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void Damage(ushort value)
        {
            currentValue -= value;
            if (currentValue < 0)
                currentValue = 0;
        }

        public void SetCurrent(int value)
        {
            currentValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void SetMaximum(int value)
        {
            maximumValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        #endregion
    }
}
