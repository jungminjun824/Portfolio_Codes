using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode] : 이 속성은 스크립트가 에디터 모드에서도 실행되도록 합니다.
[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    // ParallaxCamera : 패럴랙스 효과를 제어하는 카메라를 참조합니다.
    public ParallaxCamera parallaxCamera;
    // parallaxLayers : 패럴랙스 레이어들을 저장하는 리스트입니다.
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // Start : 게임이 시작될 때 호출됩니다.
    void Start()
    {
        // 패럴랙스 카메라가 설정되지 않았다면, 메인 카메라에서 ParallaxCamera 컴포넌트를 찾습니다.
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // 패럴랙스 카메라가 설정되어 있다면, 카메라 이동 이벤트에 Move 메서드를 추가합니다.
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // 레이어들을 설정합니다.
        SetLayers();
    }

    // SetLayers : 패럴랙스 레이어들을 설정하는 메서드입니다.
    void SetLayers()
    {
        // 기존 레이어 리스트를 초기화합니다.
        parallaxLayers.Clear();

        // 자식 객체들을 순회하며 ParallaxLayer 컴포넌트를 찾습니다.
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // ParallaxLayer 컴포넌트가 있다면, 리스트에 추가하고 이름을 설정합니다.
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    // Move : 패럴랙스 레이어들을 이동시키는 메서드입니다.
    void Move(float delta)
    {
        // 각 레이어를 순회하며 이동 메서드를 호출합니다.
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
