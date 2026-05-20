using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : NetworkBehaviour
{
    public readonly SyncVar<Room> room = new SyncVar<Room>();
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    public BoxCollider2D boxCollider;
    [ServerRpc(RequireOwnership = false)]
    public void PlayerIn(PlayerInstance pi)
    {

        pi.currentRoom.Value = this.room.Value;
        ClientPlayerIn(pi.player.Owner);
    }
    [TargetRpc]
    public void ClientPlayerIn(NetworkConnection conn)
    {

    }
    [ServerRpc(RequireOwnership = false)]
    public void PlayerOut(PlayerInstance pi)
    {
        pi.currentRoom.Value = this.room.Value;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家进入房间");

            PlayerInstance player = other.gameObject.GetComponent<PlayerInstance>();
            if (player != null)
            {
                if (player.IsOwner)
                {
                    Debug.Log("是本玩家");
                    PlayerIn(player);
                    //CameraManager.Instance.cameraFollowTarget.roomPos = this.transform.position;
                    //CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Battle;

                }
                else
                {
                    Debug.Log("不是本玩家");
                }
            }
            else
            {
                Debug.Log("找不到身上挂在的脚本");
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家离开房间");
            PlayerInstance player = other.gameObject.GetComponent<PlayerInstance>();
            if (player != null)
            {
                if (player.IsOwner)
                {
                    PlayerOut(player);
                    //CameraManager.Instance.cameraFollowTarget.cameraType = CameraType.Map;
                }
            }
            else
            {
                Debug.Log("找不到身上挂在的脚本");
            }
        }
    }
}
