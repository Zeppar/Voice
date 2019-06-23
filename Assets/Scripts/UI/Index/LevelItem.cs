using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelItem : MonoBehaviour
{
    public Text nameText;
    public Image backgroundImg;
    public Button startBtn;
    bool selected = true;

    LevelInfo levelInfo;
    void Start()
    {
        startBtn.onClick.AddListener(() => {
            SoundManager.manager.StopMusic();
            GameController.manager.levelMan.selectInfo = levelInfo;
            SceneManager.LoadScene(2);
        });   
    }

    public void SetContent(LevelInfo _levelInfo) {
        levelInfo = _levelInfo;
        nameText.text = levelInfo.name;
        backgroundImg.sprite = levelInfo.icon;
    }

    private void Update() {
        if(transform.GetSiblingIndex() == GameScrollUI.itemIdx && !selected) {
            ResetSize(true);
            selected = true;
        }
        if (transform.GetSiblingIndex() != GameScrollUI.itemIdx && selected) {
            ResetSize(false);
            selected = false;
        }
    }

    public void ResetSize(bool scale) {
        backgroundImg.transform.localScale = scale ? Vector3.one : new Vector3(0.85f, 0.85f, 1.0f);
    }


}
