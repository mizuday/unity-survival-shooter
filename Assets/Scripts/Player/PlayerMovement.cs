﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigidbody;
    private int floorMask;
    private float camRayLength = 100f;
    private PlayerConfig playerConfig;

    private void Awake()
    {
        //mendapatkan nilai mask dari layer yang bernama Floor
        floorMask = LayerMask.GetMask("Floor");

        playerConfig = GetComponent<PlayerConfig>();

        //Mendapatkan komponen Animator
        anim = GetComponent<Animator>();

        //Mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        //Mendapatkan nilai input vertical (-1,0,1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    //Method player dapat berjalan
    public void Move(float h, float v)
    {
        //Set nilai x dan y
        movement.Set(h, 0f, v);

        //Menormalisasi nilai vector agar total panjang dari vector adalah 1
        movement = movement.normalized * playerConfig.moveSpeed * Time.deltaTime;

        //Move to position
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //Buat Ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Buat raycast untuk floorHit
        RaycastHit floorHit;

        //Lakukan raycast
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            //Mendapatkan vector daro posisi player dan posisi floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            //Mendapatkan look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //Rotasi player
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    public void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
