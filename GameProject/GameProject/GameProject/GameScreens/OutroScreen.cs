using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using CoreComponents;
using CoreComponents.Controls;
using ConversationEngine;
using DialogueEngine;

namespace GameProject.GameScreens
{
    public class OutroScreen : BaseGameState
    {
        bool inDialog;
        public OutroScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            inDialog = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
           

            inDialog = true;

            Conversation.StartConversation(2); //CHANGE THIS!
            base.LoadContent();

        }
        public override void Update(GameTime gameTime)
        {
            if (inDialog)
                Conversation.Update(gameTime);
            if (Conversation.Expired)
            {
                inDialog = false;
                //GameRef.Exit(); //i HAVE TO CHANGE THIS!
            }
            if (!inDialog)
            {
                if(InputHandler.KeyPressed(Keys.Enter))
                {
                    GameRef.Exit();
                }
            }
           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            if (inDialog)
            {
                Conversation.Draw(GameRef.SpriteBatch);
            } 
            SpriteFont font = Game.Content.Load<SpriteFont>(@"Fonts\Segoe");

            if (!inDialog)
            {
                String credits = "Game Engine Programmer: Nancy Tsatsara \n" +
                                    "Character Sprite and Cover: Nicholas Hennaut \n" +
                                    "Graphics: lunar.lostgarden.com \n" +
                                    "Music: Sneaky Snitch, Kevin MacLeod (incompetech.com)\n";
                
                GameRef.SpriteBatch.DrawString(font, credits, new Vector2(30, 30), Color.White);
            }
            GameRef.SpriteBatch.End();
        }

    }
}
