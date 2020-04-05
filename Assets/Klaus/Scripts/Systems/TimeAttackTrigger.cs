using UnityEngine;

public class TimeAttackTrigger : MonoBehaviour
{
    public bool isGoal = true;
    public AudioSource audio1;
    public Collider2D collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<Collider2D>();
            return _collider;
        }
    }

    Collider2D _collider;

    void Awake()
    {
        if (TimeAttackSystem.Instance == null)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isGoal && other.CompareTag("Player"))
            TimeAttackSystem.Instance.ShowHud();
        audio1.Play();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!TimeAttackSystem.Instance.timerRunning && !isGoal)
        {
            TimeAttackSystem.Instance.PlayTimer();
        }
        else if (TimeAttackSystem.Instance.timerRunning && isGoal)
        {
            TimeAttackSystem.Instance.StopTimer();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isGoal && other.CompareTag("Player") && !TimeAttackSystem.Instance.timerRunning)
            TimeAttackSystem.Instance.HideHud();
    }
}
