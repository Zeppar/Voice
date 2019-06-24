using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class FreePaintUI : MonoBehaviour {

    public Button deleteBtn;
    public Button copyBtn;
    public Button saveBtn;
    public Button pencilBtn;
    public Button eraserBtn;
    public ColorItem colorPrefab;
    public Transform colorParent;
    public Slider sizeSlider;
    public static ColorItem selectItem = null;
    public static ColorItem lastItem = null;

    public Image copyImg;

    private void Start() {
        deleteBtn.onClick.AddListener(() => {
            GameUI.instance.ClearAllLine();
        });

        copyBtn.onClick.AddListener(() => {
            copyImg.gameObject.SetActive(!copyImg.gameObject.activeInHierarchy);
        });

        saveBtn.onClick.AddListener(() => {
            SavePhoto();
        });

        pencilBtn.onClick.AddListener(() => {
            GameUI.instance.paintInfo.color = selectItem.color;
            colorParent.gameObject.SetActive(true);
        });

        eraserBtn.onClick.AddListener(() => {
            GameUI.instance.paintInfo.color = Color.white;
            colorParent.gameObject.SetActive(false);
        });

        sizeSlider.onValueChanged.AddListener((float value) => {
            GameUI.instance.paintInfo.width = value;
        });
        sizeSlider.value = GameUI.instance.paintInfo.width;


        Util.DeleteChildren(colorParent);
        List<Color> colors = new List<Color>() { Util.ColorFromString("#ea4631"),
                                                Util.ColorFromString("#ffb820"),
                                                Util.ColorFromString("#4aed5a"),
                                                Util.ColorFromString("#32a3ea"),
                                                Util.ColorFromString("#8657ff"),
                                                Util.ColorFromString("#3a48ff"),
                                                Util.ColorFromString("#32e4ea"),
                                                Util.ColorFromString("#ea32a3"),
                                                Util.ColorFromString("#000000"),
                                                Util.ColorFromString("#ffffff")};
        for(int i = 0; i < colors.Count; i++) {
            ColorItem item = Instantiate(colorPrefab);
            item.SetContent(colors[i]);
            item.transform.SetParent(colorParent, false);
            if (i == 8)
                selectItem = item;

        }
    }

    private void Update() {
        if(lastItem != selectItem) {
            Debug.Log("ChangeColor " + selectItem.color.ToString());
            GameUI.instance.paintInfo.color = selectItem.color;
            lastItem = selectItem;
        }
    }

    private void SavePhoto() {
        DateTime time = DateTime.Now;
        ReportInfo info = new ReportInfo();
        info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
        + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
        info.type = 2;
        info.result = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
        + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00") + ".png";
        info.username = GameController.manager.accountMan.selfInfo.username;
        GameController.manager.reportMan.AddReport(info);
        string path = "";
        string filePath = "";
#if UNITY_EDITOR

        path = Application.streamingAssetsPath + "/Paint/" + info.result;
        filePath = Application.streamingAssetsPath + "/Paint";
        if(!Directory.Exists(filePath)) {
            Directory.CreateDirectory(filePath);
        }
#else
        path = Application.persistentDataPath + "/Paint/" + info.result;
        filePath = Application.persistentDataPath + "/Paint";
        if(!Directory.Exists(filePath)) {
            Directory.CreateDirectory(filePath);
        }
#endif
        StartCoroutine(CaptureScreenshot(path));
    }

    private IEnumerator CaptureScreenshot(string path) {
        Debug.Log(path);
        GetComponent<CanvasGroup>().alpha = 0;
        GameUI.instance.backBtn.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        #if UNITY_EDITOR
        ScreenCapture.CaptureScreenshot(path);
        #else
        yield return StartCoroutine(SaveScreenShotToAndroid(path));
        #endif
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GetComponent<CanvasGroup>().alpha = 1.0f;
        GameUI.instance.backBtn.gameObject.SetActive(true);
    }

    IEnumerator SaveScreenShotToAndroid(string path) {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        yield return new WaitForEndOfFrame();
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();
        yield return tex;
        byte[] bytes = tex.EncodeToPNG();
        Debug.Log(path + "!!!!!");
        File.WriteAllBytes(path, bytes);
    }



}
