using UnityEngine;
using System.Collections;

#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class SpriteTrigger : MonoBehaviour
{
    public TweenGameObject tween;
    bool isShow = false;
    public bool isRepeating;
    public float timeToRepeat;

    public bool ActiveIfIsArcadeMode = false;
    public bool reproduceInControl = true;
    protected virtual void Start()
    {
        if (SaveManager.Instance.comingFromTimeArcadeMode)
        {
            if (ActiveIfIsArcadeMode)
                OnEnterAction();

            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShow && CompareDefinition(other))
            OnEnterAction();
    }

    public virtual void OnEnterAction()
    {
        if (isShow)
            return;

        tween.InitText();
        isShow = true;
        GetComponent<Collider2D>().enabled = false;

        //if(GameObject.Find ("AS_MusikManager_Arrecho")!= null)
        //AS_Target = GameObject.Find ("AS_MusikManager_Arrecho");
        //vol = GameObject.Find ("AS_MusikManager_Arrecho").GetComponent<AudioSource>().volume;

        if (isRepeating)
            InvokeRepeating("suena", 0, timeToRepeat);
        else
            Invoke("suena", 0);
    }

    public virtual void HideAction()
    {
        tween.HideText();
        gameObject.SetActive(false);
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player");
    }

    void suena()
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (audio != null && audio.enabled)
        {

#if UNITY_PS4 && !(UNITY_EDITOR)
            if (reproduceInControl)
            {
                 audio.PlayOnDualShock4(PS4Input.PadGetUsersDetails(0).userId);
                 audio.Play();
            }
            else
            {
                audio.Play();
            }
#else
            audio.Play();
#endif

        }

    }


}
