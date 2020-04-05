using UnityEngine;
using System.Collections;

public class KlausCrazyTrigger : MonoBehaviour
{
    public PlayersID TypePlayer = PlayersID.Player1Klaus;
    public bool BlockThrow = true;
    public bool SwapJumpWithAction = true;
    public bool UserInverseInput = true;
    public bool SelectAllCharacter = false;

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
    public float TimeToBackToControl = 0.8f;

    bool isIn = false;
    float currentZoom = 0;

	public AudioSource sfx1;
	public AudioSource sfx2;
    void ManageIn(Collider2D other)
    {
        if (!isIn && !CharacterManager.Instance.firstRun)
        {
            if (CompareDefinition(other))
            {
                character = other.GetComponent<CharacterInputController>();
                spriteAngry = other.GetComponentInChildren<ChangeSpriteInAnimation>();
                move = other.GetComponent<MoveStateKlaus>();
                anim = other.GetComponentInChildren<Animator>();

                move.BlockThrow = true;
                character.SetNoInput();
                CameraFollow.Instance.ChangueTargetOnly(other.transform, (TimeToAngry + TimeToBackToControl));
                currentZoom = DynamicCameraManager.Instance.ZoomKlaus;
                DynamicCameraManager.Instance.ChangueZoomToKlaus(ZoomForKlaus, TimeToAngry * PercentTimeToZoomKlaus);
                anim.SetBool("Angry", true);
				if (sfx1 != null && sfx2 != null)
				{
					sfx1.Play ();
					sfx2.Play ();
				}
                StartCoroutine("CrazyControllerActive");

                isIn = true;
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

    IEnumerator CrazyControllerActive()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToAngry));
        DynamicCameraManager.Instance.ChangueZoomToKlaus(currentZoom, TimeZoomBack);
        spriteAngry.isActive = true;
        anim.SetBool("Angry", false);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToBackToControl));
        DynamicCameraManager.Instance.RemoveEspecialZoomForKlaus();

        move.codeState.SwapActionWithJump(move.SwapActionWithJump(SwapJumpWithAction));
        move.BlockThrow = BlockThrow;
        character.useInverseInput = UserInverseInput;
        if (SelectAllCharacter)
        {
            CharacterManager.Instance.SelectAllPermanent();
        }


    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && other.GetComponent<PlayerInfo>().playerType == TypePlayer;
    }


}
