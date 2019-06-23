using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubjectItem : MonoBehaviour, IPointerClickHandler {
    public Text nameText;
    public SubjectInfo info;

    public void OnPointerClick(PointerEventData eventData) {
        SubjectUI.selectSubject = this;
    }

    public void SetContent(SubjectInfo _info) {
        info = _info;
        nameText.text = info.title;
    }
}
