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
                if(GameController.manager.curIdentityType == IdentityType.User)
                    IndexUICtrl.instance.SetPageType(PageType.Index);
                else
                    IndexUICtrl.instance.SetPageType(PageType.UserList);
                selectItem = null;
                lastItem = null;
            }
        });
        pressureBtn.onClick.AddListener(() => {
            ShowReportList(0, GameController.manager.accountMan.selfInfo.username);
        });
        reportBtn.onClick.AddListener(() => {
            ShowReportList(1, GameController.manager.accountMan.selfInfo.username);
        });
        paintBtn.onClick.AddListener(() => {
            ShowReportList(2, GameController.manager.accountMan.selfInfo.username);
        });

        if(GameController.manager.curIdentityType == IdentityType.User)
            ShowReportList(0, GameController.manager.accountMan.selfInfo.username);
    }

    public void ShowReportList(int type, string username) {
        Util.DeleteChildren(reportParent);
        if(GameController.manager.reportMan.reportDict.ContainsKey(type)) {
            var list = GameController.manager.reportMan.GetInfos(type, username);
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
