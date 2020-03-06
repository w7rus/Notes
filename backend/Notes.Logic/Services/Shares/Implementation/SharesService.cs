using System;
using System.Collections.Generic;
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

        public IEnumerable<SharingProps> GetShares(int noteId)
        {
            return _sharesRepository.GetShares(noteId);
        }

        public async Task AddShares(int noteId, IEnumerable<SharingData> sharedUsersData)
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

                await _sharesRepository.AddShares(sharesToAdd);
            }
        }

        public async Task UpdateShares(int noteId, IEnumerable<SharingData> sharedUsersData)
        {
            if (sharedUsersData != null)
            {
                List<SharingProps> shares = new List<SharingProps>();
                shares.AddRange(_sharesRepository.GetShares(noteId));

                List<SharingProps> sharesToAdd = new List<SharingProps>();
                List<SharingProps> sharesToUpdate = new List<SharingProps>();

                foreach (var sharingData in sharedUsersData)
                {
                    var share = await _sharesRepository.GetShare(noteId, sharingData.UserId);

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

                await _sharesRepository.AddShares(sharesToAdd);
                await _sharesRepository.UpdateShares(sharesToUpdate);
                await _sharesRepository.DeleteShares(shares);
            }
        }

        public async Task DeleteShares(int noteId)
        {
            var shares = _sharesRepository.GetShares(noteId);

            if (shares == null)
                return;

            await _sharesRepository.DeleteShares(shares);
        }
    }
}
