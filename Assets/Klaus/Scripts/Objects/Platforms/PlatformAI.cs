using UnityEngine;
using System.Collections;

public class PlatformAI : MonoBehaviour
{
    protected const float MagicConversionSpeed = 100.0f;
    protected const float minDistToReachPoint = 0.15f;
    public float maxSpeed = 3.0f;
    //public float pitch;
    public GameObject platStartSFX;

    protected virtual float getSpeed()
    {
        return maxSpeed;
    }

    public float maxPitch;
    //public float maxVol;
    public Transform[] Path;
    public float[] TimeStop;
    public float DefaultTimeStop = 0.1f;

    protected bool played = false;
    //public bool adentro;
    [HideInInspector]
    public bool visible;
    protected int currentPath = 0;
    protected Vector3 dir = Vector3.zero;
    bool isDeActive = false;
    bool firstRun = true;
    public bool useCircular = false;
    AudioSource _audio = null;

    public AudioSource audio
    {
		
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    Rigidbody2D rgb2D = null;

    public Rigidbody2D rigidbody2D
    {
		
        get
        {
            if (rgb2D == null)
                rgb2D = GetComponent<Rigidbody2D>();
            return rgb2D;
        }
    }


    protected virtual void Awake()
    {
        platStartSFX.CreatePool(10);
    }

    protected virtual void OnEnable()
    {

        if (Path.Length == 0)
        {
        
            Debug.Log("Este no tiene un path especificado");
            enabled = false;
            return;
        }

        Vector2 remainDistance = Path [currentPath].position - transform.position;
		
        if (remainDistance.magnitude <= minDistToReachPoint)
        {
            ChangeSpot();
        } else
        {

            StartCoroutine("ResumeMovement", (0.0f));
        }

    }

    protected virtual void OnDisable()
    {
        firstRun = true;
        StopPlatform();
        audio.Stop();

    }

    public void DeActivePlatform()
    {
        isDeActive = true;
    }

    public void ResetPlatform(int spotIndex = 0)
    {
        currentPath = 0;
        played = false;
        isDeActive = false;
        firstRun = true;
        isGoing = true;

        StopPlatform();

        transform.position = Path[currentPath].position;
    }

    /// <summary>
    /// Para la plataforma par aq no se mueva mas.
    /// </summary>
    protected void StopPlatform()
    {
        StopAllCoroutines();
        dir = Vector2.zero;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = true;

    }

    void MinosCurrentPath()
    {
        --currentPath;
        if (currentPath < 0)
        {
            if (useCircular)
            {
                currentPath = Path.Length - 1;
                isGoing = false;
            } else
            {
                currentPath = 0;
                isGoing = true;
            }
            if (isDeActive)
            {
                StopPlatform();
                enabled = false;

            }
        }
    }

    protected virtual void ChangeSpot()
    {
        if (isGoing)
        {
            if (isDeActive)
            {
                MinosCurrentPath();
            } else
            {
               
                ++currentPath;
                if (currentPath >= Path.Length)
                {
                    if (useCircular)
                    {
                        currentPath = 0;
                        isGoing = true;
                    } else
                    {
                        --currentPath;
                        isGoing = false;
                    }
                   
                }  

             
            }
        } else
        {
            MinosCurrentPath();
        }
        if (enabled)
        {
            if (TimeStop.Length > currentPath)
            {
                StartCoroutine("ResumeMovement", TimeStop [currentPath]);
            } else
            {
                StartCoroutine("ResumeMovement", DefaultTimeStop);

            }
        }
    }

    protected virtual void FixedUpdate()
    {

        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            Vector2 remainDistance = Path [currentPath].position - transform.position;

            if (remainDistance.magnitude <= minDistToReachPoint)
                ChangeSpot();

            rigidbody2D.velocity = dir * getSpeed();
        } else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
          
    }

    protected bool isGoing = true;

    IEnumerator ResumeMovement(float time)
    {
        rigidbody2D.isKinematic = true;
        dir = Vector2.zero;
        if (platStartSFX)
        {
            platStartSFX.Spawn(transform.position, transform.rotation);
        }
        if (!firstRun)
        {

            //StartCoroutine("PitchDown");
        } else
        {
		
            firstRun = false;
        }
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        rigidbody2D.isKinematic = false;
        dir = (Path [currentPath].position - transform.position).normalized;
        rigidbody2D.velocity = dir * getSpeed();
        if (!played)
        {
            audio.Play();
            StopCoroutine("PitchUp");
            StartCoroutine("PitchUp");
            played = true;
        }
     

    }

    IEnumerator PitchUp()
    {
        for (float f = 0.5f; f < maxPitch; f += 0.015f)
        {
            audio.pitch = f;
            yield return null;
        }
    }

    IEnumerator PitchDown()
    {
        for (float g = 1f; g > 0.5f; g -= 0.03f)//baja pitch
        {
            audio.pitch = g;
            yield return null;
            StopCoroutine("PitchUp");
            StartCoroutine("PitchUp");
			
        }
		
    }

}
