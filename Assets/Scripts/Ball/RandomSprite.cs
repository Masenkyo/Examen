using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSprite : MonoBehaviour
{
    [Serializable]
    struct Item
    {
        public Texture[] phases;
    }
    
    [SerializeField]
    Item[] itemSprites;
    Phases phasesManager;
    void RandomItem()
    {
        Item chosenItem = itemSprites[Random.Range(0, itemSprites.Length)];
        phasesManager.materials[0].SetTexture("_Phase2", chosenItem.phases[0]);
        phasesManager.materials[0].SetTexture("_Phase1", chosenItem.phases[1]);
        phasesManager.materials[1].SetTexture("_Phase1", chosenItem.phases[1]);
        phasesManager.materials[1].SetTexture("_Phase2", chosenItem.phases[2]);
    }
    
    void Start()
    {
        Ball.reference.Enable.AddListener(RandomItem);
        phasesManager = GetComponent<Phases>();
        RandomItem();
    }
}
