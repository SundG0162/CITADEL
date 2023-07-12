using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ExitText : MonoBehaviour
{

    Sequence _scaleSeq;
    Sequence _startSeq;

    AudioSource _audio;

    public bool soundSettingOn = false;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_startSeq.IsActive() && _startSeq != null)
        {
            _startSeq.Kill();
        }
        _startSeq = DOTween.Sequence();
        _startSeq.Append(transform.DOMoveY(-3.82f, 1.5f).SetEase(Ease.OutBack));
    }

    private void Update()
    {
        if (Input.anyKey && _startSeq.IsActive())
        {
            _startSeq.Complete();
        }
    }

    private void OnMouseEnter()
    {
        if (SoundManager.Instance.soundSettingOn) return;
        _audio.Play();
        if (_scaleSeq.IsActive() && _scaleSeq != null)
        {
            _scaleSeq.Kill();
        }
        _scaleSeq.Append(transform.DOScale(1f, 0.25f).SetEase(Ease.OutCubic));
    }

    private void OnMouseExit()
    {
        if (_scaleSeq.IsActive() && _scaleSeq != null)
        {
            _scaleSeq.Kill();
        }
        _scaleSeq.Append(transform.DOScale(0.8f, 0.25f).SetEase(Ease.OutCubic));
    }

    private void OnMouseUp()
    {
        Application.Quit();
    }
}
