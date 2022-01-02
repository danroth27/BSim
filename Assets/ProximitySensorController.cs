using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensorController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.isTrigger) return;
        spriteRenderer.color = Color.red;
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.isTrigger) return;
        spriteRenderer.color = Color.white;
    }
}
