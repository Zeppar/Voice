using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterProgress : MonoBehaviour {
    public Sprite[] sps;
    public Image selfImage;

    public void SetProgress(int index) {
        switch (index) {
            case 0:
                selfImage.sprite = sps[0];
                break;
            case 1:
                selfImage.sprite = sps[1];
                break;
            case 2:
                selfImage.sprite = sps[2];
                break;
            default:
                break;
        }
    }
}
