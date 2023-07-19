using UnityEngine;

namespace DeExtinction.Mono;
internal class FiltorbCurrentGenerator : MonoBehaviour
{
    private static FiltorbCurrentGenerator _main;

    public static FiltorbCurrentGenerator Main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameObject("FiltorbCurrentGenerator").AddComponent<FiltorbCurrentGenerator>();
            }
            return _main;
        }
    }

    public Vector3 CurrentForceDirection { get; private set; }

    private const float _currentChangeSpeed = 0.2f;

    private void Update()
    {
        CurrentForceDirection = new Vector3(0, Mathf.Sin(Time.time * _currentChangeSpeed), 0);
    }
}
