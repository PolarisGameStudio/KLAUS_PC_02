using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceTutorial : MonoBehaviour {

    Animator currentAnim;
    // Use this for initialization
    void Start () {
		currentAnim = gameObject.GetComponent(typeof(Animator)) as Animator;
    }
	
	// Update is called once per frame
	void Update () {


        if (InputEnum.GamePad.ToString() == "keyboard")
        {
            currentAnim.SetBool("GamePad", false);
        }

        else
        {
            currentAnim.SetBool("GamePad", true);
        }




    }
}
