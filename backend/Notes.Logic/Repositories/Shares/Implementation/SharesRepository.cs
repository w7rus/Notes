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

        public IEnumerable<SharingProps> GetShares(int noteId)
        {
            return _context.Sharing
                .Include(i => i.User)
                .Where(s => s.NoteId == noteId);
        }

        public async Task<SharingProps> GetShare(int noteId, int userId)
        {
            return await _context.Sharing
                .Include(i => i.User)
                .FirstOrDefaultAsync(n => n.NoteId == noteId && n.UserId == userId);
        }

        public async Task<IEnumerable<SharingProps>> AddShares(IEnumerable<SharingProps> props)
        {
            await _context.Sharing.AddRangeAsync(props);
            await _context.SaveChangesAsync();

            return props;
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
