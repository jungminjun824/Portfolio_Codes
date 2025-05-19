using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode] : �� �Ӽ��� ��ũ��Ʈ�� ������ ��忡���� ����ǵ��� �մϴ�.
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    // ParallaxCamera : �з����� ȿ���� �����ϴ� ī�޶� �����մϴ�.
    public ParallaxCamera parallaxCamera;
    // parallaxLayers : �з����� ���̾���� �����ϴ� ����Ʈ�Դϴ�.
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // Start : ������ ���۵� �� ȣ��˴ϴ�.
    void Start()
    {
        // �з����� ī�޶� �������� �ʾҴٸ�, ���� ī�޶󿡼� ParallaxCamera ������Ʈ�� ã���ϴ�.
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // �з����� ī�޶� �����Ǿ� �ִٸ�, ī�޶� �̵� �̺�Ʈ�� Move �޼��带 �߰��մϴ�.
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // ���̾���� �����մϴ�.
        SetLayers();
    }

    // SetLayers : �з����� ���̾���� �����ϴ� �޼����Դϴ�.
    void SetLayers()
    {
        // ���� ���̾� ����Ʈ�� �ʱ�ȭ�մϴ�.
        parallaxLayers.Clear();

        // �ڽ� ��ü���� ��ȸ�ϸ� ParallaxLayer ������Ʈ�� ã���ϴ�.
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // ParallaxLayer ������Ʈ�� �ִٸ�, ����Ʈ�� �߰��ϰ� �̸��� �����մϴ�.
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    // Move : �з����� ���̾���� �̵���Ű�� �޼����Դϴ�.
    void Move(float delta)
    {
        // �� ���̾ ��ȸ�ϸ� �̵� �޼��带 ȣ���մϴ�.
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
