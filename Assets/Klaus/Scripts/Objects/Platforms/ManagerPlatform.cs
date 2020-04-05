//
// PlatformControllerInput.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerPlatform : Singleton<ManagerPlatform>
{
    public Animator Mira;
    Animator m_miraSpawned;

    Dictionary<int, PlatformInputController> platforms;
    int currentPlatform = -1;

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:ManagerPlatform"/> can move platform.
    /// </summary>
    /// <value><c>true</c> if can move platform; otherwise, <c>false</c>.</value>
    public static bool CanMovePlatform
    {
        get
        {
            if (InputEnum.GamePad != "keyboard")
            {
                return !InputTouchPS4.IsSelectingPlatforms && !CameraMovement.IsBlockingMove;
            }

            return true;
        }
    }

    /// <summary>
    /// Active the current platform
    /// </summary>
    void setMiraToPlatform()
    {
        m_miraSpawned.SetTrigger("Start");
        m_miraSpawned.transform.SetParent(platforms[currentPlatform].transform);
        StopCoroutine("SetMiraPosition");
        StartCoroutine("SetMiraPosition", (0.1f));
        platforms[currentPlatform].enabled = true;

    }
    IEnumerator SetMiraPosition(float time)
    {
        yield return new WaitForSeconds(time);
        m_miraSpawned.transform.position = platforms[currentPlatform].transform.position;
    }

    /// <summary>
    /// Select new platform and de-active de older
    /// </summary>
    /// <param name="newId"></param>
    public bool SelectPlatform(int newId)
    {
        if (newId == currentPlatform)
            return false;
        InitializeCurrentPlatform();

        platforms[currentPlatform].enabled = (false);
        currentPlatform = newId;
        DeSelectPlatform();
        return true;
    }
    public void DeSelectPlatform()
    {
        setMiraToPlatform();
    }
    public void DeselectAll()
    {
        PlatformInputController platform;
        if (platforms.TryGetValue(currentPlatform, out platform))
            platform.enabled = false;

        currentPlatform = -1;

        if (m_miraSpawned != null)
        {
            m_miraSpawned.transform.SetParent(null);
            m_miraSpawned.gameObject.SetActive(false);
        }

        StopCoroutine("SetMiraPosition");
    }

    /// <summary>
    /// If the platform is the current selected platform
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public bool isSelected(PlatformInputController platform)
    {
        return currentPlatform == platform.GetInstanceID();
    }

    void InitializeCurrentPlatform()
    {
        if (currentPlatform == -1)
        {
            foreach (int plat in platforms.Keys)
            {
                currentPlatform = plat;
                break;
            }
            // currentPlatform = plat.GetInstanceID();
            if (m_miraSpawned == null)
            {
                m_miraSpawned = Instantiate(Mira);
            }
            m_miraSpawned.gameObject.SetActive(true);
            setMiraToPlatform();
        }
    }
    #region Unity Callbacks:
    protected override void AwakeChild()
    {
        PlatformInputController[] plats = FindObjectsOfType<PlatformInputController>();
        platforms = new Dictionary<int, PlatformInputController>();
        foreach (PlatformInputController plat in plats)
        {
            /*
            if (currentPlatform == -1)
            {

                currentPlatform = plat.GetInstanceID();
            }*/

            platforms.Add(plat.GetInstanceID(), plat);
            plat.enabled = false;
        }

    }
    #endregion

    public void AddPlatform(PlatformInputController platform)
    {
        if (platform == null) return;

        PlatformInputController reference;
        if (!platforms.TryGetValue(platform.GetInstanceID(), out reference))
        {
            platforms.Add(platform.GetInstanceID(), platform);
            reference = platform;
        }

        reference.enabled = false;
    }
}
