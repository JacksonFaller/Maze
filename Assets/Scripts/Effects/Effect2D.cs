using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    class Effect2D : EffectBase
    {
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Collected(other.gameObject);
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
        }
    }
}
