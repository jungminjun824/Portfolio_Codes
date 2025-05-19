using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    float patternDelay;
    public Transform attackPos;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Entrance();
    }

    public void TakeDamage(int damage) //데미지 받는 함수 
    {
        StartCoroutine(HitColorAnimation());

        GameObject hudText = Instantiate(hudDamageTxet);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;

        CameraShake.Shake(0.2f, 3, 1);
    }

    private void Entrance() // 3라운드 들어왔을 때 보스의 자연스러운 이동
    {
        StartCoroutine(StartEntrance());

        IEnumerator StartEntrance()
        {
            rigid.velocity = new Vector3(-10f, 0, 0);
            yield return new WaitForSeconds(0.75f);
            rigid.velocity = new Vector3(0, 0, 0);

            Pattern();
        }

    }

    private void Pattern() //랜덤으로 패턴 실행
    {

        StartCoroutine(StartPattern());
        IEnumerator StartPattern()
        {
            int patternNum = Random.Range(0, 3);
            Coroutine coroutine = null;
            switch (patternNum)
            {
                case 0: coroutine = Pattern1(); break;
                case 1: coroutine = Pattern2(); break;
                case 2: coroutine = Pattern3(); break;
                default: Pattern1(); break;
            }
            yield return coroutine;

            yield return new WaitForSeconds(patternDelay);
            StartCoroutine(StartPattern());
        }
    }

    private Coroutine Pattern1() //패턴 1
    {
        transform.DOMoveY(9, 0.5f).SetEase(Ease.InQuad).OnComplete(() => transform.DORotate(new Vector3(0, 0, 40), 0.5f));
        Coroutine coroutine = StartCoroutine(StartPattern());
        return coroutine;

        IEnumerator StartPattern()
        {
            yield return new WaitForSeconds(2f);
            int shootCount = 15;
            float Delay = 0.25f;
            for (int i = 0; i < shootCount; i++)
            {
                if (i % 2 == 0)
                {
                    var bullet1 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet2 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet3 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();

                    bullet1.Init(new Vector3(-1, 0, 0), 20);
                    bullet2.Init(new Vector3(-1, -0.5f, 0), 20);
                    bullet3.Init(new Vector3(-1, -1, 0), 20);
                }
                else
                {
                    var bullet1 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet2 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet3 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();

                    bullet1.Init(new Vector3(-1, -0.25f, 0), 20);
                    bullet2.Init(new Vector3(-1, -0.75f, 0), 20);
                    bullet3.Init(new Vector3(-1, -1.25f, 0), 20);
                }
                yield return new WaitForSeconds(Delay);
            }
            transform.DORotate(new Vector3(0, 0, 0), 0.3f);
            transform.DOMove(new Vector3(-39, 4f, 0), 0.3f);
        }
    }

    private Coroutine Pattern2() // 패턴 2
    {
        transform.DOMoveX(-50, 0.5f).OnComplete(() => transform.DOMoveY(18, 0.5f).OnComplete(() => transform.DORotate(new Vector3(0, 0, 90), 0.3f)));

        Coroutine coroutine = StartCoroutine(StartPattern());
        return coroutine;

        IEnumerator StartPattern()
        {
            yield return new WaitForSeconds(2f);
            int shootCount = 15;
            float Delay = 0.1f;
            for (int i = 0; i < shootCount; i++)
            {
                if (i % 2 == 0)
                {
                    var bullet1 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet2 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet3 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet4 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet5 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();

                    bullet1.Init(new Vector3(-1f, -1, 0), 20);
                    bullet2.Init(new Vector3(-0.5f, -1, 0), 20);
                    bullet3.Init(new Vector3(0, -1, 0), 20);
                    bullet4.Init(new Vector3(0.5f, -1, 0), 20);
                    bullet5.Init(new Vector3(1, -1, 0), 20);
                }
                else
                {
                    var bullet1 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet2 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet3 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet4 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                    var bullet5 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();

                    bullet1.Init(new Vector3(0.5f, -1f, 0), 20);
                    bullet2.Init(new Vector3(0.25f, -1f, 0), 20);
                    bullet3.Init(new Vector3(0, -1f, 0), 20);
                    bullet4.Init(new Vector3(-0.25f, -1f, 0), 20);
                    bullet5.Init(new Vector3(-0.5f, -1f, 0), 20);
                }
                yield return new WaitForSeconds(Delay);
            }
            transform.DORotate(new Vector3(0, 0, 0), 0.3f);
            transform.DOMove(new Vector3(-39, 4f, 0), 0.3f);
        }
    }

    private Coroutine Pattern3() //패턴 3
    {
        Coroutine coroutine = StartCoroutine(StartPattern());
        return coroutine;

        IEnumerator StartPattern()
        {
            Vector3 position1 = new Vector3(-40, 6, 0);
            Vector3 position2 = new Vector3(-40, 1, 0);
            Vector3 position3 = new Vector3(-40, -3.5f, 0);

            Vector3 currentPos;

            yield return new WaitForSeconds(2f);
            int shootCount = 30;
            float Delay = 0.15f;
            for (int i = 0; i < shootCount; i++)
            {
                int value = Random.Range(0, 3);
                switch (value)
                {
                    case 0: currentPos = position1; break;
                    case 1: currentPos = position2; break;
                    case 2: currentPos = position3; break;

                    default: currentPos = position1; break;
                }
                transform.position = currentPos;
                var bullet1 = Instantiate(enemyBullet, attackPos.position, Quaternion.identity).GetComponent<BulletMovement>();
                bullet1.Init(new Vector3(-1, 0, 0), 20);
                yield return new WaitForSeconds(Delay);
            }
        }
    }
}
