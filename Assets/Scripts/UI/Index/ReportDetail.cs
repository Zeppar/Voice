using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ReportDetail : MonoBehaviour {
    public Text showText;
    public Image showImage;
    public ScrollRect scrollview;

    public void ShowDetail(ReportInfo info) {
        gameObject.SetActive(true);
        scrollview.gameObject.SetActive(info.type != 2);
        showImage.gameObject.SetActive(info.type == 2);
        if (info.type != 2) {
            showText.text = info.result;
        } else {
            // get image by url
            string path = "";
#if UNITY_EDITOR
            path = Application.streamingAssetsPath + "/Paint/" + info.result;
#else
            path = Application.persistentDataPath + "/Paint/" + info.result;
#endif
            StartCoroutine(LoadImage(path));
        }
    }

    private IEnumerator LoadImage(string filepath) {
        filepath = "file://" + filepath;
        var www = new WWW(filepath);
        yield return www;
        Texture2D t = www.texture;
        showImage.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }
}
