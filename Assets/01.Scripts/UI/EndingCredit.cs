using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class EndingCredit : MonoBehaviour
{
    Sequence creditSeq;
    RectTransform _endingCredit;
    

    [SerializeField]
    ScoreSO _scoreSO;

    public float speed = 150f;

    private void Awake()
    {
        _endingCredit = transform.Find("EndingCredit").GetComponent<RectTransform>();
    }

    private void Start()
    {
        transform.Find("EndingCredit/Text1 (8)").GetComponent<TextMeshProUGUI>().text = string.Format("당신의 점수는...\n\n{0:D6}점", _scoreSO.score);
        transform.Find("EndingCredit/Text1 (9)").GetComponent<TextMeshProUGUI>().text = string.Format("당신의 최고 점수는...\n\n{0:D6}점", _scoreSO.bestScore);
        if(_scoreSO.score == _scoreSO.bestScore)
        {
            transform.Find("EndingCredit/Text1 (10)").gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        _endingCredit.anchoredPosition += Vector2.up * Time.deltaTime * speed;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpeedUp();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SpeedDown();
        }
        if(_endingCredit.anchoredPosition.y >= 4200)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SpeedUp()
    {
        if (creditSeq.IsActive() && creditSeq != null)
        {
            creditSeq.Kill();
        }
        Tween faster = DOTween.To(
           getter: () => speed,
           setter: value => speed = value,
           endValue: 500f, duration: 0.5f
           ).SetEase(Ease.InSine);
        creditSeq.Append(faster);
    }

    private void SpeedDown()
    {
        if (creditSeq.IsActive() && creditSeq != null)
        {
            creditSeq.Kill();
        }
        Tween slower = DOTween.To(
           getter: () => speed,
           setter: value => speed = value,
           endValue: 150f, duration: 0.5f
           ).SetEase(Ease.InSine);
        creditSeq.Append(slower);
    }
}
