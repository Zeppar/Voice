using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectUI : MonoBehaviour {
    public Button backBtn;
    public Transform subjectParent;
    public ScrollRect scrollView;
    public SubjectItem subjectItemPrefab;
    public static SubjectItem selectSubject;
    public SubjectDetail subjectDetail;
    SubjectItem lastSubject = null;

    private void Start() {
        backBtn.onClick.AddListener(() => {
            if (subjectDetail.gameObject.activeInHierarchy) {
                subjectDetail.gameObject.SetActive(false);
                selectSubject = null;
                scrollView.gameObject.SetActive(true);
            } else {
                IndexUICtrl.instance.SetPageType(PageType.Index);
            }
        });
        InitSubject();
    }

    void InitSubject() {
        Util.DeleteChildren(subjectParent);
        foreach (KeyValuePair<uint, SubjectInfo> kv in GameController.manager.subjectMan.subjectDict) {
            SubjectItem item = Instantiate(subjectItemPrefab) as SubjectItem;
            item.SetContent(kv.Value);
            item.transform.SetParent(subjectParent, false);
        }
    }

    void Update() {
        if (lastSubject != selectSubject) {
            lastSubject = selectSubject;
            // jump to subject detail
            if (selectSubject != null) {
                subjectDetail.ShowSubject(selectSubject.info, 0);
                scrollView.gameObject.SetActive(false);
            }
        }
    }
}
