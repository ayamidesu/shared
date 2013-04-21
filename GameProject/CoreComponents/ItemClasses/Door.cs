using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class Door : InterItem
    {

        #region Field Region

        List<string> keysRequired;
        bool isLocked;
        int inLevel;
        int toLevel;
        int NextLevelX;
        int NextLevelY;

        #endregion

        #region Property Region

        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public int nextLevelX
        { get { return NextLevelX; } }

        public int nextLevelY
        { get { return NextLevelY; } }

        #endregion

        #region Constructor Region

        public Door(string Name)
            : base(Name, "")
        {

        }

        public Door(string Name,bool locked, int FromLevel, int ToLevel,int nextX,int nextY)
            : base(Name, "")
        {
            inLevel = FromLevel;
            toLevel = ToLevel;
            isLocked = locked;
            NextLevelX = nextX;
            NextLevelY = nextY;
            keysRequired = new List<string>();

            base.InteractionRadius = 85;
        }

        #endregion

        public void addKey(string key)
        {
            keysRequired.Add(key);
        }

        public void Unlock(List<Key> Inventory)
        {
            if (this.IsLocked)
            {
                int count = 0;
                int position = -1;
                foreach (string keyname in keysRequired)
                {
                    for (int i = 0; i < Inventory.Count; i++)
                    {
                        Key InvKey = Inventory[i];
                        if (keyname == InvKey.Name)
                        {
                            count++;
                            position = i;
                        }

                    }
                    if (position >= 0)
                    {
                        Inventory.RemoveAt(position);
                        position = -1;
                    }
                }

                if (count == keysRequired.Count)
                {
                    this.IsLocked = false;
                }
            }
        }

        public int interact(List<Key> Inventory)
        {
            if (this.isLocked)
            {
                Unlock(Inventory);
                if(isLocked)
                    return -1;
            }
            return toLevel;

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


           