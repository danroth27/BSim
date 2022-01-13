using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LatencyLabelController : MonoBehaviour
{
    Text latencyLabel;

    // Start is called before the first frame update
    void Start()
    {
        latencyLabel = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLatencyChanged(float latency)
    {
        latencyLabel.text = $"Latency: {latency * Time.fixedDeltaTime:f2}s";
    }

}
