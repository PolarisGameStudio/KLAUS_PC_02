using UnityEngine;
using System.Collections;
using System;
using Luminosity.IO;
using Rewired;

public class CharacterManager : Singleton<CharacterManager>
{

    InputActionOld ButtonChange = InputActionOld.Change_Character;
    InputActionOld ButtonSelectAll = InputActionOld.Select_All;
    public bool blockSelecctAll = false;
    public bool blockChangeCharacter = false;
    public CharacterInputController[] charsInput { get; protected set; }

    MoveState[] moveStateChars;

    GameObject[] arrows;

    public bool[] canMove { get; protected set; }

    public int characterCanMove
    {
        get
        {
            int aux = 0;
            for (int i = 0; i < canMove.Length; ++i)
            {
                if (canMove[i])
                    ++aux;
            }
            return aux;
        }
    }

    int currentSelected = -1;
    public Transform CurrentCharacter
    {
        get
        {
            return moveStateChars[currentSelected].transform;
        }
    }

    bool isAllSelected = false;

    public Transform Arrow;
    public Vector3 offSetArrow = new Vector3(0, 1.5f, 0);

    public float timeToSelectAll = 2.0f;

    bool canSelectAgain = true;

    public AudioSource audio1;
    public AudioSource audio2;
    public bool firstPlay = false;
    [HideInInspector]
    public bool firstRun = true;

    public PlayersID StartCharacter = PlayersID.Player1Klaus;


    // Use this for initialization
    protected override void AwakeChild()
    {
        GameObject[] allP = GameObject.FindGameObjectsWithTag("Player");
        charsInput = new CharacterInputController[allP.Length];
        moveStateChars = new MoveState[charsInput.Length];

        arrows = new GameObject[allP.Length];
        for (int i = 0; i < allP.Length; ++i)
        {
            charsInput[i] = allP[i].GetComponent<CharacterInputController>();
            charsInput[i].enabled = false;
            charsInput[i].gameObject.GetComponent<AudioListener>().enabled = false;
            moveStateChars[i] = allP[i].GetComponent<MoveState>();
            arrows[i] = Arrow.Spawn().gameObject;
            arrows[i].transform.parent = charsInput[i].transform;
            arrows[i].SetActive(false);
        }
        currentSelected = 0;

        canMove = new bool[charsInput.Length];
        for (int i = 0; i < canMove.Length; ++i)
        {
            SetCanMove(i, true);
        }

    }



    public Action<string, bool> canMoveCharacter;

    void SetCanMove(int pos, bool value)
    {
        canMove[pos] = value;
        if (!object.ReferenceEquals(canMoveCharacter, null))
        {
            canMoveCharacter(charsInput[pos].gameObject.name, value);
        }
    }

    /// <summary>
    /// Set if character can be controlled
    /// </summary>
    /// <param name="value"></param>
    /// <param name="inputChar"></param>
    public void SetPlay(bool value, CharacterInputController inputChar)
    {
        for (int i = 0; i < charsInput.Length; ++i)
        {
            if (charsInput[i] == inputChar)
            {
                SetCanMove(i, value);

                if (!value)
                {
                    if (isAllSelected)
                        NoneSelected();

                    if (currentSelected == i)
                        SelectUpdate();
                }

                break;
            }
        }
    }

    public void BecomeInmortal(bool isInmortal)
    {
        for (int i = 0; i < moveStateChars.Length; ++i)
        {
            moveStateChars[i].Inmortal = isInmortal;
        }
    }
    public void FreezeAll()
    {
        canSelectAgain = false;
        TurnCharsInput(false);
        for (int i = 0; i < moveStateChars.Length; ++i)
        {
            moveStateChars[i].useAllIdle = false;
        }
    }

    public void UnFreezeAll()
    {
        canSelectAgain = true;
        TurnCharsInput(true);
        for (int i = 0; i < moveStateChars.Length; ++i)
        {
            moveStateChars[i].useAllIdle = true;
        }
    }

    void TurnCharsInput(bool value)
    {
        if (isAllSelected)
        {
            for (int i = 0; i < charsInput.Length; ++i)
            {
                if (canMove[i])
                {
                    charsInput[i].enabled = value;
                }
            }
        }
        else
        {
            if (canMove[currentSelected])
            {
                charsInput[currentSelected].enabled = value;
            }
        }
    }

    public void FreezeAllWithTimer(float timeFreeze)
    {
        StopCoroutine("freezeWithTimer");
        StartCoroutine("freezeWithTimer", (timeFreeze));

    }

