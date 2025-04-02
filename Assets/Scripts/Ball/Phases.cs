using Unity.VisualScripting;
using UnityEngine;

public class Phases : MonoBehaviour
{
    public Material[] materials = new Material[2];
    [Range(0, 1)]
    public float durability;
    SpriteRenderer spriteRenderer;
    Ball ball;

    public float phase1amount = 1f;
    public float phase2amount = 1f;
    private void Start()
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
            phase1amount = 0.5f + (0.5f * durability);
            spriteRenderer.material.SetFloat("_DamageAmount", phase1amount);
        }
        else
        {
            spriteRenderer.material = materials[1];
            phase2amount = (2 * durability);
            spriteRenderer.material.SetFloat("_DamageAmount", phase2amount);
        }
    }

    public void ResetPhases()
    {
        spriteRenderer.material = materials[0];
    }
}
