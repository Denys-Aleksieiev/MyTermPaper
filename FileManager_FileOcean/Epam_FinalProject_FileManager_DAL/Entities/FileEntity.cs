﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Epam_FinalProject_FileManager_DAL
{
    public class FileEntity
    {
        public Guid Id { get; set; }
        [MaxLength(512)]
        public string FileName { get; set; }
        [MaxLength(10)]
        public string FileExtention { get; set; }
        [MaxLength(512)]
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        [MaxLength(128)]
        public string Hash { get; set; }
        public long Size { get; set; }
        public Guid? ShareLink { get; set; }
        public bool IsPublic { get; set; }
        public bool IsImage { get; set; }
        public bool IsAudio { get; set; }
        public bool IsVideo { get; set; }
        public bool IsDocument { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public string AnonimOwner { get; set; }
        public string Compression { get; set; }
    }
}
