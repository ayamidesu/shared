using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class ChestData
    {
       
        public string Name;
        public string TextureName;
        public int Size;
        public bool IsLocked;
        public int MinGold;
        public int MaxGold;
        public List<BaseItem> ItemCollection;

        public ChestData()
        {
            ItemCollection = new List<BaseItem>();
        }

        public ChestData(List<BaseItem> list) 
        {
            ItemCollection = list;
        }

        public void addItem(BaseItem item)
        {
            ItemCollection.Add(item);
        }

        public override string ToString()
        {
            string toString = Name + ", ";
            toString += TextureName + ", ";
            toString += IsLocked.ToString() + ", ";
            toString += MinGold.ToString() + ", ";
            toString += MaxGold.ToString();
            foreach (BaseItem item in ItemCollection)
            {
                toString += item.Name;
            }
            return toString;
        }
    }
}
