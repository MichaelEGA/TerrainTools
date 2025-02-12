# TerrainTools
Set of tools that allows you to quickly create a unity terrain. The tools are useful for quickly setting up a terrain which you can then perfect and modify using unity's terrain tools.

**Get it here:** https://github.com/MichaelEGA/TerrainTools

**Tested In:** Unity URP 6000.1.0b3

**Version History**  
12/02/2025 - Initial Commit  

**Tools**
  - **Image to Heightmap**, applies a greyscale image as a heightmap to the selected unity terrain
  - **Terrain Splatmap Painter**, procedurally paint a splatmap layer according to height and angle
  - **Tree Clearer**, deletes all tree instances from the terrain
  - **Tree Scatterer**, scatters the selected tree on the selected splatmap layer in natural clumps
  - **Vegetation Clearer**, deletes all vegetation instances from the terrain
  - **Vegetation Scatterer**, scatters the selected grass/vegetation detail on the selected splatmap layer in natural clumps

![image](https://github.com/user-attachments/assets/83015db3-56ea-4dfb-bef3-3f591c9f9979)

**Directions**

Drop the files into your assets folder. The new tools will appear under 'Terrain Tools' on the menu.

**Image to Heightmap**
1. Create a new unity terrain
2. Import your greyscale heightmap
3. In the Import Settings of your texture tick the box that says "Read/Write"
4. In the Import Settings of your texture change the format at the bottom to "R 16 but"
![image](https://github.com/user-attachments/assets/58e19d47-c08a-43b9-84e0-7d2d757fcd4a)
5. Now run the **Image to Heightmap Tool**, drag in your terrain and drag in your heightmap
6. Press Apply Heightmap

**Terrain Splatmap Painter**
1. Set up your terrain layers on the terrain
2. Run **Terrain Splatmap Painter**
3. Drag in your terrain
4. The rest is self-explanatory

**Tree Scatterer**
1. Set up your tree prototypes on the terrain
2. Run **Tree Scatterer**
3. Drag in your terrain
4. The rest is self explanatory

**Vegetation Scatterer**
1. Set up your detail prototypes on the terrain
2. Run the **Vegetation Scatterer**
3. Drag in your terrain
4. The rest is self explanatory
