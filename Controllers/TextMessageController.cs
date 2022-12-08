using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly ICount _symbolCounter;
        private readonly ICalculate _numberAdder;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, ICount SymbolCounter, ICalculate NumberAdder)
        {
            _telegramClient = telegramBotClient;
            _numberAdder = NumberAdder;
            _symbolCounter = SymbolCounter;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(" Количество символов" , "Quantity"),
                        InlineKeyboardButton.WithCallbackData(" Сумма чисел" , "Sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                        $"<b>  Наш бот умеет считать количество букв в тексте.</b> " +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine} Еще он умеет складывать введенные числа. Если вам нечем больше заняться, welcome." +
                        $"{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html,
                                                  replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:                    
                    switch (_memoryStorage.GetSession(message.Chat.Id).ButtonType)
                    {
                        case "Quantity":
                        {
                           int quantity = _symbolCounter.Count(message.Text);
                           await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                                     $"<b>В вашем сообщении {quantity} символов.{Environment.NewLine}</b>",
                                     cancellationToken: ct, parseMode: ParseMode.Html);
                                break;
                        }
                        case "Sum":
                        {
                          int sum = _numberAdder.Calculate(message.Text);
                          await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                                     $"<b>Сумма чисел: {sum}.{Environment.NewLine}</b>",
                                     cancellationToken: ct, parseMode: ParseMode.Html);
                                break;
                        }
                        default:
                        {
                          throw new InvalidOperationException();
                        }                         
                    };
                    break;                    
            }
        }
    }
}