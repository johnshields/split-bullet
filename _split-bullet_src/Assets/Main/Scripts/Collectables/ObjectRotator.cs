using UnityEngine;

/*
 * ObjectRotator
 * Script to rotate objects.
*/
public class ObjectRotator : MonoBehaviour
{
    public float x, y, z;

    private void Update()
    {
        transform.Rotate(new Vector3(x, y, z));
    }
}