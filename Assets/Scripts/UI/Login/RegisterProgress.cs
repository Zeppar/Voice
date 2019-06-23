using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterProgress : MonoBehaviour {
    public List<GameObject> gos;

    public void SetProgress(int index) {
        int changeColorCount = 0;
        switch (index) {
            case 0:
                changeColorCount = 2;
                break;
            case 1:
                changeColorCount = 4;
                break;
            case 2:
                changeColorCount = 5;
                break;
            default:
                break;
        }
        for (int i = 0; i < changeColorCount; i++) {
            gos[i].GetComponent<Image>().color = new Color(25 / 255.0f, 164 / 255.0f, 231 / 255.0f, 1.0f);
        }
    }
}
