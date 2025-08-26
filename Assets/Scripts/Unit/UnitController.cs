using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RaycastHit _raycastHit;

    private NavMeshAgent _selectUnit;
    private Vector3 _targetPosition;

    public void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out _raycastHit))
        {
            switch (_raycastHit.collider.gameObject.tag)
            {
                case "Player":
                    _selectUnit = _raycastHit.collider.gameObject.GetComponent<NavMeshAgent>();
                    break;
                case "Ground":
                    MoveTargetPosition(_raycastHit);
                    break;
            }
        }
    }

    /// <summary>
    /// クリックした場所に選択されているユニットを移動させる
    /// </summary>
    /// <param name="raycastHit"></param>
    private void MoveTargetPosition(RaycastHit raycastHit)
    {
        var distance = Vector3.Distance(_camera.gameObject.transform.position, raycastHit.point);
        var mousePosition = new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, distance);
        _targetPosition = _camera.ScreenToWorldPoint(mousePosition);

        if (_selectUnit != null)
        {
            _targetPosition.y = _selectUnit.transform.position.y;
            _selectUnit.SetDestination(_targetPosition);
        }
    }
}