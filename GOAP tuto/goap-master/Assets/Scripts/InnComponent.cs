using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnComponent : MonoBehaviour {

    [SerializeField]
    Text FoodText;

    [SerializeField]
    Text MeatText;

    [SerializeField]
    private int numMeat;

    [SerializeField]
    private int numFood;

    public int NumMeat
    {
        get
        {
            return numMeat;
        }

        set
        {
            numMeat = value;
            MeatText.text = "Meat: " + numMeat;
        }
    }

    public int NumFood
    {
        get
        {
            return numFood;
        }

        set
        {
            numFood = value;
            FoodText.text = "Food: " + numFood;
        }
    }

    private void Start()
    {
        NumMeat = numMeat;
        NumFood = numFood;
    }

   
}
