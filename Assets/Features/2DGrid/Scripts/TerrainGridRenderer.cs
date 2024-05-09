using System.Collections.Generic;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    // Interface for terrain grid renderers
    public interface ITerrainGridRenderer
    {
        void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform, float tileSize);
        GridTile[,] GetTilesArray();
    }

// Concrete implementation of terrain grid renderer
    public class TerrainGridRenderer : ITerrainGridRenderer
    {
        private GridTile[,] _tilesArray;
        private int _row = 0;
        private int _col = 0;
        
        public GridTile[,] GetTilesArray()
        {
            return _tilesArray;
        }

        public void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform,
            float tileSize)
        {
            _row = terrainGrid.TerrainGrid.Count;
            _col = terrainGrid.TerrainGrid[0].Count;
            _tilesArray = new GridTile[_row, _col];

            // Loop through the rows of the terrain grid
            for (var i = 0; i < _row; i++)
            {
                var row = terrainGrid.TerrainGrid[i];
                var j = 0;
                // Loop through the tiles in the row
                foreach (var tile in row)
                {
                    var tileType = (TileEnum)tile.TileType; // Cast TileType to TileEnum
                    var tilePrefab = gridConfig.gridObj;
                    var tilePosition = new Vector3(tileSize * j, 0, tileSize * -i);
                    TileType(tileType, gridConfig, tilePrefab);
                    var tileInstance = Object.Instantiate(tilePrefab, tilePosition, Quaternion.Euler(90f, 0f, 0f),
                        parentTransform);
                    tileInstance.name = (i + ":" + j).ToString();
                    var gridTileComponent = tileInstance.GetComponent<GridTile>();
                    gridTileComponent.TileUpdate(tilePosition, tileType);

                    // Save the instantiated tile and its GridTile component in the 2D array
                    _tilesArray[i, j] = gridTileComponent;
                    j++;
                }
            }

            CalculateAdjustTiles();
        }

        private void CalculateAdjustTiles()
        {
            for (var row = 0; row < _row; row++)
            {
                for (var col = 0; col < _col; col++)
                {
                    FindAdjustTiles(new Vector2Int(row, col));
                }
            }
        }

        private void FindAdjustTiles(Vector2Int currentTile)
        {
            var adjustTiles = new List<GridTile>();

            // Define the range of valid neighbor indices
            var minCol = Mathf.Max(0, currentTile.x - 1);
            var maxCol = Mathf.Min(_col - 1, currentTile.x + 1);
            var minRow = Mathf.Max(0, currentTile.y - 1);
            var maxRow = Mathf.Min(_row - 1, currentTile.y + 1);

            // Iterate over neighboring indices (up, down, left, right)
            for (var row = minRow; row <= maxRow; row++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    // Exclude diagonals and the current tile itself
                    if ((row == currentTile.y || col == currentTile.x) && (row != currentTile.y || col != currentTile.x))
                    {
                        adjustTiles.Add(_tilesArray[col, row]);
                    }
                }
            }

            _tilesArray[currentTile.x, currentTile.y].adjustObjs = adjustTiles;
        }

        private static void TileType(TileEnum tileEnum, GridConfig gridConfig, GameObject tile)
        {
            var cellSprite = tileEnum switch
            {
                (TileEnum.Dirt) => gridConfig.dirt,
                (TileEnum.Grass) => gridConfig.grass,
                (TileEnum.Stone) => gridConfig.stone,
                (TileEnum.Wood) => gridConfig.wood,
                _ => null
            };

            tile.GetComponent<SpriteRenderer>().sprite = cellSprite;
        }
    }
}