using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.ServiceModel;
using System.Threading;

namespace Client
{
    public class AudioCallback : IAudioCallback
    {
        private BufferedWaveProvider bwp = new BufferedWaveProvider(ClientInformation.waveFormat);
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
            if(ClientInformation.CallingWindows.ContainsKey(receiver))
                ClientInformation.CallingWindows[receiver].AcceptedCall();
        }

        public void ChannelDeclined(string receiver)
        {
            if(ClientInformation.CallingWindows.ContainsKey(receiver))
                ClientInformation.CallingWindows[receiver].DeclinedCall();       
        }

        public void StopCall(string receiver)
        {
            if (ClientInformation.CallingWindows.ContainsKey(receiver))          
                ClientInformation.CallingWindows[receiver].ClosedCall();
            if (ClientInformation.AnswerWindows.ContainsKey(receiver))
                ClientInformation.AnswerWindows[receiver].ClosedCall();
            if(Wi != null)
                Wi.StopRecording();
            if(wo != null)
                wo.Stop();
        }

        // Not part of interface

        private void wi_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Wi.DataAvailable += null;
        }        

        public void StopPlayingOutput()
        {
            if(Wi != null)
                wo.Stop();
        }

        public void StartRecording()
        {
            if(Wi != null)
                Wi.StartRecording();
        }

        public void StopRecording()
        {
            if(Wi !=null)
                Wi.StopRecording();
        }
    }
}
