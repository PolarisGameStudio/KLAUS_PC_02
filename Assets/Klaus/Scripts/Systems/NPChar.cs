using UnityEngine;
using System.Collections;
using System;

public class NPChar : MonoBehaviour {

    public CharacterInputController character;
    public CharacterManager control;
    public Action ActiveNpcCallback;
    bool isUsed = false;
    public bool Activate = true;
    // Use this for initialization
    void Start() {
        if (Activate)
        {
            //CharacterManager.Instance.SetPlay(false, character);
            control.blockChangeCharacter = true;
        }
    }



    void OnTriggerEnter2D(Collider2D other) {
        if (!isUsed) {
            if (other.CompareTag("Player")) {
                control.blockChangeCharacter = false;

               // CharacterManager.Instance.SetPlay(Activate, character);
                if (ActiveNpcCallback != null)
                    ActiveNpcCallback();
                isUsed = true;
                gameObject.SetActive(false);
            }
        }
    }
}
