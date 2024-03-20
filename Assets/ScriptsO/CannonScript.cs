using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonScript : MonoBehaviour
{
    private const string SMALL_RUNNER = "NormalRunner";
    private const string BIG_RUNNER = "BigGuyRunner";
    private GameManagerScript gmInstance;
    private Vector3 _offset;

    private float lastSpawnTime = 0f;
    [SerializeField] private float minionSpawnTime = 0.5f;
    [SerializeField] private Animator cannonTopAnimator;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private CannonMovementScript cannonMovementScript;

    [Space(10)]
    [SerializeField] private BigGuyLauncherSliderUI bigGuyLauncherSliderUI;

    private void OnEnable()
    {
        CastleScript.OnCastleDestroyed += CastleManager_OnCastleDestroyed;
    }

    private void OnDisable()
    {
        CastleScript.OnCastleDestroyed -= CastleManager_OnCastleDestroyed;
    }

    private void CastleManager_OnCastleDestroyed(GameObject obj)
    {
        transform.DOLocalMove(new Vector3(0f, transform.position.y, 0f), 1f).SetEase(Ease.InOutCirc);
    }

    private void Start()
    {
        gmInstance = GameManagerScript.Instance;
    }

    private void OnMouseDown()
    {
        var pos = transform.localPosition;
        var position = new Vector3(pos.x, pos.y, pos.z);
        _offset = position - MouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!cannonMovementScript.isMoving)
        {
            var pos = MouseWorldPosition() + _offset;
            Vector3 position;

            var xPosition = Mathf.Clamp(pos.x, minX, maxX);
            position = new Vector3(xPosition, 0f, 0f);

            //Spawn Minions
            SpawnMinions(SMALL_RUNNER);

            transform.localPosition = position;
        }
    }

    private void OnMouseUp()
    {
        if (bigGuyLauncherSliderUI.isBarFilled)
        {
            SpawnSpeacialMinion(BIG_RUNNER);
            bigGuyLauncherSliderUI.fillAmount = 0;
            bigGuyLauncherSliderUI.isBarFilled = false;
        }
    }

    private Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main!.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private void SpawnMinions(string minionName)
    {
        if (Time.time - lastSpawnTime >= minionSpawnTime)
        {
            cannonTopAnimator.SetTrigger("CannonShoot");

            var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
            if (gmInstance.GamePartsList.Count > 0)
            {
                spawnedGameObj.transform.SetParent(gmInstance.GamePartsList[0].transform);
            }
            if (!bigGuyLauncherSliderUI.isBarFilled) bigGuyLauncherSliderUI.fillAmount++;
            //Debug.Log("Spawned");
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnSpeacialMinion(string minionName)
    {
        cannonTopAnimator.SetTrigger("CannonShoot");

        var spawnedGameObj = ObjectPooler.Instance.SpawnObjectFromPool(minionName, spawnPoint.position, spawnPoint.rotation);
        if (gmInstance.GamePartsList.Count > 0)
        {
            spawnedGameObj.transform.SetParent(gmInstance.GamePartsList[0].transform);
        }
    }
}
