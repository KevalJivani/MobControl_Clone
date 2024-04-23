using TMPro;
using UnityEngine;

public class CoinUIPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI blockText;

    private void Start()
    {
        Hide();
        coinText.text = UIManager.CoinCount.ToString(); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.gameManagerEvents.OnLevelCompleted.Get().AddListener(Event_OnLevelCompleted);
    }

    private void OnDisable()
    {
        EventManager.gameManagerEvents.OnLevelCompleted.Get().RemoveListener(Event_OnLevelCompleted);
    }

    private void Event_OnLevelCompleted()
    {
        //Update Coin value
        coinText.text = UIManager.CoinCount.ToString();
    }

    private void Update()
    {
        //Update Blocks value
        blockText.text = UIManager.BlockCount.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
