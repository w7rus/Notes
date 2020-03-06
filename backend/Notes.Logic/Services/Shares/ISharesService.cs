using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Shares
{
    public interface ISharesService
    {
        public IEnumerable<SharingProps> GetShares(int noteId);
        public Task AddShares(int noteId, IEnumerable<SharingData> sharedUsersData);
        public Task UpdateShares(int noteId, IEnumerable<SharingData> sharedUsersData);
        public Task DeleteShares(int noteId);
    }
}
