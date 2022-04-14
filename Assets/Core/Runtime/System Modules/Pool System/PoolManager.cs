using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, PoolContainer> pool = new Dictionary<string, PoolContainer>();

    #region [Push]
    public void Push(PoolObject value)
    {
        if (value != null)
        {
            string id = value.GetID();
            PoolContainer container = null;

            if (!pool.TryGetValue(id, out container))
            {
                container = new PoolContainer(value);
                pool.Add(id, container);
            }

            container.Push(value);
        }
    }
    #endregion

    #region [Pop]
    public T CreateOrPop<T>(PoolObject template) where T : PoolObject
    {
        if (template != null)
        {
            string id = template.GetID();
            PoolContainer container = null;

            if (!pool.TryGetValue(id, out container))
            {
                container = new PoolContainer(template);
                pool.Add(id, container);
            }

            PoolObject value = container.Pop();
            return value as T;
        }
        return null;
    }

    public T CreateOrPop<T>(PoolObject template, Vector3 position, Quaternion rotation) where T : PoolObject
    {
        T value = CreateOrPop<T>(template);
        if (value != null)
        {
            value.transform.position = position;
            value.transform.rotation = rotation;
        }
        return value;
    }
    #endregion
}
