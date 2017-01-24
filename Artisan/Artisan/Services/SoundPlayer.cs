using System.Media;
using System.IO;

namespace Artisan.Services
{
    class NotificationSoundPlayer
    {
        public string SoundPath { get; set; }
        public SoundPlayer Player
        {
            get
            {
                return soundPlayer;
            }
        }
        private SoundPlayer soundPlayer;

        public NotificationSoundPlayer(string soundPath)
        {
            if(File.Exists(soundPath))
            {
                SoundPath = soundPath;
                soundPlayer = new SoundPlayer(SoundPath);
            }
            else
            {
                throw new FileNotFoundException("The notification sound file was not found.");
            }
        }
    }
}
