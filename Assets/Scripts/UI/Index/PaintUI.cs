using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PaintUI : MonoBehaviour {

    public Button backBtn;
    public Button paintBtn1;
    public Button paintBtn2;

    private void Start() {
        backBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Index);
        });

        paintBtn1.onClick.AddListener(() => {
            GameController.manager.isPaint = true;
            SceneManager.LoadScene(2);
        });

        paintBtn2.onClick.AddListener(() => {
            GameController.manager.levelMan.selectInfo = new LevelInfo();
            GameController.manager.levelMan.selectInfo.index = 5;
            GameController.manager.levelMan.selectInfo.bg = Resources.Load<Sprite>("Level/game5Bg");
            SceneManager.LoadScene(2);
        });
    }

}
