using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YelloKiller
{
    class Presets : MenuScreen
    {
        YellokillerGame game;

        MenuEntry F1;
        MenuEntry F2;
        MenuEntry F3;
        MenuEntry F4;
        MenuEntry F5;
        MenuEntry F6;
        MenuEntry F7;
        MenuEntry F8;

        public event EventHandler<PlayerIndexEventArgs> F1Selected;
        public event EventHandler<PlayerIndexEventArgs> F2Selected;
        public event EventHandler<PlayerIndexEventArgs> F3Selected;
        public event EventHandler<PlayerIndexEventArgs> F4Selected;
        public event EventHandler<PlayerIndexEventArgs> F5Selected;
        public event EventHandler<PlayerIndexEventArgs> F6Selected;
        public event EventHandler<PlayerIndexEventArgs> F7Selected;
        public event EventHandler<PlayerIndexEventArgs> F8Selected;

        public Presets(YellokillerGame game)
            : base(Langue.tr("Presets"))
        {
            this.game = game;

            F1 = new MenuEntry(Langue.tr("F1"));
            F2 = new MenuEntry(Langue.tr("F2"));
            F3 = new MenuEntry(Langue.tr("F3"));
            F4 = new MenuEntry(Langue.tr("F4"));
            F5 = new MenuEntry(Langue.tr("F5"));
            F6 = new MenuEntry(Langue.tr("F6"));
            F7 = new MenuEntry(Langue.tr("F7"));
            F8 = new MenuEntry(Langue.tr("F8"));


        }

        void MenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // MenuEntry selected = (MenuEntry) sender; <-- très beau aussi!
            MenuEntry selected = sender as MenuEntry;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen(selected.Text, game));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!F1.IsEvent)
            {
                F1.Selected += F1Selected;
                F1.Selected += OnCancel;
                F2.Selected += F2Selected;
                F2.Selected += OnCancel;
                F3.Selected += F3Selected;
                F3.Selected += OnCancel;
                F4.Selected += F4Selected;
                F4.Selected += OnCancel;
                F5.Selected += F5Selected;
                F5.Selected += OnCancel;
                F6.Selected += F6Selected;
                F6.Selected += OnCancel;
                F7.Selected += F7Selected;
                F7.Selected += OnCancel;
                F8.Selected += F8Selected;
                F8.Selected += OnCancel;

                MenuEntries.Add(F1);
                MenuEntries.Add(F2);
                MenuEntries.Add(F3);
                MenuEntries.Add(F4);
                MenuEntries.Add(F5);
                MenuEntries.Add(F6);
                MenuEntries.Add(F7);
                MenuEntries.Add(F8);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
