using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweUp : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
