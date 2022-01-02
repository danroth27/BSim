using BSim;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D robotCollider2D;
    Collider2D leftProximitySensorCollider2D;
    Collider2D rightProximitySensorCollider2D;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        robotCollider2D = GetComponent<Collider2D>();
        var proximitySensorColliders = GetComponentsInChildren<Collider2D>();
        leftProximitySensorCollider2D = proximitySensorColliders.First(c2d => c2d.name.Contains("Left"));
        rightProximitySensorCollider2D = proximitySensorColliders.First(c2d => c2d.name.Contains("Right"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = 2f;
        float angularSpeed = 90f;
        rb.velocity = transform.right * speed * Input.GetAxis("Vertical");
        rb.angularVelocity = -Input.GetAxis("Horizontal") * angularSpeed;
    }

    void ExecuteRobotCommand(RobotCommand command)
    {

    }

    RobotSensors GetRobotSensors()
    {
        return new RobotSensors
        {
            IsBumping = robotCollider2D.IsTouchingLayers(),
            LeftLightSensor = 0,
            RightLightSensor = 0,
            LeftProximitySensor = leftProximitySensorCollider2D.IsTouchingLayers(),
            RightProximitySensor = rightProximitySensorCollider2D.IsTouchingLayers(),
            LeftWheelSpeed = 0,
            RightWheelSpeed = 0,
            TimeStep = Time.fixedTime
        };
    }
}
