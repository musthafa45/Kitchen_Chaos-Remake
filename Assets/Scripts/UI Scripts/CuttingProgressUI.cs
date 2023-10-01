using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingProgressUI : MonoBehaviour
{
    [SerializeField] GameObject GameObjectHasProgress;
    private IHasProgress hasProgressCounter;
    [SerializeField] Image barImage;
    void Start()
    {
        if (!GameObjectHasProgress.TryGetComponent<IHasProgress>(out hasProgressCounter)) 
        { 
            Debug.LogError("There That Object Dont Have IHasProgress"); 
        }

        hasProgressCounter.OnProgressChanged += HasProgressCounter_OnProgressChanged;
        hasProgressCounter.OnProgressCanceled += HasProgressCounter_OnCutCanceled;

        barImage.fillAmount = 0;

        Hide();
    }

    private void HasProgressCounter_OnCutCanceled(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void HasProgressCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;
        if(e.ProgressNormalized == 0 || e.ProgressNormalized == 1) 
        { 
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        hasProgressCounter.OnProgressChanged -= HasProgressCounter_OnProgressChanged;
        hasProgressCounter.OnProgressCanceled -= HasProgressCounter_OnCutCanceled;
    }
}
