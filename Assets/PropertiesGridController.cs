using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PropertiesGridController : MonoBehaviour
{
    public GameObject propertyEditorPrefab;
    public GameObject propertiesGridContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPropertyEditor(PropertyInfo propertyInfo)
    {
        var propertyEditor = Instantiate(propertyEditorPrefab, propertiesGridContent.transform, worldPositionStays: false);
        //propertyEditor.GetComponent<Property>
    }

    public void AddPropertyEditors(IEnumerable<PropertyInfo> properties)
    {
        RemoveAllPropertyEditors();
        foreach (var propertyInfo in properties)
        {
            AddPropertyEditor(propertyInfo);
        }
    }

    public void RemoveAllPropertyEditors()
    {
        foreach (Transform propertyEditorTransform in propertiesGridContent.transform)
        {
            Destroy(propertyEditorTransform.gameObject);
        }
    }
}
