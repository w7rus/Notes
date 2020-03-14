using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Logic.Common;
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

        public async Task<Share> GetShare(int noteId, int userId)
        {
            return await _context.Shares.FirstOrDefaultAsync(s => s.NoteId == noteId && s.UserId == userId);
        }

        public ICollection<Share> GetShares(int noteId)
        {
            return _context.Shares
                .Include(i => i.User)
                .Where(s => s.NoteId == noteId).ToArray();
        }

        public async Task AddShare(Share share)
        {
            await _context.Shares.AddAsync(share);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShare(Share share)
        {
            _context.Shares.Update(share);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShare(Share share)
        {
            _context.Shares.Remove(share);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShares(ICollection<Share> shares)
        {
            _context.Shares.RemoveRange(shares);
            await _context.SaveChangesAsync();
        }
    }
}
