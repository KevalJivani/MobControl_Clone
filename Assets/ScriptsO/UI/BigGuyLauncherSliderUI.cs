using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigGuyLauncherSliderUI : MonoBehaviour
{
    //public event Action OnBigGuySliderFull;

    private Slider slider;

    [SerializeField] private Image fillImage;

    [Space(10)]
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Sprite normalSprite;

    [HideInInspector] public int fillAmount;
    [HideInInspector] public bool isBarFilled;


    public int SliderMaxValue;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = SliderMaxValue;
        fillImage.sprite = normalSprite;
    }

    private void Update()
    {
        UpdateUIValue();
    }

    public void UpdateUIValue()
    {
        if (fillAmount == slider.maxValue)
        {
            fillImage.sprite = completedSprite;
            isBarFilled = true;
        }
        else
        {
            fillImage.sprite = normalSprite;
        }

        slider.value = fillAmount;
    }
}
