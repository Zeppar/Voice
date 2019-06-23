using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SubjectInfo {
    public uint id;
    public string title;
    public string msg;
    public string type;
    public string descript;
    public List<QuestionInfo> questionInfos = new List<QuestionInfo>();
    public List<SubjectResult> questionResults = new List<SubjectResult>();

    public string GetResult(List<int> scoreList) {
        int totalScore = 0;
        string result = "";
        switch (id) {
            case 1:
            case 2:
            case 4:
            case 5: {
                    for (int i = 0; i < scoreList.Count; i++) {
                        totalScore += scoreList[i];
                    }
                    for (int i = 0; i < questionResults[0].levelInfoLists.Count; i++) {
                        var levelInfo = questionResults[0].levelInfoLists[i];
                        if (totalScore > levelInfo.condition) {
                            result = levelInfo.mark + "\n" + levelInfo.comment + "\n" + levelInfo.advice;
                            break;
                        }
                    }
                    if (result.Trim().Length == 0)
                        result = "测试结束";
                    return result;
                }
            case 3:
                for (int i = 0; i < questionResults.Count; i++) {
                    totalScore = 0;
                    for (int k = 0; k < questionResults[i].ids.Count; k++) {
                        totalScore += scoreList[(int)questionResults[i].ids[k] - 1];
                    }
                    for (int j = 0; j < questionResults[i].levelInfoLists.Count; j++) {
                        var levelInfo = questionResults[i].levelInfoLists[j];
                        if (totalScore > levelInfo.condition) {
                            result = levelInfo.mark + "\n" + levelInfo.comment + "\n" + levelInfo.advice;
                            break;
                        }
                    }
                }
                if (result.Trim().Length == 0)
                    result = "测试结束";
                return result;
            default:
                break;
        }
        return "测试结束";
    }
}

public class QuestionInfo {
    public string question;
    public List<string> options = new List<string>();
    public List<int> scores = new List<int>();
}

public class SubjectResult {
    public int id;
    public string name;
    public List<SubjectLevelInfo> levelInfoLists = new List<SubjectLevelInfo>();
    public List<uint> ids = new List<uint>();
}

public class SubjectLevelInfo {
    public int condition;
    public string mark;
    public string comment;
    public string advice;
}

public class SubjectManager  {

    public Dictionary<uint, SubjectInfo> subjectDict = new Dictionary<uint, SubjectInfo>();

    public void ParseSubjects() {
        TextAsset[] subjects = Resources.LoadAll<TextAsset>("Subjects");
        for(int i = 0; i < subjects.Length; i++) {
            JsonData data = JsonMapper.ToObject(subjects[i].text);
            SubjectInfo sb = new SubjectInfo();
            sb.id = (uint)data["id"];
            sb.title = (string)data["title"];
            sb.msg = (string)data["msg"];
            sb.type = (string)data["type"];
            sb.descript = (string)data["descript"];
            for(int j = 0;j < data["body"].Count; j++) {
                JsonData qes = data["body"][j];
                QuestionInfo qesInfo = new QuestionInfo();
                qesInfo.question = (string)qes["question"];
                for(int k = 0; k < qes["options"].Count; k++) {
                    qesInfo.options.Add((string)qes["options"][k]);
                }
                for (int k = 0; k < qes["scores"].Count; k++) {
                    qesInfo.scores.Add((int)qes["scores"][k]);
                }
                sb.questionInfos.Add(qesInfo);
            }
            JsonData r = data["result"];
            for (int j = 0; j < r.Count; j++) {
                JsonData result = r[j];
                SubjectResult sr = new SubjectResult();
                sr.id = (int)result["id"];
                sr.name = (string)result["name"];
                for (int k = 0; k < result["level"].Count; k++) {
                    SubjectLevelInfo levelInfo = new SubjectLevelInfo();
                    var level = result["level"][k];
                    levelInfo.condition = (int)level["condition"];
                    levelInfo.mark = (string)level["mark"];
                    levelInfo.comment = (string)level["comment"];
                    levelInfo.advice = (string)level["advice"];
                    sr.levelInfoLists.Add(levelInfo);
                }
                try {
                    sr.ids.Clear();
                    JsonData ids = result["idx"];
                    for (int k = 0; k < ids.Count; k++) {
                        sr.ids.Add((uint)ids[k]);
                    }
                } catch {
                    sr.ids = new List<uint>();
                }

                sb.questionResults.Add(sr);
            }
            subjectDict.Add(sb.id, sb);
        }
    }

}
