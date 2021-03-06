﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CoreComponents.TileEngine;
using CoreComponents.CharacterClasses;
using CoreComponents.SpriteClasses;
using CoreComponents.ItemClasses;

namespace CoreComponents.WorldClasses
{
    public class Level
    {
        #region Field Region

        readonly TileMap map;
        readonly List<Character> characters;
        readonly List<ItemSprite> chests;

        #endregion

        #region Property Region

        public TileMap Map
        {
            get { return map; }
        }
        public List<Character> Characters
        {
            get { return characters; }
        }
        public List<ItemSprite> Chests
        {
            get { return chests; }
        }
        
        #endregion

        #region Constructor Region

        public Level(TileMap tileMap)
        {
            map = tileMap;
            characters = new List<Character>();
            chests = new List<ItemSprite>();
        }
        #endregion

        #region Method Region


        public void Update(GameTime gameTime)
        {
            foreach (Character character in characters)
                character.Update(gameTime);
            foreach (ItemSprite sprite in chests)
                sprite.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            map.Draw(spriteBatch, camera);
            foreach (Character character in characters)
                character.Draw(gameTime, spriteBatch);
            foreach (ItemSprite sprite in chests)
                sprite.Draw(gameTime, spriteBatch);
        }

        #endregion

        public bool CheckSpriteCollision(Rectangle player)
        {
            for (int i = 0; i < chests.Count; i++)
            {
                if(chests[i].CheckRadius(player))
                {
                    return true;
                }
                
            }
            return false;
        }

        public int CheckInteractionRadius(Vector2 player)
        {
            for (int i = 0; i < chests.Count; i++)
            {
                if (chests[i].CheckInteraction(player))
                {
                    return i;
                }

            }
            return -1;
        }

        public bool CheckUnWalkableTile(Rectangle nextRectangle, Vector2 motion)
        {
            
            if (motion.Y < 0 && motion.X == 0)
            {
                return map.CheckUp(nextRectangle);
            }
            else if (motion.Y == 0 && motion.X < 0)
            {
                return map.CheckLeft(nextRectangle);
            }
            else if (motion.Y == 0 && motion.X > 0)
            {
                return map.CheckRight(nextRectangle);
            }
            else if (motion.Y > 0 && motion.X == 0)
            {
                return map.CheckDown(nextRectangle);
            }

            return false;
        }
    }
}
