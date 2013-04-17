using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class LevelItems : InterItem
    {
        #region Field Region

        static Random Random = new Random();
        public string TextureName;
        public bool isLocked;
        public bool canTalk;
        public int MinGold;
        public int MaxGold;
        public int requiredItem;
        public List<BaseItem> ItemCollection;
        List<string> itemsRequired;
        float chestRadius = 50;
        List<string> keysRequired;

        #endregion

        #region Property Region

        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public bool CanTalk
        {
            get { return canTalk ; }
            set { canTalk = value; }
        }

        public float ChestRadius
        {
            get { return chestRadius; }
        }




        /*public void requiredItem(string item)
        {
            itemsRequired.Add(item);
        }*/

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
     
        
        public LevelItems(string name,bool locked, int minGold, int maxGold)
            : base(name, "")
        {
            IsLocked = locked;
            MinGold = minGold;
            MaxGold = maxGold;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<BaseItem>();
            keysRequired = new List<string>();
        }

        public LevelItems(string name, bool canTalk)
            : base(name, "")
        {
            CanTalk = canTalk;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<BaseItem>();
        }


        public LevelItems(string name)
            : base(name, "")
        {
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<BaseItem>();
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


        // add DIALOGUE information for talking characters

        public override object Clone()
        {

            LevelItems chest = new LevelItems(Name,IsLocked,MinGold,MaxGold);
            foreach (BaseItem item in ItemCollection)
                chest.ItemCollection.Add(item);

            LevelItems objItem = new LevelItems(Name);
            foreach (BaseItem item in ItemCollection)
                objItem.ItemCollection.Add(item);          
            
            foreach (String key in keysRequired)
                chest.keysRequired.Add(key);
            return chest;
        }
        #endregion
    }
}
