using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky2D : MonoBehaviour
{
    public float speed = 1.5f; // ������������ ��������
    public float acceleration = 100; // ���������
    public float contactDistance = 1.25f; // ��������� � ������� ���������� ��������� � ������������
    public float gravityForce = 100; // �������������� ����

    private int layerMask;
    private Rigidbody2D body;
    private float h;
    private Vector3 direction;
    private Vector3 gravity;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;

        // ����� ���������� ����� ���� ����� �������, � ��������� ���, ���� ������������ ����������� Ignore Raycast
        // ��� ��������� � ������ �������� � �������� �� ������ ���� ��������������

        // �����, ��������� ���� ����� ��������, ������� ����������� ���� Ignore Raycast
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;

        // ��������� �����������
        direction = Vector3.down;
        gravity = Vector3.down;

        GetDirections(direction, Mathf.Infinity);
    }

    void GetDirections(Vector3 currentDirection, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, distance, layerMask);
        if (hit.collider)
        {
            SetNormal(hit.normal);
        }

        Scanner();
    }

    void SetNormal(Vector3 normal)
    {
        gravity = -normal.normalized; // ������ ���������� � ���������
        direction = Vector3.Cross(normal, Vector3.forward).normalized; // ������ ����������� ���������
    }

    void Scanner()
    {
        // ������� ���� �� ������ �� ��� �������
        // ��������� ������� � ��������� �������� � ��������� �� ���
        int arr = 6;
        float j = 0;
        float[] distance = new float[arr];
        Vector2[] normal = new Vector2[arr];

        for (int i = 0; i < arr; i++)
        {
            var x = Mathf.Sin(j);
            var y = Mathf.Cos(j);

            j += 360 * Mathf.Deg2Rad / arr;

            Vector3 dir = transform.TransformDirection(new Vector3(x, y, 0));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMask);
            if (hit.collider)
            {
                distance[i] = hit.distance;
                normal[i] = hit.normal;
                Debug.DrawLine(transform.position, hit.point, Color.cyan);
            }
            else
            {
                distance[i] = Mathf.Infinity;
            }
        }

        float min = Mathf.Min(distance); // ���������� ���������� ��������� �� �������

        // ���������� ������, ���� ����������� ��������� ������
        // ��� ��������� ���������� (������� ����) - ������ ���������
        for (int i = 0; i < arr; i++)
        {
            if (distance[i] == min && min > contactDistance)
            {
                SetNormal(normal[i]);
            }
        }
    }

    void LateUpdate()
    {
        h = Input.GetAxis("Horizontal");

        GetDirections(direction * h, contactDistance);

        Debug.DrawRay(transform.position, direction * h, Color.red);
    }

    void FixedUpdate()
    {
        body.AddForce(gravity * gravityForce * body.mass); // ��������� ����������

        body.AddForce(direction * h * speed * acceleration * body.mass); // ��������� ������ ��������

        // ����������� ��������, ����� ������ ����� ��������� ����������
        if (Mathf.Abs(body.velocity.x) > speed)
        {
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);
        }

        if (Mathf.Abs(body.velocity.y) > speed)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * speed);
        }
    }
}