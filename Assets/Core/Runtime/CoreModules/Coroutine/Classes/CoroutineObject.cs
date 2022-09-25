using System;
using System.Collections;
using UnityEngine;

public sealed class CoroutineObject : CoroutineObjectBase
{
    private Func<IEnumerator> routine;

    public CoroutineObject(MonoBehaviour owner) : base(owner) { }

    private IEnumerator Process()
    {
        yield return routine.Invoke();
        coroutine = null;
    }

    public bool Start(Func<IEnumerator> routine, bool force = false)
    {
        if ((IsProcessing() && !force) || !owner.gameObject.activeInHierarchy)
        {
            return false;
        }
        Stop();
        this.routine = routine;
        coroutine = owner.StartCoroutine(Process());
        return true;
    }

    public bool Stop()
    {
        if (IsProcessing())
        {
            owner.StopCoroutine(coroutine);
            coroutine = null;
            return true;
        }
        return false;
    }

    #region [Getter / Setter]
    public Func<IEnumerator> GetRoutine()
    {
        return routine;
    }

    private void SetRoutine(Func<IEnumerator> value)
    {
        routine = value;
    }
    #endregion
}