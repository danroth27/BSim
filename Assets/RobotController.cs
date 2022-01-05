using BSim;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RobotController : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D robotCollider2D;
    Collider2D leftProximitySensorCollider2D;
    Collider2D rightProximitySensorCollider2D;
    float vLeft;
    float vRight;

    public float RobotSpeed { get; set; } = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        robotCollider2D = GetComponent<CircleCollider2D>();
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
        var bumperForce = Vector2.Dot(-transform.right, contactPoints.Aggregate(new Vector2(), (v, cp) => v + cp.normal * cp.normalImpulse));
           
        var lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        var leftLightSensorPosition = transform.position + Quaternion.Euler(0, 0, 30) * transform.right * robotCollider2D.radius;
        var rightLightSensorPosition = transform.position + Quaternion.Euler(0, 0, -30) * transform.right * robotCollider2D.radius;

        float leftLightSensor = GetLightSensorValue(leftLightSensorPosition, lightSources);
        float rightLightSensor = GetLightSensorValue(rightLightSensorPosition, lightSources);

        return new RobotSensors
        {
            IsBumping = bumping.Any(),
            BumperForce = bumperForce,
            LeftLightSensor = leftLightSensor,
            RightLightSensor = rightLightSensor,
            LeftProximitySensor = leftProximitySensorCollider2D.IsTouching(new ContactFilter2D { useTriggers = false }),
            RightProximitySensor = rightProximitySensorCollider2D.IsTouching(new ContactFilter2D { useTriggers = false }),
            LeftWheelSpeed = vLeft,
            RightWheelSpeed = vRight,
            TimeStep = Time.fixedTime
        };
    }

    float GetLightSensorValue(Vector3 lightSensorPosition, GameObject[] lightSources)
    {
        float lightSensorValue = 0;
        
        foreach (var lightSource in lightSources)
        {
            var lightPosition = lightSource.transform.position;
            var lightSensorVector = lightSensorPosition - lightPosition;

            var lightRaycastHits = new List<RaycastHit2D>();
            Physics2D.Raycast(
                lightPosition, 
                lightSensorVector, 
                new ContactFilter2D { useTriggers = false }, 
                lightRaycastHits, 
                lightSensorVector.magnitude);

            if (!lightRaycastHits.Any(hit => hit.collider.CompareTag("Wall")))
            {
                var light = lightSource.GetComponent<Light2D>();
                lightSensorValue += Mathf.Min(light.intensity / Mathf.Pow(lightSensorVector.magnitude - light.pointLightInnerRadius + 1, 2), light.intensity);
            }
        }
        return lightSensorValue;
    }
}
