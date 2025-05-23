using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPizza : MonoBehaviour
{
    [SerializeField] variant[] variants;
    
    [Serializable]
    public struct variant
    {
        public GameObject gameObject;
        public slice[] slices;
    }

    [Serializable]
    public struct slice
    {
        public Image image;
        public TMP_Text number;
    }
    
    void OnDestroy()
    {
        PlayerManager.PlayerAddedAfter -= Temp;
        PlayerManager.PlayerRemovedAfter -= Refresh;
    }

    void Awake()
    {
        PlayerManager.PlayerAddedAfter += Temp;
        PlayerManager.PlayerRemovedAfter += Refresh;
    }

    void Temp(PlayerManager.Player obj)
    {
        Refresh();
    }
    
    void Refresh()
    {
        variants[0].gameObject.SetActive(false);
        variants[1].gameObject.SetActive(false);
        variants[2].gameObject.SetActive(false);
        variants[3].gameObject.SetActive(false);

        if (PlayerManager.Players.Count <= 1)//
        {
            variants[0].gameObject.SetActive(true);

            if (PlayerManager.Players.Count == 1)
            {
                variants[0].slices[0].image.color = PlayerManager.Players[0].ChosenPlayerEntrySet.color;
                variants[0].slices[0].number.text = "1";
            }
            else
            {
                variants[0].slices[0].image.color = Color.grey;
                variants[0].slices[0].number.text = "0";
            }
        }
        else if (PlayerManager.Players.Count == 2)
        {
            variants[1].gameObject.SetActive(true);

            for (int i = 0; i < 2; i++)
            {
                variants[1].slices[i].image.color = PlayerManager.Players[i].ChosenPlayerEntrySet.color;
                variants[1].slices[i].number.text = (i + 1).ToString();
            }
        }
        else if (PlayerManager.Players.Count == 3)
        {
            variants[2].gameObject.SetActive(true);
                
            for (int i = 0; i < 3; i++)
            {
                variants[2].slices[i].image.color = PlayerManager.Players[i].ChosenPlayerEntrySet.color;
                variants[2].slices[i].number.text = (i + 1).ToString();
            }
        }
        else if (PlayerManager.Players.Count == 4)
        {
            variants[3].gameObject.SetActive(true);
             
            for (int i = 0; i < 4; i++)
            {
                variants[3].slices[i].image.color = PlayerManager.Players[i].ChosenPlayerEntrySet.color;
                variants[3].slices[i].number.text = (i + 1).ToString();
            }
        }
    }
}
