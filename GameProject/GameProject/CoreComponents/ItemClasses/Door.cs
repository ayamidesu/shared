using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class Door : InterItem
    {

        #region Field Region

        static Random Random = new Random();
        List<string> keysRequired;
        bool isLocked;

        #endregion

        #region Property Region

        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        #endregion

        #region Constructor Region

        public Door(string Name)
            : base(Name, "")
        {
        }

        public Door(string Name,List<string> KeysRequired)
            : base(Name, "")
        {
            keysRequired = KeysRequired;
        }

        #endregion

        public void UnlockDoor(List<Key> Inventory)
        {
            if (this.IsLocked)
            {
                int count = 0;
                foreach (String key in keysRequired)
                {
                    foreach (Key InvKey in Inventory)
                    {
                        if (key == InvKey.Name)
                        {
                            count++;
                        }
                    }
                }

                if (count == keysRequired.Count)
                {
                    this.IsLocked = false;
                }
            }
        }


        public override object Clone()
        {
            Door door = new Door(this.Name);
            door.isLocked = this.isLocked;
            foreach (String key in keysRequired)
                door.keysRequired.Add(key);
            return door;
        }
    }
}
