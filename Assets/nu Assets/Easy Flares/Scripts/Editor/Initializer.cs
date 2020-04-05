using UnityEngine;
using System.Collections;
using UnityEditor;
using ch.sycoforge.Flares;

[InitializeOnLoad]
public static class Initializer
{
	// Use this for initialization
    static Initializer() 
    {
        GlobalFlareSettings.ReportErrors = false;
	}
}
