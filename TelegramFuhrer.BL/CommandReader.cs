using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramFuhrer.BL.Commands;
using TelegramFuhrer.BL.Services;
using TelegramFuhrer.Data.Entities;
using TelegramFuhrer.Data.Repositories;
using TLSharp.Core.Network;

namespace TelegramFuhrer.BL
{
    public class CommandReader
    {
        private readonly List<int> _activeUsers;

        private readonly UnityContainer _container;

        public CommandReader(UnityContainer container)
        {
            _activeUsers = new List<int>();
            _container = container;
        }

        public async Task Execute()
        {
            var messageService = _container.Resolve<IMessagesService>();
            while (true)
            {
                try
                {
                    var admins = await _container.Resolve<UserRepository>().GetAdminsAsync();
                    var dialogs = await _container.Resolve<IMessagesService>().AutoKickAsync();
                    foreach (var user in await messageService.GetDialogsAsync(dialogs))
                    {
                        if (admins.All(u => u.Id != user.Id))
                        {
                            await messageService.MarkUserMessagesAsReadAsync(user);
                            continue;
                        }
                        if (_activeUsers.Contains(user.Id)) continue;
                        _activeUsers.Add(user.Id);
                        await ProcessMessage(user);
                    }
                
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                catch (FloodException floodException)
                {
                    Thread.Sleep(floodException.TimeToWait);
                }
            }
        }

        private async Task ProcessMessage(User user)
        {
            var messageService = _container.Resolve<IMessagesService>();
            var commandLine = await messageService.GetUserMessagesAsync(user);
            while (!string.IsNullOrEmpty(commandLine))
            {
                if (string.IsNullOrEmpty(commandLine)) break;
                var commandLineArray = commandLine.Split(new [] {' '}, 2);

                var command = commandLineArray[0].TrimStart('/', '\\');
                ICommand cmd;
                try
                {
                    cmd = _container.Resolve<ICommand>(command.ToLower());
                }
                catch
                {
                    await messageService.SendMessageAsync(user, "Incorrect command");
                    break;
                }

                try
                {
                    var cmdResult = await cmd.Execute(commandLineArray.Length == 2 ? commandLineArray[1] : string.Empty);
                    await messageService.SendMessageAsync(user, cmdResult.Message);
                    while (!cmdResult.Success)
                    {
                        var waitMessageTask = messageService.WaitForMessageAsync(user);
                        waitMessageTask.Wait(TimeSpan.FromMinutes(1));
                        if (!waitMessageTask.IsCompleted) break;
                        cmdResult = await cmdResult.NextAction(waitMessageTask.Result);
                        await messageService.SendMessageAsync(user, cmdResult.Message);
                    }

                    break;
                }
                catch (Exception ex)
                {
                    await messageService.SendMessageAsync(user, ex.Message);
                    break;
                }
            }

            _activeUsers.Remove(user.Id);
        }
    }
}
