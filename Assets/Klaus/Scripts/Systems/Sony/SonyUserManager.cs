using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//TODO: PC: This now will be the Manager for Leaderboard
public class SonyUserManager : MonoSingleton<SonyUserManager>
{
    public struct PlayerProfile
    {
        public string playerID;
        public string avatarURL;
    }

    public Text onlineIDText;

    public Action<PlayerProfile> onProfileRetrieved;
    public Action onAvatarRetrieved;
    public Action onSignedIn;
    public Action onSignedOut;
    public Action onSignInError;

    #region Public Components

    public bool IsSignedIn
    {
        get
        {
            return isSignedIn;
        }
    }

    public bool IsSigninIn
    {
        get
        {
            return

                isSigninIn;
        }
    }

    public bool IsUserProfileBusy
    {
        get
        {
            return

                isUserProfileBusy;
        }
    }

    public string PlayerID { get; protected set; }
    public int PlayerAge { get; protected set; }
    public bool HasProperAge { get { return PlayerAge == -1 || PlayerAge >= SonyManager.Instance.ageRating; } }
    public Sprite PlayerAvatar { get; protected set; }

    string avatarURL;
    bool isSignedIn = false;
    bool isSigninIn = false;
    bool isUserProfileBusy = false;

    #endregion

    #region Game Cycle

    protected override void Init()
    {
        base.Init();


        SonyRequests.Instance.OnParentalControlReturned += OnUserGotAge;
        PlayerAge = -1;
    }

    IEnumerator FetchProfile()
    {
        while (!IsSignedIn || !SonyManager.Instance.IsConnected || IsUserProfileBusy)
        {
            if (!string.IsNullOrEmpty(PlayerID) && !string.IsNullOrEmpty(avatarURL))
                yield break;

            yield return null;
        }

        // If none of this string are empty, it means that the profile was already retrieved
        if (!string.IsNullOrEmpty(PlayerID) && !string.IsNullOrEmpty(avatarURL))
            yield break;

    }

    void Update()
    {
        if (SonyManager.Instance.statusText)
        {
            SonyManager.Instance.statusText.text = string.Empty;

            SonyManager.Instance.statusText.text = "Can't log in this platform.";

        }

        if (onlineIDText)
            onlineIDText.text = PlayerID;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V))
        {
            isSignedIn = !isSignedIn;

            if (isSignedIn && onSignedIn != null)
                onSignedIn();

            if (!isSignedIn && onSignedOut != null)
                onSignedOut();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerAge = PlayerAge < 12 ? 13 : 7;
            Debug.Log("Age: " + PlayerAge);
        }
#endif
    }

    #endregion

    #region Events

    void OnUserGotProfile()
    {
        SonyManager.Instance.statusText.text = "Can't log in this platform.";

        // Ask for the player's age
        SonyRequests.Instance.GetParentalControlInfo();

        // Check NP availability
        SonyRequests.Instance.CheckNpAvailability();

        // Download and display the avatar image.
        DownloadAvatar();
    }

    void OnUserGotAge(int age, bool chatRestriction, bool ugcRestriction)
    {
        PlayerAge = age;
    }

    void OnGotRemoteUserProfile()
    {

        if (onProfileRetrieved != null)
            onProfileRetrieved(new PlayerProfile() { /*playerID = profile.onlineID, avatarURL = profile.avatarURL */});
    }

    void OnUserProfileError()
    {
        //  Debug.LogError(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));
    }

    void OnSignedIn()
    {

        GetMyProfile();
        SaveManager.Instance.LoadRanksFromServer();

        if (onSignedIn != null)
            onSignedIn();
    }

    void OnSignInError()
    {



        if (SonyManager.Instance.logText)
            SonyManager.Instance.logText.text = "Sign in error. Code: ";

        if (onSignInError != null)
            onSignInError();
    }

    void OnSignedOut()
    {

        /// Debug.LogError(result.className + ": " + result.lastError + ", sce error 0x" + result.lastErrorSCE.ToString("X8"));

        if (SonyManager.Instance.logText)
            SonyManager.Instance.logText.text = "Signed out. Code: ";

        if (onSignedOut != null)
            onSignedOut();
    }

    #endregion

    #region Avatar

    public void DownloadAvatar()
    {
        // If the url is empty
        if (string.IsNullOrEmpty(avatarURL))
        {
            // Try to fetch the profile again from cache
            if (!string.IsNullOrEmpty(PlayerID))
            {
            }

            // If no url, fetch the player's profile again
            if (string.IsNullOrEmpty(avatarURL))
                GetMyProfile();
        }

        // If url is not empty but there's no avatar, download it from url
        if (!string.IsNullOrEmpty(avatarURL) && (PlayerAvatar == null || PlayerAvatar.texture == null))
            StartCoroutine(DownloadAvatar(avatarURL));
    }

    IEnumerator DownloadAvatar(string url)
    {
        Texture2D avatar = new Texture2D(4, 4, TextureFormat.DXT1, false);

        // Start a download of the given URL
        var www = new WWW(url);

        // Wait until the download is done
        yield return www;

        // Assign the downloaded image to the main texture of the object
        www.LoadImageIntoTexture(avatar);

        if (www.bytesDownloaded != 0)
        {
            // Release non-GPU texture memory.
            avatar.Apply(true, true);

            // Create sprite
            if (avatar != null)
            {
                PlayerAvatar = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), Vector2.one * 0.5f);

                if (onAvatarRetrieved != null)
                    onAvatarRetrieved();
            }
        }
    }

    #endregion

    #region Public Methods

    public void SignIn()
    {

    }

    public void GetMyProfile()
    {
        StartCoroutine("FetchProfile");
    }

    public void GetRemoteProfile(string remoteOnlineID)
    {
    }

    #endregion
}
