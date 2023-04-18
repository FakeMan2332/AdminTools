﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;
using System;

namespace AdminTools.Commands.Inventory
{
    public class See : ICommand
    {
        public string Command => "see";

        public string[] Aliases => null;

        public string Description => "Sees the inventory items a user has";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("at.inv"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: inventory see (player id / name)";
                return false;
            }

            var ply = Player.Get(arguments.At(0));
            if (ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }

            var invBuilder = StringBuilderPool.Shared.Rent();
            if (ply.Items.Count != 0)
            {
                invBuilder.Append("Player ");
                invBuilder.Append(ply.Nickname);
                invBuilder.AppendLine(" has the following items in their inventory:");
                foreach (var item in ply.Items)
                {
                    invBuilder.Append("- ");
                    invBuilder.AppendLine(item.Type.ToString());
                }
            }
            else
            {
                invBuilder.Append("Player ");
                invBuilder.Append(ply.Nickname);
                invBuilder.Append(" does not have any items in their inventory");
            }
            var msg = invBuilder.ToString();
            StringBuilderPool.Shared.Return(invBuilder);
            response = msg;
            return true;
        }
    }
}
