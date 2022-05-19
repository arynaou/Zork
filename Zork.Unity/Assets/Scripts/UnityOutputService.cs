using UnityEngine;
using Zork;
using TMPro;
using UnityEngine.UI;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    public void Write(string value)
    {
        wasWrite = true;
        OutputText.text += value;
    }

    public void Write(object value)
    {
        Write(value.ToString());
    }

    public void WriteLine(string value)
    {
        SetText(value);
    }

    public void WriteLine(object value)
    {
        WriteLine(value.ToString());
    }

    private void SetText(string value)
    {
        if (value != null)
        {
            if (wasWrite == true)
            {
                OutputText.text += value;
                wasWrite = false;
            }
            else
            {
                OutputText.text = value;
            }
            Instantiate(OutputText, TextContent.transform);
        }

        if (TextContent.transform.childCount > MaxText)
        {
            Destroy(TextContent.transform.GetChild(0).gameObject);
        }

        OutputText.text = "";
    }

    public void MoveOutput(int value)
    {
        MoveText.text = $"Move: {value}";
    }

    public void ScoreOutput(int value)
    {
        ScoreText.text = $"Score: {value}";
    }

    public void LocationOutput(string value)
    {
        LocationText.text = value;
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void BackgroundOutput(Player player)
    {
        Background.GetComponent<Image>().sprite = RoomBackground[player.Location.RoomNum];
    }

    public void ItemOutput(Player player)
    {
        foreach (GameObject placeholder in ItemPlaceHolder)
        {
            placeholder.SetActive(false);
        }

        for(int i=0; i<=player.Location.ItemList.Count-1; i++)
        {
            ItemPlaceHolder[i].gameObject.SetActive(true);
            ItemPlaceHolder[i].GetComponent<Image>().sprite = ItemList[player.Location.ItemList[i].ItemNum];
        }
    }

    public void EnemyOutput(Player player, float hitpoint, float maxHitpoint)
    {

        if (player.Location._Enemy.Name != null  && player.Location._Enemy.HitPoints>0)
        {
            EnemyPlaceHolder.SetActive(true);
            EnemyPlaceHolder.GetComponent<Image>().sprite = EnemyList[player.Location._Enemy.EnemyNum];
            HitPointImage.localScale = new Vector3(player.Location._Enemy.HitPoints / player.Location._Enemy.MaxHitPoints, 1, 1);
        }
        else
        {
            EnemyPlaceHolder.SetActive(false);
        }
    }

    public void AudioOutput(string state)
    {
        foreach (AudioClip audio in AudioList)
        {
            if (audio.name == state)
            {
                AudioSource.PlayClipAtPoint(audio, transform.position);
            }
        }
    }

    [SerializeField]
    private TextMeshProUGUI OutputText;

    [SerializeField]
    private GameObject TextContent;

    [SerializeField]
    private TextMeshProUGUI MoveText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField] 
    private int MaxText = 60;

    [SerializeField] 
    private GameObject Background;

    [NamedArrayAttribute (new string[] { "North of House" , "Clearing", "West of House", "Behind House" , "South of House" , "Forest 1" , "Forest 2" , "Forest 3" , "Up a Tree" })]
    [SerializeField] Sprite[] RoomBackground;

    [NamedArrayAttribute(new string[] { "Apple", "Banana", "Knife", "Rifle" })]
    [SerializeField] Sprite[] ItemList;

    [NamedArrayAttribute(new string[] { "Goblin","Kitten" })]
    [SerializeField] Sprite[] EnemyList;

    [NamedArrayAttribute(new string[] { "Move", "Eat", "Attack" })]
    [SerializeField] AudioClip[] AudioList;

    [SerializeField] 
    private GameObject[] ItemPlaceHolder;

    [SerializeField]
    private GameObject EnemyPlaceHolder;

    [SerializeField]
    private RectTransform HitPointImage;

    bool wasWrite = false;
}

