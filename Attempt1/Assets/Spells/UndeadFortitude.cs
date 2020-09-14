using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class UndeadFortitude : Spell
    {

        public UndeadFortitude() 
        {
            this.spellName = "Undead Fortitude";
            this.description = "Heals the caster for 10";
            this.manaCost[0] = 3;
            this.manaCost[3] = 7;
        }


        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            caster.heal(10);

            return null;
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            throw new System.NotImplementedException();
        }
    }
}