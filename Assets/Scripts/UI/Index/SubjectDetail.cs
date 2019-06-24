using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SubjectDetail : MonoBehaviour {

    public Text questionText;
    public List<Toggle> toggleLists;
    public Text resultText;
    public ScrollRect scrollView;
    public Button preQuestionBtn;

    SubjectInfo subjectInfo;
    int questionIdx;
    List<int> scoreList = new List<int>();

    private void Start() {
        for (int i = 0; i < toggleLists.Count; i++) {
            int temp = i;
            toggleLists[i].onValueChanged.AddListener((bool isOn) => {
                if (isOn)
                    ShowNextQuestion(temp);
            });
        }

        preQuestionBtn.onClick.AddListener(() => {
            ShowPrevQuestion();
        });
    }

    private void ShowPrevQuestion() {
        scoreList.Remove(scoreList.Count - 1);
        ShowSubject(subjectInfo, questionIdx - 1);
    }

    private void ShowNextQuestion(int answerIdx) {
        scoreList.Add(subjectInfo.questionInfos[questionIdx].scores[answerIdx]);
        if (questionIdx < subjectInfo.questionInfos.Count - 1) {
            ShowSubject(subjectInfo, questionIdx + 1);
        } else {
            ShowResult();
        }
    }


    public void ShowSubject(SubjectInfo _subjectInfo, int _questionIdx) {

        subjectInfo = _subjectInfo;
        questionIdx = _questionIdx;
        questionText.text = subjectInfo.questionInfos[questionIdx].question;
        scrollView.gameObject.SetActive(false);
        preQuestionBtn.gameObject.SetActive(true);
        questionText.gameObject.SetActive(true);
        for (int i = 0; i < toggleLists.Count; i++) {
            if(i < subjectInfo.questionInfos[questionIdx].options.Count) {
                toggleLists[i].gameObject.SetActive(true);
                toggleLists[i].GetComponentInChildren<Text>().text = subjectInfo.questionInfos[questionIdx].options[i];
            } else {
                toggleLists[i].gameObject.SetActive(false);
            }
        }
        if (questionIdx == 0) {
            scoreList.Clear();
            gameObject.SetActive(true);
        }
    }

    public void ShowResult() {
        scrollView.gameObject.SetActive(true);
        questionText.gameObject.SetActive(false);
        preQuestionBtn.gameObject.SetActive(false);
        for(int i = 0; i < toggleLists.Count; i ++) {
            toggleLists[i].gameObject.SetActive(false);
        }
        string result = subjectInfo.GetResult(scoreList);
        resultText.text = result;
        ReportInfo info = new ReportInfo();
        DateTime time = DateTime.Now;
        info.name = time.Year.ToString() + "-" + time.Month.ToString("00") + "-" + time.Day.ToString("00") + " "
        + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");
        info.type = 1;
        info.result = result;
        info.username = GameController.manager.accountMan.selfInfo.username;
        GameController.manager.reportMan.AddReport(info);
    }

    private void Update() {
        preQuestionBtn.interactable = questionIdx != 0;
    }

}
