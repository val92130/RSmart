using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    public class SensorsManager
    {
        Sensor _frontSensorLeft, _frontSensorRight, _downSensor, _backSensor;
        MainLoop _mainLoop;
        public SensorsManager(MainLoop mainloop)
        {
            _mainLoop = mainloop;
            _frontSensorLeft = new Sensor( _mainLoop, new AnalogInput( Cpu.AnalogChannel.ANALOG_0 ) );
            _frontSensorRight = new Sensor( _mainLoop, new AnalogInput( Cpu.AnalogChannel.ANALOG_1 ) );
            _downSensor = new Sensor( _mainLoop, new AnalogInput( Cpu.AnalogChannel.ANALOG_2 ) );
            _backSensor = new Sensor( _mainLoop, new AnalogInput( Cpu.AnalogChannel.ANALOG_3 ) );
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
