using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorItem : MonoBehaviour, IPointerClickHandler {

    public Image selectImg;
    public Image topImg;
    bool selected = true;

    public Color color;

    public void SetContent(Color _color) {
        color = _color;
        topImg.color = color;
    }

    private void Update() {
        if(FreePaintUI.selectItem == this && !selected) {
            selectImg.enabled = true;
            selected = true;
        }
        if (FreePaintUI.selectItem != this && selected) {
            selectImg.enabled = false;
            selected = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Click " + color.ToString());
        FreePaintUI.selectItem = this;
    }
}
