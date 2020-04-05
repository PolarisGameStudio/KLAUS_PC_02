using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SiluetaOffTrigger : MonoBehaviour {


    public GameObject Silueta;
    public SpriteRenderer sprite;

    public float timeDisappiring = 0.1f;
    Color baseColor;
    bool isShow = false;
    void Awake() {
        baseColor = sprite.material.color;
        baseColor.a = 0;
    }


    protected virtual void Start() {
        if (SaveManager.Instance.comingFromTimeArcadeMode) {
            gameObject.SetActive(false);
            return;
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (!isShow) {
            if (CompareDefinition(other)) {
                isShow = true;
                StartCoroutine(TimeOff());
            }

        }
    }

    protected virtual bool CompareDefinition(Collider2D other) {
        return other.gameObject.GetInstanceID() == Silueta.GetInstanceID();
    }
    IEnumerator TimeOff() {
        sprite.material.DOColor(baseColor, timeDisappiring);
        yield return new WaitForSeconds(timeDisappiring);
        Silueta.SetActive(false);
    }
}
