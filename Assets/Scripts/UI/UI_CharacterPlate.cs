using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterPlate : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI characterNameText;
    [SerializeField]
    private Image hpBarImage;

    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI maxHPText;

    [Space(10)]
    [SerializeField]
    private Character targetCharacter;

    private float curHP;
    private float curMaxHP;

    private void Start()
    {
        Assert.IsNotNull(characterNameText);
        Assert.IsNotNull(hpBarImage);
       
        bindUI(targetCharacter.GetHealthSystem());
        Init();
    }
    private void OnEnable()
    {
        UpdateUI();
        bindUI(targetCharacter.GetHealthSystem());
    }
    private void OnDisable()
    {
        unBindUI(targetCharacter.GetHealthSystem());
    }
    private void OnDestroy()
    {
        unBindUI(targetCharacter.GetHealthSystem());
    }
    public void Init()
    {
        setNameTextColor(Color.black);
        
        HealthSystem targetHealthSystem = targetCharacter.GetHealthSystem();
        curHP = targetHealthSystem.GetHP();
        curMaxHP = targetHealthSystem.GetMaxHP();

        UpdateUI();
    }
    public void UpdateUI()
    {
        setCharacterText(targetCharacter.name);
        setHPText(curHP);
        setMaxHPText(curMaxHP);
        setHPFillAmount(curHP / curMaxHP);
    }
    private void bindUI(HealthSystem targetHealthSystem)
    {
        if (targetHealthSystem == null)
        {
            return;
        }
        targetHealthSystem.OnHealthChanged += onHealthChanged;
        targetHealthSystem.OnMaxHealthChanged += onMaxHealthChanged;
        targetHealthSystem.OnDeath += onDeath;
    }
    private void unBindUI(HealthSystem targetHealthSystem)
    {
        if(targetHealthSystem == null)
        {
            return;
        }
        targetHealthSystem.OnHealthChanged -= onHealthChanged;
        targetHealthSystem.OnMaxHealthChanged -= onMaxHealthChanged;
        targetHealthSystem.OnDeath -= onDeath;
    }
    private void onHealthChanged(float oldVal, float newVal)
    {
        curHP = newVal;
        setHPText(curHP);
        setHPFillAmount(curHP / curMaxHP);
    }
    private void onMaxHealthChanged(float oldVal, float newVal)
    {
        curMaxHP = newVal;
        setMaxHPText(curMaxHP);
        setHPFillAmount(curHP / curMaxHP);
    }

    private void onDeath()
    {
        setNameTextColor(Color.red);
    }
    private void setCharacterText(string name)
    {
        characterNameText.SetText(name);
    }
    private void setHPText(float hp)
    {
        hpText.SetText(((int)hp).ToString());
    }
    private void setMaxHPText(float maxHP)
    {
        maxHPText.SetText(((int)maxHP).ToString());
    }
    private void setHPFillAmount(float hpPercentage)
    {
        hpBarImage.fillAmount = hpPercentage;
    }
    private void setNameTextColor(Color fontColor)
    {
        characterNameText.color = fontColor;
    }
}
