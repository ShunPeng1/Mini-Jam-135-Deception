using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Obstacle;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightActivatable : Activatable
{
    [SerializeField, Range(2,20)] private int _rayCount = 5;  // Number of rays to cast
    [SerializeField] private LayerMask _hitLayerMask, _triggerLayerMask;
    private PolygonCollider2D _polygonCollider2D;
    private Light2D _light2D;
    // Start is called before the first frame update
    void Start()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _light2D = GetComponent<Light2D>();
        InitializeTrigger();
        Inactive();
    }

    private void InitializeTrigger()
    {
        float range = _light2D.pointLightOuterRadius;
        float angle = _light2D.pointLightInnerAngle;
        float angleStep = angle / _rayCount;
        float currentAngle = -angle / 2f;

        Vector2[] points = new Vector2[_rayCount + 2];
        Vector2 originPosition = transform.position;
        

        for (int i = 0; i <= _rayCount; i++)
        {
            Vector2 direction = Quaternion.Euler(0f, 0f, currentAngle) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(originPosition, direction, range, _hitLayerMask);
            

            if (hit.collider != null)
            {
                points[i] = (originPosition - hit.point);
            }
            else
            {
                points[i] = direction.normalized * _light2D.pointLightOuterRadius ;
            }

            currentAngle += angleStep;
        }

        points[_rayCount + 1] = Vector2.zero; // triangle head
        _polygonCollider2D.points = points;
    }

    protected override void Active()
    {
        gameObject.SetActive(true);
    }

    protected override void Inactive()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggerLayerMask == (_triggerLayerMask | (1 << other.gameObject.layer)))
        {
            Debug.Log("Kill Player");
        }
    }
}
