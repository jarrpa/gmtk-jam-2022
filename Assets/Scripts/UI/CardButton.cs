using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // on pointer enter the button is elevated 20 pixels
    public void OnPointerEnter()
    {
        Debug.Log("Pointer entered");
        // transform.position += new Vector2(0, 20);
    }

    // on pointer exit the button is lowered 20 pixels
    public void OnPointerExit()
    {
        Debug.Log("Pointer exited");
        // transform.position += new Vector2(0, -20);
    }
}
