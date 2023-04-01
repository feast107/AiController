﻿using System.ComponentModel;

namespace AiController.Desktop.Wpf.Utilities
{
    public class ValueWrapper<T> : INotifyPropertyChanged
    {
        public ValueWrapper(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
