using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AS_ActivateButton : MonoBehaviour {
	
	public void ActivateBtn () 
	{
		GetComponent<Button>().enabled = true;
	}
}
