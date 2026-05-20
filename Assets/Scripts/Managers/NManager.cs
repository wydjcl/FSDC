using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 我自己的 网络管理器
/// </summary>
public class NManager : NetworkBehaviour
{
    public static NManager Instance;
    public Player player;
    public readonly SyncList<Player> players = new SyncList<Player>();

    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        players.OnChange += Players_OnChange;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        players.OnChange -= Players_OnChange;
    }

    private void Players_OnChange(SyncListOperation op, int index, Player oldItem, Player newItem, bool asServer)
    {
        if (asServer)
        {
            return;
        }
        if (op == SyncListOperation.Add)
        {
            PrefabFactory.Instance.CreatePlayerConnInLobbyPrefab(newItem);
        }
        if (op == SyncListOperation.RemoveAt)
        {
            Destroy(newItem.playerConnInLobby.gameObject);
        }
    }

    public void GetPlayer(Player p)
    {
        players.Add(p);
        Debug.Log("有一个玩家加入,现在玩家总数为" + players.Count);
    }
}
