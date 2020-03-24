using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Common;
using Notes.Logic.Models.Database;
using Notes.Logic.Repositories.Shares;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Shares.Implementation
{
    public class SharesService : ISharesService
    {
        private readonly ISharesRepository _sharesRepository;

        public SharesService(ISharesRepository sharesRepository)
        {
            _sharesRepository = sharesRepository;
        }

        public ICollection<Share> GetShares(int noteId)
        {
            return _sharesRepository.GetShares(noteId);
        }

        public ICollection<Share> GetShares(int noteId, int display, int page)
        {
            IEnumerable<Share> shares = _sharesRepository.GetShares(noteId).ToList();

            shares = shares.Where(s => s.UserId != 1);

            //Pagination

            shares = shares.Skip(display * (page)).Take(display);

            return shares.ToArray();
        }

        public async Task DeleteShares(int noteId)
        {
            var shares = _sharesRepository.GetShares(noteId);

            if (shares == null)
                return;

            await _sharesRepository.DeleteShares(shares);
        }

        public async Task AddShare(int noteId, int userId, SharingLevels.Level level)
        {
            await _sharesRepository.AddShare(new Share
            {
                NoteId = noteId,
                UserId = userId,
                Level = level,
            });
        }

        public async Task UpdateShare(int noteId, int userId, SharingLevels.Level level)
        {
            var share = await _sharesRepository.GetShare(noteId, userId);
            share.Level = level;
            await _sharesRepository.UpdateShare(share);
        }

        public async Task DeleteShare(int noteId, int userId)
        {
            var share = await _sharesRepository.GetShare(noteId, userId);
            await _sharesRepository.DeleteShare(share);
        }
    }
}
