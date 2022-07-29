using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using HoroscopeBot.AHoroscope;
using HoroscopeBot.Aztro;
using HoroscopeBot.CHoroscope;

namespace HoroscopeBot
{
    public class Bot
    {
        TelegramBotClient botclient = new TelegramBotClient("5429233967:AAFy0RyxsUviEYbOXAIF0Gcty_nSslHZOLo");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiveroptions = new ReceiverOptions { AllowedUpdates = { } };

        public async Task Start()
        {
            botclient.StartReceiving(HandlerUpdateAsync, HandlerError, receiveroptions, cancellationToken);
            var botMe = await botclient.GetMeAsync();
            Console.WriteLine("Бот почав працювати");
            
        }
        public string HoroType;
        public string period;
        public string Sign;
        public string SignDate;

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"\nПомилка в телеграм бот АПІ :\n{apiRequestException.ErrorCode}\n" +
                $"\n{apiRequestException.Message}",_=> exception.ToString()
            };
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Type==UpdateType.Message && update.Message.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose /keyboard, to get your horoscope" +
                    "\nChoose /allsignswestern, to get all signs of western horoscope" +
                    "\nChoose /allsignschinese, to get all signs from chinese horoscope");
                
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                            new KeyboardButton[] {"/keyboard", "/allsignswestern", "/allsignschinese"}
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "What would you like to get ? ", replyMarkup: replyKeyboardMarkup);
                return;
            }
            if(message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                            new KeyboardButton[] {"Chinese horoscope", "Western horoscope", "Day description"}
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please choose a horoscope ", replyMarkup: replyKeyboardMarkup);
                return;
            }
            if (message.Text == "/allsignswestern")
            {
                string allsigns = "Aries   21 march - 20 april"

                                +"\nTaurus   21 april - 21 may"

                                +"\nGemini     22 may — 21 june"

                                +"\nCancer      22 june — 22 july"

                                +"\nLeo     23 july — 22 august"

                                +"\nVirgo   23 august — 23 september"

                                +"\nLibra     24 september — 23 october"

                                +"\nScorpio     24 october — 22 novermber"

                                +"\nSagittarius     23 november — 21 december"

                                +"\nCapricornus     22 december — 20 january"

                                +"\nAquarius     21 january — 18 february"

                                +"\nPisces      19 february — 20 march";
                await botclient.SendTextMessageAsync(message.Chat.Id, allsigns);
                return;
            }
            if (message.Text == "/allsignschinese")
            {
                string allsigns = "Rat   鼠年 (子)	2020, 2008, 1996, 1984, 1972, 1960, 1948, 1936"
                                    +"\nOx   牛年(丑)    2021, 2009, 1997, 1985, 1973, 1961, 1949, 1937"
                                    +"\nTiger   虎年(寅)   2022, 2010, 1998, 1986, 1974, 1962, 1950, 1938"
                                    +"\nRabbit     兔年(卯)  2011, 1999, 1987, 1975, 1963, 1951, 1939"
                                    +"\nDragon     龙年(辰)  2012, 2000, 1988, 1976, 1964, 1952, 1940"
                                    +"\nSnake   蛇年(巳)   2013, 2001, 1989, 1977, 1965, 1953, 1941"
                                    +"\nHorse   马年(午)   2014, 2002, 1990, 1978, 1966, 1954, 1942"
                                    +"\nGoat   羊年(未)   2015, 2003, 1991, 1979, 1967, 1955, 1943"
                                    +"\nMonley      猴年(申)   2016, 2004, 1992, 1980, 1968, 1956, 1944"
                                    +"\nRooster     鸡年(酉)  2017, 2005, 1993, 1981, 1969, 1957, 1945"
                                    +"\nDog     狗年(戌)  2018, 2006, 1994, 1982, 1970, 1958, 1946"
                                    +"\nPig      猪年(亥)  2019, 2007, 1995, 1983, 1971, 1959, 1947";
                await botclient.SendTextMessageAsync(message.Chat.Id, allsigns);
                return;
            }
            if (message.Text == "Western horoscope")
            {
                HoroType = "Western horoscope";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please write your western horoscope sign and a period of time \nExample: aries, month ") ;
                return;
            }
            if (message.Text == "Day description")
            {
                HoroType = "Day description";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please write your western horoscope sign and a day");
                return;
            }
            if(message.Text=="Chinese horoscope")
            {
                HoroType = "Chinese horoscope";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please write your chinese horoscope sign and a period of time \nExample: rat, week");
                return;
            }

            if (HoroType == "Western horoscope")
            {
                try
                {
                    string result = "";
                    AClient aclient = new AClient();
                    string[] needenparms = message.Text.Split(", ");
                    string sign = needenparms[0];
                    string period = needenparms[1];
                    Sign = sign.ToLower();
                    if (CheckPeriod(period) == true && CheckWSign(Sign) == true)
                    {
                        switch (Sign)
                        {
                            case "aries":
                                result = aclient.GetAHoro(Sign, period).Result.aries.ToString();
                                break;
                            case "taurus":
                                result = aclient.GetAHoro(Sign, period).Result.taurus.ToString();
                                break;
                            case "gemini":
                                result = aclient.GetAHoro(Sign, period).Result.gemini.ToString();
                                break;
                            case "cancer":
                                result = aclient.GetAHoro(Sign, period).Result.cancer.ToString();
                                break;
                            case "leo":
                                result = aclient.GetAHoro(Sign, period).Result.leo.ToString();
                                break;
                            case "virgo":
                                result = aclient.GetAHoro(Sign, period).Result.virgo.ToString();
                                break;
                            case "libra":
                                result = aclient.GetAHoro(Sign, period).Result.libra.ToString();
                                break;
                            case "sagittarius":
                                result = aclient.GetAHoro(Sign, period).Result.sagittarius.ToString();
                                break;
                            case "scorpio":
                                result = aclient.GetAHoro(Sign, period).Result.scorpio.ToString();
                                break;
                            case "aquarius":
                                result = aclient.GetAHoro(Sign, period).Result.aquarius.ToString();
                                break;
                            case "pisces":
                                result = aclient.GetAHoro(Sign, period).Result.pisces.ToString();
                                break;
                            case "capricorn":
                                result = aclient.GetAHoro(Sign, period).Result.capricorn.ToString();
                                break;
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, result);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Please check if you wrote everything correctly");
                    }
                    return;
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Something went wrong, please try again later");
                }
            }
            else if (HoroType == "Day description")
            {
                try
                {
                    Aztro_Client aztroclient = new Aztro_Client();
                    string[] neededparms = message.Text.Split(", ");
                    string Sign = neededparms[0];
                    period = neededparms[1];
                    
                    if ( CheckDay(period)==true && CheckWSign(Sign) == true)
                    {
                        
                            var result = "Description: " + aztroclient.GetHoro(Sign, period).Result.description;
                            result += "\n" + "Mood :" + aztroclient.GetHoro(Sign, period).Result.mood;
                            result += "\n" + "Lucky number :" + aztroclient.GetHoro(Sign, period).Result.lucky_number;
                            result += "\n" + "Lucky time :" + aztroclient.GetHoro(Sign, period).Result.lucky_time;
                            result += "\n" + "Compatability :" + aztroclient.GetHoro(Sign, period).Result.compatibility;
                            await botClient.SendTextMessageAsync(message.Chat.Id, result);
                        
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Please check if you wrote everything correctly");
                    }
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Something went wrong, please try again later");
                }
            }
            else if (HoroType == "Chinese horoscope")
            {
                try {
                    string result = "";
                    CClient cclient = new CClient();
                    string[] neededparms = message.Text.Split(", ");
                    string sign = neededparms[0];
                    period = neededparms[1];
                    Sign = sign.ToLower();
                    if (CheckPeriod(period) == true && CheckCSign(Sign) == true)
                    {
                        switch (Sign)
                        {
                            case "ox":
                                result = cclient.GetCHoro(Sign, period).Result.Ox.ToString();
                                break;
                            case "tiger":
                                result = cclient.GetCHoro(Sign, period).Result.Tiger.ToString();
                                break;
                            case "rabbit":
                                result = cclient.GetCHoro(Sign, period).Result.Rabbit.ToString();
                                break;
                            case "dragon":
                                result = cclient.GetCHoro(Sign, period).Result.Dragon.ToString();
                                break;
                            case "snake":
                                result = cclient.GetCHoro(Sign, period).Result.Snake.ToString();
                                break;
                            case "horse":
                                result = cclient.GetCHoro(Sign, period).Result.Horse.ToString();
                                break;
                            case "goat":
                                result = cclient.GetCHoro(Sign, period).Result.Goat.ToString();
                                break;
                            case "monkey":
                                result = cclient.GetCHoro(Sign, period).Result.Monkey.ToString();
                                break;
                            case "rooster":
                                result = cclient.GetCHoro(Sign, period).Result.Rooster.ToString();
                                break;
                            case "dog":
                                result = cclient.GetCHoro(Sign, period).Result.Dog.ToString();
                                break;
                            case "pig":
                                result = cclient.GetCHoro(Sign, period).Result.Pig.ToString();
                                break;
                            case "rat":
                                result = cclient.GetCHoro(Sign, period).Result.Rat.ToString();
                                break;
                        }

                        await botClient.SendTextMessageAsync(message.Chat.Id, result);
                        
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Please check if you wrote everything correctly");
                    }
                }
                catch (Exception)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "Something went wrong, please try again later");
                }
            }

        }
        public bool CheckPeriod(string p)
        {
            if(p=="month"|| p == "week" || p == "today" || p == "tomorrow" || p == "yesterday")
            {
                return true;
            }
            return false;
        }
        public bool CheckWSign(string s)
        {
            if(s=="aries"|| s == "taurus" || s == "gemini" || s == "cancer" || s == "leo" || s == "virgo" || s == "libra" || s == "sagittarius" || s == "scorpio" || s == "capricorn" || s == "aquarius" || s == "pisces")
            {
                return true;
            }
            return false;
        }
        public bool CheckCSign(string s)
        {
            if(s == "ox" || s == "tiger" || s == "rabbit" || s == "dragon" || s == "snake" || s == "horse" || s == "goat" || s == "monkey" || s == "roster" || s == "dog" || s == "pig" || s == "rat")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckDay(string s)
        {
            if (s == "today" || s == "tomorrow" || s == "yesterday")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
