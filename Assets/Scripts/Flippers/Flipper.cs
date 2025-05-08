using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Flipper : MonoBehaviour
{
    // Broken Flipper variables
    public bool brokenFlipper;
    [SerializeField] float clickGoal = 10;
    [SerializeField] int chanceOfBreaking = 20;
    [SerializeField] float repairDifficulty = 3;
    [SerializeField] GameObject sliderBar;
    GameObject currentSliderbar;
    float time = 0;
    bool doOnce;
    
    // Movement variables
    [SerializeField] int rotateSpeed = 90;
    [HideInInspector] public bool doubleSpeedPressed;
    [HideInInspector] public float DesiredHorizontalMovement;
    float rotation;
    
    [HideInInspector] public Rigidbody2D rigidbody;
    
    // The list of all the flippers
    public static HashSet<Flipper> AllFlippers = new();
    
    #region FlipperList
    
    // Adding the flippers to the list and giving the rigidbody variable a rigidbody of the flipper
    protected virtual void Awake()
    {
        Debug.Log(gameObject.name);
        rigidbody = GetComponent<Rigidbody2D>();
        AllFlippers.Add(this);
    }
    
    // Remove everything in the list becaues it's static
    void OnDestroy() => AllFlippers.Remove(this);

    #endregion

    void FixedUpdate() => InputRotations();
    
    protected virtual void Update()
    {
        ActiveFlippers();
        
        if (rigidbody.simulated && Follow.reference.enabled && !doOnce)
            BrokenFlipper(max: chanceOfBreaking); 
            
        if (brokenFlipper)
            FixBrokenFlipper(clickGoal);
    } 

    // The rotation system
    void InputRotations() => rigidbody.angularVelocity = DesiredHorizontalMovement < 0 && !brokenFlipper
        ? rotateSpeed * -DesiredHorizontalMovement
        : DesiredHorizontalMovement > 0 && !brokenFlipper
            ? -rotateSpeed * DesiredHorizontalMovement
            : 0;
    
    void BrokenFlipper(int min = 0, int max = 20)
    {
        if (UnityEngine.Random.Range(min, max) != 0)
        {
            doOnce = true;
            return;
        }
    
        brokenFlipper = true;
        doOnce = true;
        
        currentSliderbar = Instantiate(sliderBar, transform.position + new Vector3(0,1,0), new Quaternion(0,0,0,0), transform.parent);
    }
    
    void ActiveFlippers()
    {
        float lowLimit = Camera.main.ScreenToWorldPoint(new Vector2(0, -Screen.height / 3)).y;
        float highLimit = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1;
        
        rigidbody.simulated = transform.position.y >= lowLimit && transform.position.y <= highLimit;
        
        if (brokenFlipper && transform.position.y > highLimit || transform.position.y < lowLimit)
        {
            brokenFlipper = false;
            doOnce = false;
            Destroy(currentSliderbar);
            currentSliderbar = null;
        }
        
        if (!rigidbody.simulated || brokenFlipper)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    void FixBrokenFlipper(float clickingGoal)
    {
        if (time > 0)
            time -= Time.deltaTime * repairDifficulty * PlayerManager.Players.Count;
        
        if (Gamepad.all.Any(_ => _.buttonSouth.wasPressedThisFrame))
            time += 1;
        
        if (currentSliderbar.GetComponentsInChildren<Image>().First(_ => _.sprite) is var image)
            image.fillAmount = time / clickingGoal;
        
        if (time > clickingGoal)
        {
            brokenFlipper = false;
            time = 0;
            doOnce = false;
            Destroy(currentSliderbar);
            currentSliderbar = null;
        }
    }
}