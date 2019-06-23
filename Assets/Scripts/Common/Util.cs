using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Util {

    public delegate void NoneParamCallBack();
    public static int avatarCount = 6;

    public static void DeleteChildren(Transform t) {
        List<GameObject> gos = new List<GameObject>();
        for (int i = 0; i < t.childCount; i++) {
            gos.Add(t.GetChild(i).gameObject);
        }
        gos.ForEach((obj) => {
            UnityEngine.Object.Destroy(obj);
        });
    }

    public static string SecToTimeString(float sec) {
        TimeSpan ts = new TimeSpan((long)(sec * 10000000));
        return ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
    }

}
