using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializedActObject
{
    public abstract void SaveProperties();
    
    public abstract void LoadProperties();
    
}
