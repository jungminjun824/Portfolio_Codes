using UnityEngine;

// [ExecuteInEditMode] : 이 속성은 스크립트가 에디터 모드에서도 실행되도록 합니다.
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    // parallaxFactor : 패럴랙스 효과의 강도를 조절하는 값입니다.
    public float parallaxFactor;

    // Move : 레이어를 이동시키는 메서드입니다.
    public void Move(float delta)
    {
        // 현재 위치를 기반으로 새로운 위치를 계산합니다.
        Vector3 newPos = transform.localPosition;
        // x 위치를 delta와 parallaxFactor를 곱한 값만큼 이동시킵니다.
        newPos.x -= delta * parallaxFactor;

        // 새로운 위치를 적용합니다.
        transform.localPosition = newPos;
    }
}
