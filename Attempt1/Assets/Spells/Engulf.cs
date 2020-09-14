using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class Engulf : Spell
    {

        public Engulf() 
        {
            this.manaCost[0] = 5;
            this.givesBonusTurn = true;
            this.spellName="Engulf";
            this.description = "Engulfs the opponent for 1 turn, making them unable to cast spells. Does not end your turn";

        }


        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            target.setStatusEngulfed(1);
            return null;
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            throw new System.NotImplementedException();
        }
    }
}