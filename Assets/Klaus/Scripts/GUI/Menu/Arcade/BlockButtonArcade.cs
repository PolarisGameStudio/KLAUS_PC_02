using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(Button))]
public class BlockButtonArcade : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        if (SaveManager.Instance.comingFromTimeArcadeMode)
        {
            GetComponent<Button>().interactable = false;
        }
    }

}
