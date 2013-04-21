using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

using CoreComponents;
using CoreComponents.TileEngine;
using GameProject.Components;

using CoreComponents.CharacterClasses;
using CoreComponents.SpriteClasses;
using CoreComponents.WorldClasses;
using CoreComponents.ItemClasses;

using ConversationEngine;
using DialogueEngine;

namespace GameProject.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        Engine engine = new Engine(40, 40);
        //TileMap map;
        Player player;
        World world;
        Party party;
        Texture2D containers;
        Texture2D containers2;
        Texture2D character1;
        Texture2D character2;
        Texture2D bed;
        Texture2D stool;
        Texture2D pot;
        Texture2D cellDoor;
        Texture2D openCellDoor;
        Texture2D guard;
        Texture2D exitOne;
        Texture2D table;
        bool inDialog;
        Song bgEffect;
        //Texture2D charTest;
        //interactiveCharacter jeanValleMatt;

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

        protected override void LoadContent()
        {
            containers = Game.Content.Load<Texture2D>(@"ItemSprites\chestFward");
            containers2 = Game.Content.Load<Texture2D>(@"ItemSprites\chestSide");
            character1 = Game.Content.Load<Texture2D>(@"PlayerSprites\char");
            character2 = Game.Content.Load<Texture2D>(@"PlayerSprites\Wizard");
            guard = Game.Content.Load<Texture2D>(@"PlayerSprites\guards");


            table = Game.Content.Load<Texture2D>(@"ItemSprites\table");
            bed = Game.Content.Load<Texture2D>(@"ItemSprites\prisonerBed1");
            stool = Game.Content.Load<Texture2D>(@"ItemSprites\stool");
            pot = Game.Content.Load<Texture2D>(@"ItemSprites\pot");
            //door = Game.Content.Load<Texture2D>(@"ItemSprites\door");
            cellDoor = Game.Content.Load<Texture2D>(@"ItemSprites\cellDoor");
            openCellDoor = Game.Content.Load<Texture2D>(@"ItemSprites\openCellDoor");

            exitOne = Game.Content.Load<Texture2D>(@"ItemSprites\arrow");

            Conversation.Initialize(Game.Content.Load<SpriteFont>(@"Fonts\Segoe"),
               Game.Content.Load<SoundEffect>(@"SoundEffects\ContinueDialogue"),
               Game.Content.Load<Texture2D>(@"Textures\DialogueBoxBackground"),
               new Rectangle(10, 50, 400, 100),
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

     /*       for (int i = 0; i < arrayList.Count; i++)
            {
                Conversation.Avatars.Add(Game.Content.Load<Texture2D>(@"Avatars\" + 1));
            }
       */
            inDialog = false;

            bgEffect = Game.Content.Load<Song>(@"SoundEffects\SneakySnitch");
            //SoundEffectInstance instance = bgEffect.CreateInstance();
           // instance.IsLooped = true;
            MediaPlayer.Play(bgEffect);
            MediaPlayer.IsRepeating = true;

            //containers = Game.Content.Load<Texture2D>(@"PlayerSprites\char");
            //charTest = Game.Content.Load<Texture2D>(@"PlayerSprites\char");
            createWorld();
            createPlayer();
            createParty();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

           
            //UNCOMMENT IF YOU WANT TO MOVE TO THE SECOND LEVEL BY WALKING TO THE 4TH ROW
            /*
             if (Engine.VectorToCell(player.Sprite.Position).X == 4 && world.CurrentLevel==0)
              {
                  world.CurrentLevel = 1;
                  //function to change player sprites position
                  player.SetPosition(3,3);
              }
             * */
            if(inDialog)
                Conversation.Update(gameTime);
            if (Conversation.Expired)
                inDialog = false;
            if (!inDialog)
            {
                HandleInteraction();
                player.Update(gameTime, world.Levels[world.CurrentLevel]);
                
            }
            base.Update(gameTime);

        }



        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GameRef.SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp, null,
            null,
            null,
            player.Camera.Transformation);
            world.DrawLevel(gameTime, GameRef.SpriteBatch, player.Camera);
            player.Draw(gameTime, GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
            GameRef.SpriteBatch.Begin();
            if (inDialog)
            {
                Conversation.Draw(GameRef.SpriteBatch);
            }
            base.Draw(gameTime);
            GameRef.SpriteBatch.End();
            
        }

        #endregion

        #region Other Methods Region

        protected void createWorld()
        {


            level1();
            level2();
            world.CurrentLevel = 0;
        }

        protected void level1()
        {
            

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset1 = new Tileset(tilesetTexture, 7, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\walls2");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 7, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);
            int mapWidth = 9;
            int mapHeight =9;
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
            wall.SetTile(wall.Width - 1, 0, new Tile(5, 1));

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
                wall.SetTile(wall.Width - 1, i, tile);
            }

            wall.SetTile(wall.Width - 1, wall.Height - 2, new Tile(35, 1));
            wall.SetTile(wall.Width - 1, wall.Height - 1, new Tile(41, 1));

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
                wall.SetTile(i, wall.Height - 1, tile);
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
                   int x = random.Next(0, layer.Width - 1);
                   int y = random.Next(4, layer.Height - 1);
                   int index = random.Next(2, 6);

                   Tile tile = new Tile(index, 0);
                   splatter.SetTile(x, y, tile);
               }

            //  splatter.SetTile(1, 0, new Tile(1, 1));
            //  splatter.SetTile(2, 0, new Tile(2, 1));
            // splatter.SetTile(3, 0, new Tile(3, 1));

            //map.Collision = new CollisionLayer(mapWidth, mapHeight);


            List<MapLayer> mapLayers = new List<MapLayer>();

             mapLayers.Add(layer);

             mapLayers.Add(splatter);
            mapLayers.Add(wall);
            collision.ProcessWallLayer(wall);

            TileMap map = new TileMap(tilesets, mapLayers, collision);
            Level level = new Level(map);

            //THIS IS HOW TO MAKE A TILE UNWALKABLE
            // map.BlockTile(1, 3);



            LevelItems chest = new LevelItems("Some Chest", true, 10, 20);
            chest.addKey("Some Key");

            BaseSprite chestSprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 38, 38),
            new Point(7, 3));

            ItemSprite itemSprite = new ItemSprite(
            chest,
            chestSprite);
            level.LevelItem.Add(itemSprite);

            LevelItems bed1 = new LevelItems("Bed");
            //bed1.addItem("Candle");

            BaseSprite bedSprite = new BaseSprite(
            bed,
            new Rectangle(0, 0, 45, 88),
            new Point(1, 3));

            ItemSprite bedItemSprite = new ItemSprite(
            bed1,
            bedSprite);
            level.LevelItem.Add(bedItemSprite);


            LevelItems stool1 = new LevelItems("Stool");

            BaseSprite benchSprite = new BaseSprite(
            stool,
            new Rectangle(0, 0, 22, 25),
            new Point(1, 5));

            ItemSprite benchItemSprite = new ItemSprite(
            stool1,
            benchSprite);
            level.LevelItem.Add(benchItemSprite);

            LevelItems pot1 = new LevelItems("Pot");

            BaseSprite potSprite = new BaseSprite(
            pot,
            new Rectangle(0, 0, 28, 32),
            new Point(7, 7));

            ItemSprite potItemSprite = new ItemSprite(
            pot1,
            potSprite);
            level.LevelItem.Add(potItemSprite);

           /* LevelItems cDoor = new LevelItems("Door");

            BaseSprite doorSprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 60, 84),
            new Point(5, 1));

            ItemSprite doorItemSprite = new ItemSprite(
            cDoor,
            doorSprite);
            level.LevelItem.Add(doorItemSprite);*/

            Door cDoor = new Door("Door",true,0,1,3,3);

            BaseSprite doorSprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 60, 84),
            new Point(5, 1));

            ItemSprite doorItemSprite = new ItemSprite(
            cDoor,
            doorSprite);
            level.Doors.Add(doorItemSprite);

            cDoor.addKey("Le Key");


            world.Levels.Add(level);




        }

        protected void level2()
        {
            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset1 = new Tileset(tilesetTexture, 7, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\walls2");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 7, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);
            int mapWidth2 = 15;
            int mapHeight2 = 15;
            CollisionLayer collision2 = new CollisionLayer(mapWidth2, mapHeight2);
            MapLayer wall2 = new MapLayer(mapWidth2, mapHeight2);
            Random random = new Random();
            //first row
            wall2.SetTile(0, 0, new Tile(0, 1));
            wall2.SetTile(1, 0, new Tile(1, 1));
            collision2.SetTile(0, 0, CollisionType.Unwalkable);
            collision2.SetTile(1, 0, CollisionType.Unwalkable);
            for (int i = 2; i < wall2.Width - 2; i++)
            {
                int index = random.Next(2, 4);
                Tile tile = new Tile(index, 1);
                wall2.SetTile(i, 0, tile);
            }

            wall2.SetTile(wall2.Width - 2, 0, new Tile(4, 1));
            wall2.SetTile(wall2.Width - 1, 0, new Tile(5, 1));

            collision2.SetTile(wall2.Width - 2, 0, CollisionType.Unwalkable);
            collision2.SetTile(wall2.Width - 1, 0, CollisionType.Unwalkable);
            //second row
            random = new Random();
            wall2.SetTile(0, 1, new Tile(6, 1));
            wall2.SetTile(1, 1, new Tile(7, 1));
            for (int i = 2; i < wall2.Width - 2; i++)
            {
                int index = random.Next(8, 10);
                Tile tile = new Tile(index, 1);
                wall2.SetTile(i, 1, tile);

            }

            wall2.SetTile(wall2.Width - 2, 1, new Tile(10, 1));
            wall2.SetTile(wall2.Width - 1, 1, new Tile(11, 1));
            //third row
            random = new Random();
            wall2.SetTile(0, 2, new Tile(12, 1));
            wall2.SetTile(1, 2, new Tile(13, 1));
            for (int i = 2; i < wall2.Width - 2; i++)
            {
                int index = random.Next(14, 16);
                Tile tile = new Tile(index, 1);
                wall2.SetTile(i, 2, tile);
            }

            wall2.SetTile(wall2.Width - 2, 2, new Tile(16, 1));
            wall2.SetTile(wall2.Width - 1, 2, new Tile(17, 1));

            //left column

            random = new Random();
            for (int i = 3; i < wall2.Height - 2; i++)
            {
                int index = random.Next(0, 2);
                if (index == 0) index = 18;
                else if (index == 1) index = 24;
                Tile tile = new Tile(index, 1);
                wall2.SetTile(0, i, tile);
            }

            wall2.SetTile(0, wall2.Height - 2, new Tile(30, 1));
            wall2.SetTile(0, wall2.Height - 1, new Tile(36, 1));

            //right column

            random = new Random();
            for (int i = 3; i < wall2.Width - 2; i++)
            {
                int index = random.Next(0, 2);
                if (index == 0) index = 23;
                else if (index == 1) index = 29;
                Tile tile = new Tile(index, 1);
                wall2.SetTile(wall2.Width - 1, i, tile);
            }

            wall2.SetTile(wall2.Width - 1, wall2.Height - 2, new Tile(35, 1));
            wall2.SetTile(wall2.Width - 1, wall2.Height - 1, new Tile(41, 1));

            //last row

            wall2.SetTile(1, wall2.Height - 1, new Tile(37, 1));
            wall2.SetTile(wall2.Width - 1, wall2.Height - 1, new Tile(41, 1));
            wall2.SetTile(wall2.Width - 2, wall2.Height - 1, new Tile(40, 1));

            wall2.SetTile(1, wall2.Height - 2, new Tile(31, 1));
            wall2.SetTile(wall2.Width - 2, wall2.Height - 2, new Tile(34, 1));
            random = new Random();
            for (int i = 2; i < wall2.Width - 2; i++)
            {
                int index = random.Next(38, 40);
                Tile tile = new Tile(index, 1);
                wall2.SetTile(i, wall2.Height - 1, tile);
            }


            MapLayer layer2 = new MapLayer(mapWidth2, mapHeight2);

            for (int y = 0; y < layer2.Height; y++)
            {
                for (int x = 0; x < layer2.Width; x++)
                {
                    Tile tile = new Tile(0, 0);

                    layer2.SetTile(x, y, tile);
                }
            }

            MapLayer splatter2 = new MapLayer(mapWidth2, mapHeight2);

            random = new Random();

            for (int i = 0; i < 40; i++)
            {
                int x = random.Next(0, layer2.Width - 1);
                int y = random.Next(4, layer2.Height - 1);
                int index = random.Next(2, 6);

                Tile tile = new Tile(index, 0);
                splatter2.SetTile(x, y, tile);
            }


            List<MapLayer> mapLayers2 = new List<MapLayer>();



            mapLayers2.Add(layer2);

            mapLayers2.Add(splatter2);
            mapLayers2.Add(wall2);
            collision2.ProcessWallLayer(wall2);

            TileMap map2 = new TileMap(tilesets, mapLayers2, collision2);
            Level level2 = new Level(map2);

            LevelItems chest2 = new LevelItems("Some Chest", true, 10, 60);
            chest2.addKey("Some Key");

            BaseSprite chest2Sprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 38, 38),
            new Point(3, 6));

            ItemSprite item2Sprite = new ItemSprite(
            chest2,
            chest2Sprite);
            level2.LevelItem.Add(item2Sprite);

            LevelItems chest3 = new LevelItems("Some Chest", true, 10, 20);
            chest3.addKey("Some Key");

            BaseSprite chest3Sprite = new BaseSprite(
            containers2,
            new Rectangle(0, 0, 38, 38),
            new Point(1, 13));

            ItemSprite item3Sprite = new ItemSprite(
            chest3,
            chest3Sprite);
            level2.LevelItem.Add(item3Sprite);

            LevelItems openCell1 = new LevelItems("Your cell", false);

            BaseSprite openCellDoorSprite = new BaseSprite(
            openCellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(3, 1));

            ItemSprite openCellDoorItemSprite = new ItemSprite(
            openCell1,
            openCellDoorSprite);
            level2.LevelItem.Add(openCellDoorItemSprite);



            LevelItems table1 = new LevelItems("table", false);

            BaseSprite table1Sprite = new BaseSprite(
            table,
            new Rectangle(0, 0, 120, 125),
            new Point(6, 6));

            ItemSprite tableItemSprite = new ItemSprite(
            table1,
            table1Sprite);
            level2.LevelItem.Add(tableItemSprite);


            LevelItems cellDoor2 = new LevelItems("Second Cell Door", false);

            BaseSprite cellDoor2Sprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(5, 1));

            ItemSprite cellDoorTwoItemSprite = new ItemSprite(
            cellDoor2,
            cellDoor2Sprite);
            level2.LevelItem.Add(cellDoorTwoItemSprite);

            //open cell door that links back to first room 

            LevelItems openCell2 = new LevelItems("Open cell", false);

            BaseSprite openCell2DoorSprite = new BaseSprite(
            openCellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(7, 1));

            ItemSprite openCellDoor2ItemSprite = new ItemSprite(
            openCell2,
            openCell2DoorSprite);
            level2.LevelItem.Add(openCellDoor2ItemSprite);

            LevelItems cellDoor3 = new LevelItems("Third Cell Door", true);

            BaseSprite cellDoor3Sprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(9, 1));

            ItemSprite cellDoorThreeItemSprite = new ItemSprite(
            cellDoor3,
            cellDoor3Sprite);
            level2.LevelItem.Add(cellDoorThreeItemSprite);


            // create an interactive character on the screen
            LevelItems char1 = new LevelItems("Jean Val Matt",1,2, true);
            //char1.requiredItem("Some Item");
            //LevelItems char2 = new LevelItems("Merlan", 1,2,true);

            char1.addKey("Some Second Key");
            char1.addKey("Some Key");
            Key item = new Key("wizard's hat");
            char1.addItem(item);
            item = new Key("wizard's hat 2");
            char1.addItem(item);

            BaseSprite characterSprite2 = new BaseSprite(
            character2,
            new Rectangle(0, 0, 68, 62),
            new Point(12, 3));
            ItemSprite charItemSprite2 = new ItemSprite(
            char1,
            characterSprite2);
            level2.LevelItem.Add(charItemSprite2);


            LevelItems guard1 = new LevelItems("Guard 1", 1,2,true);
            //guard1.requiredItem("Some Item");
            guard1.addKey("Some Key");

            BaseSprite guardSprite1 = new BaseSprite(
            guard,
            new Rectangle(35, 0, 31, 32),
            new Point(13, 7));
            ItemSprite guardCharSprite1 = new ItemSprite(
            guard1,
            guardSprite1);
            level2.LevelItem.Add(guardCharSprite1);

            // exit between guards
            LevelItems exitArrow1 = new LevelItems("Exit to next room (2)");

            BaseSprite exitSprite1 = new BaseSprite(
            exitOne,
            new Rectangle(0, 0, 49, 38),
            new Point(13, 8));
            ItemSprite exitArrowSprite1 = new ItemSprite(
            exitArrow1,
            exitSprite1);
            level2.LevelItem.Add(exitArrowSprite1);

            LevelItems guard2 = new LevelItems("Guard 2", true);
            //guard2.requiredItem("Some Item");

            BaseSprite guardSprite2 = new BaseSprite(
            guard,
            new Rectangle(32, 32, 31, 32),
            new Point(13, 9));
            ItemSprite guardCharSprite2 = new ItemSprite(
            guard2,
            guardSprite2);
            level2.LevelItem.Add(guardCharSprite2);

            world.Levels.Add(level2);

        }

        protected void createPlayer()
        {
            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"PlayerSprites\char4");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 50, 50, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 50, 50, 0, 50);
            animations.Add(AnimationKey.Up, animation);
            animation = new Animation(3, 50, 50, 0, 100);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 50, 50, 0, 150);
            animations.Add(AnimationKey.Left, animation);
            Vector2 start = new Vector2(3 * Engine.TileHeight, 3 * Engine.TileWidth);
            AnimatedSprite sprite = new AnimatedSprite(spriteSheet, animations, start);
            player = new Player(GameRef, sprite);

        }

        protected void createParty()
        {
            party = new Party();
            Weapon wep = new Weapon("Awesomeness", "Sword", 300, 1, Hands.One, 100, 10, 200, 2);
            party.addWeapon(wep);

            Key key = new Key("Scepter");
            party.addKey(key);
            key = new Key("Some Key");
            party.addKey(key);
            key = new Key("Le Key");
            party.addKey(key);
            Key key2 = new Key("Some Second Key");
            party.addKey(key2);
        }


        protected void HandleInteraction()
        {
            if (InputHandler.KeyDown(Keys.I))
            {
                InventoryScreen InventoryS = new InventoryScreen(GameRef,base.StateManager,party);
                StateManager.PushState(InventoryS);
            }

            if (InputHandler.KeyDown(Keys.Enter))
            {
                int item2ID = (world.Levels[world.CurrentLevel]).CheckInteractionRadius(player.Sprite.Position);
                if (item2ID >= 0)
                {
                    LevelItems level2Item = (LevelItems)world.Levels[world.CurrentLevel].LevelItem[item2ID].InItem;
                    int ConvID=level2Item.interact(party.keyInv);
                    if(ConvID>=0)
                    {
                        inDialog = true;
                        Conversation.StartConversation(ConvID);
                    }
                   
                }
                int DoorID = (world.Levels[world.CurrentLevel]).CheckDoorRadius(player.Sprite.Position);
                if ( DoorID >= 0)
                {

                   // Game.Window.Title = "Door";
                    Door door = (Door)world.Levels[world.CurrentLevel].Doors[DoorID].InItem;
                    int level = door.interact(party.keyInv);
                    if (level >= 0)
                    {
                        if (!door.IsLocked)
                        {
                            BaseSprite doorSprite = new BaseSprite(
                                openCellDoor,
                                new Rectangle(0, 0, 60, 84),
                                new Point(5, 1));
                            world.Levels[world.CurrentLevel].Doors[DoorID].Sprite = doorSprite;
                        }

                        world.CurrentLevel = level;
                        player.SetPosition(door.nextLevelX, door.nextLevelY);
                    }
                    
                }

            }

        }

        #endregion
    }
}