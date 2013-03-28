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
            Tileset tileset1 = new Tileset(tilesetTexture, 7, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\walls2");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 7, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);
            int mapWidth = 20;
            int mapHeight = 20;
            CollisionLayer collision = new CollisionLayer(mapWidth, mapHeight);
            MapLayer wall = new MapLayer(mapWidth, mapHeight);
            Random random = new Random();
            //first row
            wall.SetTile(0, 0, new Tile(0, 1));
            wall.SetTile(1, 0, new Tile(1, 1));
            collision.SetTile(0, 0, CollisionType.Unwalkable);
            collision.SetTile(1, 0, CollisionType.Unwalkable);
            for (int i = 2; i < wall.Width - 2; i++)
            {
                int index = random.Next(2, 4);
                Tile tile = new Tile(index, 1);
                wall.SetTile(i, 0, tile);
            }

            wall.SetTile(wall.Width - 2, 0, new Tile(4, 1));
            wall.SetTile(wall.Width-1, 0, new Tile(5, 1));

            collision.SetTile(wall.Width - 2, 0, CollisionType.Unwalkable);
            collision.SetTile(wall.Width - 1, 0, CollisionType.Unwalkable);
            //second row
            random = new Random();
            wall.SetTile(0, 1, new Tile(6, 1));
            wall.SetTile(1, 1, new Tile(7, 1));
            for (int i = 2; i < wall.Width - 2; i++)
            {
                int index = random.Next(8, 10);
                Tile tile = new Tile(index, 1);
                wall.SetTile(i, 1, tile);

            }

            wall.SetTile(wall.Width - 2, 1, new Tile(10, 1));
            wall.SetTile(wall.Width - 1, 1, new Tile(11, 1));
            //third row
            random = new Random();
            wall.SetTile(0, 2, new Tile(12, 1));
            wall.SetTile(1, 2, new Tile(13, 1));
            for (int i = 2; i < wall.Width - 2; i++)
            {
                int index = random.Next(14, 16);
                Tile tile = new Tile(index, 1);
                wall.SetTile(i, 2, tile);
            }

            wall.SetTile(wall.Width - 2, 2, new Tile(16, 1));
            wall.SetTile(wall.Width - 1, 2, new Tile(17, 1));

            //left column

            random = new Random();
            for (int i = 3; i < wall.Height - 2; i++)
            {
                int index = random.Next(0, 2);
                if (index == 0) index = 18;
                else if (index == 1) index = 24;
                Tile tile = new Tile(index, 1);
                wall.SetTile(0, i, tile);
            }

            wall.SetTile(0, wall.Height - 2, new Tile(30, 1));
            wall.SetTile(0, wall.Height - 1, new Tile(36, 1));

            //right column

            random = new Random();
            for (int i = 3; i < wall.Width - 2; i++)
            {
                int index = random.Next(0, 2);
                if (index == 0) index = 23;
                else if (index == 1) index = 29;
                Tile tile = new Tile(index, 1);
                wall.SetTile(wall.Width-1, i, tile);
            }

            wall.SetTile(wall.Width-1, wall.Height - 2, new Tile(35, 1));
            wall.SetTile(wall.Width-1, wall.Height - 1, new Tile(41, 1));

            //last row

            wall.SetTile(1, wall.Height - 1, new Tile(37, 1));
            wall.SetTile(wall.Width - 1, wall.Height - 1, new Tile(41, 1));
            wall.SetTile(wall.Width - 2, wall.Height - 1, new Tile(40, 1));

            wall.SetTile(1, wall.Height - 2, new Tile(31, 1));
            wall.SetTile(wall.Width - 2, wall.Height - 2, new Tile(34, 1));
            random = new Random();
            for (int i = 2; i < wall.Width - 2; i++)
            {
                int index = random.Next(38, 40);
                Tile tile = new Tile(index, 1);
                wall.SetTile(i, wall.Height-1, tile);
            }
            
           // wall.SetTile(wall.Width - 2, wall.Height - 1, new Tile(40, 1));
            //wall.SetTile(wall.Width - 1, wall.Height - 1, new Tile(41, 1));

            MapLayer layer = new MapLayer(mapWidth, mapHeight);
            
            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    Tile tile = new Tile(0, 0);

                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(mapWidth, mapHeight);

            random = new Random();

           for (int i = 0; i < 40; i++)
            {
                int x = random.Next(0, 19);
                int y = random.Next(4, 20);
                int index = random.Next(2, 6);

                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

          //  splatter.SetTile(1, 0, new Tile(1, 1));
          //  splatter.SetTile(2, 0, new Tile(2, 1));
           // splatter.SetTile(3, 0, new Tile(3, 1));

           //map.Collision = new CollisionLayer(mapWidth, mapHeight);
          

            List<MapLayer> mapLayers = new List<MapLayer>();

        /*    System.IO.StreamReader file =
                    new System.IO.StreamReader(@"test.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                Game.Window.Title = line;
                //System.Console.WriteLine(line);
            }*/


            mapLayers.Add(layer);

            mapLayers.Add(splatter);
            mapLayers.Add(wall);
            collision.ProcessWallLayer(wall);

            map = new TileMap(tilesets, mapLayers,collision);
            Level level = new Level(map);


            //THIS IS HOW TO MAKE A TILE UNWALKABLE
          //  map.BlockTile(4, 4);

            ChestData chestData = new ChestData();

            chestData.Name = "Some Chest";
            chestData.MinGold = 10;
            chestData.MaxGold = 101;
            chestData.IsLocked = false;

            Chest chest = new Chest(chestData);

            BaseSprite chestSprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 40, 40),
            new Point(4, 4));
            float radius = 30;
            ItemSprite itemSprite = new ItemSprite(
            chest,
            chestSprite,radius);

            level.Chests.Add(itemSprite);

            world.Levels.Add(level);
            world.CurrentLevel = 0;
        }

        protected void createPlayer()
        {
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\char3");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 50, 50, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 50, 50, 0, 50);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 50, 50, 0, 100);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 50, 50, 0, 150);
            animations.Add(AnimationKey.Up, animation);
            Vector2 start = new Vector2(3 * Engine.TileHeight, 3 * Engine.TileWidth);
            AnimatedSprite sprite = new AnimatedSprite(spriteSheet, animations,start);
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
            player.Update(gameTime,world.Levels[world.CurrentLevel]);
            HandleInteraction();
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

            world.DrawLevel(gameTime, GameRef.SpriteBatch, player.Camera);
            player.Draw(gameTime, GameRef.SpriteBatch);
            base.Draw(gameTime);
            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Other Methods Region

        protected void HandleInteraction()
        {
            if (InputHandler.KeyDown(Keys.Enter) ||
                  InputHandler.ButtonDown(Buttons.B, PlayerIndex.One))
            {
                int ChestId=(world.Levels[world.CurrentLevel]).CheckChestRadius(player.Sprite.Position);
                if(ChestId>=0)
                {
                    Game.Window.Title = "near chest";
                    ItemSprite c = world.Levels[world.CurrentLevel].Chests[ChestId];
                    Chest ch = (Chest)c.InItem;
                    if (ch.IsLocked)
                    {
                        Game.Window.Title = "hooray";
                    }
                    //better to push states here, so handle interaction in the gameplay screen
                }
                else
                    Game.Window.Title = "not near chest"; 
            }
        }

        #endregion
    }
}
