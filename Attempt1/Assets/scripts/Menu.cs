using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Assets.scripts
{

    class Menu
    {
        static int selectedFoe = 0;
        static int selectedHero = 0;
        static Dictionary<int, Unit> foes = new Dictionary<int, Unit>();
        static Dictionary<int, Unit> heroes = new Dictionary<int, Unit>();




        //TODO make Units use Spell class instead of strings for spell names
        public static void init() 
        {
            foes.Add(0, new Unit("Fire Imp", new int[] { 0, 10, 3, 0, 2 }, 5, 10, 16, new string[] {"Fire Blast", "", "", "" }, 
                "A firey deamon spellcaster, capable of lossing a rain of flaming bolts at their opponents"));
            foes.Add(1, new Unit("Skeleton", new int[] { 5, 0, 0, 12, 0 }, 15, 20, 2, new string[] { "Undead Fortitude", "", "", "" },
                "An undead warrior that is very durable"));
            foes.Add(2, new Unit("Slime", new int[] { 0, 0, 0, 15, 5 }, 5, 15, 5, new string[] { "Slime Spit", "", "", "" }, 
                "A strange lifeform, capable of enfuling its foes, making them unable to cast spells"));



            heroes.Add(0, new Unit("Warrior", new int[] { 2, 2, 2, 2, 2 }, 15, 10, 6, new string[] { "Charge", "", "", "" }, 
                "A strong fighter with high battle mastery"));
            heroes.Add(1, new Unit("Fire Mage", new int[] { 0, 15, 0, 0, 0 }, 0, 5, 15, new string[] { "Fire Blast", "", "", "" }, 
                "A more vunerable spellcaster, making up for low physical traits with strong magical abilities"));
            heroes.Add(2, new Unit("Ranger", new int[] { 5, 5, 5, 5, 5 }, 7, 7, 8, new string[] { "Charge", "Fire Blast", "", "" }, 
                "A well rounded fighter, using spells just as well as his blade"));



        }


        public static void selectFoe(int foeInt) 
        {
            selectedFoe = foeInt;
        }


        public static void selectHero(int heroInt) 
        {
            selectedHero = heroInt;
        }

        public static Unit getHero()
        {
            return heroes[selectedHero];
        }

        public static Unit getFoe()
        {
            return foes[selectedFoe];
        }
    }





}