using RJJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamicDesktop
{
    public class Settings
    {
        public Settings()
        {
            Folders = new List<string>();
        }
        public Settings(
            List<string> folders,
            int style,
            bool tile,
            bool sizeRestriction,
            Orientations orientation,
            Size maxSize,
            Size minSize,
            CustomTimeSpan time
        )
        {
            Folders = folders;
            Style = style;
            Tile = tile;
            SizeRestriction = sizeRestriction;
            Orientation = orientation;
            MaxSize = maxSize;
            MinSize = minSize;
            Time = time;
        }
        public Settings(string settingsStr)
        {
            Folders = new List<string>();
            JSONTypes SettingsJson = JSON.StringToObject(settingsStr);
            if (SettingsJson.Data.ContainsKey("folders"))
            {
                if (SettingsJson["folders"].Type == JSON.Types.LIST)
                {
                    foreach (JSONTypes folder in SettingsJson["folders"])
                    {
                        if (folder.Type == JSON.Types.STRING)
                        {
                            Folders.Add(folder.Data);
                        }
                    }
                }
                else
                {
                    Folders = new List<string> { "C:\\WINDOWS\\web\\wallpaper\\Theme2", "C:\\WINDOWS\\web\\wallpaper\\Windows", "C:\\WINDOWS\\web\\wallpaper\\Theme1" };
                }
            }
            else
            {
                Folders = new List<string> { "C:\\WINDOWS\\web\\wallpaper\\Theme2", "C:\\WINDOWS\\web\\wallpaper\\Windows", "C:\\WINDOWS\\web\\wallpaper\\Theme1" };
            }
            if (SettingsJson.Data.ContainsKey("style"))
            {
                if (SettingsJson["style"].Type == JSON.Types.FLOAT)
                {
                    Style = (int)SettingsJson["style"].Data;
                }
                else
                {
                    Style = 10;
                }
            }
            else
            {
                Style = 10;
            }
            if (SettingsJson.Data.ContainsKey("tile"))
            {
                if (SettingsJson["tile"].Type == JSON.Types.BOOL)
                {
                    Tile = SettingsJson["tile"].Data;
                }
                else
                {
                    Tile = false;
                }
            }
            else
            {
                Tile = false;
            }
            if (SettingsJson.Data.ContainsKey("size restriction"))
            {
                if (SettingsJson["size restriction"].Type == JSON.Types.BOOL)
                {
                    SizeRestriction = SettingsJson["size restriction"].Data;
                }
                else
                {
                    SizeRestriction = false;
                }
            }
            else
            {
                SizeRestriction = false;
            }
            if (SettingsJson.Data.ContainsKey("orientation"))
            {
                if (SettingsJson["orientation"].Type == JSON.Types.STRING)
                {
                    switch (SettingsJson["orientation"].Data)
                    {
                        case "None":
                            Orientation = Orientations.None;
                            break;
                        case "Landscape":
                            Orientation = Orientations.Landscape;
                            break;
                        case "Portrait":
                            Orientation = Orientations.Portrait;
                            break;
                        case "Square":
                            Orientation = Orientations.Square;
                            break;
                        default:
                            Orientation = Orientations.None;
                            break;
                    }
                }
                else
                {
                    Orientation = Orientations.None;
                }
            }
            else
            {
                Orientation = Orientations.None;
            }
            if (SettingsJson.Data.ContainsKey("max size"))
            {
                if (SettingsJson["max size"].Type == JSON.Types.LIST)
                {
                    if (SettingsJson["max size"][0].Type == JSON.Types.FLOAT && SettingsJson["max size"][1].Type == JSON.Types.FLOAT)
                    {
                        MaxSize = new Size((int)SettingsJson["max size"][0].Data, (int)SettingsJson["max size"][1].Data);
                    }
                }
                else
                {
                    MaxSize = new Size(0, 0);
                }
            }
            else
            {
                MaxSize = new Size(0, 0);
            }
            if (SettingsJson.Data.ContainsKey("min size"))
            {
                if (SettingsJson["min size"].Type == JSON.Types.LIST)
                {
                    if (SettingsJson["min size"][0].Type == JSON.Types.FLOAT && SettingsJson["min size"][1].Type == JSON.Types.FLOAT)
                    {
                        MinSize = new Size((int)SettingsJson["min size"][0].Data, (int)SettingsJson["min size"][1].Data);
                    }
                }
                else
                {
                    MinSize = new Size(0, 0);
                }
            }
            else
            {
                MinSize = new Size(0, 0);
            }
            if (SettingsJson.Data.ContainsKey("time"))
            {
                if (SettingsJson["time"].Type == JSON.Types.DICT)
                {
                    int milisecs = 0;
                    int sec = 0;
                    int mins = 0;
                    int hours = 0;
                    int days = 0;
                    if (SettingsJson["time"].Data.ContainsKey("milisecs"))
                    {
                        milisecs = (int)SettingsJson["time"]["milisecs"].Data;
                    }

                    if (SettingsJson["time"].Data.ContainsKey("sec"))
                    {
                        sec = (int)SettingsJson["time"]["sec"].Data;
                    }

                    if (SettingsJson["time"].Data.ContainsKey("mins"))
                    {
                        mins = (int)SettingsJson["time"]["mins"].Data;
                    }

                    if (SettingsJson["time"].Data.ContainsKey("hours"))
                    {
                        hours = (int)SettingsJson["time"]["hours"].Data;
                    }

                    if (SettingsJson["time"].Data.ContainsKey("days"))
                    {
                        days = (int)SettingsJson["time"]["days"].Data;
                    }

                    if (days + hours + mins + sec + milisecs == 0)
                    {
                        Time = new CustomTimeSpan(0, 1, 0, 0, 0);
                    }
                    else
                    {
                        Time = new CustomTimeSpan(days, hours, mins, sec, milisecs);
                    }
                }
                else
                {

                }
            }
            else
            {
                Time = new CustomTimeSpan(0, 1, 0, 0, 0);
            }
        }

        public List<string> Folders;
        public int Style;
        public bool Tile;
        public bool SizeRestriction;
        public Orientations Orientation;
        public Size MaxSize;
        public Size MinSize;
        public CustomTimeSpan Time;
        public override string ToString()
        {
            return
                "Folders : [" +
                String.Join(",", Folders) +
                "],\nStyle : " +
                Style.ToString() +
                ",\nTile : " +
                Tile +
                ",\nSize Restriction : " +
                SizeRestriction +
                ",\nOrientation : " +
                Orientation +
                ",\nMax Size : " +
                MaxSize.ToString() +
                ",\nMin Size : " +
                MinSize.ToString() +
                ",\nTime : " +
                Time;
        }
        public JSONTypes ExportSettings()
        {
            JSONTypes newSettings = new JSONDictionary();
            newSettings.Data.Add("folders", new JSONList());
            foreach (string folder in Folders)
            {
                newSettings.Data["folders"].Data.Add(new JSONString(folder));
            }
            newSettings.Data.Add("style", new JSONFloat(Style));
            newSettings.Data.Add("tile", new JSONBool(Tile));
            newSettings.Data.Add("size restriction", new JSONBool(SizeRestriction));
            newSettings.Data.Add("orientation", new JSONString(Orientation.ToString()));
            newSettings.Data.Add("max size", new JSONList());
            newSettings["max size"].Data.Add(new JSONFloat(MaxSize.Width));
            newSettings["max size"].Data.Add(new JSONFloat(MaxSize.Height));
            newSettings.Data.Add("min size", new JSONList());
            newSettings["min size"].Data.Add(new JSONFloat(MinSize.Width));
            newSettings["min size"].Data.Add(new JSONFloat(MinSize.Height));
            newSettings.Data.Add("time", new JSONDictionary());
            if (Time.MiliSecs != 0)
            {
                newSettings["time"].Data.Add("milisecs", new JSONFloat(Time.MiliSecs));
            }
            if (Time.Secs != 0)
            {
                newSettings["time"].Data.Add("secs", new JSONFloat(Time.Secs));
            }
            if (Time.Mins != 0)
            {
                newSettings["time"].Data.Add("mins", new JSONFloat(Time.Mins));
            }
            if (Time.Hours != 0)
            {
                newSettings["time"].Data.Add("hours", new JSONFloat(Time.Hours));
            }
            if (Time.Days != 0)
            {
                newSettings["time"].Data.Add("days", new JSONFloat(Time.Days));
            }
            return newSettings;
        }
        public void WriteSettings(string path)
        {
            JSONTypes ExportSettingsJSON = ExportSettings();
            string ExportSettingsStr = JSON.FormatJson(ExportSettingsJSON.ToString());
            File.WriteAllText(path, ExportSettingsStr);
        }

    }
}
