using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AngryKlausDoorTrigger : MonoBehaviour
{
    bool isIn = false;
    public PlayersID TypePlayer = PlayersID.Player1Klaus;
    public float TimeToDissmisKlaus = 0.7f;
    protected MoveState move;
    public bool ChangeLevel = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isIn)
        {
            if (CompareDefinition(other))
            {
                isIn = true;
                move = other.GetComponentInChildren<MoveState>();
                move.isEnter = true;
                StartCoroutine("DismissCharacter");

                if (ChangeLevel)
                {

                    EnterDoorManager[] doorsEnter = GameObject.FindObjectsOfType<EnterDoorManager>();

                    for (int i = 0; i < doorsEnter.Length; ++i)
                    {
                        if (doorsEnter[i] != null)
                        {
                            if (!doorsEnter[i].ItsAnotherDoor)
                            {
                                doorsEnter[i].ForceChange();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DismissCharacter()
    {
        move.m_Flip.sprite.DOColor(new Color(0, 0, 0, 0), TimeToDissmisKlaus);
        yield return new WaitForSeconds(TimeToDissmisKlaus);

        move.gameObject.SetActive(false);
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && other.GetComponent<PlayerInfo>().playerType == TypePlayer;
    }
}
