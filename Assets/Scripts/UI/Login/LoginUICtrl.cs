using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginUICtrl : MonoBehaviour {

    [Header("Common")]
    public GameObject loginGo;
    public GameObject registerGo;

    [Header("Login")]
    public InputField usernameInput;
    public InputField passwordInput;
    public Dropdown identityDropdown;
    public Button loginBtn;
    public Button registerBtn;



    private void Start() {
        identityDropdown.ClearOptions();
        List<string> strs = new List<string> { "管理员", "用户" };
        identityDropdown.AddOptions(strs);
        identityDropdown.onValueChanged.AddListener((int index) => {
            switch(index) {
                case 0:
                    GameController.manager.curIdentityType = IdentityType.Admin;
                    break;
                case 1:
                    GameController.manager.curIdentityType = IdentityType.User;
                    break;
                default:
                    break;
            }
        });
        identityDropdown.value = 0;

        loginBtn.onClick.AddListener(() => {
            // TODO send request to server
            //SceneManager.LoadScene(1);
            if (CheckLogin()) {
                if (GameController.manager.accountMan.ValidateAccount(usernameInput.text.Trim(), passwordInput.text.Trim())) {
                    SoundManager.manager.StopMusic();
                    SceneManager.LoadScene(1);
                    GameController.manager.enterFromGame = false;
                } else
                    GameController.manager.infoAlert.ShowWithText("用户名或密码错误");
            } else {
                GameController.manager.infoAlert.ShowWithText("请检查信息");
            }
        });

        registerBtn.onClick.AddListener(() => {
            GameController.manager.curFingerType = FingerPageType.Other;
            loginGo.SetActive(false);
            registerGo.SetActive(true);
        });

        SoundManager.manager.PlayMusicByPath("09000");

        GameController.manager.fingerDataList.Clear();
        GameController.manager.curFingerType = FingerPageType.Login;
        GameController.manager.fingerTemplate = new byte[0];

        identityDropdown.value = 1;
    }

    //private IEnumerator LoadMainGameObject(string path) {
    //    WWW www = new WWW(path);

    //    yield return www;
    //    Texture2D t = www.texture;
    //    img.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f)); 
    //}

    private bool CheckLogin() {
        return usernameInput.text.Trim() != "" && passwordInput.text.Trim() != "";
    }

    private static string GetAndroidExternalFilesDir() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                // Get all available external file directories (emulated and sdCards)
                AndroidJavaObject[] externalFilesDirectories = context.Call<AndroidJavaObject[]>("getExternalFilesDirs", null);
                AndroidJavaObject emulated = null;
                AndroidJavaObject sdCard = null;

                for (int i = 0; i < externalFilesDirectories.Length; i++) {
                    AndroidJavaObject directory = externalFilesDirectories[i];
                    using (AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment")) {
                        // Check which one is the emulated and which the sdCard.
                        bool isRemovable = environment.CallStatic <bool>("isExternalStorageRemovable", directory);
                        bool isEmulated = environment.CallStatic<bool>("isExternalStorageEmulated", directory);
                        if (isEmulated)
                            emulated = directory;
                        else if (isRemovable && isEmulated == false)
                            sdCard = directory;
                    }
                }
                // Return the sdCard if available
                if (sdCard != null)
                    return sdCard.Call<string>("getAbsolutePath");
                else
                    return emulated.Call<string>("getAbsolutePath");
            }
        }
    }

}
