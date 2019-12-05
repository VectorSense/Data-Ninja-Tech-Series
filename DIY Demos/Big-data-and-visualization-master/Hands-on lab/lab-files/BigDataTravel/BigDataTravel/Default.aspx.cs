using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using DarkSky.Services;
using System.Runtime.Serialization;
using NodaTime;

namespace BigDataTravel
{
    public partial class _Default : Page
    {
        private const string BASE_WEATHER_URI = "http://api.wunderground.com/api/{0}/hourly10day/q/{1}.json";

        private List<Airport> aiports = null;
        private ForecastResult forecast = null;
        private DelayPrediction prediction = null;
        private static DarkSkyService darkSky;

        // settings
        private string mlUrl;
        private string weatherApiKey;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitSettings();
            InitAirports();

            if (!IsPostBack)
            {
                txtDepartureDate.Text = DateTime.Now.AddDays(5).ToShortDateString();

                darkSky = new DarkSkyService(weatherApiKey);

                ddlOriginAirportCode.DataSource = aiports;
                ddlOriginAirportCode.DataTextField = "AirportCode";
                ddlOriginAirportCode.DataValueField = "AirportCode";
                ddlOriginAirportCode.DataBind();

                ddlDestAirportCode.DataSource = aiports;
                ddlDestAirportCode.DataTextField = "AirportCode";
                ddlDestAirportCode.DataValueField = "AirportCode";
                ddlDestAirportCode.DataBind();
                ddlDestAirportCode.SelectedIndex = 12;
            }
        }

        private void InitSettings()
        {
            mlUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["mlUrl"];
            weatherApiKey = System.Web.Configuration.WebConfigurationManager.AppSettings["weatherApiKey"];
        }

        private void InitAirports()
        {
            aiports = new List<Airport>()
            {
                new Airport() { AirportCode ="SEA", Latitude = 47.44900, Longitude = -122.30899 },
                new Airport() { AirportCode ="ABQ", Latitude = 35.04019, Longitude = -106.60900 },
                new Airport() { AirportCode ="ANC", Latitude = 61.17440, Longitude = -149.99600 },
                new Airport() { AirportCode ="ATL", Latitude = 33.63669, Longitude = -84.42810 },
                new Airport() { AirportCode ="AUS", Latitude = 30.19449, Longitude = -97.66989 },
                new Airport() { AirportCode ="CLE", Latitude = 41.41170, Longitude = -81.84980 },
                new Airport() { AirportCode ="DTW", Latitude = 42.21239, Longitude = -83.35340 },
                new Airport() { AirportCode ="JAX", Latitude = 30.49410, Longitude = -81.68789 },
                new Airport() { AirportCode ="MEM", Latitude = 35.04240, Longitude = -89.97669 },
                new Airport() { AirportCode ="MIA", Latitude = 25.79319, Longitude = -80.29060 },
                new Airport() { AirportCode ="ORD", Latitude = 41.97859, Longitude = -87.90480 },
                new Airport() { AirportCode ="PHX", Latitude = 33.43429, Longitude = -112.01200 },
                new Airport() { AirportCode ="SAN", Latitude = 32.73360, Longitude = -117.19000 },
                new Airport() { AirportCode ="SFO", Latitude = 37.61899, Longitude = -122.37500 },
                new Airport() { AirportCode ="SJC", Latitude = 37.36259, Longitude = -121.92900 },
                new Airport() { AirportCode ="SLC", Latitude = 40.78839, Longitude = -111.97799 },
                new Airport() { AirportCode ="STL", Latitude = 38.74869, Longitude = -90.37000 },
                new Airport() { AirportCode ="TPA", Latitude = 27.97550, Longitude = -82.53320 }
            };
        }

        protected async void btnPredictDelays_Click(object sender, EventArgs e)
        {
            var departureDate = DateTime.Parse(txtDepartureDate.Text);
            departureDate = departureDate.AddHours(double.Parse(txtDepartureHour.Text));

            var selectedAirport = aiports.FirstOrDefault(a => a.AirportCode == ddlOriginAirportCode.SelectedItem.Value);

            if (selectedAirport != null)
            {
                var query = new DepartureQuery()
                {
                    DepartureDate = departureDate,
                    DepartureDayOfWeek = ((int)departureDate.DayOfWeek) + 1, //Monday = 1
                    Carrier = txtCarrier.Text,
                    OriginAirportCode = selectedAirport.AirportCode,
                    OriginAirportLat = selectedAirport.Latitude,
                    OriginAirportLong = selectedAirport.Longitude,
                    DestAirportCode = ddlDestAirportCode.SelectedItem.Text
                };

                await GetWeatherForecast(query);

                if (forecast == null)
                    throw new Exception("Forecast request did not succeed. Check Settings for weatherApiKey.");

                PredictDelays(query, forecast).Wait();
            }

            UpdateStatusDisplay(prediction, forecast);
        }

        private void UpdateStatusDisplay(DelayPrediction prediction, ForecastResult forecast)
        {
            weatherForecast.ImageUrl = forecast.ForecastIconUrl;
            weatherForecast.ToolTip = forecast.Condition;

            if (String.IsNullOrWhiteSpace(mlUrl))
            {
                lblPrediction.Text = "(not configured)";
                lblConfidence.Text = "(not configured)";
                return;
            }

            if (prediction == null)
                throw new Exception("Prediction did not succeed. Check the Settings for mlUrl.");

            if (prediction.ExpectDelays)
            {
                lblPrediction.Text = "expect delays";
            }
            else
            {
                lblPrediction.Text = "no delays expected";
            }

            lblConfidence.Text = $"{(prediction.Confidence * 100.0):N2}";
        }

