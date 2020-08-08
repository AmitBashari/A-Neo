using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int PlayerDamage;
    public int WallDamage = 1;
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

    private Animator _animator;
    private Transform _target;
    private bool _skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag ("Player").transform;
        base.Start();
    }


    protected override void AttemptMove<T>(int xDir, int yDir)
    {

        if (_skipMove)
        {
            _skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        _skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs (_target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = _target.position.y > transform.position.y ? 1 : -1;
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            xDir = _target.position.x > transform.position.x ? 1 : -1;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f); 
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        _animator.SetTrigger("EnemyAttack");
        SoundManager.Instance.RanomizeSfx(EnemyAttack1, EnemyAttack2);

        hitPlayer.LoseFood(PlayerDamage);

    }

    protected override void EnemyHitWall<T>(T component)
    {
    
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("EnemyAttack");
    }

}
