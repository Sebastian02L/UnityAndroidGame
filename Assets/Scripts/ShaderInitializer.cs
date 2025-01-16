using UnityEngine;

public class ShaderInitializer : MonoBehaviour
{
    [SerializeField] private Texture _texture;
    [SerializeField] private Color _colorMult;
    [SerializeField] private bool _usesColorMult = false;
    private Renderer _rend;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        //Se crean copias de los materiales personalizadas para este objeto
        _rend.material = new Material(_rend.material);
        //Se asignan las propiedades deseadas
        _rend.material.SetTexture("_Texture", _texture);
        if(_usesColorMult) _rend.material.SetColor("_Color", _colorMult);
    }
}
