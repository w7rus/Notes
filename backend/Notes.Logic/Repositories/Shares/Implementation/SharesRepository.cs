using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Logic.Data;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Shares.Implementation
{
    public class SharesRepository : ISharesRepository
    {
        private readonly NotesWebAPIContext _context;

        public SharesRepository(NotesWebAPIContext context)
        {
            _context = context;
        }

        public ICollection<SharingProps> GetShares(int noteId)
        {
            return _context.Sharing
                .Include(i => i.User)
                .Where(s => s.NoteId == noteId).ToArray();
        }

        public async Task<SharingProps> GetShare(int noteId, int userId)
        {
            return await _context.Sharing
                .Include(i => i.User)
                .FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);
        }

        public async Task<ICollection<SharingProps>> AddShares(IEnumerable<SharingProps> props)
        {
            var sharingProps = props as SharingProps[] ?? props.ToArray();
            await _context.Sharing.AddRangeAsync(sharingProps);
            await _context.SaveChangesAsync();

            return sharingProps;
        }

        public async Task UpdateShares(IEnumerable<SharingProps> props)
        {
            _context.Sharing.UpdateRange(props);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShares(IEnumerable<SharingProps> props)
        {
            _context.Sharing.RemoveRange(props);
            await _context.SaveChangesAsync();
        }
    }
}
