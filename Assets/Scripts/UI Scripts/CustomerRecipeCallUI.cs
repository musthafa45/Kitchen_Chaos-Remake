using UnityEngine;


public class CustomerRecipeCallUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform template;
    [SerializeField] Transform ThanksUI;
    private RecipeSO _askedRecipeSO;

    private void Awake()
    {
        template.gameObject.SetActive(false);
    }
    private void Start()
    {
        ThanksUI.gameObject.SetActive(false);
        CallRecipeOnce();
    }
    
    public void EnableThanksUI()
    {
        container.gameObject.SetActive(false);
        ThanksUI.gameObject.SetActive(true);
        Destroy(ThanksUI.gameObject, 2);
    }

    private void CallRecipeOnce()
    {
        foreach (Transform child in container)  //Destroying container's Childs
        {
            if (child == template) continue;    // Clean up Templates Before Maded.
            Destroy(child.gameObject);
        }

        Transform templateTransform =  Instantiate(template,container);
        templateTransform.gameObject.SetActive(true);
        templateTransform.TryGetComponent(out CustomerRecipeCallSingleUI recipeCallSingleUI);
        //_askedRecipeSO = DeliveryManager.Instance.GetRandomRecipeSO();

        BaseCustomer baseCustomer = GetComponentInParent<BaseCustomer>();
        if (baseCustomer != null)
            _askedRecipeSO = baseCustomer.AskedRecipeSO;

        recipeCallSingleUI.SetTRecipe(_askedRecipeSO);

    }

}
