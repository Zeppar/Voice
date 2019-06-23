using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StudyItem : MonoBehaviour, IPointerClickHandler
{
    public Text nameText;
    public Image showImage;
    public Sprite[] sps;

    public StudyInfo info;

    public void OnPointerClick(PointerEventData eventData) {
        StudyUI.selectItem = this;
    }

    public void SetContent(StudyInfo _info) {
        info = _info;
        nameText.text = info.name;
        showImage.sprite = sps[(int)info.type];
    }
}
