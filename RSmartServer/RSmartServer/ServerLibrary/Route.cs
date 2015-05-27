using System;

namespace Server.Lib
{
    [Serializable]
    public class Route
    {
        string _key, _value, _response;
        public Route(string key, string value, string response)
        { 
            _key = key;
            _value = value;
            _response = response;
        }

        public Route()
        {

        }
        public string Key
        {
            get
            {
                return _key;
            }
            set { }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            set { }
        }
        public string Response
        {
            get
            {
                return _response;
            }
            set { }
        }
    }
}
