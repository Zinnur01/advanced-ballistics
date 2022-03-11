﻿using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private string poolObjectID = GenerateID(7);

    private PoolManager pool;

    private void Awake()
    {
        pool = PoolManager.Instance;
    }

    public void Push()
    {
        OnBeforePush();
        pool.Push(this);
        OnAfterPush();
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