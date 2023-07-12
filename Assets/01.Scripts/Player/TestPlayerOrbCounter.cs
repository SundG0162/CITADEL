using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Linq;

[Serializable]
public class TestOrbSet
{
    public int OrbIndex;
    public Vector3 InitPos;
    public GameObject OrbInstance;

    public Tween GoToInitPos()
    {
        return OrbInstance.transform.DOMove(InitPos, 0.7f).SetEase(Ease.InOutCubic);
    }
}

[Serializable]
public class TestPlayerOrbCounter : MonoBehaviour
{
    Vector3 forward = new Vector3(0, 0, 120);
    Vector3 back = new Vector3(0, 0, -120);

    Sequence _orbSeq;
    Sequence _rotateSeq;

    [SerializeField]
    private GameObject _fireOrbPrefab; // 배치할 오브젝트 프리팹
    [SerializeField]
    private GameObject _lightningOrbPrefab;
    [SerializeField]
    private GameObject _lightningExplosion;
    [SerializeField]
    private GameObject _fireExplosion;
    [SerializeField]
    private int numberOfObjects; // 배치할 오브젝트 개수
    [SerializeField]
    private float radius = 3f; // 원의 반지름

    public float rotationSpeed = 180f; // 회전 속도
    [SerializeField]
    public LayerMask isEnemy;
    [SerializeField]
    private float _speedUpGauge;
    [SerializeField]
    EnemySO _enemySO;
    Transform dir;

    bool fire = true;
    bool lightning = false;

    [SerializeField]
    private List<TestOrbSet> _orbList = new List<TestOrbSet>();

    private void Start()
    {
        OrbSequence(3, true); //초기에 3개 만들고 시작
    }

    private void Update()
    {
        if (!GameManager.Instance.isStarting || !GameManager.Instance.isAdding) return;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            RotationSpeedDown();
            lightning = false;
            fire = true;
            AddOrb();

        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            RotationSpeedDown();
            fire = false;
            lightning = true;
            AddOrb();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RotationSpeedUp();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            RotationSpeedDown();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            OrbLaunch();
        }
    }


    private void OrbLaunch()
    {
        if (numberOfObjects == 1) return;
        numberOfObjects--;
        List<Collider2D> coll = Physics2D.OverlapCircleAll(transform.position, 10, isEnemy).ToList();
        Collider2D col = coll[0];
        foreach (Collider2D c in coll)
        {
            if (Vector3.Distance(col.transform.position, transform.position) > Vector3.Distance(c.transform.position, transform.position))
                col = c;
        }
        transform.GetChild(1).GetComponent<PlayerOrb>().Launch(col.transform.position);
        transform.GetChild(1).SetParent(null);
        _orbList.RemoveAt(0);
        for (int j = 0; j < _orbList.Count; j++)
        {
            _orbList[j].OrbIndex = j;
        }
    }

    private void RotationSpeedUp()
    {
        if (_rotateSeq.IsActive() && _rotateSeq != null)
        {
            _rotateSeq.Kill();
        }
        Tween faster = DOTween.To(
            getter: () => rotationSpeed,
            setter: value => rotationSpeed = value,
            endValue: 540f, duration: 0.5f
            ).SetEase(Ease.InQuint);
        Tween lighter = DOTween.To(
            getter: () => transform.GetChild(0).GetComponent<Light2D>().intensity,
            setter: value => transform.GetChild(0).GetComponent<Light2D>().intensity = value,
            endValue: 0.3f, duration: 0.5f
            );
        _rotateSeq = DOTween.Sequence();
        _rotateSeq.Append(faster);
        _rotateSeq.Join(lighter);
    }

    public void RotationSpeedDown()
    {
        if (_rotateSeq.IsActive() && _rotateSeq != null)
        {
            _rotateSeq.Kill();
        }
        Tween slower = DOTween.To(
            getter: () => rotationSpeed,
            setter: value => rotationSpeed = value,
            endValue: 180f, duration: 0.5f
            ).SetEase(Ease.OutSine);
        Tween darker = DOTween.To(
            getter: () => transform.GetChild(0).GetComponent<Light2D>().intensity,
            setter: value => transform.GetChild(0).GetComponent<Light2D>().intensity = value,
            endValue: 0f, duration: 0.5f
            );
        _rotateSeq = DOTween.Sequence();
        _rotateSeq.Append(slower);
        _rotateSeq.Join(darker);
    }

    private void OrbSequence(int count = 1, bool isStartGame = false)
    {
        _orbSeq = DOTween.Sequence();
        _orbSeq.AppendInterval(0.5f);
        for (int i = 0; i < count; i++)
            OrbSpawn();
        SetOrbPosition(); //각 오브별 초기 위치 설정
        _orbSeq.Append(_orbList[0].GoToInitPos());
        for (int i = 1; i < _orbList.Count; i++)
        {
            _orbSeq.Join(_orbList[i].GoToInitPos());
        }
        _orbSeq.AppendInterval(0.5f);
        if (lightning)
        {
            GameObject obj = Instantiate(_lightningExplosion, transform.position, Quaternion.identity);
            Destroy(obj, 2f);
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 4f, isEnemy);
            foreach (Collider2D col in colls)
            {
                if (col.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.LightningDamage(0, 80f);
                }
            }
        }
        if (fire)
        {
            GameObject obj = Instantiate(_fireExplosion, transform.position, Quaternion.identity);
            Destroy(obj, 2f);
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 4f, isEnemy);
            foreach (Collider2D col in colls)
            {
                if (col.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.LightningDamage(0, 60f);
                }
            }
        }
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        if (isStartGame) //게임 시작시라면 시작 돌리고
            _orbSeq.AppendCallback(() => GameManager.Instance.StartGame());

        _orbSeq.AppendCallback(() => GameManager.Instance.MoveStart());
    }

    private void SetOrbPosition()
    {

        foreach (TestOrbSet os in _orbList)
        {
            os.InitPos = GetOrbInitPosition(os.OrbIndex); //각 오브별 초기위치 셋팅
        }
    }

    //index번째 오브의 좌표를 알아내는 함수
    private Vector3 GetOrbInitPosition(int index)
    {
        float angleStep = 360f / numberOfObjects; // 오브젝트 간의 각도 간격
        float angle = index * angleStep;
        float x = transform.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = transform.position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        return new Vector3(x, y, 0f);
    }

    private void AddOrb()
    {
        GameManager.Instance.isAdding = false;
        //정지 시퀀스 시작
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(back, 1f).SetEase(Ease.OutSine));

        seq.Append(transform.DORotate(forward * 2, 1f).SetEase(Ease.InCubic));
        for (int i = 0; i < numberOfObjects; i++)
        {
            seq.Join(transform.GetChild(i + 1).DOMove(transform.position, 0.5f).SetEase(Ease.InCubic));
        }
        //정지시퀀스 끝나면 생성하고 시퀀스 시작
        seq.OnComplete(() =>
        {
            OrbSequence(1); //한개 더 추가해서 시작
        });
    }

    private void OrbSpawn()
    {
        if (fire)
        {
            GameObject obj = Instantiate(_fireOrbPrefab, transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            _orbList.Add(new TestOrbSet { OrbIndex = numberOfObjects, InitPos = Vector3.zero, OrbInstance = obj });
            numberOfObjects++;
        }
        else if (lightning)
        {
            GameObject obj = Instantiate(_lightningOrbPrefab, transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            _orbList.Add(new TestOrbSet { OrbIndex = numberOfObjects, InitPos = Vector3.zero, OrbInstance = obj });
            numberOfObjects++;
        }
    }
}