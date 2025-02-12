using UnityEngine;
using UnityEditor;

public class VegetationClearer : EditorWindow
{
    private Terrain terrain;

    [UnityEditor.MenuItem("TerrainTools/Vegetation Clearer")]
    public static void ShowWindow()
    {
        GetWindow<VegetationClearer>("Vegetation Clearer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Clear Vegetation from Terrain", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (terrain == null)
        {
            EditorGUILayout.HelpBox("Please assign a terrain.", MessageType.Warning);
        }
        else if (GUILayout.Button("Clear Vegetation Objects"))
        {
            ClearTerrainDetailObjects();
        }
    }

    private void ClearTerrainDetailObjects()
    {
        Undo.RegisterCompleteObjectUndo(terrain.terrainData, "Clear Vegetation Objects");

        // Clear all details
        int detailLayerCount = terrain.terrainData.detailPrototypes.Length;
        for (int i = 0; i < detailLayerCount; i++)
        {
            int[,] details = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];
            terrain.terrainData.SetDetailLayer(0, 0, i, details);
        }

        EditorUtility.SetDirty(terrain.terrainData);
    }
}