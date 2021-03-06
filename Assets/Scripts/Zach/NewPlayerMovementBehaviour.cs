﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zach
{
    public class NewPlayerMovementBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _moveVector = Vector3.zero;
        private Vector3 _prevMoveVector;
        public Vector3 MoveVector
        {
            get { return _moveVector; }
        }
        public float speed = 5;
        private float baseSpeed;
        public float jumpPower = 4;
        public float gravity = 9.81f;
        public float mag;
        public float _prevMag;
        public Vector3 velocity;
        public Vector3 camRight;
        public Vector3 camForward;
        public bool isFrozen = false;
        public bool isGrounded;
        private Vector3 _prevPosition;
        private CharacterController _controller;
        private Animator _animator;
        

        private void Start()
        {
            baseSpeed = speed;
            _prevPosition = transform.position;
            _controller = GetComponent<CharacterController>();
            isGrounded = _controller.isGrounded;
            _animator = GetComponent<Animator>();
            mag = PlayerInput.InputVector.magnitude;
        }

        // Update is called once per frame
        private void Update()
        {
            speed = baseSpeed;
            mag = PlayerInput.InputVector.magnitude;
            if (mag > 0.20f)
            {
                if (_prevMag < PlayerInput.InputVector.magnitude)
                    _prevMag += Time.deltaTime;
            }
            else if (mag < 0.20f)
            {
                if (_prevMag > 0)
                {
                    _prevMag -= Time.deltaTime;
                }
            }
            if (!isFrozen)
            {
                
                var h = PlayerInput.InputVector.normalized.x;
                var v = PlayerInput.InputVector.normalized.z;
                var forward = Camera.main.transform.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward = forward.normalized;
                var right = new Vector3(forward.z, 0, -forward.x);
                var targetDir = h * right + v * forward;
                if (targetDir.magnitude > 0)
                {
                    var rot = Quaternion.Euler(targetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetDir),0.25f);
                }
                _moveVector.x = targetDir.x;
                _moveVector.z = targetDir.z;
                if (!_controller.isGrounded)
                {
                    _moveVector.y = _moveVector.y - (gravity * Time.deltaTime);
                }
                
                if (_controller.isGrounded && Input.GetButtonDown("Jump"))
                {
                    _moveVector.y = jumpPower;
                    Debug.Log("Test");
                    _animator.SetTrigger("OnJump");
                }
                speed *= _prevMag;
                _moveVector.x *= speed;
                _moveVector.z *= speed;
                _controller.Move(_moveVector * Time.deltaTime);
            }
            velocity = (transform.position - _prevPosition) / Time.deltaTime;
            _animator.SetFloat("Velocity",_controller.velocity.magnitude);
            _prevPosition = transform.position;
            isGrounded = _controller.isGrounded;
            _animator.SetBool("IsGrounded",isGrounded);
        }
    }
}