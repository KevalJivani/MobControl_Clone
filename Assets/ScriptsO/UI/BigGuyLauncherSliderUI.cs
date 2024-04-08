using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BigGuyLauncherSliderUI : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private Image fillImage;

    [Space(10)]
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Sprite normalSprite;

    [HideInInspector] public float fillAmount;
    [HideInInspector] public bool isBarFilled;

    public float SliderMaxValue;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = SliderMaxValue;
        fillImage.sprite = normalSprite;
        slider.value = fillAmount;
    }

    public void UpdateUIValue()
    {
        if (fillAmount == slider.maxValue && isBarFilled != true)
        {
            DOTween.To(() => slider.value, x => slider.value = x, fillAmount, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                fillImage.sprite = completedSprite;
                isBarFilled = true;
            });
        }
        else if (fillAmount < slider.maxValue)
        {
            DOTween.To(() => slider.value, x => slider.value = x, fillAmount, 0.2f).SetEase(Ease.Linear);

            if (isBarFilled != true)
            {
                fillImage.sprite = normalSprite;
            }
        }
    }
}
