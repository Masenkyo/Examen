using Unity.VisualScripting;
using UnityEngine;

public class Phases : MonoBehaviour
{
    public Material[] materials = new Material[2];
    [Range(0, 1)]
    public float durability;
    SpriteRenderer spriteRenderer;
    Ball ball;

    void Awake()
    {
        ball = GetComponent<Ball>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = materials[0];
    }

    void Update()
    {
        durability = ball.Durability / ball.getMaxDurability;
        Debug.Log(durability);
        if(durability > 0.5f)
        {

            float difference = 1 - durability;
            float relativeDifference = difference / 0.5f;

            spriteRenderer.material.SetFloat("_DamageAmount", relativeDifference);
        }
        else
        {
            float difference = 0.5f - durability;
            float relativeDifference = difference / 0.5f;

            spriteRenderer.material = materials[1];
            spriteRenderer.material.SetFloat("_DamageAmount", 2 * durability);
        }
        Debug.Log(durability);
    }

    public void ResetPhases()
    {
        spriteRenderer.material = materials[0];
    }
}
