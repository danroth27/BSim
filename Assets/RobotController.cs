using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = 2f;
        float angularSpeed = 90f;
        rb.velocity = transform.right * speed * Input.GetAxis("Vertical");
        rb.angularVelocity = -Input.GetAxis("Horizontal") * angularSpeed;
    }
}
