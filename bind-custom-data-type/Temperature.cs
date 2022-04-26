using System;

namespace UIToolkitExamples
{
    public enum TemperatureUnit
    {
        Celsius,
        Farenheit
    }

    [Serializable]
    public struct Temperature
    {
        public double value;
        public TemperatureUnit unit;
    }
}