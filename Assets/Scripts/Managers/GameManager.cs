using DG.Tweening;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using LiteNetLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
/// <summary>
/// 一些需要调用动态数据或方法的单例
/// </summary>
public class GameManager : MonoBehaviour
{
    public CardLayout cardLayout;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        DOTween.SetTweensCapacity(500, 200);
    }

}
public class ObservableList<T>
{
    public List<T> list = new List<T>();

    public Action<T> OnAdd;
    public Action<T> OnRemove;

    public void Add(T item)
    {
        list.Add(item);
        OnAdd?.Invoke(item);
    }

    public void Remove(T item)
    {
        if (list.Remove(item))
        {
            OnRemove?.Invoke(item);
        }
    }
}