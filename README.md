# VkAudioToTelegram
A Telegram bot that allows to search and listen to vk audio tracks

# Additional info
You can see explatationary note here: тут будет ссылка на файл, не переписывать же  в конце концов

# What shold be done to run app on your PC/server
1. First of all, make sure that you have MongoDB installed on your machine. Our application uses it to manage data about users and tracks.
2. Also, it stores all necessary api keys there. So, to make the app work you should create your own bot on Telegram, get its API key, get API key for Yandex SpeechKit Cloud API. Then create bot_users database on your MongoDB server and  execute following commands:
1) db.ak.insert({ akID: "tg", value: "YOUR_TELEGRAM_API_KEY"})
2) db.ak.insert({ akID: "ya", value: "YOUR_YANDEX_API_KEY"})
3. Run BotServerUI or install BotService (https://msdn.microsoft.com/ru-ru/library/sd8zc8ha(v=vs.110).aspx)
4. Push Start bot button or run service.
5. Enjoy.


P.S. You also can ask one of developers (https://vk.com/mehnya) to share db json files with all necessary info and then simply import them into your mongoDB via mongoimport (https://docs.mongodb.com/v3.2/reference/program/mongoimport/).
