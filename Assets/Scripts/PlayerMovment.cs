using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float speed = 1f;
    [SerializeField] float xStoper = 28f;
    //[SerializeField] private float acceleration = 1f;

    [Header("Inspect")]
    [SerializeField] Vector3 moveVector = new Vector3(0, 0, 0);

    float right = 0;


    // Update is called once per frame
    void FixedUpdate()
    {
        MovmentControlls();
    }

    void MovmentControlls()
    {

        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
        {
            if (Input.GetKey(KeyCode.D) && transform.position.x < xStoper)
            {
                right = speed;
            }
            else if (Input.GetKey(KeyCode.A) && transform.position.x > -xStoper)
            {
                right = -speed;
            }
            else
            {
                right = 0;
            }
        }
        else
        {
            right = 0;
        }

        if (right != 0)
        {
            moveVector = Vector3.zero;
            moveVector.x = right;

            //rb.AddForce(((moveVector * speed) - rb.velocity) * acceleration);
            transform.position += moveVector * speed * Time.deltaTime;
        }
    }
}