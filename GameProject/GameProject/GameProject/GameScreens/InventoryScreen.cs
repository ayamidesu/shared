using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CoreComponents;
using CoreComponents.Controls;
using CoreComponents.CharacterClasses;
using CoreComponents.ItemClasses;

namespace GameProject.GameScreens
{
    public class InventoryScreen : BaseGameState
    {
        #region Field region

        PictureBox backgroundImage;
        PictureBox arrowImage;
        List<LinkLabel> Inventory;
        float maxItemWidth = 0f;
        //float totalItemWidth = 10f;
        Party party;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public InventoryScreen(Game game, GameStateManager manager, Party Party)
            : base(game, manager)
        {
            party = Party;
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager Content = Game.Content;

            backgroundImage = new PictureBox(
                Content.Load<Texture2D>(@"Backgrounds\inventory"),
                GameRef.ScreenRectangle);
            ControlManager.Add(backgroundImage);

            Texture2D arrowTexture = Content.Load<Texture2D>(@"GUI\leftarrowUp");

            arrowImage = new PictureBox(
                arrowTexture,
                new Rectangle(
                    0,
                    0,
                    arrowTexture.Width,
                    arrowTexture.Height));
            ControlManager.Add(arrowImage);

            Inventory = new List<LinkLabel>();

            foreach (Key key in party.keyInv)
            {
                LinkLabel label = new LinkLabel(Color.Chocolate);
                label.Text = key.Name;
                label.Size = label.SpriteFont.MeasureString(label.Text);
                Inventory.Add(label);
                ControlManager.Add(label);
            }

            ControlManager.NextControl();

            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);


            Vector2 position = new Vector2(55, 120);
            Vector2 position2 = new Vector2(300, 120);

            for (int count = 0; count < ControlManager.Count; count++)
            {
                Control itemControl = ControlManager[count];

                if (itemControl is LinkLabel)
                {
                    if (itemControl.Size.X > maxItemWidth)
                    {
                        maxItemWidth = itemControl.Size.X;
                    }
                    if (count <= 11)
                    {
                        itemControl.Position = position;
                        position.Y += itemControl.Size.Y + 5f;
                    }
                    else if (count >= 12)
                    {
                        itemControl.Position = position2;
                        position2.Y += itemControl.Size.Y + 5f;
                    }
                }
            }

            ControlManager_FocusChanged(Inventory[0], null);
        }

        void ControlManager_FocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Vector2 position = new Vector2(control.Position.X + maxItemWidth + 30f, control.Position.Y);
            arrowImage.SetPosition(position);
        }

        private void menuItem_Selected(object sender, EventArgs e)
        {

        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, playerIndexInControl);
          
            if (InputHandler.KeyPressed(Keys.I))
            {
                StateManager.PopState();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            ControlManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        #endregion


    }
}
