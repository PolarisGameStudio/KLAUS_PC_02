using UnityEngine;
using System.Collections;

public class KillLink : KillObject {

    public KillObject killObj;

    public override void Kill()
    {
        killObj.Kill();
    }
}
