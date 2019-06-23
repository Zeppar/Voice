using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoAlert : MonoBehaviour {

    public Button confirmBtn;
    public Text showText;

    private Util.NoneParamCallBack callBack;

    private void Start() {
        confirmBtn.onClick.AddListener(() => {
            if (callBack != null)
                callBack();
            gameObject.SetActive(false);
        });
    }

    public void ShowWithText(string t, Util.NoneParamCallBack _callBack = null) {
        callBack = _callBack;
        showText.text = t;
        gameObject.SetActive(true);
    }

}