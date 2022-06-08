using UnityEngine;
using UnityEngine.UI;

public class MonitorTool : MonoBehaviour
{
    [SerializeField] private Text episodesText;
    [SerializeField] private Text succesText;
    [SerializeField] private Text failsText;

    private int episodesCount = -1;
    public int EpisodesCount
    {
        get { return episodesCount; }
        set { 
            episodesCount = value;
            episodesText.text = $"Episodes: {episodesCount}";
        }
    }

    private int succesCount;
    public int SuccesCount
    {
        get { return succesCount; }
        set
        {
            succesCount = value;
            succesText.text = $"Succes: {succesCount}";
        }
    }

    private int failsCount;
    public int FailsCount
    {
        get { return failsCount; }
        set
        {
            failsCount = value;
            failsText.text = $"Fails: {failsCount}";
        }
    }
}
