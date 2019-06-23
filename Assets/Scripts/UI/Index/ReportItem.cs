using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReportItem : MonoBehaviour, IPointerClickHandler {

    public Text nameText;
    public Button deleteBtn;
    public ReportInfo info;

    private void Start() {
        deleteBtn.onClick.AddListener(() => {
            GameController.manager.reportMan.DeleteReport(info);
            Destroy(gameObject);
        });
        deleteBtn.gameObject.SetActive(false);
    }

    public void SetContent(ReportInfo _info) {
        info = _info;
        nameText.text = info.name;
    }

    public void OnPointerClick(PointerEventData eventData) {
        ReportUI.selectItem = this;
    }
}
