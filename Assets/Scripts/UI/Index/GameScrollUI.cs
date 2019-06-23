using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScrollUI : MonoBehaviour {

    public Transform itemParent;
    public ScrollRect scrollView;
    public LevelItem item;

    private bool isInit = false;
    private LevelItem[] items;
    private float totalScrollLength = 0;
    private float oneItemWidth = 0;

    public static int itemIdx = 0;

    private void Start() {
        StartCoroutine("StartScroll");
    }

    private IEnumerator StartScroll() {
        while (true) {
            yield return new WaitForEndOfFrame();
            if (itemParent.childCount != 0) {
                yield return new WaitForEndOfFrame();
                items = itemParent.GetComponentsInChildren<LevelItem>();
                Debug.Log(itemParent.GetComponent<RectTransform>().sizeDelta.x);
                Debug.Log(itemParent.GetComponent<RectTransform>().sizeDelta.y);
                Debug.Log(GetComponent<RectTransform>().rect.width);
                Debug.Log(GetComponent<RectTransform>().rect.height);
                totalScrollLength = itemParent.GetComponent<RectTransform>().sizeDelta.x - GetComponent<RectTransform>().rect.width;
                oneItemWidth = item.GetComponent<LayoutElement>().preferredWidth;
                isInit = true;
                break;
            }
        }
    }

    private void Update() {
        if (!isInit)
            return;
        float scrolledWidth = scrollView.horizontalNormalizedPosition * totalScrollLength;
        //Debug.Log(scrolledWidth + " - " + oneItemWidth);
        itemIdx = (int)(scrolledWidth / oneItemWidth);
    }
}
