using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class FireBlast : Spell
    {

        public FireBlast() 
        {
            givesBonusTurn = true;
            this.spellName = "Fire Blast";
            this.manaCost[1] = 4;
            this.description = "Deals 1 damage to the opponent, does not end your turn";
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            target.takeDamage(1);

            return null;
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            throw new System.NotImplementedException();
        }
    }
}