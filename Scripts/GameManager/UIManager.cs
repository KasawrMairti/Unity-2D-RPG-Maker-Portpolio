using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManager : GameManager<UIManager>
{
    public Canvas canvas;

    [Header("Menu List")]
    public GameObject ui_upperUIFull;
    public GameObject ui_menuList;
    public GameObject ui_menu;

    [Header("Player Gage")]
    public Slider sliderHP;
    public Slider sliderMP;
    public Slider sliderExP;

    [Header("Fade In/Out Event")]
    [SerializeField] private Image FadeBlackObject;
    private bool B_EventFadeInOut = false;
    public bool b_EventFadeInOut
    {
        get
        {
            if (B_EventFadeInOut)
            {
                B_EventFadeInOut = false;
                return true;
            }
            else return false;
        }
    }

    [Header("Status Panel")]
    public TextMeshProUGUI playerStatusText;
    public GameObject playerStatusUpgradeText;
    public TextMeshProUGUI playerStatText_Bonus;
    public TextMeshProUGUI playerStatText_STR;
    public Button btn_STRUp;
    public Button btn_STRDown;
    public TextMeshProUGUI playerStatText_DEX;
    public Button btn_DEXUp;
    public Button btn_DEXDown;
    public TextMeshProUGUI playerStatText_INT;
    public Button btn_INTUp;
    public Button btn_INTDown;
    public TextMeshProUGUI playerStatText_WIS;
    public Button btn_WISUp;
    public Button btn_WISDown;
    public TextMeshProUGUI playerStatText_CON;
    public Button btn_CONUp;
    public Button btn_CONDown;
    public TextMeshProUGUI playerStatText_LUK;
    public Button btn_LUKUp;
    public Button btn_LUKDown;

    [Header("Other")]
    public GameObject o_BTNorder;
    public Image imageFull;

    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = UserDataManager.Instance.data.dtoPlayer.status;

        btn_STRUp.onClick.AddListener(() => UpgradeSTR());
        btn_STRDown.onClick.AddListener(() => DowngradeSTR());
        btn_DEXUp.onClick.AddListener(() => UpgradeDEX());
        btn_DEXDown.onClick.AddListener(() => DowngradeDEX());
        btn_INTUp.onClick.AddListener(() => UpgradeINT());
        btn_INTDown.onClick.AddListener(() => DowngradeINT());
        btn_WISUp.onClick.AddListener(() => UpgradeWIS());
        btn_WISDown.onClick.AddListener(() => DowngradeWIS());
        btn_CONUp.onClick.AddListener(() => UpgradeCON());
        btn_CONDown.onClick.AddListener(() => DowngradeCON());
        btn_LUKUp.onClick.AddListener(() => UpgradeLUK());
        btn_LUKDown.onClick.AddListener(() => DowngradeLUK());

    }


    private void Update()
    {
        UpdateText();
    }

    public void OnStage()
    {
        LoadingManager.Instance.LoadScene("BattleScene_Stage1");
    }

    public void OnBTNOpen(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void OnBTNClose(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void FadeInEvent(float time)
    {
        StartCoroutine(FadeIn(time));
    }

    private IEnumerator FadeIn(float time)
    {
        FadeBlackObject.gameObject.SetActive(true);

        float timeCur = 0.0f;
        Color colorOrigin = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color colorEnd = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        while (timeCur < time)
        {
            timeCur += Time.deltaTime;

            FadeBlackObject.color = Color.Lerp(colorOrigin, colorEnd, timeCur / time);

            yield return null;
        }

        B_EventFadeInOut = true;
    }

    public void FadeOutEvent(float time)
    {
        StartCoroutine(FadeOut(time));
    }

    private IEnumerator FadeOut(float time)
    {
        float timeCur = 0.0f;
        Color colorOrigin = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Color colorEnd = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        while (timeCur < time)
        {
            timeCur += Time.deltaTime;

            FadeBlackObject.color = Color.Lerp(colorOrigin, colorEnd, timeCur / time);

            yield return null;
        }

        FadeBlackObject.gameObject.SetActive(false);
        B_EventFadeInOut = true;
    }

    private void UpdateText()
    {
        // 내 정보 - PlayerStatus
        if (playerStatusText.gameObject.activeSelf)
        {
            playerStatusText.text =
                $" 공격력 : {playerStatus.attackDamage}\n 공격속도 : {playerStatus.attackSpeed}\n 치명타 확률 : {playerStatus.attackCritical}\n 치명타 데미지 : {playerStatus.attackCriticalDamage}\n\n " +
                $"체력 : {playerStatus.Hp} / {playerStatus.HpMax}\n 체력재생 : {playerStatus.HpRegen}\n 방어력 : {playerStatus.deffence}\n\n 회피율 : {playerStatus.avoid}";
        }

        if (playerStatusUpgradeText.activeSelf)
        {
            playerStatText_Bonus.text = $"남은 스텟 보너스 : {playerStatus.Stat_Bonus}";
            playerStatText_STR.text = $"{playerStatus.Stat_STR}";
            playerStatText_DEX.text = $"{playerStatus.Stat_DEX}";
            playerStatText_INT.text = $"{playerStatus.Stat_INT}";
            playerStatText_WIS.text = $"{playerStatus.Stat_WIS}";
            playerStatText_CON.text = $"{playerStatus.Stat_CON}";
            playerStatText_LUK.text = $"{playerStatus.Stat_LUK}";
        }
    }

    private void UpgradeSTR()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_STR++;
        }
    }

    private void DowngradeSTR()
    {
        if (playerStatus.Stat_STR > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_STR--;
        }
    }

    private void UpgradeDEX()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_DEX++;
        }
    }

    private void DowngradeDEX()
    {
        if (playerStatus.Stat_DEX > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_DEX--;
        }
    }

    private void UpgradeINT()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_INT++;
        }
    }

    private void DowngradeINT()
    {
        if (playerStatus.Stat_INT > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_INT--;
        }
    }

    private void UpgradeWIS()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_WIS++;
        }
    }

    private void DowngradeWIS()
    {
        if (playerStatus.Stat_WIS > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_WIS--;
        }
    }

    private void UpgradeCON()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_CON++;
        }
    }

    private void DowngradeCON()
    {
        if (playerStatus.Stat_CON > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_CON--;
        }
    }

    private void UpgradeLUK()
    {
        if (playerStatus.Stat_Bonus > 0)
        {
            playerStatus.Stat_Bonus--;

            playerStatus.Stat_LUK++;
        }
    }

    private void DowngradeLUK()
    {
        if (playerStatus.Stat_LUK > 0)
        {
            playerStatus.Stat_Bonus++;

            playerStatus.Stat_LUK--;
        }
    }
}
