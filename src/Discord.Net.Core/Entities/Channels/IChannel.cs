﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public interface IChannel : ISnowflakeEntity
    {
        /// <summary> Gets a collection of all users in this channel. </summary>
        IAsyncEnumerable<IReadOnlyCollection<IUser>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload);
        
        /// <summary> Gets a user in this channel with the provided id.</summary>
        Task<IUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload);
    }
}
