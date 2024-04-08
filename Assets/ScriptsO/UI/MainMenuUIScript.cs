using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private GameManagerScript gmInstance;

    private void Start()
    {
        gmInstance = GameManagerScript.Instance;

        playButton.onClick.AddListener(() =>
        {
            //Debug.Log("MainMenu Button Clicked");
            gmInstance.InstantiateLevel();

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
