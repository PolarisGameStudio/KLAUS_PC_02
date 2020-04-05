using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaZero : MonoBehaviour {

    SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        sprite.color = new Color(1f, 1f, 1f, 0f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
