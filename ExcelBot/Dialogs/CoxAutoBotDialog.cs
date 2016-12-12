/* 
 * Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using CoxAutoBot.Helpers;

namespace CoxAutoBot.Dialogs
{
    [LuisModel("d5a2e3fb-9ddd-4bb1-b166-02a2b34ed08c", "4558f71cfac848d281c2987d702ff980")]
    [Serializable]
    public partial class CoxAutoBotDialog : LuisDialog<object>
    {
        #region Constructor
        public CoxAutoBotDialog()
        {
        }
        #endregion

        #region Intents
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Bot", "None");

            // Respond
            await context.PostAsync(@"Sorry, I don't understand what you want to do. Type ""help"" to see a list of things I can do.");
            context.Wait(MessageReceived);
        }


        [LuisIntent("sayHello")]
        public async Task SayHello(IDialogContext context, LuisResult result)
        {
            try
            {
                //// Telemetry
                //TelemetryHelper.TrackDialog(context, result, "Bot", "SayHello");

                //// Did the bot already greet the user?
                //bool saidHello = false;
                //context.PrivateConversationData.TryGetValue<bool>("SaidHello", out saidHello);

                //// Get the user data
                //var user = await ServicesHelper.UserService.GetUserAsync();
                //await ServicesHelper.LogUserServiceResponse(context);

                //// Respond
                //if (saidHello)
                //{
                //    await context.PostAsync($"Hi again, {user.GivenName}!");
                //}
                //else
                //{
                //    await context.PostAsync($"Hi, {user.GivenName}!");
                //}

                await context.PostAsync($"Hi!");

                // Record that the bot said hello
                context.PrivateConversationData.SetValue<bool>("SaidHello", true);
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Sorry, something went wrong trying to get information about you ({ex.Message})");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("showHelp")]
        public async Task ShowHelp(IDialogContext context, LuisResult result)
        {
            // Telemetry
            TelemetryHelper.TrackDialog(context, result, "Bot", "ShowHelp");

            // Respond
            await context.PostAsync($@"Here is a list of things I can do for you:
                * You can ask me about total car sales. For example type ""How many cars were sold in total?""
                * If you are interested in top selling model type ""What was the top selling model?""
                * Or to find out about least selling model type ""Which vehicles sold the least amount?""
                * Also, I can tell you about the dealer who sold the most units and the region where most vehicles where sold, type ""Which dealer sold the most units?"", or ""Which Region sold the most vehicles?""
                * Enquire about sales of particular makes by typing ""Which region sold the most Photons"", or ""Which dealer sold the most Basalt Sport?""
                * Get more information about sales depending on the stock age, for instance type ""How many vehicles were sold?"" and then select one of the options
                * Understand sales of a particular model. For example, type ""Which Pluton variant was the most popular""");

            await context.PostAsync($@"Remember I'm just a bot. There are many things I still need to learn, so please tell me what you want me to get better at.");

            context.Wait(MessageReceived);
        }

        #endregion
    }
}