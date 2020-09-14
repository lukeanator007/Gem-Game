using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler

{
    public static GameObject descriptionText;
    public TextMeshProUGUI[] manaText = new TextMeshProUGUI[5];
    public TextMeshProUGUI spellName;
    string description;
    private bool mouseIsOver = false;
    private bool isDisplayingDescritption = false;

    public void loadManaText(int[] manaCost, string spellName, string description) 
    {
        for (int i = 0; i < manaCost.Length; i++)
        {
            if (manaCost[i]==0)
            {
                manaText[i].enabled = false;
            }
            else
            {
                manaText[i].enabled = true;
                manaText[i].text = manaCost[i].ToString();
            }
        }
        this.description = description;
        this.spellName.text = spellName;
    }

    private void Update()
    {
        if (mouseIsOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                descriptionText.GetComponentInChildren<TextMeshProUGUI>().text = description;
                descriptionText.transform.position = this.transform.position;

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
            descriptionText.transform.position = new Vector2(-500, -500);
            this.isDisplayingDescritption = false;
        }
        mouseIsOver = false;

        
    }
}
