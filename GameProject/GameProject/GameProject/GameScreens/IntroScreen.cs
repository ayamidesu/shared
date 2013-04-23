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
    public class IntroScreen : BaseGameState
    {
        bool inDialog;
         public IntroScreen(Game game, GameStateManager manager)
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
             Conversation.Initialize(Game.Content.Load<SpriteFont>(@"Fonts\Segoe"),
            Game.Content.Load<SoundEffect>(@"SoundEffects\ContinueDialogue"),
            Game.Content.Load<Texture2D>(@"Textures\DialogueBoxBackground"),
            new Rectangle(50, 50, 400, 100),
            Game.Content.Load<Texture2D>(@"Textures\BorderImage"),
            5,
            Color.Black,
            Game.Content.Load<Texture2D>(@"Textures\ConversationContinueIcon"),
            Game.Content);

             // Load Avatars
             DirectoryInfo directoryInfo = new DirectoryInfo(Game.Content.RootDirectory + @"\Avatars\");
             FileInfo[] fileInfo = directoryInfo.GetFiles();
             ArrayList arrayList = new ArrayList();

             foreach (FileInfo fi in fileInfo)
                 arrayList.Add(fi.FullName);

             for (int i = 0; i < arrayList.Count; i++)
             {
                 Conversation.Avatars.Add(Game.Content.Load<Texture2D>(@"Avatars\" + 1));
             }
             foreach (FileInfo fi in fileInfo)
                 arrayList.Add(fi.FullName);


             inDialog = true;

             Conversation.StartConversation(1); //CHANGE THIS!!
             base.LoadContent();

         }
         public override void Update(GameTime gameTime)
         {
             if (inDialog)
                 Conversation.Update(gameTime);
             if (Conversation.Expired)
             {
                 inDialog = false;
                 StateManager.PushState(GameRef.GamePlayScreen);
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
             GameRef.SpriteBatch.End();
         }

    }
}
