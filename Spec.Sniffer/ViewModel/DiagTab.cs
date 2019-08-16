using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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
            InternetStatusSet();
            InternetTimer();
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
        private readonly DispatcherTimer _internetTimer = new DispatcherTimer();
        private string _internetStatus;

        #endregion

        #region Microphone

        public bool? MicBtnIsChecked
        {
            get => _micBtnIsChecked;
            set
            {
                _micBtnIsChecked = value;

                //if (_micBtnIsChecked == true)
                //    MicRecord();

                RaisePropertyChanged("MicBtnIsChecked");
            }
        }

        private void MicRecordTask()

        {
            MicBtnIsChecked = true;
            _isRecording = true;
            var rec = new MicTest($"{Path.GetTempPath()}\\mictest.wav");
            rec.Start();
            Thread.Sleep(3000);
            MicBtnIsChecked = null;

            rec.Stop();
            rec.Play();
            Thread.Sleep(3000);
            MicBtnIsChecked = false;
            _isRecording = false;
        }

        private bool _isRecording = false;
        private void MicRecord()
        {
            if (_isRecording==false)
            {
                var thread = new Thread(MicRecordTask);
                thread.Start();
            }
        }

        public ICommand MicRecordCommand { get => new RelayCommand(argument =>MicRecord() ); }
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

        public ICommand TuneCommand { get => new RelayCommand(argument =>TuneBtnIsChecked=!TuneBtnIsChecked ); }
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

        private bool _camVisibility;
        private bool _micBtnIsEnabled;

        public bool CamVisibility 
        {
            get => _camVisibility;

            set
            {
                _camVisibility = value;
                RaisePropertyChanged("CamVisibility");
            }
        }


        private void CameraButton()
        {
            if (SelectedVideoDevice == null)
            {
                CamVisibility = true;
                SelectedVideoDevice = MediaDeviceList.FirstOrDefault();
            }
            else
            {
                CamVisibility = false;
                SelectedVideoDevice = null;
            }
        }
        public ICommand CamCommand { get => new RelayCommand(argument =>CameraButton() ); }

        private void CameraLoad()
        {
            MediaDeviceList = WebcamDevice.GetVideoDevices;
            SelectedVideoDevice = null;
        }

        #endregion

        #region Battery
        public BatteryStatus Batteries { get; set; } =new BatteryStatus(2);


        #endregion

        #region Drivers Status
        public DriversStatus Drivers { get; set; } = new DriversStatus(5);
        #endregion

        #region Internet status

        public string InternetStatus
        {
            get => _internetStatus;

            set { _internetStatus = value; RaisePropertyChanged("InternetStatus"); }
        }

        private void InternetTimer()
        {
            _internetTimer.Interval = TimeSpan.FromSeconds(2);
            _internetTimer.Tick += Internet_Tick;
            _internetTimer.Start();
        }

        private void Internet_Tick(object sender, EventArgs e)
        {
            InternetStatusSet();
        }

        private void InternetStatusSet()
        {
            InternetStatus = Internet.IsConnected() ? "Connected" : "Disconnected";
        }

        #endregion

        #region LCD Button
        public ICommand LcdTestCommand { get => new RelayCommand(argument =>OpenLcdTest() ); }

        private void OpenLcdTest()
        {
            LcdTest lcdTest=new LcdTest();
            lcdTest.Show();
        }
        #endregion
        public ICommand KeyboardCommand
        {
            get => new RelayCommand(argument => KeyboardTest());
        }
        private void KeyboardTest()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = $"{Directory.GetCurrentDirectory()}\\Resources\\KeyboardTest.exe";
                startInfo.Arguments = "-CS:2";
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ICommand DevmgmtCommand
        {
            get => new RelayCommand(argument => Process.Start("devmgmt.msc"));
        }
        public ICommand DiskmgmtCommand
        {
            get => new RelayCommand(argument => Process.Start("diskmgmt.msc"));
        }
        public ICommand HdTuneCommand
        {
            get => new RelayCommand(argument => HdTune());
        }
        private void HdTune()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = $"{Directory.GetCurrentDirectory()}\\Resources\\HDTune.exe";
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand ShowKeyCommand
        {
            get => new RelayCommand(argument => ShowKey());
        }
        private void ShowKey()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = $"{Directory.GetCurrentDirectory()}\\Resources\\ShowKey.exe";
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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