//
// SetLevelsMenu.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetLevelsMenu : MonoBehaviour
{
	public string currentMain = "N1";
	public Vector3 startPosition = Vector3.zero;
	public Object level;
	public Transform parent;
	protected Vector3 positionSum = Vector3.zero;
	protected static Vector3 sumX = new Vector3(0.3f,0,0);
	protected static Vector3 sumY = new Vector3(0,-0.3f,0);
	protected static Vector3 scaleSet = new Vector3(1.3f,1.3f,1.3f);


	protected static int maxX = 5;
	protected int currentX = 0;
	protected static int maxY = 3;
	protected int currentY = 0;

	protected int currentMainPos = 0;
	protected string[] mainLevelsName;
	protected Dictionary<string,List<GameObject>> levels;

	// Use this for initialization
	void Awake ()
	{
		levels = new Dictionary<string,List<GameObject>>();

		ParserXML parse = new ParserXML();
		parse.LoadFile("LevelsInfo");
		Dictionary< string,Dictionary<string, int>> levelParse = parse.GetAllLevels ();
		Dictionary< string,Dictionary<string, int>>.KeyCollection levelParseKeys = levelParse.Keys;

		mainLevelsName = new string[levelParseKeys.Count];
		int i =0;
		foreach( string main in levelParse.Keys ){
			mainLevelsName[i]=main;
			++i;

			string[] mainYlevels = new string[(levelParse[main].Keys).Count];
			(levelParse[main].Keys).CopyTo(mainYlevels,0);
			levels[main] = new List<GameObject>();
			CreateLevels(main,mainYlevels);
		}

	

	}

	void CreateLevels( string mainLevel, string[] grades){
		positionSum = startPosition;
		currentX = 0;
		currentY = 0;

		bool enabledScripts = mainLevel.Contains(currentMain);
		foreach(string scene in grades ){
			
			if(!scene.Contains("00")){
				GameObject news = (GameObject) Instantiate(level,positionSum,Quaternion.identity);
				news.name = mainLevel+scene;
				news.transform.parent = parent;
				news.transform.localScale = scaleSet;
				news.GetComponent<TextMesh>().text = scene;
				news.SetActive(enabledScripts);

				levels[mainLevel].Add(news);

				++currentX;
				positionSum += sumX;
				if(currentX >= maxX){
					++currentY;
					if(currentY >= maxY){
						Debug.LogWarning("Ya se agregaron todos los posibles cuadritos, tener cuidado.");
					}
					currentX=0;
					positionSum=startPosition;
					positionSum+=sumY*currentY;
					
				}
			}
		}
	}
	protected void ActiveLevels(bool value){
		foreach(GameObject game in levels[currentMain]){
			game.SetActive(value);
		}
	}
	public void NextLevel(){
		ActiveLevels(false);
		currentMainPos++;
		if(currentMainPos >= mainLevelsName.Length)
			currentMainPos =0;
		currentMain = mainLevelsName[currentMainPos];
		ActiveLevels(true);

	}
	public void BackLevel(){
		ActiveLevels(false);
		currentMainPos--;
		if(currentMainPos < 0)
			currentMainPos = mainLevelsName.Length -1;
		currentMain = mainLevelsName[currentMainPos];
		ActiveLevels(true);
	}
}

