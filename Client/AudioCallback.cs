using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace Client
{
    public class AudioCallback : IAudioCallback
    {
        BufferedWaveProvider bwp = new BufferedWaveProvider(new WaveFormat(44100, 16, 1));
        WaveOut wo = new WaveOut();
        public WaveIn Wi { get; set; }

        public AudioCallback(WaveIn wi)
        {
            wo.Init(bwp);
            bwp.DiscardOnBufferOverflow = true;
            wo.Volume = 1.0f;
            Wi = wi;
            Wi.RecordingStopped += wi_RecordingStopped;
        }
        public AudioCallback()
        {
            wo.Init(bwp);
            bwp.DiscardOnBufferOverflow = true;
            wo.Volume = 1.0f;
        }

        public void SendVoiceCallback(byte[] voice, int bytesRecorded)
        {
            bwp.AddSamples(voice, 0, bytesRecorded);
            wo.Play();
        }

        public void ChannelAccepted(string receiver)
        {
            Wi.StartRecording();
        }

        public void ChannelDeclined(string receiver)
        {
           
        }

        public void StopCall(string receiver)
        {
            Wi.StopRecording();
        }


        private void wi_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Wi.DataAvailable += null;
        }
    }
}