    protected IEnumerator freezeWithTimer(float timeFreeze)
    {
        FreezeAll();
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timeFreeze));
        UnFreezeAll();

    }

    /*private AudioSource _audio;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }*/

    void PlayCurrentSelected()
    {
        AudioClip currentAudio = moveStateChars[currentSelected].audioClipChange;
        audio1.clip = currentAudio;
        //Klvo mira aqui lo dell currentaudio
    }

    void SelectCurrent()
    {
        if (!charsInput[currentSelected].isBlock)
            charsInput[currentSelected].enabled = true;
        moveStateChars[currentSelected].SetIdleAnimatorSelect();
        PlayCurrentSelected();
        DynamicCameraManager.Instance.ChangueToTarget(charsInput[currentSelected].transform);
        //  CameraFollow.Instance.currentTarget = charsInput[currentSelected].transform;

        if (arrows.Length > 1)
        {
            bool canActiveArrow = true;



            for (int i = 0; i < canMove.Length; ++i)
                {
                    if (i != currentSelected)
                    {
                        canActiveArrow = canMove[i];
                        if (!canActiveArrow)
                            break;
                    }
                }
                if (canActiveArrow)
                {

                if (!blockChangeCharacter)
                {
                    arrows[currentSelected].SetActive(true);
                }
                    arrows[currentSelected].transform.position = charsInput[currentSelected].transform.position + offSetArrow;

                    if (firstPlay)
                    {
                        audio1.Play();
                        audio2.Play();
                    }
                    firstPlay = true;
                }
            
        }


        AudioListener currentKlausSelec = charsInput[currentSelected].gameObject.GetComponent<AudioListener>();
        currentKlausSelec.enabled = true;

    }
    int klausI = -1;
    /// <summary>
    /// Select all character
    /// </summary>
    bool AllSelected(bool checkAlone = false)
    {
        if (checkAlone && charsInput.Length <= 1)
            return false;
        bool canActiveArrow = true;
        for (int i = 0; i < canMove.Length; ++i)
        {
            if (i != currentSelected)
            {
                canActiveArrow = canMove[i];
                if (!canActiveArrow)
                    break;
            }
        }
        isAllSelected = true;
        for (int i = 0; i < charsInput.Length; ++i)
        {
            if (canMove[i])
            {
                if (!charsInput[i].isBlock)
                {
                    charsInput[i].enabled = true;
                }
                PlayerInfo info = charsInput[i].GetComponent<PlayerInfo>();
                if (PlayersID.Player1Klaus == info.playerType)
                {
                    PlayCurrentSelected();
                    DynamicCameraManager.Instance.ChangueToTarget(charsInput[i].transform);
                    klausI = i;
                }

                if (arrows.Length > 1 && canActiveArrow)
                {

                   

                    if(!blockChangeCharacter)
                    {
                        arrows[i].SetActive(true);
                    }
                    arrows[i].transform.position = charsInput[i].transform.position + offSetArrow;
                }
            }
        }
        for (int i = 0; i < charsInput.Length && klausI >= 0; ++i)
        {
            if (i != klausI)
            {
                charsInput[i].GetComponent<MoveState>().AnotherCharacter = charsInput[klausI].GetComponent<MoveState>();

            }
        }
        return true;
    }

    /// <summary>
    /// Select only one Character
    /// </summary>
    void NoneSelected()
    {
        isAllSelected = false;
        for (int i = 0; i < charsInput.Length && klausI >= 0; ++i)
        {
            if (i != klausI)
            {
                charsInput[i].GetComponent<MoveState>().AnotherCharacter = null;
            }
        }
        klausI = -1;
        for (int i = 0; i < charsInput.Length; ++i)
        {
            if (!charsInput[i].isBlock)
            {
                charsInput[i].SetNoInput();
                charsInput[i].enabled = false;
            }
            charsInput[i].gameObject.GetComponent<AudioListener>().enabled = false;
            arrows[i].SetActive(false);
        }

        SelectCurrent();
    }

    /// <summary>
    /// Changue the player in LateUpdate
    /// </summary>
    void SelectUpdate(bool checkAlone = false)
    {
        if (checkAlone && charsInput.Length <= 1)
            return;
        //Antiguo metoo de seleccion
        arrows[currentSelected].SetActive(false);
        if (!charsInput[currentSelected].isBlock)
        {
            charsInput[currentSelected].SetNoInput();
            charsInput[currentSelected].enabled = false;
        }
        charsInput[currentSelected].gameObject.GetComponent<AudioListener>().enabled = false;

        currentSelected = (currentSelected + 1) % charsInput.Length;
        while (!canMove[currentSelected] && canMove.Length > 1)
        {
            currentSelected = (currentSelected + 1) % charsInput.Length;
        }
        SelectCurrent();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (firstRun)
        {
            for (int i = 0; i < canMove.Length; ++i)
            {
                SetCanMove(i, canMove[i]);
            }
            if (charsInput.Length > 1)
            {
                while (!canMove[currentSelected] || charsInput[currentSelected].GetComponent<PlayerInfo>().playerType != StartCharacter)
                {
                    currentSelected = (currentSelected + 1) % charsInput.Length;
                }
            }
            SelectCurrent();
            firstRun = false;
        }
        if (!ManagerPause.Pause && canSelectAgain)
        {
            if (!BlockInput)
            {
                bool isPressed;

                if (InputEnum.GamePad.ToString() == "keyboard")
                {
                    isPressed = ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(ButtonSelectAll));
                }

                else
                {
                    //isPressed = InputEnum.NormalizeTriggerL2(InputManager.GetAxisRaw(InputEnum.GetInputString(ButtonSelectAll))) > InputTouchPS4.AxisTriggerStart;
                    isPressed = ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(ButtonSelectAll));
                }

                  
                if (isPressed && !blockSelecctAll)
                {
                    if (!isAllSelected)
                    {
                        // Hago lso cambios para selecionar todos
                        if (AllSelected(true))
                            isAllSelected = true;

                    }
                }
                else
                {
                    if (isAllSelected)
                    {
                        //Aqui hagos los cambios para seleccionar uno solo
                        NoneSelected();

                    }
                    else
                    {
                        if(!blockChangeCharacter)
                        { 
                            if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(ButtonChange)))
                            {
                                //CAmbio de personake
                                SelectUpdate(true);

                            }
                        }
                    }
                }


            }
        }

    }


    #region SelectAllAlway:

    protected bool BlockInput = false;

    public void SelectAllPermanent()
    {
        BlockInput = true;
        AllSelected();
    }

    public void ResetSelectAllPermanent()
    {
        BlockInput = false;
        NoneSelected();
    }

    #endregion

}
