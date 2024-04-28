using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    
    private bool CheckSameSprite(SpriteRenderer _spriteRenderer, Sprite _sprite)
    {
        if (_spriteRenderer.sprite == _sprite)
            return true;
        else
            return false;
    }

    public IEnumerator SpriteChangeCoroutine(Transform target, string spriteName)
    {
        SpriteRenderer[] _spriteRenderer = target.GetComponentsInChildren<SpriteRenderer>();
        Sprite _sprite = Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;

        if (!CheckSameSprite(_spriteRenderer[0], _sprite))
        {
            Color _frontColor = _spriteRenderer[0].color;
            Color _backColor = _spriteRenderer[1].color;
            _frontColor.a = 0;
            _backColor.a = 0;
            _spriteRenderer[0].color = _frontColor;
            _spriteRenderer[1].color = _backColor;

            _spriteRenderer[0].sprite = _sprite;
            _spriteRenderer[1].sprite = _sprite;
            
            while(_frontColor.a < 1)
            {
                _frontColor.a += fadeSpeed;
                _backColor.a += fadeSpeed;
                _spriteRenderer[0].color = _frontColor;
                _spriteRenderer[1].color = _backColor;
                yield return null;
            }
        }
    }
}
