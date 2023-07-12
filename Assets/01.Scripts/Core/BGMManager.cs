using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    AudioSource _currentBGM;
    AudioSource _bossBGM;

    bool boss = false;

    private void Awake()
    {
        Instance = this;
        _currentBGM = transform.Find("BackgroundMusic").GetComponent<AudioSource>();
        _bossBGM = transform.Find("BossBackgroundMusic").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(boss)
        {
            while (_currentBGM.volume > 0)
            {
                _currentBGM.volume -= Time.deltaTime;
            }
            _bossBGM.gameObject.SetActive(true);
        }
    }

    public void BossBGM()
    {
        boss = true;
    }


    public void BGMOff()
    {
        Destroy(_currentBGM.gameObject);
        Destroy(_bossBGM.gameObject);
    }
}
