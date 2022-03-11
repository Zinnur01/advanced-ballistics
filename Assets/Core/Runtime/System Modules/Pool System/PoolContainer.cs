using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : IEnumerable, IEnumerable<PoolObject>
{
    private Stack<PoolObject> stack;
    private PoolObject template;
    private string templateID;

    public PoolContainer(PoolObject template)
    {
        stack = new Stack<PoolObject>();
        this.template = template;
        templateID = template.GetID();
    }

    public void Push(PoolObject value)
    {
        value.gameObject.SetActive(false);
        stack.Push(value);
    }

    public PoolObject Pop()
    {
        if (stack.Count > 0)
        {
            PoolObject value = stack.Pop();
            value.gameObject.SetActive(true);
            return value;
        }
        PoolObject newValue = GameObject.Instantiate(template);
        newValue.SetID(templateID);
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