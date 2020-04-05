using UnityEngine;
using System.Collections;
//Esta clase tiene q ser abstracta pero el 4.3 no lo soporta
abstract public class KillObject : MonoBehaviour
{


    abstract public void Kill();
}
