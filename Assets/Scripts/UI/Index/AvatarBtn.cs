using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarBtn : MonoBehaviour, IPointerClickHandler {

    public Image img;
    private Util.NoneParamCallBack callBack;

    public int avatarIdx;

    public void SetContent(int idx, Util.NoneParamCallBack c) {
        avatarIdx = idx;
        img.sprite = Resources.Load<Sprite>("Avatar/avatar" + avatarIdx);
        callBack = c;
    }

    public void OnPointerClick(PointerEventData eventData) {
        IndexUI.selectAvatar = this;
        GameController.manager.accountMan.selfInfo.avatar = avatarIdx;
        if (callBack != null)
            callBack();
    }
}
