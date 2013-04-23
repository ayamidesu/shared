using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using ConversationEngine;
using DialogueEngine;

namespace CoreComponents.ItemClasses
{
    public class LevelItems : InterItem
    {
        #region Field Region

        static Random Random = new Random();
        public string TextureName;
        public bool isLocked;
        public bool canTalk;
        protected bool firstTime;
        public int MinGold;
        public int MaxGold;
        public int requiredItem;
        public List<Key> ItemCollection;
      //  List<string> itemsRequired;
        float chestRadius = 50;
        List<string> keysRequired;
        int ConversationBefore;
        int ConversationAfter;
        int Conversations;

        #endregion

        #region Property Region

        public int conversationAfter
        {
            get { return ConversationAfter; }
            set
            {
                ConversationAfter = value;
            }
        }

        public int conversationBefore
        {
            get { return ConversationBefore; }
            set
            {
                ConversationBefore = value;
            }
        }

        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public bool FirstTime
        {
            get { return firstTime; }
            set { firstTime = value; }
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
            ItemCollection = new List<Key>();
            keysRequired = new List<string>();
        }

        public LevelItems(string name, bool canTalk)
            : base(name, "")
        {
            CanTalk = canTalk;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<Key>();
        }

        public LevelItems(string name,int conversationBefore, int conversationAfter, bool locked)
            : base(name, "")
        {
            isLocked = locked;
            firstTime = true;
            ConversationBefore = conversationBefore;
            ConversationAfter = conversationAfter;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<Key>();
            keysRequired = new List<string>();
            
        }

        public LevelItems(string name, int conversationId ,bool locked)
            : base(name, "")
        {
            isLocked = locked;
            firstTime = true;
            Conversations = conversationId;
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<Key>();
            keysRequired = new List<string>();

        }


        public LevelItems(string name)
            : base(name, "")
        {
            base.InteractionRadius = ChestRadius;
            ItemCollection = new List<Key>();
        }

        

        #endregion

        #region Method Region

  
       public int interact(List<Key> Inventory)
        {

            if(this.FirstTime)
            {
                if (this.IsLocked)
                {
                   this.FirstTime = false;
                   return ConversationBefore;
                }
                else if(!this.IsLocked)
                {
                    //give stuff
                    foreach (Key key in ItemCollection)
                    {
                        Inventory.Add(key);
                    }
                    this.FirstTime = false;
                    int ConvId = conversationAfter;
                    conversationAfter= -1;
                    return ConvId;
                }
            }
            else if(!this.FirstTime)
            {
                if (this.IsLocked)
                {
                    Unlock(Inventory);
                    if (!this.IsLocked)
                    {
                        foreach (Key key in ItemCollection)
                        {
                            Inventory.Add(key);
                        }
                        int ConvId = conversationAfter;
                        conversationAfter = -1;
                        return ConvId;
                    }
                    else
                        return ConversationBefore;
                }
                else
                    return ConversationAfter;
            }
           
         
            return -1;
            
        } 

        
        public void addItem(Key item)
        {
            ItemCollection.Add(item);
        }

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
                    for(int i=0; i<Inventory.Count; i++)
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

        public override object Clone()
        {

            LevelItems chest = new LevelItems(Name,IsLocked,MinGold,MaxGold);
            foreach (Key item in ItemCollection)
                chest.ItemCollection.Add(item);

            chest.firstTime = true;
            chest.isLocked = this.isLocked;
            chest.conversationBefore = this.conversationBefore;
            chest.conversationAfter = this.conversationAfter;
            
            foreach (String key in keysRequired)
                chest.keysRequired.Add(key);
            return chest;
        }
        #endregion
    }
}
