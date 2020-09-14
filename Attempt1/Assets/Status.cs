using UnityEngine;
using System.Collections;

namespace Assets
{

    //WIP
    public class Status
    {
        public string name;
        public int duration = -1;
        public int maxStacks = 1;

        public Status(int duration, string name)
        {

        }



        /**
         *  returns true if this status should end
         */
        public bool startTurn(Unit unit) 
        {
            duration--;
            if (duration<0)
            {
                return true;
            }
            return false;
        }
       
    }
}