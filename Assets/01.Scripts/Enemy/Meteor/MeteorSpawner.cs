using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _meteorPrefab;
    [SerializeField]
    GameObject _shake;
    [SerializeField]
    LayerMask _player;
    GameObject obj;

    void Start()
    {
        obj = Instantiate(_meteorPrefab, new Vector3(transform.position.x + 3, 20), Quaternion.identity);
        obj.transform.SetParent(transform);
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position,obj.transform.position) <= 0.0001f)
        {
            GameObject obj = Instantiate(_shake);
            Destroy(obj , 2f);
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 1.25f, _player);
            if (coll != null)
            {
                UIManager.Instance.BeAttack(40);
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1.25f);
    }
}
