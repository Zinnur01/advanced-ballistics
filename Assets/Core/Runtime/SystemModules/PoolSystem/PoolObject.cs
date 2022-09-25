using System;
using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private string poolObjectID = GenerateID(7);

    private PoolManager pool;

    protected virtual void Awake()
    {
        pool = PoolManager.Instance;
    }

    public void Push()
    {
        OnBeforePush();
        OnBeforePushCallback?.Invoke();
        pool.Push(this);
        OnAfterPush();
        OnAfterPushCallback?.Invoke();
    }

    protected virtual void OnBeforePush() { }
    protected virtual void OnAfterPush() { }

    private static string GenerateID(int length = 32)
    {
        string id = string.Empty;

        do
            id += System.Guid.NewGuid().ToString();
        while (id.Length < length);

        if (id.Length > length)
            id = id.Substring(0, length);

        id = id.Replace("-", "");
        return id;
    }

    #region [Events]
    public event Action OnBeforePushCallback;
    public event Action OnAfterPushCallback;
    #endregion

    #region [Getter / Setter]
    public string GetID()
    {
        return poolObjectID;
    }

    public void SetID(string id)
    {
        poolObjectID = id;
    }
    #endregion
}