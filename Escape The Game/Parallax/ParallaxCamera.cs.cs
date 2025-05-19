using UnityEngine;

// [ExecuteInEditMode] : 이 속성은 스크립트가 에디터 모드에서도 실행되도록 합니다.
[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    // ParallaxCameraDelegate : 카메라 이동을 처리하는 델리게이트를 정의합니다.
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    // onCameraTranslate : 카메라 이동 이벤트를 위한 델리게이트 인스턴스입니다.
    public ParallaxCameraDelegate onCameraTranslate;

    // oldPosition : 카메라의 이전 위치를 저장합니다.
    private float oldPosition;

    // Start : 게임이 시작될 때 호출됩니다.
    void Start()
    {
        // 카메라의 초기 위치를 저장합니다.
        oldPosition = transform.position.x;
    }

    // Update : 매 프레임마다 호출됩니다.
    void Update()
    {
        // 카메라의 x 위치가 이전 위치와 다르면
        if (transform.position.x != oldPosition)
        {
            // onCameraTranslate 델리게이트가 설정되어 있다면 호출합니다.
            if (onCameraTranslate != null)
            {
                // 이전 위치와 현재 위치의 차이를 계산하여 델리게이트에 전달합니다.
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            // 현재 위치를 이전 위치로 업데이트합니다.
            oldPosition = transform.position.x;
        }
    }
}
