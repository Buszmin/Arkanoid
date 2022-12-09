using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField][Range(1, 10)] protected int score=1;
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] GameObject particles;

    [System.Serializable] public struct TileData
    {
        public Vector2Int pos;
        public int type;
    }

    public TileData data;

    void Update()
    {
        
    }

    public virtual void Hit()
    {
        Die();
    }

    protected virtual void Die()
    {
        PlayerManager.Instance.AddScore(score);
        particles.SetActive(true);
        particles.transform.parent = null;
        particles.transform.localScale = new Vector3(1, 1, 1);

        float rand = Random.Range(0f, 100f);
        if(rand <= 6)
        {
            int randomPowerUp = Random.Range(0, powerUps.Count);
            GameObject powerUp =  Instantiate(powerUps[randomPowerUp]);
            powerUp.transform.position = transform.position;
        }
        LevelGenerator.Instance.tilesToDestroy--;
        LevelGenerator.Instance.tryNextLevel();
        Destroy(gameObject);
    }
}
