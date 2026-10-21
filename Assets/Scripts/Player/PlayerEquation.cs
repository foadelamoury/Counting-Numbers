using UnityEngine;
using TMPro;
using System;

public enum NumberState
{
    biggerThanTarget,
    smallerThanTarget,
    negative

}
public class PlayerEquation : MonoBehaviour
{
   
    public static PlayerEquation Instance { get; private set; }

    [Header("UI Elements")]
    public TMP_Text equationText;
    public TMP_Text targetAnswerText;
    public TMP_Text currentResultText;

    [Header("Game State")]
    public int targetAnswer;
    public int minRandomAnswer = 1;
    public int maxRandomAnswer = 50;

    private string leftOperand = "";
    private string rightOperand = "";
    private MathOperation? pendingOp = null;

    public Action<NumberState> OncurrentResultUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }
    }

    private void Start() => GenerateNewTarget();

    private void GenerateNewTarget()
    {
        targetAnswer = UnityEngine.Random.Range(minRandomAnswer, maxRandomAnswer + 1);
        leftOperand = "";
        rightOperand = "";
        pendingOp = null;
        UpdateUI();
    }

    public void CollectNumber(NumbersSO numberSO)
    {
        string numStr = numberSO.Number.ToString();

        if (pendingOp == null)
        {
            leftOperand += numStr;
        }
        else
        {
            if (pendingOp == MathOperation.Divide && rightOperand == "" && numStr == "0")
            {
                Debug.Log("Ignored a 0 during division.");
                return;
            }
            rightOperand += numStr;
        }

        UpdateUI();
        CheckWinCondition();
    }

    public void CollectSymbol(SymbolSO symbolSO)
    {
        if (leftOperand == "") return;

        // If we already have a full equation, evaluate it first
        if (rightOperand != "" && pendingOp.HasValue)
        {
            leftOperand = Evaluate(leftOperand, rightOperand, pendingOp.Value).ToString();
            rightOperand = "";
        }

        pendingOp = symbolSO.operation;
        UpdateUI();
        CheckWinCondition();
    }

    private int Evaluate(string leftStr, string rightStr, MathOperation op)
    {
        int left = int.Parse(leftStr);
        int right = int.Parse(rightStr);

        switch (op)
        {
            case MathOperation.Add:      return left + right;
            case MathOperation.Subtract: return left - right;
            case MathOperation.Multiply: return left * right;
            case MathOperation.Divide:   return right != 0 ? left / right : 0;
            default: return right;
        }
    }

    private string OpToString(MathOperation op)
    {
        switch (op)
        {
            case MathOperation.Add:      return "+";
            case MathOperation.Subtract: return "-";
            case MathOperation.Multiply: return "*";
            case MathOperation.Divide:   return "/";
            default: return "";
        }
    }

    private int GetLiveResult()
    {
        if (leftOperand == "") return 0;
        if (rightOperand != "" && pendingOp.HasValue)
            return Evaluate(leftOperand, rightOperand, pendingOp.Value);
        return int.Parse(leftOperand);
    }

    private void UpdateUI()
    {
        if (targetAnswerText != null)
            targetAnswerText.text = "Target: " + targetAnswer;

        string eq = leftOperand;
        if (pendingOp.HasValue) eq += " " + OpToString(pendingOp.Value);
        if (rightOperand != "") eq += " " + rightOperand;

        if (equationText != null)
            equationText.text = "Equation:\n" + eq;

        if (currentResultText != null)
            currentResultText.text = "Result: " + GetLiveResult();
    }

    private void CheckWinCondition()
    {
        int liveResult = GetLiveResult();

        if (liveResult == targetAnswer)
        {
            Debug.Log("Target Reached! Generating new target.");
            GenerateNewTarget();
        }
        else
        {
            NumberState state;
            if (liveResult < 0)
                state = NumberState.negative;
            else if (liveResult > targetAnswer)
                state = NumberState.biggerThanTarget;
            else
                state = NumberState.smallerThanTarget;

            OncurrentResultUpdated?.Invoke(state);
        }
    }
}
