//
// DeletePlayerPrefs.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class DeletePlayerPrefs : MonoBehaviour
{

	public void DeleteAll () {
		LevelInfoKlaus.ResetLevels();
		Debug.Log ("Deleted all player prefs!!");
	}
}

