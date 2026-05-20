using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room : NetworkBehaviour
{
    public RoomBattleManager roomBattleManager;
    // public Image roomImage;
    // public TextMeshProUGUI roomText;
    public readonly SyncVar<Vector2Int> gridPos = new SyncVar<Vector2Int>();
    public readonly SyncVar<RoomType> roomType = new();
    public readonly SyncVar<bool> isBattle = new SyncVar<bool>();

    public readonly SyncVar<bool> isEntered = new SyncVar<bool>();
    [Header("玩家战斗点位")]
    public List<Transform> playerSpots = new List<Transform>();
    [Header("走廊和门")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;
    public GameObject corridorUp;
    public GameObject corridorDown;
    public GameObject corridorLeft;
    public GameObject corridorRight;

    public GameObject shadow;

    public RoomType _roomType;
    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.SetParent(BattleSceneManager.Instance.RoomContainer.transform);
        gridPos.OnChange += GridPos_OnChange;
        roomType.OnChange += RoomType_OnChange;
        //this.transform.SetParent(MapManager.Instance.content.transform, false);
    }



    public override void OnStopClient()
    {
        base.OnStopClient();
        gridPos.OnChange -= GridPos_OnChange;
        roomType.OnChange -= RoomType_OnChange;
    }

    private void GridPos_OnChange(Vector2Int prev, Vector2Int next, bool asServer)
    {
        //roomText.text = gridPos.Value.ToString();
    }
    private void RoomType_OnChange(RoomType prev, RoomType next, bool asServer)
    {
        _roomType = next;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            var pi = collision.gameObject.GetComponent<PlayerInstance>();
            if (IsServerStarted)
            {
                ServerPlayerIn(pi);
            }
            else
            {

            }
            if (pi.IsOwner)
            {
                ClientPlayerIn(pi);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var pi = collision.gameObject.GetComponent<PlayerInstance>();
            if (IsServerStarted)
            {
                ServerPlayerOut(pi);
            }
            else
            {

            }
            if (pi.IsOwner)
            {
                ClientPlayerOut(pi);
            }

        }
    }
    [Server]
    public void ServerPlayerIn(PlayerInstance player)
    {
        Debug.Log("服务端检测到有玩家进入房间");
        if (roomBattleManager.HaveEnemy())
        {
            Debug.Log("有敌人,准备战斗");
            roomBattleManager.characterList.Add(player);
            roomBattleManager.ServerStartBattle();
            TargetRpcPlayerToStartBattle(player.Owner, roomBattleManager.PlayerNum());
        }
        else
        {
            Debug.Log("无敌人,正常");
        }
    }
    public void ServerPlayerOut(PlayerInstance player)
    {
        roomBattleManager.characterList.Remove(player);
        Debug.Log("服务端检测到有玩家离开房间");
    }
    [TargetRpc]
    public void TargetRpcPlayerToStartBattle(NetworkConnection conn, int i)
    {
        NManager.Instance.player.playerInstance.playerCon.MoveToTransform(playerSpots[i]);
        roomBattleManager.ClientStartBattle();
    }
    [Client]
    public void ClientPlayerIn(PlayerInstance player)
    {
        shadow.gameObject.SetActive(false);
    }
    [Client]
    public void ClientPlayerOut(PlayerInstance player)
    {
        shadow.gameObject.SetActive(true);
    }

}
