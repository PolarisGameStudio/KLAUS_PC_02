using UnityEngine;
using System.Collections;
using SmartLocalization.Editor;

public class MessagesTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SaveMessagesManager.Instance.ShowSave();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SaveMessagesManager.Instance.HideSave();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SaveMessagesManager.Instance.ShowMessage("Test");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SaveMessagesManager.Instance.ShowMessage("Important Test", true);
        }
    }
}
