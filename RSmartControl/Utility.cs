using System;
using MFToolkit.Collection.Spezialized;
using Microsoft.SPOT;

namespace RSmartControl
{
    class Utility
    {
        public static NameValueCollection ParseQueryString(string query)
        {
            string text = query;

            if( text.Length == 0 )
                return null;

            string newtext = "";
            for( int i = 0; i < text.Length; i++ )
            {
                if( text[i] == 'H' )
                {
                    if( text[i + 1] == 'T' )
                    {
                        if( text[i + 2] == 'T' )
                        {
                            newtext = text.Substring( 0, i );
                        }
                    }
                }
            }

            string lastText ="";
            for( int i = 0; i < newtext.Length; i++ )
            {
                // if there are arguments
                if( newtext[i] == '?' )
                {
                    lastText = newtext.Substring( i + 1, newtext.Length - i - 1 );

                }
            }

            NameValueCollection collection = new NameValueCollection();

            if( lastText != "" )
            {
                for( int i = 0; i < lastText.Length; i++ )
                {
                    if( lastText[i] == '=' )
                    {
                        string name = lastText.Substring( 0, i );
                        string value = "";
                        lastText = lastText.Substring( i + 1, lastText.Length - i - 1 );
                        i = 0;
                        for( int j = 0; j < lastText.Length; j++ )
                        {
                            if( lastText[j] == '&' )
                            {
                                value = lastText.Substring( 0, j );
                                lastText = lastText.Substring( j + 1, lastText.Length - j - 1 );
                                break;
                            }
                        }

                        if( name != "" && value != "" )
                        {
                            collection.Add( name, value );
                        }

                    }
                }
            }
            else
            {
                return null;
            }

            if(collection.Count == 0 )
                return null;

            return collection;
        }

        public static bool IsQueryValid( string query )
        {
            string text = query;

            if( text.Length == 0 )
                return false;

            string newtext = "";
            for( int i = 0; i < text.Length; i++ )
            {
                if( text[i] == 'H' )
                {
                    if( text[i + 1] == 'T' )
                    {
                        if( text[i + 2] == 'T' )
                        {
                            newtext = text.Substring( 0, i );
                        }
                    }
                }
            }

            string lastText ="";
            for( int i = 0; i < newtext.Length; i++ )
            {
                // if there are arguments
                if( newtext[i] == '?' )
                {
                    return true;

                }
            }
            return false;
        }
    }
}