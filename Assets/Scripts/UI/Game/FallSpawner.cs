using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallSpawner : MonoBehaviour
{
    public FallGo fallGo;
    public List<Sprite> sps;
    public Animator anim;

    private float spawnSpeed = 1.0f;
    private float timer = 0.4f;


    private void Update() {
        timer += Time.deltaTime;
        if(timer > spawnSpeed) {
            FallGo go = Instantiate(fallGo) as FallGo;
            go.SetState(new Vector3(Random.Range(50, Screen.width - 50), Screen.height + 100, 0), Random.Range(8, 12),
                sps[Random.Range(0, sps.Count - 1)], Random.Range(1.0f, 1.5f));
            go.transform.SetParent(transform);
            timer = 0;
            spawnSpeed -= 0.05f;
            spawnSpeed = Mathf.Max(spawnSpeed, 0.5f);
        }
    }

    public void UpdateFallGo(float volume) {
        Debug.Log(volume);
        if(volume > 0.5f) {
            FallGo[] arr = GetComponentsInChildren<FallGo>();
            for (int i = 0; i < arr.Length; i++) {
                if(i < GetBoomCount(volume))
                    arr[i].GetComponent<Animator>().SetBool("boom", true);
            }
        }
        if (volume > 0.8f)
            anim.SetTrigger("Shake");
    }

    public int GetBoomCount(float volume) {
        int count = 0;
        if (volume > 0.5f) {
            count = 5;
        } else if (volume > 0.4f) {
            count = 4;
        } else if (volume > 0.3f) {
            count = 3;
        } else if (volume > 0.2f) {
            count = 2;
        } else if (volume > 0.1f) {
            count = 1;
        }
        return count;
    }
}
