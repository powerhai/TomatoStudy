using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Common;
using Contract.Events;
using Prism.Commands;
using Prism.Events;
namespace Tomato.MediaPlayer.ViewModels
{
    public class MediaPlayerViewModel : ViewModelBase
    {

        private System.Windows.Media.MediaPlayer mMediaPlayer = new System.Windows.Media.MediaPlayer();
        private DispatcherTimer mTimer = new DispatcherTimer();
        private ICommand mPauseCommand;
        private ICommand mPlayCommand;
        private ICommand mSetSpeedCommand;
        private double mTotalMilliseconds;
        private double mCurrentPosition;
        private bool mIsAvaliable;
        public double TotalMilliseconds
        {
            get { return mTotalMilliseconds; }
            set
            {
                mTotalMilliseconds = value;
                OnPropertyChanged();
            }
        }
        public double CurrentPosition
        {
            get { return mCurrentPosition; }
            set
            {
                mCurrentPosition = value; 
                OnPropertyChanged();
            }
        }
        public bool IsAvaliable
        {
            get { return mIsAvaliable; }
            set
            {
                mIsAvaliable = value;
                OnPropertyChanged();
            }
        }
        public MediaPlayerViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<PlayMediaEvent>().Subscribe(PlayMedia);
            eventAggregator.GetEvent<StopMediaPlayingEvent>().Subscribe(StopMediaPlaying);
            Init();
        }
         private void Init()
        {
            mMediaPlayer.MediaOpened += MMediaPlayer_MediaOpened;
            mTimer.Interval = new TimeSpan(0,0,1);
            mTimer.Tick += MTimer_Tick;
            
        }
       private void StopMediaPlaying()
        {
             mMediaPlayer.Stop();
            IsAvaliable = false;
        }

        private void MMediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            TotalMilliseconds = mMediaPlayer.NaturalDuration.TimeSpan.Ticks;
        }

        private void MTimer_Tick(object sender, EventArgs e)
        {
            CurrentPosition = mMediaPlayer.Position.Ticks;
        }

        private void PlayMedia(string path)
        {
            if(!File.Exists(path))
                return;
            IsAvaliable = true;
            mMediaPlayer.Open(new Uri(path)); 
            mMediaPlayer.Play();
            mTimer.Start();
           
        }

        public void JumpTo(double value)
        {
            mMediaPlayer.Position = new TimeSpan(Convert.ToInt64(value));
        }
        public ICommand PauseCommand
        {
            get { return mPauseCommand??(mPauseCommand = new DelegateCommand(() =>
                         {
                             mMediaPlayer.Pause();
                             mTimer.Stop();
                         })); } 
        }
        public ICommand PlayCommand
        {
            get { return mPlayCommand ??(mPlayCommand = new DelegateCommand(() =>
                         {
                             mMediaPlayer.Play();
                             mTimer.Start();
                         })); } 
        }
        public ICommand SetSpeedCommand
        {
            get { return mSetSpeedCommand ??(mSetSpeedCommand =new DelegateCommand<string>((speed) =>
                         {
                             var s = Convert.ToDouble(speed);
                             mMediaPlayer.SpeedRatio = s;
                             mMediaPlayer.Play();
                         })); } 
        }
    }

}
