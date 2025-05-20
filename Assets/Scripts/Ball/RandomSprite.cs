using System;
using UnityEngine;
using UnityEngine.Rendering;
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
    Item[] itemSprites;
    Phases phasesManager;

    public static ItemName currentItem;

    void RandomItem()
    {
        Item chosenItem = itemSprites[Random.Range(0, itemSprites.Length)];
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
