using System;
using Microsoft.SPOT;
using System.Collections;

namespace RSmartControl
{
    class Utility
    {
        public static Hashtable ParseQueryString( object query )
        {
            if( query == null )
                return null;
            string request = (string)query;
            string keyword = "HTTP";
            int ind = request.IndexOf( keyword );

            if( ind == -1 )
                return null;

            request = request.Substring( 0, ind );

            for( int i = 0; i < request.Length; i++ )
            {
                if( request[i] == '?' )
                {
                    request = request.Substring( i + 1, request.Length - i - 1 );
                    break;
                }
            }

            Hashtable collection = new Hashtable();
            while( request.IndexOf( "=" ) != -1 )
            {
                string leftArg, rightArg = null;
                ind = request.IndexOf( "=" );
                leftArg = request.Substring( 0, ind );
                request = request.Substring( ind + 1, request.Length - ind - 1 );

                int rightInd = request.IndexOf( '&' );
                if( rightInd != -1 )
                {
                    rightArg = request.Substring( 0, rightInd );
                    request = request.Substring( rightInd + 1, request.Length - 1 - rightInd );
                }
                else
                {
                    rightArg = request.Substring( 0, request.Length );
                }

                if( rightArg != null && leftArg != null )
                {
                    collection.Add( leftArg, rightArg );
                }
            }

            return collection;
        }

        public static bool IsQueryValid( string query )
        {
            string text = query;

            if( text.Length == 0 )
                return false;

            if (text.IndexOf('?') != -1)
            {
                return true;
            }
            return false;
        }

        public static double DegreeToRadian( double degree )
        {
            return degree * (System.Math.PI / 180);
        }
    }
}
