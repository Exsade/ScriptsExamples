using System.Collections.Generic;
using UnityEngine;

//This is script from test task. The requirements for the script were:
//  Tap on 'W' start playing animation "Run" in BlendTree
//  If no key is pressed- play "Idle" animation
//  Mouse click randomly starts one of the attack animations
//  Tap on 'D' must dissable animator and all rigidbodies components of character

public class CaracterView : MonoBehaviour
{
    [SerializeField] private float BlendDuration = 0.3f;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Rigidbody> _rigidbodies;

    private const string AttackClaw = "Attack_Claw";
    private const string AttackSpecial = "Attack_Special";
    private const string Blend = "Blend";

    private int _random;

    void Start()
    {
        _animator.gameObject.SetActive(true);

        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _random = Random.Range(1, 3);

            if (_random == 1)
                _animator.SetTrigger(AttackClaw);
            if (_random == 2)
                _animator.SetTrigger(AttackSpecial);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (Rigidbody rigidbody in _rigidbodies)
                rigidbody.isKinematic = false;

            _animator.enabled = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _animator.SetFloat(Blend, 1f, BlendDuration, Time.deltaTime);
        }

        else
            _animator.SetFloat(Blend, 0f, BlendDuration, Time.deltaTime);
    }
}
