using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class intArray2Comparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        public int GetHashCode(int[] a)
        {
            int b = 0;
            for (int i = 0; i < a.Length; i++)
                b = ((b << 23) | (b >> 9)) ^ a[i];
            return unchecked((int)b);
        }

        





    }
}