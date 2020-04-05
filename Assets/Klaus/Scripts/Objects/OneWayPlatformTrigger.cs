using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneWayPlatformTrigger : MonoBehaviour
{

    public Collider2D colliderOneWay;

    Dictionary<int, int> isIn = new Dictionary<int, int>();

    bool EstoyAbajo(MoveState State)
    {
        return State.groundCheck.position.y <= transform.position.y;
    }

    bool EstoyArriba(MoveState State)
    {
        return State.groundCheck.position.y >= transform.position.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!isIn.ContainsKey(id))
            {
                isIn [id] = 0;
            }
            isIn [id] += 1;
            if (isIn [id] == 1)
            {
                bool isKlaus = other.name.CompareTo("Klaus") == 0;
                bool needDown;
                if (isKlaus)
                {
                    needDown = OneWaySingleton.Instance.GetNeedDownKlaus(colliderOneWay);
                } else
                {
                    needDown = OneWaySingleton.Instance.GetNeedDownK1(colliderOneWay);

                }
                if (!needDown)
                {
                    MoveState State = other.GetComponent<MoveState>();

                    if (EstoyAbajo(State))
                    {
                        if (!Physics2D.GetIgnoreCollision(colliderOneWay, State.colliders [0]))
                        {
                            for (int i = 0; i < State.colliders.Length; ++i)
                            {
                                Physics2D.IgnoreCollision(colliderOneWay, State.colliders [i], true);
                            }
                            if (other.name.CompareTo("Klaus") == 0)
                            {
                                OneWaySingleton.Instance.SetIsKlausOneWay(colliderOneWay, true);
                            } else if (other.name.CompareTo("K1") == 0)
                            {
                                OneWaySingleton.Instance.SetIsK1OneWay(colliderOneWay, true);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MoveState State = other.GetComponent<MoveState>();
            bool isKlaus = other.name.CompareTo("Klaus") == 0;
            bool needDown;
            if (isKlaus)
            {
                needDown = OneWaySingleton.Instance.GetNeedDownKlaus(colliderOneWay);
            } else
            {
                needDown = OneWaySingleton.Instance.GetNeedDownK1(colliderOneWay);

            }
            if (EstoyArriba(State) && !needDown)
            {

                if (Physics2D.GetIgnoreCollision(colliderOneWay, State.colliders [0]))
                {
                    if (State._rigidbody2D.velocity.y < 0)
                    {

                        for (int i = 0; i < State.colliders.Length; ++i)
                        {
                            Physics2D.IgnoreCollision(colliderOneWay, State.colliders [i], false);
                        }
                        if (other.name.CompareTo("Klaus") == 0)
                        {
                            OneWaySingleton.Instance.SetIsKlausOneWay(colliderOneWay, false);

                        } else if (other.name.CompareTo("K1") == 0)
                        {
                            OneWaySingleton.Instance.SetIsK1OneWay(colliderOneWay, false);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            isIn [id] -= 1;
            if (isIn [id] <= 0)
            {

                MoveState State = other.GetComponent<MoveState>();

                for (int i = 0; i < State.colliders.Length; ++i)
                {
                    Physics2D.IgnoreCollision(colliderOneWay, State.colliders [i], false);
                }
                if (other.name.CompareTo("Klaus") == 0)
                {
                    OneWaySingleton.Instance.SetNeedDownKlaus(colliderOneWay, false);
                    OneWaySingleton.Instance.SetIsKlausOneWay(colliderOneWay, false);

                } else if (other.name.CompareTo("K1") == 0)
                {
                    OneWaySingleton.Instance.SetNeedDownK1(colliderOneWay, false);
                    OneWaySingleton.Instance.SetIsK1OneWay(colliderOneWay, false);
                }
                isIn [id] = 0;
            }
        }
    }


}
