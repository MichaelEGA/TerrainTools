using UnityEngine;
using UnityEditor;

public class TreeScatterer : EditorWindow
{
    private Terrain terrain;
    private int numberOfClumps = 10;
    private int treesPerClump = 20;
    private float clumpRadius = 10f;
    private int selectedPrototypeIndex = 0;
    private int selectedSplatmapLayer = 0;
    private string[] treePrototypeNames;
    private string[] splatmapLayerNames;

    [UnityEditor.MenuItem("TerrainTools/Tree Scatterer")]
    public static void ShowWindow()
    {
        GetWindow<TreeScatterer>("Tree Scatterer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scatter Trees on Terrain", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;
            treePrototypeNames = new string[terrainData.treePrototypes.Length];
            splatmapLayerNames = new string[terrainData.alphamapLayers];

            for (int i = 0; i < terrainData.treePrototypes.Length; i++)
            {
                treePrototypeNames[i] = terrainData.treePrototypes[i].prefab.name;
            }

            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                splatmapLayerNames[i] = $"Layer {i}";
            }

            selectedPrototypeIndex = EditorGUILayout.Popup("Tree Prototype", selectedPrototypeIndex, treePrototypeNames);
            selectedSplatmapLayer = EditorGUILayout.Popup("Splatmap Layer", selectedSplatmapLayer, splatmapLayerNames);
        }

        numberOfClumps = EditorGUILayout.IntField("Number of Clumps", numberOfClumps);
        treesPerClump = EditorGUILayout.IntField("Trees per Clump", treesPerClump);
        clumpRadius = EditorGUILayout.FloatField("Clump Radius", clumpRadius);

        if (GUILayout.Button("Scatter Trees"))
        {
            ScatterTrees();
        }
    }

    private void ScatterTrees()
    {
        if (terrain == null)
        {
            Debug.LogError("Please assign a Terrain object.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;

        int currentTreeInstances = terrainData.treeInstanceCount;

        if (currentTreeInstances <= 0)
        {
            currentTreeInstances = 1;
        }

        //This gets the old array of tree instances
        TreeInstance[] existingTrees = terrainData.treeInstances;

        // Create a new array with an additional slot for the new tree
        TreeInstance[] newTrees = new TreeInstance[currentTreeInstances + (numberOfClumps * treesPerClump)];

        //This copies the old trees to the new trees
        for (int i = 0; i < existingTrees.Length; i++)
        {
            newTrees[i] = existingTrees[i];
        }

        //This adds the new trees
        int treeIndex = currentTreeInstances - 1;

        for (int i = 0; i < numberOfClumps; i++)
        {
            // Find a random position on the selected splatmap layer
            Vector3 clumpCenter = FindRandomPositionOnLayer(terrainData, selectedSplatmapLayer);

            for (int j = 0; j < treesPerClump; j++)
            {
                // Random position within the clump radius
                float angle = Random.Range(0f, Mathf.PI * 2);
                float radius = Random.Range(0f, clumpRadius / terrainData.size.x); // Normalize radius to terrain size
                float x = clumpCenter.x + Mathf.Cos(angle) * radius;
                float z = clumpCenter.z + Mathf.Sin(angle) * radius;

                // Ensure the position is within terrain bounds
                x = Mathf.Clamp01(x);
                z = Mathf.Clamp01(z);

                // Create a TreeInstance
                TreeInstance treeInstance = new TreeInstance
                {
                    position = new Vector3(x, 0, z),
                    widthScale = 1,
                    heightScale = 1,
                    color = Color.white,
                    lightmapColor = Color.white,
                    prototypeIndex = selectedPrototypeIndex
                };

                // Adjust position to terrain height
                treeInstance.position = new Vector3(x, terrainData.GetHeight((int)(x * terrainData.heightmapResolution), (int)(z * terrainData.heightmapResolution)) / terrainData.size.y, z);

                newTrees[treeIndex++] = treeInstance;
            }

            // Set tree instances on the terrain
            terrainData.SetTreeInstances(newTrees, true);

            terrainData.RefreshPrototypes();

            terrain.Flush();

        }

        Debug.Log($"{numberOfClumps * treesPerClump} trees scattered on the terrain in {numberOfClumps} clumps on splatmap layer {selectedSplatmapLayer}.");
    }

    private Vector3 FindRandomPositionOnLayer(TerrainData terrainData, int layer)
    {
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;
        float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

        while (true)
        {
            int x = Random.Range(0, alphamapWidth);
            int z = Random.Range(0, alphamapHeight);

            if (alphamaps[x, z, layer] > 0.5f) // Adjust the threshold as needed
            {
                return new Vector3((float)x / alphamapWidth, 0, (float)z / alphamapHeight);
            }
        }
    }
}