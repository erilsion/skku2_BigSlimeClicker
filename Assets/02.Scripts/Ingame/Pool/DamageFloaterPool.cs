using UnityEngine;
using System.Collections.Generic;

public class DamageFloaterPool : MonoBehaviour
{
    [SerializeField] private DamageFloater _prefab;
    [SerializeField] private int _damageTextCount = 30;

    private readonly Queue<DamageFloater> _queue = new();
    private Transform _root;
    private bool _initialized;

    public void Initialize(Transform root)
    {
        if (_initialized) 
        {
            return;
        }
        _initialized = true;

        _root = root;

        if (_prefab == null)
        {
            return;
        }

        for (int i = 0; i < _damageTextCount; i++)
        {
            var item = CreateNew();
            Release(item);
        }
    }

    private DamageFloater CreateNew()
    {
        var item = Object.Instantiate(_prefab, _root);
        item.BindPool(this);
        item.gameObject.SetActive(false);
        return item;
    }

    public DamageFloater Get(Vector3 position)
    {
        if (_prefab == null)
        {
            return null;
        }

        var item = _queue.Count > 0 ? _queue.Dequeue() : CreateNew();

        item.transform.SetParent(null);
        item.transform.position = position;
        item.gameObject.SetActive(true);

        return item;
    }

    public void Release(DamageFloater item)
    {
        if (item == null)
        {
            return;
        }

        item.gameObject.SetActive(false);
        item.transform.SetParent(_root);

        _queue.Enqueue(item);
    }
}
