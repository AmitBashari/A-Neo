using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DmgSprite;
    public int HP = 4;
    public AudioClip ChopSound1;
    public AudioClip ChopSound2;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall (int loss)
    {
        SoundManager.Instance.RanomizeSfx(ChopSound1, ChopSound2);
        _spriteRenderer.sprite = DmgSprite;
        HP -= loss;
        if (HP <= 0)
            gameObject.SetActive(false);
    }


}
