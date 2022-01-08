using BSim;
using BSim.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class RobotController : MonoBehaviour, IRobotController, IProgrammableRobot, IPointerClickHandler
{
    private Rigidbody2D robotBody;
    private CircleCollider2D robotCollider2D;
    private ProximitySensorController leftProximitySensor, rightProximitySensor;
    private float vLeft, vRight;
    private GameObject robotProgrammer;
    public GameObject robotProgrammerPrefab;
    public GameObject canvas;


    // Start is called before the first frame update
    private void Awake()
    {
        robotBody = GetComponent<Rigidbody2D>();
        robotCollider2D = GetComponent<CircleCollider2D>();
        var proximitySensors = GetComponentsInChildren<ProximitySensorController>();
        leftProximitySensor = proximitySensors.First(proximitySensor => proximitySensor.name.Contains("Left"));
        rightProximitySensor = proximitySensors.First(proximitySensor => proximitySensor.name.Contains("Right"));

        Arbiter = new FixedPriorityArbiter(this);
        Behaviors = new List<IBehavior>
        {
            //new Gizmo(Arbiter),
            //new London(Arbiter) { Length = 4 },
            new Cruise(Arbiter),
            new Home(Arbiter),
            //new Avoid(Arbiter),
            //new AntiMoth(Arbiter),
            //new DarkPush(Arbiter),
            new Escape(Arbiter)
            //new Remote(Arbiter)
        };
        Arbiter.SetBehaviorPrioritiesInOrder(Behaviors);

        robotProgrammer = Instantiate(robotProgrammerPrefab, canvas.transform, worldPositionStays: false);
        robotProgrammer.SetActive(false);

        var robotProgrammerController = robotProgrammer.GetComponent<RobotProgrammerController>();
        robotProgrammerController.ProgrammableRobot = this;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var sensors = GetRobotSensors();
        foreach (var behavior in Behaviors)
        {
            behavior.Update(sensors);
        }
    }

    public IList<IBehavior> Behaviors { get; private set; }
    public FixedPriorityArbiter Arbiter { get; private set; }

    public void ExecuteRobotCommand(RobotCommand robotCommand)
    {
        vLeft = robotCommand.LeftWheelSpeed;
        vRight = robotCommand.RightWheelSpeed;
        robotBody.velocity = transform.right * (robotCommand.LeftWheelSpeed + robotCommand.RightWheelSpeed) / 2;
        robotBody.angularVelocity = (robotCommand.RightWheelSpeed - robotCommand.LeftWheelSpeed) * Mathf.Rad2Deg;
    }

    public RobotSensors GetRobotSensors()
    {
        // Bumper
        var contactPoints = new List<ContactPoint2D>();
        robotCollider2D.GetContacts(contactPoints);
        var bumping = contactPoints.Any(cp => Vector2.Dot(cp.normal, transform.right) < 0);
        var bumperForce = Vector2.Dot(-transform.right, contactPoints.Aggregate(new Vector2(), (v, cp) => v + cp.normal * cp.normalImpulse));

        // Light sensors
        var lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        var leftLightSensorPosition = transform.position + Quaternion.Euler(0, 0, 30) * transform.right * robotCollider2D.radius;
        var rightLightSensorPosition = transform.position + Quaternion.Euler(0, 0, -30) * transform.right * robotCollider2D.radius;
        float leftLightSensor = GetLightSensorValue(leftLightSensorPosition, lightSources);
        float rightLightSensor = GetLightSensorValue(rightLightSensorPosition, lightSources);

        return new RobotSensors
        {
            IsBumping = bumping,
            BumperForce = bumperForce,
            LeftLightSensor = leftLightSensor,
            RightLightSensor = rightLightSensor,
            LeftProximitySensor = leftProximitySensor.IsTriggered,
            RightProximitySensor = rightProximitySensor.IsTriggered,
            LeftWheelSpeed = vLeft,
            RightWheelSpeed = vRight,
            Time = Time.fixedTime
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            robotProgrammer.SetActive(true);
        }
    }

    
}
