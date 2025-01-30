using UnityEngine;

public class parallax : MonoBehaviour
{
    Material mat;                // Reference to the material of the object
    float distance;              // Distance for the texture offset
    [Range(-0.5f, 1f)]
    public float speed = 0.2f;   // Speed of the texture scrolling, can be adjusted in Unity's Inspector

    void Start()
    {
        // Get the material of the object to which this script is attached
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Increment the distance by the speed multiplied by time
        distance += Time.deltaTime * speed;

        // Apply the updated texture offset along the X-axis
        mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));
    }
}
