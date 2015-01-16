using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FineTinyIncrementVolumeControler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int minVol = -1, maxVol = -1, targetVol = -1, realVol = -1;

        #region GUI

        bool isAllSet = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            minVol = (int)min.Value;
            maxVol = (int)max.Value;
            targetVol = (int)vol.Value;

            isAllSet = true;
            SetVolume();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var item = sender as CheckBox;
            this.Topmost = item.IsChecked.Value;
        }

        private void min_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if(!Validate())
            //    return;

            minVol = (int)min.Value;
            ResetVol();
        }

        private bool Validate()
        {
            if (min == null || max == null)
                return true;

            bool validated = true;

            if (min.Value <= max.Value)
            {
                if (max.Value - 10 > -1)
                    min.Value = max.Value - 10;
                else
                {
                    max.Value = 10;
                    min.Value = 0;
                }
                validated = false;
            }

            return validated;
        }

        private void max_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (!Validate())
            //    return;

            maxVol = (int)max.Value;
            ResetVol();
        }

        void ResetVol()
        {
            if (vol == null)
                return;
            if (realVol != -1)
            {
                if (realVol > (int)maxVol)
                {
                    vol.Value = 100.0;
                    ForceSetVolume(maxVol);
                }
                else if(realVol < (int)minVol)
                {
                    vol.Value = 0.0;
                    ForceSetVolume(minVol);
                }
                else
                {
                    // ja realVol ir iekš min->max diazapona
                    double pos = (double)(realVol - minVol) / (maxVol - minVol);
                    vol.Value = pos * 100;
                }
            }
            else
                vol.Value = 50.0;
        }

        private void vol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            targetVol = (int)vol.Value;

            if(minVol != -1 && maxVol != -1)
                SetVolume();
        }

        #endregion

        private void SetVolume()
        {
            if (!isAllSet)
                return;
            int difference = maxVol - minVol;
            double ration = targetVol / 100.0;
            double delta = ration * difference;
            realVol = minVol + (int)Math.Round(delta);
            SystemVolumChanger.SetVolume(realVol);
        }

        private void ForceSetVolume(int val)
        {
            SystemVolumChanger.SetVolume(val);
        }
    }
}
