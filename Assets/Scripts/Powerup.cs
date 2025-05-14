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
    public Action OnCancel;

    void Start()
    {
        OnCancel += () => StopAllCoroutines();

        grayscale = Settings.reference.postprocessing;

        buff.Add(Heal);
        buff.Add(FullHeal);
        buff.Add(Float);
        debuff.Add(Damage);
        debuff.Add(Kill);
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
        if (random < 1)
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
        const int healingAmount = 10;
        ball.Durability += healingAmount;
    }

    void RandomDurability()
    {
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
        void Once()
        {
            ball.rigidBody.gravityScale /= scale;
            OnCancel -= Once;
        }

        OnCancel += Once;

        ball.rigidBody.gravityScale *= scale;
        yield return new WaitForSeconds(10f);
        Once();
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
            void Once()
            {
                foreach (var flipper in Flipper.AllFlippers)
                {
                    flipper.rotateSpeed = 45;
                }
                OnCancel -= Once;
            }

            OnCancel += Once;

            foreach (var flipper in Flipper.AllFlippers)
            {
                flipper.rotateSpeed = 20;
            }
            yield return new WaitForSeconds(3);
            Once();
        }

        StartCoroutine(slowcontrols());
    }

    void ColorBlind()
    {
        IEnumerator colorblind()
        {
            void Once()
            {
                grayscale.gameObject.SetActive(false);
                OnCancel -= Once;
            }

            OnCancel += Once;

            grayscale.gameObject.SetActive(true);
            yield return new WaitForSeconds(10);
            Once();
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
        //InvokeOnce(used[Random.Range(0, used.Count -1)]);
        InvokeOnce(ColorBlind);
        Disable();
    }
}
