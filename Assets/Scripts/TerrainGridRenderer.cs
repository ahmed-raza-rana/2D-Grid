using System.Linq;
using UnityEngine;
// Interface for terrain grid renderers
public interface ITerrainGridRenderer
{
    void RenderTerrainGrid(TerrainData terrainGrid, GameObject[] tilePrefabs, Transform parentTransform, float tileSize);
}

// Concrete implementation of terrain grid renderer
public class TerrainGridRenderer : ITerrainGridRenderer
{
    public void RenderTerrainGrid(TerrainData terrainGrid, GameObject[] tilePrefabs, Transform parentTransform, float tileSize)
    {
        // Loop through the rows of the terrain grid
        for (var i = 0; i < terrainGrid.TerrainGrid.Count; i++)
        {
            var row = terrainGrid.TerrainGrid[i];
            var j = 0;
            // Loop through the tiles in the row
            foreach (var tileInstance in from tile in row select tile.TileType into tileType select tilePrefabs[tileType] into tilePrefab let tilePosition = new Vector3(tileSize * j, 0, tileSize * -i) select Object.Instantiate(tilePrefab, tilePosition, Quaternion.Euler(90f, 0f, 0f), parentTransform))
            {
                j++;
            }
        }
    }
}