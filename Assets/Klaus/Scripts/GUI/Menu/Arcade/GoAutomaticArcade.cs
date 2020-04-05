using UnityEngine;
using System.Collections;

public class GoAutomaticArcade : MonoBehaviour
{
    public StartMenuPanel startmenuPanel;
    public MenuMenuPanel menuMenuPanel;
    public ArcadeMenuPanel arcadeMenuPanel;

    IEnumerator Start()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            if (SaveManager.Instance.isComingFromArcade && SaveManager.Instance.dataKlaus.isArcadeModeUnlock)
            {
                startmenuPanel.SelectFirst();
                startmenuPanel.animator.SetTrigger("QuickStep");
                menuMenuPanel.Reset();
                arcadeMenuPanel.animator.SetTrigger("In");
                menuMenuPanel.ChangueGlitchValue(1);
                arcadeMenuPanel.itemPanel.SelectFirstButton();
                yield return null;
                arcadeMenuPanel.itemPanel.SelectFirstButton();
            }
        }
    }
}
