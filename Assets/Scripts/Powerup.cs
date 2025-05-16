using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum Effects
{
    Random,
    Buff,
    Debuff,
    Heal,
    FullHeal,
    Float,
    Damage,
    Kill,
    RandomDurability,
    ColorBlind,
    Heavy,
    SlowControls
}

public class Powerup : MonoBehaviour
{
    public static List<Powerup> allPowerups = new List<Powerup>();
    UnityEvent collide = new UnityEvent();
    Ball ball = null;
    Action buff;
    Action debuff;
    Action used;
    Image grayscale;
    public Action OnCancel;

    [SerializeField]
    Effects currentEffect;
    Dictionary<Effects, Action> powerupsDict = new Dictionary<Effects, Action>();

    void OnDestroy()
    {
        allPowerups.Remove(this);
    }

    void Start()
    {
        OnCancel += () => StopAllCoroutines();

        grayscale = Settings.reference.postprocessing;

        buff += Heal;
        buff += FullHeal;
        buff += Float;
        debuff += Damage;
        debuff += Kill;
        debuff += RandomDurability;
        debuff += ColorBlind;
        debuff += Heavy;
        debuff += SlowControls;

        powerupsDict.Add(Effects.Heal, Heal);
        powerupsDict.Add(Effects.RandomDurability, RandomDurability);
        powerupsDict.Add(Effects.FullHeal, FullHeal);
        powerupsDict.Add(Effects.Damage, Damage);
        powerupsDict.Add(Effects.Kill, Kill);
        powerupsDict.Add(Effects.Float, Float);
        powerupsDict.Add(Effects.Heavy, Heavy);
        powerupsDict.Add(Effects.SlowControls, SlowControls);
        powerupsDict.Add(Effects.ColorBlind, ColorBlind);

        SetPowerup();
        allPowerups.Add(this);
    }

    public void SetPowerup()
    {
        Enable();

        Color usedColor = Color.black;
        if (currentEffect == Effects.Random)
        {
            int random = Random.Range(0, 2);
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
        }
        else if (currentEffect != Effects.Debuff && (currentEffect == Effects.Buff || buff.GetInvocationList().ToList().Contains(powerupsDict[currentEffect])))
        {
            usedColor = Color.green;
        }
        else if (currentEffect == Effects.Debuff || debuff.GetInvocationList().ToList().Contains(powerupsDict[currentEffect]))
        {
            usedColor = Color.red;
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
        yield return new WaitForSeconds(3f);
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

    void InvokeOnce(Delegate a)
    {
        void Once()
        {
            a.DynamicInvoke();
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

        switch (currentEffect)
        {
            case Effects.Random:
                InvokeOnce(used.GetInvocationList()[Random.Range(0, used.GetInvocationList().Length - 1)]);
                break;
            case Effects.Buff:
                InvokeOnce(buff.GetInvocationList()[Random.Range(0, buff.GetInvocationList().Length - 1)]);
                break;
            case Effects.Debuff:
                InvokeOnce(debuff.GetInvocationList()[Random.Range(0, debuff.GetInvocationList().Length - 1)]);
                break;
            default:
                powerupsDict[currentEffect].Invoke();
                break;
        }
        Disable();
    }
}
