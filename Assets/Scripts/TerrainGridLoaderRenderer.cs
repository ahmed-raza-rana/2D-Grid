using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Serialization;

// Main terrain grid loader and renderer component
public class TerrainGridLoaderRenderer : MonoBehaviour
{
    public string jsonFileName; // Assign the path to your JSON file in the Unity Editor
    public GameObject[] tilePrefabs; // Array of prefabs representing different tile types

    private ITerrainGridLoader _terrainGridLoader;
    private ITerrainGridRenderer _terrainGridRenderer;
    public TerrainGrid terrainGrid;

    private void Start()
    {
        _terrainGridLoader = new JsonTerrainGridLoader();
        _terrainGridRenderer = new TerrainGridRenderer();

        // Load the JSON file from the Resources folder
        var jsonFile = Resources.Load<TextAsset>(jsonFileName);
        Debug.Log(jsonFile);
        if (jsonFile != null)
        {
            terrainGrid = _terrainGridLoader.LoadTerrainGrid(jsonFile.text);
            Debug.Log(terrainGrid.grid);
            Debug.Log(terrainGrid.grid.Length);
            Debug.Log(terrainGrid.grid[0].row);
            _terrainGridRenderer.RenderTerrainGrid(terrainGrid, tilePrefabs, transform);
        }
        else
        {
            Debug.LogError("JSON file not found in Resources folder: " + jsonFileName);
        }
    }
}

// Data classes representing the terrain grid
[System.Serializable]
public class TerrainGrid
{
    public GridRow[] grid;
}

[System.Serializable]
public class GridRow
{
    public GridTile[] row;
}

[System.Serializable]
public class GridTile
{
    public int tileType;
}

