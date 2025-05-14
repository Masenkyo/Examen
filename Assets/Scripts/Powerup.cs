using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Powerup : MonoBehaviour
{
    public static List<Powerup> allPowerups = new List<Powerup>();
    UnityEvent collide = new UnityEvent();
    Ball ball = null;
    List<Action> buff = new List<Action>();
    List<Action> debuff = new List<Action>();
    List<Action> used;
    Image grayscale;
    public float waitTime;

    void Start()
    {
        grayscale = Settings.reference.postprocessing;

        buff.Add(Heal);
        debuff.Add(Kill);
        debuff.Add(Damage);
        buff.Add(FullHeal);
        buff.Add(Float);
        debuff.Add(RandomDurability);
        debuff.Add(ColorBlind);
        debuff.Add(Heavy);
        debuff.Add(SlowControls);

        SetPowerup();
        allPowerups.Add(this);
    }

    public void SetPowerup()
    {
        Enable();

        int random = Random.Range(0, 2);
        Color usedColor;
        if(random < 1)
        {
            usedColor = Color.green;
            used = buff;
        }
        else
        {
            usedColor = Color.red;
            used = debuff;
        }
        GetComponent<SpriteRenderer>().color = usedColor;
    }

    void Heal()
    {
        Debug.Log("heal");
        const int healingAmount = 10;
        ball.Durability += healingAmount;
    }

    void RandomDurability()
    {
        Debug.Log("random");
        float randomDurability = Random.Range(0.01f, ball.getMaxDurability);
        ball.Durability = randomDurability;
    }

    void FullHeal()
    {
        ball.Durability = ball.getMaxDurability;
    }

    void Damage()
    {
        const int damageAmount = 10;
        ball.Durability -= damageAmount;
    }

    void Kill()
    {
        ball.Durability -= ball.Durability;
    }

    IEnumerator gravityChange(float scale)
    {
        ball.rigidBody.gravityScale *= scale;
        yield return new WaitForSeconds(10f);
        ball.rigidBody.gravityScale /= scale;
    }

    void Float()
    {
        StartCoroutine(gravityChange(0.1f));
    }

    void Heavy()
    {
        StartCoroutine(gravityChange(3));
    }

    void SlowControls()
    {
        IEnumerator slowcontrols()
        {
            foreach(var flipper in Flipper.AllFlippers)
            {
                flipper.rotateSpeed = 20;
            }
            yield return new WaitForSeconds(3);
            foreach(var flipper in Flipper.AllFlippers)
            {
                flipper.rotateSpeed = 45;
            }
        }

        StartCoroutine(slowcontrols());
    }

    void ColorBlind()
    {
        IEnumerator colorblind()
        {
            grayscale.gameObject.SetActive(true);
            yield return new WaitForSeconds(10);
            grayscale.gameObject.SetActive(false);
        }
        StartCoroutine(colorblind());
    }

    void InvokeOnce(Action a)
    {
        void Once()
        {
            a.Invoke();
            collide.RemoveListener(Once);
        }

        collide.AddListener(Once);
        collide.Invoke();
    }

    void Disable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    void Enable()
    {
        
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Ball>(out Ball b)) return;
        ball = b;
        InvokeOnce(used[Random.Range(0, used.Count -1)]);
        Disable();
    }
}
