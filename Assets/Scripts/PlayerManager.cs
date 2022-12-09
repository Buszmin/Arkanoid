using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    int hp = 3;
    public int totalScore = 0;
    [SerializeField] List<GameObject> hearts;
    [SerializeField] TextMeshProUGUI scoreTmPro;
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject newHighScore;
    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI highScoreTmPro;
    [SerializeField] GameObject ball;
    public List<GameObject> ballsInGame = new List<GameObject>();
    public int ballsCount = 1;
    public int highScore;

    void Awake()
    {
        Instance = this;
    }

        public void AddScore(int score)
    {
        totalScore += score;
        scoreTmPro.text = "Score: " + totalScore;
    }

    public void LoseHp()
    {
        hp--;

        if (hp < 0)
        {
            Die();
        }
        else
        {
            hearts[hp].SetActive(false);
            Debug.Log("hp: " + hp);
        }
    }

    private void Die()
    {
        Time.timeScale = 0;
        deathPanel.SetActive(true);
        highScore = GetHighScore();
        highScoreTmPro.text = "";

        Destroy(scoreTmPro);

        finalScore.text = "Score: " + totalScore;

        if (highScore != 0)
        {
            highScoreTmPro.text = "High Score: " + highScore;
        }

        if (highScore < totalScore)
        {
            newHighScore.SetActive(true);
            highScore = totalScore;
            SaveHighScore(highScore);
        }
        else
        {
            newHighScore.SetActive(false);
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveHighScore(int score)
    {
        SaveLoadSystem.instance.saveHighScore(score);
    }

    private int GetHighScore()
    {
        return SaveLoadSystem.instance.LoadHighScore();
    }

    public void PowerUpSkipLvl()
    {
        LevelGenerator.Instance.NextLevel();
    }

    public void PowerUpMoreBalls()
    {
        ballsCount++;
        GameObject b = Instantiate(ball);
        b.transform.position = transform.position + new Vector3(0, 2 , 0);
    }

    public void RemoveAndRestartBalls()
    {
        foreach (GameObject ball in ballsInGame)
        {
            Destroy(ball);
        }

        ballsInGame.Clear();


        GameObject b = Instantiate(ball);
        b.transform.position = transform.position + new Vector3(0, 2, 0);

        ballsInGame.Add(b);
        ballsCount = 1;
    }


    public void LoadFromData(SaveLoadSystem.Data data)
    {
        totalScore = data.score;

        if(data.highScore > highScore)
        {
            highScore = data.highScore;
        }

        ballsCount = 0;

        foreach (GameObject ball in ballsInGame)
        {
            Destroy(ball);
        }

        ballsInGame.Clear();

        foreach (Vector2 direction in data.ballsInGameDirection)
        {
            GameObject b = Instantiate(ball);
            b.transform.position = data.ballsInGamePos[ballsCount];
            b.GetComponent<Ball>().direction = direction;

            ballsCount++;
        }

        transform.position = data.playerPos;

    }
}

