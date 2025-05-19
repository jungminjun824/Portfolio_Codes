using UnityEngine;

// [ExecuteInEditMode] : �� �Ӽ��� ��ũ��Ʈ�� ������ ��忡���� ����ǵ��� �մϴ�.
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    // ParallaxCameraDelegate : ī�޶� �̵��� ó���ϴ� ��������Ʈ�� �����մϴ�.
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    // onCameraTranslate : ī�޶� �̵� �̺�Ʈ�� ���� ��������Ʈ �ν��Ͻ��Դϴ�.
    public ParallaxCameraDelegate onCameraTranslate;

    // oldPosition : ī�޶��� ���� ��ġ�� �����մϴ�.
    private float oldPosition;

    // Start : ������ ���۵� �� ȣ��˴ϴ�.
    void Start()
    {
        // ī�޶��� �ʱ� ��ġ�� �����մϴ�.
        oldPosition = transform.position.x;
    }

    // Update : �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // ī�޶��� x ��ġ�� ���� ��ġ�� �ٸ���
        if (transform.position.x != oldPosition)
        {
            // onCameraTranslate ��������Ʈ�� �����Ǿ� �ִٸ� ȣ���մϴ�.
            if (onCameraTranslate != null)
            {
                // ���� ��ġ�� ���� ��ġ�� ���̸� ����Ͽ� ��������Ʈ�� �����մϴ�.
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            // ���� ��ġ�� ���� ��ġ�� ������Ʈ�մϴ�.
            oldPosition = transform.position.x;
        }
    }
}
