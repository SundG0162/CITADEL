using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    [SerializeField]
    GameObject _bossPrefab;
    [SerializeField]
    GameObject _cameraShake;

    private void Awake()
    {
        Instance = this;
    }

    public void BossSpawn()
    {
        StartCoroutine(BossSpawnCoroutine());
    }

    IEnumerator BossSpawnCoroutine()
    {
        GameManager.Instance.isAdding = false;
        GameManager.Instance.PlayerTrm.GetComponent<PlayerController>().boxCollider.enabled = false;
        CameraManager.Instance.CenterCam();
        yield return new WaitForSeconds(1f);
        BGMManager.Instance.BossBGM();
        yield return new WaitForSeconds(2f);
        GameObject obj = Instantiate(_cameraShake);
        Destroy(obj, 3f);
        Instantiate(_bossPrefab, transform.position, Quaternion.identity);
        UIManager.Instance.BossUIDisplay();
        MapManager.Instance.WallDisable();
        GameManager.Instance.PlayerTrm.GetComponent<PlayerController>().boxCollider.enabled = true;
        GameManager.Instance.isAdding = true;
    }
}
