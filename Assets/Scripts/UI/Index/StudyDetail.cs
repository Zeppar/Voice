using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPOI.XWPF.Model;
using NPOI.XWPF.Extractor;
using NPOI.XWPF.UserModel;
using ICSharpCode.SharpZipLib.Zip;

using NPOI.OpenXmlFormats.Wordprocessing;
using System.IO;

public class StudyDetail : MonoBehaviour {
    public Text showText;

    StudyInfo info;

    public void SetContent(StudyInfo _info) {
        info = _info;
        gameObject.SetActive(true);
        if (_info.type == FileType.TXT) {
            StartCoroutine(LoadStudyFile(info.url));
        } else if (_info.type == FileType.WORD) {
            LoadWord(info.url);
        }
    }

    private IEnumerator LoadStudyFile(string filepath) {
        yield return new WaitForEndOfFrame();
        filepath = "file://" + filepath;
        WWW www = new WWW(filepath);
        yield return www;
        showText.text = www.text;
    }

    private void LoadWord(string path) {
        Debug.Log(path);
        if (!File.Exists(path))
            return;
        using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            XWPFDocument doc = new XWPFDocument(file);
            foreach (var para in doc.Paragraphs) {
                string text = para.GetText(); //获得文本
                if (text.Trim() != "")
                    Debug.Log(text);
            }
        }

    }
}
