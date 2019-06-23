using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PaintInfo {
    public Color color;
    public float width;

    public PaintInfo() {
        color = Color.black;
        width = 0.2f;
    }
}

public class GameUI : MonoBehaviour {

    public static GameUI instance = null;

    public Image backgroundImg;
    public Image voiceSlider;
    public GameObject sliderBg;
    public Button backBtn;
    public GameObject bottomGo;

    [Header("Game")]
    public Image ballImg;
    public Image windMillImg;
    public Sprite game6SucSp;
    public Sprite game6NorSp;
    public FallSpawner fallSpawner;
    public Image paintImg;

    private AudioClip micRecord;
    private string device;
    private float volumeParam = 0;
    private bool gameEnd = false;

    public GameObject linePrefab;
    public Transform lineParent;
    private LineRenderer line;
    private LineRenderer line2;
    private int touch1Idx;
    private int touch2Idx;
    public Camera paintCamera;
    public RawImage paintCameraImg;

    public GameObject paintGo;
    public Button paintExitBtn;
    public Button paintFinishBtn;
    public Text paintTimeText;
    public FreePaintUI freePaintUI;
    public List<Sprite> game2Sps;

    public GameObject breatheGo;
    public Image lightImg;
    public Image boliImg;

    public Text heartText;
    public Text oxyText;

    private int sucTime = 0;
    private float timer = 0;
    private int lineIndex = 0;
    private bool startVoice = false;
    private float maxVolume = -1.0f;
    private bool stopGame = false;
    private float noVoiceTimer = 0;
    private int noVoiceTimes = 0;
    private float showScoreTimer = 0;
    private readonly List<string> resultList = new List<string> { "有时候最简单的渴望，却成了最遥不可及的奢求。生命中，总有些人，安然而来，静静守候，不离不弃；也有些人，浓烈如酒，疯狂似醉，却是醒来无处觅，来去都如风。缘深缘浅，如此这般：无数的相遇，无数的别离，伤感良多，或许不舍，或许期待，或许无奈，终得悟，不如守拙以清心，淡然而浅笑。",
     "生活最沉重的负担不是工作，而是无聊。心境。一切只在于自己的心境，无关其他。",
     "有人说宿命，因为往往开始便应了结局，有人说伤感，因为这颗心总是不肯静静沉默于故事中，有人说勇敢，因为品到苦楚的方是爱情，有人说惆怅，因为口在说结束时心却无法止步。人生失意的时候容易失态，于是消极和绝望就会趁隙而入。笑看人生潮起潮落，守住自己的心。",
     "有时我们的选择，只有等待，没有结果，只能黯然离开；有时我们的放弃，迫于无奈，含泪转身，却心有不甘。所以，有些曾经，关于幸福或苦痛，只能深埋心底；有些希冀，关于现在或将来，只能逐步遗忘。爱情里，分手和牵手的随时都在变换着主角，来来往往，离离散散，这就是爱情。",
     "年少时，岁月偷走我们的懵懂；青春时，岁月偷走我们的爱慕；中年时，岁月偷走我们的激情；老暮时，岁月偷走我们的一切：它让美丽失色，让爱情黯然，让智慧隐匿，让灵魂飘逸。我们要做的，和岁月较量，和时间奔跑，在卑微中活出精彩，在短暂中寻求永恒。",
     "人一辈子，没有人能打败你，只有你自己打败你自己。靠山山会倒，靠人人会跑，只有自己最可靠。没有人陪你走一辈子，所以你要适应孤独，没有人会帮你一辈子，所以你要奋斗一生。" };

