using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrap : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private BoxCollider2D worldCollider;
    private Vector3 dragOffset;
    private Camera eventCamera;

    public void Awake()
    {
        var world = GameObject.FindGameObjectWithTag("World");
        worldCollider = world.GetComponentInChildren<BoxCollider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventCamera = eventData.enterEventCamera;
        dragOffset = transform.position - GetMousePosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePosition = GetMousePosition(eventData);
        
        if (worldCollider.OverlapPoint(mousePosition))
        {
            transform.position = GetMousePosition(eventData) + dragOffset;
        }
    }

    Vector3 GetMousePosition(PointerEventData eventData)
    {
        var mousePosition = eventCamera.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        return mousePosition;
    }
}
