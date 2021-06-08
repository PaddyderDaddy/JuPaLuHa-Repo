using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform OrbitingObject;
    public Ellipse OrbitPath;

    [Range(0f, 1f)]
    public float OrbitProgress = 0f;
    public float OrbitPeriod = 3f;
    public bool OrbitActive = true;

    // Start is called before the first frame update
    void Start()
    {
        if (OrbitingObject == null)
        {
            OrbitActive = false;
            return;
        }
        SetOrbitingObjectPosition();
        StartCoroutine(AnimateOrbit());
    }

    void SetOrbitingObjectPosition()
    {
        Vector2 orbitPos = OrbitPath.Evaluate(OrbitProgress);
        OrbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }

    IEnumerator AnimateOrbit()
    {
        if (OrbitPeriod < 0.1f)
        {
            OrbitPeriod = 0.1f;
        }
        float orbitSpeed = 1f / OrbitPeriod;
        while (OrbitActive)
        {
            OrbitProgress += Time.deltaTime * orbitSpeed;
            OrbitProgress %= 1f;
            SetOrbitingObjectPosition();
            yield return null;
        }
    }
}