    public PaintInfo paintInfo = new PaintInfo();

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {

        backBtn.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });


        if (!GameController.manager.isPaint) {
            if (GameController.manager.levelMan.selectInfo.index != 1) {
                backgroundImg.sprite = GameController.manager.levelMan.selectInfo.bg;
            } else {
                backgroundImg.sprite = game2Sps[0];
            }
            InitGame();
            device = Microphone.devices[0];
            micRecord = Microphone.Start(device, true, 999, 44100);
        } else {
            InitPaint();
        }
    }

    private void InitPaint() {
        paintCameraImg.gameObject.SetActive(true);
        freePaintUI.gameObject.SetActive(true);
        bottomGo.SetActive(false);
        sliderBg.SetActive(false);
    }

    private void InitGame() {
        timer = 0;
        sucTime = 0;
        switch (GameController.manager.levelMan.selectInfo.index) {
            case 0:
                ballImg.gameObject.SetActive(true);
                break;
            case 1:
                break;
            case 2:
                sliderBg.SetActive(false);
                breatheGo.SetActive(true);
                break;
            case 3:
                windMillImg.gameObject.SetActive(true);
                break;
            case 4:
                fallSpawner.gameObject.SetActive(true);
                break;
            case 5:
                sliderBg.SetActive(false);
                paintCameraImg.gameObject.SetActive(true);
                paintImg.sprite = Resources.Load<Sprite>("Level/Game6Sp/sp" + UnityEngine.Random.Range(0, 21));
                paintGo.SetActive(true);
                paintFinishBtn.onClick.AddListener(() => {
                    int score = UnityEngine.Random.Range(0, 100);
                    string r = resultList[UnityEngine.Random.Range(0, 6)];
                    // save directly
                    ReportInfo info = new ReportInfo();
                    DateTime time = DateTime.Now;
                    info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
                    + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
                    info.type = 0;
                    info.result = info.name + "\n" + GameController.manager.accountMan.selfInfo.name + "\n" + GameController.manager.levelMan.selectInfo.name + "\n" + r;
                    GameController.manager.reportMan.AddReport(info);
                    SceneManager.LoadScene(1);
                });
                paintExitBtn.onClick.AddListener(() => {
                    SceneManager.LoadScene(1);
                });
                break;
            default:
                break;
        }
    }

    float value = 0;
    private void Update() {

        oxyText.text = GameController.manager.oxyData;
        heartText.text = GameController.manager.heartData;
        if (gameEnd)
            return;


#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.Space)) {
            value += Time.deltaTime * 0.2f;
        } else {
            //value -= Time.deltaTime * 0.2f;
            //value = Mathf.Clamp01(value);
            value = 0;
        }
#else
        value = GetSliderValue(GetCurrentVolume());
