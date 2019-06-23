using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAlert : MonoBehaviour {

    public Image showImage;
    public List<GameObject> scoreGos;
    public Image arrowImage;
    public List<Sprite> imageSps;

    private Vector2 targetPos;

    public void ShowWithLevel(int level) {
        showImage.sprite = imageSps[level];
        arrowImage.transform.localPosition = scoreGos[0].transform.localPosition;
        targetPos = new Vector2(scoreGos[level].transform.localPosition.x, arrowImage.transform.localPosition.y);
        gameObject.SetActive(true);
        StopCoroutine("SlideArrow");
        StartCoroutine("SlideArrow");
    }

    IEnumerator SlideArrow() {
        while(true) {
            if (Mathf.Abs(arrowImage.transform.localPosition.x - targetPos.x) < 0.2f) {
                arrowImage.transform.transform.localPosition = targetPos;
                break;
            }
            arrowImage.transform.localPosition = Vector2.Lerp(arrowImage.transform.localPosition, targetPos, 0.1f);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(1.0f);
        gameObject.SetActive(false);
    }

}
