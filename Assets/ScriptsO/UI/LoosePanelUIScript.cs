using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoosePanelUIScript : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    private void Start()
    {
        Hide();
        restartButton.onClick.AddListener(() =>
        {
            Debug.Log("Restart Button Clicked");
            Hide();
            var currScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currScene);
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
