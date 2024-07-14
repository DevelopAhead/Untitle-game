using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    bool _isFlip = false;
    void Start()
    {
        
    }

    [Button]
    public void Flip()
    {
        _isFlip = !_isFlip;
        transform.DORotate(new Vector3(0, 0, _isFlip ? 0 : 180), 0.5f, RotateMode.Fast);
    }
}
