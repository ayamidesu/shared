using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.ItemClasses
{
    public class Key : InterItem
    {
        #region Field Region

        static Random Random = new Random();

        #endregion

        #region Property Region

        #region Constructor Region

      

        #endregion

        public Key(string name)
            : base(name, "")
        {
            
        }

        #endregion


        public override object Clone()
        {
            Key key = new Key(this.Name);
            return key;
        }

    }

}