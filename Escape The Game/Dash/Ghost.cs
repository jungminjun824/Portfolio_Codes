using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject ghost;
    public float ghostDelay;
    private float ghostDelaySeconds;
    public bool makeGhost = false;

    private void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }
    private void Update()
    {
        // makeGhost �÷��װ� true���� Ȯ��
        if (makeGhost)
        {
            // ghostDelaySeconds�� ���� 0���� ũ��, ���� ������ ���� ����� �ð��� ����
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            // ghostDelaySeconds�� 0 ���ϰ� �Ǹ� ���� ����
            else
            {
                // ���� ��ġ�� ȸ�������� ���� ����
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                // ���� ��������Ʈ�� ������
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;

                // ������ ũ�⸦ ���� ������Ʈ�� �����ϰ� ����
                currentGhost.transform.localScale = this.transform.localScale;
                // ������ ��������Ʈ�� ���� ��������Ʈ�� ����
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                // ghostDelaySeconds�� �ʱⰪ���� �缳��
                ghostDelaySeconds = ghostDelay;

                // 1�� �Ŀ� ���� ������Ʈ �ı�
                Destroy(currentGhost, 1f);
            }
        }
    }

}
