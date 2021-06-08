using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour
{

    [SerializeField]
    private float m_FallSpeed = 0f;

    private Rigidbody2D Rigidbody;

    public bool IsGliding = false;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (IsGliding && Rigidbody.velocity.y < 0f && Mathf.Abs(Rigidbody.velocity.y) > m_FallSpeed)
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Mathf.Sign(Rigidbody.velocity.y) * m_FallSpeed);
    }

    public void StartGliding()
    {
        IsGliding = true;
    }

    public void StopGliding()
    {
        IsGliding = false;
    }
}

