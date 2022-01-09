using BSim.Behaviors;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PropertyEditorController : MonoBehaviour
{
    public Text propertyLabel;
    public InputField propertyInputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PropertyInfo PropertyInfo { get; private set; }

    public object EditableValue { get; private set; }

    public void SetEditableProperty(object editableValue, PropertyInfo propertyInfo)
    {
        EditableValue = editableValue;
        PropertyInfo = propertyInfo;
        propertyLabel.text = propertyInfo.Name.ToFriendlyName();
        propertyInputField.text = propertyInfo.GetValue(EditableValue).ToString();
    }

    public void SetPropertyValue(string value)
    {
        var convertedValue = TypeDescriptor.GetConverter(PropertyInfo.PropertyType).ConvertFromString(value);
        PropertyInfo.SetValue(EditableValue, convertedValue);
    }
}
