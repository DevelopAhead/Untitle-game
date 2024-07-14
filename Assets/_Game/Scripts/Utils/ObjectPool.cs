using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoInstance<ObjectPool>
{
    [SerializeField] Transform _transform;
    [SerializeField] List<Card> _cardList = new List<Card>();
    public ReactiveDictionary<string, ObjectPool<Card>> CardPool;
    int index = 0;
    public bool collectionChecks = true;
    public int capacity = 10;
    public int maxPoolSize = 20;
    public override void Init()
    {
        base.Init();
        CreatePool();
    }
    void CreatePool()
    {
       
        CardPool = new ReactiveDictionary<string, ObjectPool<Card>>();
        foreach (var item in _cardList)
        {
            var objectPool = new ObjectPool<Card>(CreateCard, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity, maxPoolSize);
            CardPool.Add(item.gameObject.name, objectPool);
            index++;
        }
    }

    private void OnDestroyPoolObject(Card card)
    {
        Destroy(card);
    }
    private void OnReturnedToPool(Card card)
    {
        card.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Card card)
    {
        card.gameObject.SetActive(true);
    }

    private Card CreateCard()
    {
        var go = Instantiate(_cardList[index], _transform, true);
        go.name = _cardList[index].name;
        go.ObjectPool = CardPool[go.name];
        return go;
    }
    public Card GetCard()
    {
        return CardPool["Card"].Get();
    }
    public static int IndexOf(string name)
    {
        if (!FindItemFormPool(name)) return -1;
        return Instance.CardPool.Keys.ToList().IndexOf(name);
    }
    public static void SetItemIndex(int newIndex)
    {
        Instance.index = newIndex;
        if (newIndex <= 0)
            Instance.index = 0;
    }
    public static bool FindItemFormPool(string name)
    {
        return Instance.CardPool.ContainsKey(name);
    }
}