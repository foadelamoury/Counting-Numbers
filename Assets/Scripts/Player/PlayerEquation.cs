using UnityEngine;
using TMPro;
using System;

public class PlayerEquation : MonoBehaviour
{
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
    private string pendingOp = "";

    public Action<bool> OncurrentResultUpdated; // Event to notify when the current result is more or less than the target answer

    private void Start()
    {
        GenerateNewTarget();
    }

    private void GenerateNewTarget()
    {
        targetAnswer = UnityEngine.Random.Range(minRandomAnswer, maxRandomAnswer + 1);
        ClearEquation();
        UpdateUI();
    }

    private void ClearEquation()
    {
        leftOperand = "";
        rightOperand = "";
        pendingOp = "";
    }

    public void CollectNumber(NumbersSO numberSO)
    {
        string numStr = numberSO.Number.ToString();

        if (string.IsNullOrEmpty(pendingOp))
        {
            leftOperand += numStr;
        }
        else
        {
            // Prevent divide by zero if rightOperand is empty and we collect '0'
            if (pendingOp == "/" && string.IsNullOrEmpty(rightOperand) && numStr == "0")
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
        string op = NormalizeOperator(symbolSO.operation);

        // If we haven't collected any numbers yet, ignore the symbol
        if (string.IsNullOrEmpty(leftOperand))
            return;

        // If we already have a full equation (left OP right), evaluate it first!
        if (!string.IsNullOrEmpty(rightOperand))
        {
            int result = EvaluateTwoNumbers(leftOperand, rightOperand, pendingOp);
            leftOperand = result.ToString();
            rightOperand = "";
        }

        // Set the new pending operation
        pendingOp = op;

        UpdateUI();
        CheckWinCondition();
    }

    private string NormalizeOperator(string op)
    {
        op = op.ToLower().Trim();
        if (op == "+" || op.Contains("add")) return "+";
        if (op == "-" || op.Contains("sub")) return "-";
        if (op == "*" || op.Contains("mul") || op == "x") return "*";
        if (op == "/" || op.Contains("div")) return "/";
        return op; 
    }

    private int EvaluateTwoNumbers(string leftStr, string rightStr, string operation)
    {
        int left = int.Parse(leftStr);
        int right = int.Parse(rightStr);

        switch (operation)
        {
            case "+": return left + right;
            case "-": return left - right;
            case "*": return left * right;
            case "/": return right != 0 ? left / right : 0;
            default: return right;
        }
    }

    private void UpdateUI()
    {
        if (targetAnswerText != null)
            targetAnswerText.text = "Target: " + targetAnswer;

        string eq = leftOperand;
        if (!string.IsNullOrEmpty(pendingOp)) eq += " " + pendingOp;
        if (!string.IsNullOrEmpty(rightOperand)) eq += " " + rightOperand;

        if (equationText != null)
            equationText.text = "Equation:\n" + eq;

        // Calculate live result for display
        int liveResult = 0;
        if (!string.IsNullOrEmpty(leftOperand))
        {
            if (!string.IsNullOrEmpty(rightOperand) && !string.IsNullOrEmpty(pendingOp))
            {
                liveResult = EvaluateTwoNumbers(leftOperand, rightOperand, pendingOp);
            }
            else
            {
                liveResult = int.Parse(leftOperand);
            }
        }

        if (currentResultText != null)
            currentResultText.text = "Result: " + liveResult;
    }

    private void CheckWinCondition()
    {
        // Calculate live result
        int liveResult = 0;
        if (!string.IsNullOrEmpty(leftOperand))
        {
            if (!string.IsNullOrEmpty(rightOperand) && !string.IsNullOrEmpty(pendingOp))
            {
                liveResult = EvaluateTwoNumbers(leftOperand, rightOperand, pendingOp);
            }
            else
            {
                liveResult = int.Parse(leftOperand);
            }
        }

        if (liveResult == targetAnswer)
        {
            Debug.Log("Target Reached! Generating new target.");
            GenerateNewTarget(); // This also clears the equation!
        }
        else if (liveResult > targetAnswer)
        {
            OncurrentResultUpdated?.Invoke(true); // Notify that current result is greater
        }
        else
        {
            OncurrentResultUpdated?.Invoke(false); // Notify that current result is less
        }
    }
}
