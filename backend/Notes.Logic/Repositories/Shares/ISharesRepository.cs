using System;
using System.Collections.Generic;
using System.Text;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Shares
{
    public interface ISharesRepository
    {
        public IEnumerable<SharingProps> GetShares(int noteId);
        public SharingProps GetShare(int noteId, int userId);
        public SharingProps AddShare(SharingProps props);
        public void UpdateShare(SharingProps props);
        public void DeleteShare(SharingProps props);
    }
}
