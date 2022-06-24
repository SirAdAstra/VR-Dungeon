using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private ConfigurableJoint[] joints;
    [SerializeField] private float health;
    [SerializeField] private ActiveRagdoll.JointDriveConfig jointDriveConfig;
    [SerializeField] private Animator animator;

    private enum State {Idle, Walk, Fall, Fight, Dead}
    private State state;

    private bool canAttack = true;
    private bool gettingUp = false;
    
    private void Start()
    {
        joints = gameObject.GetComponentsInChildren<ConfigurableJoint>();
        textMesh.text = health.ToString();
    }

    private void Update()
    {
        Execute();
        FallCheck();
    }

    private void ChangeState(State newState)
    {
        ExitState();
        state = newState;
    }

    private void Execute()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Walk:
                break;

            case State.Fall:
                if (gettingUp == false)
                    StartCoroutine(GetUp());
                break;
            
            case State.Fight:
                if (canAttack == true)
                    StartCoroutine(Attack());
                break;

            case State.Dead:
                break;
        }
    }

    private void ExitState()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Walk:
                animator.SetBool("moving", false);
                break;

            case State.Fall:
                animator.SetBool("fall", false);
                break;
            
            case State.Fight:
                break;

            case State.Dead:
                break;
        }
    }

    private void FallCheck()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 10f);
        if (hit.distance < 0.6f && hit.transform.tag == "floor" && state != State.Fall)
            ChangeState(State.Fall);
        else if (hit.distance > 0.6f && hit.transform.tag == "floor" && state == State.Fall)
            ChangeState(State.Idle);
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        int attackNumber = Random.Range(1, 2);
        animator.SetInteger("attack", attackNumber);
        yield return new WaitForSeconds(3f);
        animator.SetInteger("attack", 0);
        canAttack = true;
    }

    private IEnumerator GetUp()
    {
        gettingUp = true;
        animator.SetBool("fall", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("fall", false);
        gettingUp = false;
    }

    public void GetDamage(float incomigDamage)
    {
        health -= incomigDamage;
        textMesh.text = health.ToString();

        if (health <= 0)
        {
            var jointDrive = (JointDrive) jointDriveConfig;
            foreach (ConfigurableJoint joint in joints)
                {
                    joint.angularXDrive = joint.angularYZDrive = jointDrive;
                }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ChangeState(State.Fight);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            ChangeState(State.Idle);
    }
}
