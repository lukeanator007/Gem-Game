using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class SlimeSpit : Spell
    {
        public SlimeSpit() 
        {

            this.spellName = "SlimeSpit";
            this.manaCost[1] = 1;
            this.manaCost[3] = 5;
            this.description = "does 4 damage to the opponent";
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            target.takeDamage(4);
            return null;
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            throw new System.NotImplementedException();
        }
    }
}