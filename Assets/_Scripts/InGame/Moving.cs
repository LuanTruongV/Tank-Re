using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.InGame
{
    public class Moving : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [FormerlySerializedAs("_defaultLayer")] [SerializeField] private LayerMask _layerCantMove;
        [SerializeField] private Animator _animator;
        private bool _isMoving;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");


        public void TryMove(Vector2 moveDir,float moveSpeed)
        {
            if (moveDir == Vector2.zero || moveDir.magnitude > 1)
            {
                return;
            }
            transform.up = moveDir;
            if (_isMoving || IsBalk(moveDir))
            {
                return;
            }
            StartCoroutine(MoveEnumerator(moveDir,moveSpeed));
        }

        private bool IsBalk(Vector2 direction)
        {
            return Physics2D.Raycast(transform.position, direction, 1f,
                _layerCantMove);
        }
        private IEnumerator MoveEnumerator(Vector2 vector2,float moveSpeed)
        {
            _isMoving = true;
            _animator.SetTrigger(IsMoving);
            var position = _rigidbody2D.position;
            transform.position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
            float moveFloat = 0;
            while (moveFloat < 1)
            {
                moveFloat += moveSpeed * Time.deltaTime;
                moveFloat = Mathf.Clamp(moveFloat, 0f, 1f);
                var endPos = position + vector2 * moveFloat;
                if (Mathf.Abs(moveFloat - 1f) == 0)
                {
                    endPos = new Vector2(Mathf.Round(endPos.x), Mathf.Round(endPos.y));
                }
                _rigidbody2D.MovePosition(endPos);
                yield return new WaitForFixedUpdate();
            }
            _isMoving = false;
        }
    }
}