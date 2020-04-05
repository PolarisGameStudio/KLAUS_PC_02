using UnityEngine;
using System.Collections;

public class InputSelectionPlatformWithTutorial : InputSelectionPlatform
{

    public GameObject Tutorial_1;
    public GameObject Tutorial_2;

    public override void SelectByInput()
    {
        if (ManagerPlatform.Instance.SelectPlatform(controller.GetInstanceID()))
        {
            if (platTouchSFX != null && platTargetSFX != null)
            {
                platTouchSFX.Spawn(transform.position, transform.rotation);
                platTargetSFX.Spawn(transform.position, transform.rotation);
            }

            //Activo tutoial
            if (Tutorial_1 != null)
                Tutorial_1.SetActive(false);
            if (Tutorial_2 != null)
                Tutorial_2.SetActive(true);

        }
    }
}
