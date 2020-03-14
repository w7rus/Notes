using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Common;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Shares
{
    public interface ISharesRepository
    {
        public Task<Share> GetShare(int noteId, int userId);
        public ICollection<Share> GetShares(int noteId);
        public Task AddShare(Share share);
        public Task UpdateShare(Share share);
        public Task DeleteShare(Share share);
        public Task DeleteShares(ICollection<Share> shares);
    }
}
