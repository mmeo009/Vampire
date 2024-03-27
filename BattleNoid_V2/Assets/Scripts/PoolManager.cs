using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            return false;
        }

        _pools[go.name].Push(go);
        return true;
    }

    public void Clear()
    {
        _pools.Clear();
    }

    void CreatePool(GameObject original)
    {
        Pool pool = new Pool(original);
        _pools.Add(original.name, pool);
    }
    public void Destroy(GameObject go)
    {
        if (go == null) return;
        if (Push(go)) return;

        Object.Destroy(go);
    }
}
class Pool
{
    GameObject _prefabs;
    IObjectPool<GameObject> _pool;

    Transform _root;
    Transform Root
    {
        get
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefabs.name}Pool" };
                _root = go.transform;
            }

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefabs = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(_prefabs);
        go.transform.SetParent(Root);
        go.name = _prefabs.name;
        return go;
    }
    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }
    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }
    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}