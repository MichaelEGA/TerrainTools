using UnityEngine;
using UnityEditor;

public class TreeClearer : EditorWindow
{
    private Terrain terrain;

    [UnityEditor.MenuItem("TerrainTools/Tree Clearer")]
    public static void ShowWindow()
    {
        GetWindow<TreeClearer>("Tree Clearer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Clear All Trees from Terrain", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (GUILayout.Button("Clear Trees"))
        {
            ClearTrees();
        }
    }

    private void ClearTrees()
    {
        if (terrain == null)
        {
            Debug.LogError("Please assign a Terrain object.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;

        // Clear all tree instances
        terrainData.treeInstances = new TreeInstance[0];

        Debug.Log("All trees have been cleared from the terrain.");
    }
}
