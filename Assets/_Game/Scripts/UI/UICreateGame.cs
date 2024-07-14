using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.Windows;
using UnityEngine.UI;
public class UICreateGame : MonoBehaviour
{
    [SerializeField] GameObject _gameRoot;
    [SerializeField] TMP_InputField _widthInput, _heightInput;
    [SerializeField] Button _playBtn;
    private void Start()
    {
        _widthInput.onValueChanged.AddListener(CheckNumber);
        _heightInput.onValueChanged.AddListener(CheckNumber);
    }
    public void Play()
    {
        _gameRoot.SetActive(false);
        if (int.TryParse(_widthInput.text, out int widthNumber))
        {
            if (int.TryParse(_heightInput.text, out int heightNumber))
            {
                GameManager.Instance.CreateGame(widthNumber, heightNumber);
            }
        }
    }
    void CheckNumber(string input)
    {
        if (int.TryParse(_widthInput.text, out int widthNumber))
        {
            if (int.TryParse(_heightInput.text, out int heightNumber))
            {
                _playBtn.interactable = (widthNumber * heightNumber) % 2 == 0;
            }
        }
    }
}
