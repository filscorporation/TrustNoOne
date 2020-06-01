using System.Linq;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Controlls background sound
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public void Awake()
        {
            if (FindObjectsOfType<SoundManager>().Any(s => s != this))
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
    }
}
