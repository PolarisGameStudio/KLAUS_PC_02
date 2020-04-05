using UnityEngine; using System.Collections;

public class AnimatedTexture : MonoBehaviour { public int materialIndex = 0; public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f ); public string textureName = "_MainTex";

	public int TilesX;
	public int TilesY;
	public float AnimationCyclesPerSecond;
	
	void Start ()
	{
		GetComponent<Renderer>().material.mainTextureScale = new Vector2(1.0f / TilesX, 1.0f / TilesY);
		StartCoroutine(ChangeOffSet());
	}
	
	IEnumerator ChangeOffSet ()
	{
		for(int i = TilesY - 1; i > -1; i--)
		{
			for(int j = 0; j < TilesX; j++)
			{
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(1.0f / TilesX * j, 1.0f / TilesY * i);
				yield return new WaitForSeconds(1.0f / (TilesX * TilesY * AnimationCyclesPerSecond));
				if(i == 0 && j == TilesX - 1)
				{
					i = TilesY;
				}
			}
		}
	}



	Vector2 uvOffset = Vector2.zero;
	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}