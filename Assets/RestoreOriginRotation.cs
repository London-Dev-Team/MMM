using UnityEngine;

public class RestoreOriginRotation : MonoBehaviour
{
    private Quaternion lastParentRotation;

    void Start()
    {
        lastParentRotation = transform.parent.localRotation;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * 
                                  lastParentRotation * transform.localRotation;

        lastParentRotation = transform.parent.localRotation;
    }
}
