using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class UIManagerForStartScene : MonoBehaviour
{
    public static UIManagerForStartScene Instance;

    Sequence _startSeq;
    Image _fadeIn;
    RectTransform _citadel;
    

    private void Awake()
    {
        Instance = this;
        _fadeIn = transform.Find("Image").GetComponent<Image>();
        _citadel = transform.Find("CITADEL").GetComponent<RectTransform>();
        
    }

    void Start()
    {
        if (_startSeq.IsActive() && _startSeq != null)
        {
            _startSeq.Kill();
        }
        _startSeq = DOTween.Sequence();
        _startSeq.Append(_citadel.DOAnchorPosX(-345f, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
        }));
    }

    private void Update()
    {
        if (Input.anyKey && _startSeq.IsActive())
        {
            _startSeq.Complete();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    

    IEnumerator FadeInCoroutine()
    {
        _fadeIn.gameObject.SetActive(true);
        while (_fadeIn.color.a <= 255)
        {
            Color color = _fadeIn.color;

            color.a += Time.deltaTime;

            _fadeIn.color = color;
            yield return null;
        }
    }
}
