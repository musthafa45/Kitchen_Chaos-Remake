using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress 
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnProgressCanceled;
    public class OnProgressChangedEventArgs
    {
        public float ProgressNormalized;
    }
}
