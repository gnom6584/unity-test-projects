using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float DownSpeed = 10.0f;

    public bool IsDestroyed { private set; get; }

    public void Destroy()
    {
        IsDestroyed = true;
        Destroy(GetComponent<BoxCollider>());
    }

    protected virtual void Update()
    {
        if (IsDestroyed)
        {
            if (transform.localPosition.z < 0.9f)           
                transform.localPosition += DownSpeed * Time.deltaTime * Vector3.forward;
            
            else
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.9f);
        }
    }
}