#endif
        voiceSlider.fillAmount = Mathf.Lerp(voiceSlider.fillAmount, value, 0.1f);

        timer += Time.deltaTime;

        if (!GameController.manager.isPaint && GameController.manager.levelMan.selectInfo.index != 5 && timer >= 600.0f) {
            SoundManager.manager.PlayMusicByPath("09006");
            string r = resultList[UnityEngine.Random.Range(0, 6)];
            GameController.manager.infoAlert.ShowWithText(r, () => {
                // save TODO
                ReportInfo info = new ReportInfo();
                DateTime time = DateTime.Now;
                info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
                + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
                info.type = 0;
                info.result = info.name + "\n" + GameController.manager.accountMan.selfInfo.name + "\n" + GameController.manager.levelMan.selectInfo.name + "\n" + r; ;
                GameController.manager.reportMan.AddReport(info);
                SceneManager.LoadScene(1);
            });
            gameEnd = true;
            return;
        }

        if (!GameController.manager.isPaint) {
            switch (GameController.manager.levelMan.selectInfo.index) {
                case 0:
                    volumeParam = Mathf.Lerp(volumeParam, value, 0.02f);
                    float scale = 1.0f + Mathf.Clamp01(volumeParam) * 3;
                    ballImg.transform.localScale = new Vector3(scale, scale, 1.0f);
                    break;
                case 1:
                    volumeParam = Mathf.Lerp(volumeParam, value, 0.02f);
                    int index = (int)Mathf.Clamp(volumeParam * 10, 0, 3);
                    backgroundImg.sprite = game2Sps[index];
                    break;
                case 2:
                    // no end
                    break;
                case 3:
                    volumeParam = Mathf.Lerp(volumeParam, value * 80, 0.02f);
                    windMillImg.transform.Rotate(new Vector3(0, 0, 1), volumeParam);
                    break;
                case 4:
                    fallSpawner.UpdateFallGo(value);
                    break;
                case 5:
                    paintTimeText.text = Util.SecToTimeString(timer);
                    break;
                case 6:
                    if (value > 0.6) {
                        backgroundImg.sprite = game6SucSp;
                    }
                    break;
                default:
                    break;
            }
            if(value > 0.1f) {
                startVoice = true;
                noVoiceTimer = 0;
                noVoiceTimes = 0;
                showScoreTimer = 0;
            } else if(value < 0.006){
                showScoreTimer += Time.deltaTime;
                if (showScoreTimer > 1.0f) {
                    if (startVoice) {
                        ShowScore(maxVolume);
                        if (maxVolume > 0.6f) {
                            sucTime += 1;
                        } else {
                            sucTime = 0;
                        }
                        maxVolume = -1.0f;
                        if (!GameController.manager.isPaint && GameController.manager.levelMan.selectInfo.index == 6) {
                            backgroundImg.sprite = game6NorSp;
                        }
                    }
                    startVoice = false;
                    showScoreTimer = 0;
                }


                if (GameController.manager.isPaint
                    || (!GameController.manager.isPaint && (GameController.manager.levelMan.selectInfo.index == 2 || GameController.manager.levelMan.selectInfo.index == 5))) {
                    // do nothing
                } else {
                    if (!stopGame)
                        noVoiceTimer += Time.deltaTime;
                    if (noVoiceTimer > 5.0f) {
                        noVoiceTimer = 0;
                        noVoiceTimes++;
                        stopGame = true;
                        StopCoroutine("ResetStopFlag");
                        StartCoroutine("ResetStopFlag");
                        if (noVoiceTimes == 3) {
                            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString()
                    + "9005");
                            gameEnd = true;
                            string r = resultList[UnityEngine.Random.Range(0, 6)];
                            GameController.manager.infoAlert.ShowWithText(r, () => {
                                // save TODO
                                ReportInfo info = new ReportInfo();
                                DateTime time = DateTime.Now;
                                info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
                                + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
                                info.type = 0;
                                info.result = info.name + "\n" + GameController.manager.accountMan.selfInfo.name + "\n" + GameController.manager.levelMan.selectInfo.name + "\n" + r; ;
                                GameController.manager.reportMan.AddReport(info);
                                SceneManager.LoadScene(1);
                            });
                            StopCoroutine("EndGameOperation");
                            StartCoroutine("EndGameOperation");
                            return;
                        } else {
                            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                    + "00" + UnityEngine.Random.Range(0, 6));
                        }
                    }
                }
            }
            maxVolume = Mathf.Max(maxVolume, value);

            if (sucTime >= 3) {
                gameEnd = true;
                voiceSlider.fillAmount = 0;
                ballImg.gameObject.SetActive(false);
                fallSpawner.gameObject.SetActive(false);
                // save TODO
                SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex + "9004");
                string r = resultList[UnityEngine.Random.Range(0, 6)];
                GameController.manager.infoAlert.ShowWithText(r, () => {
                    // save TODOpaintiii
                    ReportInfo info = new ReportInfo();
                    DateTime time = DateTime.Now;
                    info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
                    + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
                    info.type = 0;
                    info.result = info.name + "\n" + GameController.manager.accountMan.selfInfo.name + "\n" + GameController.manager.levelMan.selectInfo.name + "\n" + r; ;
                    GameController.manager.reportMan.AddReport(info);
                    SceneManager.LoadScene(1);
                });
            }
        }

        if ((!GameController.manager.isPaint && GameController.manager.levelMan.selectInfo.index == 5) || GameController.manager.isPaint) {
#if UNITY_EDITOR
            if (Input.mousePosition.y > 200.0f && Input.mousePosition.y < Screen.height - 100.0f) {
                if (Input.GetMouseButtonDown(0)) {

                    var go = Instantiate(linePrefab, linePrefab.transform.position, transform.rotation);
                    go.transform.SetParent(lineParent, false);
                    line = go.GetComponent<LineRenderer>();//获得该物体上的LineRender组件
                    line.startColor = paintInfo.color;
                    line.endColor = paintInfo.color;
                    line.startWidth = paintInfo.width;
                    line.endWidth = paintInfo.width;
                    line.sortingOrder = lineIndex;
                    lineIndex++;
                    touch1Idx = 0;
                }
                if (Input.GetMouseButton(0)) {

                    touch1Idx++;
                    line.positionCount = touch1Idx;//设置顶点数
                    line.SetPosition(touch1Idx - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));//设置顶点位置
                                                                                                                                           //line.enabled=false;
                }
            }
