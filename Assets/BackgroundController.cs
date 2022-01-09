using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour, IPointerClickHandler
{
    public WorldController world;

    public void OnPointerClick(PointerEventData eventData) => world.OnPointerClick(eventData);
}
