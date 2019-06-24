using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class StudyUI : MonoBehaviour {
    public Button backBtn;
    public Transform parent;
    public StudyItem itemPrefab;
    public StudyDetail studyDetail;
    public GameObject foreBg;

    public Button importBtn;

    public static StudyItem selectItem = null;
    private StudyItem lastItem = null;

    private void Start() {
        backBtn.onClick.AddListener(() => {
            if (studyDetail.gameObject.activeInHierarchy) {
                studyDetail.gameObject.SetActive(false);
                foreBg.SetActive(true);
            } else {
                IndexUICtrl.instance.SetPageType(PageType.Index);
            }
        });

        importBtn.onClick.AddListener(() => {
            AndroidJavaObject obj = new AndroidJavaObject("com.berry_med.bf.spo2_bluetooth.MainActivity");
            obj.Call<int>("importFile", Application.persistentDataPath + "/Study");
        });
        InitFile();
        importBtn.gameObject.SetActive(GameController.manager.curIdentityType == IdentityType.Admin);
    }

    private void Update() {
        if(lastItem != selectItem) {
            foreBg.SetActive(false);
            studyDetail.SetContent(selectItem.info);
            lastItem = selectItem;
        }

    }

    int index = 1;
    private void InitFile() {
        Util.DeleteChildren(parent);
        string path = "";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "/Study";
#else
        path = Application.persistentDataPath + "/Study";
#endif
        path = Application.streamingAssetsPath + "/Study";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles(".", SearchOption.AllDirectories);
        index = 1;
        for (int i = 0; i < files.Length; i++) {
            if (files[i].Name.EndsWith(".txt")/* || files[i].Name.EndsWith(".pdf") || files[i].Name.EndsWith(".word")*/) {
                Debug.Log(files[i].Name);
                StudyInfo info = new StudyInfo();
                info.name = files[i].Name;
                info.url = files[i].FullName;
                info.type = FileType.TXT;
                StudyItem item = Instantiate(itemPrefab);
                item.SetContent(info);
                item.transform.SetParent(parent, false);
            }
        }

    }

   
}
