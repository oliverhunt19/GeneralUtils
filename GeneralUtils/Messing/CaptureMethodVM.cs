using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils.Messing
{
    public class CaptureMethodVM
    {
        private Task StartCapture()
        {

        }

        private Task StopCapture()
        {

        }
    }

    public class VolumetricCaptureVM : CaptureMethodVM
    {

    }

    public class CaptureVM
    {
        public List<ISensor> SensorCOMs { get; set; }

         
    }

    public enum CaptureType
    {
        Volumetric,
        SingleRead
    }

    public static class CaptureMethodVMFactory
    {
        public static IReadOnlyList<CaptureType> Create(ISensor sensor)
        {
            Type type = sensor.SensorReadingType;
            if (!type.TypeInherits<ISensorReading>())
            {
                throw new ArgumentException("Type not of sensor reading");
            }
            switch(type)
            {
                case Type _ when type == typeof(DummyReading):
                    {
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
            return new List<CaptureType>() { CaptureType.Volumetric };
        }

        public static CaptureMethodVM CreateTest(ISensor sensor)
        {
            CaptureType captureType = Create(sensor).First();
            return Create(sensor, captureType);
        }

        public static CaptureMethodVM Create<T>(ISensor<T> sensor, CaptureType captureType)
        {

        }
    }
}
