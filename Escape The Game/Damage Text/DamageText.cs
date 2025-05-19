using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ���� ������ �� �������� ���� ������ �ߴ� �ؽ�Ʈ ��ũ��Ʈ
public class DamageText : MonoBehaviour
{
    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;
    public int damage;
    TextMeshPro text;

    Color alpha;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
    }

    private void Start()
    {
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    private void Update()
    {
        // �ؽ�Ʈ�� ���� �̵���Ŵ
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime));

        // �ؽ�Ʈ�� ������ ������ ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);

        // �ؽ�Ʈ�� ������ ������Ʈ
        text.color = alpha;
    }

    // ������Ʈ�� �ı��ϴ� �Լ�
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
