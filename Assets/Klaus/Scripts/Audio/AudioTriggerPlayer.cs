using UnityEngine;
using System.Collections;

public class AudioTriggerPlayer : MonoBehaviour {

    public PlayersID TypePlayer = PlayersID.Player1Klaus;
    bool isShow = false;
	public AudioClip Steps;
	public float musik02;
	public GameObject musik03;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShow)
        {
            if (CompareDefinition(other))
            {
                isShow = true;
				GetComponent<AudioSource>().Play();
				Invoke ("changeClip",2f);
				musik02 = GameObject.Find("AS_Musik02").GetComponent<AudioSource>().GetComponent<AudioSource>().volume;
				StartCoroutine ("VolDown");
				Invoke("Musik03",7f);

            }

        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && other.GetComponent<PlayerInfo>().playerType == TypePlayer;
    }
	void changeClip (){
		GetComponent<AudioSource>().clip = Steps;
		GetComponent<AudioSource>().Play ();
	}
	void Musik03()
	{
		Instantiate(musik03, transform.position, transform.rotation);
	}
	IEnumerator VolDown()
	{
		while (GameObject.Find("AS_Musik02").GetComponent<AudioSource>().GetComponent<AudioSource>().volume >0)
		{
			GameObject.Find("AS_Musik02").GetComponent<AudioSource>().GetComponent<AudioSource>().volume -= 0.5f*Time.deltaTime;
			yield return null;
		}
	}
}
