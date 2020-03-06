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

        public SharingProps AddShare(SharingProps props)
        {
            _context.Sharing.Add(props);
            _context.SaveChanges();

            return props;
        }

        public void UpdateShare(SharingProps props)
        {
            _context.Sharing.Update(props);
            _context.SaveChanges();
        }

        public void DeleteShare(SharingProps props)
        {
            _context.Sharing.Remove(props);
            _context.SaveChanges();
        }
    }
}
