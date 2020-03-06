using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Shares
{
    public interface ISharesRepository
    {
        public IEnumerable<SharingProps> GetShares(int noteId);
        public Task<SharingProps> GetShare(int noteId, int userId);
        public Task<IEnumerable<SharingProps>> AddShares(IEnumerable<SharingProps> props);
        public Task UpdateShares(IEnumerable<SharingProps> props);
        public Task DeleteShares(IEnumerable<SharingProps> props);
    }
}
