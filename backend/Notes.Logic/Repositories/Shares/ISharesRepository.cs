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
        public IEnumerable<SharingProps> AddShares(IEnumerable<SharingProps> props);
        public void UpdateShares(IEnumerable<SharingProps> props);
        public void DeleteShares(IEnumerable<SharingProps> props);
    }
}
