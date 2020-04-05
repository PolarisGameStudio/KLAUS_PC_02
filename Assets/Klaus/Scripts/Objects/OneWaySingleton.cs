using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneWaySingleton : Singleton<OneWaySingleton>
{

    protected float TimeToNotOneWay = 0.25f;


    Dictionary<Collider2D,bool> NeedDownKlaus = new Dictionary<Collider2D,bool>();
    Dictionary<Collider2D,bool> NeedDownK1 = new Dictionary<Collider2D,bool>();

    public bool GetNeedDownKlaus(Collider2D other)
    {
        if (!NeedDownKlaus.ContainsKey(other))
        {
            NeedDownKlaus [other] = false;
        }
        return NeedDownKlaus [other];
    }

    public bool GetNeedDownK1(Collider2D other)
    {
        if (!NeedDownK1.ContainsKey(other))
        {
            NeedDownK1 [other] = false;
        }
        return NeedDownK1 [other];
    }

    public void SetNeedDown(Collider2D other, string klausname, bool value)
    {
        if (klausname == "Klaus")
        {
            if (!NeedDownKlaus.ContainsKey(other))
            {
                NeedDownKlaus [other] = value;
            }
            NeedDownKlaus [other] = value;
        } else
        {
            if (!NeedDownK1.ContainsKey(other))
            {
                NeedDownK1 [other] = value;
            }
            NeedDownK1 [other] = value;
        }
    }

    public void SetNeedDownKlaus(Collider2D other, bool value)
    {
        if (!NeedDownKlaus.ContainsKey(other))
        {
            NeedDownKlaus [other] = value;
        }
        NeedDownKlaus [other] = value;
    }

    public void SetNeedDownK1(Collider2D other, bool value)
    {
        if (!NeedDownK1.ContainsKey(other))
        {
            NeedDownK1 [other] = value;
        }
        NeedDownK1 [other] = value;
    }

    Dictionary<Collider2D,int> counterKlaus = new Dictionary<Collider2D,int>();
    Dictionary<Collider2D,Coroutine> CoroutineKlaus = new Dictionary<Collider2D,Coroutine>();

    //protected int counterKlaus = 0;
    public void  SetIsKlausOneWay(Collider2D other, bool value)
    {
        if (!counterKlaus.ContainsKey(other))
        {
            counterKlaus [other] = 0;
            CoroutineKlaus [other] = null;
        }
        if (value)
        {
            ++counterKlaus [other];
            if (CoroutineKlaus [other] != null)
                StopCoroutine(CoroutineKlaus [other]);
        } else
        {
            --counterKlaus [other];
            if (counterKlaus [other] <= 0)
            {
                counterKlaus [other] = 1;
                CoroutineKlaus [other] = StartCoroutine(ResetIsKlaus(TimeToNotOneWay, other));
            }

        }

    }

    public bool  GetIsKlausOneWay(Collider2D other)
    {
        if (!counterKlaus.ContainsKey(other))
        {
            counterKlaus [other] = 0;
            CoroutineKlaus [other] = null;
        }
        return  counterKlaus [other] > 0;
    }

    Dictionary<Collider2D,int> counterK1 = new Dictionary<Collider2D,int>();
    Dictionary<Collider2D,Coroutine> CoroutineK1 = new Dictionary<Collider2D,Coroutine>();

    //protected int counterKlaus = 0;
    public void  SetIsK1OneWay(Collider2D other, bool value)
    {
        if (!counterK1.ContainsKey(other))
        {
            counterK1 [other] = 0;
            CoroutineK1 [other] = null;
        }
        if (value)
        {
            ++counterK1 [other];
            if (CoroutineK1 [other] != null)
                StopCoroutine(CoroutineK1 [other]);
        } else
        {
            --counterK1 [other];
            if (counterK1 [other] <= 0)
            {
                counterK1 [other] = 1;
                CoroutineK1 [other] = StartCoroutine(ResetIsK1(TimeToNotOneWay, other));
            }

        }

    }

    public bool  GetIsK1OneWay(Collider2D other)
    {
        if (!counterK1.ContainsKey(other))
        {
            counterK1 [other] = 0;
            CoroutineK1 [other] = null;
        }
        return  counterK1 [other] > 0;
    }






    IEnumerator ResetIsKlaus(float timer, Collider2D other)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timer));
        counterKlaus [other] = 0;
    }

    IEnumerator ResetIsK1(float timer, Collider2D other)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timer));
        counterK1 [other] = 0;

    }
}
