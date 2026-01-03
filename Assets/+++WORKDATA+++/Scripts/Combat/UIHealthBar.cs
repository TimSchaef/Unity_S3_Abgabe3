// UIHealthBarFill.cs
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarFill : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (fillImage == null)
        {
            Debug.LogError("UIHealthBarFill: Fill Image is not assigned!");
        }
    }

    public void SetHP(int currentHP, int maxHP)
    {
        if (fillImage == null || maxHP <= 0) return;

        float normalized = Mathf.Clamp01((float)currentHP / maxHP);
        fillImage.fillAmount = normalized;

        // Optional: Farbe je nach HP
        fillImage.color = Color.Lerp(Color.red, Color.green, normalized);
    }
}


