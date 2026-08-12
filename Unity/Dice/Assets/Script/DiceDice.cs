using UnityEngine;

public class DiceDice : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        //向前上方扔
        Vector3 force = new Vector3(0, 5f, 3f);
        rb.AddForce(force,ForceMode.Impulse);

        //随机旋转
        Vector3 torque = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
            );

        rb.AddTorque(torque, ForceMode.Impulse);
    }
}
