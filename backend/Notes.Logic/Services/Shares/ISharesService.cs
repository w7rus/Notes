using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Common;
using Notes.Logic.Models.Database;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Shares
{
    public interface ISharesService
    {
        public ICollection<Share> GetShares(int noteId);
        public ICollection<Share> GetShares(int noteId, int display, int page);
        public Task AddShare(int noteId, int userId, SharingLevels.Level level);
        public Task UpdateShare(int noteId, int userId, SharingLevels.Level level);
        public Task DeleteShare(int noteId, int userId);
        public Task DeleteShares(int noteId);
    }
}
