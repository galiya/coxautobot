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
            //int? sold;

            var topSellingModel = reportData.GetTopSellingModel(ref model);
            
            var response = $"The top selling model is {model}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetTopDealerCarsSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string dealer = null;

            var topDealerCarsSold = reportData.GetTopDealerCarsSold(ref dealer);

            var response = $"The dealer with the most sold units is {dealer}";

            await ReplyWithValue(context, response);
        }

        public static async Task GetTopDealerSpecificModelSold(IDialogContext context)
        {
            CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

            string dealer = null;

            var carModel = context.UserData.Get<string>("CarModel");
            var topDealerCarsSold = reportData.GetTopDealerSpecificModelSold(carModel, ref dealer);
            
            var response = $"The dealer sold the most {carModel} is {dealer}";

            await ReplyWithValue(context, response);
        }
        
        public static async Task GetTopModelForMakeSold(IDialogContext context)
            {
                CoxAutoBotDBDataContext reportData = new CoxAutoBotDBDataContext();

                string model = null;

                var carMake = context.UserData.Get<string>("CarMake");
                var topDealerCarsSold = reportData.GetTopModelForMakeSold(carMake, ref model);
            
                var response = $"The most popular model for {carMake} variant is {model}";

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