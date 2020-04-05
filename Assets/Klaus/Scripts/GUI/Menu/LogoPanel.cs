using UnityEngine;
using System.Collections;

public class LogoPanel : MonoBehaviour
{
    public Animator LogoMenu;

    // Use this for initialization
    public void ChangueOut()
    {
        LogoMenu.SetTrigger("Out");
    }


    public void ChangueNextMenu()
    {
        ManagerMenuUI.Instance.ChangueToStartMenu();
    }
}
