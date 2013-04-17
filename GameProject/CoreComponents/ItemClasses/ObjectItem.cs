using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class ObjectItem : InterItem
    {
        #region Field Region

        //static Random Random = new Random();
        public string TextureName;
        //public int MinGold;
        //public int MaxGold;
        public List<BaseItem> ItemCollection;
        float itemRadius = 50;
        //List<string> keysRequired;

        #endregion

        #region Property Region

       
        public float ItemRadius
        {
            get { return itemRadius; }
        }

        #endregion

        #region Constructor Region

        public ObjectItem(string name)
            : base(name, "")
        {
            base.InteractionRadius = ItemRadius;
            ItemCollection = new List<BaseItem>();
        }



        #endregion

        #region Method Region


        public void addItem(BaseItem item)
        {
            ItemCollection.Add(item);
        }

        public override object Clone()
        {

            ObjectItem objItem = new ObjectItem(Name);
            foreach (BaseItem item in ItemCollection)
                objItem.ItemCollection.Add(item);

            return objItem;
        }
        #endregion
    }
}