        private async Task GetWeatherForecast(DepartureQuery departureQuery)
        {
            var departureDate = departureQuery.DepartureDate;
            forecast = null;

            try
            {
                var weatherPrediction = await darkSky.GetForecast(departureQuery.OriginAirportLat,
                    departureQuery.OriginAirportLong, new DarkSkyService.OptionalParameters
                    {
                        ExtendHourly = true,
                        DataBlocksToExclude = new List<ExclusionBlock> { ExclusionBlock.Flags,
                        ExclusionBlock.Alerts, ExclusionBlock.Minutely }
                    });
                if (weatherPrediction.Response.Hourly.Data != null && weatherPrediction.Response.Hourly.Data.Count > 0)
                {
                    var timeZone = DateTimeZoneProviders.Tzdb[weatherPrediction.Response.TimeZone];
                    var zonedDepartureDate = LocalDateTime.FromDateTime(departureDate)
                        .InZoneLeniently(timeZone);

                    forecast = (from f in weatherPrediction.Response.Hourly.Data
                                where f.DateTime == zonedDepartureDate.ToDateTimeOffset()
                                select new ForecastResult()
                                {
                                    WindSpeed = f.WindSpeed ?? 0,
                                    Precipitation = f.PrecipIntensity ?? 0,
                                    Pressure = f.Pressure ?? 0,
                                    ForecastIconUrl = GetImagePathFromIcon(f.Icon),
                                    Condition = f.Summary
                                }).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Failed retrieving weather forecast: " + ex.ToString());
            }
        }

        private string GetImagePathFromIcon<T>(T value)
            where T : struct, IConvertible
        {
            var defaultIconPath = Page.ResolveUrl("~/images/cloudy.svg");
            if (value.ToString() == "None")
            {
                return defaultIconPath;
            }
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(value.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            return attr != null ? Page.ResolveUrl($"~/images/{attr.Value}.svg") : defaultIconPath;
        }

        private async Task PredictDelays(DepartureQuery query, ForecastResult forecast)
        {
            if (string.IsNullOrEmpty(mlUrl))
            {
                return;
            }

            var departureDate = DateTime.Parse(txtDepartureDate.Text);

            prediction = new DelayPrediction();

            try
            {
                using (var client = new HttpClient())
                {
                    var predictionRequest = new PredictionRequest
                    {
                        OriginAirportCode = query.OriginAirportCode,
                        Month = query.DepartureDate.Month,
                        DayofMonth = query.DepartureDate.Day,
                        CRSDepHour = query.DepartureDate.Hour,
                        DayOfWeek = query.DepartureDayOfWeek,
                        Carrier = query.Carrier,
                        DestAirportCode = query.DestAirportCode,
                        WindSpeed = forecast.WindSpeed,
                        SeaLevelPressure = forecast.Pressure,
                        HourlyPrecip = forecast.Precipitation
                    };

                    client.BaseAddress = new Uri(mlUrl);
                    var response = await client.PostAsJsonAsync("", predictionRequest).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseResult = await response.Content.ReadAsStringAsync();
                        var token = JToken.Parse(responseResult);
                        var parsedResult = JsonConvert.DeserializeObject<List<PredictionResult>>((string)token);
                        var result = parsedResult[0];
                        double confidence = double.Parse(result.confidence.Replace("[", string.Empty).Replace("]", string.Empty).Split(new Char[] {','})[0]);
                        if (result.prediction == 1)
                        {
                            this.prediction.ExpectDelays = true;
                            this.prediction.Confidence = confidence;
                        }
                        else if (result.prediction == 0)
                        {
                            this.prediction.ExpectDelays = false;
                            this.prediction.Confidence = confidence;
                        }
                        else
                        {
                            this.prediction = null;
                        }

                    }
                    else
                    {
                        prediction = null;

                        Trace.Write($"The request failed with status code: {response.StatusCode}");

                        // Print the headers - they include the request ID and the timestamp, which are useful for debugging the failure
                        Trace.Write(response.Headers.ToString());

                        var responseContent = await response.Content.ReadAsStringAsync();
                        Trace.Write(responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                prediction = null;
                System.Diagnostics.Trace.TraceError("Failed retrieving delay prediction: " + ex.ToString());
                throw;
            }
        }
    }

    #region Data Structures

    public class PredictionRequest
    {
        public string OriginAirportCode { get; set; }
        public int Month { get; set; }
        public int DayofMonth { get; set; }
        public int CRSDepHour { get; set; }
        public int DayOfWeek { get; set; }
        public string Carrier { get; set; }
        public string DestAirportCode { get; set; }
        public double WindSpeed { get; set; }
        public double SeaLevelPressure { get; set; }
        public double HourlyPrecip { get; set; }
    }

    public class PredictionResult
    {
        public double prediction { get; set; }
        public string confidence { get; set; }
    }

    public class ForecastResult
    {
        public double WindSpeed;
        public double Precipitation;
        public double Pressure;
        public string ForecastIconUrl;
        public string Condition;
    }

    public class DelayPrediction
    {
        public bool ExpectDelays;
        public double Confidence;
    }

    public class DepartureQuery
    {
        public string OriginAirportCode;
        public double OriginAirportLat;
        public double OriginAirportLong;
        public string DestAirportCode;
        public DateTime DepartureDate;
        public int DepartureDayOfWeek;
        public string Carrier;
    }

    public class Airport
    {
        public string AirportCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    #endregion
}
