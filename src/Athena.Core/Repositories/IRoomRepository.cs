﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Athena.Core.Models;

namespace Athena.Core.Repositories
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
        /// <summary>
        /// Gets the rooms on the specified campus
        /// </summary>
        /// <param name="campus">The campus to get rooms for</param>
        /// <returns>An IEnumerable of Rooms</returns>
        Task<IEnumerable<Room>> GetRoomsOnCampusAsync(Campus campus);
    }
}