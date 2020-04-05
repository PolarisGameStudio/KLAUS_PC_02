using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour
{


    protected Dictionary<PlayersID, bool> isUsed = new Dictionary<PlayersID, bool>();

    public SpriteRenderer sprite;
    protected Color baseColor;
    public Color onColor;
    public RotateObject rotate;
    public float timeRotating = 1f;
    public GameObject checkPointSFX;

    public int PositionChase = 0;
    void Start()
    {
        baseColor = sprite.color;

    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<FSMSystem>().CurrentStateID == StateID.Dead)
                return;
            PlayersID id = other.GetComponent<PlayerInfo>().playerType;
            if (!isUsed.ContainsKey(id))
            {
                isUsed[id] = false;
            }
            if (!isUsed[id])
            {

                sprite.color = onColor;

                RotateArrow();
                ManagerCheckPoint.Instance.AddPosition(id, this);
                isUsed[id] = true;
                // enabled = false;
            }
        }
    }
    public void ResetCheckPoint(PlayersID player)
    {

        isUsed[player] = false;
        Dictionary<PlayersID, bool>.ValueCollection pare = isUsed.Values;
        bool changueColor = true;
        foreach (bool s in pare)
        {
            if (s)
            {
                changueColor = false;
                break;
            }

        }
        if (changueColor)
        {
            //     Debug.Log("Cambiar color");

            sprite.color = baseColor;
        }
        //enabled = true;
    }
    public void RotateArrow()
    {
        StopCoroutine("DontRotateArrow");
        rotate.enabled = true;
        if (checkPointSFX && sprite.enabled)
        {
            checkPointSFX.Spawn(transform.position, transform.rotation);
        }
        StartCoroutine("DontRotateArrow", timeRotating);
    }
    IEnumerator DontRotateArrow(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        rotate.enabled = false;

    }
    public virtual void CheckPointUsed() { }
}
