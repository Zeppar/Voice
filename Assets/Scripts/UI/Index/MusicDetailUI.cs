using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicDetailUI : MonoBehaviour
{
    public Text nameText;
    public Text passTimeText;
    public Text remainTimeText;
    public Slider timeSlider;
    public Button prevMusicBtn;
    public Button nextMusicBtn;
    public Button playButton;
    public Sprite[] sps; // 0 for pause 1 for play
    public Button playTypeBtn;
    public Sprite[] playTypeSps;


    MusicTypeInfo typeInfo;
    MusicInfo info;

    private void Start() {
        prevMusicBtn.onClick.AddListener(() => {
            if (MusicUI.playType != PlayType.Random) {
                int idx = info.index - 2;
                SetContent(typeInfo.musicList[idx], typeInfo);
            } else {
                PlayRandomMusic();
            }
        });
        nextMusicBtn.onClick.AddListener(() => {
            if (MusicUI.playType != PlayType.Random) {
                int idx = info.index;
                SetContent(typeInfo.musicList[idx], typeInfo);
            } else {
                PlayRandomMusic();
            }
        });
        playButton.onClick.AddListener(() => {
            if(SoundManager.manager.IsPlaying) {
                SoundManager.manager.PauseMusic();
            } else {
                SoundManager.manager.PlayMusic(info.clip);
            }
        });
        playTypeBtn.onClick.AddListener(() => {
            MusicUI.playType = (PlayType)(((int)MusicUI.playType + 1) % 4);
        });
    }

    private void PlayRandomMusic() {
        int index = Random.Range(0, typeInfo.musicList.Count);
        SetContent(typeInfo.musicList[index], typeInfo);
    }

    public void SetContent(MusicInfo _info, MusicTypeInfo _typeInfo) {
        gameObject.SetActive(true);
        typeInfo = _typeInfo;
        info = _info;
        nameText.text = info.name;
        timeSlider.value = 0;
        passTimeText.text = Util.SecToTimeString(0);
        remainTimeText.text = Util.SecToTimeString(info.time);
        SoundManager.manager.PlayMusic(info.clip);
    }

    private void Update() {
        playTypeBtn.GetComponent<Image>().sprite = playTypeSps[(int)MusicUI.playType];
        playButton.GetComponent<Image>().sprite = SoundManager.manager.IsPlaying ? sps[0] : sps[1];
        prevMusicBtn.interactable = info.index != 1;
        nextMusicBtn.interactable = info.index != typeInfo.musicList.Count;
        timeSlider.value = SoundManager.manager.PassedTime / info.time;
        passTimeText.text = Util.SecToTimeString(SoundManager.manager.PassedTime);
        remainTimeText.text = Util.SecToTimeString(info.time - SoundManager.manager.PassedTime);

        if(Mathf.Approximately(timeSlider.value, 1.0f)) {
            PlayNextMusic();
        }
    }

    private void PlayNextMusic() {
        switch (MusicUI.playType) {
            case PlayType.Single:
                //do nothing
                break;
            case PlayType.SingleLoop:
                SetContent(info, typeInfo);
                break;
            case PlayType.Random:
                PlayRandomMusic();
                break;
            case PlayType.Order:
                if(nextMusicBtn.interactable) {
                    nextMusicBtn.onClick.Invoke();
                } else {
                    SetContent(typeInfo.musicList[0], typeInfo);
                }
                break;
            default:
                break;
        }
    }
}
