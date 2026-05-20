using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : NetworkBehaviour
{
    public static BattleSceneManager Instance;
    public NetworkObject playerInstancePrefab;
    public GameObject CharacterContainer;
    public GameObject RoomContainer;
    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerStarted)
        {
            foreach (var player in NManager.Instance.players)
            {
                var pi = Instantiate(playerInstancePrefab).GetComponent<PlayerInstance>();
                Spawn(pi.NetworkObject, player.Owner);
            }
        }

    }
}
