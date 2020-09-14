using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell 
{



    public abstract HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray);
    public abstract HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem);


    public string description = "";
    public int[] manaCost = new int[5];
    public string spellName = "";
    public bool givesBonusTurn = false;
    public bool targetsGem = false;


    void payManaCost(Unit caster) 
    {
        for (int i = 0; i < manaCost.Length; i++)
        {
            caster.changeMana(i, -manaCost[i]);
        }
    }

    public HashSet<int[]> castSpell(Unit caster, Unit target, int[,] gemArray)
    {
        payManaCost(caster);
        return executeSpell(caster, target, gemArray);
    }

    public HashSet<int[]> castSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
    {
        payManaCost(caster);
        return executeSpell(caster, target, gemArray, targetGem);
    }


    public bool canCast(int[] currentMana) 
    {
        for (int i = 0; i < currentMana.Length; i++)
        {
            if (currentMana[i]<this.manaCost[i])
            {
                return false;
            }
        }
        return true;


    }

    

    


}
