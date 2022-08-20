using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /*
     * RyderAnimations
     * Script that controls the Ryder animations.
     * ref - https://youtu.be/WIl6ysorTE0
     */
    public class RyderAnimations : MonoBehaviour
    {
        private InputControls _controls;
        private Rigidbody _rb;
        private Animator _animator;
        private int _profile, _jump;
        
        private void Awake()
        {
            _controls = new InputControls();
        }


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            
            _profile = Animator.StringToHash("Profile");
            _jump = Animator.StringToHash("JumpActive");
        }
    }
}