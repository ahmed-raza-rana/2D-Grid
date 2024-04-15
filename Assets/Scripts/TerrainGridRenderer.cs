using UnityEngine;
using System.Collections;
using System.IO;

// Interface for terrain grid renderers
public interface ITerrainGridRenderer
{
    void RenderTerrainGrid(TerrainGrid terrainGrid, GameObject[] tilePrefabs, Transform parentTransform);
}

// Concrete implementation of terrain grid renderer
public class TerrainGridRenderer : ITerrainGridRenderer
{
    public void RenderTerrainGrid(TerrainGrid terrainGrid, GameObject[] tilePrefabs, Transform parentTransform)
    {
        // Loop through the rows of the terrain grid
        for (var i = 0; i < terrainGrid.grid.Length; i++)
        {
            var row = terrainGrid.grid[i];
            // Loop through the tiles in the row
            for (var j = 0; j < row.row.Length; j++)
            {
                var tileType = row.row[j].tileType;

                // Instantiate the corresponding tile prefab based on the tile type
                var tilePrefab = tilePrefabs[tileType];
                var tilePosition = new Vector3(j, 0, -i); // Assuming the grid is laid out on the XZ plane
                var tileInstance = Object.Instantiate(tilePrefab, tilePosition, Quaternion.identity, parentTransform);
            }
        }
    }
}