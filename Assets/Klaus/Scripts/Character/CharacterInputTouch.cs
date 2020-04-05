//
// CharacterInputController.cs
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

public class CharacterInputTouch : InputTouch {

    public MoveState move;

    protected override void ActionMovement(Vector2 movement){
        move.SetMovement(movement);
    }
    protected void ControlPad(){

        if(resume){
            /*/if(ReInput.players.GetPlayer(0).GetButtonDown("Dup")){
                setIsPressed(true);
                pressHanlderAux();
                tappedHandlerDirection();
                setIsPressed(false);
            }else if(ReInput.players.GetPlayer(0).GetButtonDown("Dright")){
                setIsPressed(true);
                pressHanlderAux();
                flickHandlerDirection(Vector2.right);
                setIsPressed(false);
                sono = false;

            }else if(ReInput.players.GetPlayer(0).GetButtonDown("Dleft")){
                setIsPressed(true);
                pressHanlderAux();
                flickHandlerDirection(Vector2.right*-1);
                setIsPressed(false);
                sono = false;
            }
            /*/

            if (ReInput.players.GetPlayer(0).GetButtonDown("Move Up"))
            {
                setIsPressed(true);
                pressHanlderAux();
                tappedHandlerDirection();
                setIsPressed(false);
            }
            else if (ReInput.players.GetPlayer(0).GetButtonDown("Move Right"))
            {
                setIsPressed(true);
                pressHanlderAux();
                flickHandlerDirection(Vector2.right);
                setIsPressed(false);
                sono = false;

            }
            else if (ReInput.players.GetPlayer(0).GetButtonDown("Move Left"))
            {
                setIsPressed(true);
                pressHanlderAux();
                flickHandlerDirection(Vector2.right * -1);
                setIsPressed(false);
                sono = false;
            }
        }

    }
    protected void Update(){

        ControlPad();
    }
}
