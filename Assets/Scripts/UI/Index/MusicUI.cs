using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public enum PlayType {
    Single = 0,
    SingleLoop,
    Random,
    Order
}

public class MusicUI : MonoBehaviour
{
    public Button backBtn;
    public static MusicTypeItem selectedTypeItem = null;
    public static MusicItem selectedItem = null;
    MusicTypeItem lastTypeItem = null;
    MusicItem lastMusicItem = null;

    public MusicTypeItem typeItemPrefab;
    public MusicItem itemPrefab;
    public Transform typeParent;
    public Transform musicParent;
    public GameObject foreBg;

    public Text typeNameText;
    public Button playTypeBtn;
    public Image playTypeImage;
    public Sprite[] playTypeSps;
    List<string> playTypeNames = new List<string>() {"单曲播放","单曲循环","随机播放","顺序播放"};
    public static PlayType playType = PlayType.Single;

    public MusicDetailUI musicDetailUI;

    private void Start() {
        Directory.CreateDirectory(Application.persistentDataPath + "/Success");
        File.Create(Application.persistentDataPath + "/Success.txt");
        backBtn.onClick.AddListener(() => {
            if (musicDetailUI.gameObject.activeInHierarchy) {
                foreBg.SetActive(true);
                musicDetailUI.gameObject.SetActive(false);
                lastMusicItem = null;
                selectedItem = null;
                SoundManager.manager.StopMusic();
            } else {
                IndexUICtrl.instance.SetPageType(PageType.Index);
                selectedTypeItem = null;
                lastTypeItem = null;
            }
        });
        playTypeBtn.onClick.AddListener(() => {
            playType = (PlayType)(((int)playType + 1) % 4);
        });
        InitMusic();
    }

    private void InitMusic() {
        for(int i = 0;i < GameController.manager.musicMan.typeList.Count; i ++) {
            string path = "";
    #if UNITY_EDITOR
            path = Application.streamingAssetsPath + GameController.manager.musicMan.typeList[i].url;
#else
            path = Application.persistentDataPath + GameController.manager.musicMan.typeList[i].url;
#endif
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            MusicTypeItem typeItem = Instantiate(typeItemPrefab) as MusicTypeItem;
            typeItem.SetContent(GameController.manager.musicMan.typeList[i]);
            typeItem.transform.SetParent(typeParent, false);
            selectedTypeItem = typeItem;
        }
    }

    private void Update() {
        if(lastTypeItem != selectedTypeItem) {
            ShowMusicList(selectedTypeItem.info);
            lastTypeItem = selectedTypeItem;
        }

        if(lastMusicItem != selectedItem) {
            foreBg.SetActive(false);
            musicDetailUI.SetContent(selectedItem.info, selectedTypeItem.info);
            lastMusicItem = selectedItem;
        }

        playTypeImage.sprite = playTypeSps[(int)playType];
        playTypeBtn.GetComponentInChildren<Text>().text = playTypeNames[(int)playType];
    }

    int index = 1;
    private void ShowMusicList(MusicTypeInfo info) {
        Util.DeleteChildren(musicParent);
        string path = "";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + info.url;
#else
        path = Application.persistentDataPath + info.url;
#endif
        Debug.Log(path);
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*.mp3", SearchOption.AllDirectories);
        info.musicList.Clear();
        index = 1;

        int endCount = files.Length;
        for (int i = 0; i < files.Length; i++) {
            FileInfo f = files[i];
            StartCoroutine(LoadMusic(f.FullName, info, f.Name, endCount));
        }
        typeNameText.text = info.name + "(" + info.musicList.Count + ")";

    }

    private IEnumerator LoadMusic(string filepath, MusicTypeInfo typeInfo, string musicName, int endIndex) {
        filepath = "file://" + filepath;
        using (var www = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.MPEG)) {
            yield return www.SendWebRequest();
            if (www.isNetworkError) {
                Debug.LogError(www.error);
            } else {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                MusicInfo info = new MusicInfo();
                info.clip = clip;
                info.name = musicName;
                info.index = index;
                info.time = clip.length;
                typeInfo.musicList.Add(info);
                if(index == endIndex) {
                    ShowMusicListItem(typeInfo);
                }
                index += 1;
            }
        }
    }

    private void ShowMusicListItem(MusicTypeInfo info) {
        for(int i = 0; i < info.musicList.Count; i++) {
            MusicItem item = Instantiate(itemPrefab);
            item.SetContent(info.musicList[i]);
            item.transform.SetParent(musicParent, false);
        }
        typeNameText.text = info.name + "(" + info.musicList.Count + ")";
    }
}
