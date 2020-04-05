using UnityEngine;
using System.Collections;

public class DoorPiece_Trophy : TrophieLogic
{
    public bool CollectAllTrophies = false;
    public int World = 1;
    const int IDforAll = 40;
    void OnEnable()
    {
        if (CollectAllTrophies)
        {
            CollectablesManager.gotAllPieceCallback += OnGotPieceAllCallback;
        }
        else
        {
            CollectablesManager.gotPieceCallback += OnGotPieceCallback;
        }
    }

    void OnDisable()
    {
        if (CollectAllTrophies)
        {
            CollectablesManager.gotAllPieceCallback -= OnGotPieceAllCallback;
        }
        else
        {
            CollectablesManager.gotPieceCallback -= OnGotPieceCallback;
        }
    }
    void OnGotPieceCallback()
    {
        if (UnLock)
            return;
        UnLock = true;
        enabled = false;
    }

    void OnGotPieceAllCallback(int wor)
    {
        if (UnLock)
            return;
        if (wor == World)
        {

            UnLock = true;

            bool UnlockAll = CollectablesManager.isCollectableFullForAll();
            if (UnlockAll)
            {
                TrophiesManager.Instance.AwardTrophy(IDforAll);
            }
            enabled = false;
        }
    }
}
