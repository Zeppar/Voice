using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public Button closeBtn;

    private void Start() {
        closeBtn.onClick.AddListener(() => {
            SoundManager.manager.StopMusic();
            gameObject.SetActive(false);
        });
    }
}
