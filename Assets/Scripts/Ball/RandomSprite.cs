using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum ItemName
{
    tomato,
    egg,
    kiwi,
    peach,
    bonbon,
    donut,
    cheese,
    milk,
    pepermint
}

public class RandomSprite : MonoBehaviour
{
    [Serializable]
    struct Item
    {
        public ItemName name;
        public Texture[] phases;
    }

    [SerializeField]
    List<Item> itemSprites;
    Phases phasesManager;

    public static ItemName currentItem;

    void RandomItem()
    {
        if (itemSprites.Count == 0)
        {
            SceneManager.LoadScene("Lose");
            return;
        }
        var i = Random.Range(0, itemSprites.Count);
        Item chosenItem = itemSprites[i];
        itemSprites.RemoveAt(i);
        phasesManager.materials[0].SetTexture("_Phase2", chosenItem.phases[0]);
        phasesManager.materials[0].SetTexture("_Phase1", chosenItem.phases[1]);
        phasesManager.materials[1].SetTexture("_Phase1", chosenItem.phases[1]);
        phasesManager.materials[1].SetTexture("_Phase2", chosenItem.phases[2]);
        phasesManager.shard.SetTexture("_MainTex", chosenItem.phases[3]);
        Ball.reference.GetComponent<ParticleSystemRenderer>().material = phasesManager.shard;
        currentItem = chosenItem.name;
        ProgressBar.reference.SetBar(currentItem);
    }

    void Start()
    {
        Ball.reference.Enable.AddListener(RandomItem);
        phasesManager = GetComponent<Phases>();
        RandomItem();
    }
}
