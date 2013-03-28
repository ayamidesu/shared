using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreComponents.TileEngine
{
    public class TileMap
    {
        #region Field Region

        List<Tileset> tilesets;
        List<MapLayer> mapLayers;
        CollisionLayer collision;

        static int mapWidth;
        static int mapHeight;

        #endregion

        #region Property Region

        public static int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public CollisionLayer Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        public static int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        #endregion

        #region Constructor Region

        public TileMap(List<Tileset> tilesets, List<MapLayer> layers,CollisionLayer collisionLayer)
        {
            this.tilesets = tilesets;
            this.mapLayers = layers;
            this.collision = collisionLayer;
            mapWidth = mapLayers[0].Width;
            mapHeight = mapLayers[0].Height;

            for (int i = 1; i < layers.Count; i++)
            {
                if (mapWidth != mapLayers[i].Width || mapHeight != mapLayers[i].Height)
                    throw new Exception("Map layer size exception");
            }
        }

        public TileMap(Tileset tileset, MapLayer layer)
        {
            tilesets = new List<Tileset>();
            tilesets.Add(tileset);

            mapLayers = new List<MapLayer>();
            mapLayers.Add(layer);

            mapWidth = mapLayers[0].Width;
            mapHeight = mapLayers[0].Height;
        }

        #endregion

        #region Method Region

        public bool CheckUp(Rectangle nextRectangle)
        {
            Point tile1 = Engine.VectorToCell(
                new Vector2(nextRectangle.X, nextRectangle.Y));
            Point tile2 = Engine.VectorToCell(
                new Vector2(nextRectangle.X + nextRectangle.Width - 1,
                nextRectangle.Y + nextRectangle.Height));
            bool doesCollide = false;
            if (tile1.Y < 0)
                return !doesCollide;
            int y = tile1.Y;
            for (int x = tile1.X; x <= tile2.X; x++)
                if (collision.GetTile(x, y) == CollisionType.Unwalkable)
                    doesCollide = true;
            return doesCollide;
        }

        public bool CheckLeft(Rectangle nextRectangle)
        {
            Point tile1 = Engine.VectorToCell(
                new Vector2(nextRectangle.X, nextRectangle.Y));
            Point tile2 = Engine.VectorToCell(
                new Vector2(nextRectangle.X + nextRectangle.Width,
                   nextRectangle.Y + nextRectangle.Height - 1));
            bool doesCollide = false;
            if (tile1.X < 0)
                return !doesCollide;
            int x = tile1.X;
            for (int y = tile1.Y; y <= tile2.Y; y++)
                {
                if (collision.GetTile(x,y) == CollisionType.Unwalkable)
                    doesCollide = true;
                }
        return doesCollide;
        }

        public bool CheckRight(Rectangle nextRectangle)
        {
        Point tile1 = Engine.VectorToCell(
            new Vector2(nextRectangle.X, nextRectangle.Y));

        Point tile2 = Engine.VectorToCell(
            new Vector2(nextRectangle.X + nextRectangle.Width,
            nextRectangle.Y + nextRectangle.Height - 1));

        bool doesCollide = false;
        if (tile2.X >= mapWidth)
        return !doesCollide;
        int x = tile2.X;
        for (int y = tile1.Y; y <= tile2.Y; y++)
            {
            if (collision.GetTile(x, y) == CollisionType.Unwalkable)
                doesCollide = true;
            }
        return doesCollide;
        }

        public bool CheckDown(Rectangle nextRectangle)
        {
            Point tile1 = Engine.VectorToCell(
            new Vector2(nextRectangle.X, nextRectangle.Y));
            Point tile2 = Engine.VectorToCell(
            new Vector2(nextRectangle.X + nextRectangle.Width - 1,
            nextRectangle.Y + nextRectangle.Height));
            bool doesCollide = false;
            if (tile2.Y >= mapHeight)
                return !doesCollide;
            int y = tile2.Y;
            for (int x = tile1.X; x <= tile2.X; x++)
            {
                if (collision.GetTile(x, y) == CollisionType.Unwalkable)
                    doesCollide = true;
            }
            return doesCollide;
        }



        public void BlockTile(int x, int y)
        {
            collision.SetTile(x, y,CollisionType.Unwalkable);
        }

        public void AddLayer(MapLayer layer)
        {
            if (layer.Width != mapWidth && layer.Height != mapHeight)
                throw new Exception("Map layer size exception");

            mapLayers.Add(layer);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point cameraPoint = Engine.VectorToCell(camera.Position);
            Point viewPoint = Engine.VectorToCell(
            new Vector2(
             (camera.Position.X + camera.ViewportRectangle.Width),
             (camera.Position.Y + camera.ViewportRectangle.Height)));
            
            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            Tile tile;

            Point min = new Point();
            Point max = new Point();
            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, mapHeight);

            foreach (MapLayer layer in mapLayers)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    destination.Y = y * Engine.TileHeight;
                    for (int x = min.X; x < max.X; x++)
                    {
                        tile = layer.GetTile(x, y);

                        if (tile.TileIndex == -1 || tile.Tileset == -1)
                            continue;

                        destination.X = x * Engine.TileWidth;

                        spriteBatch.Draw(
                            tilesets[tile.Tileset].Texture,
                            destination,
                            tilesets[tile.Tileset].SourceRectangles[tile.TileIndex],
                            Color.White);
                    }
                }
            }
        }

        #endregion
    }
}
