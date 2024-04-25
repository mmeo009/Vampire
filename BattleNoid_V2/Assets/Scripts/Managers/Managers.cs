using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            if(Application.isPlaying)
            {
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
            }
        }
    }

    [SerializeField] DataManager _data = new DataManager();
    [SerializeField] MonsterManager _monster = new MonsterManager();
    [SerializeField] PoolManager _pool = new PoolManager();
    [SerializeField] UIManager _UI = new UIManager();
    [SerializeField] PlayerManager _player = new PlayerManager();
    [SerializeField] CardManager _card = new CardManager();
    public static DataManager Data { get { return Instance?._data; } }
    public static MonsterManager Monster { get { return Instance?._monster; } }
    public static PoolManager Pool { get {  return Instance?._pool; } }
    public static UIManager UI { get {  return Instance?._UI; } }
    public static PlayerManager Player { get { return Instance?._player; } }
    public static CardManager Card { get { return Instance?._card; } }
}
