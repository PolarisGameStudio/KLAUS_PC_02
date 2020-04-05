using UnityEngine;
using System.Collections;

public class BulletBaseTrigger : MonoBehaviour
{

    public Collider2D box;
    public GameObject bulletDeathSFX;
    public GameObject bulletDeathGSFX;
    public bool isMegaElectricKill;
    public bool isElectricKill;
    public string isGlitch;
	public string isCollectable;

    [HideInInspector]
    public bool canKillPlayer = true;

    protected virtual void OnEnable()
    {
        canKillPlayer = true;
        box.enabled = true;
        isGlitch = Application.loadedLevelName.Substring(0, 2);
		isCollectable = Application.loadedLevelName.Substring(0, 3);
    }
    // Use this for initialization
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Glitch"))
        {
            if (box.enabled)
            {
                if (other.CompareTag("Player") && canKillPlayer)
                {
                    if (isMegaElectricKill)
                        other.GetComponent<DeadState>().typeOfDead = DeadType.MegaRay;
                    else if (isElectricKill)
                        other.GetComponent<DeadState>().typeOfDead = DeadType.Ray;
                    if(isGlitch=="W5" && isCollectable != "W5C")//klvo
                    {
                        other.GetComponent<MoveState>().Kill(bulletDeathGSFX);
                    }else//endKlvo
                    other.GetComponent<MoveState>().Kill(bulletDeathSFX);
                } else
                {
                    KillObject killer = other.GetComponent<KillObject>();
                    //                Debug.Log(other.name +" Killobject: "+ killer);
                    if (killer != null)
                        killer.Kill();
                }
                box.enabled = false;
                HandlerDestroy();

            }
        }
    }

    public virtual void HandlerDestroy()
    {

    }

}
