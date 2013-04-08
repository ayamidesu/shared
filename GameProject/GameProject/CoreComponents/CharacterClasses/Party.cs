using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CoreComponents.ItemClasses;

namespace CoreComponents.CharacterClasses
{
    public class Party
    {
        List<Character> characters;
        List<Weapon> WeaponInv;
        List<Armor> ArmorInv;
        List<Key> KeyInv;

        public List<Key> keyInv
        {
            get { return KeyInv; }
        }

        public Party() 
        {
            WeaponInv = new List<Weapon>();
            ArmorInv = new List<Armor>();
            KeyInv = new List<Key>();
            characters = new List<Character>();

        }

        public void addWeapon(Weapon weapon){
            WeaponInv.Add(weapon);
        }

        public void addArmor(Armor armor)
        {
            ArmorInv.Add(armor);
        }

        public void addKey(Key key)
        {
            KeyInv.Add(key);
        }




    }
}
