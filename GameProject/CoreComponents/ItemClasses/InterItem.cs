using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CoreComponents.ItemClasses
{
   
    public abstract class InterItem
    {
        #region Field Region

        string name;
        string type;
        float interactionRadius;

        #endregion

        #region Property Region

      
        public string Type
        {
            get { return type; }
            protected set { type = value; }
        }

        public float InteractionRadius
        {
            get { return interactionRadius; }
            protected set { interactionRadius = value; }
        }

        public string Name
        {
            get { return name; }
            protected set { name = value; }
        }
       

        #endregion

        #region Constructor Region

        public InterItem(string name, string type)
        {
            Name = name;
            Type = type;
        }

        #endregion

        #region Abstract Method Region

        public abstract object Clone();

        
        public override string ToString()
        {
            string itemString = "";

            itemString += Name + ", ";
            itemString += Type + ", ";

            return itemString;
        }

        
        #endregion
    }
}
