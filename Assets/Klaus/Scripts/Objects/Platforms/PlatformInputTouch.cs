//
// PlatformControllerInput.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;


public class PlatformInputTouch : InputTouch {
	
	public PlatformMovement move;
	protected override void ActionMovement(Vector2 movement){
		move.SetMovement(movement);
	}
	protected override void setIsPressed(bool value){
		base.setIsPressed(value);
        ManagerPlatform.Instance.SelectPlatform (move.GetInstanceID ());
	}
}
