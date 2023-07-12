using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isStarting = false;
    public bool isAdding = true;
    public bool isGameStoping = false;
    private Transform playerTrm = null;
    public Transform PlayerTrm => playerTrm;
    [SerializeField]
    ScoreSO _scoreSO;


    private void Awake()
    {
        Instance = this;
        _scoreSO.score = 0;
        playerTrm = FindObjectOfType<PlayerController>().transform;
        CreateTimeController();
        if (SceneManager.GetActiveScene().name != "SampleScene") return;
        CreateUIManager();
        CreateCameraManager();
        CreateMapManager();
    }

    private void CreateUIManager()
    {
        GameObject obj = new GameObject("UIManager");
        UIManager.Instance = obj.AddComponent<UIManager>();
        Transform canvasTrm = FindAnyObjectByType<Canvas>().transform;
        UIManager.Instance.Init(canvasTrm);
    }

    private void CreateTimeController()
    {
        TimeController.Instance = gameObject.AddComponent<TimeController>();
    }

    public void StartGame()
    {
        isStarting = true;
    }

    public void MoveStart()
    {
        isAdding = true;
    }
    private void CreateMapManager()
    {
        MapManager.Instance = GameObject.Find("Grid").AddComponent<MapManager>();
        MapManager.Instance.Init();
    }
    private void CreateCameraManager()
    {
        CameraManager.Instance = GameObject.Find("CameraSet").AddComponent<CameraManager>();
        CameraManager.Instance.Init();
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void MenuExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
