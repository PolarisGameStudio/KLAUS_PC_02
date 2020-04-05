//
// ParserXML.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class ParserXML 
{
	protected XmlReader reader ;
		// Use this for initialization
	public void LoadFile(string name)
	{
		if(reader == null){
			TextAsset asset =(TextAsset) Resources.Load(name);
			if(asset != null)
			{
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ConformanceLevel = ConformanceLevel.Fragment;
				settings.IgnoreWhitespace = true;
				settings.IgnoreComments = true;
				reader= XmlReader.Create(new StringReader(asset.text), settings);
			}else{
				Debug.LogError("No encuentro el archivo: "+name);
			}
		}
	}
	public void Reset(){
		reader.Close();
		reader = null;
	}
	public string next {get; private set;}
	public int[] GetValueNode(string levelID){
		next = "";
		string mainLevel;
		string level;
		string nextLevel;
		int numLevels = 0;
		int numGrades = 0;
		bool isLevel = false;
		int[] grades = new int[4];
		while (reader.Read()) {
			if (reader.IsStartElement()) {
//				Debug.Log("\r\n<"+reader.Name+">1");
				mainLevel = reader.GetAttribute("name");
				numLevels = int.Parse( reader.GetAttribute("levels"));
				for(int i=0;i<numLevels*2;i++){
					level = string.Empty;
					reader.Read();
					if (reader.IsStartElement()) 
					{
		//				Debug.Log("\r\n<"+reader.Name+">2");

						level =mainLevel+reader.GetAttribute("name");
						numGrades =  int.Parse( reader.GetAttribute("grades"));
						nextLevel = reader.GetAttribute("next");
						isLevel = string.Compare(level,levelID)==0;
						int kPos = 0;
						if(isLevel){
							grades = new int[numGrades];
						}
						for(int j=0;j<numGrades*2;j++){
							reader.Read();
							if (reader.IsStartElement()) {
						//		Debug.Log("\r\n<"+reader.Name+">3");
								if(isLevel){
									grades[kPos]=int.Parse(reader.GetAttribute("steps"));
									++kPos;
								}
							}
							else
							{
					//			Debug.Log("\r\n<\\"+reader.Name+">3 2");

							}
						}
						if(isLevel){
							Reset();
							next = nextLevel;
							return grades;
						}
					}
					else{
//						Debug.Log("\r\n<\\"+reader.Name+">2 2");

					}
				}
			} else{
			//	Debug.Log("\r\n<\\"+reader.Name+">1 2");
			}
		} 
		Reset();
		next = "";
		return null;
	}

	public string[] GetLevels(string nameN){
		string mainLevel;
		string level;
		int numLevels = 0;
		int numGrades = 0;
		while (reader.Read()) {
			if (reader.IsStartElement()) {
				mainLevel = reader.GetAttribute("name");
				if(!mainLevel.Contains(nameN)){
					continue;
				}
				numLevels = int.Parse( reader.GetAttribute("levels"));
				string[] grades = new string[numLevels];
				int posScenes = 0;
				for(int i=0;i<numLevels*2;i++){
					reader.Read();
					if (reader.IsStartElement()) 
					{
						level =reader.GetAttribute("name");
						numGrades =  int.Parse( reader.GetAttribute("grades"));
						grades[posScenes] = level;
						posScenes++;
						for(int j=0;j<numGrades*2;j++){
							reader.Read();
						}
					}
				} 
				Reset();
				return grades;
			}
		} 
		Reset();
		return null;
	}

	public Dictionary< string,Dictionary<string, int>> GetAllLevels(){
		Dictionary< string,Dictionary<string, int>> data = new Dictionary< string,Dictionary<string, int>>();

		string mainLevel;
		string level;
		int numLevels = 0;
		int numGrades = 0;
		while (reader.Read()) {
			if (reader.IsStartElement()) {
			
				mainLevel = reader.GetAttribute("name");
				data.Add(mainLevel,new Dictionary<string, int>());

				numLevels = int.Parse( reader.GetAttribute("levels"));
				string[] grades = new string[numLevels];
				int posScenes = 0;
				for(int i=0;i<numLevels*2;i++){
					reader.Read();
					if (reader.IsStartElement()) 
					{
						level =reader.GetAttribute("name");
						data[mainLevel].Add(level,0);
						numGrades =  int.Parse( reader.GetAttribute("grades"));
						grades[posScenes] = level;
						posScenes++;
						for(int j=0;j<numGrades*2;j++){
							reader.Read();
						}
					}
				} 
			}
		} 
		Reset();
		return data;
	}
}

