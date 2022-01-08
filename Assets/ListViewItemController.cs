using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListViewItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Text text;
    private Color color;
    private ListViewController listViewController;
    private bool isSelected;

    public Color hoverColor;
    public Color selectedColor;

    void Awake()
    {
        image = GetComponent<Image>();
        color = image.color;
        text = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        listViewController = GetComponentInParent<ListViewController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object Value { get; set; }
    public string Label
    {
        get => this.text.text;
        set => this.text.text = value;
    }

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            image.color = isSelected ? selectedColor : color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            image.color = color;

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsSelected = !IsSelected;
        listViewController.ListItemClicked(gameObject);
    }
}
