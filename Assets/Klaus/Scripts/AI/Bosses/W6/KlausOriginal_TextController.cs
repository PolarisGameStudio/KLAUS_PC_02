using UnityEngine;
using System.Collections;
public class KlausOriginal_TextController : MonoBehaviour
{
    public GameObject[] elementsToActivate;

    public Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    public TweenTextShow[] dialogues
    {
        get
        {
            if (_dialogues == null)
            {
                _dialogues = GetComponentsInChildren<TweenTextShow>();
            }
            return _dialogues;
        }
    }

    public TextMesh[] dialoguesMeshes
    {
        get
        {
            if (_dialoguesMeshes == null)
            {
                _dialoguesMeshes = new TextMesh[dialogues.Length];
                for (int i = 0; i != _dialoguesMeshes.Length; ++i)
                    _dialoguesMeshes[i] = dialogues[i].GetComponent<TextMesh>();
            }
            return _dialoguesMeshes;
        }
    }

    public PlatformController platform
    {
        get
        {
            if (_platform == null)
                _platform = GameObject.FindObjectOfType<PlatformController>();
            return _platform;
        }
    }

    PlatformController _platform;
    Animator _animator;
    TweenTextShow[] _dialogues;
    TextMesh[] _dialoguesMeshes;
    int currentDialogue = -1;

    bool bossInPlatform;
    public bool playerInPlatform;
    bool triggerLaunched;

	[Header("Audio")]
	private AudioSource audio1;
	public AudioSource audio2;
	public AudioSource audio3;
	public float maxPitch = 1;
	public float maxVol = 0.3f;
	public float speed;
	public GameObject PlatAiStartSFX;
	public GameObject Plat_AiLoop;

    public GameObject klaus;
    public FlipSprite KlausFlip;


    Transform lastKlausParent;

    void OnEnable()
    {
        platform.playerOnPlatform += PlayerOnPlatform;
        platform.playerOffPlatform += PlayerOffPlatform;
		audio1 = Plat_AiLoop.GetComponent<AudioSource>();
    }

    public void ShowEndDialogue()
    {
        if (currentDialogue == dialogues.Length)
            return;

        currentDialogue = Mathf.Clamp(++currentDialogue, 0, dialogues.Length);
        if(currentDialogue < dialogues.Length){
        dialogues[currentDialogue].InitText();

        for (int i = 0; i != currentDialogue; ++i)
            if (dialoguesMeshes[currentDialogue].font == dialoguesMeshes[i].font)
                dialogues[i].gameObject.SetActive(false);
        }
        if (currentDialogue >= dialogues.Length - 1)
        {
            foreach (GameObject obj in elementsToActivate)
                obj.SetActive(true);

            klaus.transform.SetParent(lastKlausParent);
            CharacterManager.Instance.UnFreezeAll();
        }
    }

    public void HideCurrentDialogue(int id)
    {
        for (int i = 0; i != id + 1; ++i)
            dialogues[i].gameObject.SetActive(false);
    }

    public void PlayerOnPlatform()
    {
        playerInPlatform = true;
        //KlausFlip.facingRight = true;
    }

    public void PlayerOffPlatform()
    {
        playerInPlatform = false;
    }

    public void BossOnPlatform()
    {
        bossInPlatform = true;
    }

    void Update()
    {
        if (bossInPlatform && playerInPlatform && !triggerLaunched)
        {
            triggerLaunched = true;
            CharacterManager.Instance.FreezeAll();
            lastKlausParent = klaus.transform.parent;
            klaus.transform.SetParent(platform.transform);
            animator.SetTrigger("EnablePlatform");
        }
    }

    public void StartFight()
    {
        transform.Find("Sprite").gameObject.SetActive(false);

        GameObject.FindObjectOfType<BossW6Controller>().StartIntro();
		if (AS_W6BossMusik.Instance != null)
		{
			AS_W6BossMusik.Instance.MusikFight();
		}
    }
	public void MusikIntroDown()
	{
		if (AS_W6BossMusik.Instance != null)
		{
			AS_W6BossMusik.Instance.MusikIntroDown();
		}
	}
	public void StartSFX ()
	{
		audio2.Play ();
		audio1.Play ();
		StartCoroutine("PitchUp");
	}
	public void StopSFX()
	{
		audio2.Play ();
		audio1.Stop ();
	}
	public void PitDown()
	{
		StopCoroutine("PitchUp");
		StartCoroutine("PitchDown");
	}
	public void ChainDownSFX()
	{
		audio3.Play ();
		StartCoroutine("VolUp");
	}
	public void ChainSTOPSFX()
	{
		audio3.Stop ();
	}
	public void ChainUpSFX()
	{
		audio3.Play ();
		StartCoroutine("VolDown");
	}
	IEnumerator PitchUp()//platform
	{
		for (float i = 0.5f; i < maxPitch; i += 0.35f*Time.deltaTime)
		{
			audio1.pitch = i;
			yield return null;
		}
	}
	IEnumerator PitchDown()//platform
	{
		for (float i = 1f; i > 0.5f; i -= 0.25f*Time.deltaTime)
		{
			audio1.pitch = i;
			yield return null;
		}
	}
	IEnumerator VolUp()//chain
	{
		for (float i = 0f; i < maxVol; i += 0.25f*Time.deltaTime)
		{
			audio3.volume = i;
			yield return null;
		}
	}
	IEnumerator VolDown()//chain
	{
		for (float i = maxVol; i > 0f; i -= 0.15f*Time.deltaTime)
		{
			audio3.volume = i;
			yield return null;
		}
	}
}
