using Assets.scripts;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GemArray : MonoBehaviour
{
    public Spell[] spell;
    public bool isPlayersTurn = true;
    public GameObject playerHud;
    public GameObject enemyHud;
    public GameObject playerTurnIndicator;
    public GameObject enemyTurnIndicator;
    public GameObject spellDescriptionText;
    public GameObject gameEndScrene;


    public Unit playerUnit;
    public Unit enemyUnit;

    public GameObject[] enemySelectors = new GameObject[2];

    public GameObject gemPrefab;
    public GameObject destroyer;
    public int gridSize = 8;
    int[,] gemGrid;
    HashSet<int[]> toRemove = new HashSet<int[]>(new intArray2Comparer());
    public Vector2 spawnPos = new Vector2(0f, 8f);
    public Vector2 spawnShift = new Vector2(1f, 0f);
    public Vector2 spawnShiftVertical = new Vector2(0f, 1f);
    public Vector2 destroyerOffset = new Vector2(0.5f, 0.5f);
    public Sprite[] sprites = new Sprite[6];
    bool doGemLoop = false;
    bool spellTarget = false;
    int spellTargetNumber = -1;
    bool bothAreAlive = true;
    bool bonusTurn = false;
    System.Random rnd = new System.Random();


    

    public bool getSpellTarget() 
    {
        return spellTarget;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        Unit.initSpellMap();
        Gem.findItems();
        SpellButton.descriptionText = spellDescriptionText;
        gemGrid = new int[gridSize, gridSize];

        startMatch();
        
    }


    void startMatch() 
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Gem"))
        {
            Destroy(item);
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                int tempRand = rnd.Next(0, sprites.Length);

                gemGrid[i, j] = tempRand;
            }
        }

        while (checkMatches())
        {
            foreach (int[] item in toRemove)
            {
                gemGrid[item[0], item[1]] = rnd.Next(0, sprites.Length);
            }
            toRemove.Clear();
        }



        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject tempObject = Instantiate(gemPrefab, spawnPos + (spawnShift * i) + (spawnShiftVertical * j), Quaternion.identity);
                tempObject.GetComponent<SpriteRenderer>().sprite = sprites[gemGrid[i, j]];
            }
        }

        loadUnits();
        isPlayersTurn = playerUnit.forsight>=enemyUnit.forsight;
        displayTurnIndicator();

        bonusTurn = false;
        doGemLoop = false;
        if (!isPlayersTurn)
        {
            StartCoroutine(enemyTurnSlowStart());
        }

    }




    public void displayTurnIndicator() 
    {
        playerTurnIndicator.GetComponent<Image>().enabled = isPlayersTurn;
        enemyTurnIndicator.GetComponent<Image>().enabled = !isPlayersTurn;
    }

    public void playerSpell(int spellNo)
    {
        if (!spellTarget)
        {
            bonusTurn = playerUnit.spells[spellNo].givesBonusTurn;
            if (playerUnit.spells[spellNo].targetsGem)
            {
                this.spellTarget = true;
                this.spellTargetNumber = spellNo;
            }
            else
            {
                HashSet<int[]> spellToRemove = playerUnit.spells[spellNo].castSpell(playerUnit, enemyUnit, gemGrid);
                if (spellToRemove != null)
                {
                    toRemove.UnionWith(spellToRemove);
                }
                executeSpell();
            }
        }
        
    }


    public void enemySpell(int spellNo)
    {
        bonusTurn = enemyUnit.spells[spellNo].givesBonusTurn;
        if (enemyUnit.spells[spellNo].targetsGem)
        {
            HashSet<int[]> spellToRemove = enemyUnit.spells[spellNo].castSpell(enemyUnit, playerUnit, gemGrid, new int[] { rnd.Next(0, gridSize), rnd.Next(0, gridSize) });
            if (spellToRemove != null)
            {
                toRemove.UnionWith(spellToRemove);
            }
        }
        else
        {
            HashSet<int[]> spellToRemove = enemyUnit.spells[spellNo].castSpell(enemyUnit, playerUnit, gemGrid);
            if (spellToRemove != null)
            {
                toRemove.UnionWith(spellToRemove);
            }
        }
        executeSpell();


    }




    void executeSpell() 
    {

        playerHud.GetComponent<BattleHud>().updateUnit(playerUnit);
        enemyHud.GetComponent<BattleHud>().updateUnit(enemyUnit);

        doGemLoop = true;
    }

    public void playerTargetedSpell(int[] target)
    {
        
        this.spellTarget = false;
        HashSet<int[]> spellToRemove = playerUnit.spells[spellTargetNumber].castSpell(playerUnit, enemyUnit, gemGrid, target);
        if (spellToRemove != null)
        {
            toRemove.UnionWith(spellToRemove);
        }
        executeSpell();

        this.spellTargetNumber = -1;
    }





    public bool getDoGemLoop()
    {
        return doGemLoop;
    }

