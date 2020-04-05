//
// KeyChain.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KeyChain : Singleton<KeyChain>
{
    Dictionary<PlayersID,int> keys = new Dictionary<PlayersID, int>();

    public Action<PlayersID,int> onKey;

    public void AddKey(PlayersID id)
    {
        if (keys.ContainsKey(id))
        {
            keys [id] += 1;
        } else
        {
            keys.Add(id, 1);
        }
        if (!object.ReferenceEquals(onKey, null))
        {
            onKey(id, keys [id]);
        }
    }


    public bool useKey(PlayersID id, int number)
    {


        if (number > 0 && keys.ContainsKey(id) && keys [id] >= number)
        {
            keys [id] -= number;
            if (!object.ReferenceEquals(onKey, null))
            {
                onKey(id, keys [id]);
            }
            return true;
        }

        return false;
    }
}

