using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rewired;

public class KotakuCodeArcade : MonoBehaviour
{

    InputActionOld[] CombinationKeys = { InputActionOld.Action, InputActionOld.Change_Character };
    InputActionOld[] CombinationKeysTrophy = { InputActionOld.Change_Character, InputActionOld.Throw };
    InputActionOld[] CombinationKeysMemories = { InputActionOld.Action, InputActionOld.Throw };

    public Button buttonUnlock;
    public Arcade_PopCheck arcadePopUp;
    public Button[] buttonsUnlocks2;

    public Button[] buttonsUnlocksMemories;


    int currentKey = 0;
    int currentKey2 = 0;
    int currentKey3 = 0;

    public int numberOfTimePress = 3;
    protected int currentNumberOfTimePress = 0;
    protected int currentNumberOfTimePress2 = 0;
    protected int currentNumberOfTimePress3 = 0;

    const float timeToNotPress = 0.3f;

    public bool firstUnlock = false;
    public bool secondUnlock = false;
    public bool thirdUnlock = false;

    // Update is called once per frame
    void Update()
    {

        if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(CombinationKeys[currentKey])) && !firstUnlock)
        {
            ++currentNumberOfTimePress;
            if (currentNumberOfTimePress >= numberOfTimePress)
            {
                ++currentKey;
                if (currentKey >= CombinationKeys.Length)
                {
                    SaveManager.Instance.dataKlaus.isNewGame = false;
                    SaveManager.Instance.dataKlaus.isArcadeModeUnlock = true;
                    buttonUnlock.interactable = true;
                    arcadePopUp.canChangeSection = true;
                    currentKey = 0;
                    firstUnlock = true;
                    Debug.LogError("Unlocked ArcdeMode");
                }
            }
            StopCoroutine("ResetAll");
            StartCoroutine("ResetAll", timeToNotPress);

        }
        if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(CombinationKeysTrophy[currentKey2])) && !secondUnlock)
        {
            ++currentNumberOfTimePress2;
            if (currentNumberOfTimePress2 >= numberOfTimePress)
            {
                ++currentKey2;
                if (currentKey2 >= CombinationKeysTrophy.Length)
                {
                    for (int i = 0; i < buttonsUnlocks2.Length; ++i)
                    {
                        buttonsUnlocks2[i].interactable = true;
                    }
                    for (int i = 1; i <= 6; ++i)
                    {
                        for (int j = 1; j <= 6; ++j)
                        {
                            CollectablesManager.setCollectable("W" + i + "L0" + j);

                        }
                    }
                    Debug.LogError("Unlocked Collectables");

                    currentKey2 = 0;
                    secondUnlock = true;
                }
            }
            StopCoroutine("ResetAll2");
            StartCoroutine("ResetAll2", timeToNotPress);

        }
        if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(CombinationKeysMemories[currentKey3])) && !thirdUnlock)
        {
            ++currentNumberOfTimePress3;
            if (currentNumberOfTimePress3 >= numberOfTimePress)
            {
                ++currentKey3;
                if (currentKey3 >= CombinationKeysMemories.Length)
                {
                    SaveManager.Instance.dataKlaus.collectablesItems[0].item[0] = true;
                    for (int i = 0; i < SaveManager.Instance.dataKlaus.collectablesItems.Count; ++i)
                    {
                        for (int j = 0; j < SaveManager.Instance.dataKlaus.collectablesItems[i].item.Count; ++j)
                        {
                            SaveManager.Instance.dataKlaus.collectablesItems[i].item[j] = true;
                        }
                    }

                    for (int i = 0; i < buttonsUnlocksMemories.Length; ++i)
                    {
                        buttonsUnlocksMemories[i].interactable = true;
                    }

                    currentKey3 = 0;
                    thirdUnlock = true;
                    Debug.LogError("Unlocked Memories");
                }
            }
            StopCoroutine("ResetAll3");
            StartCoroutine("ResetAll3", timeToNotPress);

        }
    }

    IEnumerator ResetAll(float time)
    {
        yield return new WaitForSeconds(time);
        currentKey = 0;
        currentNumberOfTimePress = 0;
    }
    IEnumerator ResetAll2(float time)
    {
        yield return new WaitForSeconds(time);
        currentKey2 = 0;
        currentNumberOfTimePress2 = 0;
    }
    IEnumerator ResetAll3(float time)
    {
        yield return new WaitForSeconds(time);
        currentKey3 = 0;
        currentNumberOfTimePress3 = 0;
    }
}
