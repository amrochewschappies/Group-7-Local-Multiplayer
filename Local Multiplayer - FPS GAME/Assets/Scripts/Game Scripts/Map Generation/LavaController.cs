using UnityEngine;

public class LavaController : MonoBehaviour
{
    public GameObject LavaPlane;

    private Vector3 TargetPosition;

    public float MoveSpeed;


    void Start()
    {
        // You could initialize things here if needed
    }

    void Update()
    {

        TargetPosition = new Vector3(LavaPlane.transform.position.x, LavaPlane.transform.position.y + 800, LavaPlane.transform.position.z);
        LavaPlane.transform.position = Vector3.MoveTowards(LavaPlane.transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
    }
}
