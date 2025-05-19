using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 적을 때렸을 때 데미지가 몇이 들어갔는지 뜨는 텍스트 스크립트
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
        // 텍스트를 위로 이동시킴
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime));

        // 텍스트의 투명도를 서서히 줄임
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);

        // 텍스트의 색상을 업데이트
        text.color = alpha;
    }

    // 오브젝트를 파괴하는 함수
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
