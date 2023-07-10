using UnityEngine;

public class Car : MonoBehaviour
{
    private CarScriptable car;
    private Rigidbody rb;
    private bool isInitialized = false;
    private Vector3 velocity;

    public void Initialize(CarScriptable car)
    {
        this.car = car;
        isInitialized = true;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (!isInitialized)
            return;

        //  velocity -= velocity * (1 - car.Grip / 100) * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.A))
            rb.angularVelocity -= car.TurnSpeed * Time.fixedDeltaTime * Vector3.up / 4;
        if (Input.GetKey(KeyCode.D))
            rb.angularVelocity += car.TurnSpeed * Time.fixedDeltaTime * Vector3.up / 4;
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(car.Acceleration * Time.fixedDeltaTime * transform.forward, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(car.Acceleration * Time.fixedDeltaTime * -transform.forward, ForceMode.VelocityChange);
    }
}