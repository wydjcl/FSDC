using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : NetworkBehaviour
{
    public readonly SyncVar<bool> isDead = new SyncVar<bool>();
    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.SetParent(BattleSceneManager.Instance.CharacterContainer.transform);
    }
}
