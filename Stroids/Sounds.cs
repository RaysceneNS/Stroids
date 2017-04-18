using System;
using System.Media;
using System.Reflection;

namespace Stroids
{
    public static class Sounds
    {
        private static readonly SoundPlayer FirePlayer;
        private static readonly SoundPlayer ThrustPlayer;
        private static readonly SoundPlayer BangSmallPlayer;
        private static readonly SoundPlayer BangMediumPlayer;
        private static readonly SoundPlayer BangLargePlayer;
        private static readonly SoundPlayer ExtraShipPlayer;
        
        static Sounds()
        {
            var a = Assembly.GetExecutingAssembly();
            
            FirePlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.fire.wav"));
            ThrustPlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.thrust.wav"));
            BangSmallPlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.bangSmall.wav"));
            BangMediumPlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.bangMedium.wav"));
            BangLargePlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.bangLarge.wav"));
            ExtraShipPlayer = new SoundPlayer(a.GetManifestResourceStream("Stroids.wavs.extraShip.wav"));
        }

        public static void Play(Sound sound)
        {
            switch (sound)
            {
                case Sound.Fire:
                    FirePlayer.Play();
                    break;
                case Sound.BangSmall:
                    BangSmallPlayer.Play();
                    break;
                case Sound.BangMedium:
                    BangMediumPlayer.Play();
                    break;
                case Sound.BangLarge:
                    BangLargePlayer.Play();
                    break;
                case Sound.ExtraShip:
                    ExtraShipPlayer.Play();
                    break;
                case Sound.Thrust:
                    ThrustPlayer.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sound");
            }
        }
    }

    public enum Sound
    {
        Fire,
        Thrust,
        BangSmall,
        BangMedium,
        BangLarge,
        ExtraShip
    }
}