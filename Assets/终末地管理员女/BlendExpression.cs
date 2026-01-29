using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BlendExpression : MonoBehaviour
{
    private Animator _blendTree;
    private int _expressionIndex;

    [SerializeField, Range(0f, 1f)]
    public float Blending = 0f;

    [SerializeField, Range(0f, 1f)]
    public float ExpressionWeight = 1f;

    [SerializeField]
    private float resetDelay = 50f;

    private Coroutine resetCoroutine;

    void Start()
    {
        _blendTree = GetComponent<Animator>();

        _expressionIndex = _blendTree.GetLayerIndex("Expression");
    }

    void Update()
    {
        //Fail getting animator.
        if (_blendTree == null)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            float blend = GetRandomFloatTwoDecimals();
            _blendTree.SetFloat("Blend", blend);

            Debug.Log($"Mouse Click ¡ú Blend = {blend}");

            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }

            resetCoroutine = StartCoroutine(ResetBlendAfterDelay());
        }

        //_blendTree.SetFloat("Blend", Blending);



        if (_expressionIndex != -1)
            _blendTree.SetLayerWeight(_expressionIndex, ExpressionWeight);

    }

    IEnumerator ResetBlendAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        _blendTree.SetFloat("Blend", 0.25f);
        Debug.Log("Blend reset to 0.25");

        resetCoroutine = null;
    }

    private float GetRandomFloatTwoDecimals()
    {
        float value = Random.Range(0, 1000);
        return value / 1000f;
    }
}
