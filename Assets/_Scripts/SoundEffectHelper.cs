using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    public class SoundEffectHelper : MonoBehaviour
    {
        public static SoundEffectHelper Instance;

        public AudioClip woodSound;
        public AudioClip playerShotSound;
        public AudioClip enemyShotSound;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of SoundEffectsHelper!");
            }
            Instance = this;
        }

        public void MakeWoodSound()
        {
            MakeSound(woodSound);
        }

        public void MakePlayerShotSound()
        {
            MakeSound(playerShotSound);
        }

        public void MakeEnemyShotSound()
        {
            MakeSound(enemyShotSound);
        }

        // Lance la lecture d'un son
        private void MakeSound(AudioClip originalClip)
        {
            AudioSource.PlayClipAtPoint(originalClip, transform.position);
        }
    }

}