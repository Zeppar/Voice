using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PageType {
    Index = 0,
    Subject,
    Report,
    Music,
    Study
}

public class IndexUICtrl : MonoBehaviour
{
    public static IndexUICtrl instance = null;
    public GameObject[] pages;
    public Text heartText;
    public Text oxyText;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GameController.manager.isPaint = false;
    }

    public void SetPageType(PageType type) {
        SoundManager.manager.StopMusic();
        for (int i = 0; i < pages.Length;i++) {
            pages[i].SetActive(false);
        }
        pages[(int)type].SetActive(true);
    }

    private void Update() {
        oxyText.text = GameController.manager.oxyData;
        heartText.text = GameController.manager.heartData;
    }

}
