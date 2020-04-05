//
// DoorTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Luminosity.IO;
using Rewired;
public class DoorTrigger : MonoBehaviour
{
	public GameObject doorSFX;
	public SpriteRenderer[] spriteLeft;
	public SpriteRenderer[] spriteRight;


	protected Animation spriteLeftAnim;
	protected Animation spriteRightAnim;

	public float offsetX = 0.7f;
	public float timeToNewOrder = 2.0f;

    MoveState[] klauses;
    public float minDistToReachPoint = 0.35f;

    bool allEnter = false;

    public string Control = "Left Stick Vertical";

	void Awake(){
        klauses = GameObject.FindObjectsOfType<MoveState>();

		Transform parentLeft = spriteLeft[0].transform.parent;
		Transform parentRight = spriteRight[0].transform.parent;

		spriteLeftAnim = parentLeft.gameObject.AddComponent<Animation>();
		spriteLeftAnim.playAutomatically = false;
		spriteRightAnim = parentRight.gameObject.AddComponent<Animation>();
		spriteRightAnim.playAutomatically = false;



		AnimationCurve curveSizeLeft =new AnimationCurve(new Keyframe(0, parentLeft.localPosition.x ), 
		                                                 new Keyframe(timeToNewOrder,parentLeft.localPosition.x - offsetX),
		                                                 new Keyframe(timeToNewOrder*2, parentLeft.localPosition.x));

		AnimationCurve curveSizeRight =new AnimationCurve(new Keyframe(0, parentRight.localPosition.x ), 
		                                                  new Keyframe(timeToNewOrder,parentRight.localPosition.x + offsetX),
		                                                  new Keyframe(timeToNewOrder*2, parentRight.localPosition.x));
		AnimationClip clipSizeLeft = new AnimationClip();

		AnimationClip clipSizeRight = new AnimationClip();
		clipSizeLeft.SetCurve("",  typeof(Transform), "localPosition.x", curveSizeLeft);
		clipSizeRight.SetCurve("",  typeof(Transform), "localPosition.x", curveSizeRight);
		spriteRightAnim.AddClip(clipSizeRight, "Open");
		spriteLeftAnim.AddClip(clipSizeLeft, "Open");
	}


    IEnumerator SortingDoor(float time){

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));

        foreach (SpriteRenderer rend in spriteLeft)
        {
			rend.sortingOrder = 2;
		}
		foreach(SpriteRenderer rend in spriteRight){
			rend.sortingOrder = 2;
		}
	}

    void LateUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (allEnter == false)
            {
                bool TodoRdy = true;
                for (int i = 0; i < klauses.Length; ++i)
                {
                    if (Vector3.Distance(klauses[i].transform.position, transform.position) > minDistToReachPoint)
                    {
                        TodoRdy = false;
                        break;
                    }
                }
                if (TodoRdy && ReInput.players.GetPlayer(0).GetAxis(Control) > SaveManager.Instance.dataKlaus.controlSensitivity )
                {
                    allEnter = true;
                    //Aqui les cambio de estados
                    for (int i = 0; i < klauses.Length; ++i)
                    {
                        if (!klauses[i].enabled)
                        {
                            allEnter = false;
                            break;
                        }
                    }
                    if (!allEnter)
                        return;

                    for (int i = 0; i < klauses.Length; ++i)
                    {
                        klauses[i].ChangueToEnter();
                    }
                    Instantiate(doorSFX, transform.position, transform.rotation);
                    spriteLeftAnim.Play("Open");
                    spriteRightAnim.Play("Open");

                    StartCoroutine("SortingDoor", timeToNewOrder);
                  
                    ManagerPause.Pause = (true);

                }
            }
        }
    }

}
