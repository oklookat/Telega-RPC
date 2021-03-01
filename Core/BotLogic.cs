using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Enums;
using System;

namespace Telega_RPC.Core
{
    static class BotLogic
    {
        static BotFuncs bot_funcs = new BotFuncs();
        static SystemDialog system_dialog = new SystemDialog();

        static string bot_token;
        static TelegramBotClient bot;
        static long chat_id;
        static string chat_username;

        static public void bot_init(string bot_token_settings)
        {
            bot_token = bot_token_settings;
            bot = new TelegramBotClient(bot_token);
            var me = bot.GetMeAsync().Result;
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        static public void bot_deactivate()
        {
            bot.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                chat_id = e.Message.Chat.Id;
                chat_username = e.Message.Chat.Username;
                bool security = bot_funcs.securityChecker(chat_id, chat_username);
                if (security == true)
                {
                    if (e.Message.Text.Equals("/screen"))
                    {
                        var screen_path = bot_funcs.screenShot(chat_id);
                        using (var stream = System.IO.File.OpenRead(screen_path))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
                            await bot.SendChatActionAsync(chat_id, ChatAction.UploadPhoto);
                            await bot.SendPhotoAsync(chat_id, inputOnlineFile);
                        }
                    }
                    if (e.Message.Text.Equals("/media_pp"))
                    {
                        bot_funcs.mediaPlayPause();
                        await bot.SendTextMessageAsync(chat_id, "Media: play/pause");
                    }
                    if (e.Message.Text.Equals("/media_next"))
                    {
                        bot_funcs.mediaNext();
                        await bot.SendTextMessageAsync(chat_id, "Media: next");
                    }
                    if (e.Message.Text.Equals("/media_prev"))
                    {
                        bot_funcs.mediaPrev();
                        await bot.SendTextMessageAsync(chat_id, "Media: previous");
                    }
                    if (e.Message.Text.Equals("/system_time"))
                    {
                        var date_time = bot_funcs.systemTime();
                        await bot.SendTextMessageAsync(chat_id, "Time on remote device: " + date_time);
                    }


                    if (e.Message.Text.Equals("/system_dialog"))
                    {
                        await bot.SendTextMessageAsync(chat_id, "System Dialog: write message to show it on remote device");
                        await bot.SendTextMessageAsync(chat_id, "System Dialog: for cancel write /system_dialog_cancel");
                        system_dialog.activate();
                    }
                    if (e.Message.Text.Equals("/system_dialog_cancel"))
                    {
                        system_dialog.deactivate();
                        await bot.SendTextMessageAsync(chat_id, "System Dialog: your cancelled the dialog");
                    }
                    if (!String.IsNullOrEmpty(e.Message.Text) && !e.Message.Text.Equals("/system_dialog") && !e.Message.Text.Equals("/system_dialog_cancel") && SystemDialog.dialog_active == true)
                    {
                        SystemDialog.dialog_text = e.Message.Text;
                        system_dialog.showDialog();
                    }


                }
            }
        }

        static public async void systemDialogReplyMessage(string message)
        {
            await bot.SendTextMessageAsync(chat_id, "System Dialog (reply):" + "\n" + message);
            system_dialog.deactivate();
        }
        static public async void systemDialogClosedWindowMessage()
        {
            await bot.SendTextMessageAsync(chat_id, "System Dialog: dialog closed.");
            system_dialog.deactivate();
        }

    }
}
