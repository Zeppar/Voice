using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallSpawner : MonoBehaviour
{
    public FallGo fallGo;
    public List<Sprite> sps;

    private float spawnSpeed = 0.4f;
    private float timer = 0.4f;

    private void Update() {
        timer += Time.deltaTime;
        if(timer > spawnSpeed) {
            FallGo go = Instantiate(fallGo) as FallGo;
            go.SetState(new Vector3(Random.Range(50, Screen.width - 50), Screen.height + 100, 0), Random.Range(8, 12),
                sps[Random.Range(0, sps.Count - 1)], Random.Range(1.0f, 1.5f));
            go.transform.SetParent(transform);
            spawnSpeed = Random.Range(0.4f, 0.6f);
            timer = 0;
        }
    }

    public void UpdateFallGo(float volume) {
        Debug.Log(volume);
        if(volume > 0.5f) {
            FallGo[] arr = GetComponentsInChildren<FallGo>();
            for (int i = 0; i < arr.Length; i++) {
                arr[i].GetComponent<Animator>().SetBool("boom", true);
            }
        }
    }
}
