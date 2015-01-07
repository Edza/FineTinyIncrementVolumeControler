using NAudio.CoreAudioApi;
using System;
using System.Runtime.InteropServices;

namespace FineTinyIncrementVolumeControler
{
    internal static class NativeMethods
    {
        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
    }

    public static class MSWindowsFriendlyNames
    {
        public static string WindowsXP { get { return "Windows XP"; } }

        public static string WindowsVista { get { return "Windows Vista"; } }

        public static string Windows7 { get { return "Windows 7"; } }

        public static string Windows8 { get { return "Windows 8"; } }
    }

    public static class SystemVolumChanger
    {
        public static void SetVolume(int value)
        {
            if (value < 0)
                value = 0;

            if (value > 100)
                value = 100;

            SetVolumeForWIndowsVista78(value);
        }

        public static int GetVolume()
        {
            int result = 100;
            try
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator(); // EDataFlow.eRender // ERole.eMultimedia
                MMDevice device = DevEnum.GetDefaultAudioEndpoint((DataFlow)0, (Role)1);
                result = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            }
            catch (Exception)
            {
            }

            return result;
        }

        private static void SetVolumeForWIndowsVista78(int value)
        {
            try
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint((DataFlow)0, (Role)1);

                device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100.0f;
            }
            catch (Exception)
            {
            }
        }
    }
}