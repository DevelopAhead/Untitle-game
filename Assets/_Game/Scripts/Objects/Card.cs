using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using TMPro;
public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshPro _textMesh;
    bool _isFlip = false;
    public CardData CardData { get; private set; }
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
    public void Flip()
    {
        _isFlip = !_isFlip;
        transform.DORotate(new Vector3(0, 0, _isFlip ? 0 : 180), 0.5f, RotateMode.Fast);
    }
}
