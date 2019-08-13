using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Spec.Sniffer.Model;
using Spec.Sniffer.Model.Battery;
using Spec.Sniffer.Model.Camera;

namespace Spec.Sniffer.ViewModel
{
    public class DiagTab : INotifyPropertyChanged
    {
        public DiagTab()
        {
            CameraLoad();
            _testTune = new AudioTest($"{Directory.GetCurrentDirectory()}\\Resources\\ShortTone.mp3");
            MicBtnIsChecked = false;
        }

        #region local variables

        private IEnumerable<MediaInformation> _mediaDeviceList;
        private MediaInformation _selectedVideoDevice;
        private bool _tuneBtnIsChecked;
        private readonly AudioTest _testTune;
        private bool? _micBtnIsChecked;
        private readonly DispatcherTimer _tuneTimer = new DispatcherTimer();

        #endregion

        #region Microphone

        public bool? MicBtnIsChecked
        {
            get => _micBtnIsChecked;
            set
            {
                _micBtnIsChecked = value;

                if (_micBtnIsChecked == true)
                {
                    var thread = new Thread(MicRecordTask);
                    thread.Start();
                }

                RaisePropertyChanged("MicBtnIsChecked");
            }
        }

        private void MicRecordTask()

        {
            var rec = new MicTest($"{Path.GetTempPath()}\\mictest.wav");
            rec.Start();
            Thread.Sleep(3000);
            MicBtnIsChecked = null;

            rec.Stop();
            rec.Play();
            Thread.Sleep(3000);
            MicBtnIsChecked = false;
        }

        #endregion

        #region Tune

        public bool TuneBtnIsChecked
        {
            get => _tuneBtnIsChecked;
            set
            {
                if (value)
                {
                    _tuneBtnIsChecked = true;
                    PlayTune();
                }
                else
                {
                    _tuneBtnIsChecked = false;
                    StopTune();
                }

                RaisePropertyChanged("TuneBtnIsChecked");
            }
        }

        private void PlayTune()
        {
            _testTune.Play();
            _tuneTimer.Interval = TimeSpan.FromSeconds(6.1);
            _tuneTimer.Tick += PlayTimer_Tick;
            _tuneTimer.Start();
        }

        private void StopTune()
        {
            _testTune.Stop();
            _tuneTimer.Stop();
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            TuneBtnIsChecked = false;
            _tuneTimer.Stop();
        }

        #endregion

        #region Camera

        public IEnumerable<MediaInformation> MediaDeviceList
        {
            get => _mediaDeviceList;

            set
            {
                _mediaDeviceList = value;
                RaisePropertyChanged("MediaDeviceList");
            }
        }

        public MediaInformation SelectedVideoDevice
        {
            get => _selectedVideoDevice;

            set
            {
                _selectedVideoDevice = value;
                RaisePropertyChanged("SelectedVideoDevice");
            }
        }


        private void CameraLoad()
        {
            MediaDeviceList = WebcamDevice.GetVideoDevices;
            //SelectedVideoDevice = MediaDeviceList.FirstOrDefault();
            SelectedVideoDevice = null;
        }

        #endregion

        #region Battery

        public BatteryStatus Batteries { get; set; }=new BatteryStatus();


        #endregion

        #region INotify Property handler

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}