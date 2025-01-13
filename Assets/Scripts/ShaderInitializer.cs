using UnityEngine;

public class ShaderInitializer : MonoBehaviour
{
    [SerializeField] private Texture _texture;
    [SerializeField] private Color _colorMult;
    [SerializeField] private bool _usesColorMult = false;
    [SerializeField] private float _outlineScale = 1.01f;
    private Renderer _rend;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        //Se crean copias de los materiales personalizadas para este objeto
        _rend.materials[0] = new Material(_rend.materials[0]);
        _rend.materials[1] = new Material(_rend.materials[1]);
        //Se asignan las propiedades deseadas
        _rend.materials[0].SetTexture("_Texture", _texture);
        if(_usesColorMult) _rend.materials[0].SetColor("_Color", _colorMult);
        _rend.materials[1].SetFloat("_Scale", _outlineScale);
    }
}