#else
            if (Input.touches.Length > 0) {
                    var touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Began) {
                        var go = Instantiate(linePrefab, linePrefab.transform.position, transform.rotation);
                        go.transform.SetParent(lineParent, false);
                        line = go.GetComponent<LineRenderer>();//获得该物体上的LineRender组件
                        line.startColor = paintInfo.color;
                        line.endColor = paintInfo.color;
                        line.startWidth = paintInfo.width;
                        line.endWidth = paintInfo.width;
                        line.sortingOrder = lineIndex;
                        lineIndex++;
                        touch1Idx = 0;
                    } else if (touch0.phase == TouchPhase.Moved) {
                        touch1Idx++;
                        line.positionCount = touch1Idx;//设置顶点数
                        line.SetPosition(touch1Idx - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 15)));//设置顶点位置

                    }
                if(Input.touches.Length > 1) {
                    var touch1 = Input.GetTouch(1);
                    if (touch1.phase == TouchPhase.Began) {
                        var go = Instantiate(linePrefab, linePrefab.transform.position, transform.rotation);
                        go.transform.SetParent(lineParent, false);
                        line2 = go.GetComponent<LineRenderer>();//获得该物体上的LineRender组件
                        line2.startColor = paintInfo.color;
                        line2.endColor = paintInfo.color;
                        line2.startWidth = paintInfo.width;
                        line2.endWidth = paintInfo.width;
                        line2.sortingOrder = lineIndex;
                        lineIndex++;
                        touch2Idx = 0;
                    } else if (touch1.phase == TouchPhase.Moved) {
                        touch2Idx++;
                        line2.positionCount = touch2Idx;//设置顶点数
                        line2.SetPosition(touch2Idx - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(1).position.x, Input.GetTouch(1).position.y, 15)));//设置顶点位置

                    }
                }
            }
#endif
        }
    }

    private void ShowScore(float score) {
        Debug.Log("ShowScore : " + score);
        if (score > 0.6f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "50" + UnityEngine.Random.Range(0,6));
            GameController.manager.scoreAlert.ShowWithLevel(5);
        } else if (score > 0.5f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "40" + UnityEngine.Random.Range(0, 6));
            GameController.manager.scoreAlert.ShowWithLevel(4);
        } else if (score > 0.4f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "30" + UnityEngine.Random.Range(0, 6));
            GameController.manager.scoreAlert.ShowWithLevel(3);
        } else if (score > 0.3f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "20" + UnityEngine.Random.Range(0, 6));
            GameController.manager.scoreAlert.ShowWithLevel(2);
        } else if (score > 0.2f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "10" + UnityEngine.Random.Range(0, 6));
            GameController.manager.scoreAlert.ShowWithLevel(1);
        } else if(score > 0.1f) {
            SoundManager.manager.PlayMusicByPath(GameController.manager.accountMan.selfInfo.sex.ToString() + GameController.manager.voiceType.ToString()
                + "00" + UnityEngine.Random.Range(0, 6));
            GameController.manager.scoreAlert.ShowWithLevel(0);
        }

        if(score > 0.1f) {
            stopGame = true;
            StopCoroutine("ResetStopFlag");
            StartCoroutine("ResetStopFlag");
            if(!GameController.manager.isPaint) {
                if (GameController.manager.levelMan.selectInfo.index == 1)
                    lightImg.gameObject.SetActive(true);
                else if (GameController.manager.levelMan.selectInfo.index == 6)
                    boliImg.gameObject.SetActive(true);
            }
        }

    }

    private IEnumerator ResetStopFlag() {
        voiceSlider.fillAmount = 0;
        yield return new WaitForSecondsRealtime(SoundManager.manager.Audio.clip.length + 1.0f);
        stopGame = false;
        lightImg.gameObject.SetActive(false);
        boliImg.gameObject.SetActive(false);
    }

     private float GetSliderValue(float volume) {
        if (stopGame)
            return 0;
        if (GameController.manager.isPaint)
            return 0;
        if (!GameController.manager.isPaint && (GameController.manager.levelMan.selectInfo.index == 2 || GameController.manager.levelMan.selectInfo.index == 5))
            return 0;
        if(volume > 0) {
            return volume * 4.0f;
        }
        return 0;
    }

    private float GetCurrentVolume() {
        float[] volumeData = new float[1];
        int offset = Microphone.GetPosition(device) - 1;
        if (offset < 0) {
            return 0;
        }
        micRecord.GetData(volumeData, offset);
        return volumeData[0];
    }

    public void ClearAllLine() {
        Util.DeleteChildren(lineParent);
        lineIndex = 0;
    }

    private IEnumerator EndGameOperation() {
        yield return new WaitForSecondsRealtime(5.0f);
        GameController.manager.infoAlert.confirmBtn.onClick.Invoke();
    }


}
