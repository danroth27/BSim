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
    float vLeft;
    float vRight;

    public float RobotSpeed { get; set; } = 2f;

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
        var v = Input.GetAxis("Vertical");
        var h= Input.GetAxis("Horizontal");


        vLeft = speed * Math.Max(Math.Min(v + h, 1), -1);
        vRight = speed * Math.Max(Math.Min(v - h, 1), -1);

        rb.velocity = transform.right * (vLeft + vRight) / 2f;
        rb.angularVelocity = (vRight - vLeft) * Mathf.Rad2Deg;
    }

    public void ExecuteRobotCommand(RobotCommand command)
    {

    }

    public RobotSensors GetRobotSensors()
    {
        var contactPoints = new List<ContactPoint2D>();
        robotCollider2D.GetContacts(contactPoints);
        var bumping = contactPoints.Where(cp => Vector2.Dot(cp.normal, transform.right) < 0);

        return new RobotSensors
        {
            IsBumping = bumping.Any(),
            IsPushing = bumping.Any(cp => cp.rigidbody != null),
            LeftLightSensor = 0,
            RightLightSensor = 0,
            LeftProximitySensor = leftProximitySensorCollider2D.IsTouching(new ContactFilter2D { useTriggers = false }),
            RightProximitySensor = rightProximitySensorCollider2D.IsTouching(new ContactFilter2D { useTriggers = false }),
            LeftWheelSpeed = vLeft,
            RightWheelSpeed = vRight,
            TimeStep = Time.fixedTime,
        };
    }
}
