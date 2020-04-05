using UnityEngine;
using System.Collections;

public class AS_LevelComplete : MonoBehaviour
{

    private AudioSource AS_Target;
    public AudioSource musikComplete;
    private float vol;
    public float speed;
    void Complete()
    {

        if (GameObject.Find("AS_MusikManager_Arrecho") != null)//para la musica
        {
            AS_Target = GameObject.Find("AS_MusikManager_Arrecho").GetComponent<AudioSource>();
            vol = AS_Target.volume;
            AS_Target.volume = 0;
        }
        musikComplete.Play();
        //Invoke ("VolUP",musikComplete.clip.length);
    }
    void OnLevelWasLoaded()
    {
        if (AS_Target != null)
            AS_Target.volume = vol;
    }
    /*void VolUP ()
    {
        StartCoroutine ("Up");
    }
    IEnumerator Up()
    {
        for (float f = 0; f < vol; f += speed * Time.deltaTime)
        {
            AS_Target.volume = f;
            yield return null;
        }
    }*/
}
