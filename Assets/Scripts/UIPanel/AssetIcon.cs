using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssetIcon : MonoBehaviour
{
    private Vector3 _targetPoint;
    private Vector3 _direction;
    private float _moveSpeed;
    private float _rotLerp = 0.5f;

    
    public void Init(Vector3 targetPoint, float rotLerp, float moveSpeed)
    {
        _targetPoint = targetPoint;
        _rotLerp = rotLerp;
        _moveSpeed = moveSpeed;
        _direction = Quaternion.AngleAxis(Random.Range(-120,120),Vector3.forward)* ( _targetPoint-transform.position).normalized;
        Destroy(gameObject,1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _targetPoint) < 100
        || transform.position.y<_targetPoint.y)//为了防止英魂图标从地下出去而加的判断
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetDir = ( _targetPoint-transform.position).normalized;
        _direction = Vector3.Lerp(_direction, targetDir, _rotLerp).normalized;
        transform.position += _direction * _moveSpeed*Time.deltaTime;
    }
}
