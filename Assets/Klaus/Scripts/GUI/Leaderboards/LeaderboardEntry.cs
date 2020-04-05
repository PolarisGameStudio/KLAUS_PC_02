using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LeaderboardEntry : MonoBehaviour
{
    public string playerId { get; protected set; }
    public int rank { get; protected set; }
    public float time { get; protected set; }

    public TextMeshProUGUI position;
    public Image avatar;
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;

    public Image[] images;
    public GameObject selectedObject;
    float[] originalAlpha;

    [HideInInspector]
    public LeaderboardEntry clone;
    public Vector2 limits = new Vector2(-21, -315);

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    public LeaderboardsScrollRect rectScroll
    {
        get
        {
            if (_rectScroll == null)
                _rectScroll = GameObject.FindObjectOfType<LeaderboardsScrollRect>();
            return _rectScroll;
        }
    }

    RectTransform _rectTransform;
    LeaderboardsScrollRect _rectScroll;

    void Awake()
    {
        originalAlpha = new float[images.Length];
        for (int i = 0; i != originalAlpha.Length; ++i)
            originalAlpha[i] = images[i].color.a;
    }

    public void Setup(string playerId, int position, float score, Color color)
    {
        this.playerId = playerId;
        time = score;

        this.score.text = score <= 0 ? "--:--:--.--" : HUD_TimeAttack.FormatTime(score);

        SetPosition(position);
        
        this.username.text = playerId;

        foreach (Image image in images)
        {
            image.color = color;
        }

        for (int i = 0; i != images.Length; ++i)
        {
            Color c = color;
            c.a = originalAlpha[i];
            images[i].color = c;
        }

        LoadUserProfile();
        SetupClone(color);
        enabled = clone != null;
    }

    public void SetPosition(int position)
    {
        this.rank = position > 0 ? position : int.MaxValue;

        if (position <= 0)
            this.position.text = "--";
        else if (position >= 1000)
            this.position.text = Mathf.FloorToInt((float)position / 1000f) + "K";
        else
            this.position.text = position.ToString();
        
        if (clone != null)
            clone.position.text = this.position.text;
    }

    public void Clear()
    {
        SonyUserManager.Instance.onAvatarRetrieved -= SetUserAvatar;
        SonyUserManager.Instance.onProfileRetrieved -= OnProfileLoaded;
        StopAllCoroutines();

        playerId = string.Empty;

        if (avatar.overrideSprite != null)
            avatar.overrideSprite = null;
        
        SetupClone(Color.clear);
        if (clone)
            clone.selectedObject.SetActive(false);
        if (clone)
            clone = null;
        enabled = false;
    }

    void SetupClone(Color color)
    {
        if (clone == null)
            return;

        clone.playerId = this.playerId;
        clone.position.text = this.position.text;
        clone.score.text = this.score.text;
        clone.username.text = this.username.text;
        clone.avatar.overrideSprite = avatar.overrideSprite;

        for (int i = 0; i != clone.images.Length; ++i)
        {
            Color c = color;
            c.a = clone.originalAlpha[i];
            clone.images[i].color = c;
        }
    }

    void Update()
    {
        if (clone)
        {
            clone.rectTransform.sizeDelta = rectTransform.sizeDelta;
            clone.transform.position = transform.position;

            Vector2 position = clone.rectTransform.anchoredPosition;
            position.y = Mathf.Clamp(position.y, clone.limits.y, clone.limits.x);
            clone.rectTransform.anchoredPosition = position;
        }
    }

    #region Animations

    public void SetNormal()
    {
        selectedObject.SetActive(false);
        if (clone)
            clone.selectedObject.SetActive(false);
    }

    public void SetHighlighted()
    {
        selectedObject.SetActive(true);
        if (clone)
            clone.selectedObject.SetActive(true);
        rectScroll.SetSelected(rectTransform);
    }

    public void SetPress()
    {
        selectedObject.SetActive(true);
        if (clone)
            clone.selectedObject.SetActive(true);
    }

    public void SetDisable()
    {
        //NonSelected.enabled = true;
        //canvasGroup.alpha = 0.4f;
    }

    #endregion

    #region Avatar

    void LoadUserProfile()
    {
        if (string.IsNullOrEmpty(playerId))
            return;
        
        if (playerId == SonyUserManager.Instance.PlayerID)
        {
            SonyUserManager.Instance.onAvatarRetrieved += SetUserAvatar;
            SetUserAvatar();

            if (SonyUserManager.Instance.PlayerAvatar == null || SonyUserManager.Instance.PlayerAvatar.texture == null)
                SonyUserManager.Instance.DownloadAvatar();
        }
        else
        {
            StartCoroutine("GetProfile");
        }
    }

    void SetUserAvatar()
    {
        avatar.overrideSprite = SonyUserManager.Instance.PlayerAvatar;
        clone.avatar.overrideSprite = avatar.overrideSprite;
    }

    void OnProfileLoaded(SonyUserManager.PlayerProfile profile)
    {
        if (profile.playerID == playerId)
        {
            SonyUserManager.Instance.onProfileRetrieved -= OnProfileLoaded;
            StartCoroutine(DownloadAvatar(profile.avatarURL));
        }
    }

    IEnumerator GetProfile()
    {
        while (SonyUserManager.Instance.IsUserProfileBusy)
            yield return null;

        SonyUserManager.Instance.onProfileRetrieved += OnProfileLoaded;
        SonyUserManager.Instance.GetRemoteProfile(playerId);
    }

    IEnumerator DownloadAvatar(string url)
    {
        Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT1, false);

        // Start a download of the given URL
        WWW www = new WWW(url);

        // Wait until the download is done
        yield return www;

        // Assign the downloaded image to the main texture of the object
        www.LoadImageIntoTexture(texture);

        if (www.bytesDownloaded != 0)
        {
            // Release non-GPU texture memory.
            texture.Apply(true, true);

            // Create sprite
            if (texture != null)
            {
                avatar.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                if (clone != null) clone.avatar.overrideSprite = avatar.overrideSprite;
            }
        }
    }

    #endregion
}
