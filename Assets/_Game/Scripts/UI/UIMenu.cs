using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] GameObject _gameRoot;
    [SerializeField] GameObject _createGamePanel;
    [SerializeField] GameObject _gameplayPanel;
    [SerializeField] Button LoadGameBtn;
    //[SerializeField]GameObject 

    private void Start()
    {
        GameManager.OnGameStart.AsObservable().Subscribe(_ => { _gameplayPanel.SetActive(true); }).AddTo(this);
        _gameRoot.ObserveEveryValueChanged(_ => _.activeSelf).Subscribe(isActive =>
        {
            if (isActive)
                LoadGameBtn.interactable = SaveManager.Instance.HasSaveGameData();
        }).AddTo(this);
    }
    public void NewGame()
    {
        _gameRoot.SetActive(false);
        _createGamePanel.SetActive(true);
    }
    public void LoadGame()
    {
        _gameRoot.SetActive(false);
        GameManager.Instance.Load();
    }
}
