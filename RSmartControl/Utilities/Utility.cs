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
        public static double GetAngle(Vector2 position, Vector2 orientation, Vector2 destination)
        {
            Vector2 directionVector = new Vector2(destination.X - position.X, destination.Y - position.Y);
            double angle = Vector2.GetAngle(new Vector2(orientation.X, orientation.Y), new Vector2(directionVector.X, directionVector.Y));
            return Vector2.RadianToDegree(angle);
        }



        public static ArrayList GetPathList(Robot robot, ArrayList vectorList)
        {
            ArrayList pathList = new ArrayList();

            Vector2 robotPosition = new Vector2(robot.Position.X, robot.Position.Y);

            Vector2 currentOrientation = new Vector2(robot.Orientation.X, robot.Orientation.Y);

            foreach (Vector2 p in vectorList)
            {

                double angle = GetAngle(robotPosition, currentOrientation, p);


                double oX = currentOrientation.X;
                double oY = currentOrientation.Y;
                double dX = p.X;
                double dY = p.Y;


                double ang;
                Vector2 VecToTarget = robotPosition - p;
                if ((VecToTarget.X * currentOrientation.Y) > (VecToTarget.Y * currentOrientation.X))
                {
                    ang = angle;
                    currentOrientation = TransformPoint(currentOrientation,
                        (float)(Vector2.DegreeToRadian(angle)));

                    //robot.Orientation = currentOrientation;
                }
                else
                {
                    ang = -angle;
                    currentOrientation = TransformPoint(currentOrientation,
                        -(float)(Vector2.DegreeToRadian(angle)));

                    //robot.Orientation = currentOrientation;
                }

                PathInformation pathInfo = new PathInformation(Round(ang), GetTimeToDestinationMilli(robotPosition, p), new Vector2(Round(currentOrientation.X), Round(currentOrientation.Y)), new Vector2(Round(dX), Round(dY)));
                pathList.Add(pathInfo);
                robotPosition = p;

            }

            return pathList;
        }

        public static int GetTimeToDestinationMilli(Vector2 position, Vector2 destination)
        {
            double speedCm = 46;

            double length = Vector2.Distance(position, destination);

            double dist = length / speedCm;
            return (int)dist * 1000;

        }

        public static float Round(float value)
        {
            double t = value * 100;
            t = System.Math.Round(t);
            return (float)t / 100f;
        }
        public static double Round(double value)
        {
            double t = value * 100;
            t = System.Math.Round(t);
            return t / 100f;
        }

        public static Vector2 TransformPoint(Vector2 point, float angleRadian)
        {
            Vector2 newPoint = new Vector2(point.X, point.Y);

            double x = newPoint.X;
            double y = newPoint.Y;

            float px = (float)(x * System.Math.Cos(angleRadian) - y * System.Math.Sin(angleRadian));
            float py = (float)(x * System.Math.Sin(angleRadian) + y * System.Math.Cos(angleRadian));

            return new Vector2(Round(px), Round(py));
        }

    }
}
