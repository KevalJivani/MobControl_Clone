using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelUIScript : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        Hide();
        mainMenuButton.onClick.AddListener(() =>
        {
            Debug.Log("MainMenu Button Clicked");
            Hide();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
