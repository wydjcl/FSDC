using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialSceneUIManager : SingletonMono<InitialSceneUIManager>
{
    private NetworkManager networkManager;
    public GameObject root;
    public GameObject lobby;
    public GameObject playerConnContainer;
    public GameObject hostButton;
    public GameObject clientButton;
    public GameObject disconnectButton;
    public GameObject startGameButton;
    public TMP_InputField inputField;
    //public InitialSceneUI ui;
    //private string ipAddress = "localhost";
    #region 生命周期和网络回调

    protected override void Awake()
    {
        base.Awake();
        if (networkManager == null)
        {
            networkManager = InstanceFinder.ServerManager.NetworkManager;
        }
    }
    public void Start()
    {
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;
        InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
        InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
        //SaveManager.LoadOrCreate(0);
    }

    public void OnDisable()
    {
        //InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;
        //InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
        //InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }
    private void OnServerConnectionState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            OnServerStarted();
        }
    }
    /// <summary>
    /// 服务端初始化逻辑
    /// </summary>
    void OnServerStarted()
    {
        Debug.Log("服务端初始化逻辑！");
    }

    /// <summary>
    /// 客户端执行,连接成功逻辑
    /// </summary>
    /// <param name="args"></param>
    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            //Debug.Log("客户端连接成功！");
            MyClientConnected();
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            //Debug.Log("客户端断开连接！");
            MyClientDisconnected();
        }
    }
    /// <summary>
    /// 客户端连接成功本地逻辑
    /// </summary>
    void MyClientConnected()
    {
        // 客户端连接成功后的逻辑
        //Debug.Log("客户端连接成功后的逻辑！");
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        inputField.gameObject.SetActive(false);
        disconnectButton.SetActive(true);
        ////ui.chooseBox.SetActive(true);
        if (InstanceFinder.IsServerStarted)
        {
            startGameButton.SetActive(true);
        }
    }
    /// <summary>
    /// 客户端断开连接 本地逻辑
    /// </summary>
    void MyClientDisconnected()
    {
        // 客户端连接成功后的逻辑
        Debug.Log("客户端断开成功后的逻辑！");
        hostButton.SetActive(true);
        clientButton.SetActive(true);
        inputField.gameObject.SetActive(true);
        disconnectButton.SetActive(false);
        startGameButton.SetActive(false);

        // UnloadAllExcept("InitialScene");
        //ui.chooseBox.SetActive(false);
    }
    /// <summary>
    /// 服务端监测客户端连接
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="args"></param>
    private void OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log($"客户端连入服务器,客户端ConnID：{conn.ClientId}");
            //OnClientConnected(conn);
        }
        else if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            Debug.Log($"客户端断开连接,客户端ConnID：{conn.ClientId}");
            //OnClientDisconnected(conn);
        }
    }
    #endregion

    #region UI的方法

    public void StartHost()
    {
        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
    }
    public void StartClinet(string ipAddress)
    {
        var transport = networkManager.TransportManager.Transport;

        transport.SetClientAddress(ipAddress); // 👈 关键！

        networkManager.ClientManager.StartConnection();
    }
    public void StartDisconnect()
    {
        networkManager.ServerManager.StopConnection(true);
        networkManager.ClientManager.StopConnection();
    }

    /// <summary>
    /// 服务端调用,开始游戏
    /// </summary>
    public void StartGame()
    {
        // 配置加载数据
        SceneLoadData loadData = new SceneLoadData("MainScene");
        //SceneLoadData loadData = new SceneLoadData("BattleScene");

        // 可选：叠加场景（非单一替换）
        // loadData.Options.Merge = true;

        // 服务器全局加载 → 所有客户端自动同步
        InstanceFinder.SceneManager.LoadGlobalScenes(loadData);
        // GameManager.Instance.player.DisableInitUI();

    }
    public void UnloadAllExcept(string keepScene)
    {
        //ui.gameObject.SetActive(true);//特殊化处理,开启初始场景的时候把UI打开
        int count = UnityEngine.SceneManagement.SceneManager.sceneCount;

        for (int i = 0; i < count; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

            if (scene.name != keepScene)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
    #endregion
    public void ClickStartLobby()
    {
        lobby.SetActive(true);
    }
    public void ClickQuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
    public void ClickStartHostButton()
    {
        StartHost();
    }

    public void ClickStopConnectionButton()
    {
        StartDisconnect();
    }

    public void ClickStartClientButton()
    {
        if (inputField.text == "")
        {
            Debug.Log("IP默认修改为:localhost");
            StartClinet("localhost");
            return;
        }
        StartClinet(inputField.text);
    }

    public void ClickQuitLobbyButtom()
    {
        StartDisconnect();
        lobby.SetActive(false);
    }

    public void ClickStartGameButton()
    {
        // NSceneManager.Instance.LoadBattleScene();
        NSceneManager.Instance.LoadMainScene();
    }
}
