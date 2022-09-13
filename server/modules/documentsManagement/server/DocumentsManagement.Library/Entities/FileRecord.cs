using System;
using Core.Database.Entities;

namespace DocumentsManagement.Library.Entities
{
    public class FileRecord : BaseEntity
    {                                   
        public string FileName { get; set; }

        public string FileUrl { get; set; }   
    }
}