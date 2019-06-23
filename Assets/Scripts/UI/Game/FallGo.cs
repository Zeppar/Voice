using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallGo : MonoBehaviour
{
    float speed = 0;
    public void SetState(Vector3 pos, float _speed, Sprite sp, float scale) {
        gameObject.SetActive(true);
        transform.position = pos;
        speed = _speed;
        GetComponent<Image>().sprite = sp;
        transform.localScale = new Vector3(scale, scale, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        if (transform.position.y < -50.0f)
            Destroy(gameObject);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
