using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarUI : MonoBehaviour {

    public GameObject layout;
    public GameObject selAvatarGo;
    public GameObject quatoGo;

    public Button avatarBtn;
    public Button nameBtn;
    public Button quatoBtn;
    public Button ageBtn;
    public Button sexBtn;
    public Button manBtn;
    public Button womanBtn;

    public Image avatarImage;

    public Text nameText;
    public Text quatoText;
    public Text ageText;
    public Text sexText;
    public InputField yearInput;
    public InputField monthInput;
    public InputField dayInput;
    public InputField nameInput;
    public InputField quatoInput;
    public Text quatoCountText;

    public Button deleteNameBtn;
    public Button saveQuatoBtn;


    public Transform avatarBtnParent;
    public AvatarBtn avatarBtnPrefab;

    private AvatarBtn lastBtn;

    public List<GameObject> items;

    private void Start() {
        avatarBtn.onClick.AddListener(() => {
            SetPageIdx(0);
        });
        nameBtn.onClick.AddListener(() => {
            SetPageIdx(1);
        });
        quatoBtn.onClick.AddListener(() => {
            SetPageIdx(2);
        });
        ageBtn.onClick.AddListener(() => {
            SetPageIdx(3);
        });
        sexBtn.onClick.AddListener(() => {
            SetPageIdx(4);
        });
        manBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.selfInfo.sex = 0;
            shouldUpdate = true;
            SetPageIdx(-1);
        });
        womanBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.selfInfo.sex = 1;
            shouldUpdate = true;
            SetPageIdx(-1);
        });
        deleteNameBtn.onClick.AddListener(() => {
            nameInput.text = "";
        });
        saveQuatoBtn.onClick.AddListener(() => {
            if (quatoInput.text.Trim().Length == 0) {
                GameController.manager.infoAlert.ShowWithText("请输入签名", null);
            }
            GameController.manager.accountMan.selfInfo.quato = quatoInput.text.Trim();
            SetPageIdx(-1);
        });

        InitAvatar();
    }

    public void OnBackBtnClick(Util.NoneParamCallBack callBack) {
        switch(curPage) {
            case -1:
                gameObject.SetActive(false);
                GameController.manager.accountMan.UpdateAccount(GameController.manager.accountMan.selfInfo);
                callBack?.Invoke();
                break;
            case 0:
                SetPageIdx(-1);
                break;
            case 1:
                if(nameInput.text.Trim().Length == 0) {
                    GameController.manager.infoAlert.ShowWithText("请输入昵称", null);
                    return;
                }
                GameController.manager.accountMan.selfInfo.name = nameInput.text.Trim();
                shouldUpdate = true;
                SetPageIdx(-1);
                break;
            case 2:
                shouldUpdate = true;
                SetPageIdx(-1);
                break;
            case 3:
                if (yearInput.text.Trim().Length == 0 || monthInput.text.Trim().Length == 0 || dayInput.text.Trim().Length == 0) {
                    GameController.manager.infoAlert.ShowWithText("请完善数据", null);
                    return;
                }
                GameController.manager.accountMan.selfInfo.birthday = yearInput.text.Trim() + "-" + monthInput.text.Trim() + "-" + dayInput.text.Trim();
                shouldUpdate = true;
                SetPageIdx(-1);
                break;
            case 4:
                shouldUpdate = true;
                SetPageIdx(-1);
                break;
            default:
                break;
        }
    }

    private void OnEnable() {
        SetPageIdx(-1);
        shouldUpdate = true;
    }


    private int curPage = 0;
    void SetPageIdx(int page) {
        curPage = page;
        switch (page) {
            case -1:
                layout.SetActive(true);
                selAvatarGo.SetActive(false);
                quatoGo.SetActive(false);

                for (int i = 0; i < items.Count; i++) {
                    if (i < 5) {
                        items[i].SetActive(true);
                    } else {
                        items[i].SetActive(false);
                    }
                }
                break;
            case 0:
                layout.SetActive(false);
                selAvatarGo.SetActive(true);
                quatoGo.SetActive(false);
                break;
            case 1:
                for (int i = 0; i < items.Count; i++) {
                    items[i].SetActive(false);
                }
                items[8].SetActive(true);
                break;
            case 2:
                layout.SetActive(false);
                selAvatarGo.SetActive(false);
                quatoGo.SetActive(true);
                break;
            case 3:
                for (int i = 0; i < items.Count; i++) {
                    items[i].SetActive(false);
                }
                items[5].SetActive(true);
                items[6].SetActive(true);
                items[7].SetActive(true);
                break;
            case 4:
                for (int i = 0; i < items.Count; i++) {
                    items[i].SetActive(false);
                }
                items[9].SetActive(true);
                items[10].SetActive(true);
                break;
            default:
                break;

        }
    }

    void InitAvatar() {
        Util.DeleteChildren(avatarBtnParent);
        for (int i = 0; i < Util.avatarCount; i++) {
            AvatarBtn item = Instantiate(avatarBtnPrefab) as AvatarBtn;
            item.SetContent(i + 1, () => {
                SetPageIdx(-1);
            });
            item.transform.SetParent(avatarBtnParent, false);
        }

        avatarImage.sprite = Resources.Load<Sprite>("Avatar/avatar" + GameController.manager.accountMan.selfInfo.avatar);

    }

    private bool shouldUpdate = true;
    private void Update() {
        if (lastBtn != IndexUI.selectAvatar) {
            lastBtn = IndexUI.selectAvatar;
            avatarImage.sprite = Resources.Load<Sprite>("Avatar/avatar" + IndexUI.selectAvatar.avatarIdx);
        }
        if (shouldUpdate) {
            AccountInfo info = GameController.manager.accountMan.selfInfo;
            nameText.text = info.name;
            quatoText.text = info.quato;
            ageText.text = (DateTime.Now.Year - int.Parse(info.birthday.Split('-')[0])).ToString();
            sexText.text = info.sex == 0 ? "男" : "女";
            yearInput.text = info.birthday.Split('-')[0];
            monthInput.text = info.birthday.Split('-')[1];
            dayInput.text = info.birthday.Split('-')[2];
            nameInput.text = info.name;
            quatoInput.text = info.quato;
            quatoCountText.text = quatoInput.text.Length.ToString() + "/20";
            shouldUpdate = false;
        }
    }


}
