using System.Collections.Generic;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    // Interface for terrain grid renderers
    public interface ITerrainGridRenderer
    {
        void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform, float tileSize);
    }
    public interface IAdjustTileCalculator
    {
        void CalculateAdjustTiles(Tile[,] gridArray, List<Tile>[,] adjustGrid);
    }

// Concrete implementation of terrain grid renderer
    public class TerrainGridRenderer : ITerrainGridRenderer, IAdjustTileCalculator
    { 
        private GameObject[,] tilesArray; // Declare the 2D array to store tiles
        private int _row;
        private int _col;
        
        public void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform, float tileSize)
        {
            tilesArray = new GameObject[terrainGrid.TerrainGrid.Count, terrainGrid.TerrainGrid[0].Count]; // Initialize the 2D array
            // Loop through the rows of the terrain grid
            for (var i = 0; i < terrainGrid.TerrainGrid.Count; i++)
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
                    var tileInstance = Object.Instantiate(tilePrefab, tilePosition, Quaternion.Euler(90f, 0f, 0f), parentTransform);
                    tileInstance.GetComponent<GridTile>().TileUpdate(tilePosition, tileType);
                    
                    // Save the instantiated tile in the 2D array
                    tilesArray[i, j] = tileInstance;
                    j++;
                }
            }
        }
        
        public void CalculateAdjustTiles(Tile[,] gridArray, List<Tile>[,] neighbourGrid)
        {
            for (var row = 0; row < _row; row++)
            {
                for (var col = 0; col < _col; col++)
                {
                    FindAdjustTiles();
                }
            }
        }
        
        private void FindAdjustTiles()
        {
            
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