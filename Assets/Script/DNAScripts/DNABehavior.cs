using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNABehavior : MonoBehaviour
{
    public DNA DNAModel;

    public virtual void SetUp(DNA _DNAModel)
    {
        DNAModel = _DNAModel;
        this.gameObject.GetComponent<DNAUI>().SetUp(_DNAModel);
    }

    public virtual void OnAcquire()
    {

    }
}
