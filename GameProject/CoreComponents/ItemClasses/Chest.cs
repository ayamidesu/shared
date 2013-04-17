using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class Chest : InterItem
    {
        #region Field Region

        static Random Random = new Random();
        public string TextureName;
        public bool isLocked;
        public int MinGold;
        public int MaxGold;
        public List<BaseItem> ItemCollection;
        float chestRadius = 50;
        List<string> keysRequired;

        #endregion

        #region Property Region

        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public float ChestRadius
        {
            get { return chestRadius; }
        }

        public int Gold
        {
            get
            {
                if (MinGold == 0 && MaxGold == 0)
                    return 0;
                int gold = Random.Next(MinGold, MaxGold);
                MinGold = 0;
                MaxGold = 0;
                return gold;
            }
        }

        #endregion

        #region Constructor Region

        public Chest(string name,bool locked,int minGold,int maxGold)
            : base(name, "")
        {
            IsLocked = locked;
            MinGold = minGold;
            MaxGold = maxGold;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<BaseItem>();
            keysRequired = new List<string>();
        }

         

        #endregion

        #region Method Region

        
        public void addItem(BaseItem item)
        {
            ItemCollection.Add(item);
        }

        public void addKey(string key)
        {
            keysRequired.Add(key);
        }

        public void UnlockChest(List<Key> Inventory)
        {
            if (this.IsLocked)
            {
                int count = 0;
                foreach (string keyname in keysRequired)
                {
                    foreach (Key InvKey in Inventory)
                    {
                        if (keyname == InvKey.Name)
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

            Chest chest = new Chest(Name,IsLocked,MinGold,MaxGold);
            foreach (BaseItem item in ItemCollection)
                chest.ItemCollection.Add(item);

            foreach (String key in keysRequired)
                chest.keysRequired.Add(key);
            return chest;
        }
        #endregion
    }
}
