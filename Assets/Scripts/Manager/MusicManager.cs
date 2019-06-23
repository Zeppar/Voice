using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class MusicInfo {
    public int index;
    public string name;
    public float time;
    public AudioClip clip;
}

public class MusicTypeInfo {
    public string name;
    public string type;
    public string url;
    public List<MusicInfo> musicList = new List<MusicInfo>();
}

public class MusicManager {

    public List<MusicTypeInfo> typeList = new List<MusicTypeInfo>();

    public void ParseMusic() {
        var t = Resources.Load<TextAsset>("Music/music");
        JsonData data = JsonMapper.ToObject(t.text);
        for(int i = 0; i < data.Count; i++) {
            MusicTypeInfo info = new MusicTypeInfo();
            info.type = (string)data[i]["type"];
            info.url = (string)data[i]["url"];
            info.name = (string)data[i]["name"];
            typeList.Add(info);
        }
    }

}
