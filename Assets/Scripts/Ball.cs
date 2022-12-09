using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField][Range(0.1f, 100f)] float speed;
    bool alreadyHit;

    void Start()
    {
        if (PlayerManager.Instance.ballsInGame.Contains(gameObject)==false)
        {
            PlayerManager.Instance.ballsInGame.Add(gameObject);
        }

        if (direction == Vector3.zero)
        {
            direction = (transform.right + transform.up).normalized;
            //transform.forward = -direction;
        }
    }

    void FixedUpdate()
    {
        if(alreadyHit)
        {
            alreadyHit = false;
        }

        transform.position += direction * speed * Time.deltaTime;

        if (PlayerManager.Instance.gameObject.transform.position.y > transform.position.y + 1)
        {
            if (PlayerManager.Instance.ballsCount > 1)
            {
                PlayerManager.Instance.ballsInGame.Remove(gameObject);
                Destroy(gameObject);
                PlayerManager.Instance.ballsCount--;
            }
            else
            {
                PlayerManager.Instance.LoseHp();
                direction = (transform.right + transform.up).normalized;
                transform.position = PlayerManager.Instance.gameObject.transform.position + new Vector3(0, 2, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            direction = Vector3.Reflect(direction, FindCollisonDirection(other.gameObject));
            other.GetComponent<AudioSource>().Play();
        }
            
        if (alreadyHit == false)
        {
            if (other.CompareTag("Block"))
            {
                Debug.Log("HIT: " + other.gameObject);
                direction = Vector3.Reflect(direction, FindCollisonDirection(other.gameObject));
                other.gameObject.GetComponent<Tile>().Hit();
            }
            else if (other.CompareTag("PlayerPlatform"))
            {
                direction = PlayerPlatformBounceDirecton(other.gameObject);
                other.GetComponent<AudioSource>().Play();
            }
            alreadyHit=true;
        }
        //transform.forward = -direction;
    }

    Vector3  FindCollisonDirection(GameObject obj)
    {
        Vector3 collisonNormal = -Vector3.up;

        float xDistance = transform.position.x - obj.transform.position.x;
        float yDistance = transform.position.y - obj.transform.position.y;

        xDistance = xDistance / obj.transform.localScale.x;
        yDistance = yDistance / obj.transform.localScale.y;

        if(Mathf.Abs(xDistance) >= Mathf.Abs(yDistance))
        {
            collisonNormal = new Vector3(Mathf.Sign(xDistance),0,0);
        }
        else
        {
            collisonNormal = new Vector3(0, Mathf.Sign(yDistance), 0);
        }

        return collisonNormal;
    }

    Vector3 PlayerPlatformBounceDirecton(GameObject obj)
    {
        float xDistance = transform.position.x - obj.transform.position.x;
        xDistance = xDistance / obj.transform.localScale.x;

        return (new Vector3(xDistance, 0.35f, 0)).normalized;
    }
}
