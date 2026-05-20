using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabFactory : MonoBehaviour
{
    public static PrefabFactory Instance;
    //public NetworkObject PlayerInstancePrefab;
    public GameObject PlayerConnInLobbyPrefab;
    public NetworkObject chestPrefab;
    private void Awake()
    {
        Instance = this;
    }
    #region 通用方法
    /// <summary>
    /// 生成预制体（通用）
    /// </summary>
    public T Create<T>(T prefab, Transform parent = null) where T : Component
    {
        T obj = Instantiate(prefab, parent);
        return obj;
    }

    public GameObject Create(GameObject prefab, Transform parent = null)
    {
        GameObject obj = Instantiate(prefab, parent);
        return obj;
    }
    public NetworkObject Create(NetworkObject prefab, Transform parent = null)
    {
        NetworkObject obj = Instantiate(prefab, parent);
        return obj;
    }

    /// <summary>
    /// 带位置旋转
    /// </summary>
    public T Create<T>(T prefab, Vector3 pos, Quaternion rot, Transform parent = null) where T : Component
    {
        T obj = Instantiate(prefab, pos, rot, parent);
        return obj;
    }
    #endregion

    public void CreatePlayerConnInLobbyPrefab(Player p)
    {
        var c = Create(PlayerConnInLobbyPrefab, InitialSceneUIManager.Instance.playerConnContainer.transform);
        var cc = c.GetComponent<PlayerConnInLobby>();
        p.playerConnInLobby = cc;
        cc.nameText.text = p.name;
        cc.player = p;
    }

    //public void CreatePlayerInstance()
    //{
    //    foreach (var p in NManager.Instance.players)
    //    {
    //        NetworkObject obj = Create(PlayerInstancePrefab);
    //        var pi = obj.GetComponent<PlayerInstance>();
    //        NManager.Instance.Spawn(obj, p.Owner, UnityEngine.SceneManagement.SceneManager.GetSceneByName("BattleScene"));
    //    }
    //}
}