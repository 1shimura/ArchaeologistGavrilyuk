using System;
using UnityEngine;
using UnityEngine.UI;

public class FieldCell : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    
    private int _maxDepth;
    private int _depth;
    private Color _groundColor;
    private Color _depthColor;

    public event Action<FieldCell> Clicked;

    public void Init(int depth, Color groundColor, Color depthColor)
    {
        _maxDepth = _depth = depth;
        _groundColor = groundColor;
        _depthColor = depthColor;

        _button.onClick.AddListener(OnClick);
        
        UpdateCell();
    }

    private void OnClick()
    {
        Clicked?.Invoke(this);
    }

    public void Dig()
    {
        _depth--;
        UpdateCell();
    }

    private void UpdateCell()
    {
        _image.color = Color.Lerp(_depthColor, _groundColor, _depth / (float) _maxDepth);
        _button.interactable = _depth > 0;
    }
}
