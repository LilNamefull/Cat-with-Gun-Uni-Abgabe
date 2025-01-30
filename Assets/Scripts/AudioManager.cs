using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("----------Audio Source -------")]
    [SerializeField] AudioSource musicSource;  
    [SerializeField] AudioSource SFXSource;  

    [Header("----------Audio Clip -------")]
    public AudioClip BackgroundTitel;  
    public AudioClip BackgroundLevel;  
    public AudioClip BackgroundBoss;   
    public AudioClip DeathPlayer;      
    public AudioClip DeathEnemy;       
    public AudioClip ShootPlayer;     
    public AudioClip ShootEnemy;      
    public AudioClip EnemyRocketMous;  
    public AudioClip EnemyMous;        
    public AudioClip EnemyGangster;   
    public AudioClip Boss;             
    public AudioClip PowerUp;         
    public AudioClip MachoRat;        
    public AudioClip SwortRat;        
    public AudioClip BombRat;        

    private void Start()
    {
        // By default, the level music is started
        musicSource.clip = BackgroundLevel;  // Set the music clip to the level music
        musicSource.Play();  // Play the music
        DontDestroyOnLoad(gameObject); // Ensure the AudioManager persists between scene transitions
    }

    private void OnEnable()
    {
        // Subscribe to the scene loading event to detect when scenes change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loading event when the object is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the loaded scene and change the music accordingly
        if (scene.name == "BossKampf") // Check if the scene is the Boss Fight scene
        {
            PlayMusic(BackgroundBoss); // Play boss fight music
        }
        else if (scene.name == "Level1" || scene.name == "Level2") // Check if the scene is a normal level
        {
            PlayMusic(BackgroundLevel); // Play the normal level music
        }
        else if (scene.name == "MainMenu") // Check if the scene is the main menu
        {
            PlayMusic(BackgroundTitel);  // Play title screen music
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        // If the requested music is already playing, do nothing
        if (musicSource.clip == clip) return;

        musicSource.Stop();  // Stop the current music
        musicSource.clip = clip;  // Set the new music clip
        musicSource.Play();  // Play the new music
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);  // Play the sound effect once
    }

    public void PlayEnemySound(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);  // Play the enemy-related sound effect once
    }
}