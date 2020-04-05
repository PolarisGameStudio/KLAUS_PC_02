//
// CheckCounter.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class CheckCounter 
{
	public static string nextLevel {get; private set;}

	/// <summary>
	/// Gets the letter grade.
	/// Where 1 - A, 2 - B, 3 - C, 4 - D
	/// </summary>
	/// <returns>1-4</returns>
	/// <param name="levelName">Level name.</param>
	/// <param name="counter">Counter.</param>
	public static int getLetterGrade(string levelName, int counter){
		ParserXML parse = new ParserXML();
		parse.LoadFile("LevelsInfo");

		int[] grades = parse.GetValueNode(levelName);
		nextLevel = parse.next;	
		for(int i = 0;i<grades.Length;i++){
		
			if(grades[i] >= counter){

				return i+1;
			}
		}
		return -1;
	}
}

