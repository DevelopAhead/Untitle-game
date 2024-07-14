using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using TMPro;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using UnityEngine.Pool;
using UniRx.Toolkit;
using Cysharp.Threading.Tasks;
public class Card : MonoBehaviour
{
    public static Subject<Card> OnShowCard = new Subject<Card>();
    // Start is called before the first frame update
    [SerializeField] TextMeshPro _textMesh;
    public CardData CardData { get; private set; }
    IObjectPool<Card> _objectPool;
    public IObjectPool<Card> ObjectPool { set => _objectPool = value; }
    void Start()
    {
        this.ObserveEveryValueChanged(_ => _.CardData).Subscribe(cardData =>
        {
            _textMesh.text = CardData.Number.ToString();
        }).AddTo(this);
    }
    public void SetupCardData(CardData cardData)
    {
        CardData = cardData;
    }

    [Button]
    async public void Flip(int millisecondDelay = 0)
    {
        await UniTask.Delay(millisecondDelay);
        CardData.IsFlip = !CardData.IsFlip;
        SoundManager.Instance.PlaySoundFX(SoundKeys.FLIP);
        transform.DORotate(new Vector3(0, 0, CardData.IsFlip ? 0 : 180), 0.5f, RotateMode.Fast);
        if(CardData.IsFlip)
        {
            OnShowCard.OnNext(this);
        }
    }
    public void AnimationCorrectAndRemoveCard()
    {
        CardData.IsLock = true;
        transform.DOPunchScale(new Vector3(0.02f,0.02f,0.02f), 0.5f, 1).OnComplete(async () => {
            await UniTask.Delay(1000);
            Release();
        });
    }
    public void Release()
    {
        _objectPool.Release(this);
    }
}
