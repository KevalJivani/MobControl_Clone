using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanelUIScript : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;

    private GameManagerScript gmInstance;

    private void Start()
    {
        Hide();

        gmInstance = GameManagerScript.Instance;

        nextLevelButton.onClick.AddListener(() =>
        {
            Debug.Log("NextLevel Button Clicked");
            NextLevelFunc();

            Hide();

            /*var currScene = SceneManager.GetActiveScene().buildIndex;
            if (currScene <= SceneManager.sceneCount)
            {
                SceneManager.LoadScene(currScene + 1);
            }
            else
            {
                var loadScene = Random.Range(0, SceneManager.sceneCount - 1);
                SceneManager.LoadScene(loadScene);
            }*/
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

    private void NextLevelFunc()
    {
        gmInstance.levelNo++;
        gmInstance.SaveData();

        var obj = FindObjectOfType<LevelDataScript>().gameObject;
        Destroy(obj);

        gmInstance.InstantiateLevel();
    }
}
