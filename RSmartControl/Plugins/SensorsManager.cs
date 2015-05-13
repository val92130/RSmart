using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace RSmartControl
{
    public class SensorsManager
    {
        private readonly Sensor _frontSensorLeft, _frontSensorRight, _downSensor, _backSensor;
        private readonly OutputPort _led;
        private readonly AnalogInput _magneticSensor;
        public SensorsManager()
        {
            _magneticSensor = new AnalogInput( Cpu.AnalogChannel.ANALOG_4 );
            _led = new OutputPort(Pins.GPIO_PIN_D2, false);
            _frontSensorLeft = new Sensor( new AnalogInput( Cpu.AnalogChannel.ANALOG_0 ) );
            _frontSensorRight = new Sensor( new AnalogInput( Cpu.AnalogChannel.ANALOG_1 ) );
            _downSensor = new Sensor( new AnalogInput( Cpu.AnalogChannel.ANALOG_2 ) );
            _backSensor = new Sensor( new AnalogInput( Cpu.AnalogChannel.ANALOG_3 ) );
        }


        /// <summary>
        /// Turn on the led for 100 milliseconds
        /// </summary>
        public void BlinkLed()
        {
            _led.Write(true);
            Thread.Sleep(100);
            _led.Write(false);
        }

        public AnalogInput MagneticSensor
        {
            get
            {
                return _magneticSensor;
            }
        }


        public Sensor FrontSensorLeft
        {
            get
            {
                return _frontSensorLeft;
            }
        }
        
        public Sensor FrontSensorRight
        {
            get
            {
                return _frontSensorRight;
            }
        }

        public Sensor DownSensor
        {
            get
            {
                return _downSensor;
            }
        }


        public Sensor BackSensor
        {
            get
            {
                return _downSensor;
            }
        }
    }
}
