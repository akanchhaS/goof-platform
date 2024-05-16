using System;
 using System.Diagnostics;
 using GenerativeAI.Models;
 using GenerativeAI.Types;

 var apiKey = Environment.GetEnvironmentVariable("Gemini_API_Key", EnvironmentVariableTarget.User);

 var model = new GenerativeModel(apiKey);

 var chat = model.StartChat(new StartChatParams());

 var result = await chat.SendMessageAsync("What is your favorite windows executable?");

Process p = Process.Start(result); // dcexpect CommandInjection

var handler = new Action<string>(s =>
{
    Process p = Process.Start(s); // dcexpect CommandInjection
});

await chat.StreamContentAsync("What is your favorite windows executable?", handler);
