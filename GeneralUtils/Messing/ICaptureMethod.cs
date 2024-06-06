using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils.Messing
{

    public interface ICaptureMethod
    {
        void NewReading(ISensorReading reading);
    }
    public interface ICaptureMethod<T> : ICaptureMethod
    {
        void NewReading(T reading);
    }

    public interface ISensorReading
    {

    }

    public class DummyReading : ISensorReading
    {
        public double Value { get; }
    }

    public class Volumetric : CaptureMethodBase<DummyReading>
    {
        public override void NewReading(DummyReading reading)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CaptureMethodBase<T> : ICaptureMethod<T>
    {
        public abstract void NewReading(T reading);

        public void NewReading(ISensorReading reading)
        {
            if(reading is T tReading)
            {
                NewReading(tReading);
            }
        }
    }


    public interface ISensor
    {
        Type SensorReadingType { get; }
        bool IsStreaming { get; }
        Task StartStreaming();

        Task StopStreaming();

        event EventHandler<ISensorReading> DataRecieved;
    }

    public interface ISensor<T> : ISensor
    {
        event EventHandler<T> DataReceived;
    }

    public interface ICaptureManager
    {
        Task StartCapture(bool ForceStream = false);
        Task StopCapture(bool ForceStopStream = false);
    }

    public class CaptureManager<T>
    {
        public ISensor<T> Sensor { get; }

        public ICaptureMethod<T> CaptureMethod { get; }

        public CaptureManager(ISensor<T> sensor, ICaptureMethod<T> captureMethod)
        {
            Sensor = sensor;
            CaptureMethod = captureMethod;
        }

        

        public async Task StartCapture(bool ForceStream = false)
        {
            if (ForceStream)
            {
                if (!Sensor.IsStreaming)
                {
                    await Sensor.StartStreaming();
                }
                Sensor.DataReceived += Sensor_DataRecieved;
            }
        }

        private void Sensor_DataRecieved(object? sender, T e)
        {
            CaptureMethod.NewReading(e);
        }

        public async Task StopCapture(bool ForceStopStream = false)
        {
            Sensor.DataReceived -= Sensor_DataRecieved;
            if (ForceStopStream)
            {
                await Sensor.StopStreaming();
            }
        }
    }

    public class CaptureManager
    {
        readonly ISensor Sensor;

        readonly ICaptureMethod CaptureMethod;

        public CaptureManager(ISensor sensor, ICaptureMethod captureMethod)
        {
            Sensor = sensor;
            CaptureMethod = captureMethod;
        }

        public async Task StartCapture(bool ForceStream = false)
        {
            if (ForceStream)
            {
                if (!Sensor.IsStreaming)
                {
                    await Sensor.StartStreaming();
                }
                Sensor.DataRecieved += Sensor_DataRecieved;
            }
        }

        private void Sensor_DataRecieved(object? sender, ISensorReading e)
        {
            CaptureMethod.NewReading(e);
        }

        public async Task StopCapture(bool ForceStopStream = false)
        {
            Sensor.DataRecieved-= Sensor_DataRecieved;
            if(ForceStopStream)
            {
                await Sensor.StopStreaming();
            }
        }
    }
}
