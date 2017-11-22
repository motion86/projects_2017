using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrillDeep.BAL
{
    class Worker
    {
        public static async Task TestAsync()
        {
            var API_KEY = "DPKH1ACZ6TULZ2W0";

            var StockTickers = new List<string> { "AAPL", "AMD", "MSFT", "NVDA" };

            foreach (var ticker in StockTickers)
            {
                var function = AlphaVantageApiWrapper.ApiParam.FunctionParam(AlphaVantageApiWrapper.AvFuncationEnum.Ema);
                var timePeriod = AlphaVantageApiWrapper.ApiParam.TimePeriodParam("5");
                var interval = AlphaVantageApiWrapper.ApiParam.IntervalParam(AlphaVantageApiWrapper.AvIntervalEnum.Daily);
                var seriesType = AlphaVantageApiWrapper.ApiParam.SeriesTypeParam(AlphaVantageApiWrapper.AvSeriesType.Open);

                var parameters = new List<AlphaVantageApiWrapper.ApiParam>
                {
                    function,
                    AlphaVantageApiWrapper.ApiParam.TickerParam(ticker),
                    interval,
                    timePeriod,
                    seriesType,
                };

                //Start Collecting SMA values

                var SMA_5 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "20";
                var SMA_20 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "50";
                var SMA_50 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "200";
                var SMA_200 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);

                //Change function to EMA
                function.ParamValue = AlphaVantageApiWrapper.AvFuncationEnum.Sma.ToDescription();

                timePeriod.ParamValue = "5";
                var EMA_5 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "20";
                var EMA_20 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "50";
                var EMA_50 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "200";
                var EMA_200 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);

                //Change function to RSI
                function.ParamValue = AlphaVantageApiWrapper.AvFuncationEnum.Rsi.ToDescription();

                timePeriod.ParamValue = "7";
                var RSI_7 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "14";
                var RSI_14 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "24";
                var RSI_24 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
                timePeriod.ParamValue = "50";
                var RSI_50 = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);

                //Change function to MACD
                function.ParamValue = AlphaVantageApiWrapper.AvFuncationEnum.Macd.ToDescription();
                //Remove time period to use default values (slow 12, fast 26)
                parameters.Remove(timePeriod);
                var MACD = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);

                //Change function to STOCK
                function.ParamValue = AlphaVantageApiWrapper.AvFuncationEnum.Stoch.ToDescription();
                var STOCH = await AlphaVantageApiWrapper.GetTechnical(parameters, API_KEY);
            }
        }
    }
}
