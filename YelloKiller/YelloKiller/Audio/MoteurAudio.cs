using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    public class MoteurAudio
    {
        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;
        Song[] musiques;

        public MoteurAudio()
        {
            engine = new AudioEngine(@"Content\Sons\Ambiance sonore.xgs");
            waveBank = new WaveBank(engine, @"Content\Sons\Wave Bank.xwb");
            soundBank = new SoundBank(engine, @"Content\Sons\Sound Bank.xsb");
            musiques = new Song[2];
        }

        public SoundBank SoundBank
        {
            get { return soundBank; }
        }

        public void Update()
        {
            engine.Update();
        }

        public void LoadContent(ContentManager content)
        {
            musiques[0] = content.Load<Song>(@"Sons\musique1");
            musiques[1] = content.Load<Song>(@"Sons\musique2");
        }

        public Song[] Musiques
        {
            get { return musiques; }
        }
    }
}