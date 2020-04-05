using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
public class InputSelectionPlatform : Input3DSelection {

    public TapGesture tap;
    public PlatformInputController controller;
	public GameObject platTouchSFX;
	public GameObject platTargetSFX;
	// Use this for initialization
	void Awake () {
    //    tap.Tapped += Select;
	}

    protected  void Select(object sender, System.EventArgs e)
    {
        ManagerPlatform.Instance.SelectPlatform(controller.GetInstanceID());
        {
            if (platTouchSFX != null && platTargetSFX != null)
            {
                platTouchSFX.Spawn(transform.position, transform.rotation);
                platTargetSFX.Spawn(transform.position, transform.rotation);
            }
        }
    }

    public override void SelectByInput()
    {
        if (ManagerPlatform.Instance.SelectPlatform(controller.GetInstanceID()))
        {
            if (platTouchSFX != null && platTargetSFX != null)
            {
                platTouchSFX.Spawn(transform.position, transform.rotation);
                platTargetSFX.Spawn(transform.position, transform.rotation);
            }
        }
    }
}
