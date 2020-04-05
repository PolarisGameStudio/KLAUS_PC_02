//
// ManagerFinal.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class ManagerFinal : MonoBehaviour
{
	public GameObject floatingMusik;
	public bool played = false;
	public GameObject klaus;
	public GameObject klausMuerto;

	protected bool isStop = false;
	public float distanceToStop = 5f;
	public float minOrtho = 3f;
	public float maxOrtho = 6f;

	public float timeToWakeUp = 3.0f;
	public float timeToFinish = 3.0f;

	protected float distanceForOrtho;
	protected float orthoForOrtho;

	protected Camera mainCamera;


	protected float offSetZ;
	protected static float percentToUp = 0.45f;
		// Use this for initialization
	void Awake ()
	{
		klausMuerto.GetComponent<Rigidbody2D>().transform.position = new Vector3(62.47658f,-3, 0);
		mainCamera = Camera.main;
		offSetZ = mainCamera.transform.position.z;
		klausMuerto.transform.position = new Vector3(klausMuerto.transform.position.x,klaus.transform.position.y,klausMuerto.transform.position.z);
		distanceForOrtho = Vector3.Distance(klaus.transform.position,klausMuerto.transform.position);

		orthoForOrtho = mainCamera.orthographicSize;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		klausMuerto.transform.position = new Vector3(klausMuerto.transform.position.x,-3,klausMuerto.transform.position.z);
		if(!isStop){
			mainCamera.orthographicSize = Vector3.Distance(klaus.transform.position,klausMuerto.transform.position) * orthoForOrtho / distanceForOrtho;
			mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize,minOrtho,maxOrtho);
			mainCamera.transform.position = klaus.transform.position + Vector3.right*mainCamera.orthographicSize + new Vector3(0,mainCamera.orthographicSize*percentToUp,offSetZ);
		}
		if( Vector3.Distance(klaus.transform.position,klausMuerto.transform.position) <= distanceToStop){
			isStop = true;
			Invoke ("stopMusik",0.5f);
			klaus.SendMessage("StopControl",SendMessageOptions.DontRequireReceiver);
			Invoke("wakeUpKlaus",timeToWakeUp);
		}
	}

	void wakeUpKlaus(){

		klausMuerto.GetComponent<Animator>().SetBool("Muerto",true);

		Invoke ("stopAmbient",1.0f);
		Invoke("finishim",timeToFinish);

	}
	protected ChangueLevelFade cha;

	void finishim(){
		cha = new ChangueLevelFade();
		//cha.ChangueToHard("LogoFinalKlaus");
		cha.ChangueToHard("LogoFinalKlaus");
	}
	void stopMusik(){
		if(GameObject.Find("AS_MusikManager").GetComponent<AudioSource>().volume >0){
			GameObject.Find("AS_MusikManager").GetComponent<AudioSource>().volume -= Time.deltaTime;
		}
	}
	void stopAmbient(){
		GameObject.Find("AS_AmbientN03").GetComponent<AudioSource>().mute = true;
		if (!played){
			played = true;
			Instantiate(floatingMusik, transform.position, transform.rotation);
		}
	}
}

