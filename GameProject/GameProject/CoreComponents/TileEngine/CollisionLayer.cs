using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreComponents.TileEngine
{

    public enum CollisionType { Walkable, Unwalkable };

    public class CollisionLayer
    {
        #region Field Region

        CollisionType[,] layer;

        #endregion

        #region Property Region

        public int Width
        {
            get { return layer.GetLength(1); }
        }

        public int Height
        {
            get { return layer.GetLength(0); }
        }

        #endregion

        #region Constructor Region

        public CollisionLayer(CollisionType[,] map)
        {
            this.layer = (CollisionType[,])map.Clone();
        }

        public CollisionLayer(int width, int height )
        {
            layer = new CollisionType[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SetTile(x, y, CollisionType.Walkable);
                }
            }
        }

        public bool Blocked(int row, int column)
        {
            if (layer[row, column] == CollisionType.Unwalkable)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Method Region

        public CollisionType GetTile(int x, int y)
        {
            return layer[y, x];
        }
        public void SetTile(int x, int y, CollisionType value)
        {
            layer[y, x] = value;
        }

        public void ProcessWallLayer(MapLayer layer)
        {
            Tile tile;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    tile=layer.GetTile(x,y);
                    if ((tile.TileIndex) != -1 && (tile.TileIndex) != 31 && (tile.TileIndex) != 34)
                    {
                            SetTile(x, y, CollisionType.Unwalkable);
                    }
                }
        }

        #endregion
    }
}
