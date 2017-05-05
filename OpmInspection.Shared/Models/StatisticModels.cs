using OpmInspection.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpmInspection.Shared.Models
{
    /// <summary>
    /// เก็บสถิติการใช้งานของบุคคลทั่วไปที่เข้าชมเว็บไซต์
    /// </summary>
    [Table("OfficerStatistics")]
    public class OfficerStatistic : ITrackable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string IpAddress { get; set; }

        public string Browser { get; set; }

        public string Platform { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    /// <summary>
    /// เก็บสถิติการใช้งานของเจ้าหน้าที่ที่เข้าใช้งานเว็บไซต์
    /// </summary>
    [Table("VisitorStatistics")]
    public class VisitorStatistic : ITrackable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SessionID { get; set; }

        public string IpAddress { get; set; }

        public string Browser { get; set; }

        public string Platform { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }
    }
}