using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class AccountInfo {
    public string username;
    public string password;
    // 0 for man 1 for woman
    public int sex;
    public string birthday;
    public string name;
    public int avatar;
    public string finger;
    public string quato;
}

public class AccountManager {

    public readonly Dictionary<string, AccountInfo> accountDic = new Dictionary<string, AccountInfo>();
    public AccountInfo selfInfo = null;

    public void ParseAllAccount() {
        string wwwpath = "";
        string filePath = "";
        string dirPath = "";
#if UNITY_EDITOR
        wwwpath = "file://" + Application.streamingAssetsPath + "/Account/account.json";
        filePath = Application.streamingAssetsPath + "/Account/account.json";
        dirPath = Application.streamingAssetsPath + "/Account";
#else
        wwwpath = "file://" + Application.persistentDataPath + "/Account/account.json";
        filePath = Application.persistentDataPath + "/Account/account.json";
        dirPath = Application.persistentDataPath + "/Account";
#endif
        accountDic.Clear();
        if (!File.Exists(filePath)) {
            Directory.CreateDirectory(dirPath);
            File.Create(filePath);
            Debug.Log("Create : " + filePath);
            return;
        }
        GameController.manager.StartCoroutine(LoadAccount(wwwpath));
    }

    void AddAdmin() {
        if (accountDic.ContainsKey("admin"))
            return;
        AccountInfo info = new AccountInfo();
        info.username = "admin";
        info.password = "admin";
        info.sex = 0;
        info.birthday = "2019-01-01";
        info.name = "admin";
        info.avatar = 1;
        info.finger = "";
        info.quato = "请输入签名";
        accountDic.Add(info.username, info);
    }

    IEnumerator LoadAccount(string path) {
        //www load streaming folder need add 'file://' at the begining of the url
        WWW www = new WWW(path);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Trim() == "")
            yield break;
        JsonData data = JsonMapper.ToObject(www.text);
        for (int i = 0; i < data.Count; i++) {
            JsonData dt = data[i];
            AccountInfo info = new AccountInfo();
            info.username = (string)dt["username"];
            info.password = (string)dt["password"];
            info.sex = (int)dt["sex"];
            info.birthday = (string)dt["birthday"];
            info.name = (string)dt["name"];
            info.avatar = (int)dt["avatar"];
            try {
                info.finger = (string)dt["finger"];
            } catch {
                info.finger = "";
            }
            try {
                info.quato = (string)dt["quato"];
            } catch {
                info.quato = "请输入签名";
            }
            accountDic.Add(info.username, info);
        }
        AddAdmin();
    }

    public bool ValidateAccount(string username, string password) {
        if(!accountDic.ContainsKey(username)) {
            Debug.Log("用户不存在");
            return false;
        }
        if(accountDic[username].password != password) {
            Debug.Log("密码错误");
            return false;
        }
        selfInfo = accountDic[username];
        if (username == "admin") {
            GameController.manager.curIdentityType = IdentityType.Admin;
        } else {
            GameController.manager.curIdentityType = IdentityType.User;
        }
        return true;
    }

    public bool RegisterAccount(AccountInfo info) {
        if(accountDic.ContainsKey(info.username) || info.username.ToLower() == "admin") {
            GameController.manager.infoAlert.ShowWithText("用户已存在");
            return false;
        }
        info.avatar = 1;
        accountDic[info.username] = info;
        selfInfo = info;
        SaveToFile();
        return true;
    }

    public void UpdateAccount(AccountInfo info) {
        if (!accountDic.ContainsKey(info.username)) {
            GameController.manager.infoAlert.ShowWithText("用户不存在");
            return;
        }
        accountDic[info.username] = info;
        SaveToFile();
    }

    public void SaveToFile() {
        string path = "";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "/Account/account.json";
#else
        path = Application.persistentDataPath + "/Account/account.json";
#endif
        FileStream fs = new FileStream(path, FileMode.Create);
        StringBuilder sb = new StringBuilder();
        JsonWriter w = new JsonWriter(sb);
        w.WriteArrayStart();
        foreach(var k in accountDic.Keys) {
            AccountInfo info = accountDic[k];
            w.WriteObjectStart();
            w.WritePropertyName("username");
            w.Write(info.username);
            w.WritePropertyName("password");
            w.Write(info.password);
            w.WritePropertyName("sex");
            w.Write(info.sex);
            w.WritePropertyName("birthday");
            w.Write(info.birthday);
            w.WritePropertyName("name");
            w.Write(info.name);
            w.WritePropertyName("avatar");
            w.Write(info.avatar);
            w.WritePropertyName("finger");
            w.Write(info.finger);
            w.WritePropertyName("quato");
            w.Write(info.quato);
            w.WriteObjectEnd();
        }
        w.WriteArrayEnd();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public void ValidateFingerData(string s) {
        AndroidJavaObject obj = new AndroidJavaObject("com.berry_med.bf.spo2_bluetooth.MainActivity");
        foreach(KeyValuePair<string, AccountInfo> kv in accountDic) {
            int score = obj.Call<int>("getVerity", Encoding.UTF8.GetBytes(kv.Value.finger), Encoding.UTF8.GetBytes(s));
            if(score >= 50) {
                selfInfo = kv.Value;
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                GameController.manager.curFingerType = FingerPageType.Other;
                break;
            }
        }
    }
}
