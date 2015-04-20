using System;
using Microsoft.SPOT;
using System.Collections;

namespace RSmartControl
{
    class Utility
    {
        public static Hashtable NewParser(object query)
        {
            if (query == null)
                return null;
            string request = (string)query;
            string keyword = "HTTP";
            int ind = request.IndexOf(keyword);

            if (query == null || ind == -1)
                return null;

            request = request.Substring(ind + keyword.Length, request.Length - ind - keyword.Length);

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] == '?')
                {
                    request = request.Substring(i + 1, request.Length - i - 1);
                    break;
                }
            }

            Hashtable collection = new Hashtable();
            while (request.IndexOf("=") != -1)
            {
                string leftArg, rightArg = null;
                ind = request.IndexOf("=");
                leftArg = request.Substring(0, ind);
                request = request.Substring(ind + 1, request.Length - ind - 1);

                int rightInd = request.IndexOf('&');
                if (rightInd != -1)
                {
                    rightArg = request.Substring(0, rightInd);
                    request = request.Substring(rightInd + 1, request.Length - 1 - rightInd);
                }

                if (rightArg != null && leftArg != null)
                {
                    collection.Add(leftArg, rightArg);
                }
            }

            return collection;
        }

        public static Hashtable ParseQueryString(object query)
        {
            if (query == null)
                return null;
            string text = (string)query;
            if(text == null)
            {
                return null;
            }
            if (text.Length == 0)
                return null;

            string newtext = "";

            // is it valid ?
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == 'H')
                {
                    if (text[i + 1] == 'T')
                    {
                        if (text[i + 2] == 'T')
                        {
                            newtext = text.Substring(0, i);                            
                        }
                    }
                }
            }

            string lastText = "";
            for (int i = 0; i < newtext.Length; i++)
            {
                // if there are arguments
                if (newtext[i] == '?')
                {
                    lastText = newtext.Substring(i + 1, newtext.Length - i - 1);

                }
            }

            Hashtable collection = new Hashtable();

            if (lastText != "")
            {
                for (int i = 0; i < lastText.Length; i++)
                {
                    if (lastText[i] == '=')
                    {
                        string name = lastText.Substring(0, i);
                        string value = "";
                        lastText = lastText.Substring(i + 1, lastText.Length - i - 1);
                        i = 0;
                        for (int j = 0; j < lastText.Length; j++)
                        {
                            if (lastText[j] == '&')
                            {
                                value = lastText.Substring(0, j);
                                lastText = lastText.Substring(j + 1, lastText.Length - j - 1);
                                break;
                            }
                        }

                        if (name != "" && value != "")
                        {
                            collection.Add(name, value);
                        }

                    }
                }
            }
            else
            {
                return null;
            }

            if (collection.Count == 0)
                return null;

            return collection;
        }

        public static bool IsQueryValid(string query)
        {
            string text = query;

            if (text.Length == 0)
                return false;

            string newtext = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == 'H')
                {
                    if (text[i + 1] == 'T')
                    {
                        if (text[i + 2] == 'T')
                        {
                            newtext = text.Substring(0, i);
                        }
                    }
                }
            }

            for (int i = 0; i < newtext.Length; i++)
            {
                // if there are arguments
                if (newtext[i] == '?')
                {
                    return true;

                }
            }
            return false;
        }

        public static double DegreeToRadian(double degree)
        {
            return degree*(System.Math.PI/180);
        }
    }
}
