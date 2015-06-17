using System;
using System.Collections.Generic;

namespace Server.Lib
{
    [Serializable]
    public class Route
    {
        string _key, _value, _response;
        private bool _dynamicResponse = false;
        private string _description;
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

        public Route(string key, bool dynamicResponse)
        {
            _key = key;
            _value = null;
            _dynamicResponse = true;
        }

        public Route( string key, bool dynamicResponse, string description )
        {
            _key = key;
            _value = null;
            _dynamicResponse = true;
            _description = description;
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool IsDynamic
        {
            get { return _dynamicResponse; }
        }

        public string AnalyzeQuery(string key, string value)
        {
            if (!_dynamicResponse)
            {
                throw new Exception("Cannot analyze a non dynamic route");
            }
            switch (key)
            {
                case "GetRadius" :
                    List<Vector2> pts = FetchPointsQuery(value);
                    if (pts.Count != 3)
                        goto default;
                    return Convert.ToString(Vector2.Radius(pts[0], pts[2], pts[1]));

                case "GetAngle" :
                    List<Vector2> points = FetchPointsQuery(value);
                    if( points.Count != 2 )
                        goto default;
                    return Convert.ToString( Vector2.GetAngle( points[0], points[1] ) );

                case "GetDistance":
                    List<Vector2> points2 = FetchPointsQuery( value );
                    if( points2.Count != 2 )
                        goto default;
                    return Convert.ToString( Vector2.Distance( points2[0], points2[1] ) );

                default :
                    return String.Empty;

            }
        }

        private List<Vector2> FetchPointsQuery(string query)
        {
            string[] points = query.Split( ';' );
            List<Vector2> vectors = new List<Vector2>();
            foreach (string s in points)
            {
                if (s == "" || String.IsNullOrWhiteSpace(s) || String.IsNullOrEmpty(s))
                {
                    continue;
                }
                try
                {
                    string[] pts = s.Split( ',' );
                    vectors.Add( new Vector2( double.Parse( pts[0] ), double.Parse( pts[1] ) ) );
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }

            return vectors;
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
