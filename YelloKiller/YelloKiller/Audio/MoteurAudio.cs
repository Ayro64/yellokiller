using Microsoft.Xna.Framework.Audio;

namespace YelloKiller
{
    class MoteurAudio
    {
        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;

        public MoteurAudio()
        {
            engine = new AudioEngine(@"Content\Sons\Ambiance sonore.xgs");
            waveBank = new WaveBank(engine, @"Content\Sons\Wave Bank.xwb");
            soundBank = new SoundBank(engine, @"Content\Sons\Sound Bank.xsb");
        }

        public SoundBank SoundBank
        {
            get { return soundBank; }
        }

        public void Update()
        {
            engine.Update();
        }
    }
}