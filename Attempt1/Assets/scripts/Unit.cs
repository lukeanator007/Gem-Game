using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit
{
   
    public string unitName = "";
    public string description = "";

    public int[] manaMasteries = new int[5];

    public int battleMastery = 0;
    public int fortitude = 0;
    public int forsight = 0;


    int baseHp = 10;
    int baseMaxMana = 15;
    int masteryDividor = 4;
    float manaDivisor = 15f;

    int maxHp;
    int currentHp;
    int[] maxMana = new int[5];
    int[] currentMana = new int[5];

    public string[] spellNames = new string[4];
    public Spell[] spells = new Spell[4];


    static Dictionary<string, Spell> spellMap = new Dictionary<string, Spell>();


    public Unit(string unitName, int[] manaMasteries, int battleMastery, int fortitude, int forsight, string[] spellNames, string description) 
    {
        this.unitName = unitName;
        this.manaMasteries = manaMasteries;
        this.battleMastery = battleMastery;
        this.fortitude = fortitude;
        this.forsight = forsight;
        this.spellNames = spellNames;
        this.description = description;


    }



    public string getInfo() 
    {
        return "earth: " + manaMasteries[0] + "\nfire: " + manaMasteries[1] + "\nmetal: " + manaMasteries[2] + "\nwater: " + manaMasteries[3] + 
            "\nwood: " + manaMasteries[4] + "\nbattle: " +battleMastery+ "\nfortitude: " +fortitude+ "\nforsight:"+forsight;
    }

    public static void initSpellMap() 
    {
        if (spellMap.Count>0)
        {
            return;
        }
        spellMap.Add("Thump", new ThumpSpell());
        spellMap.Add("Slime Spit", new SlimeSpit());
        //spellMap.Add("Engulf", new Engulf());
        spellMap.Add("Charge", new Charge());
        spellMap.Add("Undead Fortitude", new UndeadFortitude());
        spellMap.Add("Fire Blast", new FireBlast());

        spellMap.Add("", null);

    }

    public void takeDamage(int damage) 
    {
        this.currentHp -= damage;
        if (currentHp<0)
        {
            currentHp = 0;
        }
    }

    public void heal(int healAmount)
    {
        this.currentHp += healAmount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }


    public void setStatusEngulfed(int duration) 
    {


    }



    public void init()
    {
        for (int i = 0; i < spellNames.Length; i++)
        {
            spells[i] = spellMap[spellNames[i]];
        }

        maxHp = baseHp + fortitude;
        currentHp = maxHp;

        for (int i = 0; i < manaMasteries.Length; i++)
        {
            maxMana[i] = manaMasteries[i] + baseMaxMana;
            currentMana[i] = manaMasteries[i] / masteryDividor;
        }

    }

    public void changeMana(int manaType, int amout) 
    {
        this.currentMana[manaType] += amout;

    }



    public int[] getMaxMana()
    {
        return maxMana;
    }

    public int getMaxHp()
    {
        return maxHp;
    }

    public int getCurrentHp()
    {
        return currentHp;
    }


    public int[] getCurrentMana()
    {
        return currentMana;
    }




    void logArray(int[] arr) 
    {
        string str = "";
        for (int i = 0; i < arr.Length; i++)
        {
            str += arr[i] + ", ";
        }

        Debug.Log(str);

    }

    public int recieveGems(int[] gems)
    {
        
        if (gems == null)
        {
            return 0;
        }


        string str = "";
        for (int i = 0; i < currentMana.Length; i++)
        {
            currentMana[i] +=(int)  System.Math.Floor(gems[i]* (1f + ((float)manaMasteries[i] / manaDivisor)));
            str += (int)System.Math.Floor(gems[i] * (1f + ((float)manaMasteries[i] / manaDivisor))) + ", ";
            if (currentMana[i] > maxMana[i])
            {
                currentMana[i] = maxMana[i];
            }
        }


        //return amount of damage
        return (int)System.Math.Floor(gems[currentMana.Length] * (1f + (float)(battleMastery / manaDivisor)));


    }
}
