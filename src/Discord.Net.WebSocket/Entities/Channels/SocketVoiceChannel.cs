﻿using Discord.API.Rest;
using Discord.Audio;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.Channel;

namespace Discord.WebSocket
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class SocketVoiceChannel : SocketGuildChannel, IVoiceChannel, ISocketAudioChannel
    {
        public int Bitrate { get; private set; }
        public int UserLimit { get; private set; }

        public override IReadOnlyCollection<SocketGuildUser> Users
            => Guild.Users.Where(x => x.VoiceChannel?.Id == Id).ToImmutableArray();

        internal SocketVoiceChannel(DiscordSocketClient discord, ulong id, SocketGuild guild)
            : base(discord, id, guild)
        {
        }
        internal new static SocketVoiceChannel Create(SocketGuild guild, ClientState state, Model model)
        {
            var entity = new SocketVoiceChannel(guild.Discord, model.Id, guild);
            entity.Update(state, model);
            return entity;
        }
        internal override void Update(ClientState state, Model model)
        {
            base.Update(state, model);

            Bitrate = model.Bitrate.Value;
            UserLimit = model.UserLimit.Value;
        }

        public Task ModifyAsync(Action<ModifyVoiceChannelParams> func)
            => ChannelHelper.ModifyAsync(this, Discord, func);

        public override SocketGuildUser GetUser(ulong id)
        {
            var user = Guild.GetUser(id);
            if (user?.VoiceChannel?.Id == Id)
                return user;
            return null;
        }
        
        private string DebuggerDisplay => $"{Name} ({Id}, Voice)";
        internal new SocketVoiceChannel Clone() => MemberwiseClone() as SocketVoiceChannel;

        //IVoiceChannel
        Task<IAudioClient> IVoiceChannel.ConnectAsync() { throw new NotSupportedException(); }

        //IGuildChannel
        Task<IGuildUser> IGuildChannel.GetUserAsync(ulong id, CacheMode mode)
            => Task.FromResult<IGuildUser>(GetUser(id));
        IAsyncEnumerable<IReadOnlyCollection<IGuildUser>> IGuildChannel.GetUsersAsync(CacheMode mode)
            => ImmutableArray.Create<IReadOnlyCollection<IGuildUser>>(Users).ToAsyncEnumerable();
    }
}
