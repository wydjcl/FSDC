using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SaveManager : SingletonMono<SaveManager>
{
    //private static string SavePath =>
    //      Application.persistentDataPath + "/playerData.json";

    private string tip = "这里是玩家数据存档,你可以自由的修改数据,但请不要对版本号等进行修改";
    private string version = "0.04";
    private string playerName = "若叶睦";
    private int bagCount = 30;
    private int warehouseCount = 100;

    [ContextMenu("保存数据")]
    public void SaveTest()
    {
        Debug.Log("保存成功");
        SaveData.Instance.data.tip = tip;
        SaveData.Instance.data.version = version;
        SaveData.Instance.ConvertData();
        Save(SaveData.Instance.data, 0);
    }
    [ContextMenu("读取数据")]
    public void LoadTest()
    {
        Load(0);
    }
    [ContextMenu("删除数据")]
    public void DeleteTest()
    {
        DeleteSave();
    }
    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(0), json);
    }
    public static void Save(PlayerData data, int slot)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
    }

    // 读取
    public static void Load(int slot)
    {
        if (!File.Exists(GetPath(slot)))
        {
            Debug.Log("No save found, creating new data.");
            //return new PlayerData();
        }

        string json = File.ReadAllText(GetPath(slot));
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        SaveData.Instance.data = data;
    }

    // 删除存档
    public static void DeleteSave()
    {
        //if (File.Exists(SavePath))
        //    File.Delete(SavePath);
    }
    private static string GetPath(int slot)
    {
        return Application.persistentDataPath + $"/save_slot_{slot}.json";
    }

    public static void LoadOrCreate(int slot)
    {
        string path = GetPath(slot);

        // ✔ 1. 没有存档 → 创建新的
        if (!File.Exists(path))
        {
            Debug.Log($"Slot {slot} 不存在，创建新存档");

            PlayerData newData = new PlayerData();
            newData.tip = Instance.tip;
            newData.version = Instance.version;
            newData.playerName = Instance.playerName;
            newData.bagCount = Instance.bagCount;
            newData.warehouseCount = Instance.warehouseCount;

            Items kabao = new()
            {
                itemName = "卡包",
                amount = 3,
                index = 0,
            };
            Items yaoshui = new()
            {
                itemName = "小型治疗药水",
                amount = 3,
                index = 1,
            };
            Items yaoshui2 = new()
            {
                itemName = "小型治疗药水",
                amount = 3,
                index = 2,
            };
            newData.warehouse.Add(kabao);
            newData.warehouse.Add(yaoshui);
            newData.warehouse.Add(yaoshui2);
            //Debug.Log(newData.playerName);
            Save(newData, slot); // 可选：立即写入文件
        }
        else
        {
            Debug.Log("有存档,读取...");
        }
        Load(slot);
    }
}
[System.Serializable]
public class PlayerData
{
    [HideInInspector]
    public string tip;
    [HideInInspector]
    public string version;
    public string playerName;
    public int gold;
    public List<Items> warehouse = new List<Items>();//仓库数据,存储在本地,每次游戏开始时加载到内存,游戏结束时保存到本地
    public List<Items> bag = new List<Items>();//背包数据
    public List<Items> equips = new List<Items>();//装备
    public List<Items> vault = new List<Items>(); //保险库
    public int warehouseCount;
    public int bagCount;
}
[System.Serializable]
public class Items
{
    public string itemName;
    public int amount;
    public int index;
}
