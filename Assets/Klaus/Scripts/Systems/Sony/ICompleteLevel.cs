using UnityEngine;
using System.Collections;
using System;

public interface ICompleteLevel
{
    void CallTrophy();
    void CompleteScene();
    void CompleteLevel();
    void RegisterCompleteLevel(Action callback);
    void UnRegisterCompleteLevel(Action callback);
    void RegisterCompleteScene(Action callback);
    void UnRegisterCompleteScene(Action callback);
}

