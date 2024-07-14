using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] GameObject _gameRoot;
    [SerializeField] TextMeshProUGUI _scoreTxt;
    [SerializeField] TextMeshProUGUI _comboTxt;
    [SerializeField] TextMeshProUGUI _turnTxt;
    private void Start()
    {
        AddListener();
    }
    void AddListener()
    {
        GameManager.OnGameDataCreated.Subscribe(gameData =>
        {
            _scoreTxt.text = gameData.Score.ToString();
            _comboTxt.text = gameData.Combo.ToString();


            GameManager.Instance.ObserveEveryValueChanged(game => game.GameData.Score).Subscribe(score =>
            {
                _scoreTxt.text = score.ToString();
            }).AddTo(this);
            GameManager.Instance.ObserveEveryValueChanged(game => game.GameData.Combo).Subscribe(combo =>
            {
                _comboTxt.text = combo.ToString();
            }).AddTo(this);
            GameManager.Instance.ObserveEveryValueChanged(game => game.GameData.Turn).Subscribe(turn =>
            {
                _turnTxt.text = turn.ToString();
            }).AddTo(this);

        }).AddTo(this);
        GameManager.OnGameOver.Subscribe(gameData =>
        {
            _gameRoot.SetActive(false);
        }).AddTo(this);
    }
    public void Restart()
    {
        GameManager.Instance.ResetGame();
    }
}
