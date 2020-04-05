using UnityEngine;
using SmartLocalization.Editor;

public class HUD_ErrorMessages : MonoSingleton<HUD_ErrorMessages>
{
    bool signedOutErrorPrompted, connectionDownErrorPrompted;

    void Start()
    {
        // Don't suscribe if not coming from time attack mode
        if (!SaveManager.Instance.isComingFromArcade || !SaveManager.Instance.comingFromTimeArcadeMode)
            return;

        // if (RankingManager.Instance != null)
        //    RankingManager.Instance.OnRegisterScoreError += CheckErrors;

        if (SonyUserManager.Instance != null)
        {
            //    SonyUserManager.Instance.onSignedOut += CheckErrors;
            // SonyUserManager.Instance.onSignInError += CheckErrors;
            SonyUserManager.Instance.onSignedIn += OnSignedIn;
        }

        if (SonyManager.Instance != null)
        {
            SonyManager.Instance.onConnectionUp += OnConnectionUp;
            //  SonyManager.Instance.onConnectionDown += CheckErrors;
        }
    }

    void OnDestroy()
    {
        // if (RankingManager.InstanceExists())
        //   RankingManager.Instance.OnRegisterScoreError -= CheckErrors;

        if (SonyUserManager.InstanceExists())
        {
            // SonyUserManager.Instance.onSignedOut -= CheckErrors;
            //  SonyUserManager.Instance.onSignInError -= CheckErrors;
            SonyUserManager.Instance.onSignedIn -= OnSignedIn;
        }

        if (SonyManager.InstanceExists())
        {
            //  SonyManager.Instance.onConnectionDown -= CheckErrors;
            SonyManager.Instance.onConnectionUp -= OnConnectionUp;
        }
    }

    public void OnSignedIn()
    {
        if (!signedOutErrorPrompted)
            return;

        signedOutErrorPrompted = false;

        if (!SonyUserManager.Instance.HasProperAge)
            ShowError("UI.TimeAttack.NP_UNDERAGE");
        else
            ShowError("UI.TimeAttack.SignedIn");
    }

    public void OnConnectionUp()
    {
        if (!connectionDownErrorPrompted)
            return;

        connectionDownErrorPrompted = false;
        ShowError("UI.TimeAttack.ConnectionUp");
    }

    public void CheckErrors()
    {
        bool connected = SonyManager.Instance.IsConnected;
        bool signedIn = SonyUserManager.Instance.IsSignedIn;
        bool properAge = SonyUserManager.Instance.HasProperAge;

        if (connected && !signedIn)
        {
            signedOutErrorPrompted = true;
            ShowError("UI.TimeAttack.NP_ERR_NOT_SIGNED_IN");
        }
        else if (!connected && signedIn)
        {
            connectionDownErrorPrompted = true;
            ShowError("UI.TimeAttack.NP_ERR_NOT_CONNECTED");
        }
        else if (!connected && !signedIn)
        {
            connectionDownErrorPrompted = true;
            signedOutErrorPrompted = true;
            ShowError("UI.TimeAttack.NP_NOT_CONNECTED_NOT_SIGNED_IN");
        }
        else if (!properAge)
        {
            ShowError("UI.TimeAttack.NP_UNDERAGE");
        }
        /*else if (error != ErrorCode.NP_OK)
        {
            ShowError("UI.TimeAttack." + error.ToString());
        }*/
    }

    void ShowError(string key)
    {
        /*/
        SaveMessagesManager.Instance.ShowMessage(key);
        /*/
    }
}
