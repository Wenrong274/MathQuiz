using System;
using UnityEngine;
using UnityEngine.UI;

public class MathQuizButton : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private Text valueText;
    [SerializeField] private Button button;
    [SerializeField] private Action<int> feedBack;

    private void Awake()
    {
        button.onClick.AddListener(() => OnClick());
    }

    public void SetValue(int val)
    {
        value = val;
        valueText.text = val.ToString();
    }

    public void SetFeedBack(Action<int> act)
    {
        feedBack = act;
    }

    private void OnClick()
    {
        feedBack?.Invoke(value);
    }
}
