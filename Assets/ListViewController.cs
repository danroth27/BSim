using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ListViewSelectionChangedEvent : UnityEvent<GameObject, GameObject> { }

public class ListViewController : MonoBehaviour
{
    public GameObject listViewContent;
    public GameObject listItemPrefab;
    public ListViewSelectionChangedEvent OnSelectionChanged;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject selectedListValueItem;

    public GameObject SelectedListViewItem
    {
        get => selectedListValueItem;
        set
        {
            var oldSelection = selectedListValueItem;
            selectedListValueItem = value;
            OnSelectionChanged.Invoke(oldSelection, selectedListValueItem);
        }
    }

    public object SelectedValue => SelectedListViewItem?.GetComponent<ListViewItemController>().Value;

    public int SelectedIndex => SelectedListViewItem != null ? SelectedListViewItem.transform.GetSiblingIndex() : -1;

    public void ListItemClicked(GameObject listItem)
    {
        if (SelectedListViewItem == null)
        {
            SelectedListViewItem = listItem;
        }
        else if (listItem == SelectedListViewItem)
        {
            SelectedListViewItem = null;
        }
        else
        {
            var selectedListItemController = SelectedListViewItem.GetComponent<ListViewItemController>();
            selectedListItemController.IsSelected = false;
            SelectedListViewItem = listItem;
        }
    }

    public void AddListViewItem(GameObject listViewItem)
    {
        listViewItem.transform.SetParent(listViewContent.transform, worldPositionStays: false);
    }

    public void AddListViewItem(object value, string label)
    {
        
        var listItem = Instantiate(listItemPrefab, listViewContent.transform, worldPositionStays: false);
        var listItemController = listItem.GetComponent<ListViewItemController>();
        listItemController.Value = value;
        listItemController.Label = label;
    }

    public void RemoveListViewItem(GameObject listViewItem)
    {
        GameObject.Destroy(listViewItem);
    }

    public void RemoveSelectedItem() => RemoveListViewItem(SelectedListViewItem);

    public void InsertListViewItem(GameObject listViewItem, int index)
    {
        if (index < 0) throw new IndexOutOfRangeException($"Index {index} is less than zero");

        if (!listViewItem.transform.IsChildOf(listViewContent.transform))
        {
            AddListViewItem(listViewItem);
        }

        listViewItem.transform.SetSiblingIndex(index);
    }
}
