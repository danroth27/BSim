using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewContentController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SelectedListedItem { get; set; }

    public void ListItemClicked(GameObject listItem)
    {
        if (SelectedListedItem == null)
        {
            SelectedListedItem = listItem;
        }
        else if (listItem == SelectedListedItem)
        {
            SelectedListedItem = null;
        }
        else
        {
            var selectedListItemController = SelectedListedItem.GetComponent<ListViewItemController>();
            selectedListItemController.IsSelected = false;
            SelectedListedItem = listItem;
        }
    }
}
