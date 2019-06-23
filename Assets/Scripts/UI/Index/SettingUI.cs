using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public List<GameObject> items;
    public Button manBtn;
    public Button womanBtn;
    public Slider volumeSlider;
    public Button changePasswordBtn;
    public Button logoutBtn;
    public InputField passwordInput;
    private int pageIdx = 0;
    public int PageIdx {
        get {
            return pageIdx;
        }
    }

    private void OnEnable() {
        SetPageIdx(0);
    }

    public void ConfirmPassword() {
        if (passwordInput.text.Trim().Length == 0) {
            GameController.manager.infoAlert.ShowWithText("请填写密码");
            return;
        }
        GameController.manager.accountMan.selfInfo.password = passwordInput.text.Trim();
        SetPageIdx(0);
    }

    private void Start() {
        manBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.selfInfo.sex = 0;
        });
        womanBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.selfInfo.sex = 1;
        });
        volumeSlider.onValueChanged.AddListener((float value) => {
            // todo
        });
        changePasswordBtn.onClick.AddListener(() => {
            SetPageIdx(1);
        });
        logoutBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.UpdateAccount(GameController.manager.accountMan.selfInfo);
            SceneManager.LoadScene(0);
            GameController.manager.accountMan.selfInfo = null;
        });
    }

    private void SetPageIdx(int idx) {
        pageIdx = idx;
        if(idx == 0) {
            for(int i = 0; i < items.Count; i++) {
                items[i].SetActive(true);
            }
            items[4].SetActive(false);
        } else {
            for (int i = 0; i < items.Count; i++) {
                items[i].SetActive(false);
            }
            items[4].SetActive(true);
        }
    }
}
