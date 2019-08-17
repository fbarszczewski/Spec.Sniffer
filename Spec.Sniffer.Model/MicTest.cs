using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Spec.Sniffer.Model
{
    /// <summary>
    /// Z kazda nowa proba nagrania dzwieku output jest taki sam jak pierwszy
    /// zeby to ominac wprowadzam zmiane nazwy pliku. dzieki temu za kazdym razem klasa bedzie odtwarzac inny plik.
    /// </summary>
    public class MicTest
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        private readonly string _path;

        private static int _fileCount = 0;

        public MicTest(string path)
        {
            this._path = $"{path}\\micTest{_fileCount}.wav";
            _fileCount++;
        }


        public void Start()
        {

            record("open new Type waveaudio Alias recsound", "", 0, 0);

            record("record recsound", "", 0, 0);
        }

        public void Stop()
        {
            record($"save recsound {_path}", "", 0, 0);

            record("close recsound", "", 0, 0);

        }

        public void Play()
        {
            if (File.Exists(_path))
            {

                Uri toneUrl = new Uri(_path);
                _mediaPlayer.Open(toneUrl);
                _mediaPlayer.Play();
                try
                {
                    File.Delete(_path);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }
        }
    }
}
