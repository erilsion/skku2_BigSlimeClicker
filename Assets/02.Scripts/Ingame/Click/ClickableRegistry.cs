using UnityEngine;
using System.Collections.Generic;

public static class ClickableRegistry
{
    private static readonly List<IClickable> _list = new();

    public static IReadOnlyList<IClickable> List => _list;

    public static void Register(IClickable clickable)
    {
        if (clickable == null)
        {
            return;
        }
        if (!_list.Contains(clickable))
        {
            _list.Add(clickable);
        }
    }

    public static void Unregister(IClickable clickable)
    {
        if (clickable == null)
        {
            return;
        }
        _list.Remove(clickable);
    }
}
