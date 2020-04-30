﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;
using Notes.Logic.Repositories.Attachment;

namespace Notes.Logic.Services.Attachments.Implementation
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<Attachment> AddAttachment(int noteId, byte[] fileData, string fileExtension)
        {
            var fileName = Guid.NewGuid() + fileExtension;
            var fullPath = Path.Combine(Path.Combine("Resources", "Images"), fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await stream.WriteAsync(fileData);

            var attachment = new Attachment
            {
                NoteId = noteId,
                Filename = fileName
            };

            await _attachmentRepository.AddAttachment(attachment);

            return attachment;
        }

        public async Task<Attachment> GetAttachment(int attachmentId)
        {
            return await _attachmentRepository.GetAttachment(attachmentId);
        }

        public async Task<byte[]> GetAttachmentFile(string fileName)
        {
            var fullPath = Path.Combine(Path.Combine("Resources", "Images"), fileName);
            await using var stream = new FileStream(fullPath, FileMode.Open);

            var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            var data = memory.ToArray();

            return data;
        }
    }
}