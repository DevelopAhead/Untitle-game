using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameRoot;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] TextMeshProUGUI _turnTxt;
    [SerializeField] TextMeshProUGUI _scoreTxt;
    
    private void Start()
    {
        GameManager.OnGameOver.AsObservable().Subscribe(gameData =>
        {
            _gameRoot.SetActive(true);
            _turnTxt.text = gameData.Turn.ToString();
            _scoreTxt.text = gameData.Score.ToString();

        }).AddTo(this);
    }
    public void ToMenu()
    {
        _gameRoot.SetActive(false);
        _menuPanel.SetActive(true);
    }
}
