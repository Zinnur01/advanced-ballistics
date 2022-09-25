using UnityEngine;

public abstract class CoroutineObjectBase : ICoroutineObjectBase
{
    protected MonoBehaviour owner;
    protected Coroutine coroutine;

    public CoroutineObjectBase(MonoBehaviour owner)
    {
        this.owner = owner;
    }

    public bool IsProcessing()
    {
        return coroutine != null;
    }

    #region [Getter / Setter]
    public MonoBehaviour GetOwner()
    {
        return owner;
    }

    protected void SetOwner(MonoBehaviour value)
    {
        owner = value;
    }

    public Coroutine GetCoroutine()
    {
        return coroutine;
    }

    protected void SetCoroutine(Coroutine value)
    {
        coroutine = value;
    }
    #endregion
}