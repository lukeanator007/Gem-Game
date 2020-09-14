using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour
{
    public GameObject[] heroButtons = new GameObject[3];
    public GameObject[] foeButtons = new GameObject[3];
    public TextMeshProUGUI heroDescription;
    public TextMeshProUGUI foeDescription;
    public Color highlightColour;
    static bool hasInitiated = false;


    // Start is called before the first frame update
    void Start()
    {
        if (!hasInitiated)
        {
            hasInitiated = true;
            Menu.init();
        }

        selectFoe(0);
        selectHero(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectFoe(int foeInt) 
    {
        Menu.selectFoe(foeInt);
        foeButtons[foeInt].GetComponent<Image>().color = highlightColour;
        foeButtons[(foeInt + 1) % 3].GetComponent<Image>().color = Color.white;
        foeButtons[(foeInt + 2) % 3].GetComponent<Image>().color = Color.white;
        foeDescription.text = Menu.getFoe().description;



    }

    public void selectHero(int heroInt)
    {
        Menu.selectHero(heroInt);
        heroButtons[heroInt].GetComponent<Image>().color = highlightColour;
        heroButtons[(heroInt + 1) % 3].GetComponent<Image>().color = Color.white;
        heroButtons[(heroInt + 2) % 3].GetComponent<Image>().color = Color.white;
        heroDescription.text = Menu.getHero().description;
    }

    public void startFight()
    {
        SceneManager.LoadScene("battle Scene");

    }

}
