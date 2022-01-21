using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamicDesktop
{
    class Misc
    {
        public static Image Text(string text, int width, int height, Brush brush)
        {
            Image image = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(image))
            {
                gr.Clear(Color.Green);
                SizeF strSize = gr.MeasureString(text, new Font("consolas", 20));
                if (strSize.Width > width)
                {
                    string str = "";
                    foreach (char c in text)
                    {
                        str += c;
                        if (gr.MeasureString(str, new Font("consolas", 20)).Width > width)
                        {
                            str = str.Substring(0, str.Length - 1);
                            str += "\n" + c;
                        }
                    }
                    text = str;
                    strSize = gr.MeasureString(text, new Font("consolas", 20));
                }
                gr.DrawString(text, new Font("consolas", 20), brush, (width / 2) - (strSize.Width / 2), (height / 2) - (strSize.Height / 2));
                gr.Save();
            }
            return image;
        }
    }

    public enum Orientations
    {
        None,
        Landscape,
        Portrait,
        Square
    }

    public class CustomTimeSpan
    {
        public int Days;
        public int Hours;
        public int Mins;
        public int Secs;
        public int MiliSecs;
        public CustomTimeSpan(int days, int hours, int mins, int secs, int milisecs)
        {
            Days = days;
            Hours = hours;
            Mins = mins;
            Secs = secs;
            MiliSecs = milisecs;
        }
        public CustomTimeSpan(int milisecs)
        {
            Days = milisecs / (24 * 60 * 60 * 1000);
            Hours = milisecs / (60 * 60 * 1000) % 24;
            Mins = milisecs / (60 * 1000) % 60;
            Secs = milisecs / (1000) % 60;
            MiliSecs = milisecs / (1000) % 1000;
        }
        public double TotalMiliSecs { get { return MiliSecs + (Secs * 1000) + (Mins * 60 * 1000) + (Hours * 60 * 60 * 1000) + (Days * 24 * 60 * 60 * 1000); } }
        public double TotalSecs { get { return (MiliSecs / 1000) + Secs + (Mins * 60) + (Hours * 60 * 60) + (Days * 24 * 60 * 60); } }
        public double TotalMins { get { return (MiliSecs / 1000 / 60) + (Secs / 60) + Mins + (Hours * 60) + (Days * 24 * 60); } }
        public double TotalHours { get { return (MiliSecs / 1000 / 60 / 60) + (Secs / 60 / 60) + (Mins / 60) + Hours + (Days * 24); } }
        public double TotalDays { get { return (MiliSecs / 1000 / 60 / 60 / 24) + (Secs / 60 / 60 / 24) + (Mins / 60 / 24) + (Hours / 24) + Days; } }

        public override string ToString()
        { return "MiliSecs: '" + MiliSecs + "' Secs: '" + Secs + "' Mins: '" + Mins + "' Hours: '" + Hours + "' Days: '" + Days + "'"; }

        public static CustomTimeSpan operator -(CustomTimeSpan t)
        { return new CustomTimeSpan((int)-t.TotalMiliSecs); }

        public static CustomTimeSpan operator +(CustomTimeSpan t1, CustomTimeSpan t2)
        { return new CustomTimeSpan((int)(t1.TotalMiliSecs + t2.TotalMiliSecs)); }
        public static CustomTimeSpan operator *(CustomTimeSpan t1, CustomTimeSpan t2)
        { return new CustomTimeSpan((int)(t1.TotalMiliSecs * t2.TotalMiliSecs)); }
        public static CustomTimeSpan operator *(double factor, CustomTimeSpan t)
        { return new CustomTimeSpan((int)(factor * t.TotalMiliSecs)); }
        public static CustomTimeSpan operator /(CustomTimeSpan t, double factor)
        { return new CustomTimeSpan((int)(t.TotalMiliSecs / factor)); }
        public static CustomTimeSpan operator /(CustomTimeSpan t1, CustomTimeSpan t2)
        { return new CustomTimeSpan((int)(t1.TotalMiliSecs / t2.TotalMiliSecs)); }
        public static bool operator ==(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs == t2.TotalMiliSecs; }
        public override bool Equals(object o)
        { return false ? o == null : (TotalMiliSecs == ((CustomTimeSpan)o).TotalMiliSecs); }
        public override int GetHashCode()
        {
            return TotalMiliSecs.GetHashCode();
        }

        public static bool operator !=(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs != t2.TotalMiliSecs; }
        public static bool operator <(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs < t2.TotalMiliSecs; }
        public static bool operator >(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs > t2.TotalMiliSecs; }
        public static bool operator >=(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs >= t2.TotalMiliSecs; }
        public static bool operator <=(CustomTimeSpan t1, CustomTimeSpan t2)
        { return t1.TotalMiliSecs <= t2.TotalMiliSecs; }
    }
}
