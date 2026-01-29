using UnityEngine;
using UnityEngine.InputSystem;
using Live2D.Cubism.Framework.Expression;

public class ExpressionClickDrive : MonoBehaviour
{
    public CubismExpressionController expressionController;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var count = expressionController.ExpressionsList.CubismExpressionObjects.Length;
            int i = Random.Range(0, count);
            expressionController.CurrentExpressionIndex = i;
            Debug.Log($"Mouse Click ¡ú ExIndex = {i}");

        }
    }
}
