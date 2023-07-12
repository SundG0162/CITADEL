using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    bool _settingOpen = false;

    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _lightningOrbItemText;
    private TextMeshProUGUI _fireOrbItemText;
    private TextMeshProUGUI _warningText;
    private TextMeshProUGUI _gameOverText;
    private TextMeshProUGUI _gameOverText2;
    private TextMeshProUGUI _gameOverScoreText;

    private Image _gaugeImage;
    private Image _hpBarImage;
    private Image _bossHpBarImage;
    private Image _gaugeLockImage;

    private Image _panel;

    private Transform _beAttackEffect;
    private Transform _bossUITrm;
    private Transform _settingTrm;
    private Transform _gameOverBtnTrm;
    private RectTransform _warningImageRectTrm;

    Vector2 _startPos;

    public float hpValue = 250;

    public float bossHp = 2000;

    public int enemyHpValue = 0;

    public float _gaugeValue = 1000;
    private float _gaugeDown = 0;

    public int score = 0;

    public int fireOrb = 0;
    public int lightningOrb = 0;

    public float min = 1.5f;
    public float max = 4f;
    public float a = 0;

    private string _gameOver1 = "실패하고 말았습니다.";
    private string _gameOver2 = "다시 시도하시겠습니까?";

    public void Init(Transform canvas)
    {
        _lightningOrbItemText = canvas.Find("LightningOrb/LightningOrbCounter").GetComponent<TextMeshProUGUI>();
        _fireOrbItemText = canvas.Find("FireOrb/FireOrbCounter").GetComponent<TextMeshProUGUI>();
        _gaugeImage = canvas.Find("GaugeBar/Gauge").GetComponent<Image>();
        _gaugeLockImage = canvas.Find("GaugeBar/Lock").GetComponent<Image>();
        _hpBarImage = canvas.Find("HpBar/Gauge").GetComponent<Image>();
        _bossHpBarImage = canvas.Find("BossUI/BossHpBar/Gauge").GetComponent<Image>();
        _panel = canvas.Find("GameOverPanel").GetComponent<Image>();
        _beAttackEffect = canvas.Find("BeAttackEffect");
        _bossUITrm = canvas.Find("BossUI");
        _settingTrm = canvas.Find("Setting");
        _scoreText = canvas.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _warningImageRectTrm = canvas.Find("WarningImage").GetComponent<RectTransform>();
        _warningText = _warningImageRectTrm.Find("WarningText").GetComponent<TextMeshProUGUI>();
        _gameOverText = canvas.Find("GameOverPanel/GameOverText").GetComponent<TextMeshProUGUI>();
        _gameOverText2 = canvas.Find("GameOverPanel/GameOverText2").GetComponent<TextMeshProUGUI>();
        _gameOverScoreText = canvas.Find("GameOverPanel/ScoreText").GetComponent<TextMeshProUGUI>();
        _gameOverBtnTrm = canvas.Find("GameOverPanel/Btns");
        _startPos = _warningImageRectTrm.anchoredPosition;
    }

    private void Start()
    {
        StartCoroutine(ScoreUpCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingOpen)
            {
                SettingClose();
            }
            else
            {
                SettingOpen();
            }
        }
        if (TowerManager.Instance.blueTowerActive) return;
        _gaugeValue += _gaugeDown;
        if (_gaugeValue > 1000)
        {
            _gaugeValue = 1000;
        }
        _gaugeImage.fillAmount = _gaugeValue / 1000;
    }

    private void SettingOpen()
    {
        TowerManager.Instance.blueTowerActive = true;
        Time.timeScale = 0;
        _settingOpen = true;
        _settingTrm.gameObject.SetActive(true);
    }

    public void SettingClose()
    {
        TowerManager.Instance.blueTowerActive = false;
        Time.timeScale = 1;
        _settingOpen = false;
        _settingTrm.gameObject.SetActive(false);
    }



    public void FireOrbUp()
    {
        fireOrb++;
        _fireOrbItemText.text = fireOrb.ToString();
    }

    public void LightningOrbUp()
    {
        lightningOrb++;
        _lightningOrbItemText.text = lightningOrb.ToString();
    }

    public void FireOrbDown()
    {
        fireOrb--;
        _fireOrbItemText.text = fireOrb.ToString();
    }

    public void LightningOrbDown()
    {
        lightningOrb--;
        _lightningOrbItemText.text = lightningOrb.ToString();
    }

    public void GaugeControll(float value)
    {
        _gaugeDown = value;
    }

    public void BeAttack(float damage)
    {
        HpDown(damage);
        StopCoroutine(BeAttackCoroutine());
        StartCoroutine(BeAttackCoroutine());
    }

    IEnumerator BeAttackCoroutine()
    {
        _beAttackEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _beAttackEffect.gameObject.SetActive(false);
    }

    public void ScoreUp(int score)
    {
        this.score += score;
        _scoreText.text = string.Format("{0:D6}", this.score);
        if (this.score % 1000 == 0 && this.score > 0)
        {
            min -= 0.05f;
            max -= 0.05f;
        }
        if (this.score % 2000 == 0 && this.score > 0)
        {
            enemyHpValue += 10;
        }
    }

    public void HpDown(float value)
    {
        hpValue -= value;
        _hpBarImage.fillAmount = hpValue / 250;
    }

    public void HpUp(float value)
    {
        hpValue += value;
        if (hpValue > 250)
        {
            hpValue = 250;
        }
        _hpBarImage.fillAmount = hpValue / 250;
    }

    public void BossHpInit()
    {
        _bossHpBarImage.fillAmount = bossHp / 5000;
    }


    public void TowerAttack(string text)
    {
        _warningText.SetText(text);
        Sequence seq = DOTween.Sequence();
        seq.Append(_warningImageRectTrm.DOAnchorPosY(200, 1f).SetEase(Ease.OutBack));
        seq.AppendInterval(1f);
        seq.Append(_warningImageRectTrm.DOAnchorPosY(_startPos.y, 1f).SetEase(Ease.InBack));
    }

    public void LockActive(bool acitve)
    {
        TowerManager.Instance.blueTowerActive = acitve;
        if (acitve)
        {
            _gaugeLockImage.gameObject.SetActive(true);
        }
        else
        {
            _gaugeLockImage.gameObject.SetActive(false);
        }
    }

    public void BossUIDisplay()
    {
        _bossUITrm.gameObject.SetActive(true);
    }

    bool active = false;
    public void GameOver()
    {
        if (active) return;
        active = true;
        Time.timeScale = 0;
        BGMManager.Instance.BGMOff();
        _panel.gameObject.SetActive(true);
        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        yield return new WaitForSecondsRealtime(2f);
        _gameOverText.gameObject.SetActive(true);
        foreach (char item in _gameOver1)
        {
            _gameOverText.text += item;
            yield return new WaitForSecondsRealtime(0.15f);
        }
        yield return new WaitForSecondsRealtime(1f);
        _gameOverText2.gameObject.SetActive(true);
        foreach (char item in _gameOver2)
        {
            _gameOverText2.text += item;
            yield return new WaitForSecondsRealtime(0.15f);
        }
        yield return new WaitForSecondsRealtime(0.6f);
        _gameOverScoreText.gameObject.SetActive(true);
        _gameOverScoreText.text = string.Format("당신의 점수\n:{0:D6}", score);
        yield return new WaitForSecondsRealtime(0.8f);
        _gameOverBtnTrm.gameObject.SetActive(true);


    }

    IEnumerator ScoreUpCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ScoreUp(10);
        }
    }
}