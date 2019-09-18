using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArcheologyGameScreen : MonoBehaviour
{
    [SerializeField] private GameObject _menuScreen;
     
     [SerializeField] private Transform _fieldsGroup;
     [SerializeField] private FieldCell _fieldItem;
     [SerializeField] private TextMeshProUGUI _shovelCountText;
     [SerializeField] private TextMeshProUGUI _scoreText;
     [SerializeField] private GameObject _backpack;

     [SerializeField] private int _shovelCount = 10;
     [SerializeField] private int _fieldDepth = 5;
     [SerializeField] private int _fieldWidth = 10;
     [SerializeField] private int _fieldHeight = 10;
     [SerializeField] private float _treasureSpawnRate = .3f;

     [SerializeField] private Color _groundColor;
     [SerializeField] private Color _depthColor;

     [SerializeField] private RectTransform _canvasTransform;
     [SerializeField] private RectTransform _treasureGroupTransform;
     [SerializeField] private GraphicRaycaster _graphicRaycaster;
     
     [SerializeField] private TreasureCell _treasureCell;

     [SerializeField] private Sprite[] _treasureSprites;

     [SerializeField] private Button _menuButton;

     [SerializeField] private GameObject _losePopup;
     
     private int _currentShovelCount;
     private int _currentScore;

     private void SetCurrentShovelCount(int count)
     {
         _currentShovelCount = count;
         _shovelCountText.text = _currentShovelCount.ToString();
         _losePopup.SetActive(_currentShovelCount == 0);
     }

     private void SetCurrentScore(int score)
     {
         _currentScore = score;
         _scoreText.text = _currentScore.ToString();
     }

     private void Awake()
     {
         _menuButton.onClick.AddListener(MenuButtonClick);
     }

     private void MenuButtonClick()
     {
         _menuScreen.SetActive(true);
         gameObject.SetActive(false);
     }

     private void CellOnClick(FieldCell cell)
     {
         if (_currentShovelCount > 0)
         {
             SetCurrentShovelCount(_currentShovelCount - 1); 
             cell.Dig();
             if (Random.Range(0f, 1f) < _treasureSpawnRate)
             {
                 var treasure = Instantiate(_treasureCell, cell.transform);
                 treasure.Init(_canvasTransform, _treasureSprites[Random.Range(0, _treasureSprites.Length)], _treasureGroupTransform);
                 treasure.GetComponent<RectTransform>().SetParent(_treasureGroupTransform);
             
                 treasure.DragEnd += TreasureOnDragEnd;
             }
         }
     }

     private void TreasureOnDragEnd(PointerEventData pointerEventData, TreasureCell treasure)
     {
         var results = new List<RaycastResult>();
         _graphicRaycaster.Raycast(pointerEventData, results);
         
         foreach (var result in results)
         {
             if (result.gameObject == _backpack)
             {
                 Destroy(treasure.gameObject);
                 SetCurrentScore(_currentScore + 1);
                 return;
             }
         }
         
         treasure.ResetPosition();
     }

     public void StartNewGame()
     {
         foreach (Transform child in _fieldsGroup)
         {
             Destroy(child.gameObject);
         }

         foreach (Transform child in _treasureGroupTransform)
         {
             Destroy(child.gameObject);
         }
         
         for (var i = 0; i < _fieldWidth; ++i) 
         {
             for (var j = 0; j < _fieldHeight; ++j)
             {
                 var cell = Instantiate(_fieldItem, _fieldsGroup);
                 cell.Init(_fieldDepth, _groundColor, _depthColor);
                 cell.Clicked += CellOnClick;
             }
         }
         
         SetCurrentScore(0);
         SetCurrentShovelCount(_shovelCount);
     }
 }