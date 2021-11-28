using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MathQuizMode
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

public interface IMathQuiz
{
    MathQuizMode QuizMode { get; }
    int TermA { get; }
    int TermB { get; }
    int Answer { get; }
    int[] Options { get; }
}

/// <summary>
/// reference: 
/// https://docs.microsoft.com/en-us/visualstudio/ide/tutorial-2-create-a-timed-math-quiz
/// </summary>
public class MathQuizManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent FeedBack;
    [SerializeField] private Text TermAText;
    [SerializeField] private Text SymbolText;
    [SerializeField] private Text TermBText;
    [SerializeField] private MathQuizButton[] bonusBtns;
    IMathQuiz curQuiz;

    private void Awake()
    {
        for (int i = 0; i < bonusBtns.Length; i++)
        {
            bonusBtns[i].SetFeedBack((i) => UserAnswer(i));
        }
    }

    private void Start()
    {
        NextQuestion();
    }

    public void NextQuestion()
    {
        curQuiz = GetRandomQuiz();
        int termA = curQuiz.TermA;
        int termB = curQuiz.TermB;
        int ans = curQuiz.Answer;
        string sym = GetMathSymbol(curQuiz.QuizMode);
        TermAText.text = curQuiz.TermA.ToString();
        TermBText.text = curQuiz.TermB.ToString();
        SymbolText.text = sym;
        Debug.Log($"{termA}{sym}{termB}={ans}");
        for (int i = 0; i < curQuiz.Options.Length; i++)
            bonusBtns[i].SetValue(curQuiz.Options[i]);
    }

    private void UserAnswer(int num)
    {
        bool isSuc = curQuiz.Answer == num;
        Debug.Log("isSuc " + isSuc);
        FeedBack?.Invoke();
    }

    private string GetMathSymbol(MathQuizMode mode)
    {
        switch (mode)
        {
            default:
            case MathQuizMode.Addition:
                return "+";
            case MathQuizMode.Subtraction:
                return "-";
            case MathQuizMode.Multiplication:
                return "x";
            case MathQuizMode.Division:
                return "/";
        }
    }

    private IMathQuiz GetRandomQuiz()
    {
        MathQuizMode mode = (MathQuizMode)Random.Range(0, 4);
        MathQuiz result = GetMathQuiz(mode);
        result.SetMode(mode);
        return result;
    }

    private static MathQuiz GetMathQuiz(MathQuizMode mode)
    {
        switch (mode)
        {
            default:
            case MathQuizMode.Addition:
                return new Addition();
            case MathQuizMode.Subtraction:
                return new Subtraction();
            case MathQuizMode.Multiplication:
                return new Multiplication();
            case MathQuizMode.Division:
                return new Division();
        }
    }

    #region Math Quiz Item
    private abstract class MathQuiz : IMathQuiz
    {
        [SerializeField] private MathQuizMode quizMode;
        internal int termA = 0;
        internal int termB = 0;
        internal int answer = 0;
        internal int[] options;
        private int optionsCount = 4;

        #region IMathQuiz Item
        public MathQuizMode QuizMode => quizMode;

        public int TermA => termA;

        public int TermB => termB;

        public int Answer => answer;

        public int[] Options => options;
        #endregion

        public MathQuiz()
        {
            MathsProblem();
        }

        public abstract void MathsProblem();

        internal void SetMode(MathQuizMode mode)
        {
            quizMode = mode;
        }

        internal int[] GetRandomOptions(int rdmin, int rdmax)
        {
            List<int> result = new List<int>();
            int locationOfAnswer = Random.Range(0, optionsCount);
            for (int i = 0; i < optionsCount; i++)
            {
                if (i == locationOfAnswer)
                {
                    result.Add(answer);
                    continue;
                }

                while (true)
                {
                    int val = Random.Range(rdmin, rdmax);
                    if (val != answer && !result.Contains(val))
                    {
                        result.Add(val);
                        break;
                    }
                }
            }
            return result.ToArray();
        }
    }

    private class Addition : MathQuiz
    {
        public override void MathsProblem()
        {
            termA = Random.Range(1, 51);
            termB = Random.Range(1, 51);
            answer = termA + termB;
            options = GetRandomOptions(2, 101);
        }
    }

    private class Subtraction : MathQuiz
    {
        public override void MathsProblem()
        {
            termA = Random.Range(1, 101);
            termB = Random.Range(1, termA);
            answer = termA - termB;
            options = GetRandomOptions(1, 101);
        }
    }

    private class Multiplication : MathQuiz
    {
        public override void MathsProblem()
        {
            termA = Random.Range(2, 11);
            termB = Random.Range(1, 11);
            answer = termA * termB;
            options = GetRandomOptions(2, 101);
        }
    }

    private class Division : MathQuiz
    {
        public override void MathsProblem()
        {
            int tempQuotient = Random.Range(2, 11);
            termB = Random.Range(2, 11);
            termA = termB * tempQuotient;
            answer = termA / termB;
            options = GetRandomOptions(2, 11);
        }
    }
    #endregion
}
