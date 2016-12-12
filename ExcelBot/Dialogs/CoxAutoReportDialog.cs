/* 
 * Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

using CoxAutoBot.Helpers;
using CoxAutoBot.Workers;

namespace CoxAutoBot.Dialogs
{
    public partial class CoxAutoBotDialog : LuisDialog<object>
    {
        #region Properties
        //internal object Value { get; set; }
        #endregion

        #region Intents
        #region - Total Number Cars Sold
        [LuisIntent("TotalNumberCarsSold")]
        public async Task TotalNumberCarsSold(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "TotalNumberCarsSold");

            await CoxAutoReportWorker.GetTotalNumberCarsSold(context); 
            context.Wait(MessageReceived);
        }
        #endregion

        #region - Top Selling Model
        [LuisIntent("TopSellingModel")]
        public async Task TopSellingModel(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "TopSellingModel");

            await CoxAutoReportWorker.GetTopSellingModel(context); 
            context.Wait(MessageReceived);
        }
        #endregion

        #region - TopDealerCarsSold
        [LuisIntent("TopDealerCarsSold")]
        public async Task TopDealerCarsSold(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "TopDealerCarsSold");

            await CoxAutoReportWorker.GetTopDealerCarsSold(context); 
            context.Wait(MessageReceived);
        }
        #endregion

        #region - Top Selling region

        [LuisIntent("RegionTotalCarsSold")]
        public async Task RegionTotalCarsSold(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "RegionTotalCarsSold");

            await CoxAutoReportWorker.GetRegionTotalCarsSold(context); 
            context.Wait(MessageReceived);
        }
        #endregion

        #region - Top Selling region for a specific model
        [LuisIntent("RegionSpecificModelSold")]
        public async Task RegionSpecificModelSold(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "RegionSpecificModelSold");

            var carMake = result.Entities[0].Entity.ToLower();
            context.UserData.SetValue<string>("CarMake", carMake);

            await CoxAutoReportWorker.GetRegionSpecificModelSold(context);
            context.Wait(MessageReceived);
        }
        #endregion

        #region - Least Selling Model
        [LuisIntent("LeastSellingModel")]
        public async Task LeastSellingModel(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "LeastSellingModel");

            await CoxAutoReportWorker.GetLeastSellingModel(context);
            context.Wait(MessageReceived);
        }

        #endregion

        #region - TopDealerSpecificModelSold
        [LuisIntent("TopDealerSpecificModelSold")]
        public async Task TopDealerSpecificModelSold(IDialogContext context, LuisResult result)
        {

            //// Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "TopDealerSpecificModelSold");

            try
            {
                var carModel = result.Entities[0].Entity.ToLower();
                context.UserData.SetValue<string>("CarModel", carModel);

                await CoxAutoReportWorker.GetTopDealerSpecificModelSold(context);
                context.Wait(MessageReceived);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Sorry, something went wrong when I tried to process your message ({ex.Message})");
            }
        }
        #endregion



        #region - How Many Vehicles were sold

        private const string stockOptionLess30days = "less than 30 days";
        private const string stockOption3145days = "31-45 days";
        private const string stockOption4660days = "46-60 days";
        private const string stockOption6190days = "61-90 days";
        private const string stockOptionOver90days = "over 90 days";

        [LuisIntent("SoldCarsStockData")]
        public async Task SoldCarsStockData(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "SoldCarsStockData");

            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>()
                { stockOptionLess30days, stockOption3145days, stockOption4660days, stockOption6190days, stockOptionOver90days },
                "Which stock age are interested in?", "Not a valid option", 3);

        }
       
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;
                context.UserData.SetValue("StockPeriod", optionSelected);
                
                await CoxAutoReportWorker.GetSoldCarsStockData(context);

            }
            catch (Exception ex)
            {
                await context.PostAsync($"Sorry, something went wrong when I tried to process your message ({ex.Message})");
            }
            finally
            {
                context.Wait(MessageReceived); 
            }
        }
        

        #endregion

        #region 
        [LuisIntent("TopModelForMakeSold")]
        public async Task GetTopModelForMakeSold(IDialogContext context, LuisResult result)
        {

            //// Telemetry
            TelemetryHelper.TrackDialog(context, result, "CoxAutoReport", "TopDealerSpecificModelSold");

            try
            {
                if (result.Entities.Count > 0)
                {
                    var carMake = result.Entities[0].Entity.ToLower();
                    context.UserData.SetValue<string>("CarMake", carMake);

                    await CoxAutoReportWorker.GetTopModelForMakeSold(context);
                }
                else
                {
                    await context.PostAsync($"Sorry, I could not understand your message.");
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Sorry, something went wrong when I tried to process your message ({ex.Message})");
            }
            finally
            {
                context.Wait(MessageReceived);
            }

        }
        #endregion
        #region - Get Cell Values
        /*
        [LuisIntent("getCellValue")]
        public async Task GetCellValue(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Cells", "GetCellValue");

            var cellAddress = result.Entities[0].Entity.ToUpper();
            context.UserData.SetValue<string>("CellAddress", cellAddress);

            context.UserData.SetValue<ObjectType>("Type", ObjectType.Cell);
            context.UserData.SetValue<string>("Name", cellAddress);

            string workbookId = String.Empty;
            context.UserData.TryGetValue<string>("WorkbookId", out workbookId);
            
            if (!(String.IsNullOrEmpty(workbookId)))
            {
                await CellWorker.DoGetCellValue(context);
                context.Wait(MessageReceived);
            }
            else
            {
                context.Call<bool>(new ConfirmOpenWorkbookDialog(), AfterConfirm_GetCellValue);
            }
        }
        public async Task AfterConfirm_GetCellValue(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await CellWorker.DoGetCellValue(context);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("getActiveCellValue")]
        public async Task GetActiveCellValue(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Cells", "GetActiveCellValue");

            string workbookId = String.Empty;
            context.UserData.TryGetValue<string>("WorkbookId", out workbookId);

            if (!(String.IsNullOrEmpty(workbookId)))
            {
                await NamedItemsWorker.DoGetNamedItemValue(context);
                context.Wait(MessageReceived);
            }
            else
            {
                context.Call<bool>(new ConfirmOpenWorkbookDialog(), AfterConfirm_GetActiveCellValue);
            }
        }
        public async Task AfterConfirm_GetActiveCellValue(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await NamedItemsWorker.DoGetNamedItemValue(context);
            }
            context.Wait(MessageReceived);
        }
        */
        #endregion
        #region - Set Cell Value
        /*
        [LuisIntent("setCellNumberValue")]
        public async Task SetCellNumberValue(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Cells", "SetCellNumberValue");

            var cellAddress = LuisHelper.GetCellEntity(result.Entities);
            context.UserData.SetValue<string>("CellAddress", cellAddress);

            context.UserData.SetValue<ObjectType>("Type", ObjectType.Cell);
            context.UserData.SetValue<string>("Name", cellAddress);

            Value = LuisHelper.GetValue(result);

            string workbookId = String.Empty;
            context.UserData.TryGetValue<string>("WorkbookId", out workbookId);

            if (!(String.IsNullOrEmpty(workbookId)))
            {
                if (cellAddress != null)
                {
                    await CellWorker.DoSetCellValue(context, Value);
                }
                else
                {
                    await context.PostAsync($"You need to provide the address of a cell to set the value");
                }  
                context.Wait(MessageReceived);
            }
            else
            {
                context.Call<bool>(new ConfirmOpenWorkbookDialog(), AfterConfirm_SetCellNumberValue);
            }
        }

        public async Task AfterConfirm_SetCellNumberValue(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await CellWorker.DoSetCellValue(context, Value);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("setCellStringValue")]
        public async Task SetCellStringValue(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Cells", "SetCellStringValue");

            var cellAddress = LuisHelper.GetCellEntity(result.Entities);
            context.UserData.SetValue<string>("CellAddress", cellAddress);

            context.UserData.SetValue<ObjectType>("Type", ObjectType.Cell);
            context.UserData.SetValue<string>("Name", cellAddress);

            Value = LuisHelper.GetValue(result);

            string workbookId = String.Empty;
            context.UserData.TryGetValue<string>("WorkbookId", out workbookId);

            if (!(String.IsNullOrEmpty(workbookId)))
            {
                if (cellAddress != null)
                {
                    await CellWorker.DoSetCellValue(context, Value);
                }
                else
                {
                    await context.PostAsync($"You need to provide the name of a cell to set the value");
                }
                context.Wait(MessageReceived);
            }
            else
            {
                context.Call<bool>(new ConfirmOpenWorkbookDialog(), AfterConfirm_SetCellStringValue);
            }
        }

        public async Task AfterConfirm_SetCellStringValue(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await CellWorker.DoSetCellValue(context, Value);
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("setActiveCellValue")]
        public async Task SetActiveCellValue(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Cells", "SetActiveCellValue");

            ObjectType? type = null;
            context.UserData.TryGetValue<ObjectType?>("Type", out type);

            var name = string.Empty;
            context.UserData.TryGetValue<string>("Name", out name);

            Value = LuisHelper.GetValue(result);

            string workbookId = String.Empty;
            context.UserData.TryGetValue<string>("WorkbookId", out workbookId);

            if (!(String.IsNullOrEmpty(workbookId)))
            {
                await NamedItemsWorker.DoSetNamedItemValue(context, Value);
                context.Wait(MessageReceived);
            }
            else
            {
                context.Call<bool>(new ConfirmOpenWorkbookDialog(), AfterConfirm_SetActiveCellValue);
            }
        }

        public async Task AfterConfirm_SetActiveCellValue(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await NamedItemsWorker.DoSetNamedItemValue(context, Value);
            }
            context.Wait(MessageReceived);
        }
        */
        #endregion
        #endregion
    }
}