/* 
 * Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Bot.Builder.Dialogs;

using ExcelBot.Helpers;
using Microsoft.Bot.Connector;

namespace ExcelBot.Workers
{
    public static class CoxAutoReportWorker
    {
        public static async Task GetTotalNumberCarsSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            int? totalSold = null;
            reportData.GetTotalNumberCarsSold(ref totalSold);

            var response = $"The total number of cars sold is {totalSold}";

            await ReplyWithValue(context, response);
            
        }

        public static async Task GetTopSellingModel(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string model = null;
            reportData.GetTopSellingModel(ref model);
            
            var response = $"The top selling model is {model}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetTopDealerCarsSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string dealer = null;

            reportData.GetTopDealerCarsSold(ref dealer);
            var response = $"The dealer with the most sold units is {dealer}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetTopDealerSpecificModelSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string dealer = null;

            var carModel = context.UserData.Get<string>("CarModel");
            reportData.GetTopDealerSpecificModelSold(carModel, ref dealer);
            
            var response = $"The dealer sold the most {carModel} is {dealer}";

            await ReplyWithValue(context, response);
        }
        
        public static async Task GetTopModelForMakeSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string model = null;

            var carMake = context.UserData.Get<string>("CarMake");
            reportData.GetTopModelForMakeSold(carMake, ref model);
            
            var response = $"The most popular model for {carMake} variant is {model}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetRegionTotalCarsSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string region = null;
            
            reportData.GetRegionTotalCarsSold(ref region);

            var response = $"The region that sold most cars is {region}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetRegionSpecificModelSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string region = null;

            var carMake = context.UserData.Get<string>("CarMake");
            reportData.GetRegionSpecificModelSold(carMake, ref region);

            var response = $"The region that sold the most {carMake} is {region}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetLeastSellingModel(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string model = null;

            reportData.GetLeastSellingModel(ref model);

            var response = $"The least selling model is {model}";

            await ReplyWithValue(context, response);
        }
        
        public static async Task GetSoldCarsStockData(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string stockPeriod = context.UserData.Get<string>("StockPeriod"); 
            int? sold = null;
            reportData.GetSoldCarsStockData(stockPeriod, ref sold);

            var response = $"The number of sold vehicles is {sold}";

            await ReplyWithValue(context, response);
        }

        #region Helpers

        public static async Task ReplyWithValue(IDialogContext context, string response)
        {
            try
            {
                await context.PostAsync(response);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Sorry, something went wrong ({ex.Message})");
            }
        }
        #endregion
        
    }
}