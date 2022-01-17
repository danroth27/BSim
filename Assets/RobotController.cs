using BSim;
using BSim.Behaviors;
using BSim.Simulations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class RobotController : MonoBehaviour, IRobotController, IProgrammableRobot, IPointerClickHandler
{
    private Rigidbody2D robotBody;
    private CircleCollider2D robotCollider2D;
    public ProximitySensorController leftProximitySensor, rightProximitySensor;
    private float vLeft, vRight;
    public GameObject robotProgrammerPrefab;
    private GameObject robotProgrammer;
    private Toggle fantasyModeToggle;
    private Slider latencySlider;
    private const float wheelSpeedNoise = 0.1f;
    private Queue<RobotSensors> robotSensorReadings = new Queue<RobotSensors>();
    private RobotSensors currentSensors;
    private IBehavior executingBehavior;

    // Start is called before the first frame update
    private void Awake()
    {
        robotBody = GetComponent<Rigidbody2D>();
        robotCollider2D = GetComponent<CircleCollider2D>();

        Arbiter = new FixedPriorityArbiter(this, Behaviors);

        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        robotProgrammer = Instantiate(robotProgrammerPrefab, canvas.transform, worldPositionStays: false);
        robotProgrammer.SetActive(false);

        var robotProgrammerController = robotProgrammer.GetComponent<RobotProgrammerController>();
        robotProgrammerController.ProgrammableRobot = this;

        var simOptions = GameObject.FindGameObjectWithTag("Options");
        fantasyModeToggle = simOptions.GetComponentInChildren<Toggle>();
        latencySlider = simOptions.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        UpdateRobotSensors();
        var sensors = GetRobotSensors();
        if (sensors != null)
        {
            foreach (var behavior in Behaviors)
            {
                behavior.Update(sensors);
            }
        }
    }

    public List<IBehavior> Behaviors { get; } = new List<IBehavior>();
    public FixedPriorityArbiter Arbiter { get; private set; }

    public void UpdateBehaviors(IEnumerable<IBehavior> newBehaviors)
    {
        Behaviors.Clear();
        Behaviors.AddRange(newBehaviors);
        Arbiter.SetBehaviorPrioritiesInOrder(Behaviors);
    }

    public void ExecuteRobotCommand(RobotCommand robotCommand, IBehavior behavior)
    {
        vLeft = robotCommand.LeftWheelSpeed + GetWheelSpeedNoise();
        vRight = robotCommand.RightWheelSpeed + GetWheelSpeedNoise();
        robotBody.velocity = transform.right * (vLeft + vRight) / 2;
        robotBody.angularVelocity = (vRight - vLeft) * Mathf.Rad2Deg;
        executingBehavior = behavior;
    }

    public float GetWheelSpeedNoise() =>
        fantasyModeToggle.isOn ? 0 : UnityEngine.Random.Range(-wheelSpeedNoise, wheelSpeedNoise);

    public void UpdateRobotSensors()
    {
        // Bumper
        var contactPoints = new List<ContactPoint2D>();
        robotCollider2D.GetContacts(contactPoints);
        var bumping = contactPoints.Any(cp => Vector2.Dot(cp.normal, transform.right) < 0);
        var bumperForce = Vector2.Dot(-transform.right, contactPoints.Aggregate(new Vector2(), (v, cp) => v + cp.normal * cp.normalImpulse));

        // Light sensors
        var lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        var leftLightSensorPosition = transform.position + Quaternion.Euler(0, 0, 30) * transform.right * (robotCollider2D.radius + 0.01f);
        var rightLightSensorPosition = transform.position + Quaternion.Euler(0, 0, -30) * transform.right * (robotCollider2D.radius + 0.01f);
        float leftLightSensor = GetLightSensorValue(leftLightSensorPosition, lightSources);
        float rightLightSensor = GetLightSensorValue(rightLightSensorPosition, lightSources);

        var sensors = new RobotSensors
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

        robotSensorReadings.Enqueue(sensors);

        if (robotSensorReadings.Count > latencySlider.value)
        {
            currentSensors = robotSensorReadings.Dequeue();
        }
    }

    public RobotSensors GetRobotSensors() => currentSensors;

    public IBehavior GetExecutingBehavior() => executingBehavior;

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

            if (!lightRaycastHits.Any(hit => hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Robot")))
            {
                var light = lightSource.GetComponent<Light2D>();
                lightSensorValue += Mathf.Min(light.intensity / Mathf.Pow(lightSensorVector.magnitude - light.pointLightInnerRadius + 1, 2), light.intensity);
            }
        }
        return lightSensorValue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (robotProgrammer != null)
        {
            robotProgrammer.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        GameObject.Destroy(robotProgrammer);
    }
}