// Update is called once per frame
    
    private void Update()
    {
        if (doGemLoop)
        {
            bool isFalling = false;
            
            foreach (GameObject gem in GameObject.FindGameObjectsWithTag("Gem"))
            {
                if (System.Math.Abs(gem.GetComponent<Rigidbody2D>().velocity.y)>0.1f||gem.GetComponent<Transform>().position.y>spawnPos.y+0.2)
                {
                    isFalling = true;
                    break;
                }
            }
            if (!isFalling)
            {
                gemLoop();
            }


        }





    }

    public void toMainMenu() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void restartLevel() 
    {
        gameEndScrene.GetComponent<Canvas>().enabled = false;
        startMatch();

    }



    public HashSet<int[]> getLegalMoves() 
    {
        HashSet<int[]> legalMoves = new HashSet<int[]>(new intArray2Comparer());//a move is a pair of 2 gems, so a int[4]

        for (int i = 0; i < gridSize-1; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (gemGrid[i,j]==gemGrid[i+1,j])
                {
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 1, j + 1]) legalMoves.Add(new int[] { i - 1, j + 1, i - 1, j });
                    }
                    catch (Exception) {}
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 1, j - 1]) legalMoves.Add(new int[] { i - 1, j - 1, i - 1, j });
                    }
                    catch (Exception) { }

                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 2 , j + 1]) legalMoves.Add(new int[] { i + 2, j + 1, i + 2, j });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 2, j - 1]) legalMoves.Add(new int[] { i + 2, j - 1, i + 2, j });
                    }
                    catch (Exception) { }

                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 2, j]) legalMoves.Add(new int[] { i - 2, j, i - 1, j });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 3, j]) legalMoves.Add(new int[] { i + 3, j, i + 2, j });
                    }
                    catch (Exception) { }

                }
            }
        }



        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize-1; j++)
            {
                if (gemGrid[i, j] == gemGrid[i, j + 1])
                {
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 1, j - 1]) legalMoves.Add(new int[] { i + 1, j - 1, i, j - 1 });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 1, j - 1]) legalMoves.Add(new int[] { i - 1, j - 1, i, j - 1 });
                    }
                    catch (Exception) { }

                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 1, j + 2]) legalMoves.Add(new int[] { i + 1, j + 2, i, j + 2 });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 1, j + 2]) legalMoves.Add(new int[] { i - 1, j + 2, i, j + 2 });
                    }
                    catch (Exception) { }

                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i, j - 2]) legalMoves.Add(new int[] { i, j - 2, i , j - 1});
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i, j + 3]) legalMoves.Add(new int[] { i, j + 3, i , j + 2});
                    }
                    catch (Exception) { }

                }
            }
        }


        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize-2; j++)
            {
                if (gemGrid[i,j]==gemGrid[i,j+2])
                {
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i - 1, j + 1]) legalMoves.Add(new int[] { i - 1, j + 1, i, j + 1 });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 1, j + 1]) legalMoves.Add(new int[] { i + 1, j + 1, i, j + 1 });
                    }
                    catch (Exception) { }
                }
            }
        }

        for (int i = 0; i < gridSize - 2; i++) 
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (gemGrid[i, j] == gemGrid[i + 2, j])
                {
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 1, j - 1]) legalMoves.Add(new int[] { i + 1, j, i+1, j - 1 });
                    }
                    catch (Exception) { }
                    try
                    {
                        if (gemGrid[i, j] == gemGrid[i + 1, j + 1]) legalMoves.Add(new int[] { i + 1, j, i + 1, j + 1 });
                    }
                    catch (Exception) { }
                }
            }
        }


        return legalMoves;

    }


    void logGemGrid() 
    {
        String ans = "";
        for (int i = gridSize - 1; i > -1; i--)
        {
            for (int j = 0; j < gridSize ; j++)
            {
                ans += gemGrid[j, i]+", ";

            }
            ans += "\n";
        }
        Debug.Log(ans);

    }

    void loadUnits() 
    {
        bothAreAlive = true;
        playerUnit = Menu.getHero();
        playerUnit.init();
        enemyUnit = Menu.getFoe();
        enemyUnit.init();
        playerHud.GetComponent<BattleHud>().loadUnit(playerUnit);
        enemyHud.GetComponent<BattleHud>().loadUnit(enemyUnit);
    }



    public bool getBothAreAlive() 
    {
        return bothAreAlive;
    }

    void gemLoop() 
    {

        if (checkMatches())
        {
            int[] removedGems = removeMatches();
            if (isPlayersTurn)
            {
                enemyUnit.takeDamage(playerUnit.recieveGems(removedGems));
                playerHud.GetComponent<BattleHud>().updateUnit(playerUnit);
                enemyHud.GetComponent<BattleHud>().updateUnit(enemyUnit);
            }
            else
            {
                playerUnit.takeDamage(enemyUnit.recieveGems(removedGems));
                enemyHud.GetComponent<BattleHud>().updateUnit(enemyUnit);
                playerHud.GetComponent<BattleHud>().updateUnit(playerUnit);
                //send to enemy
            }


        }
        else 
        {
            doGemLoop = false;
            if (playerUnit.getCurrentHp()==0)
            {
                isPlayersTurn = false;
                gameEndScrene.GetComponent<Canvas>().enabled = true;
                gameEndScrene.GetComponentInChildren<TextMeshProUGUI>().text = "You are Defeated!";
            }
            else if (enemyUnit.getCurrentHp()==0)
            {
                isPlayersTurn = false;
                isPlayersTurn = false;
                gameEndScrene.GetComponent<Canvas>().enabled = true;
                gameEndScrene.GetComponentInChildren<TextMeshProUGUI>().text = "You are Victorious!";
            }
            else
            {
                if (bonusTurn)
                {
                    bonusTurn = false;
                }
                else
                {
                    isPlayersTurn = !isPlayersTurn;
                }

                displayTurnIndicator();
                if (!isPlayersTurn)
                {
                    StartCoroutine(enemyTurn());
                }
            }

            

        }
    }

    System.Collections.IEnumerator enemyTurnSlowStart() 
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(enemyTurn());

    }


    System.Collections.IEnumerator enemyTurn() 
    {
        if (bothAreAlive)
        {
            yield return new WaitForSeconds(1);

            //TODO make better
            bool castSpell = false;
            for (int i = 0; i < enemyUnit.spellNames.Length; i++)
            {
                if (enemyUnit.spells[i] != null)
                {
                    if (enemyUnit.spells[i].canCast(enemyUnit.getCurrentMana()))
                    {
                        if (rnd.Next(0,2)==0)
                        {
                            enemySpell(i);
                            castSpell = true;
                            break;
                        }
                    }
                }
                else 
                {
                    break;
                }
            }





            if (!castSpell)
            {
                HashSet<int[]> moves = getLegalMoves();
                int ran = rnd.Next(0, moves.Count);
                int count = 0;
                int[] move = null;
                foreach (int[] item in moves)
                {
                    if (count == ran)
                    {
                        move = item;
                        break;
                    }
                    count++;
                }

                enemySelectors[0].GetComponent<Transform>().position = new Vector2(move[0], move[1]);
                enemySelectors[1].GetComponent<Transform>().position = new Vector2(move[2], move[3]);
                GameObject gem1 = null;
                GameObject gem2 = null;
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Gem"))
                {
                    int y = (int)System.Math.Round(item.transform.position.y);
                    int x = (int)System.Math.Round(item.transform.position.x);
                    if (x == move[0] && y == move[1])
                    {
                        gem1 = item;
                    }
                    if (x == move[2] && y == move[3])
                    {
                        gem2 = item;
                    }


                }


                yield return new WaitForSeconds(1);

                enemySelectors[0].GetComponent<Transform>().position = new Vector2(-10, -10);
                enemySelectors[1].GetComponent<Transform>().position = new Vector2(-10, -10);

                StartCoroutine(SwapGems(new int[] { move[0], move[1] }, new int[] { move[2], move[3] }, gem1, gem2));
            }
        }
        


    }


    GameObject SpawnGem(float x, float y, int sprite) 
    {
        GameObject tempObject = Instantiate(gemPrefab, spawnPos + (spawnShift * x) + (spawnShiftVertical * y), Quaternion.identity);
        tempObject.GetComponent<SpriteRenderer>().sprite = sprites[sprite];
        return tempObject;
    }









    public IEnumerator SwapGems(int[] gem1, int[] gem2, GameObject object1, GameObject object2) 
    {



        if (getLegalMoves().Contains(new int[] { gem1[0], gem1[1], gem2[0], gem2[1]})||getLegalMoves().Contains(new int[] { gem2[0], gem2[1], gem1[0], gem1[1] }))
        {
            int temp1 = gemGrid[gem1[0], gem1[1]];
            int temp2 = gemGrid[gem2[0], gem2[1]];

            gemGrid[gem1[0], gem1[1]] = temp2;
            gemGrid[gem2[0], gem2[1]] = temp1;


            object1.GetComponent<SpriteRenderer>().sprite = sprites[temp2];
            object2.GetComponent<SpriteRenderer>().sprite = sprites[temp1];

            yield return new WaitForSeconds(1);
            doGemLoop = true;
            
        }
        else
        {
            //yield return new WaitForSeconds(0);
            Debug.Log("false");
            //todo animate illegal move
        }

        
        

    }

    private void FixedUpdate() 
    {

    }

  

    int[] removeMatches()
    {
        int[] ans = new int[sprites.Length];


        if (toRemove.Count == 0)
        {
            return null;
        }

        int[] toSpawn = new int[gridSize];
        foreach (int[] item in toRemove)
        {
            ans[gemGrid[item[0], item[1]]]++;
            gemGrid[item[0], item[1]] = -1;
            toSpawn[item[0]]++;


            Instantiate(destroyer, new Vector2((float)item[0], (float)item[1]), Quaternion.identity);

        }



        for (int i = 0; i < toSpawn.Length; i++)
        {
            if (toSpawn[i]>0)
            {
                int columnIndex = 0;
                int[] tempColumn = new int[gridSize - toSpawn[i] + 1];
                for (int j = 0; j < gridSize; j++)
                {
                    if (gemGrid[i, j] != -1)
                    {
                        tempColumn[columnIndex] = gemGrid[i, j];
                        columnIndex++;
                    }
                }


                for (int j = 0; j < tempColumn.Length-1; j++)
                {
                    gemGrid[i, j] = tempColumn[j]; 
                }



            }
            for (int j = 0; j < toSpawn[i]; j++)
            {
                int tempRan = rnd.Next(0,sprites.Length);
                SpawnGem(i, toSpawn[i]-j, tempRan);
                gemGrid[i, gridSize-j-1] = tempRan;
            }

           

        }


        toRemove.Clear();
        return ans;
    }


    bool checkMatches()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                int index=1;
                while (true)
                {
                    if (i + index >= gridSize) 
                    {
                        break;
                    }
                    if (gemGrid[i, j] == gemGrid[i + index, j])
                    {
                        index++;
                    }
                    else 
                    {
                        break;
                    }
                }
                if (index > 2)
                {
                    for (int k = 0; k < index; k++)
                    {
                        int[] tempIntArr = new int[2] { i + k, j };
                        toRemove.Add(tempIntArr); 
                    }
                    if (index > 3)
                    {
                        bonusTurn = true;
                    }
                }

                int index2 = 1;
                while (true)
                {
                    if (j + index2 >= gridSize)
                    {
                        break;
                    }
                    if (gemGrid[i, j] == gemGrid[i , j + index2])
                    {
                        index2++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (index2 > 2)
                {
                    for (int k = 0; k < index2; k++)
                    {
                        int[] tempIntArr = new int[2] { i , j + k };
                        toRemove.Add(tempIntArr);

                    }
                    if (index2 > 3)
                    {
                        bonusTurn = true;
                    }
                }

            }
        }
        return toRemove.Count != 0;

    }












}
