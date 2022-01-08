using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour, IPointerClickHandler
{
    public GameObject puckPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        Instantiate(puckPrefab, mousePosition, Quaternion.identity);
    }
}
