using Microsoft.Xna.Framework.Audio;

namespace Yellokiller
{
    class MoteurAudio
    {
        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;

        public MoteurAudio()
        {
            engine = new AudioEngine(@"Content\Ambiance sonore.xgs");
            waveBank = new WaveBank(engine, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(engine, @"Content\Sound Bank.xsb");
        }

        public void Update()
        {
            engine.Update();
        }
    }
}