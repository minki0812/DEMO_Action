using UnityEngine;

public class Axe : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 1000 * Time.deltaTime);
    }
}
