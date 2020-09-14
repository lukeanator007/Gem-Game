using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class ThumpSpell : Spell
    {

        public ThumpSpell()
        {
            this.spellName = "Thump";
            this.manaCost[1]=5;
            this.description = "does 2 damage to the opponent";
        }


        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            target.takeDamage(2);
            return null;

        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            throw new System.NotImplementedException();
        }
    }
}