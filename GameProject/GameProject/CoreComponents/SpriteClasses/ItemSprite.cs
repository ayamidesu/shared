using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CoreComponents.ItemClasses;
using CoreComponents.SpriteClasses;

namespace CoreComponents.SpriteClasses
{
    public class ItemSprite
    {
        #region Field Region

        BaseSprite sprite;
        InterItem initem;
        BaseItem item;
        float collisionRadius; //not sure where collission variable should go, subject to change

        #endregion

        #region Property Region
        public BaseSprite Sprite
        {
            get { return sprite; }
        }
        public BaseItem Item
        {
            get { return item; }
        }
        public InterItem InItem
        {
            get { return initem; }
        }
        public virtual float CollisionRadius
        {
            get { return collisionRadius; }
            set { collisionRadius = value; }
        }
        #endregion

        #region Constructor Region
        public ItemSprite(BaseItem item, BaseSprite sprite, float Radius)
        {
            this.item = item;
            this.sprite = sprite;
            this.collisionRadius = Radius;
        }
        public ItemSprite(InterItem item, BaseSprite sprite, float Radius)
        {
            this.initem = item;
            this.sprite = sprite;
            this.collisionRadius = Radius;
        }
        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }
        #endregion

        public bool CheckRadius(Vector2 position)
        {
            float distance = Vector2.Distance(
                 sprite.Position,
                 position);
            if (distance < collisionRadius)
            {
                return true;
            }
            return false;
        }
    }
}
