using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.Utilities;
public class GameManager : MonoBehaviour
{
    public static Subject<int> OnSizeChanged = new Subject<int>();
    public int Width = 2;
    public int Height = 2;
    [SerializeField] Card _cardPrefab;
    [SerializeField] GameObject _cardRef;
    [SerializeField] Vector3 _cardOffset;
    [SerializeField]CinemachineTargetGroup _targetGroup;

    [SerializeField] Vector2 _cardSize;
    [SerializeField] Vector2 _cardScale;
    [SerializeField] Vector2 _scale;
    [SerializeField]List<Card> _cardList = new List<Card>();
    List<int> _numberList;
    private void Start()
    {
        
        var go = Instantiate(_cardRef,new Vector3(0,-100,0),Quaternion.identity);
        _cardSize = go.GetComponent<MeshFilter>().mesh.bounds.size;
        _cardScale = go.transform.localScale;
        _scale = new Vector2(_cardSize.x*_cardScale.x, _cardSize.y*_cardScale.y);
        AddListener();
       // GenerateCard();
    }
    void AddListener()
    {
        this.ObserveEveryValueChanged(_ => _.Width).Subscribe(_ =>
        {
            DestroyAllCard();
            GenerateCard();
            GameSizeChanged();
        });
        this.ObserveEveryValueChanged(_ => _.Height).Subscribe(_ =>
        {
            DestroyAllCard();
            GenerateCard();
            GameSizeChanged();
        });
    }
    void GenerateCard()
    {
        int index = 0;
        _numberList = new List<int>();
        for (int i = 0; i < (Width*Height)*.5f; i++)
        {
            _numberList.Add(i);
            _numberList.Add(i);
        }
        Utils.ShuffleList(_numberList);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var go = Instantiate(_cardPrefab, new Vector3((_scale.x + _cardOffset.x)*i, _cardOffset.y, (_scale.y + _cardOffset.z)*j),Quaternion.Euler(0,0,180));
                go.SetupCardData(new CardData { Number = _numberList[index] });
                _targetGroup.AddMember(go.transform, 1, 0);
                _cardList.Add(go);
                index++;
            }
        }
    }
    void DestroyAllCard()
    {
        foreach (var item in _cardList)
        { 
            _targetGroup.RemoveMember(item.transform);
            Destroy(item.gameObject);
        }
        _cardList.Clear();
    }
    void GameSizeChanged()
    {
        OnSizeChanged.OnNext((Width > Height ? Width : Height));
    }

    
}
