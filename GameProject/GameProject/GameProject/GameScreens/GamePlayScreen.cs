using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using CoreComponents;
using CoreComponents.TileEngine;
using GameProject.Components;
using CoreComponents.SpriteClasses;
using CoreComponents.WorldClasses;
using CoreComponents.ItemClasses;

namespace GameProject.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        Engine engine = new Engine(40, 40);
        TileMap map;
        Player player;
        AnimatedSprite sprite;
        World world;
        Texture2D containers;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            world = new World(game, GameRef.ScreenRectangle);
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected void createWorld()
        {

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset1 = new Tileset(tilesetTexture, 6, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 1, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);

            MapLayer layer = new MapLayer(40, 40);

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(0, 0);

                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(40, 40);

            Random random = new Random();

            for (int i = 0; i < 80; i++)
            {
                int x = random.Next(0, 40);
                int y = random.Next(0, 40);
                int index = random.Next(2, 6);

                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            splatter.SetTile(1, 0, new Tile(1, 1));
            splatter.SetTile(2, 0, new Tile(2, 1));
            splatter.SetTile(3, 0, new Tile(3, 1));

            List<MapLayer> mapLayers = new List<MapLayer>();
            mapLayers.Add(layer);
            mapLayers.Add(splatter);

            map = new TileMap(tilesets, mapLayers);
            Level level = new Level(map);

            ChestData chestData = new ChestData();

            chestData.Name = "Some Chest";
            chestData.MinGold = 10;
            chestData.MaxGold = 101;

            Chest chest = new Chest(chestData);

            BaseSprite chestSprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 40, 40),
            new Point(10, 10));

            ItemSprite itemSprite = new ItemSprite(
            chest,
            chestSprite);

            level.Chests.Add(itemSprite);

            world.Levels.Add(level);
            world.CurrentLevel = 0;
        }

        protected void createPlayer()
        {
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\char2");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 50, 50, 150, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 50, 50, 0, 0);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 50, 50, 0, 0);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 50, 50, 150, 0);
            animations.Add(AnimationKey.Up, animation);
            sprite = new AnimatedSprite(spriteSheet, animations);
            player = new Player(GameRef, sprite);
            
        }

        protected override void LoadContent()
        {

            base.LoadContent();
            containers = Game.Content.Load<Texture2D>(@"ItemSprites\chest");
            createPlayer();
            createWorld();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp, null,
            null,
            null,
            player.Camera.Transformation);
            //map.Draw(GameRef.SpriteBatch, player.Camera);
            //sprite.Draw(gameTime, GameRef.SpriteBatch, player.Camera);

            world.DrawLevel(gameTime, GameRef.SpriteBatch, player.Camera);
            player.Draw(gameTime, GameRef.SpriteBatch);
            base.Draw(gameTime);
            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Abstract Method Region
        #endregion
    }
}
