using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NSceneManager : NetworkBehaviour
{
    public static NSceneManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #region 基础方法
    public void LoadGlobalScene(string s)
    {
        SceneLoadData loadData = new SceneLoadData(s);
        // 可选：叠加场景（非单一替换）
        // loadData.Options.Merge = true;
        InstanceFinder.SceneManager.LoadGlobalScenes(loadData);
    }
    public void LoadGlobalScene(string s, Action callback)
    {
        SceneLoadData loadData = new SceneLoadData(s);
        InstanceFinder.SceneManager.LoadGlobalScenes(loadData);
        callback?.Invoke();

    }
    public void UnloadGlobalScene(string s)
    {
        DespawnAllObjectByScene(s);
        SceneUnloadData data = new SceneUnloadData(s);
        InstanceFinder.SceneManager.UnloadGlobalScenes(data);
    }
    public void UnloadGlobalScene(string s, Action callback)
    {
        DespawnAllObjectByScene(s);
        SceneUnloadData data = new SceneUnloadData(s);
        InstanceFinder.SceneManager.UnloadGlobalScenes(data);
        callback?.Invoke();
    }
    public void DespawnAllObjectByScene(string sceneName)
    {
        var all = InstanceFinder.ServerManager.Objects.Spawned.Values;

        // ✔ 先拷贝一份（避免边遍历边修改）
        List<NetworkObject> list = new List<NetworkObject>(all);

        foreach (var nob in list)
        {
            if (!nob) continue;

            if (nob.gameObject.scene.name == sceneName)
            {
                InstanceFinder.ServerManager.Despawn(nob);
            }
        }
    }
    #endregion
    public void LoadBattleScene()
    {
        LoadGlobalScene("BattleScene", () => { DisableMainUIRoot(); });
    }
    public void LoadMainScene()
    {
        Debug.Log("开启主场景");
        LoadGlobalScene("MainScene", () => { DisableInitialUIRoot(); });
    }
    [ObserversRpc]
    public void DisableInitialUIRoot()
    {
        InitialSceneUIManager.Instance.root.SetActive(false);
    }
    [ObserversRpc]
    public void DisableMainUIRoot()
    {
        MainUIManager.Instance.root.SetActive(false);
        BagUI.Instance.root.SetActive(false);
    }

}
