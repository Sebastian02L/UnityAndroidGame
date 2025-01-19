using UnityEngine;

public class PreviewAnimation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _displacementSpeed = 5f;
    [SerializeField] private float _maxOffset = 3f;
    private float initYPos;

    private void Start()
    {
        initYPos = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);

        float offset = Mathf.Sin(Time.time * _displacementSpeed) * _maxOffset;
        transform.position = new Vector3(transform.position.x, initYPos + offset, transform.position.z);
    }
}
