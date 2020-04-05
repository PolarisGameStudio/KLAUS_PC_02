using UnityEngine;
using System.Collections;
using System;

public class CollectablesManager
{
    public static Action gotPieceCallback;
    public static Action<int> gotAllPieceCallback;

    public static int getWorld(string level)
    {
        if (level.Length < 2)
            return -1;

        string levelSub = level.Substring(1, 1);
        if (char.IsDigit(levelSub[0]))
        {
            int realLevel = int.Parse(levelSub);
            if (realLevel > 0)
                realLevel -= 1;
            return realLevel;
        }

        return -1;

    }

    public static int getLevel(string level)
    {
        if (level.Length < 5)
            return -1;

        string levelSub = level.Substring(3, 2);
        if (char.IsDigit(levelSub[0]))
        {
            int realLevel = int.Parse(levelSub);
            if (realLevel > 0)
                realLevel -= 1;
            return realLevel;
        }


        return -1;

    }

    /// <summary>
    /// Muestra el portal en el nivel si es True
    /// </summary>
    /// <returns><c>true</c>, if collectable ready was ised, <c>false</c> otherwise.</returns>
    /// <param name="level">Level.</param>
    public static bool isCollectablePortalReady(string level)
    {
        /* if (SaveManager.Instance.comingFromArcadeMode)
         {
             return true;
         }*/
        if (SaveManager.Instance.dataKlaus == null || SaveManager.Instance.dataKlaus.collectablesItems.Count <= 0)
        {
            return true;
        }
        return SaveManager.Instance.dataKlaus.collectablesItems[getWorld(level)].item[getLevel(level)];
    }

    public static bool isCollectableFullForAll()
    {
        if (SaveManager.Instance.dataKlaus == null || SaveManager.Instance.dataKlaus.collectablesItems.Count <= 0)
        {
            return false;
        }

        bool isC = true;
        for (int j = 0; j < SaveManager.Instance.dataKlaus.collectablesItems.Count; ++j)
        {
            for (int i = 0; i < SaveManager.Instance.dataKlaus.collectablesItems[j].item.Count; ++i)
            {
                isC = isC && SaveManager.Instance.dataKlaus.collectablesItems[j].item[i];
            }
        }
        return isC;
    }

    /// <summary>
    /// Si ya estan todos los objetos listos entonces muestra la peurta en vez del objeto
    /// </summary>
    /// <returns><c>true</c>, if collectable full was ised, <c>false</c> otherwise.</returns>
    /// <param name="level">Level.</param>
    public static bool isCollectableFull(string level)
    {
        /* if (SaveManager.Instance.comingFromArcadeMode)
        {
            return false;
        }*/

        if (SaveManager.Instance.dataKlaus == null || SaveManager.Instance.dataKlaus.collectablesItems.Count <= 0)
        {
            return false;
        }
        bool isC = true;
        int world = getWorld(level);
        for (int i = 0; i < SaveManager.Instance.dataKlaus.collectablesItems[world].item.Count; ++i)
        {
            isC = isC && SaveManager.Instance.dataKlaus.collectablesItems[world].item[i];
        }
        return isC;
    }

    public static bool isCollectableReady(string level)
    {
        /* if (SaveManager.Instance.comingFromArcadeMode)
        {
            return false;
        }*/
        if (SaveManager.Instance.dataKlaus == null || SaveManager.Instance.dataKlaus.collectablesItems.Count <= 0)
        {
            return false;
        }
        int world = getWorld(level);
        int levelID = getLevel(level);
        return SaveManager.Instance.dataKlaus.collectablesItems[world].item[levelID];
    }

    /// <summary>
    /// Obtiene el collectable y devuelve la posicion de este para mostrar a la UI.
    /// 0,1,2,3,4 - 5 Se muestra algo especial.
    /// </summary>
    /// <returns>The collectable.</returns>
    /// <param name="level">Level.</param>
    public static int setCollectable(string level)
    {
        /*if (SaveManager.Instance.comingFromArcadeMode)
        {
            return -1;
        }*/
        if (SaveManager.Instance.dataKlaus == null || SaveManager.Instance.dataKlaus.collectablesItems.Count <= 0)
        {
            return -1;
        }
        int world = getWorld(level);

        if (!SaveManager.Instance.dataKlaus.collectablesItems[world].item[getLevel(level)])
            if (gotPieceCallback != null)//Trophy Hack
                gotPieceCallback();

        SaveManager.Instance.dataKlaus.collectablesItems[world].item[getLevel(level)] = true;
        SaveManager.Instance.Save();


        int posC = -1;
        for (int i = 0; i < SaveManager.Instance.dataKlaus.collectablesItems[world].item.Count; ++i)
        {
            if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[i])
            {
                ++posC;
            }
        }
        if (posC == 5)
        {
            if (gotAllPieceCallback != null)
                gotAllPieceCallback(world + 1);
        }
        return posC;
    }

    public static int GetCollectedPieces(string level)
    {
        // TODO: Devolver aca el numero de piezas coleccionadas en el nivel scene (W0L00)
        int world = getWorld(level);
        int levelID = getLevel(level);
        int number = 0;

        if (world < 0 || levelID < 0)
            return 0;
        if (world == 3 && levelID == 4)
        {
            if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[4])
            {
                number += 1;
            }
            if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[5])
            {
                number += 1;
            }

        }
        else if (world == 3 && levelID == 5)
        {

        }
        else if (world == 5)
        {
            if (levelID == 0)
            {
                //1 y 2
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[0])
                {
                    number += 1;
                }
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[1])
                {
                    number += 1;
                }
            }
            else if (levelID == 1)
            {
                //3 y 4
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[2])
                {
                    number += 1;
                }
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[3])
                {
                    number += 1;
                }
            }
            else if (levelID == 2)
            {
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[4])
                {
                    number += 1;
                }
            }
            else if (levelID == 3)
            {
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[5])
                {
                    number += 1;
                }
            }
        }
        else
        {
#if UNITY_EDITOR
            if (SaveManager.Instance != null
               && SaveManager.Instance.dataKlaus != null
              && SaveManager.Instance.dataKlaus.collectablesItems != null)
#endif
                if (SaveManager.Instance.dataKlaus.collectablesItems[world].item[levelID])
                {
                    number = 1;
                }
        }

        return number;
    }

    public static int GetTotalPieces(string level)
    {
        // TODO: Devolver aca el numero de piezas totales en el nivel scene (W0L00)

        int world = getWorld(level);
        int levelID = getLevel(level);
        int number = 0;

        if (world < 0 || levelID < 0)
            return 0;
        if (world == 3 && levelID == 4)
        {
            number = 2;
        }
        else if (world == 3 && levelID == 5)
        {
            number = 0;
        }
        else if (world == 5)
        {
            if (levelID == 0)
            {
                number = 2;
            }
            else if (levelID == 1)
            {
                number = 2;

            }
            else if (levelID == 2)
            {
                number = 1;
            }
            else if (levelID == 3)
            {
                number = 1;
            }
        }
        else
        {
            number = 1;
        }

        return number;
    }
}
