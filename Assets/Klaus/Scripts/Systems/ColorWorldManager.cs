using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColorWorldManager : PersistentSingleton<ColorWorldManager>
{

    [Serializable]
    public class ColorWorld
    {
        public int world = 1;
        public Color color;
    }
    public ColorWorld[] WorldColors = new ColorWorld[11];
    protected int currentWorld = 1;
    public void OnLevelWasLoaded(int level)
    {
        if (SaveManager.Instance.dataKlaus == null)
        {
            currentWorld = 0;
            return;
        }

        if (Application.loadedLevelName.Substring(0, 1) == "W")
        {
            SaveManager.Instance.dataKlaus.world = int.Parse(Application.loadedLevelName.Substring(1, 1));
        }
        else
        {
            try {
            SaveManager.Instance.dataKlaus.world = int.Parse(SaveManager.Instance.dataKlaus.GetCurrentLevel().Substring(1, 1));
            }catch {
                Debug.Log(SaveManager.Instance.dataKlaus.GetCurrentLevel() + " " + SaveManager.Instance.dataKlaus.GetCurrentLevel().Substring(1, 1));
            }
        }

        LoadWorldColor();
        LanguageValueSave.UpdateLanguage();
    }

    public void LoadWorldColor()
    {
        currentWorld = SaveManager.Instance.dataKlaus.world;

        if (currentWorld >= 6)
        {
            currentWorld = 6;
            string newNumberLevel = SaveManager.Instance.dataKlaus.GetCurrentLevel().Substring(4, 1);
            currentWorld += Convert.ToInt32(newNumberLevel) - 1;
        }

        LightBarManager.Instance.SetLightColor(getColorScene());
    }

    public Color getColorScene()
    {
        ColorWorld world = Array.Find<ColorWorld>(WorldColors, x => x == null ? false : x.world == currentWorld);
        return world != null ? world.color : Color.red;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public Color getGlitchColor()
    {
        ColorWorld world = Array.Find<ColorWorld>(WorldColors, x => x == null ? false : x.world == 5);
        return world != null ? world.color : Color.clear;
    }

    public Color getWorldColor(int id)
    {
        ColorWorld world = Array.Find<ColorWorld>(WorldColors, x => x == null ? false : x.world == id);
        return world != null ? world.color : Color.clear;
    }
}
