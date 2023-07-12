using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    float rand;
    Vector3 parentPos;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rand = Random.Range(12, 16f);
        parentPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, parentPos, rand * Time.deltaTime);
        if(Vector3.Distance(transform.position,parentPos) <= 0.001f)
        {
            _animator.SetTrigger("Break");
        }
    }

    public void DestroyMeteor()
    {
        Destroy(gameObject);
    }
}
