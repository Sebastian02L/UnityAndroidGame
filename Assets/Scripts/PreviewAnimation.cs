using UnityEngine;

public class PreviewAnimation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _displacementSpeed = 5f;
    [SerializeField] private float _maxOffset = 3f;
    private Vector3 _startingPos;

    private void Awake()
    {
        _startingPos = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);

        float offset = Mathf.Sin(Time.time * _displacementSpeed) * _maxOffset;
        this.transform.localPosition = _startingPos + new Vector3(0, offset, 0);
    }
}
