using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static  LevelGenerator Instance;
    [SerializeField] Vector3 seed;

    [SerializeField] GameObject normalTilePrefab;
    [SerializeField] GameObject hardTilePrefab;
    [SerializeField] GameObject hardTilePrefabDamaged;
    [SerializeField] GameObject metalTilePrefab;

    [SerializeField] Transform tilesParent;

    [SerializeField] Vector2 startPos = new Vector2(-32.5f, 33.5f);
    public GameObject[,] tiles = new GameObject[17,13];
    public int tilesToDestroy=0;
    public int lvl = 0;

    void Awake()
    {
        Instance = this;
        SpawnWall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnWall()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                float noise = Mathf.PerlinNoise(((float)i + 1.0f) * seed.x + ((float)lvl * 0.30f), ((float)j +1.0f) * seed.y + ((float)lvl * 0.30f));
                //Debug.Log(noise);

                if(noise > 0.6f)
                {
                    noise = Mathf.PerlinNoise(((float)i + 1.0f) * seed.z + ((float)lvl * 0.30f), ((float)j + 1.0f) * seed.z + ((float)lvl * 0.30f));

                    if (noise >= 0.6f)
                    {
                        tilesToDestroy ++;
                        tiles[i, j] = Instantiate(normalTilePrefab);
                        tiles[i, j].GetComponent<Tile>().data.type = 0;
                    }
                    else if (noise <= 0.4f)
                    {
                        tilesToDestroy++;
                        tiles[i, j] = Instantiate(hardTilePrefab);
                        tiles[i, j].GetComponent<Tile>().data.type = 1;
                    }
                    else 
                    {
                        tiles[i, j] = Instantiate(metalTilePrefab);
                        tiles[i, j].GetComponent<Tile>().data.type = 3;
                    }

                    tiles[i, j].transform.position = startPos + new Vector2(i * 4, -j * 2);
                    tiles[i, j].transform.parent = tilesParent;
                    tiles[i, j].GetComponent<Tile>().data.pos = new Vector2Int(i, j);
                    

                }
            }
        }
        lvl++;
    }

    public void LoadFromData(SaveLoadSystem.Data data)
    {
        tilesToDestroy = 0;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] != null)
                {
                    Destroy(tiles[i, j]);
                }
            }
        }

        lvl = data.lvl;

        foreach (Tile.TileData tile in data.tilesData)
        {
            switch (tile.type)
            {
                case 0:
                    tilesToDestroy++;
                    tiles[tile.pos.x, tile.pos.y] = Instantiate(normalTilePrefab);
                    tiles[tile.pos.x, tile.pos.y].GetComponent<Tile>().data.type = 0;
                    break;
                case 1:
                    tilesToDestroy++;
                    tiles[tile.pos.x, tile.pos.y] = Instantiate(hardTilePrefab);
                    tiles[tile.pos.x, tile.pos.y].GetComponent<Tile>().data.type = 1;
                    break;
                case 2:
                    tilesToDestroy++;
                    tiles[tile.pos.x, tile.pos.y] = Instantiate(hardTilePrefabDamaged);
                    tiles[tile.pos.x, tile.pos.y].GetComponent<Tile>().data.type = 2;
                    break;
                case 3:
                    tiles[tile.pos.x, tile.pos.y] = Instantiate(metalTilePrefab);
                    tiles[tile.pos.x, tile.pos.y].GetComponent<Tile>().data.type = 3;
                    break;
                default:
                    Debug.LogError("Corrupted Data");
                    break;
            }

            tiles[tile.pos.x, tile.pos.y].transform.position = startPos + new Vector2(tile.pos.x * 4, -tile.pos.y * 2);
            tiles[tile.pos.x, tile.pos.y].transform.parent =  tilesParent;
            tiles[tile.pos.x, tile.pos.y].GetComponent<Tile>().data.pos = new Vector2Int(tile.pos.x, tile.pos.y);
        }
    }



    public void tryNextLevel()
    {
        //Debug.Log("tilesToDestroy: " + tilesToDestroy);
        if(tilesToDestroy <= 0)
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        tilesToDestroy = 0;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] != null)
                {
                    Destroy(tiles[i, j]);
                }
            }
        }
        PlayerManager.Instance.RemoveAndRestartBalls();
        SpawnWall();
    }
}
