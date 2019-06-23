using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTypeItem : MonoBehaviour
{
    public Text nameText;
    public Image showImage;

    public MusicTypeInfo info;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            MusicUI.selectedTypeItem = this;
        });
    }

    public void SetContent(MusicTypeInfo _info) {
        info = _info;
        showImage.sprite = Resources.Load<Sprite>("Music/" + info.type);
        nameText.text = info.name;
    }
}
