using UnityEngine;

public class K1AnimationTrigger : MonoBehaviour
{
    public CutsceneController madKlaus;
    public bool removeTarget;
    public bool startAnimation;
    public float zoom;
    public float timeToTarget, timeToZoom;
	public AudioSource aSource;
	public float delay;
	private bool played = false;

    bool k1Entered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!k1Entered && LayerMask.LayerToName(other.gameObject.layer) == "Player2")
        {
            k1Entered = true;

            if (startAnimation){
                madKlaus.StartAnimation();
				if(!played)
				{
					played = true;
					aSource.PlayDelayed (delay);
				}
			}
            else if (!removeTarget)
                madKlaus.AddTarget(transform, zoom, timeToTarget, timeToZoom);
            else
                madKlaus.RemoveTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (k1Entered && LayerMask.LayerToName(other.gameObject.layer) == "Player2")
        {
            k1Entered = false;
        }
    }
}
