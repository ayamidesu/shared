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
        #endregion

        #region Constructor Region
        public ItemSprite(BaseItem item, BaseSprite sprite)
        {
            this.item = item;
            this.sprite = sprite;
        }
        public ItemSprite(InterItem item, BaseSprite sprite)
        {
            this.initem = item;
            this.sprite = sprite;
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

        public bool CheckRadius(Rectangle player)
        {

            Rectangle item = sprite.Rectangle;
            player.X += 25;
            player.Y += 25;
            item.X += item.Width / 2;
            item.Y += item.Height / 2;
            bool collide = (Math.Abs(player.X - item.X) * 2 < (player.Width + item.Width)) &&
                (Math.Abs(player.Y - item.Y) * 2 < (player.Height + item.Height));
            return collide;
           /* Vector2 position;
            position.X=player.X;
            position.Y=player.Y;
            if ((position - sprite.Position).Length() < (Math.Abs(player.Y * player.Height / 2) + Math.Abs(item.Y * item.Height / 2)))
            {
                return false;
            }
            return true;*/
            
            
        }

        public bool CheckInteraction(Vector2 player)
        {

            float distance = Vector2.Distance(
                 sprite.Position,
                 player);
            if (distance <= initem.InteractionRadius)
            {
                return true;
            }
            return false;
        }
    }
}
