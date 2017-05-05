using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using OpmInspection.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpmInspection.Shared.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    [Table("Users")]
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>, ITrackable
    {
        public ApplicationUser()
        {
            this.OfficerStatistics = new List<OfficerStatistic>();
        }

        [DefaultValue(1)]
        [ForeignKey("Skin")]
        public int SkinId { get; set; }

        [DefaultValue(false)]
        public bool PasswordChanged { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }

        public virtual Skin Skin { get; set; }

        public virtual ICollection<OfficerStatistic> OfficerStatistics { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here

            return userIdentity;
        }
    }

    [Table("Roles")]
    public class ApplicationRole : IdentityRole<int, UserRole>, ITrackable
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedAt { get; set; }
    }

    [Table("UserLogin")]
    public class UserLogin : IdentityUserLogin<int>
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public override int UserId
        {
            get { return base.UserId; }
            set { base.UserId = value; }
        }

        [Key]
        [Column(Order = 1)]
        public override string LoginProvider
        {
            get { return base.LoginProvider; }
            set { base.LoginProvider = value; }
        }

        [Key]
        [Column(Order = 2)]
        public override string ProviderKey
        {
            get { return base.ProviderKey; }
            set { base.ProviderKey = value; }
        }

        public virtual ApplicationUser User { get; set; }
    }

    [Table("UserRole")]
    public class UserRole : IdentityUserRole<int>
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public override int UserId
        {
            get { return base.UserId; }
            set { base.UserId = value; }
        }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Role")]
        public override int RoleId
        {
            get { return base.RoleId; }
            set { base.RoleId = value; }
        }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }

    [Table("UserClaim")]
    public class UserClaim : IdentityUserClaim<int>
    {
        [ForeignKey("User")]
        public override int UserId
        {
            get { return base.UserId; }
            set { base.UserId = value; }
        }

        public virtual ApplicationUser User { get; set; }
    }
}