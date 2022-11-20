using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Tile
{
    [SerializeField] internal string name;
    [SerializeField] internal GameObject asset;

    public Tile(string name)
    {
        this.name = name;
    }
}
public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] [Min(3)] private int row;
    [SerializeField] [Min(3)] private int col;

    [SerializeField] private List<Tile> Tiles;

    GameObject parent;

    public void SetUpTiles()
    {
        Tiles.Clear();
        Tiles.Add(new Tile("Standard"));
        Tiles.Add(new Tile("CornerTopLeft"));
        Tiles.Add(new Tile("CornerTopRight"));
        Tiles.Add(new Tile("CornerBottomLeft"));
        Tiles.Add(new Tile("CornerBottomRight"));
        Tiles.Add(new Tile("WallLeft"));
        Tiles.Add(new Tile("WallRight"));
        Tiles.Add(new Tile("WallTop"));
        Tiles.Add(new Tile("WallBottom"));
    }

    public bool CheckForAssets()
    {
        return Tiles.All(t => t.asset);
    }
    
    public void GenerateTiles()
    {
        parent = new GameObject("Room");
        
        for (var i = 1; i <= row; i++)
        {
            for (var j = 1; j <= col; j++)
            {
                if (i == 1)
                {
                    if (j == 1)
                        SpawnTile(i, j, "CornerBottomLeft");
                    else if (j < col)
                        SpawnTile(i, j, "WallBottom");
                    else if (j == col)
                        SpawnTile(i, j, "CornerBottomRight");
                }
                else if (i == row)
                {
                    if (j == 1)
                        SpawnTile(i, j, "CornerTopLeft");
                    else if (j < col)
                        SpawnTile(i, j, "WallTop");
                    else if (j == col)
                        SpawnTile(i, j, "CornerTopRight");
                }
                else if (i > 1 && i < row)
                {
                    if (j == 1)
                        SpawnTile(i, j, "WallLeft");
                    else if (j < col)
                        SpawnTile(i, j, "Standard");
                    else if (j == col)
                        SpawnTile(i, j, "WallRight");
                }
            }
        }
    }

    private void SpawnTile(int z, int x, string name)
    {
        GameObject obj = Tiles.Find(x => x.name.Contains(name)).asset;
        Instantiate(obj, new Vector3(x*2,0,z*2), Quaternion.identity, parent.transform);
    }
    
}
