using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Tilemaps;

//�÷��̾ ��� ���� �����ϰ� ��� ������ �ع���Ű�� ������ ������ ������ �������� ��ũ��Ʈ
public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWatPlatfrom;
    [SerializeField] private BoxCollider2D playerCollider;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.DownArrow))
        {
            if(currentOneWatPlatfrom != null)
            {
                StartCoroutine(DisableCollision());
                StartCoroutine(DisableBoxCollision());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWatPlatfrom = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWatPlatfrom = null;
        }
    }
    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = currentOneWatPlatfrom.GetComponent<CompositeCollider2D>();
        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(0.35f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }

    private IEnumerator DisableBoxCollision()
    {
        BoxCollider2D platformCollider = currentOneWatPlatfrom.GetComponent<BoxCollider2D>();
        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(0.35f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}
