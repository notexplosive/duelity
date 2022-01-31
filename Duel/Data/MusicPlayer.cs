using Machina.Engine;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public enum TrackName
    {
        Title,
        ThistownA,
        ThistownB,
        Oasis,
        Mines,
        Knight
    }

    public class MusicPlayer
    {
        private readonly Track minesTrack;
        private readonly Track oasisTrack;
        private readonly Track knightTrack;
        private readonly Track titleTrack;
        private readonly Track thistownATrack;
        private readonly Track thistownBTrack;
        private readonly List<Track> allTracks;

        public MusicPlayer()
        {
            this.titleTrack = new Track("bgm_title");
            this.thistownATrack = new Track("bgm_1a");
            this.thistownBTrack = new Track("bgm_1b");
            this.minesTrack = new Track("bgm_mines");
            this.oasisTrack = new Track("bgm_oasis");
            this.knightTrack = new Track("bgm_knight");

            this.allTracks = new List<Track>
            {
                this.titleTrack,
                this.thistownATrack,
                this.thistownBTrack,
                this.minesTrack,
                this.oasisTrack,
                this.knightTrack
            };
        }

        public void StopAll()
        {
            foreach (var track in this.allTracks)
            {
                track.Stop();
            }
        }

        public void PlayTrack(TrackName trackName)
        {
            StopAll();

            switch (trackName)
            {
                case TrackName.Title:
                    this.titleTrack.Play();
                    break;
                case TrackName.ThistownA:
                    this.thistownATrack.Play();
                    break;
                case TrackName.ThistownB:
                    this.thistownBTrack.Play();
                    break;
                case TrackName.Oasis:
                    this.oasisTrack.Play();
                    break;
                case TrackName.Mines:
                    this.minesTrack.Play();
                    break;
                case TrackName.Knight:
                    this.knightTrack.Play();
                    break;
            }
        }

        public class Track
        {
            private readonly SoundEffectInstance sound;

            public Track(string trackName)
            {

                this.sound = MachinaClient.Assets.GetSoundEffectInstance(trackName);

                // this is the most "Jam Code" code I've ever written.
                if (trackName == "bgm_knight")
                {
                    this.sound.Volume = 0.35f;
                }

                if (trackName == "bgm_mines")
                {
                    this.sound.Volume = 0.35f;
                }

                if (trackName != "bgm_title")
                {
                    this.sound.IsLooped = true;
                }

                if(trackName == "bgm_1b"){
                    this.sound.Volume = 0.5f;
                }
            }

            public void Play()
            {
                this.sound.Play();
            }


            public void Stop()
            {
                this.sound.Stop();
            }
        }
    }
}
