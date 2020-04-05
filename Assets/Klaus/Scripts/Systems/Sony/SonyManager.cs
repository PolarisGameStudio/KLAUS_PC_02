using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SonyManager : MonoSingleton<SonyManager>
{
    bool npReady = false;
    bool isConnected = false;

    public Text statusText;
    public Text logText;

    public bool IsConnected
    {
        get
        {
            return
#if UNITY_PS4 && !UNITY_EDITOR
                Sony.NP.System.IsConnected && connectionStatus == Sony.NP.System.EnumConnectionStatus.NET_CTL_STATE_IPOBTAINED; 
#else
                isConnected;
#endif
        }
    }

    public Action onConnectionUp;
    public Action onConnectionDown;

    #region Game Cycle

    public Image[] images;
    public int ageRating = 7;

    protected override void Init()
    {
        base.Init();
        InitializeSystem();
    }

    void Update()
    {
#if UNITY_PS4 && !UNITY_EDITOR
        Main.Update();
#endif

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            isConnected = !isConnected;

            if (isConnected && onConnectionUp != null)
                onConnectionUp();

            if (!isConnected && onConnectionDown != null)
                onConnectionDown();
        }
#endif
    }

    #endregion

    #region Logs

    void OnLog()
    {
    }

    void OnLogWarning()
    {
    }

    void OnLogError()
    {
    }

    #endregion

    #region Initialization

    void InitializeSystem()
    {
    }

    void OnNPInitialized()
    {
        Debug.Log("Platform initialized!");
        if (logText)
            logText.text = "Platform initalized!";

        npReady = true;
        StartCoroutine("WaitUntilConnectionChecked");
    }

    IEnumerator WaitUntilConnectionChecked()
    {
        yield return null;
        SignIn();
    }

    #endregion

    #region Network Connection

    bool pendingRequest;

    public void RequestNetInfo()
    {
        if (pendingRequest) return;
        pendingRequest = true;


    }

    void OnSystemGotNetInfo()
    {
        pendingRequest = false;
    }

    void OnConnectionUp()
    {
        Debug.Log("Connection Up");

        if (logText)
            logText.text = "Connection up. Status: ";

        if (onConnectionUp != null)
            onConnectionUp();
    }

    void OnConnectionDown()
    {
        Debug.Log("Connection Down");

        // Determining the reason for loss of connection...
        //
        // When connection is lost we can call System.GetLastConnectionError() to obtain
        // the NetCtl error status and reason for loss of connection.
        //
        // ResultCode.lastError will be either NP_ERR_NOT_CONNECTED
        // or NP_ERR_NOT_CONNECTED_FLIGHT_MODE.
        //
        // For the case where ResultCode.lastError == NP_ERR_NOT_CONNECTED further information about
        // the disconnection reason can be inferred from ResultCode.lastErrorSCE which contains
        // the SCE NetCtl error code relating to the disconnection (please refer to SCE SDK docs when
        // interpreting this code).


        if (logText)
            logText.text = "Connection lost. Code: ";

        if (onConnectionDown != null)
            onConnectionDown();
    }

    #endregion

    #region Public Methods

    public void SignIn()
    {
#if UNITY_PS4 && !UNITY_EDITOR
        if (npReady)
        {
            SonyUserManager.Instance.SignIn();
        }
        else
        {
            if (logText)
                logText.text = "Can't sign in yet. NP not initialized.";
        }
#else
        if (logText)
            logText.text = "Sign in not available on this platform.";
#endif
    }

    #endregion
}
