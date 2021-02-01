using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource knifeAudioSource;
    [SerializeField] AudioSource backgroundAudioSource;
    [SerializeField] AudioSource cylinderSoundsSource;

    [SerializeField] AudioClip[] cylinderHitAudioClips;
    [SerializeField] AudioClip dropCylinderSound;
    [SerializeField] AudioClip menuBackgroundSound;
    [SerializeField] AudioClip gameBackgroundSound;
    [SerializeField] AudioClip knifeClangSound;

    [SerializeField] AudioClip[] breathClips;
    private void Start()
    {
        PlayMenuBackgroundMusic();
    }
    public void PlayKnifeHitSound()
    {
        knifeAudioSource.clip = cylinderHitAudioClips[Random.Range(0, cylinderHitAudioClips.Length)];
        knifeAudioSource.volume = 1f;
        knifeAudioSource.Play();
    }
    public void PlayKnifeClangSound()
    {
        knifeAudioSource.clip = knifeClangSound;
        knifeAudioSource.volume = 1f;
        knifeAudioSource.Play();
    }
    public void PlayDropCylinderSound()
    {
        cylinderSoundsSource.clip = dropCylinderSound;
        cylinderSoundsSource.Play();
        cylinderSoundsSource.loop = false;
    }
    public void PlayGameBackgroundMusic()
    {
        backgroundAudioSource.clip = gameBackgroundSound;
        backgroundAudioSource.Play();
    }
    public void PlayMenuBackgroundMusic()
    {
        backgroundAudioSource.clip = menuBackgroundSound;
        backgroundAudioSource.Play();
    }
    public void ChangeCylinderSound(LevelModel levelModel)
    {
        if (levelModel.CylinderSound == SoundType.SlowBreath)  
            cylinderSoundsSource.clip = breathClips[0];
        else if (levelModel.CylinderSound == SoundType.FastBreath)
            cylinderSoundsSource.clip = breathClips[1];
        else if (levelModel.CylinderSound == SoundType.Scream)
            cylinderSoundsSource.clip = breathClips[2];
        cylinderSoundsSource.loop = true;

        if (levelModel.CylinderSound == SoundType.Disabled)
        {
            cylinderSoundsSource.Stop();
        }
        else
        {
            cylinderSoundsSource.Play();
        }
    }

    public void StopCylinderSound()
    {
        cylinderSoundsSource.Stop();
    }
}
