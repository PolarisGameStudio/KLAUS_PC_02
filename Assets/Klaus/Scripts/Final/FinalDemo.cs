using UnityEngine;
using System.Collections;

public class FinalDemo : MonoBehaviour {

    public InputController Klaus;
    public GameObject floatingMusik;
	public GameObject musikFinal;
	public GameObject ambient;
	public GameObject industry1;
	public GameObject industry2;
	public GameObject industry3;
    public bool isEnter = false;
    protected ChangueLevelFade cha;
    public float changueLevel = 4.0f;

    public CameraFollow cam;
    public Vector3 PosTarget;

    private float dampTime = 3.15f;
    private Vector3 velocity = Vector3.zero;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnter)
        {
            if(other.CompareTag("Player")){
                isEnter = true;
                cam.enabled = false;
               // dampTime =  cam.dampTime;
                cam.GetComponent<CameraMovement>().enabled = false;
                Klaus.GetComponent<MoveStateKlaus>().Stop();
                Klaus.enabled = false;
                CharacterManager.Instance.enabled = false;
                Invoke("StartFloating", 0.3f);
                Invoke("ChangueScene", changueLevel);
				musikFinal = GameObject.Find("AS_MusikPreFinal(Clone)");
             
				//musikFinal.audio.volume =0;
				ambient = GameObject.Find("AS_AmbientN01");
				//ambient.audio.volume=0;
            }
        }

    }

    void StartFloating()
    {
       Instantiate(floatingMusik, transform.position, transform.rotation);
       if (industry1 != null)
       {
           industry1.GetComponent<AudioSource>().volume = 0;
           industry2.GetComponent<AudioSource>().volume = 0;
           industry3.GetComponent<AudioSource>().volume = 0;
       }
		//AudioListener.volume=0;

    }
    void ChangueScene()
    {
		//AudioListener.volume=1;
        LoadScene load = gameObject.AddComponent<LoadScene>();
        load.Load("LogoFinalKlaus");
    }
    void Update()
    {

        if (musikFinal != null && musikFinal.GetComponent<AudioSource>().volume > 0 && isEnter)
        {
            musikFinal.GetComponent<AudioSource>().volume -= 0.01f;
        }

        if (ambient != null && ambient.GetComponent<AudioSource>().volume > 0 && isEnter)
        {
            ambient.GetComponent<AudioSource>().volume -= 0.005f;
        }
    }

    void FixedUpdate()
    {
        if (isEnter)
        {
            cam.GetComponent<Rigidbody2D>().velocity = velocity;
          //  cam.transform.position = Vector3.Lerp(cam.transform.position,PosTarget,Time.deltaTime);
        }
    }
    void LateUpdate()
    {
        if (isEnter)
        {
    
            Vector3.SmoothDamp(cam.transform.position, PosTarget, ref velocity, dampTime);
        }

    }
}
