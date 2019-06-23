using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    public Button backBtn;
    public Button pressureBtn;
    public Button reportBtn;
    public Button paintBtn;

    public static ReportItem selectItem = null;
    private ReportItem lastItem = null;
    public ReportItem reportPrefab;
    public Transform reportParent;
    public ReportDetail reportDetail;
    public GameObject foreGo;

    private void Start() {
        backBtn.onClick.AddListener(() => {
            if (reportDetail.gameObject.activeInHierarchy) {
                reportDetail.gameObject.SetActive(false);
                pressureBtn.gameObject.SetActive(true);
                reportBtn.gameObject.SetActive(true);
                paintBtn.gameObject.SetActive(true);
                foreGo.SetActive(true);
            } else {
                IndexUICtrl.instance.SetPageType(PageType.Index);
                selectItem = null;
                lastItem = null;
            }
        });
        pressureBtn.onClick.AddListener(() => {
            ShowReportList(0);
        });
        reportBtn.onClick.AddListener(() => {
            ShowReportList(1);
        });
        paintBtn.onClick.AddListener(() => {
            ShowReportList(2);
        });
        pressureBtn.onClick.Invoke();
    }

    public void ShowReportList(int type) {
        Util.DeleteChildren(reportParent);
        if(GameController.manager.reportMan.reportDict.ContainsKey(type)) {
            var list = GameController.manager.reportMan.reportDict[type];
            for (int i = 0; i < list.Count; i++) {
                ReportItem item = Instantiate(reportPrefab);
                item.SetContent(list[i]);
                item.transform.SetParent(reportParent, false);
            }
        }
    }

   
    private void Update() {
        if(lastItem != selectItem) {
            pressureBtn.gameObject.SetActive(false);
            reportBtn.gameObject.SetActive(false);
            paintBtn.gameObject.SetActive(false);
            reportDetail.ShowDetail(selectItem.info);
            foreGo.SetActive(false);
            lastItem = selectItem;
        }
    }
}
