using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;
    public float velMagnitude;
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] bool chainWindow;
    public bool attacking;
    public bool dodging;
    public int stage;

    int light_1 = Animator.StringToHash("LightAttack_1");
    int light_2 = Animator.StringToHash("LightAttack_2");
    int light_3 = Animator.StringToHash("LightAttack_3");

    int heavy_1 = Animator.StringToHash("HeavyAttack_1");
    int heavy_2 = Animator.StringToHash("HeavyAttack_2");

    int[] lightAttacks;
    int[] heavyAttacks;

    private void Awake()
    {
        chainWindow = false;
        attacking = false;
        stage = 0;

        lightAttacks = new int[] { light_1, light_2, light_3 };
        heavyAttacks = new int[] { heavy_1, heavy_2 };
    }

    void Update()
    {
        if (!dodging)
        {
            horizontalInput = Input.GetAxisRaw("JoyHorizontal");
            verticalInput = Input.GetAxisRaw("JoyVertical");
        }
        
        animator.SetFloat("VelMagnitude", velMagnitude);
        animator.SetFloat("HorizontalInput", horizontalInput);
        animator.SetFloat("VerticalInput", verticalInput);
        animator.SetBool("Dodging", dodging);

        
    }

    IEnumerator LightCombo()
    {
        StopCoroutine("OpenChainWindow");
        attacking = true;
        chainWindow = false;

        animator.Play(lightAttacks[stage]);

        /*if (stage < 2)
        {
            stage++;
        }
        else
        {
            stage = 0;
            controller.SendMessage("Thrust", 250f, SendMessageOptions.RequireReceiver);
        }*/
        
        switch (stage)
        {
            case 0:
                stage++;
                yield return new WaitForSeconds(0.5f);
                break;
            case 1:
                stage++;
                yield return new WaitForSeconds(0.4f);
                break;
            case 2:
                stage = 0;
                controller.SendMessage("Thrust", 250f, SendMessageOptions.RequireReceiver);
                yield return new WaitForSeconds(1.5f);
                break;
        }

        StartCoroutine(OpenChainWindow(0.5f));
        attacking = !attacking;        
    }

    IEnumerator OpenChainWindow(float time)
    {
        chainWindow = true;
        yield return new WaitForSeconds(time);
        chainWindow = false;
        if (!attacking)
            stage = 0;
    }
}
