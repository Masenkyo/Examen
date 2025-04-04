using Unity.VisualScripting;
using UnityEngine;

public class Phases : MonoBehaviour
{
    public Material[] materials = new Material[2];
    [Range(0, 1)]
    public float durability;
    SpriteRenderer spriteRenderer;
    Ball ball;

    private void Start()
    {
        ball = GetComponent<Ball>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = materials[0];
    }

    void Update()
    {
        durability = ball.Durability / ball.getMaxDurability;
        if(durability > 0.5f)
        {
            spriteRenderer.material.SetFloat("_DamageAmount", 0.5f + (0.5f * durability));
        }
        else
        {
            spriteRenderer.material = materials[1];
            spriteRenderer.material.SetFloat("_DamageAmount", 2 * durability);
        }
    }

    public void ResetPhases()
    {
        spriteRenderer.material = materials[0];
    }
}
