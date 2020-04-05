using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD_CollectedPieces : MonoBehaviour
{
    public CanvasGroup canvasGroup
    {
        get
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;
        }
    }

    public Text text
    {
        get
        {
            if (_text == null)
                _text = GetComponentInChildren<Text>();
            return _text;
        }
    }

    public GameObject[] sprites;

    CanvasGroup _canvasGroup;
    Text _text;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name.Split('-')[0];
        int total = CollectablesManager.GetTotalPieces(sceneName);

        if (total != 0)
            text.text = CollectablesManager.GetCollectedPieces(sceneName) + "/" + total;

        canvasGroup.alpha = total != 0 ? 1 : 0;

        int worldIndex = Mathf.Clamp((int)char.GetNumericValue(sceneName[1]) - 1, 0, sprites.Length - 1);
        for (int i = 0; i != sprites.Length; ++i)
            sprites[i].SetActive(i == worldIndex);
    }
}
