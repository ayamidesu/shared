using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CoreComponents;
using CoreComponents.TileEngine;
using CoreComponents.SpriteClasses;

namespace GameProject.Components
{
    public class Player
    {
        #region Field Region

        Camera camera;
        Game1 gameRef;
        readonly AnimatedSprite sprite;

        #endregion

        #region Property Region

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        #endregion

        #region Constructor Region

        public Player(Game game, AnimatedSprite sprite)
        {
            gameRef = (Game1)game;
            camera = new Camera(gameRef.ScreenRectangle);
            this.sprite = sprite;
        }
        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            sprite.Update(gameTime);
            Vector2 motion = new Vector2();

            if (InputHandler.KeyDown(Keys.W) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickUp, PlayerIndex.One))
            {
                sprite.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickDown, PlayerIndex.One))
            {
                sprite.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }

            if (InputHandler.KeyDown(Keys.A) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, PlayerIndex.One))
            {
                sprite.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickRight, PlayerIndex.One))
            {
                sprite.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
            {
                sprite.IsAnimating = true;
                motion.Normalize();

                sprite.Position += motion * sprite.Speed;
                sprite.LockToMap();
                camera.LockToSprite(sprite);
            }
            else
            {
                sprite.IsAnimating = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            sprite.Draw(gameTime, spriteBatch, camera);
        }

        #endregion
    }
}
