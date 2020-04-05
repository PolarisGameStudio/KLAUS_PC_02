using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class AI_ExplodeTrigger : MonoBehaviour
{
    protected bool isExplode = false;
    public AI_Killeable kill;
    public SpriteRenderer sprite;
    public float TimeToKillSelf = 0.1f;
    public float TimeToKillAnyPlayerCerca = 0.1f;
    List<MoveState> especialRequest = new List<MoveState>();
    protected Color StartColor;
    public Color EndColor;
    public float TimeChangueColor = 0.1f;
    Tweener colorTween;

    public LookToPlayer finderPlayer;
    public LayerMask whatsKill;
    public float DistanceExplode = 3.0f;
    public ParticleSystem humoPar;

    [Header("Audio")]
    //public double frequency = 1320;
    //public double gain = 0.2;
    //public bool isMuteLoop;
    public AudioSource audio1;
    //public float delay = 0.4f;
    //public float divisor = 7;
    //public bool isLoop;
    //private double increment;
    //private double phase;
    //private double sampling_frequency = 48000;

    /*void OnAudioFilterRead(float[] data, int channels)
	{
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (int i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}
	IEnumerator BeepLoop()
	{	
		isLoop = true;
		while(isLoop)
		{
			audio1.mute = !audio1.mute;
			if(delay>0)
			{
				delay -= (delay/divisor);
			}
			yield return new WaitForSeconds(delay);
		}
	}*/
    void Awake()
    {
        StartColor = sprite.color;
        colorTween = sprite.DOColor(EndColor, TimeChangueColor).SetLoops(2, LoopType.Yoyo);
        colorTween.Pause();
    }

    void OnEnable()
    {
        humoPar.Play();
    }

    void OnDisable()
    {
        StopCoroutine("StartExplode");
        /*StopCoroutine ("BeepLoop");
		audio1.mute = true;
		isLoop = false;
		delay = 0.6f;*/
        finderPlayer.ResetIsPlayerNear(false);
        colorTween.Pause();
        isExplode = false;
        isInstantKill = false;
        sprite.color = StartColor;
    }

    public void Explode()
    {

        if (!isExplode)
        {
            isExplode = true;
            StartCoroutine("StartExplode");
            //StartCoroutine ("BeepLoop");
        }
    }

    bool isInstantKill = false;

    public void Explode(MoveState Target)
    {
        if (isInstantKill)
            return;

        if (!especialRequest.Contains(Target))
            especialRequest.Add(Target);

        InstantKill();

    }

    IEnumerator StartExplode()
    {
        float duration = TimeToKillSelf;
        while (duration > 0) //check time and listen for keypress
        {
            if (!ManagerStop.Stop && !ManagerPause.Pause)
            {
                duration -= Time.deltaTime; //deduce time passed this frame.

                if (!colorTween.IsPlaying())
                {
                    colorTween.Kill();
                    colorTween = sprite.DOColor(EndColor, TimeChangueColor * (duration / TimeToKillSelf)).SetLoops(2, LoopType.Yoyo);
                    colorTween.PlayForward();
                    audio1.Play();
                }
            }

            yield return null; //yield for one(1) frame.
        }
        //StopCoroutine ("BeepLoop");
        //audio1.mute = true;
        //isLoop = false;
        //delay = 0.4f;
        colorTween.Pause();
        humoPar.Stop();
        KillSelf();
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(TimeToKillAnyPlayerCerca));
        KillAll();

    }

    void KillSelf()
    {
        sprite.color = StartColor;
        kill.Kill();

    }

    void KillAll()
    {
        for (int i = 0; i < especialRequest.Count; i++)
        {
            if (especialRequest[i] != null)
                especialRequest[i].Kill();
        }
        //Aqui cheque lso objetos Icrush que esten alrededor
        Collider2D[] result = new Collider2D[10];
        bool killer = Physics2D.OverlapCircleNonAlloc(transform.position, DistanceExplode, result, whatsKill) > 0;
        if (killer)
        {
            for (int i = 0; i < result.Length; ++i)
            {
                if (result[i] != null)
                {
                    if (result[i].CompareTag("Player"))
                    {
                        result[i].GetComponent<MoveState>().Kill();
                    }
                    else
                    {

                        ICrushObject objectCr = result[i].GetComponent<ICrushObject>();
                        if (objectCr != null)
                        {
                            if (objectCr != kill)
                            {
                                objectCr.Crush(TypeCrush.NONE);
                            }
                        }
                    }
                }
            }
        }
        especialRequest.Clear();
    }

    void InstantKill()
    {
        StopCoroutine("StartExplode");
        isInstantKill = true;
        isExplode = true;
        humoPar.Stop();
        KillSelf();
        KillAll();
    }

    void LateUpdate()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop && !isExplode)
        {
            if (finderPlayer.isPlayerNear)
            {
                Explode();
            }
        }
    }
}
