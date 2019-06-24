using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using LitJson;
using System.Text;

public class ReportInfo {
    public string name;
    public int type; // 0 for pressure 1 for subject 2 for paint
    public string result;
    public string username;
}

public class ReportManager {

    public Dictionary<int, List<ReportInfo>> reportDict = new Dictionary<int, List<ReportInfo>>();

    public void ParseReport() {
        string wwwpath = "";
        string filePath = "";
        string dirPath = "";
#if UNITY_EDITOR
        wwwpath = "file://" + Application.streamingAssetsPath + "/Report/report.json";
        filePath = Application.streamingAssetsPath + "/Report/report.json";
        dirPath = Application.streamingAssetsPath + "/Report";
#else
        wwwpath = "file://" + Application.persistentDataPath + "/Report/report.json";
        filePath = Application.persistentDataPath + "/Report/report.json";
        dirPath = Application.persistentDataPath + "/Report";
#endif
        reportDict.Clear();
        if (!File.Exists(filePath)) {
            Directory.CreateDirectory(dirPath);
            File.Create(filePath);
            Debug.Log("Create : " + filePath);
            return;
        }
        GameController.manager.StartCoroutine(LoadReport(wwwpath));
    }

    public List<ReportInfo> GetInfos(int type, string username) {
        List<ReportInfo> l = new List<ReportInfo>();
        for (int i = 0; i < GameController.manager.reportMan.reportDict[type].Count; i++) {
            if (GameController.manager.reportMan.reportDict[type][i].username == username.Trim())
                l.Add(GameController.manager.reportMan.reportDict[type][i]);
        }
        return l;
    }

    IEnumerator LoadReport(string path) {
        //www load streaming folder need add 'file://' at the begining of the url
        WWW www = new WWW(path);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Trim() == "")
            yield break;
        JsonData data = JsonMapper.ToObject(www.text);
        for (int i = 0; i < data.Count; i++) {
            JsonData dt = data[i];
            ReportInfo info = new ReportInfo();
            info.name = (string)dt["name"];
            info.type = (int)dt["type"];
            info.result = (string)dt["result"];
            try {
                info.username = (string)dt["username"];
            } catch {
                info.username = "shabi";
            }
            if (!reportDict.ContainsKey(info.type))
                reportDict[info.type] = new List<ReportInfo>();
            reportDict[info.type].Add(info);
        }
    }

    public void DeleteReport(ReportInfo info) {
        if(reportDict[info.type].Contains(info)) {
            reportDict[info.type].Remove(info);
            SaveToFile();
        } 
    }

    public void AddReport(ReportInfo info) {
        if (!reportDict.ContainsKey(info.type))
            reportDict[info.type] = new List<ReportInfo>();
        if (!reportDict[info.type].Contains(info)) {
            reportDict[info.type].Add(info);
            SaveToFile();
        }
    }

    private void SaveToFile() {
        string path = "";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "/Report/report.json";
#else
        path = Application.persistentDataPath + "/Report/report.json";
#endif
        FileStream fs = new FileStream(path, FileMode.Create);
        StringBuilder sb = new StringBuilder();
        JsonWriter w = new JsonWriter(sb);
        w.WriteArrayStart();
        foreach (var k in reportDict.Keys) {
            List<ReportInfo> list = reportDict[k];
            for(int i = 0; i < list.Count; i++) {
                w.WriteObjectStart();
                w.WritePropertyName("name");
                w.Write(list[i].name);
                w.WritePropertyName("type");
                w.Write(list[i].type);
                w.WritePropertyName("result");
                w.Write(list[i].result);
                w.WritePropertyName("username");
                w.Write(list[i].username);
                w.WriteObjectEnd();
            }
        }
        w.WriteArrayEnd();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

}
