using UnityEngine;
using System.Collections.Generic;

// Main terrain grid loader and renderer component
public class TerrainGridLoaderRenderer : MonoBehaviour
{
    public string jsonFileName; // Assign the path to your JSON file in the Unity Editor
    public GameObject[] tilePrefabs; // Array of prefabs representing different tile types

    public float tileSize;
    public Quaternion tileRotation;

    private ITerrainGridLoader _terrainGridLoader;
    private ITerrainGridRenderer _terrainGridRenderer;
    public TerrainData terrainGrid;

    private void Start()
    {
        _terrainGridLoader = new JsonTerrainGridLoader();
        _terrainGridRenderer = new TerrainGridRenderer();

        // Load the JSON file from the Resources folder
        var jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile != null)
        {
            terrainGrid = _terrainGridLoader.LoadTerrainGrid(jsonFile.text);
            _terrainGridRenderer.RenderTerrainGrid(terrainGrid, tilePrefabs, transform, tileSize);
        }
        else
        {
            Debug.LogError("JSON file not found in Resources folder: " + jsonFileName);
        }
    }
}

// Data classes representing the terrain grid
[System.Serializable]
public class TerrainData
{
    public List<List<Tile>> TerrainGrid { get; set; }
}

[System.Serializable]
public class Tile
{
    public int TileType { get; set; }
}

