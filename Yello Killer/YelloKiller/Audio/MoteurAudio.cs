using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class MoteurAudio
    {
        #region Properties

        public bool IsMoteurAudio
        {
            get { return isMoteurAudio; }
            protected internal set { isMoteurAudio = value; }
        }

        bool isMoteurAudio = true;

        #endregion

        #region Fields

        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue cue;
       
        #endregion

        #region Initialization
        public MoteurAudio()
        {
            ///<remarks>
            /// Ces trois trucs-là, à obtenir après la création du .xap, avec XACT2 et pas XACT3, et à placer manuellement dans
            /// Yellokiller\bin\x86\Debug
            ///</remarks>

            engine = new AudioEngine("son.xgs"); 
            waveBank = new WaveBank(engine, "Wave Bank.xwb");
            soundBank = new SoundBank(engine, "Sound Bank.xsb");
            
            cue = soundBank.GetCue("collision");
            cue.Play();
        }
        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }
        #endregion

        #region Update and Draw

        public void Update()
        {
            engine.Update();
        }
        #endregion

    }
        
}
