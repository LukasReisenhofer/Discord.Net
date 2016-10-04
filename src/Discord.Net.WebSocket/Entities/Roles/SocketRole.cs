﻿using Discord.API.Rest;
using Discord.Rest;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Model = Discord.API.Role;

namespace Discord.WebSocket
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class SocketRole : SocketEntity<ulong>, IRole
    {
        public SocketGuild Guild { get; }

        public Color Color { get; private set; }
        public bool IsHoisted { get; private set; }
        public bool IsManaged { get; private set; }
        public string Name { get; private set; }
        public GuildPermissions Permissions { get; private set; }
        public int Position { get; private set; }

        public bool IsEveryone => Id == Guild.Id;
        public string Mention => MentionUtils.MentionRole(Id);

        internal SocketRole(SocketGuild guild, ulong id)
            : base(guild.Discord, id)
        {
            Guild = guild;
        }
        internal static SocketRole Create(SocketGuild guild, ClientState state, Model model)
        {
            var entity = new SocketRole(guild, model.Id);
            entity.Update(state, model);
            return entity;
        }
        internal void Update(ClientState state, Model model)
        {
            Name = model.Name;
            IsHoisted = model.Hoist;
            IsManaged = model.Managed;
            Position = model.Position;
            Color = new Color(model.Color);
            Permissions = new GuildPermissions(model.Permissions);
        }

        public Task ModifyAsync(Action<ModifyGuildRoleParams> func)
            => RoleHelper.ModifyAsync(this, Discord, func);
        public Task DeleteAsync()
            => RoleHelper.DeleteAsync(this, Discord);

        public override string ToString() => Name;
        private string DebuggerDisplay => $"{Name} ({Id})";
        internal SocketRole Clone() => MemberwiseClone() as SocketRole;

        //IRole
        IGuild IRole.Guild => Guild;
    }
}
