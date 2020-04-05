using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdaptTreatmillProportions : MonoBehaviour {

    public SurfaceEffector2D surface;
    float variation = 5;
	// Use this for initialization
	void Start () {
      //  surface = GameObject.FindObjectOfType<SurfaceEffector2D>();
    //    Debug.Log("This is the surface speed " + surface.speed);
        surface.speed = surface.speed * variation;
        surface.forceScale = surface.forceScale / variation;

     //   Debug.Log("This is the new surface speed " + surface.speed);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
