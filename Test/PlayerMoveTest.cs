using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private Rigidbody2D _rb;
    //private CharacterRenderer characterRenderer;
    private IDisposable inputDisposable;

    // Start is called before the first frame update
    void Start()
    {
        inputDisposable = this.UpdateAsObservable()
            .Subscribe(_ => Move());
    }

    private void Move()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
        {
            _rb.velocity = new Vector2(x, y).normalized * _moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        inputDisposable.Dispose();
    }
}
