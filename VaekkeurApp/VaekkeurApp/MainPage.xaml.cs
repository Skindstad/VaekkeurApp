﻿using MediaManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace VaekkeurApp
{
    public partial class MainPage : ContentPage
    {
        DateTime _triggerTime;
        public MainPage()
        {
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
        }

        bool OnTimerTick()
        {
            if (_switch.IsToggled && DateTime.Now >= _triggerTime)
            {
                _switch.IsToggled = false;
                DisplayAlert("Timer Alert", "The '" + _entry.Text + "' timer has elapsed", "OK");
            }
            return true;
        }


        /// <summary>
        /// Checks if there is a match between device time and the given time in seconds
        /// </summary>
        /// <param name="alarmTimeInSeconds"></param>
        /// <param name="alarmToneUrl"></param>
        /// <returns></returns>

        //public async Task CheckTimeForMatch(int alarmTimeInSeconds, string alarmToneUrl)
        public void CheckTimeForMatch(int alarmTimeInSeconds, string alarmToneUrl)
        {
            // Device time now
            DateTime time = DateTime.Now;

            // Recalculating device time into total seconds
            int hoursInSeconds = time.Hour * 60 * 60;
            int minutesInSeconds = time.Minute * 60;
            int seconds = time.Second;
            int totalTimeInSeconds = hoursInSeconds + minutesInSeconds + seconds;

            // Check if there is a match between device time and alarm time
            if (totalTimeInSeconds == alarmTimeInSeconds)
            {
                // If match play a sound
                //await CrossMediaManager.Current.Play(alarmToneUrl);

                var stream = GetStreamFromFile("Battlefield.mp3");
                var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                audio.Load(stream);
                audio.Play();
            }
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("VaekkeurApp." + filename);

            return stream;
        }

        void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                SetTriggerTime();
            }
        }
        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            SetTriggerTime();
        }

        void SetTriggerTime()
        {
            if (_switch.IsToggled)
            {
                _triggerTime = DateTime.Today + TodayTime.Time;
                if(_triggerTime < DateTime.Now)
                {
                    _triggerTime += TimeSpan.FromDays(1);
                }
            }
        }




    }
}
