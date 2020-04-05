﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ListValues{
	public List<int> hashes = new List<int>(100000);
	public List<Transform> prefabs = new List<Transform>(100000);
	public List<int> directions = new List<int>(100000);
	public List<Transform> instances = new List<Transform>(100000);
}

public class TileMap : MonoBehaviour
{
	static Queue<PathTile> queue = new Queue<PathTile>();
	static List<PathTile> closed = new List<PathTile>();
	static Dictionary<PathTile, PathTile> source = new Dictionary<PathTile, PathTile>();

	public const int maxColumns = 10000;

	public int zOrder = 0;
	public float tileSize = 1;
	public Transform tilePrefab;
	public TileSet tileSet;

	public bool connectDiagonals;
	public bool cutCorners;


	/*
	public List<int> hashes = new List<int>(100000);
	public List<Transform> prefabs = new List<Transform>(100000);
	public List<int> directions = new List<int>(100000);
	public List<Transform> instances = new List<Transform>(100000);
*/
	public List<ListValues> orders = new List<ListValues>(100);
	void Start()
	{
		UpdateConnections();
	}

	public int GetHash(int x, int y)
	{
		return (x + TileMap.maxColumns / 2) + (y + TileMap.maxColumns / 2) * TileMap.maxColumns;
	}
	
	public int GetIndex(int x, int y,int z)
	{
		return (orders[z]).hashes.IndexOf(GetHash(x, y));
	}
	
	public Vector3 GetPosition(int index, int z)
	{
		index = (orders[z]).hashes[index];
		return new Vector3(((index % maxColumns) - (maxColumns / 2)) * tileSize, ((index / maxColumns) - (maxColumns / 2)) * tileSize, 0);
	}
	public void GetPosition(int index,int z, out int x, out int y)
	{
		index = (orders[z]).hashes[index];
		x = (index % maxColumns) - (maxColumns / 2);
		y = (index / maxColumns) - (maxColumns / 2);
	}

	public void UpdateConnections()
	{
		//Build connections
		PathTile r, l, f, b;
		foreach(ListValues lis in orders){
			int z = orders.IndexOf(lis);
			for (int i = 0; i < lis.instances.Count; i++)
			{
				var tile = lis.instances[i].GetComponent<PathTile>();
				if (tile != null)
				{
					int x, y;

					GetPosition(i,z, out x, out y);
					tile.connections.Clear();
					r = Connect(tile,z, x, y, x + 1, y);
					l = Connect(tile,z, x, y, x - 1, y);
					f = Connect(tile,z, x, y, x, y + 1);
					b = Connect(tile,z, x, y, x, y - 1);
					if (connectDiagonals)
					{
						if (cutCorners)
						{
							Connect(tile,z, x, y, x + 1, y + 1);
							Connect(tile,z, x, y, x - 1, y - 1);
							Connect(tile,z, x, y, x - 1, y + 1);
							Connect(tile,z, x, y, x + 1, y - 1);
						}
						else
						{
							if (r != null && f != null)
								Connect(tile,z, x, y, x + 1, y + 1);
							if (l != null && b != null)
								Connect(tile,z, x, y, x - 1, y - 1);
							if (l != null && f != null)
								Connect(tile,z, x, y, x - 1, y + 1);
							if (r != null && b != null)
								Connect(tile,z, x, y, x + 1, y - 1);
						}
					}
				}
			}
		}
	}

	PathTile Connect(PathTile tile, int z,int x, int y, int toX, int toY)
	{
		var index = GetIndex(toX, toY,z);
		if (index >= 0)
		{
			var other = (orders[z]).instances[index].GetComponent<PathTile>();
			if (other != null)
			{
				tile.connections.Add(other);
				return other;
			}
		}
		return null;
	}

	PathTile GetPathTile(int x, int y, int z)
	{
		var index = GetIndex(x, y,z);
		if (index >= 0)
			return (orders[z]).instances[index].GetComponent<PathTile>();
		else
			return null;
	}
	public PathTile GetPathTile(Vector3 position,int z)
	{
		var x = Mathf.RoundToInt(position.x / tileSize);
		var y = Mathf.RoundToInt(position.y / tileSize);
		return GetPathTile(x, y,z);
	}
	
	public bool FindPath(PathTile start, PathTile end, List<PathTile> path, Predicate<PathTile> isWalkable)
	{
		if (!isWalkable(end))
			return false;
		closed.Clear();
		source.Clear();
		queue.Clear();
		closed.Add(start);
		source.Add(start, null);
		if (isWalkable(start))
			queue.Enqueue(start);
		while (queue.Count > 0)
		{
			var tile = queue.Dequeue();
			if (tile == end)
			{
				path.Clear();
				while (tile != null)
				{
					path.Add(tile);
					tile = source[tile];
				}
				path.Reverse();
				return true;
			}
			else
			{
				foreach (var connection in tile.connections)
				{
					if (!closed.Contains(connection) && isWalkable(connection))
					{
						closed.Add(connection);
						source.Add(connection, tile);
						queue.Enqueue(connection);
					}
				}
			}
		}
		return false;
	}
	public bool FindPath(PathTile start, PathTile end, List<PathTile> path)
	{
		return FindPath(start, end, path, tile => true);
	}
	public bool FindPath(Vector3 start, Vector3 end, List<PathTile> path, Predicate<PathTile> isWalkable, int z)
	{
		var startTile = GetPathTile(start,z);
		var endTile = GetPathTile(end,z);
		return startTile != null && endTile != null && FindPath(startTile, endTile, path, isWalkable);
	}
	public bool FindPath(Vector3 start, Vector3 end, List<PathTile> path, int z)
	{
		return FindPath(start, end, path, tile => true,z);
	}
}
