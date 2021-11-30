using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MathQuizUI : MonoBehaviour
{
    [SerializeField] private MathQuizManager MathQuizManager;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text text;
    Sequence seq;

    public void OnQuizFeedback(bool on)
    {
        StopTween();
        text.text = on ? "Success" : "Fail";
        seq.Append(canvasGroup.DOFade(1, .3f));
        seq.Append(canvasGroup.DOFade(0, .3f).SetDelay(1.5f).OnComplete(() => { MathQuizManager.NextQuestion(); }));
    }

    public void HideGameResult()
    {
        StopTween();
        seq.Append(canvasGroup.DOFade(0, .3f));
    }

    private void StopTween()
    {
        if (seq != null)
            if (seq.IsPlaying())
                seq.Pause();
        seq = DOTween.Sequence();
    }
}
