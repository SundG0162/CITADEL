using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void OKBtnPush()
    {
        UIManager.Instance.SettingClose();
    }

    public void ExitBtnPush()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void HelpBtnPush()
    {
        UIManagerHelp.Instance.HelpBtn();
    }

    public void CloseBtnPush()
    {
        UIManagerHelp.Instance.CloseBtn();
    }

    public void LeftBtnPush()
    {
        UIManagerHelp.Instance.LeftBtn();
    }

    public void RightBtnPush()
    {
        UIManagerHelp.Instance.RightBtn();
    }
}
