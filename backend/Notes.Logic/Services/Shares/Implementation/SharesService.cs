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
            if (sharedUsersData != null)
            {
                List<SharingProps> sharesToAdd = new List<SharingProps>();

                foreach (var sharingData in sharedUsersData)
                {
                    sharesToAdd.Add(new SharingProps()
                    {
                        Level = sharingData.Level,
                        NoteId = noteId,
                        UserId = sharingData.UserId
                    });
                }

                _sharesRepository.AddShares(sharesToAdd);
            }
        }

        public void UpdateShares(int noteId, IEnumerable<SharingData> sharedUsersData)
        {
            if (sharedUsersData != null)
            {
                List<SharingProps> shares = new List<SharingProps>();
                shares.AddRange(_sharesRepository.GetShares(noteId));

                List<SharingProps> sharesToAdd = new List<SharingProps>();
                List<SharingProps> sharesToUpdate = new List<SharingProps>();

                foreach (var sharingData in sharedUsersData)
                {
                    var share = _sharesRepository.GetShare(noteId, sharingData.UserId);

                    if (share == null)
                    {
                        sharesToAdd.Add(new SharingProps()
                        {
                            Level = sharingData.Level,
                            NoteId = noteId,
                            UserId = sharingData.UserId
                        });
                    }
                    else
                    {
                        share.Level = sharingData.Level;
                        sharesToUpdate.Add(share);
                        shares.Remove(share);
                    }
                }

                _sharesRepository.AddShares(sharesToAdd);
                _sharesRepository.UpdateShares(sharesToUpdate);
                _sharesRepository.DeleteShares(shares);
            }
        }

        public void DeleteShares(int noteId)
        {
            var shares = _sharesRepository.GetShares(noteId);

            if (shares == null)
                return;

            _sharesRepository.DeleteShares(shares);
        }
    }
}
