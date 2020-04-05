/*
 * Copyright (c) 2012 Made With Mosnter Love (Pty) Ltd
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

public class StateEngine : MonoBehaviour
{
    public event Action<System.Enum> Changed;

    StateMapping currentState;
    StateMapping destinationState;

    Dictionary<Enum, StateMapping> stateLookup;
    Dictionary<string, Delegate> methodLookup;

    readonly string[] ignoredNames = new[] { "add", "remove", "get", "set" };

    bool isInTransition = false;
    IEnumerator currentTransition;
    IEnumerator exitRoutine;
    IEnumerator enterRoutine;
    IEnumerator queuedChange;

    public void Initialize<T>(StateBehaviour entity)
    {
        // Define States
        var values = Enum.GetValues(typeof(T));
        stateLookup = new Dictionary<Enum, StateMapping>();
        for (int i = 0; i != values.Length; ++i)
        {
            var mapping = new StateMapping((Enum)values.GetValue(i));
            stateLookup.Add(mapping.state, mapping);
        }

        // Reflect methods
        var methods = entity.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public |
                                  BindingFlags.NonPublic);

        // Bind methods to states
        var separator = "_".ToCharArray();
        for (int i = 0; i != methods.Length; ++i)
        {
            var names = methods[i].Name.Split(separator);

            // Ignore functions without an underscore
            if (names.Length <= 1)
                continue;

            Enum key;
            try
            {
                key = (Enum)Enum.Parse(typeof(T), names[0]);
            }
            catch (ArgumentException)
            {
                // Some things (evetns, properties) generate automatic method. Ignore these
                for (int j = 0; j != ignoredNames.Length; ++j)
                    if (names[0] == ignoredNames[j])
                        goto SkipWarning;

                Debug.LogWarning("Method with name " + methods[i].Name + " could not resolve a matching state. Check method spelling");
                continue;

            SkipWarning:
                continue;
            }

            var targetState = stateLookup[key];

            switch (names[1])
            {
                case "Enter":
                    if (methods[i].ReturnType == typeof(IEnumerator))
                    {
                        targetState.Enter = CreateDelegate<Func<IEnumerator>>(methods[i], entity);
                    }
                    else
                    {
                        var action = CreateDelegate<Action>(methods[i], entity);
                        targetState.Enter = () => { action(); return null; };
                    }
                    break;
                case "Exit":
                    if (methods[i].ReturnType == typeof(IEnumerator))
                    {
                        targetState.Exit = CreateDelegate<Func<IEnumerator>>(methods[i], entity);
                    }
                    else
                    {
                        var action = CreateDelegate<Action>(methods[i], entity);
                        targetState.Exit = () => { action(); return null; };
                    }
                    break;
                case "Finally":
                    targetState.Finally = CreateDelegate<Action>(methods[i], entity);
                    break;
                case "Update":
                    targetState.Update = CreateDelegate<Action>(methods[i], entity);
                    break;
                case "LateUpdate":
                    targetState.LateUpdate = CreateDelegate<Action>(methods[i], entity);
                    break;
                case "FixedUpdate":
                    targetState.FixedUpdate = CreateDelegate<Action>(methods[i], entity);
                    break;
                case "TriggerEnter":
                    targetState.TriggerEnter = CreateDelegate<Action<Collider>>(methods[i], entity);
                    break;
                case "TriggerStay":
                    targetState.TriggerStay = CreateDelegate<Action<Collider>>(methods[i], entity);
                    break;
                case "TriggerExit":
                    targetState.TriggerExit = CreateDelegate<Action<Collider>>(methods[i], entity);
                    break;
                case "CollisionEnter":
                    targetState.CollisionEnter = CreateDelegate<Action<Collision>>(methods[i], entity);
                    break;
                case "CollisionStay":
                    targetState.CollisionStay = CreateDelegate<Action<Collision>>(methods[i], entity);
                    break;
                case "CollisionExit":
                    targetState.CollisionExit = CreateDelegate<Action<Collision>>(methods[i], entity);
                    break;
                case "TriggerEnter2D":
                    targetState.TriggerEnter2D = CreateDelegate<Action<Collider2D>>(methods[i], entity);
                    break;
                case "TriggerStay2D":
                    targetState.TriggerStay2D = CreateDelegate<Action<Collider2D>>(methods[i], entity);
                    break;
                case "TriggerExit2D":
                    targetState.TriggerExit2D = CreateDelegate<Action<Collider2D>>(methods[i], entity);
                    break;
                case "CollisionEnter2D":
                    targetState.CollisionEnter2D = CreateDelegate<Action<Collision2D>>(methods[i], entity);
                    break;
                case "CollisionStay2D":
                    targetState.CollisionStay2D = CreateDelegate<Action<Collision2D>>(methods[i], entity);
                    break;
                case "CollisionExit2D":
                    targetState.CollisionExit2D = CreateDelegate<Action<Collision2D>>(methods[i], entity);
                    break;
            }
        }
    }

    V CreateDelegate<V>(MethodInfo method, Object target) where V : class
    {
        var ret = (Delegate.CreateDelegate(typeof(V), target, method) as V);

        if (ret == null)
            throw new ArgumentException("Unabled to create delegate for method called " + method.Name);

        return ret;
    }

    public void ChangeState(Enum newState, StateTransition transition = StateTransition.Safe)
    {
        if (stateLookup == null)
            throw new Exception("States have not been configured, please call initialized before trying to set state");

        if (!stateLookup.ContainsKey(newState))
            throw new Exception("No state with the name " + newState.ToString() + " can be found. Please make sure you are called the correct type the statemachine was initialized with");

        var nextState = stateLookup[newState];

        // Uncomment this to block loop transitions
        // if (currentState == nextState) return;

        // Cancel any queued changes.
        if (queuedChange != null)
        {
            StopCoroutine(queuedChange);
            queuedChange = null;
        }

        switch (transition)
        {
            // case StateMachineTransition.Blend:
            // Do nothing - allows the state transitions to overlap each other. This is a dumb idea, as previous state might trigger new changes. 
            // A better way would be to start the two couroutines at the same time. IE don't wait for exit before starting start.
            // How does this work in terms of overwrite?
            // Is there a way to make this safe, I don't think so? 
            // break;
            case StateTransition.Safe:
                if (isInTransition)
                {
                    if (exitRoutine != null) // We are already exiting current state on our way to our previous target state
                    {
                        // Overwrite with our new target
                        destinationState = nextState;
                        return;
                    }

                    if (enterRoutine != null) // We are already entering our previous target state. Need to wait for that to finish and call the exit routine.
                    {
                        // Damn, I need to test this hard
                        queuedChange = WaitForPreviousTransition(nextState);
                        StartCoroutine(queuedChange);
                        return;
                    }
                }
                break;
            case StateTransition.Overwrite:
                if (currentTransition != null) StopCoroutine(currentTransition);

                if (exitRoutine != null) StopCoroutine(exitRoutine);

                if (enterRoutine != null) StopCoroutine(enterRoutine);

                if (currentState != null) currentState.Finally();

                currentState = null; // We need to set current state to null so that we don't trigger it's exit routine
                break;
        }

        isInTransition = true;
        currentTransition = ChangeToNewStateRoutine(nextState);
        StartCoroutine(currentTransition);
    }

    IEnumerator ChangeToNewStateRoutine(StateMapping newState)
    {
        destinationState = newState; // Chache this so that we can overwrite it and hijack a transition

        if (currentState != null)
        {
            exitRoutine = currentState.Exit();

            if (exitRoutine != null)
                yield return StartCoroutine(exitRoutine);

            exitRoutine = null;

            currentState.Finally();
        }

        currentState = destinationState;

        if (currentState != null)
        {
            enterRoutine = currentState.Enter();

            if (enterRoutine != null)
                yield return StartCoroutine(enterRoutine);

            enterRoutine = null;

            // Broadcast change only after enter transition has begun. 
            if (Changed != null)
                Changed(currentState.state);
        }

        isInTransition = false;
    }

    IEnumerator WaitForPreviousTransition(StateMapping nextState)
    {
        while (isInTransition)
            yield return null;

        ChangeState(nextState.state);
    }

    void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdate();
    }

    void Update()
    {
        if (currentState != null && !IsInTransition)
            currentState.Update();
    }

    void LateUpdate()
    {
        if (currentState != null && !IsInTransition)
            currentState.LateUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerExit(other);
    }

    void OnCollisionEnter(Collision other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionEnter(other);
    }

    void OnCollisionStay(Collision other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionStay(other);
    }

    void OnCollisionExit(Collision other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionExit(other);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerEnter2D(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerStay2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.TriggerExit2D(other);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionEnter2D(other);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionStay2D(other);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (currentState != null && !IsInTransition)
            currentState.CollisionExit2D(other);
    }

    public static void DoNothing() { }
    public static void DoNothingCollider(Collider other) { }
    public static void DoNothingCollision(Collision other) { }
    public static void DoNothingCollider2D(Collider2D other) { }
    public static void DoNothingCollision2D(Collision2D other) { }
    public static IEnumerator DoNothingCoroutine() { yield break; }

    public Enum GetState()
    {
        return currentState == null ? null : currentState.state;
    }

    public bool IsInTransition
    {
        get { return isInTransition; }
    }
}

public enum StateTransition
{
    // Blend,
    Overwrite,
    Safe
}

public class StateMapping
{
    public Enum state;

    public Func<IEnumerator> Enter = StateEngine.DoNothingCoroutine;
    public Func<IEnumerator> Exit = StateEngine.DoNothingCoroutine;
    public Action Finally = StateEngine.DoNothing;
    public Action Update = StateEngine.DoNothing;
    public Action LateUpdate = StateEngine.DoNothing;
    public Action FixedUpdate = StateEngine.DoNothing;
    public Action<Collider> TriggerEnter = StateEngine.DoNothingCollider;
    public Action<Collider> TriggerStay = StateEngine.DoNothingCollider;
    public Action<Collider> TriggerExit = StateEngine.DoNothingCollider;
    public Action<Collision> CollisionEnter = StateEngine.DoNothingCollision;
    public Action<Collision> CollisionStay = StateEngine.DoNothingCollision;
    public Action<Collision> CollisionExit = StateEngine.DoNothingCollision;
    public Action<Collider2D> TriggerEnter2D = StateEngine.DoNothingCollider2D;
    public Action<Collider2D> TriggerStay2D = StateEngine.DoNothingCollider2D;
    public Action<Collider2D> TriggerExit2D = StateEngine.DoNothingCollider2D;
    public Action<Collision2D> CollisionEnter2D = StateEngine.DoNothingCollision2D;
    public Action<Collision2D> CollisionStay2D = StateEngine.DoNothingCollision2D;
    public Action<Collision2D> CollisionExit2D = StateEngine.DoNothingCollision2D;

    public StateMapping(Enum state)
    {
        this.state = state;
    }
}