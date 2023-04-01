﻿using System.ComponentModel;

namespace AiController.Desktop.Wpf.Models
{
    public class AppConfig : INotifyPropertyChanged
    {
        public string ApiHost { get; set; } = "";
        public string ApiKey { get; set; } = string.Empty;
        public string ApiGptModel { get; set; } = "gpt-3.5-turbo";
        public int ApiTimeout { get; set; } = 5000;
        public double Temerature { get; set; } = 1;

        public string[] SystemMessages { get; set; } = new string[]
        {

        };

        public bool WindowAlwaysOnTop { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
