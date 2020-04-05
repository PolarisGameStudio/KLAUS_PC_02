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

public class InputController : MonoBehaviour {
   



    /// <summary>
    /// Accion que sucede una vez que el movimiento se cumpla
    /// </summary>
    /// <param name="movement"></param>
    protected virtual void ActionMovement(Vector2 movement){
    }


    /// <summary>
    /// Manage the input every update
    /// </summary>
    protected  virtual void ControlPad(){
    }


    #region UnityCallbacks
    protected virtual void FixedUpdate(){

        ControlPad();
    }

    protected void flickHandlerDirection(Vector2 direction){

        ActionMovement(direction);

    }
    #endregion

}
