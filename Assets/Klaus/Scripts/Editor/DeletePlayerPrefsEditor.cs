using UnityEngine;
using UnityEditor;
using System.Collections;

public class DeletePlayerPrefsEditor : MonoBehaviour {

	[MenuItem("Utils/DeleteSavefile")]
	public static void PanicButton () {
		PlayerPrefs.DeleteAll();
		Debug.Log ("Deleted all player prefs!!");
	}
}
