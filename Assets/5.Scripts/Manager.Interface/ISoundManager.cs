namespace Manager.Interface
{
    public interface ISoundManager
    {
        void PlaySFX(string soundName);
        void PlayBGM(string musicName);
        void StopAllBGM();
        void SetBGMVolume(float volume);
        void SetSFXVolume(float volume);
        float BGMVolume { get; }
        float SFXVolume { get; }
    }
}