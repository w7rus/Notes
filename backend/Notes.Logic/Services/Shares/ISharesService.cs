using System;
using System.Collections.Generic;
using System.Text;
using Notes.Logic.Models.Database;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Shares
{
    public interface ISharesService
    {
        public IEnumerable<SharingProps> GetShares(int noteId);
        public void AddShares(int noteId, IEnumerable<SharingData> sharedUsersData);
        public void UpdateShares(int noteId, IEnumerable<SharingData> sharedUsersData);
        public void DeleteShares(int noteId);
    }
}
