using System;

namespace Server.Lib
{
    [Serializable]
    public class Route
    {
        string _key, _value, _response;

        /// <summary>
        /// Creates a new route
        /// </summary>
        /// <param name="key">Route Key</param>
        /// <param name="value">Route Value</param>
        /// <param name="response">Response to return when someone access the route</param>
        public Route(string key, string value, string response)
        { 
            _key = key;
            _value = value;
            _response = response;
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
