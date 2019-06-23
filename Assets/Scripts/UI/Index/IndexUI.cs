using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IndexUI : MonoBehaviour {
    enum IndexPageType {
        Main = 0,
        Setting,
        Level,
        Voice,
        Avatar,
        Question
    }
    [Header("Common")]
    public Button questionBtn;
    public Button settingBtn;
    public Button backBtn;

    [Header("Main")]
    public Button voiceBtn;
    public Button studyBtn;
    public Button musicBtn;
    public Button subjectBtn;
    public Button paintBtn;
    public Button reportBtn;
    public Button breatheBtn;
    public Button avatarBtn;

    [Header("Level")]
    public Transform levelItemParent;
    public LevelItem levelItemPrefab;

    [Header("Voice")]
    public List<Button> voiceBtns;

    [Header("Pages")]
    public GameObject[] pages;

    [Header("Avatar")]
    public Text nameText;
    public Transform avatarBtnParent;
    public AvatarBtn avatarBtnPrefab;
    public static AvatarBtn selectAvatar;
    AvatarBtn lastAvatar = null;

    private IndexPageType curPageType = IndexPageType.Main;


    private void Start() {
        questionBtn.onClick.AddListener(() => {
            SetPageType(IndexPageType.Question);
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex + "9001");
        });
        settingBtn.onClick.AddListener(() => {
            SetPageType(IndexPageType.Setting);
        });

        avatarBtn.onClick.AddListener(() => {
            SetPageType(IndexPageType.Avatar);
        });
        voiceBtn.onClick.AddListener(() => {
            SetPageType(IndexPageType.Voice);
            Debug.Log(GameController.manager.accountMan.selfInfo.sex + "=======");
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex + "9002");
        });
        studyBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Study);
        });
        musicBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Music);
        });
        subjectBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Subject);
        });
        paintBtn.onClick.AddListener(() => {
            GameController.manager.isPaint = true;
            SceneManager.LoadScene(2);
        });
        reportBtn.onClick.AddListener(() => {
            IndexUICtrl.instance.SetPageType(PageType.Report);
        });
        breatheBtn.onClick.AddListener(() => {
            GameController.manager.levelMan.selectInfo = new LevelInfo();
            GameController.manager.levelMan.selectInfo.index = 2;
            GameController.manager.levelMan.selectInfo.bg = Resources.Load<Sprite>("Level/game2Bg");
            SceneManager.LoadScene(2);
        });

        backBtn.onClick.AddListener(() => {
            if (curPageType == IndexPageType.Setting) {
                SettingUI setting = pages[(int)IndexPageType.Setting].GetComponent<SettingUI>();
                if(setting.PageIdx == 0) {
                    SetPageType(IndexPageType.Main);
                    GameController.manager.accountMan.UpdateAccount(GameController.manager.accountMan.selfInfo);
                } else {
                    setting.ConfirmPassword();
                }
            } else {
                SetPageType(IndexPageType.Main);
            }
        });

        for(int i = 0; i< voiceBtns.Count; i++) {
            int id = i;
            voiceBtns[i].onClick.AddListener(() => {
                GameController.manager.voiceType = id + 1;
                SetPageType(IndexPageType.Level);
                SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex + "9003");
            });
        }


        SetPageType(IndexPageType.Main);

        InitSelectGame();
        InitAvatar();

    }

    void InitSelectGame() {
        Util.DeleteChildren(levelItemParent);
        for (int i = 0; i < GameController.manager.levelMan.levelInfos.Count; i++) {
            LevelItem item = Instantiate(levelItemPrefab) as LevelItem;
            item.SetContent(GameController.manager.levelMan.levelInfos[i]);
            item.transform.SetParent(levelItemParent, false);
        }
    }

    void InitAvatar() {
        nameText.text = GameController.manager.accountMan.selfInfo.name;
        Util.DeleteChildren(avatarBtnParent);
        for (int i = 0; i < Util.avatarCount; i++) {
            AvatarBtn item = Instantiate(avatarBtnPrefab) as AvatarBtn;
            item.SetContent(i + 1, () => {
                SetPageType(IndexPageType.Main);
            });
            item.transform.SetParent(avatarBtnParent, false);
        }
        avatarBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avatar/avatar" + GameController.manager.accountMan.selfInfo.avatar);

    }

   

    void SetPageType(IndexPageType pageType) {
        SoundManager.manager.StopMusic();
        curPageType = pageType;
        for(int i = 0; i < pages.Length; i++) {
            pages[i].SetActive(false);
        }
        pages[(int)pageType].SetActive(true);
        settingBtn.gameObject.SetActive(pageType == IndexPageType.Main);
        questionBtn.gameObject.SetActive(pageType == IndexPageType.Main);
        backBtn.gameObject.SetActive(pageType != IndexPageType.Main);
    }

    private void Update() {
        if(lastAvatar != selectAvatar) {
            lastAvatar = selectAvatar;
            avatarBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avatar/avatar" + selectAvatar.avatarIdx);
        }
    }
}
