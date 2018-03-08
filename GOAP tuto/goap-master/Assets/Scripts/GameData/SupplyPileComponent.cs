using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SupplyPileComponent : MonoBehaviour
{
    [SerializeField]
    Text ToolsText;

    [SerializeField]
    Text OresText;

    [SerializeField]
    Text LogsText;

    [SerializeField]
    Text FirewoodText;


    [SerializeField]
    private int numTools; // for mining ore and chopping logs

    [SerializeField]
    private int numLogs; // makes firewood

    [SerializeField]
    private int numFirewood; // what we want to make

    [SerializeField]
    private int numOre; // makes tools


    private void Start()
    {
        NumTools = numTools;
        NumLogs = numLogs;
        NumFirewood = numFirewood;
        NumOre = numOre;
    }

    public int NumTools
    {
        get
        {
            return numTools;
        }

        set
        {
            numTools = value;
            ToolsText.text = "Tools: " + numTools.ToString();
        }
    }

    public int NumLogs
    {
        get
        {
            return numLogs;
        }

        set
        {
            numLogs = value;
            LogsText.text = "Logs: " + numLogs.ToString();
        }
    }

    public int NumFirewood
    {
        get
        {
            return numFirewood;
        }

        set
        {
            numFirewood = value;
            FirewoodText.text = "Firewood: " + numFirewood.ToString();
        }
    }

    public int NumOre
    {
        get
        {
            return numOre;
        }

        set
        {
            numOre = value;
            OresText.text = "Ores: " + numOre.ToString();
        }
    }
}

