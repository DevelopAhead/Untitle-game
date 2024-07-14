using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.Linq;
public class GameManager : MonoInstance<GameManager>
{
    public static Subject<int> OnSizeChanged = new Subject<int>();
    public static Subject<Unit> OnGameStart = new Subject<Unit>();
    public static Subject<GameData> OnGameDataCreated = new Subject<GameData>();
    public static Subject<GameData> OnGameOver = new Subject<GameData>();
    [SerializeField] Card _cardPrefab;
    [SerializeField] GameObject _cardRef;
    [SerializeField] Vector3 _cardOffset;
    [SerializeField]CinemachineTargetGroup _targetGroup;

    [SerializeField] Vector2 _cardSize;
    [SerializeField] Vector2 _cardScale;
    [SerializeField] Vector2 _scale;
    [SerializeField]List<Card> _cardList = new List<Card>();
    List<Card> _cardCompareList = new List<Card>();

    public GameData GameData;
    //Rule
    private void Start()
    {
        Application.targetFrameRate = 60;
        var go = Instantiate(_cardRef,new Vector3(0,-100,0),Quaternion.identity);
        _cardSize = go.GetComponent<MeshFilter>().mesh.bounds.size;
        _cardScale = go.transform.localScale;
        _scale = new Vector2(_cardSize.x*_cardScale.x, _cardSize.y*_cardScale.y);
       AddListener();
       // GenerateCard();
    }
    void AddListener()
    {
        Card.OnShowCard.Subscribe(card =>
        {
            _cardCompareList.Add(card);

            if (_cardCompareList.Count >= 2)
            {
                CompareCard(_cardCompareList[0], _cardCompareList[1]);
                _cardCompareList.Clear();
            }
            Save();

        }).AddTo(this);
    }
    public void CreateGame(int width,int height)
    {
        this.GameData = new GameData
        {
            Width = width,
            Height = height
        };
        GameData.NumberList = new List<int>();
        for (int i = 0; i < (GameData.Width * GameData.Height) * .5f; i++)
        {
            GameData.NumberList.Add(i);
            GameData.NumberList.Add(i);
        }
        Utils.ShuffleList(GameData.NumberList);

        StartGame();
    }
    public void StartGame()
    {
        _cardCompareList.Clear();
        DestroyAllCard();
        GenerateCard();
        GameSizeChanged();
        OnGameStart.OnNext(default);
        OnGameDataCreated.OnNext(GameData);
    }
    
    async void CompareCard(Card firstCard,Card secondCard)
    {
        await UniTask.Delay(250);
        if(firstCard.CardData.Number.Equals(secondCard.CardData.Number))
        {
            Correct();
            GameData.NumberList[firstCard.CardData.Index] = -1;
            GameData.NumberList[secondCard.CardData.Index] = -1;
            _cardList.Remove(firstCard);
            _cardList.Remove(secondCard);
            firstCard.AnimationCorrectAndRemoveCard();
            secondCard.AnimationCorrectAndRemoveCard();
            CheckGameState();
        }
        else
        {
            InCorrect();
            firstCard.Flip(500);
            secondCard.Flip(500);
        }
        GameData.Turn++;
        Save();
    }
    async void CheckGameState()
    {
        if (GameData.NumberList.All(index => index == -1))
        {
            await UniTask.Delay(1000);
            SoundManager.Instance.PlaySoundFX(SoundKeys.WIN);
            OnGameOver.OnNext(GameData);
        }
    }
    public void ResetGame()
    {
        GameData.Score = 0;
        GameData.Combo = 1;
        GameData.Turn = 1;
        CreateGame(GameData.Width, GameData.Height);
    }
    void Correct()
    {
        SoundManager.Instance.PlaySoundFX(SoundKeys.CORRECT);
        GameData.Score += GameData.Combo;
        GameData.Combo++;
    }
    void InCorrect()
    {
        SoundManager.Instance.PlaySoundFX(SoundKeys.INCORRECT);
        GameData.Combo = 1;
    }
    public void Save()
    {
        SaveManager.Instance.SaveGameData(GameData);
    }
    public void Load()
    {
        GameData = SaveManager.Instance.LoadData();
        StartGame();
    }
    
    void GenerateCard()
    {
        int index = 0;
        ObjectPool.SetItemIndex(ObjectPool.IndexOf("Card"));
        
        for (int i = 0; i < GameData.Width; i++)
        {
            for (int j = 0; j < GameData.Height; j++)
            {
                Debug.Log(GameData.NumberList[index]);
                if (GameData.NumberList[index] >= 0)
                {
                    var card = ObjectPool.Instance.GetCard();
                    card.transform.position = new Vector3((_scale.x + _cardOffset.x) * i, _cardOffset.y, (_scale.y + _cardOffset.z) * j);
                    card.transform.rotation = Quaternion.Euler(0, 0, 180);
                    card.SetupCardData(new CardData { Number = GameData.NumberList[index], Index = index ,IsLock = false,IsFlip = false});
                    _targetGroup.AddMember(card.transform, 1, 0);
                    _cardList.Add(card);
                }
                index++;
            }
        }
    }
    void DestroyAllCard()
    {
        foreach (var item in _cardList)
        { 
            _targetGroup.RemoveMember(item.transform);
            item.Release();
        }
        _cardList.Clear();
    }
    void GameSizeChanged()
    {
        OnSizeChanged.OnNext((GameData.Width > GameData.Height ? GameData.Width : GameData.Height));
    }

    
}
