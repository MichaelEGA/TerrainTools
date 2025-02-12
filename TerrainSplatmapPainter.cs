using UnityEditor;
using UnityEngine;

public class TerrainSplatmapPainter : EditorWindow
{
    private Terrain terrain;
    private float heightThreshold = 10f;
    private float angleThreshold = 30f;
    private string[] splatmapLayerNames;
    private int splatMapIndex = 0;

    [UnityEditor.MenuItem("TerrainTools/Terrain Splatmap Painter")]
    public static void ShowWindow()
    {
        GetWindow<TerrainSplatmapPainter>("Splatmap Painter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Terrain Splatmap Painter", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);

        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;

            splatmapLayerNames = new string[terrainData.alphamapLayers];

            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                splatmapLayerNames[i] = $"Layer {i}";
            }


            splatMapIndex = EditorGUILayout.Popup("Splatmap Layer", splatMapIndex, splatmapLayerNames);
        }

        heightThreshold = EditorGUILayout.FloatField("Height Threshold", heightThreshold);
        angleThreshold = EditorGUILayout.FloatField("Angle Threshold", angleThreshold);

        if (GUILayout.Button("Paint Splatmap"))
        {
            PaintSplatmap();
        }
    }

    private void PaintSplatmap()
    {
        if (terrain == null)
        {
            Debug.LogError("Please assign a terrain.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float normalizedX = (float)x / (width - 1);
                float normalizedY = (float)y / (height - 1);
                float terrainHeight = terrainData.GetHeight((int)(normalizedX * terrainData.heightmapResolution), (int)(normalizedY * terrainData.heightmapResolution));
                float slope = terrainData.GetSteepness(normalizedX, normalizedY);

                if (terrainHeight >= heightThreshold && slope >= angleThreshold)
                {
                    for (int i = 0; i < terrainData.alphamapLayers; i++)
                    {
                        if (i == splatMapIndex)
                        {
                            splatmapData[y, x, i] = 1;
                        }
                        else
                        {
                            splatmapData[y, x, i] = 0;
                        }
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
        Debug.Log("Splatmap painting completed.");
    }
}
