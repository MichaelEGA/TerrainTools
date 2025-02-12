using UnityEngine;
using UnityEditor;

public class VegetationScatterer : EditorWindow
{
    private Terrain terrain;
    private int numberOfClumps = 10;
    private int clumpRadius = 10;
    private int detailLayerIndex = 0;
    private int splatmapLayerIndex = 0;
    private string[] detailPrototypeNames;

    [UnityEditor.MenuItem("TerrainTools/Vegetation Scatterer")]
    public static void ShowWindow()
    {
        GetWindow<VegetationScatterer>("Vegetation Scatterer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scatter Vegetation on Terrain", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;


            detailPrototypeNames = new string[terrainData.detailPrototypes.Length];

            for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
            {
                detailPrototypeNames[i] = terrainData.detailPrototypes[i].prototype.name;
            }

            detailLayerIndex = EditorGUILayout.Popup("Vegetation Prototype", detailLayerIndex, detailPrototypeNames);
        }

        numberOfClumps = EditorGUILayout.IntField("Number of Clumps", numberOfClumps);
        clumpRadius = EditorGUILayout.IntField("Clump Radius", clumpRadius);
        splatmapLayerIndex = EditorGUILayout.IntField("Splatmap Layer Index", splatmapLayerIndex);

        if (GUILayout.Button("Scatter Vegetation"))
        {
            ScatterVegetation();
        }
    }

    private void ScatterVegetation()
    {
        // Get the terrain data
        TerrainData terrainData = terrain.terrainData;

        terrainData.SetDetailScatterMode(DetailScatterMode.InstanceCountMode);

        if (detailLayerIndex != -1)
        {
            // Get the size of the detail layer
            int detailWidth = terrainData.detailWidth;
            int detailHeight = terrainData.detailHeight;
            int detailResolution = terrainData.detailResolution;
            int alphamapResolution = terrainData.alphamapResolution;

            float[,,] splatmaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

            // Iterate to place specified number of clumps
            for (int i = 0; i < numberOfClumps; i++)
            {
                // Randomly choose a position for the clump
                int clumpCenterX = Random.Range(0, detailWidth);
                int clumpCenterY = Random.Range(0, detailHeight);

                // Add details in a circular area around the clump center
                for (int y = -clumpRadius; y <= clumpRadius; y++)
                {
                    for (int x = -clumpRadius; x <= clumpRadius; x++)
                    {
                        int posX = clumpCenterX + x;
                        int posY = clumpCenterY + y;

                        // Check if the position is within the terrain bounds and within the clump radius
                        if (posX >= 0 && posX < detailWidth && posY >= 0 && posY < detailHeight && x * x + y * y <= clumpRadius * clumpRadius)
                        {
                            // Get current details at the position
                            int[,] detailLayer = terrainData.GetDetailLayer(posX, posY, 1, 1, detailLayerIndex);

                            int alphaX = Mathf.FloorToInt((float)posX / detailResolution * alphamapResolution);
                            int alphaY = Mathf.FloorToInt((float)posY / detailResolution * alphamapResolution);

                            float splatValue = splatmaps[alphaY, alphaX, splatmapLayerIndex];

                            if (splatValue > 0.5f) // Adjust threshold as needed
                            {
                                // Set the detail density
                                detailLayer[0, 0] = 1;

                                // Apply the modified detail layer back to the terrain
                                terrainData.SetDetailLayer(posX, posY, detailLayerIndex, detailLayer);
                            }
                        }
                    }
                }
            }

            terrain.Flush();

            terrainData.RefreshPrototypes();
        }
    }
}
