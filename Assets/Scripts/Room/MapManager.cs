using DG.Tweening;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 管理房间生成
/// </summary>
public class MapManager : NetworkBehaviour
{
    //房间与与房间距离 ,36,25

    public static MapManager Instance;
    public MapDataSO mapData;
    public NetworkObject roomPrefab;
    // public NetworkObject roomObjectPrefab;
    //public GameObject corridorH;
    //public GameObject corridorS;

    public GameObject mapGameObject;
    public GameObject viewport;
    public GameObject content;

    //public GameObject exitButtom;
    public int seed;
    public System.Random rng;
    public readonly SyncVar<int> roomsCount = new SyncVar<int>();
    public readonly SyncDictionary<Vector2Int, Room> rooms = new();
    public int width = 10;
    public int height = 10;
    public int mainPathLength = 8;
    public float branchChance = 0.4f;

    public bool haveOpenDoor = false;

    private Vector2Int[] dirs = new Vector2Int[]
  {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
  };
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (!haveOpenDoor)
        {
            // Debug.Log("请求开门");
            // Debug.Log($"字典房间数:{rooms.Count},总计房间数:{roomsCount.Value}");
            if (rooms.Count >= roomsCount.Value && roomsCount.Value > 0)
            {
                OpenAllDoors();
                haveOpenDoor = true;
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerStarted)
        {
            seed = (int)(Random.value * int.MaxValue);
            Debug.Log("种子为:" + seed);
            rng = new System.Random(seed);
            Generate();
        }
    }
    [ContextMenu("生成地图")]
    public void Generate()
    {
        if (!IsServerStarted)
        {
            Debug.LogError("必须服务器生成！");
            return;
        }

        rooms.Clear();

        // ===== 1. 主路径 =====
        List<Vector2Int> mainPath = GenerateMainPath();

        //  ===== 2.分支 =====
        foreach (var pos in mainPath)
        {
            if (Random.value < branchChance)
                GenerateBranch(pos, Random.Range(2, 4));
        }


        // ===== 3. 创建房间 =====
        var keys = new List<Vector2Int>(rooms.Keys);
        foreach (var kv in keys)
        {
            CreateRoomObject(kv);
        }
        // rooms[new Vector2Int(0, 0)].canExplore.Value = true;
        SetRandomExit();
        SetBoss();
        SetChests();
        roomsCount.Value = rooms.Count;
        Debug.Log($"生成房间数量: {rooms.Count}");
        ClientStart();
        //  Invoke("OpenAllDoors", 1.5f);
    }
    [ObserversRpc]
    public void ClientStart()
    {
        // rooms[new Vector2Int(0, 0)].ServerClick(GameManager.Instance.player);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ClientMoveToRoomRpc(Player player, Vector2Int pos)
    {
        if (rooms.ContainsKey(pos))
        {
            //rooms[pos].ServerClick(player);
        }
    }

    // =========================
    // 主路径（随机游走）
    // =========================
    private List<Vector2Int> GenerateMainPath()
    {
        List<Vector2Int> path = new();

        Vector2Int pos = Vector2Int.zero;
        path.Add(pos);
        rooms[pos] = null;

        for (int i = 0; i < mainPathLength; i++)
        {
            Vector2Int dir = dirs[rng.Next(0, dirs.Length)];
            Vector2Int next = pos + dir;

            if (rooms.ContainsKey(next))
                continue;

            pos = next;
            path.Add(pos);
            rooms[pos] = null;
        }

        return path;
    }

    // =========================
    // 分支生成
    // =========================
    private void GenerateBranch(Vector2Int start, int length)
    {
        Vector2Int pos = start;

        for (int i = 0; i < length; i++)
        {
            Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
            Vector2Int next = pos + dir;

            if (rooms.ContainsKey(next))
                break;

            rooms[next] = null;
            pos = next;
        }
    }

    // =========================
    // 创建房间对象
    // =========================
    private void CreateRoomObject(Vector2Int pos)
    {
        bool up = rooms.ContainsKey(pos + Vector2Int.up);
        bool down = rooms.ContainsKey(pos + Vector2Int.down);
        bool left = rooms.ContainsKey(pos + Vector2Int.left);
        bool right = rooms.ContainsKey(pos + Vector2Int.right);

        RoomType type = GetRoomType(pos);

        Vector3 worldPos = new Vector3(pos.x * width, pos.y * height, 0);

        NetworkObject obj = Instantiate(roomPrefab, worldPos, Quaternion.identity);
        Room room = obj.GetComponent<Room>();


        //room.Init(type, pos);

        room.gridPos.Value = pos;
        room.roomType.Value = type;

        Spawn(obj.gameObject, null, gameObject.scene);

        rooms[pos] = room;

        //  RoomObject roomObj = Instantiate(roomObjectPrefab).GetComponent<RoomObject>();
        //  roomObj.transform.position = new Vector2(pos.x * 28, pos.y * 18);

        //room.roomObject.Value = roomObj;
        //  roomObj.room.Value = room;

        //   Spawn(roomObj.NetworkObject, null, gameObject.scene);

    }

    // =========================
    // 房间类型分配
    // =========================
    private RoomType GetRoomType(Vector2Int pos)
    {
        // 起点
        if (pos == Vector2Int.zero)
            return RoomType.Start;

        // 最远点当出口
        //if (pos.magnitude > 6)
        //    return RoomType.Exit;

        // 随机类型
        int r = rng.Next(0, 100);
        return RoomType.Chest;
        if (r < 30f) return RoomType.Normal;
        if (r < 60f) return RoomType.Chest;
        if (r < 65f) return RoomType.Task;
        if (r < 70f) return RoomType.Exit;//TODO至少弄几个离开通道
        return RoomType.Normal;
    }
    public void SetRandomExit()
    {
        List<Room> candidates = new List<Room>();

        foreach (var kv in rooms)
        {
            Room room = kv.Value;

            if (room == null) continue;

            // ❌ 排除 start 和 boss
            if (room.roomType.Value == RoomType.Start) continue;
            if (room.roomType.Value == RoomType.Boss) continue;

            candidates.Add(room);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("没有可用房间可设置 Exit");
            return;
        }
        int index = rng.Next(0, candidates.Count);
        Room selected = candidates[index];

        selected.roomType.Value = RoomType.Exit;
        Debug.Log("逃生房间为" + selected.gridPos.Value);
    }

    public void SetBoss()
    {
        List<Room> candidates = new List<Room>();

        foreach (var kv in rooms)
        {
            Room room = kv.Value;

            if (room == null) continue;

            // ❌ 排除 start 和 boss
            if (room.roomType.Value == RoomType.Start) continue;
            if (room.roomType.Value == RoomType.Exit) continue;

            candidates.Add(room);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("没有可用房间可设置Boss");
            return;
        }
        int index = rng.Next(0, candidates.Count);
        Room selected = candidates[index];

        selected.roomType.Value = RoomType.Boss;
        Debug.Log("Boss房间为" + selected.gridPos.Value);
    }

    public void SetChests()
    {
        foreach (var r in rooms)
        {
            if (r.Value.roomType.Value == RoomType.Chest)
            {
                // 1️⃣ 随机生成 1~4 个宝箱
                int chestCount = rng.Next(1, 5);

                for (int i = 0; i < chestCount; i++)
                {
                    float x = rng.Next(-8, 8);
                    float y = rng.Next(-3, 3);

                    Vector3 spawnPos = new Vector3(x + r.Value.transform.position.x, y + r.Value.transform.position.y, -0.1f);

                    var c = Instantiate(PrefabFactory.Instance.chestPrefab, spawnPos, Quaternion.identity).GetComponent<Chest>();

                    //c.room.Value = r.Value;

                    Spawn(c, null, this.gameObject.scene);

                    c.InitChest(mapData.GenerateDrops());
                }
            }
        }
    }
    /// <summary>
    /// 点击房间后开放周围房间
    /// </summary>
    /// <param name="pos"></param>
    [Server]
    public void OpenAroundRooms(Vector2Int pos)
    {
        Vector2Int[] dirs8 = new Vector2Int[]
    {
       // new Vector2Int(-1,  1), // 左上
        new Vector2Int( 0,  1), // 上
        //new Vector2Int( 1,  1), // 右上

        new Vector2Int(-1,  0), // 左
        new Vector2Int( 1,  0), // 右

       // new Vector2Int(-1, -1), // 左下
        new Vector2Int( 0, -1), // 下
        //new Vector2Int( 1, -1), // 右下
    };
        // rooms[pos].explored.Value = true;
        foreach (var dir in dirs8)
        {
            Vector2Int target = pos + dir;

            if (rooms.ContainsKey(target))
            {
                //  rooms[target].canExplore.Value = true;
            }
        }
    }
    // [ObserversRpc]
    public void OpenAllDoors()
    {
        Debug.Log("开门");
        foreach (var room in rooms.Values)
        {
            OpenDoor(room);
        }
    }
    public void OpenDoor(Room r)
    {
        if (r.roomType.Value == RoomType.Start)
        {
            r.shadow.SetActive(false);
        }
        Vector2Int pos = r.gridPos.Value;

        // 上
        if (rooms.ContainsKey(pos + Vector2Int.up))
        {
            r.doorUp.SetActive(false);
            r.corridorUp.SetActive(true);
            //Instantiate(corridorS, r.roomObject.Value.transform.position + Vector3.up * 9, Quaternion.identity);
        }


        // 下
        if (rooms.ContainsKey(pos + Vector2Int.down))
        {
            r.doorDown.SetActive(false);
            r.corridorDown.SetActive(true);
            //r.roomObject.Value.doorDown.SetActive(false);
            //Instantiate(corridorS, r.roomObject.Value.transform.position + Vector3.up * -9, Quaternion.identity);
        }


        // 左
        if (rooms.ContainsKey(pos + Vector2Int.left))
        {
            r.doorLeft.SetActive(false);
            r.corridorLeft.SetActive(true);
            //r.roomObject.Value.doorLeft.SetActive(false);
            //Instantiate(corridorH, r.roomObject.Value.transform.position + Vector3.right * -14, Quaternion.identity);
        }

        // 右
        if (rooms.ContainsKey(pos + Vector2Int.right))
        {
            r.doorRight.SetActive(false);
            r.corridorRight.SetActive(true);
            //r.roomObject.Value.doorRight.SetActive(false);
            //Instantiate(corridorH, r.roomObject.Value.transform.position + Vector3.right * 14, Quaternion.identity);
        }


    }

    public void DebugRoom(Room r)
    {

    }
}
