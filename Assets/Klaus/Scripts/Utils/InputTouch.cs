//
// InputController.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System;
using TouchScript.Gestures;

public enum ActionShowInput{
	Default,
	Pause,
	Left,
	Right,
};

public class InputTouch : MonoBehaviour {

	public GameObject touchSFX;
	public bool sono = false;


	

	protected ActionShowInput action = ActionShowInput.Default;

	private bool isPressed =false;
    public bool IsPressed
    {

        get
        {
            return isPressed;
        }
    }

	protected virtual void setIsPressed(bool value){
	
		isPressed = value;
	}

	protected float correctSize = 1.0f;

	private void OnEnable()
	{
		// subscribe to gesture's Tapped event
		GetComponent<TapGesture>().Tapped += tappedHandler;
		GetComponent<FlickCrazyGesture>().Flicked += flickHandler;
		GetComponent<PressGesture>().Pressed += pressHandler;
		GetComponent<ReleaseGesture>().Released += releaseHandler;
	}
	
	private void OnDisable()
	{
		// don't forget to unsubscribe
		GetComponent<TapGesture>().Tapped -= tappedHandler;
		GetComponent<FlickCrazyGesture>().Flicked -= flickHandler;
		GetComponent<PressGesture>().Pressed -= pressHandler;
		GetComponent<ReleaseGesture>().Released -= releaseHandler;
		
	}
	protected virtual void ActionMovement(Vector2 movement){
	}

	private void tappedHandler(object sender, EventArgs e)
	{

		if(resume){
			tappedHandlerDirection();
		}

		setIsPressed( false);
	}
	protected void tappedHandlerDirection(){
		
		ActionMovement(Vector2.zero);
		
		action = ActionShowInput.Pause;
	
	}
	private void flickHandler(object sender, EventArgs e)
	{


		//Debug.Log("Flick");
		if(resume){
			FlickCrazyGesture flick = sender as FlickCrazyGesture;
			Vector2 direction = flick.ScreenFlickVector.normalized ;
			flickHandlerDirection(direction);
			
		}

		setIsPressed(false);
	}
	protected void flickHandlerDirection(Vector2 direction){
		if(direction.x > 0 || direction.y > 0){
			//	Debug.Log("Flick horizontal derecha");
			
			ActionMovement(direction);
			action = ActionShowInput.Right;
		}else if(direction.x < 0 || direction.y < 0 ){
			//	Debug.Log("Flick horizontal izquierda");
			
			ActionMovement(direction);
			action = ActionShowInput.Left;
		}

	}
	private void pressHandler(object sender, EventArgs e)
	{
		setIsPressed(true);
		//Debug.Log("Flick");
		if(resume){
			pressHanlderAux();
		
		}
	}
	protected void pressHanlderAux(){
		if (!sono){
			sono = true;
			Instantiate(touchSFX, transform.position, transform.rotation);
		}
	}


	private void releaseHandler(object sender, EventArgs e)
	{
		setIsPressed(false);
		sono = false;
		//Debug.Log("Flick");

	}


	
	
	protected bool resume = true;
	public void OnPauseGame(){
		resume = false;


	} 
	public void OnResumeGame(){
		resume = true;

	}
	
}
