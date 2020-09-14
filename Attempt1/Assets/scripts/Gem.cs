using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gem : MonoBehaviour
{
    public static  GameObject gemHandler;
    public static GameObject selector;
    public static Sprite[] sprites=new Sprite[6];
    static int[] selectedGem = new int[] { -10, -10 };
    static GameObject selectedGemObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    static public void findItems() 
    {
        gemHandler = GameObject.Find("GemHandler");
        selector = GameObject.Find("Selector");
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("destroyer"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!gemHandler.GetComponent<GemArray>().getDoGemLoop() && gemHandler.GetComponent<GemArray>().isPlayersTurn && gemHandler.GetComponent<GemArray>().getBothAreAlive())
            {
                if (gemHandler.GetComponent<GemArray>().getSpellTarget())
                {
                    gemHandler.GetComponent<GemArray>().playerTargetedSpell(new int[] { (int)System.Math.Round(this.transform.position.x), (int)System.Math.Round(this.transform.position.y) });
                    return;
                }

                int x = (int)System.Math.Round(transform.position.x);
                int y = (int)System.Math.Round(transform.position.y);

                if (selectedGem[0] == x && selectedGem[1] == y)
                {
                    selectedGem[0] = -10;
                    selectedGem[1] = -10;
                    selectedGemObject = null;
                    selector.GetComponent<Transform>().position = new Vector3(selectedGem[0], selectedGem[1], 0);
                    return;
                }

                if ((x == selectedGem[0] && System.Math.Abs(y - selectedGem[1]) == 1) || (y == selectedGem[1] && System.Math.Abs(x - selectedGem[0]) == 1))
                {
                    //TODO animate gem swap
                    GameObject tempGameObject = this.gameObject;
                    StartCoroutine(gemHandler.GetComponent<GemArray>().SwapGems(new int[] { selectedGem[0], selectedGem[1] }, new int[] { x, y }, selectedGemObject, tempGameObject));



                    selectedGem[0] = -10;
                    selectedGem[1] = -10;
                    selectedGemObject = null;
                }
                else
                {
                    selectedGem[0] = x;
                    selectedGem[1] = y;
                    selectedGemObject = this.gameObject;
                }
                selector.GetComponent<Transform>().position = new Vector3(selectedGem[0], selectedGem[1], 0);
            }
            

            





        }

    }


}
