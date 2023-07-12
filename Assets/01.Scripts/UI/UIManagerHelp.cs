using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class UIManagerHelp : MonoBehaviour
{
    public static UIManagerHelp Instance;

    private RectTransform _image;

    Transform _helpScreen;
    Transform _descriptions;
    TextMeshProUGUI _pageText;

    private int _currentPage = 0;

    public int CurrentPage
    {
        get => _currentPage;
        set => _currentPage = Mathf.Clamp(value, 0, 8);
    }

    private void Awake()
    {
        Instance = this;
        _image = transform.Find("Image").GetComponent<RectTransform>();
        _helpScreen = transform.Find("HelpScreen");
        _descriptions = _helpScreen.Find("Descriptions");
        _pageText = _descriptions.Find("Pages").GetComponent<TextMeshProUGUI>();
    }

    public void NextScene()
    {
        _image.gameObject.SetActive(true);
        StartCoroutine(Next());
        _image.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic);
    }

    public void HelpBtn()
    {
        _helpScreen.gameObject.SetActive(true);
    }

    public void CloseBtn()
    {
        _helpScreen.gameObject.SetActive(false);
    }

    public void LeftBtn()
    {
        if (CurrentPage >= 0    )
        {
            _descriptions.GetChild(CurrentPage).gameObject.SetActive(false);
            CurrentPage--;
        }
        _descriptions.GetChild(CurrentPage).gameObject.SetActive(true);
        _pageText.text = CurrentPage + 1 + "/8";
    }

    public void RightBtn()
    {
        if (CurrentPage < 7)
        {
            _descriptions.GetChild(CurrentPage).gameObject.SetActive(false);
            CurrentPage++;
        }
        _descriptions.GetChild(CurrentPage).gameObject.SetActive(true);
        _pageText.text = CurrentPage + 1 + "/8";
    }


    IEnumerator Next()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(2);
    }
}
