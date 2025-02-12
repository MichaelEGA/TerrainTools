using UnityEngine;
using UnityEditor;

public class ImageToHeightmapEditor : EditorWindow
{
    private Texture2D heightmapImage;
    private Terrain terrain;
    private float heightMultiplier = 1.0f;

    [UnityEditor.MenuItem("TerrainTools/Image To Heightmap")]
    public static void ShowWindow()
    {
        GetWindow<ImageToHeightmapEditor>("Image To Heightmap");
    }

    private void OnGUI()
    {
        GUILayout.Label("Heightmap Settings", EditorStyles.boldLabel);

        heightmapImage = (Texture2D)EditorGUILayout.ObjectField("Heightmap Image", heightmapImage, typeof(Texture2D), false);
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        heightMultiplier = EditorGUILayout.FloatField("Height Multiplier", heightMultiplier);

        if (GUILayout.Button("Apply Heightmap"))
        {
            if (heightmapImage != null && terrain != null)
            {
                ApplyHeightmap();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a heightmap image and a terrain.", "OK");
            }
        }
    }

    private void ApplyHeightmap()
    {
        int width = heightmapImage.width;
        int height = heightmapImage.height;
        float[,] heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = heightmapImage.GetPixel(x, y);
                float heightValue = pixel.r * heightMultiplier;
                heights[y, x] = heightValue;
            }
        }

        terrain.terrainData.heightmapResolution = width + 1;
        terrain.terrainData.SetHeights(0, 0, heights);
    }
}
