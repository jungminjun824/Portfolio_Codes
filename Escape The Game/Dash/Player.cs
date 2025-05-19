

IEnumerator CorDash()
{
    canDash = false;
    isDashing = true;
    ghost.makeGhost = true;
    float currentGravity = rigid.gravityScale;

    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"));
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"));

    rigid.gravityScale = 0;
    rigid.velocity = new Vector2(dir * dashPower, 0f);

    yield return new WaitForSeconds(dashingTime);

    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyBullet"), false);

    rigid.gravityScale = currentGravity;
    isDashing = false;
    ghost.makeGhost = false;

    yield return new WaitForSeconds(dashingCooldown);

    canDash = true;
}