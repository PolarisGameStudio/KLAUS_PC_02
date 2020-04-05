using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UILoadLevelButton : MonoBehaviour
{

    public Text text;

    public void LoadLevel()
    {
        LoadLevelManager.Instance.LoadLevelWithLoadingScene(text.text, false);
    }
}
