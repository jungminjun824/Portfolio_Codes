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
        // makeGhost 플래그가 true인지 확인
        if (makeGhost)
        {
            // ghostDelaySeconds가 아직 0보다 크면, 지난 프레임 이후 경과한 시간을 빼줌
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            // ghostDelaySeconds가 0 이하가 되면 유령 생성
            else
            {
                // 현재 위치와 회전값으로 유령 생성
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                // 현재 스프라이트를 가져옴
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;

                // 유령의 크기를 현재 오브젝트와 동일하게 설정
                currentGhost.transform.localScale = this.transform.localScale;
                // 유령의 스프라이트를 현재 스프라이트로 설정
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                // ghostDelaySeconds를 초기값으로 재설정
                ghostDelaySeconds = ghostDelay;

                // 1초 후에 유령 오브젝트 파괴
                Destroy(currentGhost, 1f);
            }
        }
    }

}
