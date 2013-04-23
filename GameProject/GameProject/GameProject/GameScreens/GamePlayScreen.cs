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
        Player player;
        World world;
        Party party;
        Song bgEffect;
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
        Texture2D crate;
        Texture2D char2;
        Texture2D exitLoc;
        Texture2D plaque;
        bool inDialog;

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
            char2 = Game.Content.Load<Texture2D>(@"PlayerSprites\largeCharacterSheet");

            crate = Game.Content.Load<Texture2D>(@"ItemSprites\crate1");
            table = Game.Content.Load<Texture2D>(@"ItemSprites\table");
            bed = Game.Content.Load<Texture2D>(@"ItemSprites\prisonerBed1");
            stool = Game.Content.Load<Texture2D>(@"ItemSprites\stool");
            pot = Game.Content.Load<Texture2D>(@"ItemSprites\pot");
            plaque = Game.Content.Load<Texture2D>(@"ItemSprites\plaque");
            exitLoc = Game.Content.Load<Texture2D>(@"ItemSprites\EXIT");
            
            
            //door = Game.Content.Load<Texture2D>(@"ItemSprites\door");
            cellDoor = Game.Content.Load<Texture2D>(@"ItemSprites\cellDoor");
            openCellDoor = Game.Content.Load<Texture2D>(@"ItemSprites\openCellDoor");

            exitOne = Game.Content.Load<Texture2D>(@"ItemSprites\arrow");

           
            inDialog = false;

            //load background music
            bgEffect = Game.Content.Load<Song>(@"SoundEffects\SneakySnitch");
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

           
          
            if(inDialog)
                Conversation.Update(gameTime);
            if (Conversation.Expired)
                inDialog = false;
            if (!inDialog)
            {
                HandleInteraction();
                player.Update(gameTime, world.Levels[world.CurrentLevel]);
                base.Update(gameTime);
            }

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

            GameRef.SpriteBatch.End();
            GameRef.SpriteBatch.Begin();
            if (inDialog)
            {
                Conversation.Draw(GameRef.SpriteBatch);
            }
            //activeItem.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
            base.Draw(gameTime);
            
        }

        #endregion

        #region Other Methods Region

        protected void createWorld()
        {
            level1();
            level3();
            level4();
            level2();
            world.CurrentLevel = 2;
        }

        #region Level1Code
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

            LevelItems chest = new LevelItems("Chest Room 1", 7, 8, true);
            chest.addKey("Yellow Key");
            Key greenKey = new Key("Green Key");
            chest.addItem(greenKey);
            Key sword = new Key("Sword");
            chest.addItem(sword);

            BaseSprite chestSprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 38, 38),
            new Point(7, 3));

            ItemSprite itemSprite = new ItemSprite(
            chest,
            chestSprite);
            level.LevelItem.Add(itemSprite);

            LevelItems bed1 = new LevelItems("Bed", 36, 9, true);

            Key jailCellKey = new Key("Jail Cell Key");
            bed1.addItem(jailCellKey);

            BaseSprite bedSprite = new BaseSprite(
            bed,
            new Rectangle(0, 0, 45, 88),
            new Point(1, 3));

            ItemSprite bedItemSprite = new ItemSprite(
            bed1,
            bedSprite);
            level.LevelItem.Add(bedItemSprite);


            LevelItems stool1 = new LevelItems("Stool", 28, 29, true);

            BaseSprite benchSprite = new BaseSprite(
            stool,
            new Rectangle(0, 0, 22, 25),
            new Point(1, 5));

            ItemSprite benchItemSprite = new ItemSprite(
            stool1,
            benchSprite);
            level.LevelItem.Add(benchItemSprite);

            LevelItems pot1 = new LevelItems("Pot", -1, 30, false);
            
            Key redKey = new Key("Red Key");
            pot1.addItem(redKey);
            Key potatoSkin = new Key("Potato Skin");
            pot1.addItem(potatoSkin);

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

            Door cDoor = new Door("Your Door",true, 0,3,3,3);
            cDoor.addKey("Jail Cell Key");

            BaseSprite doorSprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 60, 84),
            new Point(5, 1));

            ItemSprite doorItemSprite = new ItemSprite(
            cDoor,
            doorSprite);
            level.Doors.Add(doorItemSprite);

            //cDoor.addKey("Le Key");


            world.Levels.Add(level);

        }
        #endregion
        #region Level2 Code

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

            LevelItems chest2 = new LevelItems("Chest 1 Level 2", 11, 12, true);
            chest2.addKey("Red Key");

            Key yellowKey = new Key("Yellow Key");
            chest2.addItem(yellowKey);
            Key wizardsHat = new Key("Wizards Hat");
            chest2.addItem(wizardsHat);

            
            BaseSprite chest2Sprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 38, 38),
            new Point(3, 6));

            ItemSprite item2Sprite = new ItemSprite(
            chest2,
            chest2Sprite);
            level2.LevelItem.Add(item2Sprite);

            LevelItems chest3 = new LevelItems("Chest 2 Level 2", 13, 14, true);
            chest3.addKey("Blue Key");
            Key breastplateOfWomanliness = new Key("Breastplate");
            chest3.addItem(breastplateOfWomanliness);

            BaseSprite chest3Sprite = new BaseSprite(
            containers2,
            new Rectangle(0, 0, 38, 38),
            new Point(1, 13));

            ItemSprite item3Sprite = new ItemSprite(
            chest3,
            chest3Sprite);
            level2.LevelItem.Add(item3Sprite);

            //cDoor.addKey("Le Key");

            Door openCell1 = new Door("Your cell", true, 1, 0, 5, 4);

            BaseSprite openCellDoorSprite = new BaseSprite(
            openCellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(3, 1));

            ItemSprite openCellDoorItemSprite = new ItemSprite(
            openCell1,
            openCellDoorSprite);
            level2.Doors.Add(openCellDoorItemSprite);



            LevelItems table1 = new LevelItems("table", -1, 26, false);

            BaseSprite table1Sprite = new BaseSprite(
            table,
            new Rectangle(0, 0, 120, 125),
            new Point(6, 6));

            ItemSprite tableItemSprite = new ItemSprite(
            table1,
            table1Sprite);
            level2.LevelItem.Add(tableItemSprite);


            LevelItems cellDoor2 = new LevelItems("Second Cell Door", -1, 26,false);
            

            BaseSprite cellDoor2Sprite = new BaseSprite(
            cellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(7, 1));

            ItemSprite cellDoorTwoItemSprite = new ItemSprite(
            cellDoor2,
            cellDoor2Sprite);
            level2.LevelItem.Add(cellDoorTwoItemSprite);

            //open cell door that links to a second room 

          
            Door openCell2 = new Door("Open cell", true, 2,1,5,4);

            BaseSprite openCell2DoorSprite = new BaseSprite(
            openCellDoor,
            new Rectangle(0, 0, 63, 80),
            new Point(5, 1));

            ItemSprite openCellDoor2ItemSprite = new ItemSprite(
            openCell2,
            openCell2DoorSprite);
            level2.Doors.Add(openCellDoor2ItemSprite);

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
            LevelItems char1 = new LevelItems("Wizard",19,20, true);
            char1.addKey("Wizards Hat");

            Key goldBar = new Key("Gold Bar");
            char1.addItem(goldBar);
                     
            BaseSprite characterSprite2 = new BaseSprite(
            character2,
            new Rectangle(0, 0, 68, 62),
            new Point(12, 3));
            ItemSprite charItemSprite2 = new ItemSprite(
            char1,
            characterSprite2);
            level2.LevelItem.Add(charItemSprite2);


            LevelItems guard1 = new LevelItems("Guard 1", 15, 16, true);
            guard1.addKey("Gold Bar");

            Key orangeKey = new Key("Orange Key");
            guard1.addItem(orangeKey);

            BaseSprite guardSprite1 = new BaseSprite(
            guard,
            new Rectangle(35, 0, 31, 32),
            new Point(13, 6));
            ItemSprite guardCharSprite1 = new ItemSprite(
            guard1,
            guardSprite1);
            level2.LevelItem.Add(guardCharSprite1);

            Door exit2Door = new Door("DoorTo4", true, 3, 2, 2, 3);
            exit2Door.addKey("Purple Key");

            BaseSprite doorSprite = new BaseSprite(
            exitLoc,
            new Rectangle(0, 0, 60, 42),
            new Point(14, 8));

            ItemSprite doorItem2Sprite = new ItemSprite(
            exit2Door,
            doorSprite);
            level2.Doors.Add(doorItem2Sprite);

            LevelItems guard2 = new LevelItems("Guard 2", 17,18, true);
            guard2.addKey("Breastplate");

            BaseSprite guardSprite2 = new BaseSprite(
            guard,
            new Rectangle(32, 32, 31, 32),
            new Point(13, 10));
            ItemSprite guardCharSprite2 = new ItemSprite(
            guard2,
            guardSprite2);
            level2.LevelItem.Add(guardCharSprite2);

            world.Levels.Add(level2);

        }
        #endregion

        #region Level3 Code

        protected void level3()
        {


            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset1 = new Tileset(tilesetTexture, 7, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\walls2");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 7, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);
            int mapWidth = 9;
            int mapHeight = 9;
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


            List<MapLayer> mapLayers3 = new List<MapLayer>();

            mapLayers3.Add(layer);

            mapLayers3.Add(splatter);
            mapLayers3.Add(wall);
            collision.ProcessWallLayer(wall);

            TileMap map = new TileMap(tilesets, mapLayers3, collision);
            Level level3 = new Level(map);

            //THIS IS HOW TO MAKE A TILE UNWALKABLE
            // map.BlockTile(1, 3);



            LevelItems chest4 = new LevelItems("Chest 1 Level 3", 24, 25, true);
            chest4.addKey("Orange Key");

            Key purpleKey = new Key("Purple Key");
            chest4.addItem(purpleKey);

            Key luckyCoin = new Key("Lucky Coin");
            chest4.addItem(luckyCoin);

            BaseSprite chestSprite4 = new BaseSprite(
            containers,
            new Rectangle(0, 0, 38, 38),
            new Point(7, 3));

            ItemSprite itemSprite = new ItemSprite(
            chest4,
            chestSprite4);
            level3.LevelItem.Add(itemSprite);

            //LevelItems bed1 = new LevelItems("Bed");
            //bed1.addItem("Candle");

            LevelItems charact2 = new LevelItems("Cimba", 21, 22, true);
            charact2.addKey("Lucky Coin");

            Key blueKey = new Key("Blue Key");
            charact2.addItem(blueKey);

            BaseSprite charSprite2 = new BaseSprite(
            char2,
            new Rectangle(0, 48, 38, 45),
            new Point(7, 5));

            ItemSprite charact2Sprite = new ItemSprite(
            charact2,
            charSprite2);
            level3.LevelItem.Add(charact2Sprite);


            LevelItems stool2 = new LevelItems("Stool", -1, 26, false);

            BaseSprite benchSprite = new BaseSprite(
            stool,
            new Rectangle(0, 0, 22, 25),
            new Point(1, 7));

            ItemSprite benchItemSprite2 = new ItemSprite(
            stool2,
            benchSprite);
            level3.LevelItem.Add(benchItemSprite2);

            LevelItems pot2 = new LevelItems("Pot", 27, 26, true);
            Key shield1 = new Key("Shield");
            pot2.addItem(shield1);

            BaseSprite potSprite2 = new BaseSprite(
            pot,
            new Rectangle(0, 0, 28, 32),
            new Point(7, 7));

            ItemSprite potItemSprite3 = new ItemSprite(
            pot2,
            potSprite2);
            level3.LevelItem.Add(potItemSprite3);

            Door exitDoor = new Door("Door", true, 1, 3, 5, 3);

            BaseSprite doorSprite = new BaseSprite(
            openCellDoor,
            new Rectangle(0, 0, 60, 84),
            new Point(5, 1));

            ItemSprite doorItemSprite = new ItemSprite(
            exitDoor,
            doorSprite);
            level3.Doors.Add(doorItemSprite);

            LevelItems crate1 = new LevelItems("crate", 23, 26, true);

            Key helmet = new Key("Helmet");
            crate1.addItem(helmet);

            BaseSprite crateSprite1 = new BaseSprite(
            crate,
            new Rectangle(0, 0, 104, 113),
            new Point(1, 3));

            ItemSprite crateItemSprite1 = new ItemSprite(
            crate1,
            crateSprite1);
            level3.LevelItem.Add(crateItemSprite1);
            
            world.Levels.Add(level3);


           
        } // level 3 end

        #endregion

        #region Level4Code

        protected void level4()
        {          

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\planktileset");
            Tileset tileset1 = new Tileset(tilesetTexture, 7, 1, 40, 40);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Tilesets\walls2");
            Tileset tileset2 = new Tileset(tilesetTexture, 6, 7, 40, 40);

            List<Tileset> tilesets = new List<Tileset>();
            tilesets.Add(tileset1);
            tilesets.Add(tileset2);
            int mapWidth = 13;
            int mapHeight = 15;
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

            //wall.SetTile(wall.Width - 2, wall.Height - 1, new Tile(40, 1));
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
            Level level4 = new Level(map);

            LevelItems plaque1 = new LevelItems("Plaque", 38, 37, true);
            //plaque1.addKey("Purple Key")

            Key brownKey = new Key("Brown Key");
            plaque1.addItem(brownKey);

            BaseSprite plaqueSprite1 = new BaseSprite(
            plaque,
            new Rectangle(0, 0, 50, 35),
            new Point(1, 2));

            ItemSprite plaque1Sprite = new ItemSprite(
            plaque1,
            plaqueSprite1);
            level4.LevelItem.Add(plaque1Sprite);
            
            Door exit4Door = new Door("Final Door", true, 1, 2, 5, 5);
            exit4Door.addKey("Scepter");

            BaseSprite doorSprite = new BaseSprite(
            exitLoc,
            new Rectangle(0, 0, 60, 84),
            new Point(12, 11));

            ItemSprite doorItem4Sprite = new ItemSprite(
            exit4Door,
            doorSprite);
            level4.Doors.Add(doorItem4Sprite);
            

            LevelItems chest5 = new LevelItems("Level 4 Chest 1", 32,31,true);
            chest5.addKey("Brown Key");

            Key goldKey = new Key("Gold Key");
            chest5.addItem(goldKey);

            Key potionOfAwesomeness = new Key("Awesomeness");
            chest5.addItem(potionOfAwesomeness);

            Key mysteriousBox = new Key("Mystery Box");
            chest5.addItem(mysteriousBox);

            BaseSprite chest5Sprite = new BaseSprite(
            containers,
            new Rectangle(0, 0, 60, 84),
            new Point(2, 13));

            ItemSprite chest5ItemSprite = new ItemSprite(
            chest5,
            chest5Sprite);
            level4.LevelItem.Add(chest5ItemSprite);

            LevelItems charact3 = new LevelItems("Lacie", 34, 35, true);
            charact3.addKey("Gold Key");

            Key blueKey = new Key("Brown Key");
            charact3.addItem(blueKey);

            BaseSprite charSprite3 = new BaseSprite(
            char2,
            new Rectangle(323, 190, 36, 45),
            new Point(8, 6));

            ItemSprite charact3Sprite = new ItemSprite(
            charact3,
            charSprite3);
            level4.LevelItem.Add(charact3Sprite);

            LevelItems wizard2 = new LevelItems("Wizard", -1, 33, false);
            //wizard2.addKey("Rainbow Key");

            BaseSprite wizardSprite2 = new BaseSprite(
            character2,
            new Rectangle(0, 0, 68, 62),
            new Point(10, 10));
            ItemSprite wizItemSprite2 = new ItemSprite(
            wizard2,
            wizardSprite2);
            level4.LevelItem.Add(wizItemSprite2);


            //Creating the invisable map using the map.BlockTile method, as seen before
            map.BlockTile(3, 3);
            map.BlockTile(3, 4);
            map.BlockTile(3, 5);
            map.BlockTile(3, 6);
            map.BlockTile(2, 9);
            map.BlockTile(3, 9);
            map.BlockTile(3, 12);
            map.BlockTile(3, 13); 
            map.BlockTile(6, 5);
            map.BlockTile(6, 6);
            map.BlockTile(6, 7);
            map.BlockTile(6, 8);
            map.BlockTile(6, 9);
            map.BlockTile(6, 10);
            map.BlockTile(6, 11);
            map.BlockTile(6, 12);
            map.BlockTile(6, 13);
            map.BlockTile(7, 5);
            map.BlockTile(8, 5);
            map.BlockTile(9, 5);
            map.BlockTile(9, 6);
            map.BlockTile(9, 7);
            //map.BlockTile(9, 8);
           // map.BlockTile(9, 10);
            //map.BlockTile(10, 10); 
 
            world.Levels.Add(level4);

        } // level 4 code

        #endregion




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

           
            Key key2 = new Key("Red Key");
            party.addKey(key2);
            Key key3 = new Key("Purple Key");
            party.addKey(key3);
            Key key4 = new Key("Jail Cell Key");
            party.addKey(key4);
            Key key5 = new Key("Blue Key");
            party.addKey(key5);
            Key key6 = new Key("Green Key");
            party.addKey(key6);
            Key key7 = new Key("Orange Key");
            party.addKey(key7);
            Key key8 = new Key("Yellow Key");
            party.addKey(key8);
        }


        protected void HandleInteraction()
        {
            if (InputHandler.KeyPressed(Keys.I))
            {
                InventoryScreen InventoryS = new InventoryScreen(GameRef,base.StateManager,party);
                StateManager.PushState(InventoryS);
            }

            if (InputHandler.KeyPressed(Keys.Enter))
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

                    Game.Window.Title = "Door";
                    Door door = (Door)world.Levels[world.CurrentLevel].Doors[DoorID].InItem;
                    int level = door.interact(party.keyInv);
                    //change player's position
                    if (level >= 0)
                    {
                        if (!door.IsLocked && door.Name=="Your Door")
                        {
                            BaseSprite doorSprite = new BaseSprite(
                                openCellDoor,
                                new Rectangle(0, 0, 60, 84),
                                new Point(5, 1));
                            world.Levels[world.CurrentLevel].Doors[DoorID].Sprite = doorSprite;
                        }
                        if (!door.IsLocked && door.Name == "Final Door")
                        {
                            StateManager.PushState(GameRef.Outroscreen);
                        }
                        
                    }
                    
                    
                }

            }

        }

        #endregion
    }
}