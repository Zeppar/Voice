using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdentityType {
    Admin,
    User
}

public enum FingerPageType {
    Login = 0,
    Register,
    Other
}

public class GameController : MonoBehaviour {

    public static GameController manager = null;
    // common ui
    public InfoAlert infoAlert;
    public ScoreAlert scoreAlert;

    // manager
    public LevelManager levelMan = new LevelManager();
    public AccountManager accountMan = new AccountManager();
    public SubjectManager subjectMan = new SubjectManager();
    public MusicManager musicMan = new MusicManager();
    public ReportManager reportMan = new ReportManager();

    public bool isPaint = false;
    public int voiceType = 1;

    public List<string> fingerDataList = new List<string>();
    public string heartData = "";
    public string oxyData = "";
    public byte[] fingerTemplate;

    public FingerPageType curFingerType = FingerPageType.Login;
    public IdentityType curIdentityType;

    private void Awake() {
        if(manager == null) {
            manager = this;
            DontDestroyOnLoad(this);
        } else if(manager != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        levelMan.ParseLevelInfo();
        accountMan.ParseAllAccount();
        subjectMan.ParseSubjects();
        musicMan.ParseMusic();
        reportMan.ParseReport();
    }

    public void GetFingerData(string s) {
        if (curFingerType == FingerPageType.Other) {
            return;
        } else if (curFingerType == FingerPageType.Login) {
            // check user
            accountMan.ValidateFingerData(s);
        } else {
            fingerDataList.Add(s);
            if (fingerDataList.Count == 3) {
                AndroidJavaObject obj = new AndroidJavaObject("com.berry_med.bf.spo2_bluetooth.MainActivity");
                fingerTemplate = obj.Call<byte[]>("getTemplate", System.Text.Encoding.UTF8.GetBytes(fingerDataList[1]),
                    System.Text.Encoding.UTF8.GetBytes(fingerDataList[2]), System.Text.Encoding.UTF8.GetBytes(fingerDataList[3]));
                if (fingerTemplate.Length == 0)
                    fingerDataList.Clear();
            }
        }
    }

    public void GetOxiData(string s) {
        string[] arr = s.Split('|');
        oxyData = arr[0];
        heartData = arr[1];
    }

}
