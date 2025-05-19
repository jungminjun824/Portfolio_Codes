using UnityEngine;

// [ExecuteInEditMode] : �� �Ӽ��� ��ũ��Ʈ�� ������ ��忡���� ����ǵ��� �մϴ�.
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // parallaxFactor : �з����� ȿ���� ������ �����ϴ� ���Դϴ�.
    public float parallaxFactor;

    // Move : ���̾ �̵���Ű�� �޼����Դϴ�.
    public void Move(float delta)
    {
        // ���� ��ġ�� ������� ���ο� ��ġ�� ����մϴ�.
        Vector3 newPos = transform.localPosition;
        // x ��ġ�� delta�� parallaxFactor�� ���� ����ŭ �̵���ŵ�ϴ�.
        newPos.x -= delta * parallaxFactor;

        // ���ο� ��ġ�� �����մϴ�.
        transform.localPosition = newPos;
    }
}
