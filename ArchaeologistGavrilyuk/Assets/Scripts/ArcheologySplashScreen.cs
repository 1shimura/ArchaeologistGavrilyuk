using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcheologySplashScreen : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private ArcheologyGameScreen _gameScreen;

    private void Awake()
    {
        _gameScreen.gameObject.SetActive(false);
        _playButton.onClick.AddListener(ShowGameScreen);
    }

    private void ShowGameScreen()
    {
        _gameScreen.gameObject.SetActive(true);
        _gameScreen.StartNewGame();
        
        gameObject.SetActive(false);
    }
}
