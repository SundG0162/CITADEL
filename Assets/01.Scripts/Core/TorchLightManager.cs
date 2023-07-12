using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class TorchLightManager : MonoBehaviour
{
    public static TorchLightManager Instance;
    [SerializeField]
    List<Light2D> _torchlights = new List<Light2D>();
    [SerializeField]
    List<Light2D> _playerLight = new List<Light2D>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            Light2D light = transform.GetChild(i).GetChild(0).GetComponent<Light2D>();
            _torchlights.Add(light);
        }
    }

    private void Start()
    {
        _playerLight = GameManager.Instance.PlayerTrm.GetComponentsInChildren<Light2D>().ToList();
    }

    public void BlackTowerAttack()
    {
        float rand = Random.Range(4f, 6f);
        StartCoroutine(BlackTowerAttackCoroutine(rand));
    }

    public void LightDisble()
    {
        _torchlights.ForEach(t => t.enabled = false);
        _playerLight.ForEach(light => light.enabled = false);
    }

    IEnumerator BlackTowerAttackCoroutine(float delayTime)
    {
        _torchlights.ForEach(t => t.enabled = false);
        _playerLight.ForEach(light => light.intensity -= 1f);
        yield return new WaitForSeconds(delayTime);
        _torchlights.ForEach(t => t.enabled = true);
        _playerLight.ForEach(light => light.intensity += 1f);

    }
}
