using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProximitySensorController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Collider2D proximitySensorCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        proximitySensorCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsTriggered { get; set; }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.isTrigger) return;
        spriteRenderer.color = Color.red;
        IsTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.isTrigger) return;

        var contacts = new List<Collider2D>();
        proximitySensorCollider2D.GetContacts(contacts);
        if (contacts.Any(c2d => !c2d.isTrigger)) return;

        spriteRenderer.color = Color.white;
        IsTriggered = false;
    }
}
