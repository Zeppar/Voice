using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LevelInfo {
    public int index;
    public string name;
    public Sprite icon;
    public Sprite bg;
}

public class LevelManager {

    public List<LevelInfo> levelInfos = new List<LevelInfo>();
    public LevelInfo selectInfo = null;

    public void ParseLevelInfo() {
        TextAsset ta = Resources.Load<TextAsset>("Level/level");
        JsonData data = JsonMapper.ToObject(ta.text);
        for(int i = 0; i < data.Count; i ++) {
            var dt = data[i];
            LevelInfo info = new LevelInfo();
            info.index = (int)dt["index"];
            info.name = (string)dt["name"];
            try {
                info.icon = Resources.Load<Sprite>("Level/game" + info.index);
            } catch {
                info.icon = null;
            }
            try {
                info.bg = Resources.Load<Sprite>("Level/game" + info.index + "Bg");
            } catch {
                info.bg = null;
            }
            levelInfos.Add(info);
        }
    }
}
