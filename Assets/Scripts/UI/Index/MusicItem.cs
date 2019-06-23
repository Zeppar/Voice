using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicItem : MonoBehaviour, IPointerClickHandler
{
    public Text indexText;
    public Text nameText;
    public Text timeText;

    public MusicInfo info;

    private void Start() {

    }

    public void SetContent(MusicInfo _info) {
        info = _info;
        indexText.text = info.index.ToString();
        nameText.text = info.name;
        timeText.text = Util.SecToTimeString(info.time);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        MusicUI.selectedItem = this;
    }
}
