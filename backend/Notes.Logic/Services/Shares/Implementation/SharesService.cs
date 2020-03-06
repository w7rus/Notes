using System;
using System.Collections.Generic;
using System.Text;
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

        public IEnumerable<SharingProps> GetShares(int noteId)
        {
            return _sharesRepository.GetShares(noteId);
        }

        public void AddShares(int noteId, IEnumerable<SharingData> sharedUsersData)
        {
            // Check if sharedUsersData array is not empty
            if (sharedUsersData != null)
            {
                // We are iterating since we need each one of sharingData exclusively
                foreach (var sharingData in sharedUsersData)
                {
                    // Create a new share object
                    var share = new SharingProps()
                    {
                        Level = sharingData.Level,
                        NoteId = noteId,
                        UserId = sharingData.UserId
                    };

                    // Add new share object
                    _sharesRepository.AddShare(share);
                }
            }
        }

        public void UpdateShares(int noteId, IEnumerable<SharingData> sharedUsersData)
        {
            // Check if sharedUsersData array is not empty
            if (sharedUsersData != null)
            {
                // We would need that to know which share object to delete
                List<SharingProps> shares = new List<SharingProps>();
                shares.AddRange(_sharesRepository.GetShares(noteId));

                // We are iterating since we need each one of sharingData exclusively
                foreach (var sharingData in sharedUsersData)
                {
                    // Try to get required share
                    var share = _sharesRepository.GetShare(noteId, sharingData.UserId);

                    // If the share object isn't found, add it
                    if (share == null)
                    {
                        _sharesRepository.AddShare(new SharingProps()
                        {
                            Level = sharingData.Level,
                            NoteId = noteId,
                            UserId = sharingData.UserId
                        });
                    }
                    // Update share object if found
                    else
                    {
                        share.Level = sharingData.Level;
                        _sharesRepository.UpdateShare(share);

                        shares.Remove(share);
                    }
                }

                // Now remaining objects in shares are those which are marked for deletion
                foreach (var share in shares)
                {
                    _sharesRepository.DeleteShare(share);
                }
            }
        }

        public void DeleteShares(int noteId)
        {
            // Get all shares by noteId
            var shares = _sharesRepository.GetShares(noteId);

            // If no shares were found, return
            if (shares == null)
                return;

            // If found, delete each one of shares exclusively
            foreach (var share in shares)
            {
                _sharesRepository.DeleteShare(share);
            }
        }
    }
}
