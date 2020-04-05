using UnityEngine;

public class IHack : MonoBehaviour
{
    protected CPUTrigger _cpu;

    public CPUTrigger cpu
    {
        get
        {
            return _cpu;
        }
        set
        {
            _cpu = value;
        }
    }
    // Use this for initialization
    public virtual void HackedSystem()
    {
    }

    public virtual void ResetAll()
    {
    }

}
