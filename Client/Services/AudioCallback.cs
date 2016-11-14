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
        private BufferedWaveProvider bwp = new BufferedWaveProvider(new WaveFormat(8000, 1));
        private WaveOut wo = new WaveOut();
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
            ClientInformation.CallingWindows[receiver].AcceptedCall();
        }

        public void ChannelDeclined(string receiver)
        {
            ClientInformation.CallingWindows[receiver].DeclinedCall();       
        }

        public void StopCall(string receiver)
        {
            if (ClientInformation.CallingWindows.ContainsKey(receiver))
            {
                ClientInformation.CallingWindows[receiver].ClosedCall();
            }
            if (ClientInformation.AnswerWindows.ContainsKey(receiver))
            {
                ClientInformation.AnswerWindows[receiver].ClosedCall();
            }
            Wi.StopRecording();
            wo.Stop();
        }

        // Not part of interface

        private void wi_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Wi.DataAvailable += null;
        }        

        public void StopPlayingOutput()
        {
            wo.Stop();
        }

        public void StartRecording()
        {
            Wi.StartRecording();
        }

        public void StopRecording()
        {
            Wi.StopRecording();
        }
    }
}
