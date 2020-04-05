//
// TextAnimVideo.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class TextAnimVideo : MonoBehaviour
{
	public TextMesh text;
	public GameObject sounds;
	public bool sono = false;
	public void Letter(string textToShow){
		text.text += " " + textToShow;
		if (sono == false && !GameObject.Find("AS_Typing1(Clone)")){
			Instantiate(sounds, transform.position, transform.rotation);	
			sono = true;
		}
	}

	public void ChangueScene(){
		ChangueLevelFade cha= new ChangueLevelFade();
		cha.ChangueTo("N100",1.0f);
		
	}
}

