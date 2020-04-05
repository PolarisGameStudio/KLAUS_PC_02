using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour
{

    public PlayersID playerType;
    public MoveState move;

    public const float timeToPushMove = 2.0f;
    /*
    void OnEnable()
    {
        StartCoroutine("PushMove", timeToPushMove);
    }

    void OnDisable()
    {
        StopCoroutine("PushMove");
    }

    IEnumerator PushMove(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));

        ManagerAnalytics.CharacterMove(Application.loadedLevelName,
                             Application.loadedLevelName,
                             SaveManager.Instance.comingFromTimeArcadeMode,
                             gameObject.name,
                             transform.position.x,
                             transform.position.y,
                             move.canRun && move.enabled);
        StartCoroutine("PushMove", timeToPushMove);
    }*/
}
