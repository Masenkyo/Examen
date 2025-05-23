using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Flipper : MonoBehaviour
{
    // Broken Flipper variables
    [SerializeField] float clickGoal = 10;
    [SerializeField] int chanceOfBreaking = 20;
    [SerializeField] float repairDifficulty = 3;
    [SerializeField] GameObject sliderBar;
    GameObject currentSliderbar;
    float progress = 0;
    bool doOnce;
    
    // Movement variables
    public int rotateSpeed = 90;
    [HideInInspector] public float DesiredHorizontalMovement;
    float rotation;
    
    [HideInInspector] public Rigidbody2D rigidbody;
    protected Collider2D collider;
    
    // The list of all the flippers
    public static HashSet<Flipper> AllFlippers = new();
    
    #region FlipperList
    
    // Adding the flippers to the list and giving the rigidbody variable a rigidbody of the flipper
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        AllFlippers.Add(this);
        
        // setup repairing/breaking
        OnDurabilityChanged += (oldVal, newVal) =>
        {
            // got repaired
            if (oldVal < 1f && newVal == 1f)
            {
                progress = 0;
                doOnce = false;
                Destroy(currentSliderbar);
                Destroy(createdRepairPrompt);
                currentSliderbar = null;
            }

            // got broken
            if (newVal < 1f && oldVal == 1f)
            {
                currentSliderbar = Instantiate(sliderBar, transform.position + new Vector3(0,1,0), new Quaternion(0,0,0,0), transform.parent);
                createdRepairPrompt = Instantiate(RepairPrompt, transform.position, Quaternion.identity);
            }
            
            // reflect
            if (newVal < 1f)
                currentSliderbar.GetComponentsInChildren<Image>().First(_ => _.sprite != null).fillAmount = newVal / 1f;
        };
    }
    
    // Remove everything in the list becaues it's static
    void OnDestroy() => AllFlippers.Remove(this);

    #endregion

    protected virtual void FixedUpdate() => InputRotations();
    
    protected virtual void Update()
    {
        ActiveFlippers();
        
        if (rigidbody.simulated && Follow.reference.enabled && !doOnce)
            RngBreakFlipper(max: chanceOfBreaking); 
            
        if (Durability < 1f)
            RepairProgression();
    } 

    // The rotation system
    void InputRotations() => rigidbody.angularVelocity = DesiredHorizontalMovement < 0 && Durability == 1f
        ? rotateSpeed * -DesiredHorizontalMovement
        : DesiredHorizontalMovement > 0 && Durability == 1f
            ? -rotateSpeed * DesiredHorizontalMovement
            : 0;
    
    // Creates the chance that a flipper wil break
    void RngBreakFlipper(int min = 0, int max = 20)
    {
        if (Random.Range(min, max) != 0)
        {
            doOnce = true;
            return;
        }

        Durability = 0f;
        doOnce = true;
    }
    
    // Activate flippers that are in radius
    void ActiveFlippers()
    {
        float lowLimit = Camera.main.ScreenToWorldPoint(new Vector2(0, -Screen.height)).y;
        float highLimit = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1;
        
        rigidbody.simulated = transform.position.y >= lowLimit && transform.position.y <= highLimit;
        
        if (Durability < 1f && transform.position.y > highLimit || transform.position.y < lowLimit)
        {
            Durability = 1f;
            doOnce = false;
            Destroy(currentSliderbar);
            currentSliderbar = null;
        }

        if (!rigidbody.simulated || Durability < 1f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    GameObject createdRepairPrompt;
    public GameObject RepairPrompt;

    protected float Durability
    {
        get => _durability;
        set
        {
            value = Mathf.Clamp(value, 0f, 1f);
            OnDurabilityChanged(_durability, value);
            _durability = value;
        }
    } float _durability = 1f;
    public Action<float, float> OnDurabilityChanged = (oldVal, newVal) => { };
    
    // Fix the flippers that have been broken
    void RepairProgression()
    {
        if (progress > 0)
            progress -= Time.deltaTime * repairDifficulty * PlayerManager.Players.Count;
        
        if (Gamepad.all.Any(_ => _.buttonSouth.wasPressedThisFrame) || Input.GetKeyDown(KeyCode.B))
            progress += 1;

        Durability = progress / clickGoal;
    }
}