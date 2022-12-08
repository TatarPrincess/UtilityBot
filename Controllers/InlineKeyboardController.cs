using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;
using Telegram.Bot.Types.Enums;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).ButtonType = callbackQuery.Data;

            // Перенаправляем на сервис, соответствующий нажатой кнопке
            string buttonText = callbackQuery.Data switch
            {
                "Quantity" => " Количество символов",
                "Sum" => " Сумма чисел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбрано - {buttonText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Введите текст", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
