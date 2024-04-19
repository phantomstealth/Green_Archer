using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioSource ShieldStopSound;
    public AudioSource WalkArcherSound;
    public AudioSource DeathArcherSound;
    public AudioSource HitArcherSound;
    public AudioSource DeathYellowMonsterSound;
    public AudioSource PlayOneShotSound;
    public AudioSource PlayOneShotPlayerSound;
    public AudioSource Орку_БольноSound;
    public AudioSource Орк_УмираетSound;
    public AudioSource Принцесса_Неубит_ДраконSound;

    public static AudioSource ShieldStop;
    public static AudioSource WalkArcher;
    public static AudioSource HitArcher;
    public static AudioSource DeathArcher;
    public static AudioSource DeathYellowMonster;
    public static AudioSource PlayOneShot;
    public static AudioSource PlayOneShotPlayer;
    public static AudioSource Орку_больно;
    public static AudioSource Орк_умирает;
    public static AudioSource Принцесса_Неубит_Дракон;

    // Start is called before the first frame update
    void Start()
    {
        WalkArcher= WalkArcherSound;
        DeathArcher = DeathArcherSound;
        HitArcher = HitArcherSound;
        DeathYellowMonster = DeathYellowMonsterSound;
        PlayOneShot = PlayOneShotSound;
        PlayOneShotPlayer = PlayOneShotPlayerSound;
        Орку_больно = Орк_УмираетSound;
        Орк_умирает = Орк_УмираетSound;
        ShieldStop = ShieldStopSound;
        Принцесса_Неубит_Дракон = Принцесса_Неубит_ДраконSound;

    }

    public static void PlaySound(AudioSource snd)
    {
        //snd.Stop();
        snd.Play();
    }

    public static void PlayOneShotSource(AudioSource snd,AudioClip clip)
    {
        snd.PlayOneShot(clip);
    }

    public static void PlayOneShotPlayerSource(AudioSource snd, AudioClip clip)
    {
        snd.PlayOneShot(clip);
    }


    public static void StopSound(AudioSource snd)
    {
        snd.Stop();
    }
}
