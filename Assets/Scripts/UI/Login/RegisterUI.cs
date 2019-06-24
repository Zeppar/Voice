using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterUI : MonoBehaviour {

    public RegisterProgress registerProgress;
    public List<GameObject> pages;

    [Header("Page1")]
    public InputField phoneInput;
    public InputField codeInput;
    public InputField passwordInput;
    public InputField passwordConfirmInput;
    public Button getCodeBtn;
    public Button page1NextBtn;

    [Header("Page2")]
    public InputField nameInput;
    public InputField yearInput;
    public InputField monthInput;
    public InputField dayInput;
    public Toggle manToggle;
    public Toggle womanToggle;
    public Button page2NextBtn;

    [Header("Page3")]
    public Button finishBtn;
    public Text stepText;
    AccountInfo info = null;

    private void OnEnable() {
        info = new AccountInfo();
        ActivePage(0);
    }

    private void Start() {
        // page1
        getCodeBtn.onClick.AddListener(() => {

        });
        page1NextBtn.onClick.AddListener(() => {
            if (CheckRegister(0)) {
                info.username = phoneInput.text.Trim();
                info.password = passwordInput.text.Trim();
                ActivePage(1);
            } else {
                GameController.manager.infoAlert.ShowWithText("请检查信息");
            }
        });
        // page2
        manToggle.onValueChanged.AddListener((bool isOn) => {
            info.sex = 0;
        });
        womanToggle.onValueChanged.AddListener((bool isOn) => {
            info.sex = 1;
        });
        manToggle.isOn = true;
        page2NextBtn.onClick.AddListener(() => {
            if (CheckRegister(1)) {
                info.name = nameInput.text.Trim();
                info.birthday = yearInput.text.Trim() + "-" + monthInput.text.Trim() + "-" + dayInput.text.Trim(); 
                ActivePage(2);
                GameController.manager.curFingerType = FingerPageType.Register;
            } else {
                GameController.manager.infoAlert.ShowWithText("请检查信息");
            }
        });
        // page3
        finishBtn.onClick.AddListener(() => {
            if (CheckRegister(2)) {
                info.finger = System.Text.Encoding.UTF8.GetString(GameController.manager.fingerTemplate);
                bool suc = GameController.manager.accountMan.RegisterAccount(info);
                if (suc) {
                    SoundManager.manager.StopMusic();
                    SceneManager.LoadScene(1);
                    GameController.manager.enterFromGame = false;
                    GameController.manager.curFingerType = FingerPageType.Other;
                }
            } else {
                GameController.manager.infoAlert.ShowWithText("请检查信息");
            }
        });

        manToggle.isOn = true;
    }

    void ActivePage(int index) {
        for (int i = 0; i < pages.Count; i++) {
            pages[i].SetActive(false);
        }
        pages[index].SetActive(true);
        registerProgress.SetProgress(index);
    }

    private bool CheckRegister(int pageIndex) {
        switch(pageIndex) {
            case 0:
                return phoneInput.text.Trim() != "" 
                    && passwordInput.text.Trim() != "" && passwordConfirmInput.text.Trim() != ""
                    && passwordInput.text.Trim() == passwordConfirmInput.text.Trim();
            case 1:
                return nameInput.text.Trim() != "" && yearInput.text.Trim() != ""
                    && monthInput.text.Trim() != "" && dayInput.text.Trim() != "";
            case 2:
                return GameController.manager.fingerTemplate.Length != 0;
            default:
                return false;
        }
    }

    private void Update() {
        stepText.text = (GameController.manager.fingerDataList.Count + 1) + "/3";
    }
}
