using UnityEngine;

namespace Features._2DGrid.Scripts
{
    // Interface for terrain grid renderers
    public interface ITerrainGridRenderer
    {
        void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform, float tileSize);
    }

// Concrete implementation of terrain grid renderer
    public class TerrainGridRenderer : ITerrainGridRenderer
    {
        public void RenderTerrainGrid(TerrainData terrainGrid, GridConfig gridConfig, Transform parentTransform, float tileSize)
        {
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
                    j++;
                }
            }
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