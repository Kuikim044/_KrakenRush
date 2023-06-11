using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public Transform rotationCenter; // �ѵ�ط���ͧ��������ع�ͺ
    public float rotationSpeed = 10f; // �������ǡ����ع
    public float movementRadius = 5f; // ����ա������͹���
    public float movementSpeed = 2f; // �������ǡ������͹���

    private float angle = 0f;

    private void Update()
    {
        // ��ع�ͺ�ѵ�ط���˹��᡹ Y
        transform.RotateAround(rotationCenter.position, Vector3.back, rotationSpeed * Time.deltaTime);

        // ����͹����ͺ����ѵ�ط���˹�
        angle += movementSpeed * Time.deltaTime;
        float x = Mathf.Sin(angle) * movementRadius;
        float y = Mathf.Cos(angle) * movementRadius;
        transform.position = rotationCenter.position + new Vector3(x, y, 0f);
    }
}
