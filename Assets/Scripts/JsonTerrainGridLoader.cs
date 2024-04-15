using UnityEngine;
using System.Collections;
using System.IO;

// Interface for terrain grid loaders
public interface ITerrainGridLoader
{
    TerrainGrid LoadTerrainGrid(string filePath);
}

// Concrete implementation of terrain grid loader using JSON
public class JsonTerrainGridLoader : ITerrainGridLoader
{
    public TerrainGrid LoadTerrainGrid(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("JSON string is null or empty");
            return null;
        }

        Debug.Log("JSON string: " + json);

        // Attempt to deserialize JSON string
        TerrainGrid terrainGrid = JsonUtility.FromJson<TerrainGrid>(json);
        if (terrainGrid == null)
        {
            Debug.LogError("Failed to deserialize JSON string");
        }

        return terrainGrid;
    }
}