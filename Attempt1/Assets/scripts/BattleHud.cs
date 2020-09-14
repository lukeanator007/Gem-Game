using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class BattleHud : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TextMeshProUGUI[] manaText = new TextMeshProUGUI[5];
    public Color spellHighlight;
    public Button[] spellButtons = new Button[4];
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI nameText;
    private bool mouseIsOver;
    private bool isDisplayingDescritption;
    public GameObject descriptionText;
    private string description;



    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (mouseIsOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                descriptionText.GetComponentInChildren<TextMeshProUGUI>().text = description;

                this.isDisplayingDescritption = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDisplayingDescritption)
        {
            descriptionText.GetComponentInChildren<TextMeshProUGUI>().text = "";
            this.isDisplayingDescritption = false;
        }
        mouseIsOver = false;


    }





    public void loadUnit(Unit unit)
    {
        int highest = 0;
        for (int i = 0; i < unit.getMaxMana().Length; i++)
        {
            if (unit.getMaxMana()[i] < highest)
            {
                highest = unit.getMaxMana()[i];
            }
        }

        for (int i = 0; i < unit.getMaxMana().Length; i++)
        {
            manaText[i].text = unit.getCurrentMana()[i].ToString();
        }
        nameText.text = unit.unitName;

        this.description = unit.getInfo();
        description = unit.getInfo();
        loadSpellButtons(unit);
        setHp(unit);
    }

    void setHp(Unit unit)
    {
        hpText.text = "HP: " + unit.getCurrentHp().ToString() + "/" + unit.getMaxHp().ToString();

    }



    public void updateSpellButtons(Unit unit)
    {
        for (int i = 0; i < spellButtons.Length; i++)
        {

            if (!unit.spellNames[i].Equals(""))
            {
                if (unit.spells[i].canCast(unit.getCurrentMana()))
                {
                    spellButtons[i].GetComponent<Image>().color = spellHighlight;
                    spellButtons[i].enabled = true;
                }
                else
                {
                    spellButtons[i].GetComponent<Image>().color = Color.white;
                    spellButtons[i].enabled = false;
                }
            }

        }
    }

    public void loadSpellButtons(Unit unit)
    {
        for (int i = 0; i < spellButtons.Length; i++)
        {
            if (unit.spells[i] == null)
            {
                spellButtons[i].gameObject.SetActive(false);
                spellButtons[i].enabled = false;
                continue;
            }
            else
            {
                spellButtons[i].gameObject.SetActive(true);
                spellButtons[i].enabled = true;
                spellButtons[i].GetComponent<SpellButton>().loadManaText(unit.spells[i].manaCost, unit.spells[i].spellName, unit.spells[i].description);
            }
        }



        updateSpellButtons(unit);
    }



    public void updateUnit(Unit unit)
    {
        updateSpellButtons(unit);
        setHp(unit);
        updateMana(unit.getCurrentMana());

    }

    public void updateMana(int[] mana)
    {
        for (int i = 0; i < mana.Length; i++)
        {
            manaText[i].text = mana[i].ToString();
        }
    }





}
