using UnityEngine;
using UnityEngine.UI;

public class PowerBarUI : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.value = 0f;
    }

    public void SetPower(float normalizedPower)
    {
        slider.value = Mathf.Clamp01(normalizedPower);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}