using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMoreBalls : PoweUp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerPlatform"))
        {
            PlayerManager.Instance.PowerUpMoreBalls();
            Destroy(gameObject);
        }
    }
}
