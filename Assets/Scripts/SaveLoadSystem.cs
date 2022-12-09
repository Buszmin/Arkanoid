using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem instance;
    public static bool load;

    [System.Serializable] public struct Data
    {
        public int lvl;
        public int score;
        public int highScore;
        public List<Vector2> ballsInGamePos;
        public List<Vector2> ballsInGameDirection;
        public Vector2 playerPos;
        public List<Tile.TileData> tilesData;
    }


    private void Start()
    {
        instance = this;

        Load();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Load();
        }
        
    }

    public Data data;

    public void Save()
    {
        if(data.ballsInGamePos.Count>0)
        {
            data.ballsInGamePos.Clear();
            data.ballsInGameDirection.Clear();
            data.tilesData.Clear();
        }

        PlayerManager playerManager = PlayerManager.Instance;

        data.lvl = LevelGenerator.Instance.lvl;
        data.score = playerManager.totalScore;
        data.highScore = playerManager.highScore;

        foreach (GameObject ball in playerManager.ballsInGame)
        {
            data.ballsInGamePos.Add(ball.transform.position);
            data.ballsInGameDirection.Add(ball.GetComponent<Ball>().direction);
        }


        for (int i = 0; i < LevelGenerator.Instance.tiles.GetLength(0); i++)
        {
            for (int j = 0; j < LevelGenerator.Instance.tiles.GetLength(1); j++)
            {
                if (LevelGenerator.Instance.tiles[i, j] != null)
                {
                    data.tilesData.Add(LevelGenerator.Instance.tiles[i, j].GetComponent<Tile>().data);
                }
            }
        }

        data.playerPos = playerManager.transform.position;

        string dataString = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", dataString);
        Debug.Log(Application.persistentDataPath.ToString() + "/save.json");
    }

    public void Load()
    {
        if (load)
        {
            if (System.IO.File.Exists(Application.persistentDataPath.ToString() + "/save.json"))
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(Application.persistentDataPath.ToString() + "/save.json");
                string dataString = streamReader.ReadToEnd();
                streamReader.Close();
                data = JsonUtility.FromJson<Data>(dataString);

                LevelGenerator.Instance.LoadFromData(data);
                PlayerManager.Instance.LoadFromData(data);
            }
            load = false;
        }
    }

    public void saveHighScore(int score)
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + "/highScore.json", score.ToString());
        Debug.Log(Application.persistentDataPath.ToString() + "/highScore.json");
    }

    public int LoadHighScore()
    {
        if (System.IO.File.Exists(Application.persistentDataPath.ToString() + "/highScore.json"))
        {
            System.IO.StreamReader streamReader = new System.IO.StreamReader(Application.persistentDataPath.ToString() + "/highScore.json");
            string dataString = streamReader.ReadToEnd();
            streamReader.Close();
            return int.Parse(dataString);
        }
        else
        {
            return 0;
        }    
    }
}
