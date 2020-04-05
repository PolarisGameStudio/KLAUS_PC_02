using UnityEngine;
using System.Collections;

public class AngryKlausTrigger : MonoBehaviour
{
    bool isIn = false;
    public PlayersID TypePlayer = PlayersID.Player1Klaus;

    MoveStateKlaus move;
    CharacterInputController character;
    ChangeSpriteInAnimation spriteAngry;
    Animator anim;

    public float TimeToAngry = 2.5f;
    protected float PerCentTimeCenterKlaus = 0.9f;
    public float ZoomForKlaus = 1.0f;
    [Range(0, 1)]
    public float PercentTimeToZoomKlaus = 1.0f;
    public float TimeZoomBack = 0.2f;
    public float TimeToBackK1 = 0.8f;

    public Vector2 DirMove = new Vector2(1, 0);
    public float SmadeDirPercentPlatofmr = 0.2f;
    protected float currentZoom = 0;

    public AudioSource sfx1;
    public AudioSource sfx2;
    void ManageIn(Collider2D other)
    {
        if (!isIn && !CharacterManager.Instance.firstRun)
        {
            if (CompareDefinition(other))
            {
                isIn = true;
                spriteAngry = other.GetComponentInChildren<ChangeSpriteInAnimation>();

                if (spriteAngry.isActive)
                    return;
                AddCallbackRestorte();

                character = other.GetComponent<CharacterInputController>();
                move = other.GetComponent<MoveStateKlaus>();
                anim = other.GetComponentInChildren<Animator>();
                character.isBlock = true;
                character.enabled = false;
                move.BlockThrow = true;
                character.SetNoInput();
                CameraFollow.Instance.ChangueTargetOnly(other.transform, (TimeToAngry + TimeToBackK1));
                currentZoom = DynamicCameraManager.Instance.ZoomKlaus;
                DynamicCameraManager.Instance.ChangueZoomToKlaus(ZoomForKlaus, TimeToAngry * PercentTimeToZoomKlaus);
                anim.SetBool("Angry", true);
                if (sfx1 != null && sfx2 != null)
                {
                    sfx1.Play();
                    sfx2.Play();
                }
                StartCoroutine("AngryControllerActive");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ManageIn(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        ManageIn(other);

    }

    IEnumerator AngryControllerActive()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToAngry));
        DynamicCameraManager.Instance.ChangueZoomToKlaus(currentZoom, TimeZoomBack);
        spriteAngry.isActive = true;
        anim.SetBool("Angry", false);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToBackK1));
        DynamicCameraManager.Instance.RemoveEspecialZoomForKlaus();

        //Aqui empieza la logica para controlar la AI
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.01f));
        move.percentPlatformMoveSameDir = SmadeDirPercentPlatofmr;
        while (true)
        {
            character.enabled = false;
            move.SetMovement(DirMove);
            yield return null;
        }


    }

    void Callback(Vector2 dir)
    {
        if (dir.x > 0)
            DirMove = new Vector2(1, 0);
        else if (dir.x < 0)
            DirMove = new Vector2(-1, 0);

        //    DirMove.x = dir.x;
    }

    void AddCallbackRestorte()
    {
        ResorteTrigger[] triggers = GameObject.FindObjectsOfType<ResorteTrigger>();
        for (int i = 0; i < triggers.Length; ++i)
        {
            triggers[i].ForceCallback = Callback;
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && other.GetComponent<PlayerInfo>().playerType == TypePlayer;
    }
}
