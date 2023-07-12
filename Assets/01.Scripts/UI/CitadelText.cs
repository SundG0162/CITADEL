using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CitadelText : MonoBehaviour
{
    Sequence _startSeq;
    void Start()
    {
        _startSeq = DOTween.Sequence();
        _startSeq.Append(GetComponent<RectTransform>().DOAnchorPosX(-100f, 0.7f).SetEase(Ease.OutBack));
    }

    private void Update()
    {
        if (Input.anyKey && _startSeq.IsActive())
        {
            _startSeq.Complete();
        }
    }
}
