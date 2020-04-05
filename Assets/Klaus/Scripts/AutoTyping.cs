using UnityEngine;
using System.Collections;

public class AutoTyping : MonoBehaviour {
  string myString = "This is a auto typing text...";
	float waitTime=1;
    // Use this for initialization
    void Start () {
		Debug.Log(myString );
        StartCoroutine("AutoType");
	}


   
    // Update is called once per frame
    IEnumerator AutoType () {
        foreach(char letter in myString.ToCharArray()){
			GetComponent<TextMesh>().text += letter; //for unity 5x

          yield return new WaitForSeconds(waitTime);
        }
    }
}
