using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UserItem : MonoBehaviour
{
    public Text nameText;
    public Text sexText;
    public Text ageText;
    public Text dateText;
    public Button checkBtn;
    public Button changePwdBtn;
    public Button deleteBtn;

    private UserListUI userListUI;
    public AccountInfo info;

    private void Start() {
        checkBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Report);
            FindObjectOfType<ReportUI>().ShowReportList(0, info.username);
        });

        changePwdBtn.onClick.AddListener(() => {
            userListUI.ShowChangePasswordAlert(this);
        });

        deleteBtn.onClick.AddListener(() => {
            userListUI.ShowDeleteUserAlert(this);
        });
    }

    public void SetContent(AccountInfo _info, UserListUI _userListUI) {
        info = _info;
        userListUI = _userListUI;
        nameText.text = info.name;
        sexText.text = info.sex == 0 ? "男" : "女";
        ageText.text = (DateTime.Now.Year - int.Parse(info.birthday.Split('-')[0])).ToString();
        dateText.text = info.birthday;
    }

    private void Update() {
        checkBtn.gameObject.SetActive(!userListUI.isEditing);
        changePwdBtn.gameObject.SetActive(userListUI.isEditing);
        deleteBtn.gameObject.SetActive(userListUI.isEditing);
    }
}
