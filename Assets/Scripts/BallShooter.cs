﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public Rigidbody ball;
    public Transform firePos; //볼이 생성되는 위치
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;
    public float minForce = 15f;
    public float maxForce = 30f;
    public float chargingTime = 0.75f;

    private float currentForce;
    private float chargeSpeed;
    private bool fired;

    private void OnEnable()
    {
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
    }

    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;
    }

    private void Update()
    {
        if (fired == true) return;
        powerSlider.value = minForce;
        if (currentForce >= maxForce && !fired) //강제 발사해야 하는 경우
        {
            currentForce = maxForce;
            // 강제로 발사 처리
            Fire();
        }
        else if (Input.GetButtonDown("Fire1")) //발사 버튼 누르기 시작
        {
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton("Fire1") && !fired) //발사 버튼 누르는 중
        {
            currentForce = currentForce + chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        else if (Input.GetButtonUp("Fire1") && !fired) //발사 버튼 뗌
        {
            // 발사 처리
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;

        Rigidbody ballInstance= Instantiate(ball,firePos.position,firePos.rotation);
        ballInstance.velocity = currentForce * firePos.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce;
    }
}
