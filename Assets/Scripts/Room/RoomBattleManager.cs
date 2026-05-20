using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomBattleManager : NetworkBehaviour
{
    public Room room;
    public readonly SyncList<Character> characterList = new SyncList<Character>();
    public List<Character> _characterList = new List<Character>();
    public bool isBattle;

    public override void OnStartClient()
    {
        base.OnStartClient();
        characterList.OnChange += CharacterList_OnChange;
    }

    private void CharacterList_OnChange(SyncListOperation op, int index, Character oldItem, Character newItem, bool asServer)
    {
        _characterList.Clear();
        _characterList.AddRange(characterList);
    }

    public int PlayerNum()
    {
        int n = -1;
        foreach (var c in characterList)
        {
            if (c is PlayerInstance)
            {
                n++;
            }
        }
        return n;
    }
    [Client]
    public bool HaveEnemy()
    {
        foreach (var c in characterList)
        {
            if (c is Enemy)
            {
                if (!c.isDead.Value)
                {
                    return true;
                }
            }
        }
        return false;
    }
    [Server]
    public void ServerStartBattle()
    {
        isBattle = true;
    }
    [Client]
    public void ClientStartBattle()
    {
        CameraManager.Instance.cameraFollowTarget.roomPos = this.transform.position;
        CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Battle;
    }
    [ContextMenu("服务端结束战斗")]
    public void ServerStopBattle()
    {
        isBattle = false;
        foreach (var c in characterList)
        {
            if (c is PlayerInstance)
            {
                ClientStopBattle(c.Owner);
            }
        }
    }
    [TargetRpc]
    public void ClientStopBattle(NetworkConnection conn)
    {
        Debug.Log("该客户端结束战斗");
        CameraManager.Instance.cameraFollowTarget.roomPos = this.transform.position;
        CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Map;
        NManager.Instance.player.playerInstance.playerCon.canMove = true;
    }

}
