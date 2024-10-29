using UnityEngine;

[ExecuteInEditMode]
public class FillAmountController : MonoBehaviour
{
    public Material fillMaterial;  // 쉐이더가 적용된 머티리얼
    [Range(0, 0.5f)]
    public float fillAmount = 0.0f;  // Fill Amount 값 (0~1)

    void OnValidate()
    {
        UpdateFillAmount();
    }

    // Material의 Fill Amount 속성을 업데이트하는 메서드
    private void UpdateFillAmount()
    {
        if (fillMaterial != null)
        {
            fillMaterial.SetFloat("_FillAmount", fillAmount);
        }
    }
}
