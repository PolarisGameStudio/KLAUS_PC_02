using UnityEngine;
using System.Collections;

public class TriggerCameraMove : TriggerHistory
{
    public float TimeStop = 1.0f;

    public Transform Spot;
    public float newZoom = 5.2f;
    public float TimeToReachThatZoom = 1.0f;
    public float TimeToReachSpot = 1.0f;
    public bool K1 = false;
    public bool OnlyKlaus = false;
    public AudioSource audio;

    private void Start()
    {
        if(OnlyKlaus)
        {
            audio = this.gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
            audio.enabled = false;
        }
    }

    protected override void OnEnterAction(Collider2D other)
    {
        if(!OnlyKlaus)
        { 
            base.OnEnterAction(other);
            CharacterManager.Instance.FreezeAllWithTimer(TimeStop);
            if (!K1)
            {
                DynamicCameraManager.Instance.ChangueEspecialTargetForKlaus(Spot,TimeToReachSpot, newZoom, TimeToReachThatZoom);
            }
            else
            {
                DynamicCameraManager.Instance.ChangueEspecialTargetForK1(Spot, TimeToReachSpot, newZoom, TimeToReachThatZoom);
            }
            StartCoroutine("StopWatchSpot", TimeToReachSpot + TimeStop);
        }

        if (OnlyKlaus)
        {
            GameObject Klaus = other.gameObject;
            if (Klaus.name=="Klaus")
            {
                audio.enabled = true;
                Debug.Log("I entered the collision");
                base.OnEnterAction(other);
                CharacterManager.Instance.FreezeAllWithTimer(TimeStop);

                DynamicCameraManager.Instance.ChangueEspecialTargetForKlaus(Spot, TimeToReachSpot, newZoom, TimeToReachThatZoom);
            
          
                StartCoroutine("StopWatchSpot", TimeToReachSpot + TimeStop);

                BoxCollider2D trigger = this.gameObject.GetComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                trigger.enabled = false;
            }
        }



    }

    public IEnumerator StopWatchSpot(float timeFreeze)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timeFreeze));
        if (!K1)
        {
            DynamicCameraManager.Instance.RemoveEspecialTargetForKlaus();
        }
        else
        {

            DynamicCameraManager.Instance.RemoveEspecialTargetForK1();
        }

    }
    /*
    protected override void OnExitAction(Collider2D other)
    {
        DynamicCameraManager.Instance.RemoveEspecialTargetForKlaus();
    }*/
}
