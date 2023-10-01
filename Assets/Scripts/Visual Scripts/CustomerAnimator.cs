using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CustomerAnimator : MonoBehaviour
{
    [SerializeField] private BaseCustomer baseCustomer;
    [SerializeField] private Rig rig;
    private readonly float rigTransitionTime = .4f;
    private const string ISMOVING = "IsMoving";
    //private const string ISGETHIT = "IsHit";
    private Animator animator;

    private void Start()
    {
        if(rig != null)
        {
            rig.weight = 0f;
            StartCoroutine(SetRigWeightTo1(rigTransitionTime));  // Test
        }
        
        baseCustomer.OnCustomerGetRecipe += BaseCustomer_OnCustomerGetRecipe;
    }
    private void OnDisable()
    {
        baseCustomer.OnCustomerGetRecipe -= BaseCustomer_OnCustomerGetRecipe;
    }
    private void BaseCustomer_OnCustomerGetRecipe(object sender, System.EventArgs e)
    {
        if(rig != null)
        {
            StartCoroutine(SetRigWeightTo1(rigTransitionTime));  // Call the coroutine to gradually set rig.weight to 1 over 10 seconds
        }

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool(ISMOVING, baseCustomer.IsMoving());
    }

    IEnumerator SetRigWeightTo1(float duration)
    {
        float elapsedTime = 0f;
        float startWeight = rig.weight;
        float targetWeight = 1f;

        while(elapsedTime < duration)
        {
            rig.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set correctly
        rig.weight = targetWeight;
    }

    //IEnumerator SetRigWeightTo0(float duration)  // For the Reset Position
    //{
    //    float elapsedTime = 0f;
    //    float startWeight = rig.weight;
    //    float targetWeight = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        rig.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure the final value is set correctly
    //    rig.weight = targetWeight;
    //}




}
