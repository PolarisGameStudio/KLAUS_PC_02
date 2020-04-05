using UnityEngine;
using System.Collections;

public class AS_PlayW5BossMusik : MonoBehaviour 
{

	public bool played = false;
	void  OnTriggerEnter2D(Collider2D other)
	{        
		if(other.CompareTag("Player")){
			
			if(!played)
			{
				played = true;
				AS_W5BossMusik.Instance.K1Musik();
			}
		}
	}
}
