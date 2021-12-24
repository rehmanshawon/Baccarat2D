using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFace : MonoBehaviour
{
    public Image Card;
    public Sprite[] spade;
    public Sprite[] diamond;
    public Sprite[] heart;
    public Sprite[] club;
    
    // variable to hold a reference to our SpriteRenderer component
    // Link in the Inspector
    //[SerializeField] SpriteRenderer spriteRenderer;

   private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
        // get a reference to the SpriteRenderer component on this gameObject
        int face = 0;
        switch (s)
        {
            case "pc1":
                 face = CardFace.GetPlayerCard1();
                break;
            case "pc2":
                face = CardFace.GetPlayerCard2();
                break;
            case "pc3":
                face = CardFace.GetPlayerCard3();
                break;
            case "bc1":
                face = CardFace.GetBankerCard1();
                break;
            case "bc2":
                face = CardFace.GetBankerCard2();
                break;
            case "bc3":
                face = CardFace.GetBankerCard3();
                break;
            default:
                face = 0;
                break;
        }
        
        
        Card.sprite = face>100 && face <200? club[face%100]
            :face>200 && face<300?diamond[face%200]
            :face>300 && face<400?heart[face%300]
            :face>400?spade[face%400]
            :null;
        //Debug.Log(CardFace.GetFace());

    }
}
