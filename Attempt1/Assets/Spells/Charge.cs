using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class Charge : Spell
    {
        public Charge() 
        {
            for (int i = 0; i < manaCost.Length; i++)
            {
                manaCost[i] = 3;
            }
            this.targetsGem = true;
            this.spellName = "Charge";
            this.description = "Destroys an entire row of gems";


        }


        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray)
        {
            throw new System.NotImplementedException();
        }

        public override HashSet<int[]> executeSpell(Unit caster, Unit target, int[,] gemArray, int[] targetGem)
        {
            HashSet<int[]> toRemove = new HashSet<int[]>();

            for (int i = 0; i < 8; i++)
            {
                toRemove.Add(new int[] { i, targetGem[1] });
            }

            return toRemove;

        }
    }
}