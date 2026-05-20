using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : NetworkBehaviour
{
    public PlayerConnInLobby playerConnInLobby;
    private float pingTimer;
    public readonly SyncVar<string> playerName = new SyncVar<string>();
    //public readonly SyncVar<Room> currentRoom = new SyncVar<string>();
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerStarted)
        {
            NManager.Instance.GetPlayer(this);
        }
        if (IsOwner)
        {
            NManager.Instance.player = this;
            name = "本人玩家";
        }
        else
        {
            name = "非本人玩家";
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (playerConnInLobby != null)
        {
            Destroy(playerConnInLobby.gameObject);
        }
    }

    private void Update()
    {
        ClientDebugPing();
    }

    #region 延迟输出
    public void ClientDebugPing()
    {
        if (IsOwner)
        {
            if (IsServerStarted)
            {
                return;
            }
            if (playerConnInLobby != null)
            {
                if (playerConnInLobby.gameObject.activeSelf)
                {
                    pingTimer += Time.deltaTime;

                    if (pingTimer >= 2f)
                    {
                        pingTimer -= 2f; // 用减法更稳（避免累计误差）
                        float p = InstanceFinder.TimeManager.RoundTripTime;
                        DebugPingRPC(p);
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DebugPingRPC(float p)
    {
        DebugPing(p);
    }
    [ObserversRpc]
    public void DebugPing(float p)
    {
        if (playerConnInLobby != null)
        {
            if (playerConnInLobby.gameObject.activeSelf)
            {
                playerConnInLobby.pingText.text = p + "ms";
            }
        }
    }
    #endregion
}
