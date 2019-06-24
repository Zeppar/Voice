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

    public static Color ColorFromString(string s) {
        // string must be #xxxxxxxx
        int R = int.Parse(s.Substring(1, 2),
            System.Globalization.NumberStyles.HexNumber);
        int G = int.Parse(s.Substring(3, 2),
            System.Globalization.NumberStyles.HexNumber);
        int B = int.Parse(s.Substring(5, 2),
            System.Globalization.NumberStyles.HexNumber);
        try {
            int A = int.Parse(s.Substring(7, 2),
                System.Globalization.NumberStyles.HexNumber);
            return new Color(R / 255f, G / 255f, B / 255f, A / 255f);
        } catch {
            return new Color(R / 255f, G / 255f, B / 255f, 1f);
        }
    }

}
