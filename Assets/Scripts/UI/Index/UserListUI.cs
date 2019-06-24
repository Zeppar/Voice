using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserListUI : MonoBehaviour
{
    public Button backBtn;
    public Button editBtn;
    public bool isEditing = false;

    public GameObject passwordAlert;
    public GameObject deleteUserAlert;

    public InputField passwordInput;
    public InputField passwordConfirmInput;

    public Button passwordConfirmBtn;
    public Button passwordCancelBtn;
    public Button passwordCloseBtn;

    public Button deleteUserConfirmBtn;
    public Button deleteUserCancelBtn;
    public Button deleteUserCloseBtn;

    public UserItem itemPrefab;
    public Transform content;


    private void Start() {
        editBtn.onClick.AddListener(() => {
            isEditing = !isEditing;
            if(isEditing) {
                editBtn.GetComponentInChildren<Text>().text = "退出编辑";
            } else {
                editBtn.GetComponentInChildren<Text>().text = "编辑";
            }
        });

        backBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Index);
        });

        passwordConfirmBtn.onClick.AddListener(() => {
            if(passwordInput.text.Trim().Length == 0 || passwordConfirmInput.text.Trim().Length == 0) {
                GameController.manager.infoAlert.ShowWithText("请完善信息");
                return;
            }
            if (passwordInput.text.Trim() != passwordConfirmInput.text.Trim()) {
                GameController.manager.infoAlert.ShowWithText("请检查输入信息");
                return;
            }
            item.info.password = passwordInput.text.Trim();
            GameController.manager.accountMan.UpdateAccount(item.info);
            Debug.Log("-----");
            passwordAlert.SetActive(false);
        });
        passwordCancelBtn.onClick.AddListener(() => {
            passwordAlert.SetActive(false);
        });
        passwordCloseBtn.onClick.AddListener(() => {
            passwordAlert.SetActive(false);
        });
        deleteUserConfirmBtn.onClick.AddListener(() => {
            GameController.manager.accountMan.accountDic.Remove(item.info.username);
            Destroy(item.gameObject);
            item = null;
            GameController.manager.accountMan.SaveToFile();
            deleteUserAlert.SetActive(false);
        });
        deleteUserCancelBtn.onClick.AddListener(() => {
            deleteUserAlert.SetActive(false);
        });
        deleteUserCloseBtn.onClick.AddListener(() => {
            deleteUserAlert.SetActive(false);
        });

        InitUser();
    }

    private void InitUser() {
        Util.DeleteChildren(content);
        foreach (var kv in GameController.manager.accountMan.accountDic) {
            if (kv.Key.ToLower() == "admin")
                continue;
            UserItem item = Instantiate(itemPrefab);
            item.SetContent(kv.Value, this);
            item.transform.SetParent(content);
        }
    }

    private UserItem item;
    public void ShowChangePasswordAlert(UserItem _item) {
        item = _item;
        passwordAlert.SetActive(true);
    }

    public void ShowDeleteUserAlert(UserItem _item) {
        item = _item;
        deleteUserAlert.SetActive(true);
    }

}
