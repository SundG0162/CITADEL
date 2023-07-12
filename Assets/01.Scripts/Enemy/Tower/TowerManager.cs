using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    public int towerCount = 4;
    public bool blueTowerActive = false;
    public float delayTime = 9;

    public bool redTower = false;
    public bool blueTower = false;
    public bool greenTower = false;
    public bool blackTower = false;
    public float DelayTime
    {
        get => delayTime;
        set
        {
            if(value >= 6)
            {
                delayTime = 6;
            }
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(TowerAttackCoroutine());    
    }

    void Update()
    {
        towerCount = transform.childCount - 2;
        if(UIManager.Instance.score % 5000 == 0 && UIManager.Instance.score != 0)
        {
            DelayTime--;
        }
    }

    IEnumerator TowerAttackCoroutine()
    {
        yield return new WaitForSeconds(15f);
        while (transform.childCount != 1)
        {
            int count = Random.Range(0, towerCount);
            Debug.Log(count);
            Transform tower = transform.GetChild(count);
            if (tower.name == "RedTower")
            {
                tower.GetComponent<TowerRed>().TowerAttack();
            }
            else if (tower.name == "BlueTower")
            {
                GameManager.Instance.PlayerTrm.GetChild(0).GetComponent<PlayerOrbCounter>().RotationSpeedDown();
                tower.GetComponent<TowerBlue>().TowerAttack();
            }
            else if (tower.name == "BlackTower")
            {
                tower.GetComponent<TowerBlack>().TowerAttack();
            }
            else if (tower.name == "GreenTower")
            {
                tower.GetComponent<TowerGreen>().TowerAttack();
            }
            yield return new WaitForSeconds(DelayTime);
        }
    }
}
