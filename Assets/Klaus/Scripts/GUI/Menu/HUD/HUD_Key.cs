using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Key : MonoBehaviour
{
    
    public CanvasGroup canvas;

    public CanvasGroup Klaus;
    public CanvasGroup K1;
    //  public GameObject klausBG;
    //   public GameObject k1BG;

    public Text klausKeys;
    public Text k1Keys;
    //   public Image klausImageKey;
    //   public Image k1ImageKey;
    //   public Image klausImageFace;
    //   public Image k1ImageFace;
    public string pre = "x";

    public Animator KlausKey;
    public Animator K1Key;

    void Start()
    {
        //  CharacterManager.Instance.canMoveCharacter += OnCanMove;
        KeyChain.Instance.onKey += OnSetKey;
        Show();
    }

    void OnCanMove(string name, bool value)
    {
        if (string.Compare(name, "Klaus") == 0)
        {
            if (value)
            {
                ShowKlaus();
            } else
            {
                HideKlaus();
            }
        } else if (string.Compare(name, "K1") == 0)
        {
            if (value)
            {
                ShowK1();
            } else
            {
                HideK1();
            }
        }
    }

    public void Show()
    {
        ShowCharacter(canvas, 1);
    }

    public void Hide()
    {
        ShowCharacter(canvas, 0);
    }

    public void ShowKlaus()
    {
        ShowCharacter(Klaus, 1);
    }

    public void HideKlaus()
    {
        ShowCharacter(Klaus, 0);
    }

    public void ShowK1()
    {
        ShowCharacter(K1, 1);
    }

    public void HideK1()
    {
        ShowCharacter(K1, 0);
    }

    void ShowCharacter(CanvasGroup canvas, float value)
    {
        canvas.alpha = value;
    }



    #region KeysLogic:

    void OnSetKey(PlayersID id, int value)
    {

        if (id == PlayersID.Player1Klaus)
        {
            SetKlausKey(value);
        } else if (id == PlayersID.Player2K1)
        {
            SetK1Key(value);
        }
    }

    public void SetKlausKey(int keys)
    {
        SetKey(klausKeys, keys);
        if (keys > 0)
            ShowKey(true);
        else
            HideKey(true);
    }

    public void SetK1Key(int keys)
    {
        SetKey(k1Keys, keys);
        if (keys > 0)
            ShowKey(false);
        else
            HideKey(false);
    }

    void SetKey(Text text, int keys)
    {
        text.text = pre + keys;
    }

    public void ShowKey(bool klaus)
    {

        if (klaus)
        {
            KlausKey.SetTrigger("Show");

            //    klausImageKey.gameObject.SetActive(true);
            //    klausBG.gameObject.SetActive(true);
            //    klausImageFace.gameObject.SetActive(true);
        } else
        {
            K1Key.SetTrigger("Show");

            // k1ImageKey.gameObject.SetActive(true);
            // k1BG.gameObject.SetActive(true);
            // k1ImageFace.gameObject.SetActive(true);

        }

        //     klausKeys.gameObject.SetActive(true);
        //     k1Keys.gameObject.SetActive(true);
    }

    public void HideKey(bool klaus)
    {
        if (klaus)
        {
            KlausKey.SetTrigger("Hide");
            /*
            klausImageKey.gameObject.SetActive(false);
            klausKeys.gameObject.SetActive(false);
            klausBG.gameObject.SetActive(false);
            klausImageFace.gameObject.SetActive(false);
            */

        } else
        {
            K1Key.SetTrigger("Hide");

            /*
            k1ImageKey.gameObject.SetActive(false);
            k1Keys.gameObject.SetActive(false);
            k1BG.gameObject.SetActive(false);
            k1ImageFace.gameObject.SetActive(false);*/

        }


    }

    #endregion

}
