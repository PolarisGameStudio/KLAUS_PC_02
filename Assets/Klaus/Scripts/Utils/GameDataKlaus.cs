//
// GameDataKlaus.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameDataKlaus 
{    
	//public int scoreN101;
	//public string N101;
	public Dictionary< string,Dictionary<string, int>> levels = new Dictionary<string, Dictionary<string, int>>();

	public byte[] bigData;
	bool testBigData = false;
	const int bigDataSize = 4*1024*1024;

	public GameDataKlaus()
	{
        
		if (testBigData)
		{
			bigData = new byte[bigDataSize];
		}
	}
	public void InitData(){
		ParserXML parse = new ParserXML();
		parse.LoadFile("LevelsInfo");
		levels.Clear();

		levels = parse.GetAllLevels();
	}
	public int getLevelData(string level){
		string mainlevel = level.Substring(0,2);
		string levelnumb = level.Substring(2);

		return levels[mainlevel][levelnumb];
	}
	public void setLevelData(string level, int value){
		string mainlevel = level.Substring(0,2);
		string levelnumb = level.Substring(2);
		
		levels[mainlevel][levelnumb] = value;
	}
	public byte[] WriteToBuffer()
	{
		if (testBigData)
		{
			for (int i = 0; i < bigDataSize; i++)
			{
				bigData[i] = (byte)(i % 13);
			}
		}
		
		System.IO.MemoryStream output = new MemoryStream(bigDataSize+16);
		System.IO.BinaryWriter writer = new BinaryWriter(output);

		foreach(KeyValuePair< string,Dictionary<string, int>> main in levels)
		{
			foreach(KeyValuePair<string, int> leve in main.Value){
				writer.Write(main.Key+":"+leve.Key+":"+leve.Value.ToString());
			}
		}
		//writer.Write(scoreN101);
		//writer.Write(score);
		if (testBigData)
		{
			writer.Write(bigData);
		}
		writer.Close();
		return output.GetBuffer();
	}
	
	public void ReadFromBuffer(byte[] buffer)
	{
		System.IO.MemoryStream input = new MemoryStream(buffer);
		System.IO.BinaryReader reader = new BinaryReader(input);
		InitData();

		Dictionary<string,Dictionary<string, int>>.ValueCollection valCol = levels.Values;
		foreach(Dictionary<string, int> main in valCol)
		{
			for(int i =0;i<main.Count;i++){
				string[] value =reader.ReadString().Split(':');
				(levels[value[0]])[value[1]] = int.Parse(value[2]);
			}
		}
		//scoreN101 = reader.ReadInt32();
		//score = reader.ReadInt32();
		if (testBigData)
		{
			bigData = reader.ReadBytes(bigDataSize);
		}
		reader.Close();
		
		if (testBigData)
		{
			int correct = 0;
			for (int i = 0; i < bigDataSize; i++)
			{
				if (bigData[i] != (byte)(i % 13))
				{
					//OnScreenLog.Add("Read invalid data");
					break;
				}
				correct++;
			}
			
			if (correct == bigDataSize)
			{
				//OnScreenLog.Add("Read valid data");
			}
		}
	}
}

