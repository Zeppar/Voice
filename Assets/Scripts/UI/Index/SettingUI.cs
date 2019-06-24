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
    public Sprite[] manSp;
    public Sprite[] womanSp;
    public Slider volumeSlider;
    public Button changePasswordBtn;
    public Button logoutBtn;
    public InputField passwordInput;
    public InputField passwordConfirmInput;
    public Button confirmBtn;
    public Button bluetoothBtn;

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
        bluetoothBtn.onClick.AddListener(() => {
            AndroidJavaObject obj = new AndroidJavaObject("com.berry_med.bf.spo2_bluetooth.MainActivity");
            obj.Call("scan");
        });
        confirmBtn.onClick.AddListener(() => {
            if (passwordInput.text.Trim().Length == 0 || passwordConfirmInput.text.Trim().Length == 0) {
                GameController.manager.infoAlert.ShowWithText("请完善修改信息");
                return;
            }
            if (passwordInput.text.Trim() != passwordConfirmInput.text.Trim()) {
                GameController.manager.infoAlert.ShowWithText("两次输入不一致");
                return;
            }
            GameController.manager.accountMan.selfInfo.password = passwordInput.text.Trim();
            GameController.manager.infoAlert.ShowWithText("修改成功", null);
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
            items[5].SetActive(false);
            items[6].SetActive(false);
            items[7].SetActive(false);
        } else {
            for (int i = 0; i < items.Count; i++) {
                items[i].SetActive(false);
            }
            items[5].SetActive(true);
            items[6].SetActive(true);
            items[7].SetActive(true);
        }
    }

    private void Update() {
        manBtn.GetComponent<Image>().sprite = GameController.manager.accountMan.selfInfo.sex == 0 ? manSp[0] : manSp[1];
        womanBtn.GetComponent<Image>().sprite = GameController.manager.accountMan.selfInfo.sex == 1 ? womanSp[0] : womanSp[1];
    }
}
