using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translate.Models
{
    public enum PropertyType
    {
        String,
        DateTime
    }

    public enum Status
    {
        Initialize = 0,
        Validating = 1,
        Validated = 2,
        Rejected = 3,
        UploadSuccess = 4,
        UploadFailed = 5,
        Finished = 6,
        UpdateSapSuccess = 7,
        UpdateSapFailed = 8,
        Success = 9
    }

    public abstract class AbstractCommon
    {
        [Key]
        public int Id { set; get; }
    }

    [Table("QReconciles")]
    public class QReconcile : AbstractCommon
    {
        public DateTime DocumentDate { set; get; } = DateTime.MinValue;
        public string Reference { set; get; } = "";
        public string Site { set; get; } = "";
        public string Category { set; get; } = "";
        public string TCode { set; get; } = "";
        public string ObjectType { set; get; } = "";
        public string Creator { set; get; } = "";
        public DateTime ModifiedDate { set; get; } = DateTime.Now;

        public string Reference1 { set; get; } = "";
        public string Reference2 { set; get; } = "";
    }

    [Table("QRejectReasons")]
    public class QRejectReason : AbstractCommon
    {
        public string ReasonId { set; get; }
        public string Description { set; get; }

        [ForeignKey("QFileId")]
        public QFile QFile { set; get; }
        public int QFileId { set; get; }
    }

    [Table("QTemplates")]
    public class QTemplate : AbstractCommon
    {
        // -- [Index]
        public string Template { set; get; }

        // -- [Index]
        public bool ValidationRequire { set; get; }
        public string ValidationKey { set; get; }
        public string ValidationChecklistsText { set; get; }
    }

    [Table("QProperties")]
    public class QProperty : AbstractCommon
    {
        public string Name { set; get; }
        public string Title { set; get; }
        public PropertyType PropertyType { set; get; }

        public string StringValue { set; get; }
        public float NumberValue { set; get; }
        public DateTime DateTimeValue { set; get; } = DateTime.MinValue;

        [ForeignKey("QFileId")]
        public QFile QFile { set; get; }
        public int QFileId { set; get; }
    }

    [Table("QFiles")]
    public class QFile : AbstractCommon
    {
        public string OriginalFileName { set; get; } = string.Empty;
        public string LocalFileName { set; get; } = string.Empty;
        public string Creator { set; get; } = string.Empty;
        public string Validator { set; get; } = string.Empty;
        public string Template { set; get; } = string.Empty;

        public DateTime CreateDate { set; get; } = DateTime.Now;
        public DateTime UploadDate { set; get; } = DateTime.MinValue;
        public DateTime ValidateDate { set; get; } = DateTime.MinValue;

        public string Uuid { set; get; } = string.Empty;

        public string AlfrescoUuid { set; get; } = string.Empty;

        public string NameFormat { set; get; } = string.Empty;
        public string PathFormat { set; get; } = string.Empty;
        public string TitleFormat { set; get; } = string.Empty;
        public string DescriptionFormat { set; get; } = string.Empty;

        public string FinalName { set; get; } = string.Empty;
        public string FinalPath { set; get; } = string.Empty;

        public string Comment { set; get; } = string.Empty;

        public Status Status { set; get; } = Status.Initialize;

        public int Pages { set; get; }

        // speedup
        public string Reference { set; get; }
        public string DocumentType { set; get; }
        public string Category { set; get; }
        public string Department { set; get; }
        public string Site { set; get; }
        public Migration Migration { set; get; }
    }

    public enum Migration
    {
        Default,
        Success
    }
}