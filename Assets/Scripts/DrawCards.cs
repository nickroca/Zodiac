using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject SummonCard;
    public GameObject Card2;
    public GameObject PlayerHandArea;

    public void OnClick() 
    {
        for(int i = 0; i < 5; i++) 
        {
            GameObject card = Instantiate(SummonCard, new Vector2(0,0), Quaternion.identity);
            card.transform.SetParent(PlayerHandArea.transform, false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
