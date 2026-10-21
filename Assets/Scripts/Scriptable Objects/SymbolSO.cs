using UnityEngine;


public enum MathOperation
{
    Add,
    Subtract,
    Multiply,
    Divide
}

[CreateAssetMenu(fileName = "SymbolSO", menuName = "Calculations/SymbolSO", order = 1)]
public class SymbolSO : ScriptableObject
{
  public string Name;

  public MathOperation operation;
  public GameObject symbolGO;
}
