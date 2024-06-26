using Newtonsoft.Json;
using UnityEngine;

namespace Features._2DGrid.Scripts
{
    // Interface for terrain grid loaders
    public interface ITerrainGridLoader
    {
        TerrainData LoadTerrainGrid(string filePath);
    }

// Concrete implementation of terrain grid loader using JSON
    public class JsonTerrainGridLoader : ITerrainGridLoader
    {
        public TerrainData LoadTerrainGrid(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("JSON string is null or empty");
                return null;
            }

            // Attempt to deserialize JSON string
            //TerrainGrid terrainGrid = JsonUtility.FromJson<TerrainGrid>(json);
            TerrainData terrainGrid = JsonConvert.DeserializeObject<TerrainData>(json);

            return terrainGrid;
        }
    }
}