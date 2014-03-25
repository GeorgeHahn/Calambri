using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calambri.Interfaces;
using CGurus.Weather.WundergroundAPI;

namespace Calambri.Weather
{
    public class WeatherRenderer : Renderer
    {
        private List<Color> segments;
        private int segmentLength;
        private int segmentExtra;
        private DateTime weatherTime;
        private List<string> conditions; 

        private bool weatherIsFresh()
        {
            return weatherTime != DateTime.MinValue && // Weather fetched at least once
                (weatherTime.AddHours(1) < DateTime.Now); // Weather was refreshed in the last hour
        }

        private void GetWeather()
        {
            // TODO: Gonna need some configuration to provide API keys and location infomation (etc) from config
            // Will regenerate this key later if necessary

            WApi weather = new WApi("af110fd8db184115");
            var forecast = weather.GetForecastHourlyUS("NC", "Charlotte").Hourly_Forecast;

            var segmentCount = forecast.Length;
            segmentLength = PixelCount / segmentCount; // TODO: Evenly distribute extra pixels
            segmentExtra = PixelCount % segmentCount;
            segments = new List<Color>(segmentCount); // TODO: Animated patterns?
            conditions = new List<string>(segmentCount);

            // See full list of conditions at http://www.wunderground.com/weather/api/d/docs?d=resources/phrase-glossary#current_condition_phrases
            foreach (var hour in forecast)
            {
                var condition = hour.Condition;
                conditions.Add(condition);

                //Unknown Precipitation
                //Unknown
                Color thisSegment = new Color(0, 0, 0, 0);


                //Clear
                if (condition.Contains("Clear"))
                {
                    thisSegment = new Color(255, 255, 0);
                }

                //Squalls
                //Thunderstorm
                //Squalls
                //[Light/Heavy] Thunderstorm
                //[Light/Heavy] Thunderstorms and Rain
                //[Light/Heavy] Thunderstorms and Snow
                //[Light/Heavy] Thunderstorms and Ice Pellets
                //[Light/Heavy] Thunderstorms with Hail
                //[Light/Heavy] Thunderstorms with Small Hail
                else if (condition.Contains("Squalls") || condition.Contains("Thunderstorm"))
                {
                    thisSegment = new Color(0, 13, 199);
                }

                //Overcast
                //Partly Cloudy
                //Mostly Cloudy
                //Scattered Clouds
                //Funnel Cloud
                else if (condition.Contains("Overcast") || condition.Contains("Cloud"))
                {
                    thisSegment = new Color(209, 209, 209);
                }

                //[Light/Heavy] Snow
                //[Light/Heavy] Snow Grains
                //[Light/Heavy] Ice Crystals
                //[Light/Heavy] Ice Pellets
                //[Light/Heavy] Hail
                //[Light/Heavy] Low Drifting Snow
                //[Light/Heavy] Blowing Snow
                //[Light/Heavy] Snow Showers
                //[Light/Heavy] Snow Blowing Snow Mist
                //[Light/Heavy] Ice Pellet Showers
                //[Light/Heavy] Hail Showers
                //[Light/Heavy] Small Hail Showers
                //Small Hail
                else if (condition.Contains("Snow") || condition.Contains("Ice") || condition.Contains("Hail"))
                {
                    thisSegment = new Color(255, 255, 255);
                }

                //[Light/Heavy] Freezing Drizzle
                //[Light/Heavy] Freezing Rain
                //[Light/Heavy] Drizzle
                //[Light/Heavy] Rain
                //[Light/Heavy] Rain Mist
                //[Light/Heavy] Rain Showers
                //[Light/Heavy] Spray
                else if (condition.Contains("Drizzle") || condition.Contains("Rain") || condition.Contains("Spray"))
                {
                    thisSegment = new Color(94, 177, 209);
                }

                //[Light/Heavy] Fog
                //[Light/Heavy] Fog Patches
                //[Light/Heavy] Freezing Fog
                //Patches of Fog
                //Shallow Fog
                //Partial Fog
                //[Light/Heavy] Mist
                //[Light/Heavy] Smoke
                //[Light/Heavy] Haze
                else if (condition.Contains("Fog") || condition.Contains("Mist") || condition.Contains("Smoke") || condition.Contains("Haze"))
                {
                    thisSegment = new Color(175, 212, 227);
                }

                //[Light/Heavy] Volcanic Ash
                //[Light/Heavy] Widespread Dust
                //[Light/Heavy] Sand
                //[Light/Heavy] Dust Whirls
                //[Light/Heavy] Sandstorm
                //[Light/Heavy] Low Drifting Widespread Dust
                //[Light/Heavy] Low Drifting Sand
                //[Light/Heavy] Blowing Widespread Dust
                //[Light/Heavy] Blowing Sand
                else if (condition.Contains("Ash") || condition.Contains("Dust") || condition.Contains("Sand"))
                {
                    thisSegment = new Color(227, 204, 150);
                }

                segments.Add(thisSegment);
            }
        }

        public override PixelBuffer Render()
        {
            buffer.Clear();

            if(!weatherIsFresh())
                GetWeather();

            int pos = 0;
            var thisSegLen = 0;
            var extraPixels = segmentExtra;
            foreach (var segment in segments)
            {
                thisSegLen = segmentLength + (extraPixels > 0 ? 1 : 0); // Pad an extra pixel to fill display
                buffer.DrawSegment(pos, segment, thisSegLen);
                pos += thisSegLen;

                if (extraPixels > 0)
                    extraPixels--;
            }

            return buffer;
        }
    }
}
