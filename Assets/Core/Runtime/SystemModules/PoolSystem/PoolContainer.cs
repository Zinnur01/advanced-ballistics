using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : IEnumerable, IEnumerable<PoolObject>
{
    private Transform parent;
    private Stack<PoolObject> stack;
    private PoolObject template;
    private string templateID;

    public PoolContainer(PoolObject template)
    {
        stack = new Stack<PoolObject>();

        this.template = template;
        templateID = template.GetID();

        parent = new GameObject($"[Pool Container] - {template.name}").transform;
        parent.SetParent(PoolManager.Instance.transform);
    }

    public void Push(PoolObject value)
    {
        value.gameObject.SetActive(false);
        stack.Push(value);
        value.transform.SetParent(parent);
    }

    public PoolObject Pop()
    {
        if (stack.Count > 0)
        {
            PoolObject value = stack.Pop();
            value.gameObject.SetActive(true);
            value.transform.SetParent(null);
            return value;
        }
        PoolObject newValue = GameObject.Instantiate(template);
        newValue.SetID(templateID);
        newValue.transform.SetParent(null);
        return newValue;
    }

    #region [IEnumerable / IEnumerable<T> Implementation]
    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)stack).GetEnumerator();
    }

    IEnumerator<PoolObject> IEnumerable<PoolObject>.GetEnumerator()
    {
        return stack.GetEnumerator();
    }
    #endregion
}