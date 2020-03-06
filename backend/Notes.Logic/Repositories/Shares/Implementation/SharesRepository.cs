using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return _context.Sharing.Where(s => s.NoteId == noteId);
        }

        public SharingProps GetShare(int noteId, int userId)
        {
            return _context.Sharing.FirstOrDefault(n => n.NoteId == noteId && n.UserId == userId);
        }

        public IEnumerable<SharingProps> AddShares(IEnumerable<SharingProps> props)
        {
            _context.Sharing.AddRange(props);
            _context.SaveChanges();

            return props;
        }

        public void UpdateShares(IEnumerable<SharingProps> props)
        {
            _context.Sharing.UpdateRange(props);
            _context.SaveChanges();
        }

        public void DeleteShares(IEnumerable<SharingProps> props)
        {
            _context.Sharing.RemoveRange(props);
            _context.SaveChanges();
        }
    }
}
